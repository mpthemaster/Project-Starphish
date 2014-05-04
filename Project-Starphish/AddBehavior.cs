using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private void MainAddBehaviors()
        {
            if (behaviorFirstTime)
            {
                behaviorFirstTime = false;
                comboPickTimeDailyBehavior.SelectedIndex = 0;
                //sorts the grid view by the date by default
                dataGridViewDailyBehaviorTracking.Sort(dataGridViewDailyBehaviorTracking.Columns[0], ListSortDirection.Ascending);
            }
            SetDates();
            DisplayData();
        }

        /// <summary>
        /// Displays the data from the database in the grid view
        /// </summary>
        private void DisplayData()
        {
            try
            {
                //Filling in the grid view with the database table. 
                //We also added the table in as a datasource with the little triangle in the upper right corner of the grid view
                //Thats where we change the names to be not exactly what the database names were, and make it so the ID does not display
                //The ID could not be first because apparently the first thing in the table must display, even if you set it not to. 
                this.bEHAVIORTableAdapter.FillBy(this.projectStarphishDataSet.BEHAVIOR, personId, startDate.ToString(), endDate.ToString());
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void dataGridViewDailyBehaviorTracking_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //Get what cell has an error.
            int errorRow = e.RowIndex + 1;
            string errorColumn = (sender as DataGridView).Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText;

            //If the error is because no data is entered, display that to the user.
            //Else if the error is an incorrect format (e.g. string for int), display that to the user and clear the new input for the original.
            //Else if the exception is due to duplicating information, left the user know.
            if (e.Exception is NoNullAllowedException)
                MessageBox.Show("The cell under the " + errorColumn + " Column and on Row #" + errorRow + " must not be empty.");
            else if (e.Exception is FormatException)
            {
                if (e.ColumnIndex == 0)
                    MessageBox.Show("The cell under the " + errorColumn + " Column and Row #" + errorRow + " must be correctly formatted as a date (e.g. 11/23/14).");
                else
                    MessageBox.Show("The cell under the " + errorColumn + " Column and Row #" + errorRow + " must be an integer.");
                (sender as DataGridView).CancelEdit();
            }
            else if (e.Exception is ConstraintException)
                MessageBox.Show("The Row #" + errorRow + " must have a unique date, shift time, or behavior name that is different from a currently existing row.");
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

                if (startDate > endDate)
                {
                    MessageBox.Show("Error: The start date must be before the end date.");
                }
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
                datePickerBeginDailyBehavior.Visible = true;
                datePickerEndDailyBehavior.Visible = true;
                lblStartDateBeh.Visible = true;
                lblEndDateBeh.Visible = true;
                lblTo.Visible = true;
            }
            else
            {
                datePickerBeginDailyBehavior.Visible = false;
                datePickerEndDailyBehavior.Visible = false;
                lblStartDateBeh.Visible = false;
                lblEndDateBeh.Visible = false;
                lblTo.Visible = false;
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

        private void btnSaveDailyBehavior_Click(object sender, EventArgs e)
        {
            this.Validate();

            this.tableAdapterManager.UpdateAll(this.projectStarphishDataSet);

            try
            {
                this.Validate();
                this.bEHAVIORBindingSource.EndEdit();
                this.bEHAVIORTableAdapter.Update(this.projectStarphishDataSet.BEHAVIOR);
                lblDatabaseUpdated.Visible = true;
                timer1.Enabled = true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Update failed" + ex.ToString());
            }

            SetDates();
            DisplayData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDatabaseUpdated.Visible = false;
            timer1.Enabled = false;
        }

        private void dataGridViewDailyBehaviorTracking_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["pERSONIDDataGridViewTextBoxColumn"].Value = personId;
        }
    }
}