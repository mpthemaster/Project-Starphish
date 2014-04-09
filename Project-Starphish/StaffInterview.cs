using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    internal class StaffInterview
    {
        private SqlConnection connection;
        private int personID;

        public int PersonID { get { return personID; } }

        private DateTime interviewDate;
        private string intervieweeName;

        private List<Behavior> behaviors = new List<Behavior>();

        public List<Behavior> Behaviors { get { return behaviors; } }

        private List<string> strengths = new List<string>();

        public List<string> Strengths { get { return strengths; } }

        public StaffInterview(SqlConnection connection, int personID, DateTime interviewDate, string intervieweeName)
        {
            this.connection = connection;
            this.personID = personID;
            this.interviewDate = interviewDate;
            this.intervieweeName = intervieweeName;
        }

        /// <summary>
        /// Loads the Staff interview up with the specified data.
        /// </summary>
        public void retrieveData()
        {
            connection.Open();
            loadStrengths();
            loadBehaviors();
            loadAntecedents();
            loadQABFs();
            connection.Close();
        }

        /// <summary>
        /// Loads the specified staff interview's strengths from the DB.
        /// </summary>
        private void loadStrengths()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT STRENGTH FROM STAFF_INTERVIEW_STRENGTH WHERE PERSON_ID = @PERSON_ID AND INTERVIEW_DATE = @INTERVIEW_DATE AND STAFF_INTERVIEWED = @STAFF_INTERVIEWED";
            command = new SqlCommand(statement, connection);
            command.Parameters.AddWithValue("@PERSON_ID", personID);
            command.Parameters.AddWithValue("@INTERVIEW_DATE", interviewDate);
            command.Parameters.AddWithValue("@STAFF_INTERVIEWED", intervieweeName);
            reader = command.ExecuteReader();

            //Get the information from the Staff Interview.
            while (reader.Read())
                strengths.Add((string)reader["STRENGTH"]);
            reader.Close();
        }

        /// <summary>
        /// Loads the staff interview's behaviors from the DB.
        /// </summary>
        private void loadBehaviors()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT BEHAVIOR, SEVERITY, FREQUENCY FROM STAFF_INTERVIEW_BEHAVIOR WHERE PERSON_ID = @PERSON_ID AND INTERVIEW_DATE = @INTERVIEW_DATE AND STAFF_INTERVIEWED = @STAFF_INTERVIEWED";
            command = new SqlCommand(statement, connection);
            command.Parameters.AddWithValue("@PERSON_ID", personID);
            command.Parameters.AddWithValue("@INTERVIEW_DATE", interviewDate);
            command.Parameters.AddWithValue("@STAFF_INTERVIEWED", intervieweeName);
            reader = command.ExecuteReader();

            //Get the information from the Staff Interview.
            while (reader.Read())
            {
                string behaviorName = (string)reader["BEHAVIOR"];
                string behaviorSeverity = (string)reader["SEVERITY"];
                string behaviorFrequency = (string)reader["FREQUENCY"];

                behaviors.Add(new Behavior(behaviorName, behaviorSeverity, behaviorFrequency));
            }
            reader.Close();
        }

        // <summary>
        /// Loads the staff interview's behaviors' antecedents from the DB.
        /// </summary>
        private void loadAntecedents()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT BEHAVIOR, ANTECEDENT, CATEGORY FROM STAFF_INTERVIEW_ANTECEDENT WHERE PERSON_ID = @PERSON_ID AND INTERVIEW_DATE = @INTERVIEW_DATE AND STAFF_INTERVIEWED = @STAFF_INTERVIEWED";
            command = new SqlCommand(statement, connection);
            command.Parameters.AddWithValue("@PERSON_ID", personID);
            command.Parameters.AddWithValue("@INTERVIEW_DATE", interviewDate);
            command.Parameters.AddWithValue("@STAFF_INTERVIEWED", intervieweeName);
            reader = command.ExecuteReader();

            //Get the information from the Staff Interview.
            while (reader.Read())
            {
                string behaviorName = (string)reader["BEHAVIOR"];
                string behaviorAntecedent = (string)reader["ANTECEDENT"];
                string behaviorCategory = (string)reader["CATEGORY"];

                //Figure out what behavior the antecedent belongs to and add it.
                foreach (Behavior behavior in behaviors)
                    if (behavior.Name == behaviorName)
                    {
                        behavior.Antecedents.Add(behaviorAntecedent, behaviorCategory);
                        break;
                    }
            }
            reader.Close();
        }

        /// <summary>
        /// Loads the staff interview's behaviors' QABFs from the DB.
        /// </summary>
        private void loadQABFs()
        {
            //Connect to the database.
            SqlDataReader reader;
            SqlCommand command;
            string statement;

            statement = "SELECT BEHAVIOR, QABF_STATUS, Q1, Q2, Q3, Q4, Q5, Q6, Q7, Q8, Q9, Q10, Q11, Q12, Q13, Q14, Q15, Q16, Q17, Q18, Q19, Q20, Q21, Q22, Q23, Q24, Q25 FROM STAFF_INTERVIEW_QABF WHERE PERSON_ID = @PERSON_ID AND INTERVIEW_DATE = @INTERVIEW_DATE AND STAFF_INTERVIEWED = @STAFF_INTERVIEWED";
            command = new SqlCommand(statement, connection);
            command.Parameters.AddWithValue("@PERSON_ID", personID);
            command.Parameters.AddWithValue("@INTERVIEW_DATE", interviewDate);
            command.Parameters.AddWithValue("@STAFF_INTERVIEWED", intervieweeName);
            reader = command.ExecuteReader();

            //Get the information from the Staff Interview.
            while (reader.Read())
            {
                string behaviorName = (string)reader["BEHAVIOR"];

                //Figure out what behavior this QABF belongs to, and then add it to it.
                foreach (Behavior behavior in behaviors)
                    if (behavior.Name == behaviorName)
                    {
                        behavior.Qabf = new QABF();
                        if ((string)reader["QABF_STATUS"] == "0")
                            behavior.Qabf.Completed = false;
                        else
                            behavior.Qabf.Completed = true;

                        for (int i = 0; i < behavior.Qabf.questions.Length; i++)
                            behavior.Qabf.questions[i].Answer = (string)reader["Q" + (i + 1)];
                        break;
                    }
            }
            reader.Close();
        }
    }
}