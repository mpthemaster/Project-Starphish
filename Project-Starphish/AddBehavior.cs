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
   
        private void MainAddBehaviors()
        {
            if (behaviorFirstTime)
            {
                behaviorFirstTime = false;
                comboPickTimeDailyBehavior.SelectedIndex = 0;                  
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
                this.bEHAVIORTableAdapter.FillBy(this.projectStarphishDataSet.BEHAVIOR, personId, startDate.ToString(), endDate.ToString());
                
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
//            dataGridViewDailyBehaviorTracking.DataSource = bEHAVIORBindingSource;
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

        private void btnSaveDailyBehavior_Click(object sender, EventArgs e)
        {
            this.Validate();
            
            this.tableAdapterManager.UpdateAll(this.projectStarphishDataSet);

            try
            {
                this.Validate();
                this.bEHAVIORBindingSource.EndEdit();
                this.bEHAVIORTableAdapter.Update(this.projectStarphishDataSet.BEHAVIOR);
                MessageBox.Show("Update successful");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Update failed" + ex.ToString());
            }

            SetDates();
            DisplayData();
        }

        private void dataGridViewDailyBehaviorTracking_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["pERSONIDDataGridViewTextBoxColumn"].Value = personId;
        }

    }
}
