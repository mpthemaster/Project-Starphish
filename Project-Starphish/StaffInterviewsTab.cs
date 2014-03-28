using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    partial class FormMain
    {
        /// <summary>
        /// Loads up the pre-existing staff interviews to view.
        /// </summary>
        private void mainStaffInterview()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT PERSON_ID, INTERVIEW_DATE, STAFF_INTERVIEWED FROM STAFF_INTERVIEW_ANTECEDENT";
            command = new SqlCommand(statement, connection);
            reader = command.ExecuteReader();

            //Get all the staff interviews and display them.
            while (reader.Read())
                if ((int)reader["PERSON_ID"] == personId)
                {
                }
            reader.Close();
        }
    }
}