namespace SharePoint2010Extension
{
    partial class SPExtension
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
            this.button1 = new System.Windows.Forms.Button();
            this.lstSecurityGroup = new System.Windows.Forms.ListBox();
            this.btnbindusers = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(2, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Test Extension";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lstSecurityGroup
            // 
            this.lstSecurityGroup.FormattingEnabled = true;
            this.lstSecurityGroup.Location = new System.Drawing.Point(9, 12);
            this.lstSecurityGroup.Name = "lstSecurityGroup";
            this.lstSecurityGroup.Size = new System.Drawing.Size(120, 95);
            this.lstSecurityGroup.TabIndex = 1;
            // 
            // btnbindusers
            // 
            this.btnbindusers.Location = new System.Drawing.Point(136, 13);
            this.btnbindusers.Name = "btnbindusers";
            this.btnbindusers.Size = new System.Drawing.Size(75, 23);
            this.btnbindusers.TabIndex = 2;
            this.btnbindusers.Text = "Bind Groups";
            this.btnbindusers.UseVisualStyleBackColor = true;
            this.btnbindusers.Click += new System.EventHandler(this.btnbindusers_Click);
            // 
            // SPExtension
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 267);
            this.Controls.Add(this.btnbindusers);
            this.Controls.Add(this.lstSecurityGroup);
            this.Controls.Add(this.button1);
            this.Name = "SPExtension";
            this.Text = "SPExtension";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox lstSecurityGroup;
        private System.Windows.Forms.Button btnbindusers;

    }
}