using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnGraphs1_Click(object sender, EventArgs e)
        {
            if (radNum1.Checked)
            {
                Dictionary<string, int> tags = new Dictionary<string, int>() 
                { 
                    { "Physical Aggression", 16 },
                    { "Verbal Aggression", 24 },
                    { "Short attention span", 11 },
                    { "Irritability", 51 },
                    { "Skipping meals", 178 }
                };
                startCharts(tags);
            }
            else if (radNum2.Checked)
            {
                Dictionary<string, int> tags = new Dictionary<string, int>() 
                { 
                    { "12/02/13", 1 },
                    { "12/04/13", 1 },
                    { "12/06/13", 1 },
                    { "12/08/13", 1 },
                    { "12/10/13", 1 },
                    { "12/12/13", 2 },
                    { "12/14/13", 1 },
                    { "12/16/13", 1 },
                    { "12/18/13", 0 },
                    { "12/20/13", 1 },
                    { "12/22/13", 2 },
                    { "12/24/13", 2 }
                };
                startCharts(tags);
            }
            else if (radNum3.Checked)
            {
                Dictionary<string, int> tags = new Dictionary<string, int>() 
                { 
                    { "Physical Aggression", 16 },
                    { "Verbal Aggression", 24 },
                    { "Short attention span", 11 },
                    { "Irritability", 51 },
                    { "Skipping meals", 178 }
                };
                startCharts(tags);
            }

            
            
        }

        private void chkNumbers_CheckedChanged(object sender, EventArgs e)
        {
            if (chartTest1.Series[0].IsValueShownAsLabel == false)
            {
                chartTest1.Series[0].IsValueShownAsLabel = true;
                chartTest2.Series[0].IsValueShownAsLabel = true;
                chartTest3.Series[0].IsValueShownAsLabel = true;
                chartTest4.Series[0].IsValueShownAsLabel = true;
            }
            else
            {
                chartTest1.Series[0].IsValueShownAsLabel = false;
                chartTest2.Series[0].IsValueShownAsLabel = false;
                chartTest3.Series[0].IsValueShownAsLabel = false;
                chartTest4.Series[0].IsValueShownAsLabel = false;
            }
        }

        private void startCharts(Dictionary<string,int> tags)
        {
            chartTest1.Series[0].Points.Clear();
            chartTest1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            foreach (string tagname in tags.Keys)
            {
                chartTest1.Series[0].Points.AddXY(tagname, tags[tagname]);
            }

            chartTest2.Series[0].Points.Clear();
            chartTest2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartTest2.Series[0].BorderWidth = 2;
            foreach (string tagname in tags.Keys)
            {
                chartTest2.Series[0].Points.AddXY(tagname, tags[tagname]);
            }

            chartTest3.Series[0].Points.Clear();
            chartTest3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            foreach (string tagname in tags.Keys)
            {
                chartTest3.Series[0].Points.AddXY(tagname, tags[tagname]);
            }

            chartTest4.Series[0].Points.Clear();
            chartTest4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pyramid;
            foreach (string tagname in tags.Keys)
            {
                chartTest4.Series[0].Points.AddXY(tagname, tags[tagname]);
            }
        }
    }
}