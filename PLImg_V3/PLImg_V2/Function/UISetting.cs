using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace PLImg_V2
{
    public class UISetting
    {
        void SettingChart(Chart chart)
        {
            chart.ChartAreas[0].InnerPlotPosition.X = 0;
        }

    }
}
