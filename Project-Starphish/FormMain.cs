using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GUI
{
    public partial class FormMain : Form
    {
        private string theConnectionString;
        private string insertStatement;
        private SqlConnection connection;
        private SqlCommand command;

        public FormMain()
        {
            InitializeComponent();
            theConnectionString = "Data Source=localhost\\PROJECTSTARPHISH;Initial Catalog=ProjectStarphish;Integrated Security=True";
            //UPDATE PERSON SET  FNAME = @FNAME, //For updating an existing person.
            insertStatement = "INSERT INTO PERSON (FNAME, MNAME, LNAME, IDENTIFYING_MARKS, PHOTO, AGENCY_NAME, P_ADDRESS, PHONE, ADMITTANCE_DATE, DATE_OF_BIRTH, AGE, GENDER, RACE, HAIR_COLOR, HEIGHT, P_WEIGHT, BSU, MCI, INSURANCE_CARRIER, POLICY_NUM, MANAGED_CARE_COMPANY, SSN) VALUES        (@FNAME, @MNAME, @LNAME, @IDENTIFYING_MARKS, @PHOTO, @AGENCY_NAME, @P_ADDRESS, @PHONE, @ADMITTANCE_DATE, @DATE_OF_BIRTH, @AGE, @GENDER, @RACE, @HAIR_COLOR, @HEIGHT, @P_WEIGHT, @BSU, @MCI, @INSURANCE_CARRIER, @POLICY_NUM, @MANAGED_CARE_COMPANY, @SSN)";
            connection = new SqlConnection(theConnectionString);
            command = new SqlCommand(insertStatement, connection);
        }

        /// <summary>
        /// Opens up a blank Staff Interview form.
        /// </summary>
        private void btnAddInterview_Click(object sender, EventArgs e)
        {
            FormStaffInterview staffInterview = new FormStaffInterview(1);
            staffInterview.Show();
        }

        private void btnSaveClient_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            Image image = new Bitmap("test.bmp");
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            connection.Open();
            command.Parameters.AddWithValue("@FNAME", txtFirstName.Text);
            command.Parameters.AddWithValue("@MNAME", txtMiddleName.Text);
            command.Parameters.AddWithValue("@LNAME", txtLastName.Text);
            command.Parameters.AddWithValue("@IDENTIFYING_MARKS", txtIdentifyingMarks.Text);
            command.Parameters.AddWithValue("@PHOTO", ms.ToArray());
            command.Parameters.AddWithValue("@AGENCY_NAME", txtAgencyName.Text);
            command.Parameters.AddWithValue("@P_ADDRESS", txtAddress.Text);
            command.Parameters.AddWithValue("@PHONE", txtTelephoneNum.Text);
            command.Parameters.AddWithValue("@ADMITTANCE_DATE", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@DATE_OF_BIRTH", txtDateOfBirth.Text);
            command.Parameters.AddWithValue("@AGE", txtAge.Text);
            command.Parameters.AddWithValue("@GENDER", comboGender.Text);
            command.Parameters.AddWithValue("@RACE", comboRace.Text);
            command.Parameters.AddWithValue("@HAIR_COLOR", txtHairColor.Text);
            command.Parameters.AddWithValue("@HEIGHT", txtHeight.Text);
            command.Parameters.AddWithValue("@P_WEIGHT", txtWeight.Text);
            command.Parameters.AddWithValue("@BSU", txtBSUNum.Text);
            command.Parameters.AddWithValue("@MCI", txtMCINum.Text);
            command.Parameters.AddWithValue("@INSURANCE_CARRIER", txtInsuranceCarrier.Text);
            command.Parameters.AddWithValue("@POLICY_NUM", txtPolicyNum.Text);
            command.Parameters.AddWithValue("@MANAGED_CARE_COMPANY", txtFirstName.Text);
            command.Parameters.AddWithValue("@SSN", txtSocialSecurityNum.Text);

            command.ExecuteNonQuery();
            connection.Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectStarphishDataSet.EMERGENCY_CONTACT' table. You can move, or remove it, as needed.
            this.eMERGENCY_CONTACTTableAdapter.Fill(this.projectStarphishDataSet.EMERGENCY_CONTACT);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.NEXT_OF_KIN' table. You can move, or remove it, as needed.
            this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.COUNTY_RESPONSIBLE' table. You can move, or remove it, as needed.
            this.cOUNTY_RESPONSIBLETableAdapter.Fill(this.projectStarphishDataSet.COUNTY_RESPONSIBLE);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.NEW_LIGHT_SUPPORT' table. You can move, or remove it, as needed.
            this.nEW_LIGHT_SUPPORTTableAdapter.Fill(this.projectStarphishDataSet.NEW_LIGHT_SUPPORT);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.PERSON' table. You can move, or remove it, as needed.
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
        }

        private void addClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
            pERSONBindingSource.MoveLast();
        }

        private void listClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            //it won't compile without this now. Woo.
        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            // if (tabControl1.SelectedIndex == 1)
            if (tabControl1.SelectedIndex == 2)
                mainGraph();
            //if (tabControl1.SelectedIndex == 3)
        }

        private void btnViewInterview_Click(object sender, EventArgs e)
        {
            FormStaffInterview staffInterview = new FormStaffInterview(1, "name");
            staffInterview.Show();
        }

        private void btnRemoveInterview_Click(object sender, EventArgs e)
        {
        }

        private void lstInterviews_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dataGridViewBehaviorsStaffInterviews_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void removeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            //If the user doesn't cancel the image file selection, save it and display it picbox.
            if (dialogFileOpenImage.ShowDialog() != DialogResult.Cancel)
                ;
        }

        private void btnAddISP_Click(object sender, EventArgs e)
        {
            //If the user doesn't cancel the ISP file selection, save it and display it in the listbox.
            if (dialogFileOpenImage.ShowDialog() != DialogResult.Cancel)
                ;
        }

        private void btnViewISP_Click(object sender, EventArgs e)
        {
        }

        private void btnRemoveISP_Click(object sender, EventArgs e)
        {
        }

        private void btnRemoveNextOfKin_Click(object sender, EventArgs e)
        {
        }

        private void btnAddNextOfKin_Click(object sender, EventArgs e)
        {
        }

        private void btnRemoveEmergencyContact_Click(object sender, EventArgs e)
        {
        }

        private void btnAddEmergencyContact_Click(object sender, EventArgs e)
        {
        }
    }
}