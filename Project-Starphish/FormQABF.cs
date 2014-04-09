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
    public partial class FormQABF : Form
    {
        //Whether or not the questions are on the first page. Whether or not all changes made should be saved when the form is closed.
        private bool firstPage = true, saveAll = false;

        private Behavior[] originalBehaviors, behaviors; //The original behaviors stay unchanged unless their copies' (behaviors) changes are saved.

        private Behavior selectedBehavior; //The currently selected behavior from the list of nodes.

        /// <summary>
        /// Creates a new QABF Form.
        /// </summary>
        /// <param name="behaviors">All the behaviors from a staff interview.</param>
        public FormQABF(Behavior[] originalBehavior, string intervieweeName, string formTitle)
        {
            InitializeComponent();
            lblStaffInterviewee.Text = intervieweeName + " Interviewed";
            this.Text = "QABFs - " + formTitle;
            originalBehaviors = originalBehavior;

            //Make a copy of each behavior so that changes aren't automatically saved to them.
            behaviors = new Behavior[originalBehaviors.Length];
            for (int i = 0; i < behaviors.Length; i++)
            {
                behaviors[i] = new Behavior(originalBehaviors[i].Name, originalBehaviors[i].Severity, originalBehaviors[i].Frequency);
                if (originalBehaviors[i].Qabf != null)
                    behaviors[i].Qabf = originalBehaviors[i].Qabf.copy();
            }

            TreeNode uncompletedNode = treeViewQABFs.Nodes[0];
            TreeNode completedNode = treeViewQABFs.Nodes[1];

            //Foreach behavior, check if a QABF for it already exists.
            //  If a QABF exists, check if it has been completed.
            //      If the QABF has been completed, add it to the treeview node for completed lists.
            //      Else the QABF hasn't been completed, so add it to the treeview node for uncompleted lists.
            //  Else a QABF doesn't exist, so create it and add it to the treeview node for uncompleted lists.
            foreach (Behavior behavior in behaviors)
                if (behavior.Qabf != null)
                    if (behavior.Qabf.Completed)
                        treeViewQABFs.Nodes[1].Nodes.Add(behavior.Name, behavior.Name);
                    else
                        uncompletedNode.Nodes.Add(behavior.Name, behavior.Name);
                else
                {
                    behavior.Qabf = new QABF();
                    uncompletedNode.Nodes.Add(behavior.Name, behavior.Name);
                }

            //If there are behaviors with uncompleted QABFS, select the first one in the treeview and expand them all to be seen.
            //Else If there are behaviors with completed QABFS, select the first one in the treeview and expand them all to be seen.
            //Else ....
            if (uncompletedNode.Nodes.Count > 0)
            {
                uncompletedNode.ExpandAll();
                treeViewQABFs.SelectedNode = uncompletedNode.Nodes[0];
            }
            else if (completedNode.Nodes.Count > 0)
            {
                completedNode.ExpandAll();
                treeViewQABFs.SelectedNode = completedNode.Nodes[0];
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            lblPageNum.Text = "Page 2";
            btnNextPage.Enabled = false;
            btnPreviousPage.Enabled = true;
            firstPage = false;
            hideExtraQuestions();
            displayPage2();
        }

        /// <summary>
        /// Hides extra labels and comboboxes for the QABF when the second page is selected.
        /// </summary>
        private void hideExtraQuestions()
        {
            lblQ18.Visible = false;
            lblQ17.Visible = false;
            lblQ16.Visible = false;
            lblQ15.Visible = false;
            lblQ14.Visible = false;
            lblQ13.Visible = false;
            lblQ12.Visible = false;
            lblQ11.Visible = false;
            lblQ10.Visible = false;
            lblQ9.Visible = false;
            lblQ8.Visible = false;

            comboScore18.Visible = false;
            comboScore17.Visible = false;
            comboScore16.Visible = false;
            comboScore15.Visible = false;
            comboScore14.Visible = false;
            comboScore13.Visible = false;
            comboScore12.Visible = false;
            comboScore11.Visible = false;
            comboScore10.Visible = false;
            comboScore9.Visible = false;
            comboScore8.Visible = false;
        }

        /// <summary>
        /// Displays all the labels and comboxes for the QABF when the first page is selected.
        /// </summary>
        private void showExtraQuestions()
        {
            lblQ18.Visible = true;
            lblQ17.Visible = true;
            lblQ16.Visible = true;
            lblQ15.Visible = true;
            lblQ14.Visible = true;
            lblQ13.Visible = true;
            lblQ12.Visible = true;
            lblQ11.Visible = true;
            lblQ10.Visible = true;
            lblQ9.Visible = true;
            lblQ8.Visible = true;

            comboScore18.Visible = true;
            comboScore17.Visible = true;
            comboScore16.Visible = true;
            comboScore15.Visible = true;
            comboScore14.Visible = true;
            comboScore13.Visible = true;
            comboScore12.Visible = true;
            comboScore11.Visible = true;
            comboScore10.Visible = true;
            comboScore9.Visible = true;
            comboScore8.Visible = true;
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            getFirstPageSetUp();
            displayPage1();
        }

        /// <summary>
        /// Makes the page display change from the second page to the first one.
        /// </summary>
        private void getFirstPageSetUp()
        {
            lblPageNum.Text = "Page 1";
            btnNextPage.Enabled = true;
            btnPreviousPage.Enabled = false;
            firstPage = true;
            showExtraQuestions();
        }

        /// <summary>
        /// Displays all of a behavior's QABF questions and answers on page 1.
        /// </summary>
        private void displayPage1()
        {
            QuestionQABF[] questions = selectedBehavior.Qabf.questions;
            string[] answers = selectedBehavior.Qabf.TempAnswers;

            lblQ1.Text = questions[0].Question;
            lblQ2.Text = questions[1].Question;
            lblQ3.Text = questions[2].Question;
            lblQ4.Text = questions[3].Question;
            lblQ5.Text = questions[4].Question;
            lblQ6.Text = questions[5].Question;
            lblQ7.Text = questions[6].Question;
            lblQ8.Text = questions[7].Question;
            lblQ9.Text = questions[8].Question;
            lblQ10.Text = questions[9].Question;
            lblQ11.Text = questions[10].Question;
            lblQ12.Text = questions[11].Question;
            lblQ13.Text = questions[12].Question;
            lblQ14.Text = questions[13].Question;
            lblQ15.Text = questions[14].Question;
            lblQ16.Text = questions[15].Question;
            lblQ17.Text = questions[16].Question;
            lblQ18.Text = questions[17].Question;

            comboScore1.SelectedIndex = comboScore1.Items.IndexOf(answers[0]);
            comboScore2.SelectedIndex = comboScore2.Items.IndexOf(answers[1]);
            comboScore3.SelectedIndex = comboScore3.Items.IndexOf(answers[2]);
            comboScore4.SelectedIndex = comboScore4.Items.IndexOf(answers[3]);
            comboScore5.SelectedIndex = comboScore5.Items.IndexOf(answers[4]);
            comboScore6.SelectedIndex = comboScore6.Items.IndexOf(answers[5]);
            comboScore7.SelectedIndex = comboScore7.Items.IndexOf(answers[6]);
            comboScore8.SelectedIndex = comboScore8.Items.IndexOf(answers[7]);
            comboScore9.SelectedIndex = comboScore9.Items.IndexOf(answers[8]);
            comboScore10.SelectedIndex = comboScore10.Items.IndexOf(answers[9]);
            comboScore11.SelectedIndex = comboScore11.Items.IndexOf(answers[10]);
            comboScore12.SelectedIndex = comboScore12.Items.IndexOf(answers[11]);
            comboScore13.SelectedIndex = comboScore13.Items.IndexOf(answers[12]);
            comboScore14.SelectedIndex = comboScore14.Items.IndexOf(answers[13]);
            comboScore15.SelectedIndex = comboScore15.Items.IndexOf(answers[14]);
            comboScore16.SelectedIndex = comboScore16.Items.IndexOf(answers[15]);
            comboScore17.SelectedIndex = comboScore17.Items.IndexOf(answers[16]);
            comboScore18.SelectedIndex = comboScore18.Items.IndexOf(answers[17]);
        }

        /// <summary>
        /// Displays all of a behavior's QABF questions and answers on page 2.
        /// </summary>
        private void displayPage2()
        {
            QuestionQABF[] questions = selectedBehavior.Qabf.questions;
            string[] answers = selectedBehavior.Qabf.TempAnswers;

            lblQ1.Text = questions[18].Question;
            lblQ2.Text = questions[19].Question;
            lblQ3.Text = questions[20].Question;
            lblQ4.Text = questions[21].Question;
            lblQ5.Text = questions[22].Question;
            lblQ6.Text = questions[23].Question;
            lblQ7.Text = questions[24].Question;

            comboScore1.SelectedIndex = comboScore1.Items.IndexOf(answers[18]);
            comboScore2.SelectedIndex = comboScore2.Items.IndexOf(answers[19]);
            comboScore3.SelectedIndex = comboScore3.Items.IndexOf(answers[20]);
            comboScore4.SelectedIndex = comboScore4.Items.IndexOf(answers[21]);
            comboScore5.SelectedIndex = comboScore5.Items.IndexOf(answers[22]);
            comboScore6.SelectedIndex = comboScore6.Items.IndexOf(answers[23]);
            comboScore7.SelectedIndex = comboScore7.Items.IndexOf(answers[24]);
        }

        private void treeViewQABFs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //If the selected node is a behavior, display its information.
            if (treeViewQABFs.SelectedNode.Parent != null)
            {
                //Foreach behavior in behavior,
                //  If the selected node matches the behavior's name, this is the behavior that we want.
                foreach (Behavior behavior in behaviors)
                    if (behavior.Name == treeViewQABFs.SelectedNode.Name)
                    {
                        selectedBehavior = behavior;
                        lblSelectedBehavior.Text = "Current Selection: " + selectedBehavior.Name;
                        break;
                    }

                //If the first page of QABF questions and answers is showing, then display the selected behavior's page one information.
                //Ese the second page of QABF questions and answers is showing, so display the selected behavior's page one information.
                if (firstPage)
                    displayPage1();
                else
                    displayPage2();
            }
        }

        private void btnSaveOneQABF_Click(object sender, EventArgs e)
        {
            //Save all the temp answers as permenent.
            selectedBehavior.Qabf.saveTempAnswers();

            //If this behavior wasn't saved previously, mark this behavior's QABF as completed, and display this properly in the treeview.
            if (!selectedBehavior.Qabf.Completed)
            {
                selectedBehavior.Qabf.Completed = true;
                string name = selectedBehavior.Name;
                treeViewQABFs.Nodes[1].Nodes.Add(name, name);
                treeViewQABFs.Nodes[0].Nodes.RemoveByKey(selectedBehavior.Name);
                treeViewQABFs.Nodes[1].ExpandAll();
                treeViewQABFs.SelectedNode = treeViewQABFs.Nodes[1].Nodes.Find(name, false)[0]; //Finds the newly created mode to select it.
            }
        }

        private void comboScore1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 1,
            //Else the user is on the second page, so update the temporary answer for question 19.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[0] = (string)comboScore1.SelectedItem;
            else
                selectedBehavior.Qabf.TempAnswers[18] = (string)comboScore1.SelectedItem;
        }

        private void comboScore2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 2,
            //Else the user is on the second page, so update the temporary answer for question 20.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[1] = (string)comboScore2.SelectedItem;
            else
                selectedBehavior.Qabf.TempAnswers[19] = (string)comboScore2.SelectedItem;
        }

        private void comboScore3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 3,
            //Else the user is on the second page, so update the temporary answer for question 21.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[2] = (string)comboScore3.SelectedItem;
            else
                selectedBehavior.Qabf.TempAnswers[20] = (string)comboScore3.SelectedItem;
        }

        private void comboScore4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 4,
            //Else the user is on the second page, so update the temporary answer for question 22.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[3] = (string)comboScore4.SelectedItem;
            else
                selectedBehavior.Qabf.TempAnswers[21] = (string)comboScore4.SelectedItem;
        }

        private void comboScore5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 5,
            //Else the user is on the second page, so update the temporary answer for question 23.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[4] = (string)comboScore5.SelectedItem;
            else
                selectedBehavior.Qabf.TempAnswers[22] = (string)comboScore5.SelectedItem;
        }

        private void comboScore6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 6,
            //Else the user is on the second page, so update the temporary answer for question 24.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[5] = (string)comboScore6.SelectedItem;
            else
                selectedBehavior.Qabf.TempAnswers[23] = (string)comboScore6.SelectedItem;
        }

        private void comboScore7_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 7,
            //Else the user is on the second page, so update the temporary answer for question 25.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[6] = (string)comboScore7.SelectedItem;
            else
                selectedBehavior.Qabf.TempAnswers[24] = (string)comboScore7.SelectedItem;
        }

        private void comboScore8_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 8,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[7] = (string)comboScore8.SelectedItem;
        }

        private void comboScore9_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 9,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[8] = (string)comboScore9.SelectedItem;
        }

        private void comboScore10_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 10,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[9] = (string)comboScore10.SelectedItem;
        }

        private void comboScore11_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 11,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[10] = (string)comboScore11.SelectedItem;
        }

        private void comboScore12_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 12,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[11] = (string)comboScore12.SelectedItem;
        }

        private void comboScore13_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 13,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[12] = (string)comboScore13.SelectedItem;
        }

        private void comboScore14_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 14,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[13] = (string)comboScore14.SelectedItem;
        }

        private void comboScore15_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 15,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[14] = (string)comboScore15.SelectedItem;
        }

        private void comboScore16_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 16,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[15] = (string)comboScore16.SelectedItem;
        }

        private void comboScore17_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 17,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[16] = (string)comboScore17.SelectedItem;
        }

        private void comboScore18_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user is on the first page, update the temporary answer for question 18,
            //Else the user is on the second page, so there is no information to display.
            if (firstPage)
                selectedBehavior.Qabf.TempAnswers[17] = (string)comboScore18.SelectedItem;
        }

        private void btnClearTempChanges_Click(object sender, EventArgs e)
        {
            //If the user is sure that he wants to undo all the QABF data changes since the last save for a specific behavior, then do that.
            //Else do nothing.
            if (MessageBox.Show("Are you sure you want undo all of your QABF data changes since your last save for " + selectedBehavior.Name + "?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                //Resets all temp answers to a behavior's QABF saved answers and updates this to the screen.
                selectedBehavior.Qabf.resetTempAnswers();

                //If the second page of QABF questions and answers is showing, then make the first page display.
                if (!firstPage)
                    getFirstPageSetUp();
                displayPage1();
            }
        }

        private void btnResetQABF_Click(object sender, EventArgs e)
        {
            //If the user is sure that he wants to remove all the QABF data for a specific behavior, then do that.
            //Else do nothing.
            if (MessageBox.Show("Are you sure you want erase all of your QABF data for " + selectedBehavior.Name + "?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                selectedBehavior.Qabf.reset();

                //If the behavior's QABF has previously beens saved, change the node's parent and set its status to uncomplete.
                if (selectedBehavior.Qabf.Completed)
                {
                    selectedBehavior.Qabf.Completed = false;

                    string name = selectedBehavior.Name;
                    treeViewQABFs.Nodes[0].Nodes.Add(name, name);
                    treeViewQABFs.Nodes[1].Nodes.RemoveByKey(selectedBehavior.Name);
                    treeViewQABFs.Nodes[0].ExpandAll();
                    treeViewQABFs.SelectedNode = treeViewQABFs.Nodes[0].Nodes.Find(name, false)[0]; //Finds the newly created mode to select it.
                }

                //If the second page of QABF questions and answers is showing, then make the first page display.
                if (!firstPage)
                    getFirstPageSetUp();
                displayPage1();
            }
        }

        private void FormQABF_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool unsavedModification = false;

            //If the user pressed the exit box, check if there are any unsaved changes.
            if (!saveAll)
                for (int i = 0; i < behaviors.Length; i++)
                    if (behaviors[i].Qabf.isModified() || (originalBehaviors[i].Qabf != null && behaviors[i].Qabf.Completed != originalBehaviors[i].Qabf.Completed))
                    {
                        unsavedModification = true;
                        break;
                    }

            if (unsavedModification && unsavedModification && MessageBox.Show("Are you sure you want to leave? You will lose all unsaved data.", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                e.Cancel = true;
        }

        private void btnSaveQABFs_Click(object sender, EventArgs e)
        {
            //Save any unsaved modifications to the copies and then to the actual behaviors from the copies.
            for (int i = 0; i < behaviors.Length; i++)
            {
                if (behaviors[i].Qabf.isModified())
                {
                    behaviors[i].Qabf.Completed = true;
                    behaviors[i].Qabf.saveTempAnswers();
                }
                originalBehaviors[i].Qabf = behaviors[i].Qabf;
            }
            saveAll = true;
            this.Close();
        }
    }
}