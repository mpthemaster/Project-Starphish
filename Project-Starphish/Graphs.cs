using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private List<TotalBehavior> totalBehaviors2 = new List<TotalBehavior>();

        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();

        //a dictionary object to hold the relevant behaviors 
        Dictionary<string, int> tags = new Dictionary<string, int>();

        Dictionary<DateTime, int> totalBehaviors = new Dictionary<DateTime, int>();


        private void retrieveDailyBehavior()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            //Get the information from the Staff Interview and display it.
            connection.Open();
            statement = "SELECT PERSON_ID, BEHAVIOR, BEHAVIOR_DATE, BEHAVIOR_SHIFT FROM BEHAVIOR";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if ((int)reader["PERSON_ID"] == personId)
                {
                    string behaviorName = (string)reader["BEHAVIOR"];
                    DateTime behaviorDate = (DateTime)reader["BEHAVIOR_DATE"];
                    string behaviorShift = (string)reader["BEHAVIOR_SHIFT"];

                    dailyBehaviors.Add(new DailyBehavior(behaviorName, "Severe", behaviorDate, behaviorShift));
                }
            }
            reader.Close();
            connection.Close();

            foreach (DailyBehavior dailyBehavior in dailyBehaviors)
            {
                // MessageBox.Show(dailyBehavior.Behavior + dailyBehavior.Date.ToShortDateString() + dailyBehavior.Shift);
                // listBehaviorsToGraph.Items.Contains
            }
        }

        /// <summary>
        /// The function that starts the graphing tab
        /// </summary>
        private void mainGraph()
        {
            retrieveDailyBehavior();

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

                for (int i = 0; i < dailyBehaviors.Count; i++)
                {
                    //if the behavior is not already in the list box, add it
                    if (!listBehaviorsToGraph.Items.Contains(dailyBehaviors[i].Behavior))
                        listBehaviorsToGraph.Items.Add(dailyBehaviors[i].Behavior);
                }

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

                    getBehaviors(startDate, endDate);
                }
                else if (comboPickTimeGraphs.SelectedIndex == 1)//last 60 days
                {
                    startDate = DateTime.Today.AddDays(-60);
                    endDate = DateTime.Today;

                    getBehaviors(startDate, endDate);
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

                    getBehaviors(startDate, endDate);
                }
            }
            else if (radUseCustomDates.Checked)
            {
                startDate = new DateTime(datePickerBeginGraphs.Value.Year, datePickerBeginGraphs.Value.Month, 1);
                endDate = new DateTime(datePickerEndGraphs.Value.Year, datePickerEndGraphs.Value.Month, System.DateTime.DaysInMonth(datePickerEndGraphs.Value.Year, datePickerEndGraphs.Value.Month));
                getBehaviors(startDate, endDate);
            }
            else if (radUseCustomQuarters.Checked)
            {
                int quarter = 0;

                //The companies quarters are:
                //Quarter 1: July to September
                //Quarter 2: October - December
                //Quarter 3: January - March
                //Quarter 4: April - June

                //Checks to see if none of the checkboxes are checked
                if (!chkQuarter1.Checked && !chkQuarter2.Checked && !chkQuarter3.Checked && !chkQuarter4.Checked)
                    MessageBox.Show("Error: No Quarter was Selected");

                //July August September
                if (DateTime.Today.Month == 07 || DateTime.Today.Month == 08 || DateTime.Today.Month == 09)
                {
                    quarter = 1;
                }

                //October November December
                else if (DateTime.Today.Month == 10 || DateTime.Today.Month == 11 || DateTime.Today.Month == 12)
                {
                    quarter = 2;
                }

                //January February or March
                else if (DateTime.Today.Month == 01 || DateTime.Today.Month == 02 || DateTime.Today.Month == 03)
                {
                    quarter = 3;
                }

                //April May June
                else if (DateTime.Today.Month == 04 || DateTime.Today.Month == 05 || DateTime.Today.Month == 06)
                {
                    quarter = 4;
                }


                if (chkQuarter1.Checked)
                {//July August September
                    if (quarter == 1)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 07, 01);
                        endDate = DateTime.Today;
                    }
                    else
                    {
                        startDate = new DateTime(DateTime.Today.Year - 1, 07, 01);
                        endDate = new DateTime(DateTime.Today.Year - 1, 09, DateTime.DaysInMonth(DateTime.Today.Year - 1, 09));
                    }
                    getBehaviors(startDate, endDate);
                }
                if (chkQuarter2.Checked)
                {
                    if (quarter == 2)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 10, 01);
                        endDate = DateTime.Today;
                    }
                    else
                    {
                        startDate = new DateTime(DateTime.Today.Year - 1, 10, 01);
                        endDate = new DateTime(DateTime.Today.Year - 1, 12, DateTime.DaysInMonth(DateTime.Today.Year - 1, 12));
                    }
                    getBehaviors(startDate, endDate);
                }
                if (chkQuarter3.Checked)
                {
                    if (quarter == 3)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 01, 01);
                        endDate = DateTime.Today;
                    }
                    else
                    {
                        startDate = new DateTime(DateTime.Today.Year - 1, 1, 01);
                        endDate = new DateTime(DateTime.Today.Year - 1, 3, DateTime.DaysInMonth(DateTime.Today.Year - 1, 3));
                    }
                    getBehaviors(startDate, endDate);
                }
                if (chkQuarter4.Checked)
                {
                    if (quarter == 4)
                    {
                        startDate = new DateTime(DateTime.Today.Year, 04, 01);
                        endDate = DateTime.Today;
                    }
                    else
                    {
                        startDate = new DateTime(DateTime.Today.Year - 1, 4, 01);
                        endDate = new DateTime(DateTime.Today.Year - 1, 6, DateTime.DaysInMonth(DateTime.Today.Year - 1, 6));
                    }
                    getBehaviors(startDate, endDate);
                }
            }

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
            DateTime currentDate = startDate;

            //Adds every single date in the range to the totalBehaviors object
            while (currentDate.CompareTo(endDate) == -1)
            {
                totalBehaviors.Add(currentDate, 0);
                currentDate = currentDate.AddDays(1);
            }

            for (int i = 0; i < dailyBehaviors.Count(); i++)//for every behavior 
            {
                //this checks to see if the behavior is between the allowed start and end date
                if ((dailyBehaviors[i].Date >= startDate) && (dailyBehaviors[i].Date <= endDate))
                {
                    //if the same type of behavior already exists it will go to the else and increment it
                    if (tags.ContainsKey(dailyBehaviors[i].Behavior))
                        tags[dailyBehaviors[i].Behavior] = tags[dailyBehaviors[i].Behavior] + 1;
                    else
                    {
                        //if the behavior has not yet been added, this will add it to the dictionary object
                        tags.Add(dailyBehaviors[i].Behavior, 1);
                    }
                    if (comboBehaviorsToGraph.SelectedIndex == 0)//all behaviors
                        totalBehaviors[dailyBehaviors[i].Date] = totalBehaviors[dailyBehaviors[i].Date] + 1;

                }
            }

            if (comboBehaviorsToGraph.SelectedIndex == 1)//top 5 behaviors
            {
                for (int i = 0; i < dailyBehaviors.Count(); i++)//for every behavior 
                {
                    //this checks to see if the behavior is between the allowed start and end date
                    if ((dailyBehaviors[i].Date >= startDate) && (dailyBehaviors[i].Date <= endDate))
                    {
                    }
                }
            }
        }

        /// <summary>
        /// This function is what actually creates the graphs. It clears the previous data in the graphs
        /// so that information does not overlap, and graphs either every behavior, the top 5 behaviors,
        /// or whichever ones you specifically select
        /// </summary>
        private void createGraphs()
        {
            //This controls whether the charts have their values shown as numbers or not
            chartPieDailyOccurences.Series[0].IsValueShownAsLabel = true;
            chartPyramidOccurences.Series[0].IsValueShownAsLabel = true;

            if (comboBehaviorsToGraph.SelectedIndex == 0)//Graph all
            {
                foreach (DateTime date in totalBehaviors.Keys)
                {
                    chartTotalBehaviors.Series[0].Points.AddXY(date.Month + "/" + date.Day, totalBehaviors[date]);
                }

                foreach (string tagname in tags.Keys)
                {
                    chartPieDailyOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);
                    chartPyramidOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);
                }
            }
            else if (comboBehaviorsToGraph.SelectedIndex == 1)//top 5
            {
                int count = 0;
                tags = tags.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
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
                foreach (string tagname in tags.Keys)
                {
                    //for every selected item in the list box
                    for (int i = 0; i < listBehaviorsToGraph.SelectedItems.Count; i++)
                    {
                        //If the selected item is the same as the behavior about to be printed
                        if (listBehaviorsToGraph.SelectedItems[i].ToString() == tagname)
                        {
                            //Actually print it
                            chartPyramidOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);
                            chartPieDailyOccurences.Series[0].Points.AddXY(tagname, tags[tagname]);
                        }
                    }
                }
            }
            chartPyramidOccurences.Series[0].Sort(PointSortOrder.Ascending);
            chartPieDailyOccurences.Series[0].Sort(PointSortOrder.Ascending);

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
            chartTotalBehaviors.Series[0].Points.Clear();
            tags.Clear();
            totalBehaviors.Clear();

            getGraphRange();

            //The function to create the graphs, the behaviors are passed in the
            //dictionary object 'tags'
            createGraphs();
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