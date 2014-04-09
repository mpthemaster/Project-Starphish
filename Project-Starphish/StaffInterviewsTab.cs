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
        private List<StaffInterview> staffInterviews = new List<StaffInterview>();
        private List<StaffInterview> selectedInterviews = new List<StaffInterview>();

        /// <summary>
        /// Loads up the pre-existing staff interviews to view.
        /// </summary>
        public void mainStaffInterview()
        {
            staffInterviews.Clear();

            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            connection.Open();
            statement = "SELECT PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED FROM STAFF_INTERVIEW ORDER BY INTERVIEW_DATE DESC";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            lstInterviews.Items.Clear();

            //Get all the staff interviews and display them.
            while (reader.Read())
                if ((int)reader["PERSON_ID"] == personId)
                {
                    DateTime interviewDate = (DateTime)reader["INTERVIEW_DATE"];
                    lstInterviews.Items.Add((string)reader["STAFF_INTERVIEWED"] + " - " + interviewDate.ToShortDateString());

                    staffInterviews.Add(new StaffInterview(connection, personId, interviewDate, (string)reader["STAFF_INTERVIEWED"]));
                }
            reader.Close();
            connection.Close();

            //Loads up all the data from the staff interviews in the DB.
            foreach (StaffInterview staffInterview in staffInterviews)
                staffInterview.retrieveData();
        }

        private void btnViewInterview_Click(object sender, EventArgs e)
        {
            //If the user selected an interview, display the interview.
            //Else the user hasn't selected anything, so don't display anything.
            if (lstInterviews.SelectedItem != null)
            {
                string name;
                DateTime date;
                getInterviewInfo(out name, out date);

                FormStaffInterview staffInterview = new FormStaffInterview(personId, name, date, this);
                staffInterview.Show();
            }
            else
                MessageBox.Show("An interview needs to be selected before one can be viewed.", "Error - No Interview Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void getInterviewInfo(out string name, out DateTime date)
        {
            string nameDate = (string)lstInterviews.SelectedItem;
            int separatorIndex = nameDate.IndexOf('-');
            name = nameDate.Substring(0, separatorIndex - 1);
            string dateInfo = nameDate.Substring(separatorIndex + 1);
            string[] dateParts = dateInfo.Split('/');
            date = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[0]), int.Parse(dateParts[1]));
        }

        /// <summary>
        /// Opens up a blank Staff Interview form.
        /// </summary>
        private void btnAddInterview_Click(object sender, EventArgs e)
        {
            FormStaffInterview staffInterview = new FormStaffInterview(personId, this);
            staffInterview.Show();

            //Disable the remove and view buttons because no interview is selected now.
            btnRemoveInterview.Enabled = false;
            btnViewInterview.Enabled = false;
        }

        private void lstInterviews_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridViewStrengths.Rows.Clear();
            selectedInterviews.Clear();

            //If an interview is selected, enable the buttons to view and remove it.
            //Else an interview isn't selected, so disable the buttons to view and remove interviews.
            if (lstInterviews.SelectedItem != null)
            {
                btnRemoveInterview.Enabled = true;
                btnViewInterview.Enabled = true;

                //Figure out what staff interviews are selected.
                ListBox.SelectedObjectCollection collection = lstInterviews.SelectedItems;
                foreach (Object selectedPerson in collection)
                    foreach (StaffInterview staffInterview in staffInterviews)
                        if (staffInterview.IntervieweeName == ((string)selectedPerson).Substring(0, ((string)selectedPerson).IndexOf('-') - 1))
                        {
                            selectedInterviews.Add(staffInterview);
                            break;
                        }

                //Display the selected staff interviews' results.
                calculateStrengths();
            }
            else
            {
                btnRemoveInterview.Enabled = false;
                btnViewInterview.Enabled = false;
            }
        }

        /// <summary>
        /// Calculates the strengths from the selected staff interviews.
        /// </summary>
        private void calculateStrengths()
        {
            Dictionary<string, int> strengths = new Dictionary<string, int>(); //Holds each distinct strength and the number of staff interviews that said it.

            //Foreach staff interview, go through its list of strengths.
            //  For all the strengths in a given staff interview, check if it has already been added to the strengths dictionary.
            //      If it hasn't been added, add it.
            //      Else it has been added, so increase the value that holds how many staff interviews specified this strength by 1.
            foreach (StaffInterview staffInterview in selectedInterviews)
                for (int i = 0; i < staffInterview.Strengths.Count; i++)
                    if (!strengths.ContainsKey(staffInterview.Strengths[i]))
                        strengths.Add(staffInterview.Strengths[i], 1);
                    else
                        strengths[staffInterview.Strengths[i]]++;

            //Calculate the average for each strength and display it if it rounds up to 1.
            foreach (String strength in strengths.Keys)
            {
                double average = strengths[strength] / (double)selectedInterviews.Count;

                if (average >= .5)
                {
                    dataGridViewStrengths.Rows.Add(strength, average);
                }
            }
        }

        private void btnRemoveInterview_Click(object sender, EventArgs e)
        {
            //If a staff interview is selected and the user confirms the removal, then remove the staff interview.
            if (lstInterviews.SelectedItem != null)
            {
                if (MessageBox.Show("Are you sure you want to remove the selected staff interview? The data contained within cannot be retrieved after being removed.", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    string name;
                    DateTime date;
                    getInterviewInfo(out name, out date);
                    connection.Open();

                    //Remove all strength data associated with the staff interview.
                    string statement = "DELETE FROM STAFF_INTERVIEW_STRENGTH WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + date + "' AND STAFF_INTERVIEWED='" + name + "'";
                    SqlCommand command = new SqlCommand(statement, connection);
                    command.ExecuteNonQuery();

                    //Remove all antecedent data associated with the staff interview.
                    statement = "DELETE FROM STAFF_INTERVIEW_ANTECEDENT WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + date + "' AND STAFF_INTERVIEWED='" + name + "'";
                    command = new SqlCommand(statement, connection);
                    command.ExecuteNonQuery();

                    //Remove all behavior data associated with the staff interview.
                    statement = "DELETE FROM STAFF_INTERVIEW_BEHAVIOR WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + date + "' AND STAFF_INTERVIEWED='" + name + "'";
                    command = new SqlCommand(statement, connection);
                    command.ExecuteNonQuery();

                    //Remove all interview data associated with the staff interview.
                    statement = "DELETE FROM STAFF_INTERVIEW WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + date + "' AND STAFF_INTERVIEWED='" + name + "'";
                    command = new SqlCommand(statement, connection);
                    command.ExecuteNonQuery();

                    connection.Close();
                    mainStaffInterview();

                    //Disable the remove and view buttons because no interview is selected now.
                    btnRemoveInterview.Enabled = false;
                    btnViewInterview.Enabled = false;
                }
            }
            else
                MessageBox.Show("An interview needs to be selected before one can be removed.", "Error - No Interview Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}