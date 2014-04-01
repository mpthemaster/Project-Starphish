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
        private List<BehaviorsOnSpecifiedDate> behaviorsOnSpecifiedDate = new List<BehaviorsOnSpecifiedDate>();
        private List<BehaviorsOccured> timesBehaviorsOccured = new List<BehaviorsOccured>();

        DateTime startDate = new DateTime();
        DateTime endDate = new DateTime();


        private void retrieveDailyBehavior()
        {
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

                    FillDateList();
                    getBehaviors(startDate, endDate);
                }
                else if (comboPickTimeGraphs.SelectedIndex == 1)//last 60 days
                {
                    startDate = DateTime.Today.AddDays(-60);
                    endDate = DateTime.Today;

                    FillDateList();
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

                    FillDateList();
                    getBehaviors(startDate, endDate);
                }
            }
            else if (radUseCustomDates.Checked)
            {
                startDate = new DateTime(datePickerBeginGraphs.Value.Year, datePickerBeginGraphs.Value.Month, 1);
                endDate = new DateTime(datePickerEndGraphs.Value.Year, datePickerEndGraphs.Value.Month, DateTime.DaysInMonth(datePickerEndGraphs.Value.Year, datePickerEndGraphs.Value.Month));

                FillDateList();
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
                    getBehaviors(startDate, endDate);
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
                    getBehaviors(startDate, endDate);
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
                    getBehaviors(startDate, endDate);
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
            //sorts the array of dates and occurences on that day by the date
            List<BehaviorsOnSpecifiedDate> behaviorsOnSpecifedDate = behaviorsOnSpecifiedDate.OrderBy(o => o.Date).ToList();

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
            //Sorting the list of behaviors that occured
            timesBehaviorsOccured = timesBehaviorsOccured.OrderByDescending(o => o.Occurences).ToList();

            //for (int i = 0; i < timesBehaviorsOccured.Count(); i++)
            //{
            //    MessageBox.Show(timesBehaviorsOccured[i].Behavior + timesBehaviorsOccured[i].Occurences);
            //}



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
            
            for(int i = 0; i < behaviorsOnSpecifiedDate.Count(); i++)
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
                for(int i = 0; i < timesBehaviorsOccured.Count; i++)
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
            chartPyramidOccurences.Series[0].Sort(PointSortOrder.Ascending);
            chartPieDailyOccurences.Series[0].Sort(PointSortOrder.Ascending);

            if (behaviorsOnSpecifiedDate.Count > 1)
            {
                /////////////////////For calculating the Trend Line
                chartTotalBehaviors.Series["TrendLine"].ChartType = SeriesChartType.Line;
                chartTotalBehaviors.Series["TrendLine"].BorderWidth = 2;
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

            timesBehaviorsOccured.Clear();
            behaviorsOnSpecifiedDate.Clear();

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