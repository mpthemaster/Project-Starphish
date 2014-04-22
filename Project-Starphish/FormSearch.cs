using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormSearch : Form
    {
        private string theConnectionString;
        private string searchStatement;
        private SqlConnection connection;
        private SqlCommand command;
        private string[] arrClient;
        private string carriedOver;

        /// <summary>
        /// Creates a Form for searching for a client in the db.
        /// </summary>
        /// <param name="nameToSearchFor">The name of the client to search for.</param>
        public FormSearch(string nameToSearchFor)
        {
            InitializeComponent();

            carriedOver = nameToSearchFor;
            

            theConnectionString = "Data Source=localhost\\PROJECTSTARPHISH;Initial Catalog=ProjectStarphish;Integrated Security=True";
            searchStatement = "SELECT * FROM PERSON";
            connection = new SqlConnection(theConnectionString);
            command = new SqlCommand(searchStatement, connection);
        }

        private void Search_Load(object sender, EventArgs e)
        {
            PopulateListBox();
            txtSearchBox.Text = carriedOver;
        }

        private void PopulateListBox()
        {
            //In this code we're connecting to the Database and getting all the LNAMEs into an array.
            int clientCount = 0;
            int i = 0;
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            //Counting how many clients there are.
            while (reader.Read())
            {
                clientCount++;
            }
            connection.Close();
            arrClient = new string[clientCount];
            connection.Open();
            SqlDataReader reader2 = command.ExecuteReader();
            //Putting clients into the array.
            while (reader2.Read())
            {
                arrClient[i++] = reader2["FNAME"].ToString() + ' ' + reader2["LNAME"].ToString();
            }
            connection.Close();
            //Printing the full client list to the array.
            for (int j = 0; j < arrClient.Length; j++)
            {
                listClientSearch.Items.Add(arrClient[j]);
            }
        }

        private void listClientSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            FormMain.searchName = listClientSearch.SelectedItem.ToString();
            btnSearchOk.Enabled = true;
        }

        private void txtSearchBox_TextChanged(object sender, EventArgs e)
        {
            listClientSearch.Items.Clear();
            btnSearchOk.Enabled = false;
            for (int i = 0; i < arrClient.Length; i++)
            {
                if (arrClient[i].Contains(txtSearchBox.Text))
                {
                    listClientSearch.Items.Add(arrClient[i]);
                }
            }
            if (txtSearchBox.Text == "" || txtSearchBox.Text == null)
            {
                listClientSearch.Items.Clear();
                for (int j = 0; j < arrClient.Length; j++)
                {
                    listClientSearch.Items.Add(arrClient[j]);
                }
            }
        }
    }
}