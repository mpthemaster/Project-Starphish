/*
 *  Remove and Add buttons for Parts I and III should be disabled and enabled as appropriate, though error code is in place if I don't have the time to do this.
 *
 * Possible Bug: While not causing any problems right now, it should be known that behaviors can have a frequency called "Less Often" while the parent node for that
 *              is called "LessOften". Any comparisons of these two and this issue will need to be fixed.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormStaffInterview : Form
    {
        //Allows the form to have its interview list box appropriately updated.
        private FormMain form;

        //All behaviors that have been added in an interview.
        private List<Behavior> behaviors = new List<Behavior>();

        private const string theConnectionString = "Data Source=localhost\\PROJECTSTARPHISH;Initial Catalog=ProjectStarphish;Integrated Security=True";
        private SqlConnection connection = new SqlConnection(theConnectionString);

        //Used for putting things into and getting out of the database.
        private int personId; //The ID of Shaun Burke's client.

        private string intervieweeName; //The name of the interviewee.
        private DateTime interviewDate; //The date of the interview.
        private bool newInterview; //Whether this is a new interview (putting a new row into the table) or retreiving an interview from the database.

        /// <summary>
        /// Assumes a new staff interview is being created.
        /// </summary>
        /// <param name="personId">The ID of the client that this interview is being conducted for.</param>
        /// <param name="form">The main form of the program that this form returns to.</param>
        public FormStaffInterview(int personId, FormMain form)
        {
            InitializeComponent();

            this.personId = personId;
            newInterview = true;
            this.form = form;
            setToDefaultOptions();
        }

        /// <summary>
        /// Assumes a staff interview with the specified person is being viewed.
        /// </summary>
        /// <param name="personId">The ID of the client that this interview is being conducted for.</param>
        /// <param name="intervieweeName">The interviewee's name.</param>
        /// <param name="form">The main form of the program that this form returns to.</param>
        public FormStaffInterview(int personId, string intervieweeName, DateTime interviewDate, FormMain form)
        {
            InitializeComponent();

            this.personId = personId;
            this.intervieweeName = intervieweeName;
            this.interviewDate = interviewDate;
            newInterview = false;
            this.form = form;

            setToDefaultOptions();
            retrieveInterviewData();
        }

        /// <summary>
        /// Sets comboboxes to their default options.
        /// </summary>
        private void setToDefaultOptions()
        {
            //Gives the comboboxes default picked options.
            comboStrengthOfEmotion.SelectedIndex = 0;
            comboStrengthOfTolerance.SelectedIndex = 0;
            comboStrengthOfWill.SelectedIndex = 0;
            comboRationalCivicStrength.SelectedIndex = 0;
            comboBehavior.SelectedIndex = 0;
            comboBehaviorFrequency.SelectedIndex = 0;
            comboBehaviorSeverity.SelectedIndex = 0;
            comboPhysiologicalCause.SelectedIndex = 0;
            comboPsychologicalCause.SelectedIndex = 0;
            comboEnvironmentalCause.SelectedIndex = 0;
            comboSocialCause.SelectedIndex = 0;
        }

        private void btnSaveStaffInterview_Click(object sender, EventArgs e)
        {
            //Do from error checking first to make sure required fields are filled out.
            if (txtStaffIntervieweeName.Text == "")
            {
                MessageBox.Show("Please enter the name of the person you are interviewing.", "Error - Interviewee Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtStaffIntervieweeName.Focus();
                return;
            }

            connection.Open();

            //If this is a new interview, create a new entry in the database,
            //Else this isn't a new interview, so update the current entry.
            if (newInterview)
            {
                //First try to save an entry into the STAFF_INTERVIEW table. If it fails, then a new interview was being made as a duplicate
                if (saveStaffInterview())
                {
                    //Now save an entry into the STAFF_INTERVIEW_STRENGTH table.
                    saveStrengths();

                    //Now save an entry into the STAFF_INTERVIEW_BEHAVIOR table.
                    saveBehaviors();

                    //Now save an entry into the STAFF_INTERVIEW_ANTECEDENT table.
                    saveAntecedents();

                    //Now save an entry into the STAFF_INTERVIEW_QABF table.

                    connection.Close();
                    form.mainStaffInterview();
                    this.Close();
                }
            }
            else
            {
                updateStrengths();
                updateAntecedents();
                updateBehaviors();
                updateStaffInterview();

                connection.Close();
                form.mainStaffInterview();
                this.Close();
                //statement =
                //UPDATE PERSON SET  FNAME = @FNAME, //For updating an existing person.
                //command = new SqlCommand(statement, connection);
            }
        }

        /// <summary>
        /// Saves all the generic staff interview data.
        /// </summary>
        /// <returns>Whether the save succeeded.</returns>
        private bool saveStaffInterview()
        {
            //Connect to the database.
            string statement;
            SqlCommand command;

            statement = "INSERT INTO STAFF_INTERVIEW (PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, STAFF_ROLE, INTERVIEWER) VALUES        (@PERSON_ID, @INTERVIEW_DATE, @STAFF_INTERVIEWED, @STAFF_ROLE, @INTERVIEWER)";
            command = new SqlCommand(statement, connection);
            command.Parameters.AddWithValue("@PERSON_ID", personId);
            command.Parameters.Add("@INTERVIEW_DATE", SqlDbType.DateTime).Value = datePickerStaffInterview.Value;
            command.Parameters.AddWithValue("@STAFF_INTERVIEWED", txtStaffIntervieweeName.Text);
            command.Parameters.AddWithValue("@STAFF_ROLE", txtStaffRole.Text);
            command.Parameters.AddWithValue("@INTERVIEWER", txtInterviewerName.Text);

            //Catches an error if an interview with the same keys tries to be added by the user.
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("An interview with " + txtStaffIntervieweeName.Text + " on " + datePickerStaffInterview.Value.ToShortDateString() + " has already been recorded. Either delete or modify the pre-existing interview.",
                    "Fatal Error - Duplicate Interview", MessageBoxButtons.OK, MessageBoxIcon.Error);
                connection.Close();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Saves all strengths from the node tree into the database.
        /// </summary>
        private void saveStrengths()
        {
            //Connect to the database.
            string statement;
            SqlCommand command;

            statement = "INSERT INTO STAFF_INTERVIEW_STRENGTH (PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, STRENGTH, CATEGORY) VALUES       (@PERSON_ID, @INTERVIEW_DATE, @STAFF_INTERVIEWED, @STRENGTH, @CATEGORY)";
            command = new SqlCommand(statement, connection);
            foreach (TreeNode parentNode in treeViewStrengths.Nodes)
                foreach (TreeNode childNode in parentNode.Nodes)
                {
                    command.Parameters.AddWithValue("@PERSON_ID", personId);
                    command.Parameters.Add("@INTERVIEW_DATE", SqlDbType.DateTime).Value = datePickerStaffInterview.Value;
                    command.Parameters.AddWithValue("@STAFF_INTERVIEWED", txtStaffIntervieweeName.Text);
                    command.Parameters.AddWithValue("@STRENGTH", childNode.Name);
                    command.Parameters.AddWithValue("@CATEGORY", parentNode.Name);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
        }

        /// <summary>
        /// Saves all behaviors and their information from the node tree into the database.
        /// </summary>
        private void saveBehaviors()
        {
            //Connect to the database.
            string statement;
            SqlCommand command;

            statement = "INSERT INTO STAFF_INTERVIEW_BEHAVIOR (PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, BEHAVIOR, SEVERITY, FREQUENCY) VALUES       (@PERSON_ID, @INTERVIEW_DATE, @STAFF_INTERVIEWED, @BEHAVIOR, @SEVERITY, @FREQUENCY)";
            command = new SqlCommand(statement, connection);
            foreach (Behavior behavior in behaviors)
            {
                command.Parameters.AddWithValue("@PERSON_ID", personId);
                command.Parameters.Add("@INTERVIEW_DATE", SqlDbType.DateTime).Value = datePickerStaffInterview.Value;
                command.Parameters.AddWithValue("@STAFF_INTERVIEWED", txtStaffIntervieweeName.Text);
                command.Parameters.AddWithValue("@BEHAVIOR", behavior.Name);
                command.Parameters.AddWithValue("@SEVERITY", behavior.Severity);
                command.Parameters.AddWithValue("@FREQUENCY", behavior.Frequency);
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
        }

        /// <summary>
        /// Saves all behaviors' antecedents from the node tree into the database.
        /// </summary>
        private void saveAntecedents()
        {
            //Connect to the database.
            string statement;
            SqlCommand command;

            statement = "INSERT INTO STAFF_INTERVIEW_ANTECEDENT (PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, BEHAVIOR, ANTECEDENT, CATEGORY) VALUES       (@PERSON_ID, @INTERVIEW_DATE, @STAFF_INTERVIEWED, @BEHAVIOR, @ANTECEDENT, @CATEGORY)";
            command = new SqlCommand(statement, connection);
            foreach (Behavior behavior in behaviors)
            {
                Dictionary<string, string>.KeyCollection keys = behavior.Antecedents.Keys;
                foreach (string key in keys)
                {
                    command.Parameters.AddWithValue("@PERSON_ID", personId);
                    command.Parameters.Add("@INTERVIEW_DATE", SqlDbType.DateTime).Value = datePickerStaffInterview.Value;
                    command.Parameters.AddWithValue("@STAFF_INTERVIEWED", txtStaffIntervieweeName.Text);
                    command.Parameters.AddWithValue("@BEHAVIOR", behavior.Name);
                    command.Parameters.AddWithValue("@ANTECEDENT", key);
                    command.Parameters.AddWithValue("@CATEGORY", behavior.Antecedents[key]);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Gets all the data from the database that pertains to the user-selected staff interview and displays it to the screen.
        /// </summary>
        private void retrieveInterviewData()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            //Get the information from the Staff Interview and display it.
            connection.Open();
            statement = "SELECT PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, STAFF_ROLE, INTERVIEWER FROM STAFF_INTERVIEW";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                if ((int)reader["PERSON_ID"] == personId && ((DateTime)reader["INTERVIEW_DATE"]) == interviewDate && (string)reader["STAFF_INTERVIEWED"] == intervieweeName)
                {
                    datePickerStaffInterview.Value = (DateTime)reader["INTERVIEW_DATE"];
                    txtStaffIntervieweeName.Text = (string)reader["STAFF_INTERVIEWED"];
                    txtStaffRole.Text = (string)reader["STAFF_ROLE"];
                    txtInterviewerName.Text = (string)reader["INTERVIEWER"];
                }
            }
            reader.Close();

            //Get the information from the rest of the Staff Interview and display it.
            loadStrengths();
            loadBehaviors();
            loadAntecedents();
            connection.Close();
        }

        /// <summary>
        /// Loads and display's the specified staff interview's strengths.
        /// </summary>
        private void loadStrengths()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, STRENGTH, CATEGORY FROM STAFF_INTERVIEW_STRENGTH";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            //Get all the category nodes for strengths.
            TreeNode emotionStrengths = treeViewStrengths.Nodes[0];
            TreeNode willStrengths = treeViewStrengths.Nodes[1];
            TreeNode rationalStrengths = treeViewStrengths.Nodes[2];
            TreeNode toleranceStrengths = treeViewStrengths.Nodes[3];
            TreeNode otherStrengths = treeViewStrengths.Nodes[4];

            //Get the information from the Staff Interview and display it.
            while (reader.Read())
                if ((int)reader["PERSON_ID"] == personId && ((DateTime)reader["INTERVIEW_DATE"]) == interviewDate && (string)reader["STAFF_INTERVIEWED"] == intervieweeName)
                {
                    string strengthName = (string)reader["STRENGTH"];
                    string category = (string)reader["CATEGORY"];

                    if (category == emotionStrengths.Name)
                        emotionStrengths.Nodes.Add(strengthName, strengthName);
                    else if (category == willStrengths.Name)
                        willStrengths.Nodes.Add(strengthName, strengthName);
                    else if (category == rationalStrengths.Name)
                        rationalStrengths.Nodes.Add(strengthName, strengthName);
                    else if (category == toleranceStrengths.Name)
                        toleranceStrengths.Nodes.Add(strengthName, strengthName);
                    else
                        otherStrengths.Nodes.Add(strengthName, strengthName);
                }
            reader.Close();
        }

        /// <summary>
        /// Loads and displays the staff interview's behaviors.
        /// </summary>
        private void loadBehaviors()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, BEHAVIOR, SEVERITY, FREQUENCY FROM STAFF_INTERVIEW_BEHAVIOR";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            //Get all the category nodes for behaviors.
            TreeNode hourlyBehaviors = treeViewBehaviors.Nodes[0];
            TreeNode dailyBehaviors = treeViewBehaviors.Nodes[1];
            TreeNode weeklyBehaviors = treeViewBehaviors.Nodes[2];
            TreeNode lessOftenBehaviors = treeViewBehaviors.Nodes[3];

            //Get the information from the Staff Interview and display it.
            while (reader.Read())
                if ((int)reader["PERSON_ID"] == personId && ((DateTime)reader["INTERVIEW_DATE"]) == interviewDate && (string)reader["STAFF_INTERVIEWED"] == intervieweeName)
                {
                    string behaviorName = (string)reader["BEHAVIOR"];
                    string behaviorSeverity = (string)reader["SEVERITY"];
                    string behaviorFrequency = (string)reader["FREQUENCY"];

                    //Figure out what category in the treeview that the behavior belongs to.
                    if (behaviorFrequency == hourlyBehaviors.Name)
                        hourlyBehaviors.Nodes.Add(behaviorName, behaviorName + " - " + behaviorSeverity);
                    else if (behaviorFrequency == dailyBehaviors.Name)
                        dailyBehaviors.Nodes.Add(behaviorName, behaviorName + " - " + behaviorSeverity);
                    else if (behaviorFrequency == weeklyBehaviors.Name)
                        weeklyBehaviors.Nodes.Add(behaviorName, behaviorName + " - " + behaviorSeverity);
                    else
                        lessOftenBehaviors.Nodes.Add(behaviorName, behaviorName + " - " + behaviorSeverity);

                    //Add the behavior to the list.
                    behaviors.Add(new Behavior(behaviorName, behaviorSeverity, behaviorFrequency));
                }
            reader.Close();
        }

        // <summary>
        /// Loads and displays the staff interview's behaviors' antecedents.
        /// </summary>
        private void loadAntecedents()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED, BEHAVIOR, ANTECEDENT, CATEGORY FROM STAFF_INTERVIEW_ANTECEDENT";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            //Create all the category nodes for behaviors' antecedents and add their causes.
            foreach (Behavior behavior in behaviors)
                treeViewAntecedents.Nodes.Add(behavior.Name, behavior.Name);

            //Get the information from the Staff Interview and display it.
            while (reader.Read())
                if ((int)reader["PERSON_ID"] == personId && ((DateTime)reader["INTERVIEW_DATE"]) == interviewDate && (string)reader["STAFF_INTERVIEWED"] == intervieweeName)
                {
                    string behaviorName = (string)reader["BEHAVIOR"];
                    string behaviorAntecedent = (string)reader["ANTECEDENT"];
                    string behaviorCategory = (string)reader["CATEGORY"];

                    //Figure out what category in the treeview that the antecedent belongs to and add it.
                    foreach (Behavior behavior in behaviors)
                        if (behavior.Name == behaviorName)
                        {
                            behavior.Antecedents.Add(behaviorAntecedent, behaviorCategory);

                            TreeNode[] behaviorNode = treeViewAntecedents.Nodes.Find(behaviorName, false);

                            //If the cause's category doesn't exist, create it.
                            if (!behaviorNode[0].Nodes.ContainsKey(behaviorCategory))
                                behaviorNode[0].Nodes.Add(behaviorCategory, behaviorCategory);
                            behaviorNode[0].Nodes[behaviorNode[0].Nodes.IndexOfKey(behaviorCategory)].Nodes.Add(behaviorAntecedent, behaviorAntecedent);
                        }
                }
            reader.Close();
        }

        /// <summary>
        /// Updates the database's strengths data with the modified data by first deleting all existing records and then adding all the ones from the form in.
        /// </summary>
        private void updateStrengths()
        {
            //Connect to the database.
            string statement = "DELETE FROM STAFF_INTERVIEW_STRENGTH WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + interviewDate + "' AND STAFF_INTERVIEWED='" + intervieweeName + "'";
            SqlCommand command = new SqlCommand(statement, connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the database's antecedents data with the modified data by first deleting all existing records.
        /// </summary>
        private void updateAntecedents()
        {
            //Connect to the database.
            string statement = "DELETE FROM STAFF_INTERVIEW_ANTECEDENT WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + interviewDate + "' AND STAFF_INTERVIEWED='" + intervieweeName + "'";
            SqlCommand command = new SqlCommand(statement, connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the database's behaviors data with the modified data by first deleting all existing records and then adding all the ones from the form in.
        /// </summary>
        private void updateBehaviors()
        {
            //Connect to the database.
            string statement = "DELETE FROM STAFF_INTERVIEW_BEHAVIOR WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + interviewDate + "' AND STAFF_INTERVIEWED='" + intervieweeName + "'";
            SqlCommand command = new SqlCommand(statement, connection);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Updates the database's behaviors data with the modified data by first deleting all existing records and then adding all the ones from the form in.
        /// </summary>
        private void updateStaffInterview()
        {
            //Connect to the database.
            string statement = "DELETE FROM STAFF_INTERVIEW WHERE PERSON_ID='" + personId + "' AND INTERVIEW_DATE='" + interviewDate + "' AND STAFF_INTERVIEWED='" + intervieweeName + "'";
            SqlCommand command = new SqlCommand(statement, connection);
            command.ExecuteNonQuery();
            saveStaffInterview();
            saveStrengths();
            saveBehaviors();
            saveAntecedents();
        }

        /// <summary>
        /// Displays all uncompleted and completed QABFs in a new form.
        /// </summary>
        private void btnQABFs_Click(object sender, EventArgs e)
        {
            FormQABF formQABF = new FormQABF();
            formQABF.ShowDialog();
        }

        private void comboBehavior_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user selected "Other", enable the textbox for filling in custom behavior.
            //Else the user selected an option other than "Other", so disable the textbox for filling in custom behavior.
            if (comboBehavior.SelectedIndex == comboBehavior.Items.Count - 1)
                txtBehaviorOther.Enabled = true;
            else
                txtBehaviorOther.Enabled = false;
        }

        private void comboPhysiologicalCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user selected "Other", enable the textbox for filling in custom Cause.
            //Else the user selected an option other than "Other", so disable the textbox for filling in a custom cause.
            if (comboPhysiologicalCause.SelectedIndex == comboPhysiologicalCause.Items.Count - 1)
                txtPhysiologicalCauseOther.Enabled = true;
            else
                txtPhysiologicalCauseOther.Enabled = false;
        }

        private void comboEnvironmentalCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user selected "Other", enable the textbox for filling in custom Cause.
            //Else the user selected an option other than "Other", so disable the textbox for filling in a custom cause.
            if (comboEnvironmentalCause.SelectedIndex == comboEnvironmentalCause.Items.Count - 1)
                txtEnvironmentalCauseOther.Enabled = true;
            else
                txtEnvironmentalCauseOther.Enabled = false;
        }

        private void comboPsychologicalCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user selected "Other", enable the textbox for filling in custom Cause.
            //Else the user selected an option other than "Other", so disable the textbox for filling in a custom cause.
            if (comboPsychologicalCause.SelectedIndex == comboPsychologicalCause.Items.Count - 1)
                txtPsychologicalCauseOther.Enabled = true;
            else
                txtPsychologicalCauseOther.Enabled = false;
        }

        private void comboSocialCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user selected "Other", enable the textbox for filling in custom Cause.
            //Else the user selected an option other than "Other", so disable the textbox for filling in a custom cause.
            if (comboSocialCause.SelectedIndex == comboSocialCause.Items.Count - 1)
                txtSocialCauseOther.Enabled = true;
            else
                txtSocialCauseOther.Enabled = false;
        }

        private void btnAddStrengthOfEmotion_Click(object sender, EventArgs e)
        {
            TreeNode emotionStrengths = treeViewStrengths.Nodes[0];
            string newEmotion = (string)comboStrengthOfEmotion.SelectedItem;

            //If the user-selected emotional strength is not already in treeview, then add it and expand its parent's view.
            //Else display a messagebox that lets the user know that that particular strength as already been added.
            if (!emotionStrengths.Nodes.ContainsKey(newEmotion))
            {
                emotionStrengths.Nodes.Add(newEmotion, newEmotion);
                emotionStrengths.ExpandAll();
            }
            else
                MessageBox.Show("This Strength of Emotion has already been added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnAddStrengthOfWill_Click(object sender, EventArgs e)
        {
            TreeNode willStrengths = treeViewStrengths.Nodes[1];
            string newWill = (string)comboStrengthOfWill.SelectedItem;

            //If the user-selected will strength is not already in treeview, then add it and expand its parent's view.
            //Else display a messagebox that lets the user know that that particular strength as already been added.
            if (!willStrengths.Nodes.ContainsKey(newWill))
            {
                willStrengths.Nodes.Add(newWill, newWill);
                willStrengths.ExpandAll();
            }
            else
                MessageBox.Show("This Strength of Will has already been added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnAddRationalCivicStrength_Click(object sender, EventArgs e)
        {
            TreeNode civicStrengths = treeViewStrengths.Nodes[2];
            string newCivic = (string)comboRationalCivicStrength.SelectedItem;

            //If the user-selected Rational/Civic strength is not already in treeview, then add it and expand its parent's view.
            //Else display a messagebox that lets the user know that that particular strength as already been added.
            if (!civicStrengths.Nodes.ContainsKey(newCivic))
            {
                civicStrengths.Nodes.Add(newCivic, newCivic);
                civicStrengths.ExpandAll();
            }
            else
                MessageBox.Show("This Rational and Civic Strength has already been added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnAddStrengthOfTolerance_Click(object sender, EventArgs e)
        {
            TreeNode toleranceStrengths = treeViewStrengths.Nodes[3];
            string newTolerance = (string)comboStrengthOfTolerance.SelectedItem;

            //If the user-selected tolerance strength is not already in treeview, then add it and expand its parent's view.
            //Else display a messagebox that lets the user know that that particular strength as already been added.
            if (!toleranceStrengths.Nodes.ContainsKey(newTolerance))
            {
                toleranceStrengths.Nodes.Add(newTolerance, newTolerance);
                toleranceStrengths.ExpandAll();
            }
            else
                MessageBox.Show("This Strength of Tolerance has already been added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnAddOtherStrength_Click(object sender, EventArgs e)
        {
            TreeNode otherStrengths = treeViewStrengths.Nodes[4];
            string newOther = txtOtherStrength.Text;

            //If the user-entered other strength is not already in treeview, then add it and expand its parent's view.
            //Else display a messagebox that lets the user know that that particular strength as already been added.
            if (!otherStrengths.Nodes.ContainsKey(newOther))
            {
                otherStrengths.Nodes.Add(newOther, newOther);
                otherStrengths.ExpandAll();
                txtOtherStrength.Text = "";
            }
            else
                MessageBox.Show("This Other Strength has already been added.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnRemoveStrength_Click(object sender, EventArgs e)
        {
            TreeNode nodeToRemove = treeViewStrengths.SelectedNode;

            //If the node to remove isn't a top level node (e.g. "Strength of Emotions"), then remove it.
            //Else display an error message to let the user know.
            if (nodeToRemove.Parent != null)
                nodeToRemove.Remove();
            else
                MessageBox.Show("Category names cannot be removed.", "Error - Illegal Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnAddBehavior_Click(object sender, EventArgs e)
        {
            bool behaviorAdded; //For whether a behavior was successfully added to the treeview.
            Behavior tempBehavior; //The behavior to create or update.

            //If the user is supposed to have entered a custom behavior and hasn't, alert the user and stop the behavior from being added.
            if (txtBehaviorOther.Enabled && txtBehaviorOther.Text == "")
            {
                MessageBox.Show("You must enter or pick a behavior name.", "Error - Missing Behavior Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBehaviorOther.Focus();
                return;
            }

            //Gets info from the user about the behavior and sets it.
            string name, severity, frequency;
            severity = (string)comboBehaviorSeverity.SelectedItem;
            frequency = (string)comboBehaviorFrequency.SelectedItem;

            //Figures out where to get the behavior's name from.
            if (txtBehaviorOther.Enabled)
                name = txtBehaviorOther.Text;
            else
                name = (string)comboBehavior.SelectedItem;

            tempBehavior = new Behavior(name, severity, frequency);

            //If the behavior is being updated, delete it from the behaviors list and from the nodes. It will just be added as new to everything to avoid different problems.
            if (btnAddBehavior.Text == "Save Behavior")
            {
                foreach (Behavior behavior in behaviors)
                {
                    if (behavior.Name == treeViewBehaviors.SelectedNode.Name)
                    {
                        behaviors.Remove(behavior);
                        treeViewBehaviors.Nodes.Remove(treeViewBehaviors.SelectedNode);
                        break;
                    }
                }
            }

            //Figures out what parent node this behavior should be attached to in the treeview.
            switch (tempBehavior.Frequency)
            {
                case "Hourly":
                    behaviorAdded = addBehaviorToHourly(tempBehavior);
                    break;

                case "Daily":
                    behaviorAdded = addBehaviorToDaily(tempBehavior);
                    break;

                case "Weekly":
                    behaviorAdded = addBehaviorToWeekly(tempBehavior);
                    break;

                case "Less Often":
                    behaviorAdded = addBehaviorToLessOften(tempBehavior);
                    break;

                default:
                    MessageBox.Show("A non-existent frequency has been chosen!");
                    behaviorAdded = false;
                    break;
            }

            //If the behavior was successfully added, clear the comboboxes and textbox and add it to the other treeview.
            if (behaviorAdded)
            {
                resetBehaviorInputs();
                treeViewAntecedents.Nodes.Add(tempBehavior.Name, tempBehavior.Name);
                behaviors.Add(tempBehavior);
            }
        }

        /// <summary>
        /// Resets all Part II Behavior inputs to their default values.
        /// </summary>
        private void resetBehaviorInputs()
        {
            comboBehaviorSeverity.SelectedIndex = 0;
            comboBehaviorFrequency.SelectedIndex = 0;
            comboBehavior.SelectedIndex = 0;
            txtBehaviorOther.Text = "";
        }

        /// <summary>
        /// Adds the behavior as a child of Less Often.
        /// </summary>
        /// <param name="behavior">The behavior to add.</param>
        /// <returns>Whether the behavior was successfully added.</returns>
        private bool addBehaviorToLessOften(Behavior behavior)
        {
            TreeNode hourlyBehavior = treeViewBehaviors.Nodes[3];
            string newBehavior = behavior.Name;

            if (!isBehaviorExisting(newBehavior))
            {
                hourlyBehavior.Nodes.Add(newBehavior, newBehavior + " - " + behavior.Severity);
                hourlyBehavior.ExpandAll();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the behavior as a child of Weekly.
        /// </summary>
        /// <param name="behavior">The behavior to add.</param>
        /// <returns>Whether the behavior was successfully added.</returns>
        private bool addBehaviorToWeekly(Behavior behavior)
        {
            TreeNode hourlyBehavior = treeViewBehaviors.Nodes[2];
            string newBehavior = behavior.Name;

            if (!isBehaviorExisting(newBehavior))
            {
                hourlyBehavior.Nodes.Add(newBehavior, newBehavior + " - " + behavior.Severity);
                hourlyBehavior.ExpandAll();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the behavior as a child of Dayly.
        /// </summary>
        /// <param name="behavior">The behavior to add.</param>
        /// <returns>Whether the behavior was successfully added.</returns>
        private bool addBehaviorToDaily(Behavior behavior)
        {
            TreeNode hourlyBehavior = treeViewBehaviors.Nodes[1];
            string newBehavior = behavior.Name;

            if (!isBehaviorExisting(newBehavior))
            {
                hourlyBehavior.Nodes.Add(newBehavior, newBehavior + " - " + behavior.Severity);
                hourlyBehavior.ExpandAll();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the behavior as a child of Hourly.
        /// </summary>
        /// <param name="behavior">The behavior to add.</param>
        /// <returns>Whether the behavior was successfully added.</returns>
        private bool addBehaviorToHourly(Behavior behavior)
        {
            TreeNode hourlyBehavior = treeViewBehaviors.Nodes[0];
            string newBehavior = behavior.Name;

            if (!isBehaviorExisting(newBehavior))
            {
                hourlyBehavior.Nodes.Add(newBehavior, newBehavior + " - " + behavior.Severity);
                hourlyBehavior.ExpandAll();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a behavior already exists.
        /// </summary>
        /// <param name="behaviorName">The behavior to check for.</param>
        /// <returns>Whether the behavior already exists.</returns>
        private bool isBehaviorExisting(string behaviorName)
        {
            //Foreach parent node in the treeview,
            //  Check If the user-entered behavior is already one of its children,
            //      Alert the user to this issue and return true.
            //Else the user-entered behavior doesn't exist, so return false.
            foreach (TreeNode node in treeViewBehaviors.Nodes)
                if (node.Nodes.ContainsKey(behaviorName))
                {
                    MessageBox.Show("This Behavior has already been added.", "Error - Duplicate Behavior", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true;
                }
            return false;
        }

        private void btnRemoveBehavior_Click(object sender, EventArgs e)
        {
            TreeNode nodeToRemove = treeViewBehaviors.SelectedNode;

            //If the node to remove isn't a top level node (e.g. "Hourly"), then remove it.
            //Else display an error message to let the user know.
            if (nodeToRemove.Parent != null)
            {
                nodeToRemove.Remove();
                treeViewAntecedents.Nodes.RemoveByKey(nodeToRemove.Name);

                //Foreach behavior in the behaviors list,
                //  Check If  it matches the behavior to remove and remove it if it does.
                foreach (Behavior behavior in behaviors)
                    if (behavior.Name == nodeToRemove.Name)
                    {
                        behaviors.Remove(behavior);
                        return;
                    }
            }
            else
                MessageBox.Show("Category names cannot be removed.", "Error - Illegal Action", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void treeViewBehaviors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            resetBehaviorInputs();
            //If the user selected a node that is a behavior, enable it to be removed and edited.
            //Else the user selected a root node (e.g. "Hourly"), and should not be able to remove it, but can now add a new behavior (behavior inputs are cleared).
            if (treeViewBehaviors.SelectedNode.Parent != null)
            {
                btnRemoveBehavior.Enabled = true;
                btnAddBehavior.Text = "Save Behavior";

                //Foreach behavior in the behaviors list,
                //  Check If  it matches the behavior the currently selected behavior and load up its info so that it can be modified.
                foreach (Behavior behavior in behaviors)
                    if (behavior.Name == treeViewBehaviors.SelectedNode.Name)
                    {
                        displayBehaviorInfo(behavior);
                        return;
                    }
            }
            else
            {
                btnRemoveBehavior.Enabled = false;
                btnAddBehavior.Text = "Add Behavior";
            }
        }

        /// <summary>
        /// Displays a selected behavior's information to the user. This enables the user to review and modify any previously added behaviors.
        /// </summary>
        /// <param name="behavior">The behavior to have its info displayed to the user.</param>
        private void displayBehaviorInfo(Behavior behavior)
        {
            bool otherName = true; //Keeps track of whether the name of the behavior was custom or from the combobox.

            //Search the name combobox and see if the name of the behavior is in there.
            //If it is, select that on the combox.
            foreach (Object behaviorName in comboBehavior.Items)
                if ((string)behaviorName == behavior.Name)
                {
                    otherName = false;
                    comboBehavior.SelectedIndex = comboBehavior.Items.IndexOf(behaviorName);
                    break;
                }

            //If the combobox didn't contain the behavior's name, set the combobox to other and set the name in the custom name textbox.
            if (otherName)
            {
                comboBehavior.SelectedIndex = comboBehavior.Items.Count - 1;
                txtBehaviorOther.Text = behavior.Name;
            }

            //Search the frequency combobox and set its selectedindex to match that of the behavior's frequency.
            foreach (Object behaviorFrequency in comboBehaviorFrequency.Items)
                if ((string)behaviorFrequency == behavior.Frequency)
                {
                    comboBehaviorFrequency.SelectedIndex = comboBehaviorFrequency.Items.IndexOf(behaviorFrequency);
                    break;
                }

            //Search the severity combobox and set its selectedindex to match that of the behavior's severity.
            foreach (Object behaviorSeverity in comboBehaviorSeverity.Items)
                if ((string)behaviorSeverity == behavior.Severity)
                {
                    comboBehaviorSeverity.SelectedIndex = comboBehaviorSeverity.Items.IndexOf(behaviorSeverity);
                    break;
                }
        }

        private void btnAddPhysiologicalCause_Click(object sender, EventArgs e)
        {
            //If the selected node is a behavior and not a child of a behavior, add the physiological cause to the treeview.
            if (treeViewAntecedents.SelectedNode != null && treeViewAntecedents.SelectedNode.Parent == null)
            {
                const string cause = "Physiological Causes";
                TreeNodeCollection behaviorNodes = treeViewAntecedents.SelectedNode.Nodes;
                string selectedCause;

                //If a custom Cause wasn't used, get the cause from the combobox.
                //Else a custom Cause was entered by the user, so get it from the textbox.
                if (comboPhysiologicalCause.SelectedIndex != comboPhysiologicalCause.Items.Count - 1)
                    selectedCause = (string)comboPhysiologicalCause.SelectedItem;
                else
                    selectedCause = txtPhysiologicalCauseOther.Text;

                //If this cause already exists for the specified behavior, check if the cause has already been added.
                //Else the cause doesn't exist for the specified behavior, so add it.
                if (behaviorNodes.ContainsKey(cause))
                {
                    //If the cause has already been added, display an error message to the user and return.
                    if (behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.ContainsKey(selectedCause))
                    {
                        MessageBox.Show("The selected Physiological Cause has already been added.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                    behaviorNodes.Add(cause, cause);

                //Add and show the new node (cause) in the treeview.
                behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.Add(selectedCause, selectedCause);
                treeViewAntecedents.SelectedNode.ExpandAll();

                addToBehaviorsAntecedentsList(cause, selectedCause);
            }
            else
                MessageBox.Show("A behavior needs to be selected before a Physiological Cause can be added to it.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Add to the behavior's list of antecedents.
        /// </summary>
        /// <param name="cause"The type of cause.></param>
        /// <param name="selectedCause">The cause itself.</param>
        private void addToBehaviorsAntecedentsList(string cause, string selectedCause)
        {
            foreach (Behavior behavior in behaviors)
                if (behavior.Name == treeViewAntecedents.SelectedNode.Name)
                {
                    behavior.Antecedents.Add(selectedCause, cause);
                    break;
                }
        }

        private void btnAddEnvironmentalCause_Click(object sender, EventArgs e)
        {
            //If the selected node is a behavior and not a child of a behavior, add the cause to the treeview.
            if (treeViewAntecedents.SelectedNode != null && treeViewAntecedents.SelectedNode.Parent == null)
            {
                const string cause = "Environmental Causes";
                TreeNodeCollection behaviorNodes = treeViewAntecedents.SelectedNode.Nodes;
                string selectedCause;

                //If a custom Cause wasn't used, get the cause from the combobox.
                //Else a custom Cause was entered by the user, so get it from the textbox.
                if (comboEnvironmentalCause.SelectedIndex != comboEnvironmentalCause.Items.Count - 1)
                    selectedCause = (string)comboEnvironmentalCause.SelectedItem;
                else
                    selectedCause = txtEnvironmentalCauseOther.Text;

                //If this cause already exists for the specified behavior, check if the cause has already been added.
                //Else the cause doesn't exist for the specified behavior, so add it.
                if (behaviorNodes.ContainsKey(cause))
                {
                    //If the cause has already been added, display an error message to the user and return.
                    if (behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.ContainsKey(selectedCause))
                    {
                        MessageBox.Show("The selected Environmental Cause has already been added.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                    behaviorNodes.Add(cause, cause);

                //Add and show the new node (cause) in the treeview.
                behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.Add(selectedCause, selectedCause);
                treeViewAntecedents.SelectedNode.ExpandAll();

                addToBehaviorsAntecedentsList(cause, selectedCause);
            }
            else
                MessageBox.Show("A behavior needs to be selected before an Environmental Cause can be added to it.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnAddPsychologicalCause_Click(object sender, EventArgs e)
        {
            //If the selected node is a behavior and not a child of a behavior, add the cause to the treeview.
            if (treeViewAntecedents.SelectedNode != null && treeViewAntecedents.SelectedNode.Parent == null)
            {
                const string cause = "Psychological Causes";
                TreeNodeCollection behaviorNodes = treeViewAntecedents.SelectedNode.Nodes;
                string selectedCause;

                //If a custom Cause wasn't used, get the cause from the combobox.
                //Else a custom Cause was entered by the user, so get it from the textbox.
                if (comboPsychologicalCause.SelectedIndex != comboPsychologicalCause.Items.Count - 1)
                    selectedCause = (string)comboPsychologicalCause.SelectedItem;
                else
                    selectedCause = txtPsychologicalCauseOther.Text;

                //If this cause already exists for the specified behavior, check if the cause has already been added.
                //Else the cause doesn't exist for the specified behavior, so add it.
                if (behaviorNodes.ContainsKey(cause))
                {
                    //If the cause has already been added, display an error message to the user and return.
                    if (behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.ContainsKey(selectedCause))
                    {
                        MessageBox.Show("The selected Psychological Cause has already been added.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                    behaviorNodes.Add(cause, cause);

                //Add and show the new node (cause) in the treeview.
                behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.Add(selectedCause, selectedCause);
                treeViewAntecedents.SelectedNode.ExpandAll();

                addToBehaviorsAntecedentsList(cause, selectedCause);
            }
            else
                MessageBox.Show("A behavior needs to be selected before a Psychological Cause can be added to it.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnAddSocialCause_Click(object sender, EventArgs e)
        {
            //If the selected node is a behavior and not a child of a behavior, add the cause to the treeview.
            if (treeViewAntecedents.SelectedNode != null && treeViewAntecedents.SelectedNode.Parent == null)
            {
                const string cause = "Social Causes";
                TreeNodeCollection behaviorNodes = treeViewAntecedents.SelectedNode.Nodes;
                string selectedCause;

                //If a custom Cause wasn't used, get the cause from the combobox.
                //Else a custom Cause was entered by the user, so get it from the textbox.
                if (comboSocialCause.SelectedIndex != comboSocialCause.Items.Count - 1)
                    selectedCause = (string)comboSocialCause.SelectedItem;
                else
                    selectedCause = txtSocialCauseOther.Text;

                //If this cause already exists for the specified behavior, check if the cause has already been added.
                //Else the cause doesn't exist for the specified behavior, so add it.
                if (behaviorNodes.ContainsKey(cause))
                {
                    //If the cause has already been added, display an error message to the user and return.
                    if (behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.ContainsKey(selectedCause))
                    {
                        MessageBox.Show("The selected Social Cause has already been added.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                    behaviorNodes.Add(cause, cause);

                //Add and show the new node (cause) in the treeview.
                behaviorNodes[behaviorNodes.IndexOfKey(cause)].Nodes.Add(selectedCause, selectedCause);
                treeViewAntecedents.SelectedNode.ExpandAll();

                addToBehaviorsAntecedentsList(cause, selectedCause);
            }
            else
                MessageBox.Show("A behavior needs to be selected before a Social Cause can be added to it.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnRemoveAntecedents_Click(object sender, EventArgs e)
        {
            //If a Cause or Category is selected, allow it to be removed.
            //Else display an error message.
            if (treeViewAntecedents.SelectedNode != null && treeViewAntecedents.SelectedNode.Parent != null)
            {
                //If a Cause is selected, remove it.
                //Else a Category and all its children are being selected to be removed, so display a warning message.
                if (treeViewAntecedents.SelectedNode.Parent.Parent != null)
                {
                    //Remove an antecendent from the behavior's list of antecedents.
                    foreach (Behavior behavior in behaviors)
                        if (behavior.Name == treeViewAntecedents.SelectedNode.Parent.Parent.Name)
                        {
                            behavior.Antecedents.Remove(treeViewAntecedents.SelectedNode.Name);
                            break;
                        }

                    //If the Antecedent category will contain no more Causes after this cause is removed, then remove the category as well as its child.
                    //Else there are other causes within this Antecedent category, so only remove the selected cause.
                    if (treeViewAntecedents.SelectedNode.Parent.Nodes.Count == 1)
                        treeViewAntecedents.SelectedNode.Parent.Remove();
                    else
                        treeViewAntecedents.SelectedNode.Remove();
                }
                else
                {
                    //If the user agrees that he or she would like to remove the antecdent category and all of its causes, do so.
                    //Else do nothing.
                    if (MessageBox.Show("You have selected to remove an Antecedent Category and all of its causes. Press yes if you wish to remove them.",
                        "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        //Remove all the behavior's selected antecendents from the behavior's list of antecedents.
                        foreach (Behavior behavior in behaviors)
                            if (behavior.Name == treeViewAntecedents.SelectedNode.Parent.Name)
                            {
                                foreach (TreeNode node in treeViewAntecedents.SelectedNode.Nodes)
                                    behavior.Antecedents.Remove(node.Name);
                                break;
                            }

                        treeViewAntecedents.SelectedNode.Remove();
                    }
                }
            }
            else
                MessageBox.Show("An Antecedent needs to be selected before it can be removed.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}