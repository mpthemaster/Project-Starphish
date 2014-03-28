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
        private int personId;
        private string theConnectionString;
        private string insertStatement;
        private string insertStatementNLS;
        private string insertStatementCR;
        private string updateStatement;
        private string updateStatementNLS;
        private string updateStatementCR;
        private SqlConnection connection;
        private SqlCommand command;
        private SqlCommand commandNLS;
        private SqlCommand commandCR;
        private SqlCommand commandUpdate;
        private SqlCommand commandNLSUpdate;
        private SqlCommand commandCRUpdate;

        public FormMain()
        {
            InitializeComponent();
            theConnectionString = "Data Source=localhost\\PROJECTSTARPHISH;Initial Catalog=ProjectStarphish;Integrated Security=True";
            //UPDATE PERSON SET  FNAME = @FNAME, //For updating an existing person.
            insertStatement = "INSERT INTO PERSON (FNAME, MNAME, LNAME, IDENTIFYING_MARKS, PHOTO, AGENCY_NAME, P_ADDRESS, ZIP, PHONE, ADMITTANCE_DATE, DATE_OF_BIRTH, AGE, GENDER, RACE, HAIR_COLOR, HEIGHT, P_WEIGHT, BSU, MCI, INSURANCE_CARRIER, POLICY_NUM, MANAGED_CARE_COMPANY, SSN) VALUES        (@FNAME, @MNAME, @LNAME, @IDENTIFYING_MARKS, @PHOTO, @AGENCY_NAME, @P_ADDRESS, @ZIP, @PHONE, @ADMITTANCE_DATE, @DATE_OF_BIRTH, @AGE, @GENDER, @RACE, @HAIR_COLOR, @HEIGHT, @P_WEIGHT, @BSU, @MCI, @INSURANCE_CARRIER, @POLICY_NUM, @MANAGED_CARE_COMPANY, @SSN)";
            insertStatementNLS = "INSERT INTO NEW_LIGHT_SUPPORT (PERSON_ID, SITE_SUPERVISOR_NAME, SITE_SUPERVISOR_PHONE, PROGRAM_COORDINATOR_NAME, PROGRAM_COORDINATOR_PHONE, PROGRAM_SPECIALIST_NAME, PROGRAM_SPECIALIST_PHONE) VALUES        (@PERSON_ID, @SITE_SUPERVISOR_NAME, @SITE_SUPERVISOR_PHONE, @PROGRAM_COORDINATOR_NAME, @PROGRAM_COORDINATOR_PHONE, @PROGRAM_SPECIALIST_NAME, @PROGRAM_SPECIALIST_PHONE)";
            insertStatementCR = "INSERT INTO COUNTY_RESPONSIBLE (PERSON_ID, COUNTY_NAME, Supports_Coordinator_Name, Supports_Coordinator_Address, SC_PHONE) VALUES        (@PERSON_ID, @COUNTY_NAME, @Supports_Coordinator_Name, @Supports_Coordinator_Address, @SC_PHONE)";
            updateStatement = "UPDATE PERSON SET FNAME = @FNAME, MNAME = @MNAME, LNAME = @LNAME, IDENTIFYING_MARKS = @IDENTIFYING_MARKS, PHOTO = @PHOTO, AGENCY_NAME = @AGENCY_NAME, P_ADDRESS = @P_ADDRESS, ZIP = @ZIP, PHONE = @PHONE, ADMITTANCE_DATE = @ADMITTANCE_DATE, DATE_OF_BIRTH = @DATE_OF_BIRTH, AGE = @AGE, GENDER = @GENDER, RACE = @RACE, HAIR_COLOR = @HAIR_COLOR, HEIGHT = @HEIGHT, P_WEIGHT = @P_WEIGHT, BSU = @BSU, MCI = @MCI, INSURANCE_CARRIER = @INSURANCE_CARRIER, POLICY_NUM = @POLICY_NUM, MANAGED_CARE_COMPANY = @MANAGED_CARE_COMPANY WHERE SSN = @SSN";
            updateStatementNLS = "UPDATE NEW_LIGHT_SUPPORT SET SITE_SUPERVISOR_NAME = @SITE_SUPERVISOR_NAME, SITE_SUPERVISOR_PHONE = @SITE_SUPERVISOR_PHONE, PROGRAM_COORDINATOR_NAME = @PROGRAM_COORDINATOR_NAME, PROGRAM_COORDINATOR_PHONE = @PROGRAM_COORDINATOR_PHONE, PROGRAM_SPECIALIST_NAME = @PROGRAM_SPECIALIST_NAME, PROGRAM_SPECIALIST_PHONE = @PROGRAM_SPECIALIST_PHONE WHERE PERSON_ID = @PERSON_ID";
            updateStatementCR = "UPDATE COUNTY_RESPONSIBLE SET COUNTY_NAME = @COUNTY_NAME, Supports_Coordinator_Name = @Supports_Coordinator_Name, Supports_Coordinator_Address = @Supports_Coordinator_Address, SC_PHONE = @SC_PHONE WHERE PERSON_ID = @PERSON_ID";
            connection = new SqlConnection(theConnectionString);
            command = new SqlCommand(insertStatement, connection);
            commandNLS = new SqlCommand(insertStatementNLS, connection);
            commandCR = new SqlCommand(insertStatementCR, connection);
            commandUpdate = new SqlCommand(updateStatement, connection);
            commandNLSUpdate = new SqlCommand(updateStatementNLS, connection);
            commandCRUpdate = new SqlCommand(updateStatementCR, connection);
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
            command.Parameters.AddWithValue("@ZIP", txtZipCode.Text);
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
            command.Parameters.AddWithValue("@MANAGED_CARE_COMPANY", txtManagedCareCompany.Text);
            command.Parameters.AddWithValue("@SSN", txtSocialSecurityNum.Text);

            commandNLS.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandNLS.Parameters.AddWithValue("@SITE_SUPERVISOR_NAME", txtSiteSupervisorName.Text);
            commandNLS.Parameters.AddWithValue("@SITE_SUPERVISOR_PHONE", txtSiteSupervisorTelephoneNum.Text);
            commandNLS.Parameters.AddWithValue("@PROGRAM_COORDINATOR_NAME", txtProgramCoordinatorName.Text);
            commandNLS.Parameters.AddWithValue("@PROGRAM_COORDINATOR_PHONE", txtProgramCoordinatorTelephoneNum.Text);
            commandNLS.Parameters.AddWithValue("@PROGRAM_SPECIALIST_NAME", txtProgramSpecialistName.Text);
            commandNLS.Parameters.AddWithValue("@PROGRAM_SPECIALIST_PHONE", txtProgramSpecialistPhoneNum.Text);

            commandCR.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandCR.Parameters.AddWithValue("@COUNTY_NAME", txtCountyResponsible.Text);
            commandCR.Parameters.AddWithValue("@Supports_Coordinator_Name", txtSupportsCoordinatorName.Text);
            commandCR.Parameters.AddWithValue("@Supports_Coordinator_Address", txtSupportsCoordinatorAddress.Text);
            commandCR.Parameters.AddWithValue("@SC_PHONE", txtSupportsCoordinatorTelephoneNum.Text);

            command.ExecuteNonQuery();
            commandNLS.ExecuteNonQuery();
            commandCR.ExecuteNonQuery();

            connection.Close();
            revertForm();
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
            clearForm();
            listClients.ClearSelected();
            listClients.Enabled = false;
            txtSearchClient.Enabled = false;
            btnRemoveNextOfKin.Enabled = false;
            btnAddNextOfKin.Enabled = false;
            lstNextOfKin.Enabled = false;
            lstISP.Enabled = false;
            lstEmergencyContacts.Enabled = false;
            txtNextOfKinName.Enabled = false;
            txtNextOfKinTelephoneNum.Enabled = false;
            txtNextOfKinAddress.Enabled = false;
            btnRemoveEmergencyContact.Enabled = false;
            btnAddEmergencyContact.Enabled = false;
            txtEmergencyContactName.Enabled = false;
            txtEmergencyContactTelephoneNum.Enabled = false;
            txtEmergencyContactAddress.Enabled = false;
            btnAddISP.Enabled = false;
            btnViewISP.Enabled = false;
            btnRemoveISP.Enabled = false;
            btnSaveClient.Enabled = true;
            btnModifyClient.Enabled = false;
            btnCancel.Enabled = true;
            txtSocialSecurityNum.Enabled = true;
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
            if (tabControl1.SelectedIndex == 3)
                mainStaffInterview();
        }

        private void btnModifyClient_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            Image image = new Bitmap("test.bmp");
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            connection.Open();
            commandUpdate.Parameters.AddWithValue("@FNAME", txtFirstName.Text);
            commandUpdate.Parameters.AddWithValue("@MNAME", txtMiddleName.Text);
            commandUpdate.Parameters.AddWithValue("@LNAME", txtLastName.Text);
            commandUpdate.Parameters.AddWithValue("@IDENTIFYING_MARKS", txtIdentifyingMarks.Text);
            commandUpdate.Parameters.AddWithValue("@PHOTO", ms.ToArray());
            commandUpdate.Parameters.AddWithValue("@AGENCY_NAME", txtAgencyName.Text);
            commandUpdate.Parameters.AddWithValue("@P_ADDRESS", txtAddress.Text);
            commandUpdate.Parameters.AddWithValue("@ZIP", txtZipCode.Text);
            commandUpdate.Parameters.AddWithValue("@PHONE", txtTelephoneNum.Text);
            commandUpdate.Parameters.AddWithValue("@ADMITTANCE_DATE", txtAdmittanceDate.Text);
            commandUpdate.Parameters.AddWithValue("@DATE_OF_BIRTH", txtDateOfBirth.Text);
            commandUpdate.Parameters.AddWithValue("@AGE", txtAge.Text);
            commandUpdate.Parameters.AddWithValue("@GENDER", comboGender.Text);
            commandUpdate.Parameters.AddWithValue("@RACE", comboRace.Text);
            commandUpdate.Parameters.AddWithValue("@HAIR_COLOR", txtHairColor.Text);
            commandUpdate.Parameters.AddWithValue("@HEIGHT", txtHeight.Text);
            commandUpdate.Parameters.AddWithValue("@P_WEIGHT", txtWeight.Text);
            commandUpdate.Parameters.AddWithValue("@BSU", txtBSUNum.Text);
            commandUpdate.Parameters.AddWithValue("@MCI", txtMCINum.Text);
            commandUpdate.Parameters.AddWithValue("@INSURANCE_CARRIER", txtInsuranceCarrier.Text);
            commandUpdate.Parameters.AddWithValue("@POLICY_NUM", txtPolicyNum.Text);
            commandUpdate.Parameters.AddWithValue("@MANAGED_CARE_COMPANY", txtManagedCareCompany.Text);
            commandUpdate.Parameters.AddWithValue("@SSN", txtSocialSecurityNum.Text);

            commandNLSUpdate.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandNLSUpdate.Parameters.AddWithValue("@SITE_SUPERVISOR_NAME", txtSiteSupervisorName.Text);
            commandNLSUpdate.Parameters.AddWithValue("@SITE_SUPERVISOR_PHONE", txtSiteSupervisorTelephoneNum.Text);
            commandNLSUpdate.Parameters.AddWithValue("@PROGRAM_COORDINATOR_NAME", txtProgramCoordinatorName.Text);
            commandNLSUpdate.Parameters.AddWithValue("@PROGRAM_COORDINATOR_PHONE", txtProgramCoordinatorTelephoneNum.Text);
            commandNLSUpdate.Parameters.AddWithValue("@PROGRAM_SPECIALIST_NAME", txtProgramSpecialistName.Text);
            commandNLSUpdate.Parameters.AddWithValue("@PROGRAM_SPECIALIST_PHONE", txtProgramSpecialistPhoneNum.Text);

            commandCRUpdate.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandCRUpdate.Parameters.AddWithValue("@COUNTY_NAME", txtCountyResponsible.Text);
            commandCRUpdate.Parameters.AddWithValue("@Supports_Coordinator_Name", txtSupportsCoordinatorName.Text);
            commandCRUpdate.Parameters.AddWithValue("@Supports_Coordinator_Address", txtSupportsCoordinatorAddress.Text);
            commandCRUpdate.Parameters.AddWithValue("@SC_PHONE", txtSupportsCoordinatorTelephoneNum.Text);

            commandUpdate.ExecuteNonQuery();
            commandNLSUpdate.ExecuteNonQuery();
            commandCRUpdate.ExecuteNonQuery();

            connection.Close();
        }

        private void clearForm()
        {
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtAgencyName.Text = "";
            txtAdmittanceDate.Text = "";
            txtAddress.Text = "";
            txtZipCode.Text = "";
            txtTelephoneNum.Text = "";
            txtSocialSecurityNum.Text = "";
            txtDateOfBirth.Text = "";
            txtAge.Text = "";
            comboGender.SelectedIndex = 0;
            comboRace.SelectedIndex = 0;
            txtRaceOther.Text = "";
            txtRaceOther.Enabled = false;
            txtRaceOther.Visible = false;
            txtHairColor.Text = "";
            txtHeight.Text = "";
            txtWeight.Text = "";
            txtIdentifyingMarks.Text = "";
            txtBSUNum.Text = "";
            txtMCINum.Text = "";
            txtInsuranceCarrier.Text = "";
            txtPolicyNum.Text = "";
            txtManagedCareCompany.Text = "";
            picClient.Image = null;
            txtSiteSupervisorName.Text = "";
            txtSiteSupervisorTelephoneNum.Text = "";
            txtProgramCoordinatorName.Text = "";
            txtProgramCoordinatorTelephoneNum.Text = "";
            txtProgramSpecialistName.Text = "";
            txtProgramSpecialistPhoneNum.Text = "";
            txtCountyResponsible.Text = "";
            txtSupportsCoordinatorName.Text = "";
            txtSupportsCoordinatorAddress.Text = "";
            txtSupportsCoordinatorTelephoneNum.Text = "";
        }

        private void btnViewInterview_Click(object sender, EventArgs e)
        {
            FormStaffInterview staffInterview = new FormStaffInterview(1, "Michael", DateTime.Today);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will delete all unsaved changes. Are you sure you wish to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                revertForm();
            }
        }

        private void revertForm()
        {
            clearForm();
            listClients.ClearSelected();
            listClients.Enabled = true;
            txtSearchClient.Enabled = true;
            btnRemoveNextOfKin.Enabled = true;
            btnAddNextOfKin.Enabled = true;
            lstNextOfKin.Enabled = true;
            lstISP.Enabled = true;
            lstEmergencyContacts.Enabled = true;
            txtNextOfKinName.Enabled = true;
            txtNextOfKinTelephoneNum.Enabled = true;
            txtNextOfKinAddress.Enabled = true;
            btnRemoveEmergencyContact.Enabled = true;
            btnAddEmergencyContact.Enabled = true;
            txtEmergencyContactName.Enabled = true;
            txtEmergencyContactTelephoneNum.Enabled = true;
            txtEmergencyContactAddress.Enabled = true;
            btnAddISP.Enabled = true;
            btnViewISP.Enabled = true;
            btnRemoveISP.Enabled = true;
            btnSaveClient.Enabled = false;
            btnModifyClient.Enabled = true;
            btnCancel.Enabled = false;
            txtSocialSecurityNum.Enabled = false;
        }
    }
}