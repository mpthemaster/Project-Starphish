using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormMain
    {
        private bool behaviorFirstTime = true;

        //This Stores all of the behaviors for the specified client in a List, it is a different 
        private List<TotalBehaviors> databaseBehaviors = new List<TotalBehaviors>();

        private void MainAddBehaviors()
        {
            if (behaviorFirstTime)
            {
                behaviorFirstTime = false;
                comboPickTimeDailyBehavior.SelectedIndex = 0;                  
            }
            RetrieveBehaviorsFromDatabase();
            SetDates();
            DisplayData();
        }

        private void RetrieveBehaviorsFromDatabase()
        {
            databaseBehaviors.Clear();

            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            //Get the information from the Staff Interview and display it.
            connection.Open();
            statement = "SELECT PERSON_ID, BEHAVIOR, SEVERITY, BEHAVIOR_DATE, BEHAVIOR_SHIFT, STAFF_NAME FROM BEHAVIOR";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if ((int)reader["PERSON_ID"] == personId)
                {
                    string behaviorName = (string)reader["BEHAVIOR"];
                    string behaviorSeverity = (string)reader["SEVERITY"];
                    DateTime behaviorDate = (DateTime)reader["BEHAVIOR_DATE"];
                    string behaviorShift = (string)reader["BEHAVIOR_SHIFT"];
                    string behaviorStaffName = (string)reader["STAFF_NAME"];

                    databaseBehaviors.Add(new TotalBehaviors(behaviorName, behaviorSeverity, behaviorDate, behaviorShift, behaviorStaffName));
                }
            }
            reader.Close();
            connection.Close();
        }

        /// <summary>
        /// Displays the data from the database in the grid view
        /// </summary>
        private void DisplayData()
        {
            dataGridViewDailyBehaviorTracking.Rows.Clear();


            for (int i = 0; i < databaseBehaviors.Count; i++)
            {
                if (databaseBehaviors[i].Date >= startDate && databaseBehaviors[i].Date <= endDate)
                {
                    this.dataGridViewDailyBehaviorTracking.Rows.Add(
                        databaseBehaviors[i].Date,
                        databaseBehaviors[i].Shift,
                        databaseBehaviors[i].Behavior,
                        databaseBehaviors[i].Severity,
                        "1",
                        databaseBehaviors[i].Staff);
                }
            }
        }

        private void SetDates()
        {
            if (chkUseCustomDatesDailyBehavior.Checked == false)
            {
                if (comboPickTimeDailyBehavior.SelectedIndex == 0)//last 30 days
                {
                    startDate = DateTime.Today.AddDays(-30);
                    endDate = DateTime.Today;
                }
                else if (comboPickTimeDailyBehavior.SelectedIndex == 1)//last 60
                {
                    startDate = DateTime.Today.AddDays(-60);
                    endDate = DateTime.Today;
                }
                else if (comboPickTimeDailyBehavior.SelectedIndex == 2)//Current Quarter
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
            else
            {
                startDate = new DateTime(datePickerBeginDailyBehavior.Value.Year, datePickerBeginDailyBehavior.Value.Month, 1);
                endDate = new DateTime(datePickerEndDailyBehavior.Value.Year, datePickerEndDailyBehavior.Value.Month, DateTime.DaysInMonth(datePickerEndDailyBehavior.Value.Year, datePickerEndDailyBehavior.Value.Month));
            }
        }

        private void comboPickTimeDailyBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDates();
            DisplayData();
        }

        private void chkUseCustomDatesDailyBehavior_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseCustomDatesDailyBehavior.Checked == true)
            {
                datePickerBeginDailyBehavior.Enabled = true;
                datePickerEndDailyBehavior.Enabled = true;
            }
            else
            {
                datePickerBeginDailyBehavior.Enabled = false;
                datePickerEndDailyBehavior.Enabled = false;
            }

            SetDates();
            DisplayData();
        }

        private void datePickerEndDailyBehavior_ValueChanged(object sender, EventArgs e)
        {
            SetDates();
            DisplayData();
        }

        private void datePickerBeginDailyBehavior_ValueChanged(object sender, EventArgs e)
        {
            SetDates();
            DisplayData();
        }

        private void btnAddBehavior_Click(object sender, EventArgs e)
        {
            dataGridViewDailyBehaviorTracking.ClearSelection();
        }  
    }
}
