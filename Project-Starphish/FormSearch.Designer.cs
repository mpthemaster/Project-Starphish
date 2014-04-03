namespace GUI
{
    partial class FormSearch
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
            this.label1 = new System.Windows.Forms.Label();
            this.listClientSearch = new System.Windows.Forms.ListBox();
            this.btnSearchOk = new System.Windows.Forms.Button();
            this.btnSearchCancel = new System.Windows.Forms.Button();
            this.txtSearchBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search for a person by his/her Last Name.";
            // 
            // listClientSearch
            // 
            this.listClientSearch.FormattingEnabled = true;
            this.listClientSearch.Location = new System.Drawing.Point(26, 116);
            this.listClientSearch.Name = "listClientSearch";
            this.listClientSearch.Size = new System.Drawing.Size(87, 134);
            this.listClientSearch.TabIndex = 1;
            this.listClientSearch.SelectedIndexChanged += new System.EventHandler(this.listClientSearch_SelectedIndexChanged);
            // 
            // btnSearchOk
            // 
            this.btnSearchOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSearchOk.Location = new System.Drawing.Point(143, 141);
            this.btnSearchOk.Name = "btnSearchOk";
            this.btnSearchOk.Size = new System.Drawing.Size(93, 23);
            this.btnSearchOk.TabIndex = 2;
            this.btnSearchOk.Text = "Select";
            this.btnSearchOk.UseVisualStyleBackColor = true;
            // 
            // btnSearchCancel
            // 
            this.btnSearchCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSearchCancel.Location = new System.Drawing.Point(143, 203);
            this.btnSearchCancel.Name = "btnSearchCancel";
            this.btnSearchCancel.Size = new System.Drawing.Size(93, 23);
            this.btnSearchCancel.TabIndex = 3;
            this.btnSearchCancel.Text = "Cancel Search";
            this.btnSearchCancel.UseVisualStyleBackColor = true;
            // 
            // txtSearchBox
            // 
            this.txtSearchBox.Location = new System.Drawing.Point(26, 62);
            this.txtSearchBox.Name = "txtSearchBox";
            this.txtSearchBox.Size = new System.Drawing.Size(210, 20);
            this.txtSearchBox.TabIndex = 4;
            this.txtSearchBox.TextChanged += new System.EventHandler(this.txtSearchBox_TextChanged);
            // 
            // Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.txtSearchBox);
            this.Controls.Add(this.btnSearchCancel);
            this.Controls.Add(this.btnSearchOk);
            this.Controls.Add(this.listClientSearch);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "Search";
            this.Text = "Search";
            this.Load += new System.EventHandler(this.Search_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listClientSearch;
        private System.Windows.Forms.Button btnSearchOk;
        private System.Windows.Forms.Button btnSearchCancel;
        private System.Windows.Forms.TextBox txtSearchBox;
    }
}