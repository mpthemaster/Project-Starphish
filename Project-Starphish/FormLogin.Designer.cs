namespace GUI
{
    partial class FormLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSecurityAnswer = new System.Windows.Forms.TextBox();
            this.lblSecurityQuestion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnResetAccount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Please login to use this program.";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(79, 51);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(152, 20);
            this.txtPassword.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Password: ";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(79, 25);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(152, 20);
            this.txtUsername.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Username: ";
            // 
            // txtSecurityAnswer
            // 
            this.txtSecurityAnswer.Location = new System.Drawing.Point(12, 230);
            this.txtSecurityAnswer.Multiline = true;
            this.txtSecurityAnswer.Name = "txtSecurityAnswer";
            this.txtSecurityAnswer.Size = new System.Drawing.Size(260, 52);
            this.txtSecurityAnswer.TabIndex = 20;
            // 
            // lblSecurityQuestion
            // 
            this.lblSecurityQuestion.Location = new System.Drawing.Point(12, 177);
            this.lblSecurityQuestion.Name = "lblSecurityQuestion";
            this.lblSecurityQuestion.Size = new System.Drawing.Size(260, 50);
            this.lblSecurityQuestion.TabIndex = 17;
            this.lblSecurityQuestion.Text = "Security Question: ";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(260, 44);
            this.label3.TabIndex = 21;
            this.label3.Text = "If you can\'t remember your username or password, then enter the answer to your se" +
    "curity question to reset your account details.";
            // 
            // btnLogin
            // 
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLogin.Location = new System.Drawing.Point(107, 77);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 22;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnResetAccount
            // 
            this.btnResetAccount.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnResetAccount.Location = new System.Drawing.Point(98, 288);
            this.btnResetAccount.Name = "btnResetAccount";
            this.btnResetAccount.Size = new System.Drawing.Size(88, 23);
            this.btnResetAccount.TabIndex = 23;
            this.btnResetAccount.Text = "Reset Account";
            this.btnResetAccount.UseVisualStyleBackColor = true;
            this.btnResetAccount.Click += new System.EventHandler(this.btnResetAccount_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 316);
            this.Controls.Add(this.btnResetAccount);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSecurityAnswer);
            this.Controls.Add(this.lblSecurityQuestion);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogin";
            this.Text = "Sky Pie - Login";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogin_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSecurityAnswer;
        private System.Windows.Forms.Label lblSecurityQuestion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnResetAccount;
    }
}