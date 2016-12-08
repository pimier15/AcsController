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
    public delegate void TransFeedBackPos(double[] XYZPos);

    public class MainModule
    {
        readonly int YStep = 100;

        public event TransbyteArr       evtByteArrOneLine     ;
        public event TransImgArr        evtRealimg            ;
        public event TransSplitImgArr   evtFScanImgOnGoing    ;
        public event TransScanStatus    evtScanStart      ;
        public event TransScanStatus    evtScanEnd        ;
        public event TransDoubleNumber  evtVarianceValue      ;
        public event TransFeedBackPos   evtFeedbackPos        ;

        Timer                 ProfileTimer    ;
        MessageBasedSession   MbSession       ;
        AFCalc                AFMeasrue       ;
        FullScanData          DataFullScan    ;
        IGrabMana             GrabM           ;
        IMemberDefine         DalsaMemObj     ;
        GrabStatus            StatusGrab      ;
        FullScanState         StatusFullScan  ;
        ICameraSetting        CameraSet       ;
        SigmaRStageControl   RStageController ;
        AcsContol             AcsXYZControl   ;

        Image<Gray, byte>     CurrentImg      ;
        byte[]                ImgSrcByte      ;
        int                   BuffCount       ;
        int                   LineCount       ;
        int                   UnitCount       ;
        double[]              FeedBackPos     ;
        int NextXPos;
        ScanDirection         ScanDirec       ;
        int StartP;
        int EndP;

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
            FeedBackPos = new double[3];
            
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
            ScanDirec = ScanDirection.Forward;

            Console.WriteLine( StatusFullScan.ToString() );

            #region New
            switch (StatusFullScan) {
                case FullScanState.Stop:
                    StatusFullScan = FullScanState.Wait;
                    Freeze();
                    //evtScanEnd();
                    break;

                case FullScanState.Pause:
                    double currentYPos = AcsXYZControl.GetMotorFPos()[1];
                    AcsXYZControl.YMove( 3 );
                    AcsXYZControl.Wait2ArriveEpsilon( "Y", currentYPos + 3 , 0.02);
                    System.Threading.Thread.Sleep( 100 ); // Need Pos Check Later //sjw
                    if ( ScanDirec == ScanDirection.Forward )
                    {
                        ScanDirec = ScanDirection.Back;
                        NextXPos = StartP;
                    }
                    else
                    {
                        ScanDirec = ScanDirection.Forward;
                        NextXPos = EndP;
                    }
                

                    AcsXYZControl.XMove( NextXPos );
                    StatusFullScan = FullScanState.Start;
                    
                    break;

                case FullScanState.Start:
                    //evtScanStart();
                    ImgSrcByte = Matrix.Concatenate<byte>(ImgSrcByte, buffData);
                    //SaveFullDat(ImgSrcByte,LineCount,UnitCount,BuffCount);

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
            byte[] sliced = GrabM.DataTransFromBufferOneLine(DalsaMemObj.Buffers);
            //byte[] sliced = ArrayControl.Slice( buffOneLineDatga, 0, 6143 );
            //buffOneLineDatga = null;
            evtVarianceValue( AFMeasrue.CalcAFV( sliced ) );
            evtByteArrOneLine( sliced );
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

        #region Feedback
        public void GetFeedbackPos()
        {
            while ( true )
            {
                evtFeedbackPos( AcsXYZControl.GetMotorFPos() );
                Task.Delay( 500 ).Wait();
            }
        }
        #endregion

        #region Scanning
        void SetStartEnd( int startposX , int endposX )
        {
            StartP = startposX;
            EndP = endposX;
        }

        public async void StartLineScan(int startposX, int endposX , int speed)
        {
            ReadyLineScan( startposX );
            DataFullScan.LineLimit = 0;
            InitCount();
            SetDir();
            await Task.Run(()=> {
                ImgSrcByte = new byte[0];
                System.Threading.Thread.Sleep( 2000 );
                AcsXYZControl.XMove( endposX );
                System.Threading.Thread.Sleep( 2500 );
                StatusFullScan = FullScanState.Start;
            });
        }

        async public void ReadyLineScan( int startpos )
        {
            AcsXYZControl.SetXSpeed( 200 );
            AcsXYZControl.XMove( startpos );
            AcsXYZControl.Wait2ArriveEpsilon( "X", startpos, 1.0 );
            AcsXYZControl.SetXSpeed( DataFullScan.ScanSpeed );
            await Task.Delay( 1500 );
        }

        public async void StartFullScan(int startposX, int startposY,int endposX, int xSpeed)
        {
            DataFullScan.LineLimit = 3;
            InitCount();
            SetDir();
            
            await Task.Run(() => {
                //ScanInit(startposX, startposX, endposX, xSpeed);
                ImgSrcByte = new byte[0];
                //CheckGrabStatus();
                AcsXYZControl.XMove( endposX );
                BeginScanning(endposX, startposY);
            });
        }

        void SetDir()
        {
            string dirTempPath = String.Format(ImgBasePath + DateTime.Now.ToString("MM/dd/HH/mm/ss"));
            CheckAndCreateFolder cacf = new CheckAndCreateFolder(dirTempPath);
            cacf.SettingFolder( dirTempPath );
            GrabM.SetDirPath( dirTempPath );
        }

        void LlineScanInit( int startposX )
        {
            AcsXYZControl.SetSpeed( 100 , 100 , 100 );
            AcsXYZControl.XMove( startposX );
            //AcsXYZControl.YMove( startposY );
            System.Threading.Thread.Sleep( 2000 );
        }

        void ScanInit(int startposX, int startposY, int endposX, int speed)
        {
            AcsXYZControl.SetSpeed( 100 , 100 , 100 );
            AcsXYZControl.XMove( startposX );
            //AcsXYZControl.YMove( startposY );
            System.Threading.Thread.Sleep( 2000 );
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
            //XYStageControler.XYMoveAbsPos(posX, posY);
            StatusFullScan = FullScanState.Start;
            //XYStageControler.XYWait2ArriveEpsilon(posX, posY, 80);
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
        public void EnableStage( int axis ) { AcsXYZControl.EnableMotor(axis); }

        public void DisableStage( int axis ) {
                AcsXYZControl.DisableMotor( axis );
        }

        public void XYOrigin()
        {
        }

        public void XMoveAbsPos( double posX )
        {
            AcsXYZControl.SetXSpeed( 400 );
            AcsXYZControl.XMove( posX );
            AcsXYZControl.Wait2ArriveEpsilon( "X", posX, 1.0 );
            AcsXYZControl.SetXSpeed( DataFullScan.ScanSpeed );

        }

        public void disz( ) {
            AcsXYZControl.DisZ();
        }

        public void YMoveAbsPos( double posY )
        {
            AcsXYZControl.SetYSpeed( 400 );
            AcsXYZControl.YMove( posY );
            AcsXYZControl.Wait2ArriveEpsilon( "Y", posY, 1.0 );
        }

        public void XYWait2Arrive(int targetPosX,int targetPosY)
        {
            Console.WriteLine( "arrived come in " );
        }

        public void XYSetSpeed(int speedX, int speedY)
        {
            AcsXYZControl.SetSpeed( speedX, speedY , 100 );
        }

        public void HaltStage( ) {
            AcsXYZControl.Halt();
        }

        public void Home( ) {
            AcsXYZControl.Home();
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

        public void ZMoveAbsPos(double posZ)
        {
            AcsXYZControl.ZMove( posZ );
        }

        public void ZMoveRelPos(double posZ ) {
            AcsXYZControl.ZMoveRel( posZ );
        }

        public void ZWait2Arrive(int targetPosZ)
        {
            
        }

        public void SetSpeed(int speedX,int speedY, int speedZ)
        {
            AcsXYZControl.SetSpeed(speedX,speedY,speedZ);
        }

        public void Buffclear( ) { }

        #endregion

        #region RStage Order
        public void ROrigin( )
        {
            RStageController.Origin();
        }

        public void RMoveAbsPos( double posR )
        {
            RStageController.MoveAbsPos( posR );
        }

        public void RSetSpeed( int speedR, int accR )
        {
            RStageController.SetRSpeed( speedR, accR );
        }

        public string RGetPosition( ) {
            return RStageController.GetPosition();
        }
        public string RPositionRead( ) {
            string fir = RStageController.GetPosition();
            var splitarr = fir.Split( ',' );
            try
            {
                var output = Convert.ToDouble(splitarr[0]) / 400 ;
                return output.ToString();
            }
            catch ( Exception )
            {
                return  "999.99";
            }
        }

        public void RStageClose( ) {
            RStageController.RstageRelease();
        }
        #endregion

        #region Init
        void TimerSetting()
        {
            ProfileTimer = new Timer();
            ProfileTimer.Interval = 500;
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

        public void XYZStageInit(string addIP)
        {
            AcsXYZControl.Connect( addIP );
            //AcsXYZControl.Home();
        }

        public void XYZStageInitCom( string comport ) {
            AcsXYZControl.ConnectCom( comport );
            //AcsXYZControl.Home();
        }

        public void RStageInit( int port )
        {
            //RStageControler = new DummySigmaRStageControl();
            RStageController = new SigmaRStageControl();
            RStageController.RStageConnect( port.ToString() );
            //if ( !RStageController.RStageConnect( port.ToString() ) ) { RStageController = new DummySigmaRStageControl(); }
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
                        if ( imgbox[i, j].Image != null )
                        {
                            string temp = i.ToString( "D2" ) + "_"+j.ToString( "D3" );
                            string outpath = System.IO.Path.Combine( savepath, temp );
                            imgbox[i, j].Image.Save( String.Format( outpath + ".bmp" ) );
                        }
                    }
                }

            }
            catch ( Exception ex)
            {
                MessageBox.Show( ex.ToString() );
            }
        }
        #endregion

        public void ChangeBuffNum(int input ) {
            DataFullScan.BuffLimit = input;
        }

        public void ChangeUnitNum(int input ) {
            DataFullScan.UnitLimit = input;
        }

        public void ChangeScanSpeed(int input ) {
            DataFullScan.ScanSpeed = input;

        }

        public void TestMethod() {
            RStageController.GO();
        }
    }
}
