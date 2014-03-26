using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public partial class FormMain : Form
    {
        bool firstTime = true;

        /// <summary>
        /// The function that starts the graphing tab
        /// </summary>
        private void mainGraph()
        {
            if (firstTime)
            {
                //Setting the two combo boxes in the graph tab to have default selections
                comboPickTimeGraphs.SelectedIndex = 0;
                comboBehaviorsToGraph.SelectedIndex = 1;

                //Only the combo box that selects the date is enabled by default
                everythingDisabled();
                comboPickTimeGraphs.Enabled = true;

                //likewise, the list box for custom behaviors is disabled by default
                listBehaviorsToGraph.Enabled = false;

                createGraphs();
                firstTime = false;
            }
            
        }

        /// <summary>
        /// Checks to see which behaviors are selected to be graphed, how far 
        /// back the graph should go, and then calls the function to graph them
        /// </summary>
        private void getGraphRange()
        {
            if (radUseTimeFrames.Checked)
            {
                if (comboPickTimeGraphs.SelectedIndex == 0)//last month
                {
                    // MessageBox.Show("hbfdt");
                }
                else if (comboPickTimeGraphs.SelectedIndex == 1)//last 60 days
                {

                }
                else if (comboPickTimeGraphs.SelectedIndex == 2)//last quarter
                {
                }
            }
            else if (radUseCustomDates.Checked)
            {
                DateTime result = datePickerBeginGraphs.Value.Date;
                MessageBox.Show(result.ToShortDateString());
            }
            else if (radUseCustomQuarters.Checked)
            {
                if (chkQuarter1.Checked)
                {
                }
                if (chkQuarter2.Checked)
                {
                }
                if (chkQuarter3.Checked)
                {
                }
                if (chkQuarter4.Checked)
                {
                }
            }
        }

        private void createGraphs()
        {
        }

        private void radUseTimeFrames_Click(object sender, EventArgs e)
        {
            everythingDisabled();
            comboPickTimeGraphs.Enabled = true;
        }

        private void radUseCustomDates_Click(object sender, EventArgs e)
        {
            everythingDisabled();
            datePickerBeginGraphs.Enabled = true;
            datePickerEndGraphs.Enabled = true;
        }

        private void radUseCustomQuarters_Click(object sender, EventArgs e)
        {
            everythingDisabled();
            chkQuarter1.Enabled = true;
            chkQuarter2.Enabled = true;
            chkQuarter3.Enabled = true;
            chkQuarter4.Enabled = true;
        }

        /// <summary>
        /// Sets all the graph options to be disabled so that they don't need to be written out like that
        /// specifically every time a radio button selection changes
        /// </summary>
        private void everythingDisabled()
        {
            comboPickTimeGraphs.Enabled = false;
            chkQuarter1.Enabled = false;
            chkQuarter2.Enabled = false;
            chkQuarter3.Enabled = false;
            chkQuarter4.Enabled = false;
            datePickerBeginGraphs.Enabled = false;
            datePickerEndGraphs.Enabled = false;
        }

        private void btnGenerateGraphs_Click(object sender, EventArgs e)
        {
            getGraphRange();
        }

        private void comboBehaviorsToGraph_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBehaviorsToGraph.SelectedIndex == 0)
            {
                listBehaviorsToGraph.Enabled = false;
                listBehaviorsToGraph.ClearSelected();
            }
            if (comboBehaviorsToGraph.SelectedIndex == 1)
            {
                listBehaviorsToGraph.Enabled = false;
                listBehaviorsToGraph.ClearSelected();
            }
            if (comboBehaviorsToGraph.SelectedIndex == 2)
                listBehaviorsToGraph.Enabled = true;
        }
    }
}
