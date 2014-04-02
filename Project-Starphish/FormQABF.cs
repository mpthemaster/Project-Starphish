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
        //Whether or not the questions are on the first page.
        private bool firstPage = true;

        private Behavior[] behaviors;

        private Behavior selectedBehavior; //The currently selected behavior from the list of nodes.

        /// <summary>
        /// Creates a new QABF Form.
        /// </summary>
        /// <param name="behaviors">All the behaviors from a staff interview.</param>
        public FormQABF(Behavior[] behaviors)
        {
            InitializeComponent();
            this.behaviors = behaviors;

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
            lblPageNum.Text = "Page 1";
            btnNextPage.Enabled = true;
            btnPreviousPage.Enabled = false;
            showExtraQuestions();
            displayPage1();
        }

        /// <summary>
        /// Displays all of a behavior's QABF questions and answers on page 1.
        /// </summary>
        private void displayPage1()
        {
            QuestionQABF[] questions = selectedBehavior.Qabf.questions;

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
            lblQ2.Text = questions[11].Question;
            lblQ13.Text = questions[12].Question;
            lblQ14.Text = questions[13].Question;
            lblQ15.Text = questions[14].Question;
            lblQ16.Text = questions[15].Question;
            lblQ17.Text = questions[16].Question;
            lblQ18.Text = questions[17].Question;

            comboScore1.SelectedIndex = comboScore1.Items.IndexOf(questions[0].Answer);
            comboScore2.SelectedIndex = comboScore2.Items.IndexOf(questions[1].Answer);
            comboScore3.SelectedIndex = comboScore3.Items.IndexOf(questions[2].Answer);
            comboScore4.SelectedIndex = comboScore4.Items.IndexOf(questions[3].Answer);
            comboScore5.SelectedIndex = comboScore5.Items.IndexOf(questions[4].Answer);
            comboScore6.SelectedIndex = comboScore6.Items.IndexOf(questions[5].Answer);
            comboScore7.SelectedIndex = comboScore7.Items.IndexOf(questions[6].Answer);
            comboScore8.SelectedIndex = comboScore8.Items.IndexOf(questions[7].Answer);
            comboScore9.SelectedIndex = comboScore9.Items.IndexOf(questions[8].Answer);
            comboScore10.SelectedIndex = comboScore10.Items.IndexOf(questions[9].Answer);
            comboScore11.SelectedIndex = comboScore11.Items.IndexOf(questions[10].Answer);
            comboScore12.SelectedIndex = comboScore12.Items.IndexOf(questions[11].Answer);
            comboScore13.SelectedIndex = comboScore13.Items.IndexOf(questions[12].Answer);
            comboScore14.SelectedIndex = comboScore14.Items.IndexOf(questions[13].Answer);
            comboScore15.SelectedIndex = comboScore15.Items.IndexOf(questions[14].Answer);
            comboScore16.SelectedIndex = comboScore16.Items.IndexOf(questions[15].Answer);
            comboScore17.SelectedIndex = comboScore17.Items.IndexOf(questions[16].Answer);
            comboScore18.SelectedIndex = comboScore18.Items.IndexOf(questions[17].Answer);
        }

        /// <summary>
        /// Displays all of a behavior's QABF questions and answers on page 2.
        /// </summary>
        private void displayPage2()
        {
            QuestionQABF[] questions = selectedBehavior.Qabf.questions;

            lblQ1.Text = questions[18].Question;
            lblQ2.Text = questions[19].Question;
            lblQ3.Text = questions[20].Question;
            lblQ4.Text = questions[21].Question;
            lblQ5.Text = questions[22].Question;
            lblQ6.Text = questions[23].Question;
            lblQ7.Text = questions[24].Question;

            comboScore1.SelectedIndex = comboScore1.Items.IndexOf(questions[18].Answer);
            comboScore2.SelectedIndex = comboScore2.Items.IndexOf(questions[19].Answer);
            comboScore3.SelectedIndex = comboScore3.Items.IndexOf(questions[20].Answer);
            comboScore4.SelectedIndex = comboScore4.Items.IndexOf(questions[21].Answer);
            comboScore5.SelectedIndex = comboScore5.Items.IndexOf(questions[22].Answer);
            comboScore6.SelectedIndex = comboScore6.Items.IndexOf(questions[23].Answer);
            comboScore7.SelectedIndex = comboScore7.Items.IndexOf(questions[24].Answer);
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
    }
}