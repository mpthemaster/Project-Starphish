using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormLogin : Form
    {
        private string accountName, accountPassword, securityAnswer;
        private bool close = true; //Whether of not the form should close. It shouldn't close when there is a validation error.
        private bool reset = false; //Whether the user account info needs to be reset.

        public bool Reset { get { return reset; } }

        /// <summary>
        /// Creates a form for the user to login.
        /// </summary>
        /// <param name="accountName">The name of the account stored in the db.</param>
        /// <param name="accountPassword">The password of the account stored in the db.</param>
        /// <param name="securityQuestion">The security question for the account stored in the db.</param>
        /// <param name="securityAnswer">The answer to the security question for the account stored in the db.</param>
        public FormLogin(string accountName, string accountPassword, string securityQuestion, string securityAnswer)
        {
            InitializeComponent();

            this.accountName = accountName;
            this.accountPassword = accountPassword;
            this.securityAnswer = securityAnswer;
            lblSecurityQuestion.Text = securityQuestion;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //If the account information doesn't match, alert the user and don't exit the form.
            if (accountName != txtUsername.Text || accountPassword != txtPassword.Text)
            {
                close = false;
                MessageBox.Show("The username or password was incorrect.", "Incorrect Login Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If the form shouldn't close because invalidate information was entered, stop it from closing.
            if (!close)
            {
                e.Cancel = true;
                close = true;
            }
        }

        private void btnResetAccount_Click(object sender, EventArgs e)
        {
            //If the security information doesn't match, alert the user and don't exit the form.
            //Else the security information does match, so allow the user to reset the account information.
            if (securityAnswer != txtSecurityAnswer.Text)
            {
                close = false;
                MessageBox.Show("The security answer was incorrect.", "Incorrect Reset Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                reset = true;
        }
    }
}