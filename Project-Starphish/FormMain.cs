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
            fillBox(connection);
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
            command.Parameters.AddWithValue("@SSN", txtSocialSecurityNum.Text);

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

        private void fillBox(SqlConnection connection)
        {
            DataTable _table = retrieveNames(connection);
            //binds the datatable we made earlier to the listbox and sets it to display what was set to the name field.
            //I did it like this because this was the only way I found that would let me print multiple columns in the box.
            listClients.DataSource = _table;
            listClients.DisplayMember = _table.Columns["NAME"].ToString();
        }

        private void listClients_SelectedIndexChanged(object sender, EventArgs e)
        {
           //ignore this for now, still working on getting it to work. What we need to do here is check what was selected and look through
           //the databse to find the value selected. From there we'll probably set our global ssn variable and then proceed to fill in the form
        }
        private DataTable retrieveNames(SqlConnection connection)
        {
            //This gets all the columns from the database and places them in a datatable for easier access by the gui
            connection.Open();
            SqlDataAdapter _adapter = new SqlDataAdapter("SELECT FName + ' ' + MName + ' ' + LName AS NAME, * FROM PERSON", connection);
            DataTable _table = new DataTable();
            _adapter.Fill(_table);
            connection.Close();
            return _table;
        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 0)
           // if (tabControl1.SelectedIndex == 1)
            if (tabControl1.SelectedIndex == 2)
                mainGraph();
            //if (tabControl1.SelectedIndex == 3)
        }
    }
}