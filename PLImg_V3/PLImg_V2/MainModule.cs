using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDalsaCamera;
using ISigmaKokiStage;
using DALSA.SaperaLT.SapClassBasic;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Accord.Math;
using System.Threading.Tasks;
using NationalInstruments.VisaNS;
using IDctXYZStage;

namespace PLImg_V2
{
    enum GrabStatus { Frea,Grab }
    enum ScanDirection { Forward,Back }

    enum FullScanState { Start, Pause, Stop, Wait}

    public delegate void TransbyteArr(byte[] imgarr);
    public delegate void TransImgArr(Image<Gray,Byte> img);
    public delegate void TransNumber(int num);
    public delegate void TransDoubleNumber(double num);
    public delegate void TransScanStatus(); 
    public delegate void TransSplitImgArr(Image<Gray, Byte> img,int lineNum,int unitNum);
    public delegate void TransLineBuffNum(int linenum, int unitnum);

    public class MainModule
    {
        readonly int YStep = 100;

        public event TransbyteArr       evtByteArrOneLine     ;
        public event TransImgArr        evtRealimg            ;
        public event TransImgArr        evtScanImgOnGoing     ;
        public event TransSplitImgArr   evtFScanImgOnGoing    ;
        public event TransLineBuffNum   evtlineUnitNum        ;
        public event TransNumber        evtBuffNum            ;
        public event TransScanStatus    evtLineScanStart      ;
        public event TransScanStatus    evtLineScanCom        ;
        //public event TransScanStatus    evtFullScanStart      ;
        public event TransScanStatus    evtFullScanCom        ;
        public event TransDoubleNumber  evtVarianceValue      ;

        Timer                 ProfileTimer    ;
        MessageBasedSession   MbSession       ;
        AFCalc                AFMeasrue       ;
        FullScanData          DataFullScan    ;
        CheckAndCreateFolder  FolderControl   ;
        IGrabMana             GrabM           ;
        IMemberDefine         DalsaMemObj     ;
        GrabStatus            StatusGrab      ;
        FullScanState         StatusFullScan  ;
        ICameraSetting        CameraSet       ;
        IXYStageOrder         XYStageControler;
        IZStageOrder          ZStageControler ;
        ISigmakokiStageUnit   RStageControler ;
        AcsContol             AcsXYZControl   ;

        Image<Gray, byte>     CurrentImg      ;
        byte[]                ImgSrcByte      ;
        byte[][]              ImgSrcByteArray ;
        bool                  ScanStart       ;
        bool                  ScanStop        ;
        List<byte[]>          GrabedBuffPool  ;
        bool                  IsPaused        ; 
        int                   BuffCount       ;
        int                   LineCount       ;
        int                   UnitCount       ;
        int                   ScanNum         ;

        string ImgBasePath = "C:\\ImgFullScanTemp\\" ;

        public MainModule()
        {
            DalsaMemObj              = new DalsaMember()        ;
            GrabM                    = new GrabMana(ImgBasePath);
            CameraSet                = new CameraSetting()      ;
            IConnection DalsaConnect = new Connection()         ;
            AFMeasrue                = new AFCalc()             ;
            DataFullScan             = new FullScanData()       ;
            AcsXYZControl             = new AcsContol()          ;
            
            CreateDeviceMana(DalsaConnect);
            TimerSetting();
            StatusFullScan = FullScanState.Wait;
        }


        #region Event Method
        Image<Gray, byte> Buff2Img(byte[] data, int count)
        {
            Image<Gray, byte> buffImgData = new Image<Gray, byte>(DalsaMemObj.Buffers.Width, DalsaMemObj.Buffers.Height * count);
            buffImgData.Bytes = data;
            return buffImgData;
        }

        void GrabDoneEventMethod(object sender, SapXferNotifyEventArgs evt)
        {
            byte[] buffData = GrabM.DataTransFromBuffer(DalsaMemObj.Buffers);
            evtRealimg(Buff2Img(buffData, 1));

            #region New
            switch (StatusFullScan) {
                case FullScanState.Stop:
                    evtFullScanCom();
                    StatusFullScan = FullScanState.Wait;
                    break;

                case FullScanState.Pause:
                    XYMoveAbsPos(DataFullScan.PosXStart,DataFullScan.PosYStart*(LineCount+1));
                    XYWait2Arrive( DataFullScan.PosXStart, DataFullScan.PosYStart * (LineCount + 1) );
                    StatusFullScan = FullScanState.Start;
                    break;

                case FullScanState.Start:
                    ImgSrcByte = Matrix.Concatenate<byte>(ImgSrcByte, buffData);
                    SaveFullDat(ImgSrcByte,LineCount,UnitCount,BuffCount);

                    evtFScanImgOnGoing(Buff2Img(ImgSrcByte, BuffCount+1), LineCount, UnitCount);
                    //evtlineUnitNum(LineCount, UnitCount); // for Watch

                    if (BuffCount == DataFullScan.BuffLimit)
                    {
                        ImgSrcByte = null;
                        ImgSrcByte = new byte[0];
                        BuffCount = 0;

                        if (UnitCount == DataFullScan.UnitLimit)
                        {
                            UnitCount = 0;

                            if (LineCount == DataFullScan.LineLimit)
                            {
                                StatusFullScan = FullScanState.Stop;
                                break;
                            }
                            else
                            {
                                LineCount += 1;
                                StatusFullScan = FullScanState.Pause;
                            }
                            break;
                        }
                        else { UnitCount += 1; }
                    }
                    else { BuffCount++; }
                    break;
            }
            #endregion
        }

        async void TempSaveCurrentImg(byte[] data, int count)
        {
            CurrentImg = await Task.Run(()=> Buff2Img(data, count));
        }

        void ProfileEventMethod(object sender, EventArgs e)
        {
            byte[] buffOneLineDatga = GrabM.DataTransFromBufferOneLine(DalsaMemObj.Buffers);
            evtByteArrOneLine(ArrayControl.Slice(buffOneLineDatga, 0, 6143));
            evtVarianceValue(AFMeasrue.CalcAFV (ArrayControl.Slice(buffOneLineDatga, 0, 6143)) );
        }

        async void SaveLineDat(byte[] inputArr, int buffNum, int scanNum)
        {
            await Task.Run(() => GrabM.TempSaveGrabDataLineMode(inputArr, buffNum, scanNum));
        }
        async void SaveFullDat(byte[] inputArr, int lineNum, int unitNum, int buffNum)
        {
            await Task.Run(() => GrabM.TempSaveGrabDataFullMode(inputArr, lineNum, unitNum, buffNum));
        }
        #endregion

        #region Scanning
        public async void StartLineScan(int startposX, int startposY, int endposX, int speed)
        {
            DataFullScan.LineLimit = 0;
            InitCount();

            string dirTempPath = String.Format(ImgBasePath + DateTime.Now.ToString("u"));
            CheckAndCreateFolder cacf = new CheckAndCreateFolder(dirTempPath);
            GrabM.SetDirPath(dirTempPath);

            evtLineScanStart();
            await Task.Run(()=> {
                ScanInit(startposX, startposX, endposX, speed);
                ImgSrcByte = new byte[0];
                CheckGrabStatus();

                XYStageControler.XYMoveAbsPos(endposX, startposY);
                ScanStart = true;
                XYStageControler.XYWait2ArriveEpsilon(endposX, startposY, 80);
                ScanStop = true;
            });
        }

        public async void StartFullScan(int startposX, int startposY,int endposX, int xSpeed)
        {
            DataFullScan.LineLimit = 3;
            InitCount();

            string dirTempPath = String.Format(ImgBasePath + DateTime.Now.ToString("MM/dd/HH/mm/ss"));
            CheckAndCreateFolder cacf = new CheckAndCreateFolder(dirTempPath);
            cacf.SettingFolder(dirTempPath);
            GrabM.SetDirPath(dirTempPath);
            await Task.Run(() => {
                ScanInit(startposX, startposX, endposX, xSpeed);
                ImgSrcByte = new byte[0];
                CheckGrabStatus();

                BeginScanning(endposX, startposY);
            });
        }

        void ScanInit(int startposX, int startposY, int endposX, int speed)
        {
            XYStageControler.XYSetSpeed(20000, 20000, 0, 0);
            XYStageControler.XYMoveAbsPos(startposX, startposY);
            XYStageControler.XYWait2Arrive(startposX, startposY);
            XYStageControler.XYSetSpeed(speed, 500, 0, 0); // 500 is speed of Y. it is fixed Value
        }

        void CheckGrabStatus()
        {
            if (StatusGrab == GrabStatus.Frea)
            {
                if (DalsaMemObj.Xfer != null)
                {
                    StatusGrab = GrabStatus.Grab;
                    DalsaMemObj.Xfer.Grab();
                }
            }
        }

        void BeginScanning(int posX,int posY)
        {
            XYStageControler.XYMoveAbsPos(posX, posY);
            StatusFullScan = FullScanState.Start;
            XYStageControler.XYWait2ArriveEpsilon(posX, posY, 80);
        }

        #endregion

        #region Order
        public void Grap()
        {
            if (DalsaMemObj.Xfer != null)
            {
                StatusGrab = GrabStatus.Grab;
                DalsaMemObj.Xfer.Grab();
                ProfileTimer.Start();
            }
        }

        public void Freeze()
        {
            if (DalsaMemObj.Xfer != null)
            {
                StatusGrab = GrabStatus.Frea;
                DalsaMemObj.Xfer.Freeze();
                DalsaMemObj.Xfer.Wait(800);
                ProfileTimer.Stop();
            }
        }

        public void SaveImg(string path)
        {
            if (CurrentImg != null) CurrentImg.ToBitmap().Save(path);
            else
            {
                MessageBox.Show("Image is not Saved");
            }
        }

        public void DisposeMem()
        {
            var creatobject = new CreatesObjects();
            creatobject.DestroysObjects(DalsaMemObj);
        }

        #endregion

        #region XYStageOrder
        public void XYOrigin()
        {
        }

        public void XMoveAbsPos(int posX)
        {
            AcsXYZControl.XMove( posX );
        }

        public void YMoveAbsPos(int posY )
        {
            AcsXYZControl.YMove( posY );
        }

        public void XYWait2Arrive(int targetPosX,int targetPosY)
        {
            Console.WriteLine( "arrived come in " );
        }

        public void XYSetSpeed(int speedX, int speedY)
        {
            AcsXYZControl.SetSpeed( speedX, speedY , 100 );
        }

        #endregion

        #region Camera Setting
        public void SetExposure(double inputValue)
        {
            CameraSet.SetExposureTime(MbSession, inputValue);
        }

        public void SetLineRate( int inputValue)
        {
            CameraSet.SetLineRate(MbSession, inputValue);
        }


        #endregion

        #region ZStageOrder
        public void ZOrigin()
        {
            
        }

        public void ZMoveAbsPos(int posZ)
        {
            AcsXYZControl.ZMove( posZ );
        }

        public void ZWait2Arrive(int targetPosZ)
        {
            
        }

        public void SetSpeed(int speedX,int speedY, int speedZ)
        {
            AcsXYZControl.SetSpeed(speedX,speedY,speedZ);
        }


        #endregion

        #region RStage Order
        public void ROrigin( )
        {
            RStageControler.Origin();
        }

        public void RMoveAbsPos( int posR )
        {
            RStageControler.MoveAbsPos( posR );
        }

        public void RWait2Arrive( int targetPosR)
        {
            RStageControler.Wait2Arrive( targetPosR );
        }

        public void RSetSpeed( int speedR, int accR )
        {
            RStageControler.SetRSpeed( speedR, accR );
        }
        #endregion

        #region Init
        void TimerSetting()
        {
            ProfileTimer = new Timer();
            ProfileTimer.Interval = 100;
            ProfileTimer.Tick += new EventHandler(ProfileEventMethod);
        }

        bool CreateDeviceMana(IConnection connectModule)
        {
            try
            {
                DalsaMemObj.ServerLocation = new SapLocation(connectModule.ServerName, connectModule.ResourceIndex);
                DalsaMemObj.Acquisition = new SapAcquisition(DalsaMemObj.ServerLocation, connectModule.ConfigFile);

                if (SapBuffer.IsBufferTypeSupported(DalsaMemObj.ServerLocation, SapBuffer.MemoryType.ScatterGather))
                    DalsaMemObj.Buffers = new SapBufferWithTrash(2, DalsaMemObj.Acquisition, SapBuffer.MemoryType.ScatterGather);
                else
                    DalsaMemObj.Buffers = new SapBufferWithTrash(2, DalsaMemObj.Acquisition, SapBuffer.MemoryType.ScatterGatherPhysical);

                var objSetting = new ObjectSetting();
                objSetting.AcqusitionSetting(DalsaMemObj.Acquisition);

                DalsaMemObj.Xfer = new SapAcqToBuf(DalsaMemObj.Acquisition, DalsaMemObj.Buffers);
                DalsaMemObj.Xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;

                DalsaMemObj.View = new SapView(DalsaMemObj.Buffers);

                DalsaMemObj.Xfer.XferNotify += new SapXferNotifyHandler(GrabDoneEventMethod);
                DalsaMemObj.Xfer.XferNotifyContext = DalsaMemObj.View;

                var creatobject = new CreatesObjects();
                creatobject.CreatEndSqObject(DalsaMemObj.Buffers, DalsaMemObj.Xfer, DalsaMemObj.View);
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public void XYStageInit(int port)
        {
            AcsXYZControl.Connect( port );
            //XYStageControler = new DctXYStageControl();
            //ZStageControler = new DctZStageControl();
            //if (!XYStageControler.XYStageConnect(port.ToString())) { XYStageControler = new DumyDctXYStageControl(); }
            //if (!ZStageControler.ZStageConnect(port.ToString())) { ZStageControler = new DumyDctZStageControl(); }
        }

        public void RStageInit( int port )
        {
            RStageControler = new SigmaRStageControl();
            if ( !RStageControler.RStageConnect( port.ToString() ) ) { RStageControler = new DummySigmaRStageControl(); }
        }

        public void ConnectVISA2Cam(string path)
        {
            ConnectVISA visaConnection = new ConnectVISA();
            visaConnection.Connect2VISA(ref MbSession, path);
        }

        void InitCount()
        {
            BuffCount = 0;
            UnitCount = 0;
            LineCount = 0;
        }
        #endregion

        #region Save
        public void SaveImageData( Emgu.CV.UI.ImageBox[,] imgbox,string savepath )
        {
            try
            {

                for ( int i = 0; i < imgbox.GetLength( 0 ); i++ )
                {
                    for ( int j = 0; j < imgbox.GetLength( 1 ); j++ )
                    {
                        string temp = i.ToString( "D2" ) + "_"+j.ToString( "D3" );
                        string outpath = System.IO.Path.Combine( savepath, temp );
                        imgbox[i, j].Image.Save( String.Format( outpath + ".bmp" ) );
                    }
                }

            }
            catch ( Exception ex)
            {
                MessageBox.Show( ex.ToString() );
            }
        }
        #endregion
    }
}
