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
    public partial class FormCreateLogin : Form
    {
        private bool close = true; //Whether of not the form should close. It shouldn't close when there is a validation error.

        private string accountName;

        public string AccountName { get { return accountName; } }

        private string accountPassword;

        public string AccountPassword { get { return accountPassword; } }

        private string securityQuestion;

        public string SecurityQuestion { get { return securityQuestion; } }

        private string securityAnswer;

        public string SecurityAnswer { get { return securityAnswer; } }

        /// <summary>
        /// Creates a new user account.
        /// </summary>
        public FormCreateLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new user account when an existing one is being reset.
        /// </summary>
        /// <param name="securityQuestion">The existing security question.</param>
        public FormCreateLogin(string securityQuestion)
        {
            InitializeComponent();

            txtSecurityAnswer.Text = securityAnswer;
            txtSecurityQuestion.Text = securityQuestion;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //If the password and retyped password don't match, alert the user and stop the form from closing.
            //Else if any text field on the form is empty, alert the user that all are required and stop the form from closing.
            //Else the form successfully validates, so save its info.
            if (txtPassword.Text != txtRetypePassword.Text)
            {
                MessageBox.Show("The passwords don't match. Please retype your password again.", "Passwords Don't Match", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = "";
                txtRetypePassword.Text = "";
                txtPassword.Focus();
                close = false;
            }
            else if (String.IsNullOrEmpty(txtPassword.Text) || String.IsNullOrEmpty(txtRetypePassword.Text) || String.IsNullOrEmpty(txtSecurityAnswer.Text) ||
                String.IsNullOrEmpty(txtSecurityQuestion.Text) || String.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("All fields on the form must be filled out.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                close = false;
            }
            else
            {
                accountName = txtUsername.Text;
                accountPassword = txtPassword.Text;
                securityQuestion = txtSecurityQuestion.Text;
                securityAnswer = txtSecurityAnswer.Text;
            }
        }

        private void FormCreateLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If the form shouldn't close because invalidate information was entered, stop it from closing.
            if (!close)
            {
                e.Cancel = true;
                close = true;
            }
        }
    }
}