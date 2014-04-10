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
        public static string searchName;
        private string theConnectionString;
        private string insertStatement;
        private string insertNOK;
        private string deleteNOK;
        private string insertEC;
        private string deleteEC;
        private string insertStatementNLS;
        private string insertStatementCR;
        private string updateStatement;
        private string updateStatementNLS;
        private string updateStatementCR;
        private string updateStatementNoPic;
        private string deleteStatement;
        private string getPic;
        private string addISP;
        private string deleteISP;
        private string selectISP;
        private string selectBehaviors;
        private string search;
        private SqlConnection connection;
        private SqlCommand command;
        private SqlCommand commandNoPic;
        private SqlCommand commandInsertNOK;
        private SqlCommand commandDeleteNOK;
        private SqlCommand commandInsertEC;
        private SqlCommand commandDeleteEC;
        private SqlCommand commandNLS;
        private SqlCommand commandCR;
        private SqlCommand commandUpdate;
        private SqlCommand commandNLSUpdate;
        private SqlCommand commandCRUpdate;
        private SqlCommand commandDelete;
        private SqlCommand commandGetPic;
        private SqlCommand commandAddISP;
        private SqlCommand commandDeleteISP;
        private SqlCommand commandViewISP;
        private SqlCommand commandSearch;
        private SqlCommand commandSelectBehaviors;
        private MemoryStream ms = new MemoryStream();
        private int x;
        private bool pdfMade = false;
        private int tempFilesCount = 0;
        private bool searched = false;
        private bool changedPic = false;

        public FormMain()
        {
            InitializeComponent();
            theConnectionString = "Data Source=localhost\\PROJECTSTARPHISH;Initial Catalog=ProjectStarphish;Integrated Security=True";
            insertStatement = "INSERT INTO PERSON (FNAME, MNAME, LNAME, IDENTIFYING_MARKS, PHOTO, AGENCY_NAME, P_ADDRESS, ZIP, PHONE, ADMITTANCE_DATE, DATE_OF_BIRTH, AGE, GENDER, RACE, HAIR_COLOR, HEIGHT, P_WEIGHT, BSU, MCI, INSURANCE_CARRIER, POLICY_NUM, MANAGED_CARE_COMPANY, SSN) VALUES        (@FNAME, @MNAME, @LNAME, @IDENTIFYING_MARKS, @PHOTO, @AGENCY_NAME, @P_ADDRESS, @ZIP, @PHONE, @ADMITTANCE_DATE, @DATE_OF_BIRTH, @AGE, @GENDER, @RACE, @HAIR_COLOR, @HEIGHT, @P_WEIGHT, @BSU, @MCI, @INSURANCE_CARRIER, @POLICY_NUM, @MANAGED_CARE_COMPANY, @SSN)";
            insertStatementNLS = "INSERT INTO NEW_LIGHT_SUPPORT (PERSON_ID, SITE_SUPERVISOR_NAME, SITE_SUPERVISOR_PHONE, PROGRAM_COORDINATOR_NAME, PROGRAM_COORDINATOR_PHONE, PROGRAM_SPECIALIST_NAME, PROGRAM_SPECIALIST_PHONE) VALUES        (@PERSON_ID, @SITE_SUPERVISOR_NAME, @SITE_SUPERVISOR_PHONE, @PROGRAM_COORDINATOR_NAME, @PROGRAM_COORDINATOR_PHONE, @PROGRAM_SPECIALIST_NAME, @PROGRAM_SPECIALIST_PHONE)";
            insertStatementCR = "INSERT INTO COUNTY_RESPONSIBLE (PERSON_ID, COUNTY_NAME, Supports_Coordinator_Name, Supports_Coordinator_Address, SC_PHONE) VALUES        (@PERSON_ID, @COUNTY_NAME, @Supports_Coordinator_Name, @Supports_Coordinator_Address, @SC_PHONE)";
            updateStatement = "UPDATE PERSON SET FNAME = @FNAME, MNAME = @MNAME, LNAME = @LNAME, IDENTIFYING_MARKS = @IDENTIFYING_MARKS, PHOTO = @PHOTO, AGENCY_NAME = @AGENCY_NAME, P_ADDRESS = @P_ADDRESS, ZIP = @ZIP, PHONE = @PHONE, ADMITTANCE_DATE = @ADMITTANCE_DATE, DATE_OF_BIRTH = @DATE_OF_BIRTH, AGE = @AGE, GENDER = @GENDER, RACE = @RACE, HAIR_COLOR = @HAIR_COLOR, HEIGHT = @HEIGHT, P_WEIGHT = @P_WEIGHT, BSU = @BSU, MCI = @MCI, INSURANCE_CARRIER = @INSURANCE_CARRIER, POLICY_NUM = @POLICY_NUM, MANAGED_CARE_COMPANY = @MANAGED_CARE_COMPANY WHERE SSN = @SSN";
            updateStatementNoPic = "UPDATE PERSON SET FNAME = @FNAME, MNAME = @MNAME, LNAME = @LNAME, IDENTIFYING_MARKS = @IDENTIFYING_MARKS, AGENCY_NAME = @AGENCY_NAME, P_ADDRESS = @P_ADDRESS, ZIP = @ZIP, PHONE = @PHONE, ADMITTANCE_DATE = @ADMITTANCE_DATE, DATE_OF_BIRTH = @DATE_OF_BIRTH, AGE = @AGE, GENDER = @GENDER, RACE = @RACE, HAIR_COLOR = @HAIR_COLOR, HEIGHT = @HEIGHT, P_WEIGHT = @P_WEIGHT, BSU = @BSU, MCI = @MCI, INSURANCE_CARRIER = @INSURANCE_CARRIER, POLICY_NUM = @POLICY_NUM, MANAGED_CARE_COMPANY = @MANAGED_CARE_COMPANY WHERE SSN = @SSN";
            updateStatementNLS = "UPDATE NEW_LIGHT_SUPPORT SET SITE_SUPERVISOR_NAME = @SITE_SUPERVISOR_NAME, SITE_SUPERVISOR_PHONE = @SITE_SUPERVISOR_PHONE, PROGRAM_COORDINATOR_NAME = @PROGRAM_COORDINATOR_NAME, PROGRAM_COORDINATOR_PHONE = @PROGRAM_COORDINATOR_PHONE, PROGRAM_SPECIALIST_NAME = @PROGRAM_SPECIALIST_NAME, PROGRAM_SPECIALIST_PHONE = @PROGRAM_SPECIALIST_PHONE WHERE PERSON_ID = @PERSON_ID";
            updateStatementCR = "UPDATE COUNTY_RESPONSIBLE SET COUNTY_NAME = @COUNTY_NAME, Supports_Coordinator_Name = @Supports_Coordinator_Name, Supports_Coordinator_Address = @Supports_Coordinator_Address, SC_PHONE = @SC_PHONE WHERE PERSON_ID = @PERSON_ID";
            insertNOK = "INSERT INTO NEXT_OF_KIN (PERSON_ID, NAME, NOK_ADDRESS, PHONE)VALUES (@PERSON_ID, @NAME, @NOK_ADDRESS, @PHONE)";
            deleteNOK = "DELETE FROM NEXT_OF_KIN WHERE PERSON_ID = @PERSON_ID AND UNIQUEID = @UNIQUEID";
            insertEC = "INSERT INTO EMERGENCY_CONTACT (PERSON_ID, NAME, EC_ADDRESS, PHONE)VALUES (@PERSON_ID, @NAME, @EC_ADDRESS, @PHONE)";
            deleteEC = "DELETE FROM EMERGENCY_CONTACT WHERE PERSON_ID = @PERSON_ID AND UNIQUEID = @UNIQUEID";
            addISP = "INSERT INTO PERSON_ISP (PERSON_ID, ISPNAME, ISP)VALUES (@PERSON_ID, @ISPNAME, @ISP)";
            deleteISP = "DELETE FROM PERSON_ISP WHERE PERSON_ID = @PERSON_ID AND ISPNAME = @ISPNAME";
            deleteStatement = "DELETE FROM PERSON WHERE SSN = @SSN";
            search = "SELECT * FROM PERSON WHERE LNAME = @LNAME";
            selectBehaviors = "SELECT BEHAVIOR_DATE AS Date, BEHAVIOR_SHIFT AS Shift, BEHAVIOR AS Behavior, SEVERITY AS Severity, SHIFT_TOTAL AS Shift_Total, STAFF_NAME AS Staff FROM BEHAVIOR WHERE PERSON_ID = 0";
            selectISP = "SELECT ISP FROM PERSON_ISP WHERE PERSON_ID = @PERSON_ID AND ISPNAME = ISPNAME";
            getPic = "SELECT PHOTO FROM PERSON WHERE SSN = @SSN";
            connection = new SqlConnection(theConnectionString);
            command = new SqlCommand(insertStatement, connection);
            commandNLS = new SqlCommand(insertStatementNLS, connection);
            commandCR = new SqlCommand(insertStatementCR, connection);
            commandUpdate = new SqlCommand(updateStatement, connection);
            commandNLSUpdate = new SqlCommand(updateStatementNLS, connection);
            commandCRUpdate = new SqlCommand(updateStatementCR, connection);
            commandDelete = new SqlCommand(deleteStatement, connection);
            commandInsertNOK = new SqlCommand(insertNOK, connection);
            commandDeleteNOK = new SqlCommand(deleteNOK, connection);
            commandInsertEC = new SqlCommand(insertEC, connection);
            commandDeleteEC = new SqlCommand(deleteEC, connection);
            commandGetPic = new SqlCommand(getPic, connection);
            commandAddISP = new SqlCommand(addISP, connection);
            commandDeleteISP = new SqlCommand(deleteISP, connection);
            commandViewISP = new SqlCommand(selectISP, connection);
            commandSearch = new SqlCommand(search, connection);
            commandNoPic = new SqlCommand(updateStatementNoPic, connection);
            commandSelectBehaviors = new SqlCommand(selectBehaviors, connection);
        }

        private void btnSaveClient_Click(object sender, EventArgs e)
        {
            if (Int32.TryParse(txtSocialSecurityNum.Text, out personId))
            {
                if (txtSocialSecurityNum.Text != "" && txtLastName.Text != "")
                {
                    if (picClient.Image != null)
                        picClient.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);  
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
                    if (comboRace.Text == "Other")
                        command.Parameters.AddWithValue("@RACE", txtRaceOther.Text);
                    else
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

                    command.Parameters.Clear();
                    commandNLS.Parameters.Clear();
                    commandCR.Parameters.Clear();

                    connection.Close();
                    revertForm();
                    this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
                    this.cOUNTY_RESPONSIBLETableAdapter.Fill(this.projectStarphishDataSet.COUNTY_RESPONSIBLE);
                    this.nEW_LIGHT_SUPPORTTableAdapter.Fill(this.projectStarphishDataSet.NEW_LIGHT_SUPPORT);
                }
                else
                    MessageBox.Show("You must input a Social Security Number and Last Name.");
            }
            else
                MessageBox.Show("Social Security must be a number with no special chacters or spaces.");
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectStarphishDataSet.BEHAVIOR' table. You can move, or remove it, as needed.
            this.bEHAVIORTableAdapter.Fill(this.projectStarphishDataSet.BEHAVIOR);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.PERSON_ISP' table. You can move, or remove it, as needed.
            this.pERSON_ISPTableAdapter.Fill(this.projectStarphishDataSet.PERSON_ISP);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.EMERGENCY_CONTACT' table. You can move, or remove it, as needed.
            this.eMERGENCY_CONTACTTableAdapter.Fill(this.projectStarphishDataSet.EMERGENCY_CONTACT);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.NEXT_OF_KIN' table. You can move, or remove it, as needed.
            this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.COUNTY_RESPONSIBLE' table. You can move, or remove it, as needed.
            this.cOUNTY_RESPONSIBLETableAdapter.Fill(this.projectStarphishDataSet.COUNTY_RESPONSIBLE);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.NEW_LIGHT_SUPPORT' table. You can move, or remove it, as needed.
            this.nEW_LIGHT_SUPPORTTableAdapter.Fill(this.projectStarphishDataSet.NEW_LIGHT_SUPPORT);
            // TODO: This line of code loads data into the 'projectStarphishDataSet.PERSON' table. You can move, or remove it, as needed.
            projectStarphishDataSet.Tables["PERSON"].Columns.Add("NAME", typeof(string), "FNAME + LNAME");
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
            comboRace.SelectedIndex = 3;
            int.TryParse(txtSocialSecurityNum.Text, out personId);
            EventArgs x = new EventArgs();
            listClients_SelectedIndexChanged(this, x);
        }

        private void addClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
            pERSONBindingSource.MoveLast();
            clearForm();
            listClients.ClearSelected();
            listClients.Enabled = false;
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
            btnSearchClients.Enabled = false;
            btnViewISP.Enabled = false;
            btnRemoveISP.Enabled = false;
            btnSaveClient.Enabled = true;
            btnSaveClient.Visible = true;
            btnModifyClient.Enabled = false;
            btnModifyClient.Visible = false;
            btnCancel.Enabled = true;
            btnCancel.Visible = true;
            txtSocialSecurityNum.Enabled = true;
        }

        //Michael - I updated this to TryParse so that it doesn't fail when none is selected. This is a bug. Delete this when fixed.
        private void listClients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listClients.SelectedItem != null)
            {
                int.TryParse(txtSocialSecurityNum.Text, out personId);
                if (searched)
                {
                    commandSearch.Parameters.Clear();
                    connection.Close();
                    searched = false;
                }
                commandGetPic.Parameters.AddWithValue("@SSN", txtSocialSecurityNum.Text);
                connection.Open();
                using (SqlDataReader reader = commandGetPic.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] picData = reader["PHOTO"] as byte[] ?? null;

                        if (picData != null && picData.Length > 0)
                        {
                            using (MemoryStream ms2 = new MemoryStream(picData))
                            {
                                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms2);
                                picClient.Image = bmp;
                            }
                        }
                        else if (picData == null || picData.Length == 0)
                            picClient.Image = null;
                    }
                    commandGetPic.Parameters.Clear();
                    connection.Close();
                }

                this.Text = "Sky Pie - " + txtFirstName.Text + ' ' + txtLastName.Text;
                graphFirstTime = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
            if (tabControl1.SelectedIndex == 1)
                MainAddBehaviors();
            else if (tabControl1.SelectedIndex == 2)
                mainGraph();
            else if (tabControl1.SelectedIndex == 3)
                mainStaffInterview();
        }

        private void btnModifyClient_Click(object sender, EventArgs e)
        {
            if (changedPic)
            {
                changedPic = false;
                if (picClient.Image != null)
                    picClient.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                connection.Open();
                commandUpdate.Parameters.AddWithValue("@FNAME", txtFirstName.Text);
                commandUpdate.Parameters.AddWithValue("@MNAME", txtMiddleName.Text);
                commandUpdate.Parameters.AddWithValue("@LNAME", txtLastName.Text);
                commandUpdate.Parameters.AddWithValue("@IDENTIFYING_MARKS", txtIdentifyingMarks.Text);
                command.Parameters.AddWithValue("@PHOTO", ms.ToArray());
                commandUpdate.Parameters.AddWithValue("@AGENCY_NAME", txtAgencyName.Text);
                commandUpdate.Parameters.AddWithValue("@P_ADDRESS", txtAddress.Text);
                commandUpdate.Parameters.AddWithValue("@ZIP", txtZipCode.Text);
                commandUpdate.Parameters.AddWithValue("@PHONE", txtTelephoneNum.Text);
                commandUpdate.Parameters.AddWithValue("@ADMITTANCE_DATE", txtAdmittanceDate.Text);
                commandUpdate.Parameters.AddWithValue("@DATE_OF_BIRTH", txtDateOfBirth.Text);
                commandUpdate.Parameters.AddWithValue("@AGE", txtAge.Text);
                commandUpdate.Parameters.AddWithValue("@GENDER", comboGender.Text);
                if (comboRace.Text == "Other")
                    commandUpdate.Parameters.AddWithValue("@RACE", txtRaceOther.Text);
                else
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

                commandUpdate.Parameters.Clear();
                commandNLSUpdate.Parameters.Clear();
                commandCRUpdate.Parameters.Clear();

                connection.Close();
            }
            else if (!changedPic)
            {
                connection.Open();
                commandNoPic.Parameters.AddWithValue("@FNAME", txtFirstName.Text);
                commandNoPic.Parameters.AddWithValue("@MNAME", txtMiddleName.Text);
                commandNoPic.Parameters.AddWithValue("@LNAME", txtLastName.Text);
                commandNoPic.Parameters.AddWithValue("@IDENTIFYING_MARKS", txtIdentifyingMarks.Text);
                commandNoPic.Parameters.AddWithValue("@AGENCY_NAME", txtAgencyName.Text);
                commandNoPic.Parameters.AddWithValue("@P_ADDRESS", txtAddress.Text);
                commandNoPic.Parameters.AddWithValue("@ZIP", txtZipCode.Text);
                commandNoPic.Parameters.AddWithValue("@PHONE", txtTelephoneNum.Text);
                commandNoPic.Parameters.AddWithValue("@ADMITTANCE_DATE", txtAdmittanceDate.Text);
                commandNoPic.Parameters.AddWithValue("@DATE_OF_BIRTH", txtDateOfBirth.Text);
                commandNoPic.Parameters.AddWithValue("@AGE", txtAge.Text);
                commandNoPic.Parameters.AddWithValue("@GENDER", comboGender.Text);
                if (comboRace.Text == "Other")
                    commandNoPic.Parameters.AddWithValue("@RACE", txtRaceOther.Text);
                else
                    commandNoPic.Parameters.AddWithValue("@RACE", comboRace.Text);
                commandNoPic.Parameters.AddWithValue("@HAIR_COLOR", txtHairColor.Text);
                commandNoPic.Parameters.AddWithValue("@HEIGHT", txtHeight.Text);
                commandNoPic.Parameters.AddWithValue("@P_WEIGHT", txtWeight.Text);
                commandNoPic.Parameters.AddWithValue("@BSU", txtBSUNum.Text);
                commandNoPic.Parameters.AddWithValue("@MCI", txtMCINum.Text);
                commandNoPic.Parameters.AddWithValue("@INSURANCE_CARRIER", txtInsuranceCarrier.Text);
                commandNoPic.Parameters.AddWithValue("@POLICY_NUM", txtPolicyNum.Text);
                commandNoPic.Parameters.AddWithValue("@MANAGED_CARE_COMPANY", txtManagedCareCompany.Text);
                commandNoPic.Parameters.AddWithValue("@SSN", txtSocialSecurityNum.Text);

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

                commandNoPic.ExecuteNonQuery();
                commandNLSUpdate.ExecuteNonQuery();
                commandCRUpdate.ExecuteNonQuery();

                commandNoPic.Parameters.Clear();
                commandNLSUpdate.Parameters.Clear();
                commandCRUpdate.Parameters.Clear();

                connection.Close();
            }
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
            this.cOUNTY_RESPONSIBLETableAdapter.Fill(this.projectStarphishDataSet.COUNTY_RESPONSIBLE);
            this.nEW_LIGHT_SUPPORTTableAdapter.Fill(this.projectStarphishDataSet.NEW_LIGHT_SUPPORT);
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

        private void dataGridViewBehaviorsStaffInterviews_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void removeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will delete all record of this person. This cannot be undone. Are you sure you wish to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                connection.Open();
                commandDelete.Parameters.AddWithValue("@SSN", Convert.ToInt32(txtSocialSecurityNum.Text));
                commandDelete.ExecuteNonQuery();
                commandDelete.Parameters.Clear();
                connection.Close();
                this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            //If the user doesn't cancel the image file selection, save it and display it picbox.
            if (dialogFileOpenImage.ShowDialog() != DialogResult.Cancel)
            {
                picClient.Image = new Bitmap(dialogFileOpenImage.FileName);
                changedPic = true;
            }
        }

        private void btnAddISP_Click(object sender, EventArgs e)
        {
            //If the user doesn't cancel the ISP file selection, save it and display it in the listbox.
            if (dialogFileOpenISP.ShowDialog() != DialogResult.Cancel)
            {
                int nameAdendum = 2;
                bool ready = false;
                FileStream st = new FileStream(dialogFileOpenISP.FileName, FileMode.Open);
                byte[] buffer = new byte[st.Length];
                st.Read(buffer, 0, (int)st.Length);
                st.Close();

                connection.Open();
                commandAddISP.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
                commandAddISP.Parameters.AddWithValue("@ISPNAME", Path.GetFileName(dialogFileOpenISP.FileName));
                    //while (!ready)
                    //{
                    //    try
                    //    {
                    //        commandAddISP.Parameters.AddWithValue("@ISPNAME", Path.GetFileName(dialogFileOpenISP.FileName + "_" + nameAdendum.ToString()));
                    //        ready = true;
                    //    }
                    //    catch
                    //    {
                    //        ready = false;
                    //        nameAdendum++;
                    //    }
                    //}
                commandAddISP.Parameters.AddWithValue("@ISP", buffer);
                try
                {
                    commandAddISP.ExecuteNonQuery();
                }
                catch 
                {
                    while (!ready)
                    {
                        try
                        {
                            commandAddISP.Parameters.Clear();
                            commandAddISP.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
                            commandAddISP.Parameters.AddWithValue("@ISPNAME", Path.GetFileName(dialogFileOpenISP.FileName + "_" + nameAdendum.ToString()));
                            commandAddISP.Parameters.AddWithValue("@ISP", buffer);
                            commandAddISP.ExecuteNonQuery();
                            ready = true;
                        }
                        catch
                        {
                            ready = false;
                            nameAdendum++;
                        }
                    }
                }
                commandAddISP.Parameters.Clear();
                connection.Close();

                this.pERSON_ISPTableAdapter.Fill(this.projectStarphishDataSet.PERSON_ISP);
            }
        }

        private void btnViewISP_Click(object sender, EventArgs e)
        {
            connection.Open();
            commandViewISP.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandViewISP.Parameters.AddWithValue("@ISPNAME", lstISP.SelectedValue);
            byte[] buffer = (byte[])commandViewISP.ExecuteScalar();
            commandViewISP.Parameters.Clear();
            connection.Close();
            FileStream fs = new FileStream(@"temp" + tempFilesCount + ".pdf", FileMode.Create);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();
            System.Diagnostics.Process.Start(@"temp" + tempFilesCount + ".pdf");
            tempFilesCount++;

            pdfMade = true;
        }

        private void btnRemoveISP_Click(object sender, EventArgs e)
        {
            connection.Open();
            commandDeleteISP.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandDeleteISP.Parameters.AddWithValue("@ISPNAME", lstISP.SelectedValue);

            commandDeleteISP.ExecuteNonQuery();
            commandDeleteISP.Parameters.Clear();
            connection.Close();

            this.pERSON_ISPTableAdapter.Fill(this.projectStarphishDataSet.PERSON_ISP);
        }

        private void btnRemoveNextOfKin_Click(object sender, EventArgs e)
        {
            connection.Open();
            commandDeleteNOK.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandDeleteNOK.Parameters.AddWithValue("@UNIQUEID", lstNextOfKin.SelectedValue);
            commandDeleteNOK.ExecuteNonQuery();
            commandDeleteNOK.Parameters.Clear();
            connection.Close();

            this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
        }

        private void btnAddNextOfKin_Click(object sender, EventArgs e)
        {
            connection.Open();
            commandInsertNOK.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandInsertNOK.Parameters.AddWithValue("@NAME", txtNextOfKinName.Text);
            commandInsertNOK.Parameters.AddWithValue("@NOK_ADDRESS", txtNextOfKinAddress.Text);
            commandInsertNOK.Parameters.AddWithValue("@PHONE", txtNextOfKinTelephoneNum.Text);

            commandInsertNOK.ExecuteNonQuery();
            commandInsertNOK.Parameters.Clear();
            connection.Close();

            this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
        }

        private void btnRemoveEmergencyContact_Click(object sender, EventArgs e)
        {
            connection.Open();
            commandDeleteEC.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandDeleteEC.Parameters.AddWithValue("@UNIQUEID", lstEmergencyContacts.SelectedValue);
            commandDeleteEC.ExecuteNonQuery();
            commandDeleteEC.Parameters.Clear();
            connection.Close();

            this.eMERGENCY_CONTACTTableAdapter.Fill(this.projectStarphishDataSet.EMERGENCY_CONTACT);
        }

        private void btnAddEmergencyContact_Click(object sender, EventArgs e)
        {
            connection.Open();
            commandInsertEC.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandInsertEC.Parameters.AddWithValue("@NAME", txtEmergencyContactName.Text);
            commandInsertEC.Parameters.AddWithValue("@EC_ADDRESS", txtEmergencyContactAddress.Text);
            commandInsertEC.Parameters.AddWithValue("@PHONE", txtEmergencyContactTelephoneNum.Text);

            commandInsertEC.ExecuteNonQuery();
            commandInsertEC.Parameters.Clear();
            connection.Close();

            this.eMERGENCY_CONTACTTableAdapter.Fill(this.projectStarphishDataSet.EMERGENCY_CONTACT);
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
            btnSearchClients.Enabled = true;
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
            btnSaveClient.Visible = false;
            btnModifyClient.Enabled = true;
            btnModifyClient.Visible = true;
            btnCancel.Enabled = false;
            btnCancel.Visible = false;
            txtSocialSecurityNum.Enabled = false;
        }

        private void comboRace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboRace.Text == "Other")
            {
                txtRaceOther.Enabled = true;
                txtRaceOther.Visible = true;
            }
            else
            {
                txtRaceOther.Enabled = false;
                txtRaceOther.Visible = false;
            }
        }

        private void txtSocialSecurityNum_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtSocialSecurityNum.Text, out x) && txtSocialSecurityNum.Text != "")
            {
                MessageBox.Show("Please only enter numbers in this field.");
                txtSocialSecurityNum.Text = "";
            }
        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtAge.Text, out x) && txtAge.Text != "")
            {
                MessageBox.Show("Please only enter numbers in this field.");
                txtAge.Text = "";
            }
        }

        private void txtBSUNum_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtBSUNum.Text, out x) && txtBSUNum.Text != "")
            {
                MessageBox.Show("Please only enter numbers in this field.");
                txtBSUNum.Text = "";
            }
        }

        private void txtMCINum_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtMCINum.Text, out x) && txtMCINum.Text != "")
            {
                MessageBox.Show("Please only enter numbers in this field.");
                txtMCINum.Text = "";
            }
        }

        private void txtPolicyNum_TextChanged(object sender, EventArgs e)
        {
            if (!Int32.TryParse(txtPolicyNum.Text, out x) && txtPolicyNum.Text != "")
            {
                MessageBox.Show("Please only enter numbers in this field.");
                txtPolicyNum.Text = "";
            }
        }

        private void txtSearchClient_TextChanged(object sender, EventArgs e)
        {
        }

        //Uh, Ken, you're not even getting a variable from searchClient. You're just setting the listbox equal to the searchClient form.
        private void btnSearchClients_Click(object sender, EventArgs e)             //|
        {                                                                           //|
            FormSearch searchClient = new FormSearch(txtSearch.Text);               //|
            if (searchClient.ShowDialog() == DialogResult.OK)                       //|
            {
                searched = true;
                commandSearch.Parameters.AddWithValue("@LNAME", searchName);
                connection.Open();
                SqlDataReader reader = commandSearch.ExecuteReader();
                commandSearch.Parameters.Clear();
                reader.Read();
                listClients.SelectedValue = reader["SSN"];
                reader.Close();
                connection.Close();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If on a tab with data that could need to be saved, then prompt the user about possible data lose,
            //  If the user doesn't want to continue exiting, stop the form from closing.
            //  Else continue closing the form.
            if ((tabControl1.SelectedIndex == 0 || tabControl1.SelectedIndex == 1) && MessageBox.Show("Are you sure you want to exit Sky Pie? You will lose all unsaved data.", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                e.Cancel = true;

            try
            {
                if (pdfMade)
                    for (int i = 0; i <= tempFilesCount; i++)
                    {
                        try
                        {
                            File.Delete(@"temp" + i + ".pdf");
                        }
                        catch (System.Exception ex2)
                        {
                            MessageBox.Show("A PDF is missing and will not be deleted.");
                        }
                    }
            }
            catch (System.Exception exception)
            {
                MessageBox.Show(exception.Message + " Please close all open ISP files.");
                e.Cancel = true;
            }

            
        }

        /// <summary>
        /// The copy button for the line chart on the graphs page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chartTotalBehaviors.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
            }
        }

        /// <summary>
        /// The save button for the line chart on the graphs page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chartTotalBehaviors.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
            }
        }

        /// <summary>
        /// The Copy button for the pie chart on the graphs page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                chartPieDailyOccurences.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
            }
        }

        /// <summary>
        /// Save button for the pie chart on the graphs page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 chartPieDailyOccurences.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
            }

        }

        /// <summary>
        /// Copy button for the pyramid chart on the graphs page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem2_Click(object sender, EventArgs e)
        {   
            using (MemoryStream ms = new MemoryStream())
            {
                chartPyramidOccurences.SaveImage(ms, ChartImageFormat.Bmp);
                Bitmap bm = new Bitmap(ms);
                Clipboard.SetImage(bm);
            }
        }

        /// <summary>
        /// Save button for the pyramid chart on the graphs page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem2_Click(object sender, EventArgs e)
        {          
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                chartPyramidOccurences.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
            }
        }
    }
}