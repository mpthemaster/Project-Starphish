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
        private bool selectedQABFBehavior = false;

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
            dataGridViewBehaviorsStaffInterviews.Rows.Clear();
            dataGridViewAntecedents.Rows.Clear();
            chartQABFAnalysis.Series[0].Points.Clear();
            selectedInterviews.Clear();
            selectedQABFBehavior = false;

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
                calculateBehaviors();
                calculateAntecedents();
                calculateQABFs();
            }
            else
            {
                btnRemoveInterview.Enabled = false;
                btnViewInterview.Enabled = false;
            }
        }

        /// <summary>
        /// Calculates information about the QABFs from the selected staff interviews.
        /// </summary>
        private void calculateQABFs()
        {
            //If a behavior is selected, display the QABF chart for it.
            if (selectedQABFBehavior)
            {
                List<int> answers = new List<int>();

                //Foreach staff interview, go through its list of strengths.
                //  Foreach behavior in a given staff interview, check if it matches the selected behavior for QABF analysis.
                //      If it matches the selected behavior, add its answers for totaling.
                //      Else check the next behavior to see if it matches the selected behavior.
                foreach (StaffInterview staffInterview in selectedInterviews)
                    foreach (Behavior behavior in staffInterview.Behaviors)
                        if (behavior.Name == (string)dataGridViewBehaviorsStaffInterviews.SelectedRows[0].Cells[0].Value)
                        {
                            if (behavior.Qabf != null)
                            {
                                //If the list has not had any answers added to it yet, add the answers for the first time.
                                //Else the list has had answers added to it, so add the answers to the pre-existing totals.
                                if (answers.Count == 0)
                                    for (int i = 0; i < behavior.Qabf.questions.Length; i++)
                                        answers.Add(qabfTextToNum(behavior.Qabf.questions[i].Answer));
                                else
                                    for (int i = 0; i < behavior.Qabf.questions.Length; i++)
                                        answers[i] += qabfTextToNum(behavior.Qabf.questions[i].Answer);
                            }
                            break;
                        }

                double attentionAnswer = 0, escapeAnswer = 0, nonsocialAnswer = 0, physicalAnswer = 0, tangibleAnswer = 0;

                for (int i = 0; i < answers.Count; i++)
                {
                    double answerAverage = answers[i] / (double)selectedInterviews.Count;

                    switch (i + 1)
                    {
                        //Answers for the Attention category.
                        case 1:
                        case 6:
                        case 11:
                        case 16:
                        case 21:
                            attentionAnswer += answerAverage;
                            break;

                        //Answers for the Escape category.
                        case 2:
                        case 7:
                        case 12:
                        case 17:
                        case 22:
                            escapeAnswer += answerAverage;
                            break;

                        //Answers for the Non-Social category.
                        case 3:
                        case 8:
                        case 13:
                        case 18:
                        case 23:
                            nonsocialAnswer += answerAverage;
                            break;

                        //Answers for the Physical category.
                        case 4:
                        case 9:
                        case 14:
                        case 19:
                        case 24:
                            physicalAnswer += answerAverage;
                            break;

                        //Answers for the Tangible category.
                        case 5:
                        case 10:
                        case 15:
                        case 20:
                        case 25:
                            tangibleAnswer += answerAverage;
                            break;

                        default:
                            break;
                    }
                }
                chartQABFAnalysis.Series[0].Points.Clear();
                chartQABFAnalysis.Series[0].Points.AddXY("Attention", attentionAnswer / 5);
                chartQABFAnalysis.Series[0].Points.AddXY("Escape", escapeAnswer / 5);
                chartQABFAnalysis.Series[0].Points.AddXY("Non-Social", nonsocialAnswer / 5);
                chartQABFAnalysis.Series[0].Points.AddXY("Physical", physicalAnswer / 5);
                chartQABFAnalysis.Series[0].Points.AddXY("Tangible", tangibleAnswer / 5);
            }
        }

        /// <summary>
        /// Calculates information about the antecedents from the selected staff interviews.
        /// </summary>
        private void calculateAntecedents()
        {
            Dictionary<string, int[]> antecedents = new Dictionary<string, int[]>();

            //Foreach staff interview, go through its list of behaviors.
            //  Foreach behavior in a given staff interview, check if it has already been added to the dictionaries,
            //      If it hasn't been added, add it.
            //      Else it has been added, so increase the value in each dictionary by the values contained within the antecedents.
            foreach (StaffInterview staffInterview in selectedInterviews)
                foreach (Behavior behavior in staffInterview.Behaviors)
                    if (!antecedents.ContainsKey(behavior.Name))
                    {
                        int[] ants = new int[4];

                        //Add up the number of each type of antecedents.
                        foreach (string antecedent in behavior.Antecedents.Keys)
                            switch (behavior.Antecedents[antecedent])
                            {
                                case "Physiological Causes":
                                    ants[0]++;
                                    break;

                                case "Environmental Causes":
                                    ants[1]++;
                                    break;

                                case "Psychological Causes":
                                    ants[2]++;
                                    break;

                                case "Social Causes":
                                    ants[3]++;
                                    break;

                                default:
                                    break;
                            }
                        antecedents.Add(behavior.Name, ants);
                    }
                    else
                    {
                        //Add up the number of each type of antecedents.
                        int[] ants = antecedents[behavior.Name];
                        foreach (string antecedent in behavior.Antecedents.Keys)
                            switch (behavior.Antecedents[antecedent])
                            {
                                case "Physiological Causes":
                                    ants[0]++;
                                    break;

                                case "Environmental Causes":
                                    ants[1]++;
                                    break;

                                case "Psychological Causes":
                                    ants[2]++;
                                    break;

                                case "Social Causes":
                                    ants[3]++;
                                    break;

                                default:
                                    break;
                            }
                    }

            //Calculate the averages for each behavior's types of antecedents, and the combined average of all the antecedents and display them.
            foreach (string behavior in antecedents.Keys)
            {
                double physiologicalAverage = antecedents[behavior][0] / (double)selectedInterviews.Count;
                double environmentalAverage = antecedents[behavior][1] / (double)selectedInterviews.Count;
                double psychologicalAverage = antecedents[behavior][2] / (double)selectedInterviews.Count;
                double socialAverage = antecedents[behavior][3] / (double)selectedInterviews.Count;

                //Calculates the total average.
                double total = 0;
                for (int i = 0; i < antecedents[behavior].Length; i++)
                    total += antecedents[behavior][i];
                total /= (double)selectedInterviews.Count;

                dataGridViewAntecedents.Rows.Add(behavior, total.ToString("0.##"), physiologicalAverage.ToString("0.##"), environmentalAverage.ToString("0.##"), psychologicalAverage.ToString("0.##"), socialAverage.ToString("0.##"));
            }
        }

        /// <summary>
        /// Calcuates information about the behaviors from the selected staff interviews.
        /// </summary>
        private void calculateBehaviors()
        {
            Dictionary<string, int> behaviorFrequency = new Dictionary<string, int>();
            Dictionary<string, int> behaviorSeverity = new Dictionary<string, int>();

            //Foreach staff interview, go through its list of behaviorss.
            //  Foreach behavior in a given staff interview, check if its frequencies and severities have already been added to the dictionaries,
            //      If they haven't been added, add them.
            //      Else they have been added, so increase the value in each dictionary by the values contained within the Behavior.
            foreach (StaffInterview staffInterview in selectedInterviews)
                foreach (Behavior behavior in staffInterview.Behaviors)
                    if (!behaviorFrequency.ContainsKey(behavior.Name))
                    {
                        behaviorFrequency.Add(behavior.Name, frequencyTextToNum(behavior.Frequency));
                        behaviorSeverity.Add(behavior.Name, severityTextToNum(behavior.Severity));
                    }
                    else
                    {
                        behaviorFrequency[behavior.Name] += frequencyTextToNum(behavior.Frequency);
                        behaviorSeverity[behavior.Name] += severityTextToNum(behavior.Severity);
                    }

            //Calculate the averages for each behavior's frequency, severity, and combined total and display them.
            foreach (string behavior in behaviorFrequency.Keys)
            {
                double frequencyAverage = behaviorFrequency[behavior] / (double)selectedInterviews.Count;
                double severityAverage = behaviorSeverity[behavior] / (double)selectedInterviews.Count;
                double totalAverage = frequencyAverage + severityAverage;

                dataGridViewBehaviorsStaffInterviews.Rows.Add(behavior, frequencyAverage.ToString("0.##"), severityAverage.ToString("0.##"), totalAverage.ToString("0.##"));
            }

            //Sort the rows and give them rank based on the combined total of frequency and severity.
            dataGridViewBehaviorsStaffInterviews.Sort(dataGridViewBehaviorsStaffInterviews.Columns[dataGridViewBehaviorsStaffInterviews.Columns.Count - 2], System.ComponentModel.ListSortDirection.Descending);
            for (int i = 0; i < dataGridViewBehaviorsStaffInterviews.Rows.Count - 1; i++)
                dataGridViewBehaviorsStaffInterviews.Rows[i].Cells[dataGridViewBehaviorsStaffInterviews.Columns.Count - 1].Value = i + 1;
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
            foreach (string strength in strengths.Keys)
            {
                double average = strengths[strength] / (double)selectedInterviews.Count;

                if (average >= .5)
                {
                    dataGridViewStrengths.Rows.Add(strength, average.ToString("0.##"));
                }
            }
        }

        /// <summary>
        /// Converts the qabf answer to corresponding number values for calculations.
        /// </summary>
        /// <param name="qabfText">The qabf answer.</param>
        /// <returns>The number value for a specific qabf answer.</returns>
        private int qabfTextToNum(string qabfText)
        {
            switch (qabfText)
            {
                case "Doesn't Apply":
                case "Never ":
                    return 0;

                case "Rarely":
                    return 1;

                case "Some":
                    return 2;

                case "Often":
                    return 3;

                default:
                    return -1;
            }
        }

        /// <summary>
        /// Converts the severity text options to corresponding number values for calculations.
        /// </summary>
        /// <param name="severityText">The severity option.</param>
        /// <returns>The number value for a specific severity text.</returns>
        private int severityTextToNum(string severityText)
        {
            switch (severityText)
            {
                case "Mild":
                    return 1;

                case "Moderate":
                    return 2;

                case "Severe":
                    return 3;

                default:
                    return 0;
            }
        }

        /// <summary>
        /// Converts the frequency text options to corresponding number values for calculations.
        /// </summary>
        /// <param name="frequencyText">The frequency option.</param>
        /// <returns>The number value for a specific frequency text.</returns>
        private int frequencyTextToNum(string frequencyText)
        {
            switch (frequencyText)
            {
                case "Hourly":
                    return 4;

                case "Daily":
                    return 3;

                case "Weekly":
                    return 2;

                case "Less Often":
                    return 1;

                default:
                    return 0;
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

        private void dataGridViewBehaviorsStaffInterviews_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewBehaviorsStaffInterviews.SelectedRows.Count > 0)
                selectedQABFBehavior = true;
            else
                selectedQABFBehavior = false;

            calculateQABFs();
        }

        private void dataGridViewBehaviorsStaffInterviews_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}