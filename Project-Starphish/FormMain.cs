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
            theConnectionString = "Data Source=MSIGAMING\\PROJECTSTARPHISH;Initial Catalog=ProjectStarphish;Integrated Security=True";
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
            FormStaffInterview staffInterview = new FormStaffInterview();
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
            command.Parameters.AddWithValue("@IDENTIFYING_MARKS", txtFirstName.Text);
            command.Parameters.AddWithValue("@PHOTO", ms.ToArray());
            command.Parameters.AddWithValue("@AGENCY_NAME", txtFirstName.Text);
            command.Parameters.AddWithValue("@P_ADDRESS", txtFirstName.Text);
            command.Parameters.AddWithValue("@PHONE", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@ADMITTANCE_DATE", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@DATE_OF_BIRTH", txtDateOfBirth.Text);
            command.Parameters.AddWithValue("@AGE", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@GENDER", txtFirstName.Text);
            command.Parameters.AddWithValue("@RACE", txtFirstName.Text);
            command.Parameters.AddWithValue("@HAIR_COLOR", txtFirstName.Text);
            command.Parameters.AddWithValue("@HEIGHT", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@P_WEIGHT", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@BSU", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@MCI", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@INSURANCE_CARRIER", txtFirstName.Text);
            command.Parameters.AddWithValue("@POLICY_NUM", txtAdmittanceDate.Text);
            command.Parameters.AddWithValue("@MANAGED_CARE_COMPANY", txtFirstName.Text);
            command.Parameters.AddWithValue("@SSN", 3);

            command.ExecuteNonQuery();
            connection.Close();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'projectStarphishDataSet.PERSON' table. You can move, or remove it, as needed.
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
        }

        private void addClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pERSONTableAdapter.Fill(this.projectStarphishDataSet.PERSON);
            pERSONBindingSource.MoveLast();
        }
    }
}