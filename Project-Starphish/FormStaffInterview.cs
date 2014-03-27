using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormStaffInterview : Form
    {
        //All behaviors that have been added in an interview.
        private List<Behavior> behaviors = new List<Behavior>();

        /// <summary>
        /// Assumes a new staff interview is being created.
        /// </summary>
        public FormStaffInterview()
        {
            InitializeComponent();

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
        }

        /// <summary>
        /// Assumes a staff interview with the specified person is being viewed.
        /// </summary>
        /// <param name="intervieweeName">The interviewee's name.</param>
        public FormStaffInterview(string intervieweeName)
        {
            InitializeComponent();

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

            retrieveInterviewData();
        }

        private void retrieveInterviewData()
        {
        }

        private void loadStrengths()
        {
        }

        private void loadBehaviors()
        {
        }

        private void loadAntecedents()
        {
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

            behaviors.Add(tempBehavior);

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
            }
            else
                MessageBox.Show("A behavior needs to be selected before a Physiological Cause can be added to it.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        treeViewAntecedents.SelectedNode.Remove();
                }
            }
            else
                MessageBox.Show("An Antecedent needs to be selected before it can be removed.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}