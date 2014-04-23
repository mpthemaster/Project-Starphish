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
        private string updateNOK;
        private string updateEC;

        private SqlCommand commandUpdateNOK;
        private SqlCommand commandUpdateEC;
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

        //private MemoryStream ms = new MemoryStream();
        private int x;

        private bool pdfMade = false;
        private int tempFilesCount = 0;
        private bool searched = false;
        private bool changedPic = false;
        private bool login = false;
        public static string searchString = "";

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
            insertNOK = "INSERT INTO NEXT_OF_KIN (PERSON_ID, NAME, NOK_ADDRESS, PHONE, RELATIONSHIP)VALUES (@PERSON_ID, @NAME, @NOK_ADDRESS, @PHONE, @RELATIONSHIP)";
            deleteNOK = "DELETE FROM NEXT_OF_KIN WHERE PERSON_ID = @PERSON_ID AND UNIQUEID = @UNIQUEID";
            insertEC = "INSERT INTO EMERGENCY_CONTACT (PERSON_ID, NAME, EC_ADDRESS, PHONE)VALUES (@PERSON_ID, @NAME, @EC_ADDRESS, @PHONE)";
            deleteEC = "DELETE FROM EMERGENCY_CONTACT WHERE PERSON_ID = @PERSON_ID AND UNIQUEID = @UNIQUEID";
            addISP = "INSERT INTO PERSON_ISP (PERSON_ID, ISPNAME, ISP)VALUES (@PERSON_ID, @ISPNAME, @ISP)";
            deleteISP = "DELETE FROM PERSON_ISP WHERE PERSON_ID = @PERSON_ID AND ISPNAME = @ISPNAME";
            deleteStatement = "DELETE FROM PERSON WHERE SSN = @SSN";
            search = "SELECT * FROM PERSON WHERE FNAME + ' ' + LNAME = @NAME";
            selectBehaviors = "SELECT BEHAVIOR_DATE AS Date, BEHAVIOR_SHIFT AS Shift, BEHAVIOR AS Behavior, SEVERITY AS Severity, SHIFT_TOTAL AS Shift_Total, STAFF_NAME AS Staff FROM BEHAVIOR WHERE PERSON_ID = 0";
            selectISP = "SELECT ISP FROM PERSON_ISP WHERE PERSON_ID = @PERSON_ID AND ISPNAME = ISPNAME";
            getPic = "SELECT PHOTO FROM PERSON WHERE SSN = @SSN";
            updateNOK = "UPDATE NEXT_OF_KIN SET NAME = @NAME, NOK_ADDRESS = @NOK_ADDRESS, PHONE = @PHONE, RELATIONSHIP = @RELATIONSHIP WHERE PERSON_ID = @PERSON_ID AND UNIQUEID = @UNIQUEID";
            updateEC = "UPDATE EMERGENCY_CONTACT SET NAME = @NAME, EC_ADDRESS = @EC_ADDRESS, PHONE = @PHONE WHERE PERSON_ID = @PERSON_ID AND UNIQUEID = @UNIQUEID";

            commandUpdateNOK = new SqlCommand(updateNOK, connection);
            commandUpdateEC = new SqlCommand(updateEC, connection);
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

        private void userLogin()
        {
            //Get user info from the db.
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            //Get the information from the Staff Interview and display it.
            try
            {
                connection.Open();
            }
            catch (SqlException)
            {
                MessageBox.Show("No server found. The program will now exit.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1001);
            }
            statement = "SELECT * FROM SIGNIN";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            string accountName = "", accountPassword = "", securityQuestion = "", securityAnswer = "";

            while (reader.Read())
            {
                accountName = (string)reader["ACCOUNT_NAME"];
                accountPassword = (string)reader["ACCOUNT_PASSWORD"];
                securityQuestion = (string)reader["SECURITY_QUESTION"];
                securityAnswer = (string)reader["SECURITY_ANSWER"];
            }
            reader.Close();
            connection.Close();

            //If there is a user in the db, get login information.
            bool reset = false;
            if (!String.IsNullOrEmpty(accountName))
            {
                FormLogin formLogin = new FormLogin(accountName, accountPassword, securityQuestion, securityAnswer);

                //If the login form is cancelled, exit.
                //Else the user is logged in or wants to reset.
                if (formLogin.ShowDialog() != DialogResult.OK)
                    Environment.Exit(1001);
                else
                    reset = formLogin.Reset;
            }

            //If no user exists or the user wants to reset account information, make the user create a login or reset an ex.
            if (String.IsNullOrEmpty(accountName) || reset)
            {
                FormCreateLogin formCreateLogin;

                //If the user wants to reset account information, pass the creation form a security question and security answer.
                //Else the user doesn't want to reset account information, so just instantiate the creation form as normal.
                if (reset)
                    formCreateLogin = new FormCreateLogin(securityQuestion);
                else
                    formCreateLogin = new FormCreateLogin();

                //If the user doesn't create a new account, exit.
                //Else the user created a new account, so save it to the db.
                if (formCreateLogin.ShowDialog() != DialogResult.OK)
                    Environment.Exit(1001);
                else
                {
                    connection.Open();

                    //If the user wants to reset account information, delete the existing account information before adding the new information.
                    if (reset)
                    {
                        statement = "DELETE FROM SIGNIN";
                        command = new SqlCommand(statement, connection);
                        command.ExecuteNonQuery();
                    }

                    //Hashes all sensitive information that could be used to login or answer the security question.
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(formCreateLogin.AccountPassword);
                    data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                    String hashPassword = System.Text.Encoding.ASCII.GetString(data);

                    data = System.Text.Encoding.ASCII.GetBytes(formCreateLogin.SecurityAnswer);
                    data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                    String hashAnswer = System.Text.Encoding.ASCII.GetString(data);

                    data = System.Text.Encoding.ASCII.GetBytes(formCreateLogin.AccountName);
                    data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
                    String hashName = System.Text.Encoding.ASCII.GetString(data);

                    statement = "INSERT INTO SIGNIN (ACCOUNT_NAME, ACCOUNT_PASSWORD, SECURITY_QUESTION, SECURITY_ANSWER) VALUES        (@ACCOUNT_NAME, @ACCOUNT_PASSWORD, @SECURITY_QUESTION, @SECURITY_ANSWER)";
                    command = new SqlCommand(statement, connection);
                    command.Parameters.AddWithValue("@ACCOUNT_NAME", hashName);
                    command.Parameters.AddWithValue("@SECURITY_QUESTION", formCreateLogin.SecurityQuestion);
                    command.Parameters.AddWithValue("@SECURITY_ANSWER", hashAnswer);
                    command.Parameters.AddWithValue("@ACCOUNT_PASSWORD", hashPassword);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        private void btnSaveClient_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                if (!String.IsNullOrEmpty(txtSocialSecurityNum.Text) && !String.IsNullOrEmpty(txtLastName.Text) && !String.IsNullOrEmpty(txtFirstName.Text))
                {
                    if (Int32.TryParse(txtSocialSecurityNum.Text, out personId))
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
                        listClients_SelectedIndexChanged(this, e);
                    }
                    else
                        MessageBox.Show("Social Security must be a number with no special chacters or spaces.");
                }
                else
                    MessageBox.Show("You must input a Social Security Number, Last Name, and First Name.");
            }
            catch (Exception)
            {
                MessageBox.Show("Something has gone wrong. This person cannot be saved. Please make sure the Social Security Number is unique.");
                connection.Close();
            }

            ms.Dispose();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Make the user login or set up login info.
            if (!login)
            {
                userLogin();
            }
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
            if (!login)
            {
                projectStarphishDataSet.Tables["PERSON"].Columns.Add("NAME", typeof(string), "FNAME + ' ' + LNAME");
                login = true;
            }
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
            listClients.DataSource = pERSONBindingSource;
            listClients.ValueMember = "SSN";
            listClients.DisplayMember = "NAME";

            int.TryParse(txtSocialSecurityNum.Text, out personId);
            EventArgs x = new EventArgs();
            if (listClients.Items.Count > 0)
            {
                listClients_SelectedIndexChanged(this, x);
            }
            else
            {
                addClientToolStripMenuItem_Click(this, x);
                btnCancel.Visible = false;
            }
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
            addClientToolStripMenuItem.Enabled = false;
            removeClientToolStripMenuItem.Enabled = false;
            ((Control)tabPage2).Enabled = false;
            ((Control)tabPage3).Enabled = false;
            ((Control)tabPage4).Enabled = false;
        }

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
                        {
                            picClient.Image = null;
                        }
                    }
                    commandGetPic.Parameters.Clear();
                    connection.Close();
                }
                if (txtRaceOther.Text == "Caucasian")
                    comboRace.SelectedIndex = 0;
                else if (txtRaceOther.Text == "African-American")
                    comboRace.SelectedIndex = 1;
                else if (txtRaceOther.Text == "Latino")
                    comboRace.SelectedIndex = 2;
                else
                {
                    comboRace.SelectedIndex = 3;
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
            MemoryStream ms = new MemoryStream();
            if (!String.IsNullOrEmpty(txtLastName.Text) && !String.IsNullOrEmpty(txtFirstName.Text))
            {
                if (changedPic)
                {
                    changedPic = false;
                    if (picClient.Image != null)
                        picClient.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    connection.Close();
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
            else
            {
                MessageBox.Show("You can't save a person with no last name and/or no first name.");
            }
            ms.Close();

            listClients.SelectedIndex = 0;
            listClients_SelectedIndexChanged(this, e);
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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }

        private void removeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listClients.SelectedItem != null && MessageBox.Show("This will delete all record of this person. This cannot be undone. Are you sure you wish to continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                connection.Open();
                commandDelete.Parameters.AddWithValue("@SSN", Convert.ToInt32(txtSocialSecurityNum.Text));
                commandDelete.ExecuteNonQuery();
                commandDelete.Parameters.Clear();
                connection.Close();
                this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
                this.bEHAVIORTableAdapter.Fill(this.projectStarphishDataSet.BEHAVIOR);
                this.pERSON_ISPTableAdapter.Fill(this.projectStarphishDataSet.PERSON_ISP);
                this.eMERGENCY_CONTACTTableAdapter.Fill(this.projectStarphishDataSet.EMERGENCY_CONTACT);
                this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
                this.cOUNTY_RESPONSIBLETableAdapter.Fill(this.projectStarphishDataSet.COUNTY_RESPONSIBLE);
                this.nEW_LIGHT_SUPPORTTableAdapter.Fill(this.projectStarphishDataSet.NEW_LIGHT_SUPPORT);
                if (listClients.Items.Count > 0)
                {
                    listClients_SelectedIndexChanged(this, e);
                }
                else
                {
                    addClientToolStripMenuItem_Click(this, e);
                    btnCancel.Visible = false;
                }
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            MemoryStream ms = new MemoryStream();
            //If the user doesn't cancel the image file selection, save it and display it picbox.
            if (dialogFileOpenImage.ShowDialog() != DialogResult.Cancel)
            {
                picClient.Image = new Bitmap(dialogFileOpenImage.FileName);
                picClient.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                changedPic = true;
            }
            ms.Close();
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
            try
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
            catch
            {
                MessageBox.Show("No ISP to view");
            }
        }

        private void btnRemoveISP_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                commandDeleteISP.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
                commandDeleteISP.Parameters.AddWithValue("@ISPNAME", lstISP.SelectedValue);

                commandDeleteISP.ExecuteNonQuery();
                commandDeleteISP.Parameters.Clear();
                connection.Close();

                this.pERSON_ISPTableAdapter.Fill(this.projectStarphishDataSet.PERSON_ISP);
            }
            catch
            {
                MessageBox.Show("No ISP to remove.");
                connection.Close();
            }
        }

        private void btnRemoveNextOfKin_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                commandDeleteNOK.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
                commandDeleteNOK.Parameters.AddWithValue("@UNIQUEID", lstNextOfKin.SelectedValue);
                commandDeleteNOK.ExecuteNonQuery();
                commandDeleteNOK.Parameters.Clear();
                connection.Close();

                this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
            }
            catch
            {
                MessageBox.Show("No Next of Kin has been selected to remove.");
                connection.Close();
            }
        }

        private void btnAddNextOfKin_Click(object sender, EventArgs e)
        {
            connection.Open();
            commandInsertNOK.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
            commandInsertNOK.Parameters.AddWithValue("@NAME", txtNextOfKinName.Text);
            commandInsertNOK.Parameters.AddWithValue("@NOK_ADDRESS", txtNextOfKinAddress.Text);
            commandInsertNOK.Parameters.AddWithValue("@PHONE", txtNextOfKinTelephoneNum.Text);
            commandInsertNOK.Parameters.AddWithValue("@RELATIONSHIP", txtRelation.Text);

            commandInsertNOK.ExecuteNonQuery();
            commandInsertNOK.Parameters.Clear();
            connection.Close();

            this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
        }

        private void btnRemoveEmergencyContact_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                commandDeleteEC.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
                commandDeleteEC.Parameters.AddWithValue("@UNIQUEID", lstEmergencyContacts.SelectedValue);
                commandDeleteEC.ExecuteNonQuery();
                commandDeleteEC.Parameters.Clear();
                connection.Close();

                this.eMERGENCY_CONTACTTableAdapter.Fill(this.projectStarphishDataSet.EMERGENCY_CONTACT);
            }
            catch
            {
                MessageBox.Show("No Emergency Contact has been selected to remove.");
                connection.Close();
            }
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
                FormMain_Load(this, e);
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
            addClientToolStripMenuItem.Enabled = true;
            removeClientToolStripMenuItem.Enabled = true;
            ((Control)tabPage2).Enabled = true;
            ((Control)tabPage3).Enabled = true;
            ((Control)tabPage4).Enabled = true;
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

        private void btnSearchClients_Click(object sender, EventArgs e)
        {
            FormSearch searchClient = new FormSearch(txtSearch.Text);
            if (searchClient.ShowDialog() == DialogResult.OK)
            {
                searched = true;
                commandSearch.Parameters.AddWithValue("@NAME", searchName);
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
            //If the form is closing because the user didn't create an account, just close (stops unauthorized access).
            if (Environment.ExitCode == 1001)
                return;

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
                        catch (System.Exception)
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchString = txtSearch.Text;
        }

        private void txtTelephoneNum_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtTelephoneNum.Text.Length == 10 && ulong.TryParse(txtTelephoneNum.Text, out num))
            {
                string pn = txtTelephoneNum.Text;

                txtTelephoneNum.Text = String.Format("({0}) {1}-{2}", pn.Substring(0, 3), pn.Substring(3, 3), pn.Substring(6));
            }
        }

        private void txtNextOfKinTelephoneNum_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtNextOfKinTelephoneNum.Text.Length == 10 && ulong.TryParse(txtNextOfKinTelephoneNum.Text, out num))
            {
                string pn = txtNextOfKinTelephoneNum.Text;

                txtNextOfKinTelephoneNum.Text = String.Format("({0}) {1}-{2}", pn.Substring(0, 3), pn.Substring(3, 3), pn.Substring(6));
            }
        }

        private void txtEmergencyContactTelephoneNum_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtEmergencyContactTelephoneNum.Text.Length == 10 && ulong.TryParse(txtEmergencyContactTelephoneNum.Text, out num))
            {
                string pn = txtEmergencyContactTelephoneNum.Text;

                txtEmergencyContactTelephoneNum.Text = String.Format("({0}) {1}-{2}", pn.Substring(0, 3), pn.Substring(3, 3), pn.Substring(6));
            }
        }

        private void txtSiteSupervisorTelephoneNum_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtSiteSupervisorTelephoneNum.Text.Length == 10 && ulong.TryParse(txtSiteSupervisorTelephoneNum.Text, out num))
            {
                string pn = txtSiteSupervisorTelephoneNum.Text;

                txtSiteSupervisorTelephoneNum.Text = String.Format("({0}) {1}-{2}", pn.Substring(0, 3), pn.Substring(3, 3), pn.Substring(6));
            }
        }

        private void txtProgramCoordinatorTelephoneNum_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtProgramCoordinatorTelephoneNum.Text.Length == 10 && ulong.TryParse(txtProgramCoordinatorTelephoneNum.Text, out num))
            {
                string pn = txtProgramCoordinatorTelephoneNum.Text;

                txtProgramCoordinatorTelephoneNum.Text = String.Format("({0}) {1}-{2}", pn.Substring(0, 3), pn.Substring(3, 3), pn.Substring(6));
            }
        }

        private void txtProgramSpecialistPhoneNum_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtProgramSpecialistPhoneNum.Text.Length == 10 && ulong.TryParse(txtProgramSpecialistPhoneNum.Text, out num))
            {
                string pn = txtProgramSpecialistPhoneNum.Text;

                txtProgramSpecialistPhoneNum.Text = String.Format("({0}) {1}-{2}", pn.Substring(0, 3), pn.Substring(3, 3), pn.Substring(6));
            }
        }

        private void txtSupportsCoordinatorTelephoneNum_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtSupportsCoordinatorTelephoneNum.Text.Length == 10 && ulong.TryParse(txtSupportsCoordinatorTelephoneNum.Text, out num))
            {
                string pn = txtSupportsCoordinatorTelephoneNum.Text;

                txtSupportsCoordinatorTelephoneNum.Text = String.Format("({0}) {1}-{2}", pn.Substring(0, 3), pn.Substring(3, 3), pn.Substring(6));
            }
        }

        private void txtAdmittanceDate_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtAdmittanceDate.Text.Length == 8 && ulong.TryParse(txtAdmittanceDate.Text, out num))
            {
                string pn = txtAdmittanceDate.Text;

                txtAdmittanceDate.Text = String.Format("{0}/{1}/{2}", pn.Substring(0, 2), pn.Substring(2, 2), pn.Substring(4));
            }
            else if (txtAdmittanceDate.Text.Length == 10 || txtAdmittanceDate.Text.Length == 9 || txtAdmittanceDate.Text.Length == 8)
            {
            }
            else
            {
                MessageBox.Show("Invalid date, please use the format mmddyyyy.");

                txtAdmittanceDate.Focus();
            }
        }

        private void txtDateOfBirth_Leave(object sender, EventArgs e)
        {
            ulong num;

            if (txtDateOfBirth.Text.Length == 8 && ulong.TryParse(txtDateOfBirth.Text, out num))
            {
                string pn = txtDateOfBirth.Text;

                txtDateOfBirth.Text = String.Format("{0}/{1}/{2}", pn.Substring(0, 2), pn.Substring(2, 2), pn.Substring(4));
            }
            else if (txtDateOfBirth.Text.Length == 10 || txtDateOfBirth.Text.Length == 9 || txtDateOfBirth.Text.Length == 8)
            {
            }
            else
            {
                MessageBox.Show("Invalid date, please use the format mmddyyyy.");

                txtDateOfBirth.Focus();
            }
        }

        private void btnEditNOK_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                commandUpdateNOK.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
                commandUpdateNOK.Parameters.AddWithValue("@UNIQUEID", lstNextOfKin.SelectedValue);
                commandUpdateNOK.Parameters.AddWithValue("@NAME", txtNextOfKinName.Text);
                commandUpdateNOK.Parameters.AddWithValue("@PHONE", txtNextOfKinTelephoneNum.Text);
                commandUpdateNOK.Parameters.AddWithValue("@ADDRESS", txtNextOfKinAddress.Text);
                commandUpdateNOK.Parameters.AddWithValue("@RELATION", txtRelation.Text);
                commandUpdateNOK.ExecuteNonQuery();
                commandUpdateNOK.Parameters.Clear();
                connection.Close();

                this.nEXT_OF_KINTableAdapter.Fill(this.projectStarphishDataSet.NEXT_OF_KIN);
            }
            catch
            {
                MessageBox.Show("No Next of Kin has been selected to edit.");
                connection.Close();
            }
        }

        private void btnEditContact_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                commandUpdateEC.Parameters.AddWithValue("@PERSON_ID", txtSocialSecurityNum.Text);
                commandUpdateEC.Parameters.AddWithValue("@UNIQUEID", lstEmergencyContacts.SelectedValue);
                commandUpdateEC.Parameters.AddWithValue("@NAME", txtEmergencyContactName.Text);
                commandUpdateEC.Parameters.AddWithValue("@PHONE", txtEmergencyContactTelephoneNum.Text);
                commandUpdateEC.Parameters.AddWithValue("@ADDRESS", txtEmergencyContactAddress.Text);
                commandUpdateEC.ExecuteNonQuery();
                commandUpdateEC.Parameters.Clear();
                connection.Close();

                this.eMERGENCY_CONTACTTableAdapter.Fill(this.projectStarphishDataSet.EMERGENCY_CONTACT);
            }
            catch
            {
                MessageBox.Show("No Emergency Contact has been selected to edit.");
                connection.Close();
            }
        }
    }
}