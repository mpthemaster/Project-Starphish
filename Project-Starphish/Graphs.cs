using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public partial class FormMain
    {
        private bool firstTime = true;
        private List<DailyBehavior> dailyBehaviors = new List<DailyBehavior>();

        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();


        /// <summary>
        /// The function that starts the graphing tab
        /// </summary>
        private void mainGraph()
        {
            DailyBehavior temp = new DailyBehavior(5, "brooding", "mild", new DateTime(2014, 02, 20), "morning", "Kevin");
            DailyBehavior temp2 = new DailyBehavior(5, "brooding", "severe", new DateTime(2014, 01, 30), "morning", "Kevin");

            DailyBehavior temp3 = new DailyBehavior(5, "throwing", "severe", new DateTime(2014, 02, 14), "morning", "Kevin");
            DailyBehavior temp4 = new DailyBehavior(5, "throwing", "severe", new DateTime(2014, 03, 22), "morning", "Kevin");

            DailyBehavior temp5 = new DailyBehavior(5, "raging", "severe", new DateTime(2014, 01, 15), "morning", "Kevin");
            DailyBehavior temp6 = new DailyBehavior(5, "raging", "severe", new DateTime(2014, 03, 20), "morning", "Kevin");

            DailyBehavior temp7 = new DailyBehavior(5, "eating", "severe", new DateTime(2014, 02, 20), "morning", "Kevin");
            DailyBehavior temp8 = new DailyBehavior(5, "eating", "severe", new DateTime(2014, 01, 30), "morning", "Kevin");

            DailyBehavior temp9 = new DailyBehavior(5, "not eating", "severe", new DateTime(2013, 12, 20), "morning", "Kevin");
            DailyBehavior temp10 = new DailyBehavior(5, "not eating", "severe", new DateTime(2013, 12, 25), "morning", "Kevin");

            DailyBehavior temp11 = new DailyBehavior(5, "whistling", "severe", new DateTime(2013, 01, 10), "morning", "Kevin");
            DailyBehavior temp12 = new DailyBehavior(5, "whistling", "severe", new DateTime(2014, 02, 04), "morning", "Kevin");
            DailyBehavior temp14 = new DailyBehavior(5, "whistling", "severe", new DateTime(2014, 02, 28), "morning", "Kevin");
            DailyBehavior temp15 = new DailyBehavior(5, "whistling", "severe", new DateTime(2014, 03, 04), "morning", "Kevin");

            DailyBehavior temp13 = new DailyBehavior(5, "gaming", "severe", new DateTime(2014, 03, 17), "morning", "Kevin");

            // foreach(DailyBehavior DailyBehavior in dailyBehaviors)
            //  {
            //       dailyBehaviors.Add(DailyBehavior);
            // }
            dailyBehaviors.Add(temp);
            dailyBehaviors.Add(temp2);
            dailyBehaviors.Add(temp3);
            dailyBehaviors.Add(temp14);
            dailyBehaviors.Add(temp15);
            dailyBehaviors.Add(temp4);
            dailyBehaviors.Add(temp5);
            dailyBehaviors.Add(temp6);
            dailyBehaviors.Add(temp7);
            dailyBehaviors.Add(temp8);
            dailyBehaviors.Add(temp9);
            dailyBehaviors.Add(temp10);
            dailyBehaviors.Add(temp11);
            dailyBehaviors.Add(temp12);
            dailyBehaviors.Add(temp13);
            dailyBehaviors.Add(temp);
            dailyBehaviors.Add(temp);
            dailyBehaviors.Add(temp);


            int i = 0;
            foreach (DailyBehavior DailyBehavior in dailyBehaviors)
            {
                //  MessageBox.Show(temp.Behavior);
                i++;
            }


            if (firstTime)
            {
                //Setting the two combo boxes in the graph tab to have default selections
                comboPickTimeGraphs.SelectedIndex = 0;//last 30 days
                comboBehaviorsToGraph.SelectedIndex = 1;//top 5 behaviors

                //Only the combo box that selects the date is enabled by default
                everythingDisabled();
                comboPickTimeGraphs.Enabled = true;

                //likewise, the list box for custom behaviors is disabled by default
                //because it defaults to use the top 5 behaviors
                listBehaviorsToGraph.Enabled = false;

                getGraphRange();
                firstTime = false;
            }
        }

        /// <summary>
        /// Checks to see which behaviors are selected to be graphed, how far
        /// back the graph should go, and then calls the function to graph them
        /// </summary>
        private void getGraphRange()
        {
            if (radUseTimeFrames.Checked)
            {
                if (comboPickTimeGraphs.SelectedIndex == 0)//last 30 days
                {
                    startDate = DateTime.Today.AddDays(-30);
                    endDate = DateTime.Today;
                }
                else if (comboPickTimeGraphs.SelectedIndex == 1)//last 60 days
                {
                    startDate = DateTime.Today.AddDays(-60);
                    endDate = DateTime.Today;
                }
                else if (comboPickTimeGraphs.SelectedIndex == 2)//current quarter
                {
                    endDate = DateTime.Today;

                    //The companies quarters are:
                    //Quarter 1: July to September
                    //Quarter 2: October - December
                    //Quarter 3: January - March
                    //Quarter 4: April - June

                    //July August September
                    if (DateTime.Today.Month == 07 || DateTime.Today.Month == 08 || DateTime.Today.Month == 09)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 07, 01);
                    }

                    //October November December
                    else if (DateTime.Today.Month == 10 || DateTime.Today.Month == 11 || DateTime.Today.Month == 12)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 10, 01);
                    }

                    //January February or March
                    else if (DateTime.Today.Month == 01 || DateTime.Today.Month == 02 || DateTime.Today.Month == 03)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 01, 01);
                    }

                    //April May June
                    else if (DateTime.Today.Month == 04 || DateTime.Today.Month == 05 || DateTime.Today.Month == 06)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 04, 01);
                    }
                }
            }
            else if (radUseCustomDates.Checked)
            {
                startDate = new DateTime(datePickerBeginGraphs.Value.Year, datePickerBeginGraphs.Value.Month, 1);
                endDate = new DateTime(datePickerBeginGraphs.Value.Year, datePickerBeginGraphs.Value.Month, System.DateTime.DaysInMonth(datePickerBeginGraphs.Value.Year, datePickerBeginGraphs.Value.Month));
            }
            else if (radUseCustomQuarters.Checked)
            {
                //The companies quarters are:
                //Quarter 1: July to September
                //Quarter 2: October - December
                //Quarter 3: January - March
                //Quarter 4: April - June

                //Checks to see if none of the checkboxes are checked
                if (!chkQuarter1.Checked && !chkQuarter2.Checked && !chkQuarter3.Checked && !chkQuarter4.Checked)
                    MessageBox.Show("Error: No Quarter was Selected");

                if (chkQuarter1.Checked)
                {           
                }
                if (chkQuarter2.Checked)
                {
                }
                if (chkQuarter3.Checked)
                {
                }
                if (chkQuarter4.Checked)
                {
                }
            }
            getBehaviors(startDate, endDate);
        }

        /// <summary>
        /// This gets all of the behaviors from the dailyBehaviors list and puts them into
        /// a dictionary object(after culling them depending on the date of the behaviors and the
        /// dates selected to be viewed)
        /// </summary>
        /// <param name="startDate">The beginning of the date range to be graphed</param>
        /// <param name="endDate">The end of the date range to be graphed</param>
        private void getBehaviors(DateTime startDate, DateTime endDate)
        {

            //a dictionary object to hold the relevant behaviors 
            Dictionary<string, int> tags = new Dictionary<string, int>();
            for (int i = 0; i < dailyBehaviors.Count(); i++)//for every behavior 
            {
                //this checks to see if the behavior is between the allowed start and end date
                if (dailyBehaviors[i].Date >= startDate && dailyBehaviors[i].Date <= endDate)
                {
                    //if the behavior has not yet been added, this will add it to the dictionary object
                    //if the same type of behavior already exists it will go to the else and increment it
                    if (tags.ContainsKey(dailyBehaviors[i].Behavior))
                        tags[dailyBehaviors[i].Behavior] = tags[dailyBehaviors[i].Behavior] + 1;
                    else
                        tags.Add(dailyBehaviors[i].Behavior, 1);
                }
            }
            //The function to create the graphs, the behaviors are passed in the
            //dictionary object 'tags'
            createGraphs(tags);
        }

        /// <summary>
        /// This function is what actually creates the graphs. It clears the previous data in the graphs
        /// so that information does not overlap, and graphs either every behavior, the top 5 behaviors,
        /// or whichever ones you specifically select
        /// </summary>
        /// <param name="tags">The dictionary object with all the the behaviors in it, already filtered out
        /// depending on the date range selected</param>
        private void createGraphs(Dictionary<string, int> tags)
        {
            //sorts the dictionary by value
            tags = tags.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            //This controls whether the charts have their values shown as numbers or not
            chartPieDailyOccurences.Series[0].IsValueShownAsLabel = true;
            chartPyramidOccurences.Series[0].IsValueShownAsLabel = true;

            if (comboBehaviorsToGraph.SelectedIndex == 0)//Graph all
            {
                foreach (string tagname in tags.Keys)
                {
                    chartPieDailyOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);
                }
                foreach (string tagname in tags.Keys)
                {
                    chartPyramidOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);
                }
            }
            else if (comboBehaviorsToGraph.SelectedIndex == 1)//top 5
            {
                int count = 0;
                foreach (string tagname in tags.Keys)
                {
                    chartPyramidOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);
                    chartPieDailyOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);

                    if (++count == 5)
                        break;
                }
            }
            else if (comboBehaviorsToGraph.SelectedIndex == 2)//custom
            {
            }


            ///////////////////////For calculating the Trend Line
            //chartTotalBehaviors.Series["TrendLine"].ChartType = SeriesChartType.Line;
            //chartTotalBehaviors.Series["TrendLine"].BorderWidth = 2;
            //chartTotalBehaviors.Series["TrendLine"].Color = Color.Red;
            //// Line of best fit is linear
            //string typeRegression = "Linear";//"Exponential";//
            //// The number of days for Forecasting
            //string forecasting = "1";
            //// Show Error as a range chart.
            //string error = "false";
            //// Show Forecasting Error as a range chart.
            //string forecastingError = "false";
            //// Formula parameters
            //string parameters = typeRegression + ',' + forecasting + ',' + error + ',' + forecastingError;
            //chartTotalBehaviors.Series[0].Sort(PointSortOrder.Ascending, "X");
            //// Create Forecasting Series.
            //chartTotalBehaviors.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters, chartTotalBehaviors.Series[0], chartTotalBehaviors.Series["TrendLine"]);
            ///////////////////////////


        }

        private void radUseTimeFrames_Click(object sender, EventArgs e)
        {
            everythingDisabled();
            comboPickTimeGraphs.Enabled = true;
        }

        private void radUseCustomDates_Click(object sender, EventArgs e)
        {
            everythingDisabled();
            datePickerBeginGraphs.Enabled = true;
            datePickerEndGraphs.Enabled = true;
        }

        private void radUseCustomQuarters_Click(object sender, EventArgs e)
        {
            everythingDisabled();
            chkQuarter1.Enabled = true;
            chkQuarter2.Enabled = true;
            chkQuarter3.Enabled = true;
            chkQuarter4.Enabled = true;
        }

        /// <summary>
        /// Sets all the graph options to be disabled so that they don't need to be written out like that
        /// specifically every time a radio button selection changes
        /// </summary>
        private void everythingDisabled()
        {
            comboPickTimeGraphs.Enabled = false;
            chkQuarter1.Enabled = false;
            chkQuarter2.Enabled = false;
            chkQuarter3.Enabled = false;
            chkQuarter4.Enabled = false;
            datePickerBeginGraphs.Enabled = false;
            datePickerEndGraphs.Enabled = false;
        }

        private void btnGenerateGraphs_Click(object sender, EventArgs e)
        {
            //This clears all the previous data in the charts
            chartPieDailyOccurences.Series[0].Points.Clear();
            chartPyramidOccurences.Series[0].Points.Clear();

            getGraphRange();
        }

        private void comboBehaviorsToGraph_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBehaviorsToGraph.SelectedIndex == 0)
            {
                listBehaviorsToGraph.Enabled = false;
                listBehaviorsToGraph.ClearSelected();
            }
            if (comboBehaviorsToGraph.SelectedIndex == 1)
            {
                listBehaviorsToGraph.Enabled = false;
                listBehaviorsToGraph.ClearSelected();
            }
            if (comboBehaviorsToGraph.SelectedIndex == 2)
                listBehaviorsToGraph.Enabled = true;
        }
    }
}