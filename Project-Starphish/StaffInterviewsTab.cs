//Note: When implementing the display system, just have the view and remove buttons disabled when multiple listbox items are selected.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    partial class FormMain
    {
        /// <summary>
        /// Loads up the pre-existing staff interviews to view.
        /// </summary>
        public void mainStaffInterview()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            connection.Open();
            statement = "SELECT PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED FROM STAFF_INTERVIEW";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            lstInterviews.Items.Clear();

            //Get all the staff interviews and display them.
            while (reader.Read())
                if ((int)reader["PERSON_ID"] == personId)
                {
                    DateTime interviewDate = (DateTime)reader["INTERVIEW_DATE"];
                    lstInterviews.Items.Add((string)reader["STAFF_INTERVIEWED"] + " - " + interviewDate.ToShortDateString());
                }
            reader.Close();
            connection.Close();
        }

        private void btnViewInterview_Click(object sender, EventArgs e)
        {
            //If the user selected an interview, display the interview.
            //Else the user hasn't selected anything, so don't display anything.
            if (lstInterviews.SelectedItem != null)
            {
                string nameDate = (string)lstInterviews.SelectedItem;
                int separatorIndex = nameDate.IndexOf('-');
                string name = nameDate.Substring(0, separatorIndex - 1);
                string dateInfo = nameDate.Substring(separatorIndex + 1);
                string[] dateParts = dateInfo.Split('/');
                DateTime date = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[0]), int.Parse(dateParts[1]));

                FormStaffInterview staffInterview = new FormStaffInterview(personId, name, date, this);
                staffInterview.Show();
            }
            else
                MessageBox.Show("An interview needs to be selected before one can be viewed.", "Error - No Interview Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Opens up a blank Staff Interview form.
        /// </summary>
        private void btnAddInterview_Click(object sender, EventArgs e)
        {
            FormStaffInterview staffInterview = new FormStaffInterview(personId, this);
            staffInterview.Show();
        }

        private void lstInterviews_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If an interview is selected, enable the buttons to view and remove it.
            //Else an interview isn't selected, so disable the buttons to view and remove interviews.
            if (lstInterviews.SelectedItem != null)
            {
                btnRemoveInterview.Enabled = true;
                btnViewInterview.Enabled = true;
            }
            else
            {
                btnRemoveInterview.Enabled = false;
                btnViewInterview.Enabled = false;
            }
        }
    }
}