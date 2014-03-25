using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormStaffInterview : Form
    {
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
            comboSocialCause.SelectedIndex = 0;
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
            if (txtBehaviorOther.Enabled && txtBehaviorOther.Text == "")
            {
                MessageBox.Show("You must enter or pick a behavior name.", "Error - Missing Behavior Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}