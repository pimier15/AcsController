using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace DalsaTest
{
    public class SeriesMana
    {
        public void InitChartArea(Chart chart)
        {
            if (chart == null) return;
            
            chart.ChartAreas[0].AxisX.LineColor              = System.Drawing.Color.White;
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor    = System.Drawing.Color.White;
            chart.ChartAreas[0].AxisY.LineColor              = System.Drawing.Color.White;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor    = System.Drawing.Color.White;
            chart.ChartAreas[0].BackColor                    = System.Drawing.Color.Black;

            chart.ChartAreas[0].AxisX.Title = "Pixel Number";
            chart.ChartAreas[0].AxisY.Title = "Intensity";
            chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;

            chart.ChartAreas[0].AxisY.Maximum  = 300;
            chart.ChartAreas[0].AxisX.Maximum  = 6144;
            chart.ChartAreas[0].AxisX.Interval = 1000;
            chart.ChartAreas[0].AxisY.Interval = 50;
            chart.ChartAreas[0].AxisX.Minimum  = 0;
            chart.ChartAreas[0].AxisX.Maximum  = 6144;

            chart.ChartAreas[0].Position.X = 0;
            chart.ChartAreas[0].Position.Y = 0;
            chart.ChartAreas[0].InnerPlotPosition.X = 0;
            chart.ChartAreas[0].InnerPlotPosition.Y = 0;

            chart.ChartAreas[0].Position.Width  = 100;
            chart.ChartAreas[0].Position.Height = 100;
            chart.ChartAreas[0].InnerPlotPosition.Width  = 100;
            chart.ChartAreas[0].InnerPlotPosition.Height = 100;

            chart.BackColor = System.Drawing.Color.FromArgb(96, 60, 186);
        }

        public void SetMinMaxChart(Chart chart, Series series)
        {
            chart.ChartAreas[0].AxisY.Maximum  = 260;
            chart.ChartAreas[0].AxisX.Maximum  = 6144;
            chart.ChartAreas[0].AxisX.Interval = 1000;
            chart.ChartAreas[0].AxisY.Interval = 50;
            chart.ChartAreas[0].AxisX.Maximum  = 6144;
        }

        public void InitSeries(Series series)
        {
            if (series == null) return;
            series.ChartType = SeriesChartType.Line;
            series.Color = System.Drawing.Color.Ivory;
        }

        public void SetDataToSeries(Series series, List<byte> inputlistX, List<byte> inputlistY)
        {
            for (int i = 0; i < inputlistX.Count; i++)
            {
                series.Points.AddXY(inputlistX[i], inputlistY[i]);
            }
        }

        public void SetDataToSeries(Series series, double[] inputX)
        {
            if (series.Points != null) series.Points.Clear();
            for (int i = 0; i < inputX.Length ; i++)
            {
                series.Points.AddXY( i, inputX[i]);
            }
        }

    }
}
