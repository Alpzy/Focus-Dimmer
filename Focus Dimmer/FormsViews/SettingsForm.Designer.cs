namespace Focus_Dimmer.FormsViews
{
    partial class SettingsForm
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
            this.SaveBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OnOffCB = new System.Windows.Forms.ComboBox();
            this.OnOffLabel = new System.Windows.Forms.Label();
            this.DimmingModeLbl = new System.Windows.Forms.Label();
            this.DimmingModeCB = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // SaveBtn
            // 
            this.SaveBtn.BackColor = System.Drawing.Color.GreenYellow;
            this.SaveBtn.Location = new System.Drawing.Point(245, 86);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(80, 26);
            this.SaveBtn.TabIndex = 0;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.BackColor = System.Drawing.Color.Salmon;
            this.CancelBtn.Location = new System.Drawing.Point(12, 86);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(80, 26);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = false;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OnOffCB
            // 
            this.OnOffCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OnOffCB.FormattingEnabled = true;
            this.OnOffCB.Location = new System.Drawing.Point(154, 18);
            this.OnOffCB.Name = "OnOffCB";
            this.OnOffCB.Size = new System.Drawing.Size(78, 21);
            this.OnOffCB.TabIndex = 2;
            // 
            // OnOffLabel
            // 
            this.OnOffLabel.AutoSize = true;
            this.OnOffLabel.Location = new System.Drawing.Point(45, 21);
            this.OnOffLabel.Name = "OnOffLabel";
            this.OnOffLabel.Size = new System.Drawing.Size(47, 15);
            this.OnOffLabel.TabIndex = 3;
            this.OnOffLabel.Text = "On / Off";
            // 
            // DimmingModeLbl
            // 
            this.DimmingModeLbl.AutoSize = true;
            this.DimmingModeLbl.Location = new System.Drawing.Point(42, 56);
            this.DimmingModeLbl.Name = "DimmingModeLbl";
            this.DimmingModeLbl.Size = new System.Drawing.Size(93, 15);
            this.DimmingModeLbl.TabIndex = 4;
            this.DimmingModeLbl.Text = "Dimming Mode";
            // 
            // DimmingModeCB
            // 
            this.DimmingModeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DimmingModeCB.FormattingEnabled = true;
            this.DimmingModeCB.Location = new System.Drawing.Point(154, 53);
            this.DimmingModeCB.Name = "DimmingModeCB";
            this.DimmingModeCB.Size = new System.Drawing.Size(78, 21);
            this.DimmingModeCB.TabIndex = 5;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 121);
            this.Controls.Add(this.DimmingModeCB);
            this.Controls.Add(this.DimmingModeLbl);
            this.Controls.Add(this.OnOffLabel);
            this.Controls.Add(this.OnOffCB);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SaveBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "Edit Focus Dimmer Default Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Label OnOffLabel;
        private System.Windows.Forms.Label DimmingModeLbl;
        private System.Windows.Forms.ComboBox DimmingModeCB;
        private System.Windows.Forms.ComboBox OnOffCB;
    }
}