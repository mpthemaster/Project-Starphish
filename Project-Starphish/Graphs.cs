using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public partial class FormMain
    {
        private bool graphFirstTime = true;

        //This list holds all of the actual behaviors that occured
        private List<DailyBehavior> dailyBehaviors = new List<DailyBehavior>();

        //This list holds how many behaviors occured on a specific date
        private List<BehaviorsOnSpecifiedDate> behaviorsOnSpecifiedDate = new List<BehaviorsOnSpecifiedDate>();

        //This tallys up how many times each behavior that is listed occured, which is
        //used for detemining the top 5 behaviors
        private List<BehaviorsOccured> timesBehaviorsOccured = new List<BehaviorsOccured>();

        //These are used to denote the beginning and end dates of what behaviors to graph
        private DateTime startDate = new DateTime();

        private DateTime endDate = new DateTime();

        /// <summary>
        /// This function accesses the database and puts all of the behaviors into the
        /// dailyBehaviors list
        /// </summary>
        private void retrieveDailyBehavior()
        {
            //Clears the previous list of dailyBehaviors now that a new one will be gotten
            dailyBehaviors.Clear();

            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            //Get the information from the Staff Interview and display it.
            connection.Open();
            statement = "SELECT PERSON_ID, BEHAVIOR, BEHAVIOR_DATE FROM BEHAVIOR";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if ((int)reader["PERSON_ID"] == personId)
                {
                    string behaviorName = (string)reader["BEHAVIOR"];
                    DateTime behaviorDate = (DateTime)reader["BEHAVIOR_DATE"];

                    dailyBehaviors.Add(new DailyBehavior(behaviorName, behaviorDate));
                }
            }
            reader.Close();
            connection.Close();
        }

        /// <summary>
        /// This is the function that is called whenever the graphing tab is opened
        /// </summary>
        private void mainGraph()
        {
            //Getting the behaviors from the database
            retrieveDailyBehavior();

            //adding all of the behaviors to the list box
            for (int i = 0; i < dailyBehaviors.Count; i++)
            {
                //if the behavior is not already in the list box, add it
                if (!listBehaviorsToGraph.Items.Contains(dailyBehaviors[i].Behavior))
                    listBehaviorsToGraph.Items.Add(dailyBehaviors[i].Behavior);
            }

            if (graphFirstTime)
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

                graphFirstTime = false;
            }
        }

        private void btnGenerateGraphs_Click(object sender, EventArgs e)
        {
            //This function gets the time constaints for the behaviors
            getGraphRange();
        }

        /// <summary>
        /// Checks to see the range of dates the graph should be between
        /// </summary>
        private void getGraphRange()
        {
            #region"Time Frames"
            if (radUseTimeFrames.Checked)
            {
                if (comboPickTimeGraphs.SelectedIndex == 0)//last 30 days
                {
                    startDate = DateTime.Today.AddDays(-30);
                    endDate = DateTime.Today;

                    clear();
                    FillDateList();
                    getBehaviors();
                    createGraphs();
                }
                else if (comboPickTimeGraphs.SelectedIndex == 1)//last 60 days
                {
                    startDate = DateTime.Today.AddDays(-60);
                    endDate = DateTime.Today;

                    clear();
                    FillDateList();
                    getBehaviors();
                    createGraphs();
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
                    clear();
                    FillDateList();
                    getBehaviors();
                    createGraphs();
                }
            }
            #endregion
            #region"Custom Dates"
            else if (radUseCustomDates.Checked)
            {
                startDate = new DateTime(datePickerBeginGraphs.Value.Year, datePickerBeginGraphs.Value.Month, 1);
                endDate = new DateTime(datePickerEndGraphs.Value.Year, datePickerEndGraphs.Value.Month, DateTime.DaysInMonth(datePickerEndGraphs.Value.Year, datePickerEndGraphs.Value.Month));

                //This is to prevent a start date that is after the end date
                if (startDate > endDate)
                {
                    MessageBox.Show("Error: The start date must be before the end date");
                }
                else
                {
                    clear();
                    FillDateList();
                    getBehaviors();
                    createGraphs();
                }
            }
            #endregion
            #region"Custom Quarters"
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
                else
                {
                    clear();

                    //Based on the current quarter, this array holds the start dates for the current quarters and
                    //the previous quarters
                    DateTime[] dateTime = new DateTime[4];

                    //July August September
                    if (DateTime.Today.Month == 07 || DateTime.Today.Month == 08 || DateTime.Today.Month == 09)
                    {
                        quarter = 1;

                        dateTime[0] = new DateTime(DateTime.Today.Year, 7, 1);
                        dateTime[1] = new DateTime(DateTime.Today.Year - 1, 10, 1);
                        dateTime[2] = new DateTime(DateTime.Today.Year, 1, 1);
                        dateTime[3] = new DateTime(DateTime.Today.Year, 4, 1);
                    }

                    //October November December
                    else if (DateTime.Today.Month == 10 || DateTime.Today.Month == 11 || DateTime.Today.Month == 12)
                    {
                        quarter = 2;

                        dateTime[0] = new DateTime(DateTime.Today.Year, 7, 1);
                        dateTime[1] = new DateTime(DateTime.Today.Year, 10, 1);
                        dateTime[2] = new DateTime(DateTime.Today.Year, 1, 1);
                        dateTime[3] = new DateTime(DateTime.Today.Year, 4, 1);
                    }

                    //January February or March
                    else if (DateTime.Today.Month == 01 || DateTime.Today.Month == 02 || DateTime.Today.Month == 03)
                    {
                        quarter = 3;

                        dateTime[0] = new DateTime(DateTime.Today.Year - 1, 7, 1);
                        dateTime[1] = new DateTime(DateTime.Today.Year - 1, 10, 1);
                        dateTime[2] = new DateTime(DateTime.Today.Year, 1, 1);
                        dateTime[3] = new DateTime(DateTime.Today.Year - 1, 4, 1);
                    }

                    //April May June
                    else if (DateTime.Today.Month == 04 || DateTime.Today.Month == 05 || DateTime.Today.Month == 06)
                    {
                        quarter = 4;

                        dateTime[0] = new DateTime(DateTime.Today.Year - 1, 7, 1);
                        dateTime[1] = new DateTime(DateTime.Today.Year - 1, 10, 1);
                        dateTime[2] = new DateTime(DateTime.Today.Year, 1, 1);
                        dateTime[3] = new DateTime(DateTime.Today.Year, 4, 1);
                    }

                    if (chkQuarter1.Checked)
                    {//July August September
                        startDate = dateTime[0];
                        if (quarter == 1)
                        {
                            endDate = DateTime.Today;
                        }
                        else
                        {
                            endDate = new DateTime(startDate.Year, startDate.Month + 2, DateTime.DaysInMonth(startDate.Year, startDate.Month + 2));
                        }
                        FillDateList();
                        getBehaviors();
                    }
                    if (chkQuarter2.Checked)
                    {
                        startDate = dateTime[1];
                        if (quarter == 2)
                        {
                            endDate = DateTime.Today;
                        }
                        else
                        {
                            endDate = new DateTime(startDate.Year, startDate.Month + 2, DateTime.DaysInMonth(startDate.Year, startDate.Month + 2));
                        }
                        FillDateList();
                        getBehaviors();
                    }
                    if (chkQuarter3.Checked)
                    {
                        startDate = dateTime[2];
                        if (quarter == 3)
                        {
                            endDate = DateTime.Today;
                        }
                        else
                        {
                            endDate = new DateTime(startDate.Year, startDate.Month + 2, DateTime.DaysInMonth(startDate.Year, startDate.Month + 2));
                        }
                        FillDateList();
                        getBehaviors();
                    }
                    if (chkQuarter4.Checked)
                    {
                        startDate = dateTime[3];
                        if (quarter == 4)
                        {
                            endDate = DateTime.Today;
                        }
                        else
                        {
                            endDate = new DateTime(startDate.Year, startDate.Month + 2, DateTime.DaysInMonth(startDate.Year, startDate.Month + 2));
                        }
                        FillDateList();
                        getBehaviors();
                    }
                    createGraphs();
                }
            }
            #endregion
        }

        /// <summary>
        /// This gets all of the behaviors from the dailyBehaviors list and calculates how many
        /// behaviors occured on each date in the selected range and puts them in the behaviorsOnSpecificDate
        /// list, as well as calculates how many times each behavior occured, and puts it into the timesBehaviorsOccured
        /// list
        /// </summary>
        private void getBehaviors()
        {
            //sorts the array of dates and occurences on that day by the date
            List<BehaviorsOnSpecifiedDate> behaviorsOnSpecifedDate = behaviorsOnSpecifiedDate.OrderBy(o => o.Date).ToList();

            #region"Populate timesBehaviorsOccured for right graphs"
            ////////////This is to populate timesBehaviorOccured, which tracks how many times each behavior that occured occured
            for (int i = 0; i < dailyBehaviors.Count(); i++)//for every behavior
            {
                //this checks to see if the behavior is between the allowed start and end date
                if (dailyBehaviors[i].Date >= startDate && dailyBehaviors[i].Date <= endDate)
                {
                    bool behaviorPresent = false;

                    //populate the list of how many times each behavior occured
                    for (int x = 0; x < timesBehaviorsOccured.Count(); x++)
                    {
                        //If the behavior is already in there, increment it by 1
                        //and mark behaviorPresent as true so that it doesn't add the behavior in later
                        if (timesBehaviorsOccured[x].Behavior == dailyBehaviors[i].Behavior)
                        {
                            timesBehaviorsOccured[x].Occurences++;
                            behaviorPresent = true;
                            break;
                        }
                    }

                    //If the behavior that occured was not already in the list of behaviors with how many times they
                    //occured, add it
                    if (behaviorPresent == false)
                    {
                        timesBehaviorsOccured.Add(new BehaviorsOccured(dailyBehaviors[i].Behavior));
                    }
                }
            }
            ////////////
            #endregion

            //Sorting the list of behaviors that occured so that it is in order of most to least,
            //which is necessary for picking out the top 5 behaviors
            timesBehaviorsOccured = timesBehaviorsOccured.OrderByDescending(o => o.Occurences).ToList();

            #region"Populate behaviorsOnSpecificDate for left graph"
            ///////////Populates behaviorsOnSpecificDate, which keeps track of how many behaviors the
            //consumer has had on each day
            for (int i = 0; i < dailyBehaviors.Count(); i++)//for every behavior
            {
                //this checks to see if the behavior is between the allowed start and end date
                if (dailyBehaviors[i].Date >= startDate && dailyBehaviors[i].Date <= endDate)
                {
                    //go through the list of dates and increment the number of behaviors occured on a specific date
                    for (int x = 0; x < behaviorsOnSpecifedDate.Count(); x++)
                    {
                        //When it finds the date that matches up with the behavior
                        if (behaviorsOnSpecifedDate[x].Date == dailyBehaviors[i].Date)
                        {
                            if (comboBehaviorsToGraph.SelectedIndex == 0)//graph all
                            {
                                behaviorsOnSpecifedDate[x].Occurences++;
                            }

                            else if (comboBehaviorsToGraph.SelectedIndex == 1)//top 5
                            {
                                int totalTop = 5;

                                if (timesBehaviorsOccured.Count < 5)
                                    totalTop = timesBehaviorsOccured.Count;

                                //only add it if it is one of the top 5 behaviors
                                for (int m = 0; m < totalTop; m++)
                                {
                                    //if the behavior is on the top 5
                                    if (dailyBehaviors[i].Behavior == timesBehaviorsOccured[m].Behavior)
                                    {
                                        behaviorsOnSpecifedDate[x].Occurences++;
                                    }
                                }
                            }

                            else if (comboBehaviorsToGraph.SelectedIndex == 2)//custom
                            {
                                for (int m = 0; m < listBehaviorsToGraph.SelectedItems.Count; m++)
                                {
                                    if (listBehaviorsToGraph.SelectedItems[m].ToString() == dailyBehaviors[i].Behavior)
                                    {
                                        behaviorsOnSpecifedDate[x].Occurences++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ////////////////
            #endregion
        }

        /// <summary>
        /// This function is what actually creates the graphs. It just makes the totalBehaviors graph on the left
        /// without any checking because that information is already formatted, and graphs the two
        /// on the right depending on which behaviors were selected to be graphed
        /// </summary>
        private void createGraphs()
        {
            //This controls whether the charts have their values shown as numbers or not
            chartPieDailyOccurences.Series[0].IsValueShownAsLabel = true;
            chartPyramidOccurences.Series[0].IsValueShownAsLabel = true;

            //changes the number of values displayed at the bottom of the left graph depending
            //on how many data points there are
            if (behaviorsOnSpecifiedDate.Count >= 600)
                chartTotalBehaviors.ChartAreas[0].AxisX.Interval = 60;
            else if (behaviorsOnSpecifiedDate.Count >= 250)
                chartTotalBehaviors.ChartAreas[0].AxisX.Interval = 30;
            else if (behaviorsOnSpecifiedDate.Count >= 85)
                chartTotalBehaviors.ChartAreas[0].AxisX.Interval = 10;
            else if (behaviorsOnSpecifiedDate.Count >= 55)
                chartTotalBehaviors.ChartAreas[0].AxisX.Interval = 4;
            else if (behaviorsOnSpecifiedDate.Count >= 15)
                chartTotalBehaviors.ChartAreas[0].AxisX.Interval = 2;
            else
                chartTotalBehaviors.ChartAreas[0].AxisX.Interval = 1;

            chartTotalBehaviors.Series[0].Color = Color.Black;
            chartTotalBehaviors.Series[0].BorderWidth = 2;

            //Creates the left graph, doesn't need any if statements because the list is already
            //formatted correctly
            for (int i = 0; i < behaviorsOnSpecifiedDate.Count(); i++)
            {
                chartTotalBehaviors.Series[0].Points.AddXY(
                    behaviorsOnSpecifiedDate[i].Date.Month +
                    "/" +
                    behaviorsOnSpecifiedDate[i].Date.Day,
                    behaviorsOnSpecifiedDate[i].Occurences);
            }

            if (comboBehaviorsToGraph.SelectedIndex == 0)//Graph all
            {
                for (int i = 0; i < timesBehaviorsOccured.Count; i++)
                {
                    chartPyramidOccurences.Series[0].Points.AddXY(
                        timesBehaviorsOccured[i].Behavior, timesBehaviorsOccured[i].Occurences);
                    chartPieDailyOccurences.Series[0].Points.AddXY(
                        timesBehaviorsOccured[i].Behavior, timesBehaviorsOccured[i].Occurences);
                }
            }
            else if (comboBehaviorsToGraph.SelectedIndex == 1)//top 5
            {
                int totalTop = 5;

                if (timesBehaviorsOccured.Count < 5)
                    totalTop = timesBehaviorsOccured.Count;

                //only add it if it is one of the top 5 behaviors
                for (int i = 0; i < totalTop; i++)
                {
                    chartPyramidOccurences.Series[0].Points.AddXY(
                        timesBehaviorsOccured[i].Behavior, timesBehaviorsOccured[i].Occurences);
                    chartPieDailyOccurences.Series[0].Points.AddXY(
                        timesBehaviorsOccured[i].Behavior, timesBehaviorsOccured[i].Occurences);
                }
            }
            else if (comboBehaviorsToGraph.SelectedIndex == 2)//custom
            {
                for (int i = 0; i < timesBehaviorsOccured.Count; i++)
                {
                    for (int x = 0; x < listBehaviorsToGraph.SelectedItems.Count; x++)
                    {
                        if (listBehaviorsToGraph.SelectedItems[x].ToString() == timesBehaviorsOccured[i].Behavior)
                        {
                            chartPyramidOccurences.Series[0].Points.AddXY(
                                timesBehaviorsOccured[i].Behavior, timesBehaviorsOccured[i].Occurences);
                            chartPieDailyOccurences.Series[0].Points.AddXY(
                                timesBehaviorsOccured[i].Behavior, timesBehaviorsOccured[i].Occurences);
                        }
                    }
                }
            }

            //this sorts the two charts on the right so that the largest values are at the top
            chartPyramidOccurences.Series[0].Sort(PointSortOrder.Ascending);
            chartPieDailyOccurences.Series[0].Sort(PointSortOrder.Ascending);

            //Don't do this is there is 1 or 0 behaviors in the list, the program will crash because
            //It cant create an average line with only 1 or 0 behaviors
            if (behaviorsOnSpecifiedDate.Count > 1)
            {
                /////////////////////For calculating the Trend Line
                chartTotalBehaviors.Series["TrendLine"].ChartType = SeriesChartType.Line;
                chartTotalBehaviors.Series["TrendLine"].BorderWidth = 5;
                chartTotalBehaviors.Series["TrendLine"].Color = Color.Red;
                // Line of best fit is linear
                string typeRegression = "Linear";//"Exponential";//
                // The number of days for Forecasting
                string forecasting = "1";
                // Show Error as a range chart.
                string error = "false";
                // Show Forecasting Error as a range chart.
                string forecastingError = "false";
                // Formula parameters
                string parameters = typeRegression + ',' + forecasting + ',' + error + ',' + forecastingError;
                // Create Forecasting Series.
                chartTotalBehaviors.DataManipulator.FinancialFormula(FinancialFormula.Forecasting, parameters, chartTotalBehaviors.Series[0], chartTotalBehaviors.Series["TrendLine"]);
                /////////////////////////              
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // chart1.SaveImage("dfgdfg.png", ChartImageFormat.Png);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Chart copyitem = sender as Chart;

           // MessageBox.Show(copyitem.ToString());
            using (MemoryStream ms = new MemoryStream())
            {
                copyitem.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
            }
            
        }

        /// <summary>
        /// This adds every date in the chosen range to the behaviorsOnSpecifiedDate list with a default
        /// value of 0, it is in its own seperate function because of the checkboxes
        /// </summary>
        private void FillDateList()
        {
            DateTime currentDate = startDate;

            //Adds every single date in the range to the totalBehaviors object
            while (currentDate.CompareTo(endDate) <= 0)
            {
                behaviorsOnSpecifiedDate.Add(new BehaviorsOnSpecifiedDate(currentDate));
                currentDate = currentDate.AddDays(1);
            }
        }

        /// <summary>
        /// Clears out all of the data so that information doesn't overlap when a new graph is made
        /// </summary>
        private void clear()
        {
            //This clears all the previous data in the charts
            chartPieDailyOccurences.Series[0].Points.Clear();
            chartPyramidOccurences.Series[0].Points.Clear();
            chartTotalBehaviors.Series[0].Points.Clear();

            //This clears all of the Lists of objects except for the list of behaviors that occured,
            //that will be cleared whenever a new list is gotten
            timesBehaviorsOccured.Clear();
            behaviorsOnSpecifiedDate.Clear();
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

        #region"Radio buttons"
        /// <summary>
        /// Disables the controls depending on which radio button is pressed
        /// </summary>
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
    }
        #endregion
}