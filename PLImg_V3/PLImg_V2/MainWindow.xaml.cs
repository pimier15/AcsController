﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using IDalsaCamera;
using DALSA.SaperaLT.SapClassBasic;
using DALSA.SaperaLT;
using Accord.Math;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;

namespace PLImg_V2
{
    enum StageEnableState {
        Enabled,
        Disabled
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        string CamPath     ;
        string XYStagePort    ;
        string ControllerIP    ;
        int RStagePort     ;
        bool FeedBackOn = false;

        MainModule ModMain;
        public SeriesCollection seriesbox { get; set; }
        public ChartValues<int> chartV { get; set; }
        List<int> XLabels;
        List<int> YValue;
        ImageBox[,] ImgBoxArr;
        IDisplayEmgu ModDisplay;
        StageEnableState StageState; 

        public MainWindow()
        {
            InitializeComponent();
            InitConnectPort();
            InitMainMod();
            InitChart();
            InitImgBox();
            SetImgBoxStretch();
            DataContext = this;
        }

        #region Display
        void DisplayAF(double input)
        {
            lblAFV.Content = input.ToString();
        }

        void DisplayRealTime(Image<Gray, byte> img)
        {
            ModDisplay = new Display();
            ModDisplay.Display(imgboxReal, img);
        }

        void DisplayScaned(Image<Gray, byte> img)
        {
            ModDisplay = new Display();
        }

        void DisplayRealTimeProfile(byte[] input)
        {
            AsySetLineValue(Arr2List(input), seriesbox);
        }

        void DisplayBuffNumber(int num)
        {
            lblBuffNum.BeginInvoke(() => lblBuffNum.Content = num.ToString());
        }

        void DisplayFullScanImg(Image<Gray, Byte> img, int lineNum, int unitNum)
        {
            ModDisplay = new Display();
            ModDisplay.Display(ImgBoxArr[unitNum , lineNum], img);
        }

        void SetImgBoxFit()
        {
            foreach (var item in ImgBoxArr)
            {
                item.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        void SetImgBoxStretch()
        {
            foreach (var item in ImgBoxArr)
            {
                item.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        #endregion

        #region Init
        void InitConnectPort( )
        {
            ConnectionData condata = new ConnectionData();
            CamPath     = condata.CameraPath    ;
            XYStagePort = condata.DctStagePort  ;
            ControllerIP = condata.ControllerIP  ;
            RStagePort  = condata.RStage        ;
        }


        void InitMainMod( )
        {
            StageState = StageEnableState.Enabled;

            ModMain = new MainModule();
            ModMain.evtRealimg += new TransImgArr( DisplayRealTime );
            ModMain.evtByteArrOneLine += new TransbyteArr( DisplayRealTimeProfile );
            ModMain.evtVarianceValue += new TransDoubleNumber( DisplayAF );
            ModMain.evtBuffNum += new TransNumber( DisplayBuffNumber );
            ModMain.evtScanImgOnGoing += new TransImgArr( DisplayScaned );
            ModMain.evtLineScanStart += new TransScanStatus( SetImgBoxStretch );
            ModMain.evtLineScanCom += new TransScanStatus( SetImgBoxFit );
            ModMain.evtFScanImgOnGoing += new TransSplitImgArr( DisplayFullScanImg );
            ModMain.evtFeedbackPos += new TransFeedBackPos( DisplayPos );

            imgboxReal.SizeMode = PictureBoxSizeMode.StretchImage;

            ModMain.ConnectVISA2Cam( CamPath );
            //ModMain.XYZStageInit( ControllerIP );
            ModMain.XYZStageInitCom( XYStagePort );
            ModMain.RStageInit( RStagePort );

            InitViewWin();
        }

        void InitChart( )
        {
            XLabels = new List<int>();
            YValue = new List<int>();
        }

        void InitImgBox()
        {
            ImgBoxArr = new ImageBox[4,4];
            ImgBoxArr[0, 0] = imgboxScan00;
            ImgBoxArr[0, 1] = imgboxScan01;
            ImgBoxArr[0, 2] = imgboxScan02;
            ImgBoxArr[0, 3] = imgboxScan03;

            ImgBoxArr[1, 0] = imgboxScan10;
            ImgBoxArr[1, 1] = imgboxScan11;
            ImgBoxArr[1, 2] = imgboxScan12;
            ImgBoxArr[1, 3] = imgboxScan13;

            ImgBoxArr[2, 0] = imgboxScan20;
            ImgBoxArr[2, 1] = imgboxScan21;
            ImgBoxArr[2, 2] = imgboxScan22;
            ImgBoxArr[2, 3] = imgboxScan23;

            ImgBoxArr[3, 0] = imgboxScan30;
            ImgBoxArr[3, 1] = imgboxScan31;
            ImgBoxArr[3, 2] = imgboxScan32;
            ImgBoxArr[3, 3] = imgboxScan33;
        }

        void ClearImgBox()
        {
            for (int i = 0; i < ImgBoxArr.GetLength(0); i++)
            {
                for (int j = 0; j < ImgBoxArr.GetLength(1); j++)
                {
                    ImgBoxArr[i, j].Image = null;
                }
            }
        }

        void InitViewWin( )
        {
            nudEndXPos.Value = 80;
            nudStartXPos.Value = 120;
            nudStartYPos.Value = 0;
            nudEndYPos.Value = 0;
            nudXSpeed.Value = 50;
            nudYSpeed.Value = 50;
            nudExtime.Value = 400;
            nudlinerate.Value = 4000;
            nudRSpeed.Value = 200;
            nudGoXPos.Value = 100;
            nudGoYPos.Value = 50;
            nudGoZPos.Value = 27.9;
            nudZSpeed.Value = 10;
        }

        void DisplayPos(double[] inputPos)
        {
            Task.Run( ( ) => lblXpos.BeginInvoke( (Action)(( ) => lblXpos.Content = inputPos[0].ToString()) ) );
            Task.Run( ( ) => lblYpos.BeginInvoke( (Action)(( ) => lblYpos.Content = inputPos[1].ToString()) ) );
            //Task.Run( ( ) => lblZpos.BeginInvoke( (Action)(( ) => lblYpos.Content = inputPos[2].ToString()) ) );
        }
        #endregion

        #region MainWindowEvent
        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            flyFunc.IsOpen = false;
            flySetting.IsOpen = true;
        }
        private void btnFunc_Click(object sender, RoutedEventArgs e)
        {
            flySetting.IsOpen = false;
            flyFunc.IsOpen = true;
        }
        private void btnLineScan_Click(object sender, RoutedEventArgs e)
        {
            ClearImgBox();
            ModMain.StartLineScan( (int)nudStartXPos.Value, (int)nudEndXPos.Value, (int)nudXSpeed.Value );
        }
        private void btnFullScan_Click(object sender, RoutedEventArgs e)
        {
            ClearImgBox();
            ModMain.StartFullScan((int)nudStartXPos.Value, (int)nudStartYPos.Value,(int)nudEndXPos.Value,(int)nudXSpeed.Value);
        }

        #region Camera
        private void btnGrap_Click(object sender, RoutedEventArgs e)
        {
            ModMain.Grap();
        }
        private void btnFreeze_Click( object sender, RoutedEventArgs e )
        {
            ModMain.Freeze();
        }
        private void btnSaveData_Click(object sender, RoutedEventArgs e)
        {
            string savePath = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if ( fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                savePath = fbd.SelectedPath;
            }
            ModMain.SaveImageData( ImgBoxArr, savePath );
        }

        private void nudExtime_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double?> e )
        {
            try
            {
                ModMain.SetExposure( (double)nudExtime.Value );
            }
            catch ( Exception )
            {
                nudExtime.Value = 0;
            }
        }
        private void nudlinerate_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double?> e )
        {
            try
            {
                //ModMain.SetLineRate( (int)nudlinerate.Value );
            }
            catch ( Exception )
            {
                nudlinerate.Value = 0;
            }
        }
        #endregion

        #region Stage
        /* Helper*/
        void SetSpeedXYZ( )
        {
            try
            {
                ModMain.SetSpeed( (int)nudXSpeed.Value, (int)nudYSpeed.Value, (int)nudZSpeed.Value );
            }
            catch ( Exception )
            {
                nudXSpeed.Value = 2;
                nudYSpeed.Value = 0;
                nudZSpeed.Value = 0;
            }
        }

        //common
        private void btnOrigin_Click( object sender, RoutedEventArgs e ) {
            ModMain.Home();
        }

        private void btnEnable_Click( object sender, RoutedEventArgs e ) {
            ModMain.EnableStage();
            StageState = StageEnableState.Enabled;
        }

        private void btnDisable_Click( object sender, RoutedEventArgs e ) {
            ModMain.DisableStage(0);
            ModMain.DisableStage(1);
            ModMain.DisableStage(2);
            StageState = StageEnableState.Disabled;
        }
        private void btnXYBuffClear_Click( object sender, RoutedEventArgs e ) {
            ModMain.Buffclear();
        }

        private void btnHalt_Click( object sender, RoutedEventArgs e ) {
            ModMain.HaltStage();
        }

        private void nudXSpeed_KeyUp( object sender, System.Windows.Input.KeyEventArgs e ) {
            ModMain.SetSpeed( (int)nudXSpeed.Value, (int)nudYSpeed.Value, (int)nudZSpeed.Value );
        }

        private void nudYSpeed_KeyUp( object sender, System.Windows.Input.KeyEventArgs e ) {
            ModMain.SetSpeed( (int)nudXSpeed.Value, (int)nudYSpeed.Value, (int)nudZSpeed.Value );
        }

        private void nudZSpeed_KeyUp( object sender, System.Windows.Input.KeyEventArgs e ) {
            ModMain.SetSpeed( (int)nudXSpeed.Value, (int)nudYSpeed.Value, (int)nudZSpeed.Value );
        }


        // XYStage //
        private void btnYMove_Click( object sender, RoutedEventArgs e )
        {
            if ( StageState == StageEnableState.Enabled )
            {
                ModMain.YMoveAbsPos( (int)nudGoYPos.Value );

                if ( !FeedBackOn )
                {
                    Task.Run( ( ) => ModMain.GetFeedbackPos() );
                    
                    FeedBackOn = true;
                }

            }
        }

        private void btnXMove_Click( object sender, RoutedEventArgs e )
        {
            if ( StageState == StageEnableState.Enabled )
            {
                ModMain.XMoveAbsPos( (int)nudGoXPos.Value );
                if ( !FeedBackOn )
                {
                    Task.Run( ( ) => ModMain.GetFeedbackPos() );
                    FeedBackOn = true;
                }
            }
        }

        

        // ZStage //
        private void btnZMove_Click( object sender, RoutedEventArgs e )
        {
            if(StageState == StageEnableState.Enabled) ModMain.ZMoveAbsPos( (int)nudGoZPos.Value );
        }

        // R Stage //
        private void btnRMove_Click( object sender, RoutedEventArgs e )
        {
            ModMain.RMoveAbsPos( (int)nudGoRPos.Value );
        }
        private void nudRSpeed_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double?> e )
        {
            ModMain.RSetSpeed( (int)nudRSpeed.Value, 0 );
        }
        private void btnROrigin_Click( object sender, RoutedEventArgs e )
        {
            ModMain.ROrigin();
        }
        private void btnRForceStop_Click( object sender, RoutedEventArgs e )
        {
            
        }



        #endregion

        #endregion

        #region Chart
        async void AsySetLineValue(List<int> Yinput, SeriesCollection seriescol)
        {
            makeseries();
            //connectValue();
            
            await Task.Run(() => {
                YValue = Yinput;
                for (int i = 0; i < YValue.Count; i++)
                {
                    XLabels.Add(i);
                }

                connectValue();
                lineProChart.Dispatcher.BeginInvoke( (Action)(( ) => seriesbox[0].Values = chartV) );
            } );
            
            

            //await Task.Run(() => lineProChart.Dispatcher.BeginInvoke(
            //    (Action)(() => seriesbox[0].Values = chartV)));
        }
        void connectValue()
        {
            chartV = new ChartValues<int>(YValue);
        }
        void makeseries()
        {
            seriesbox = new SeriesCollection {
                new LineSeries {
                    Title = "LineProfileSeries",
                    Values = chartV,
                    PointGeometry = null

                }
            };
        }
        List<int> Arr2List(byte[] input)
        {
            List<int> output = new List<int>();

            for (int i = 0; i < input.Length; i++)
            {
                output.Add((int)input[i]);
            }
            return output;
        }




        #endregion

        private void Label_KeyUp( object sender, System.Windows.Input.KeyEventArgs e )
        {

        }

        private void nudlinerate_KeyUp( object sender, System.Windows.Input.KeyEventArgs e )
        {
            ModMain.SetLineRate( (int)nudlinerate.Value );
        }


        private void nudRSpeed_KeyUp( object sender, System.Windows.Input.KeyEventArgs e ) {
            
        }

        private void MetroWindow_Closing( object sender, System.ComponentModel.CancelEventArgs e ) {
            ModMain.DisposeStage();
        }

        private void btnXDisable_Click( object sender, RoutedEventArgs e ) {
            ModMain.DisableStage( 0 );
        }

        private void btnYDisable_Click( object sender, RoutedEventArgs e ) {
            ModMain.DisableStage( 1 );
        }

        private void btnZDisable_Click( object sender, RoutedEventArgs e ) {
            ModMain.disz();
        }
    }
}
