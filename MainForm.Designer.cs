namespace LogComparisonHelper
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.SelectAppLabel = new System.Windows.Forms.Label();
            this.ProcessListComboBox = new System.Windows.Forms.ComboBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.StopButton = new System.Windows.Forms.Button();
            this.MinimizeCheckbox = new System.Windows.Forms.CheckBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.helpButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectAppLabel
            // 
            this.SelectAppLabel.Location = new System.Drawing.Point(13, 13);
            this.SelectAppLabel.Name = "SelectAppLabel";
            this.SelectAppLabel.Size = new System.Drawing.Size(322, 33);
            this.SelectAppLabel.TabIndex = 0;
            this.SelectAppLabel.Text = "Select the application whose instances you want to keep in sync.\r\nOnly applicatio" +
    "ns with more than one running instance are shown:";
            // 
            // ProcessListComboBox
            // 
            this.ProcessListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProcessListComboBox.FormattingEnabled = true;
            this.ProcessListComboBox.Location = new System.Drawing.Point(338, 19);
            this.ProcessListComboBox.Name = "ProcessListComboBox";
            this.ProcessListComboBox.Size = new System.Drawing.Size(293, 21);
            this.ProcessListComboBox.TabIndex = 1;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(338, 74);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 3;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // StopButton
            // 
            this.StopButton.Location = new System.Drawing.Point(420, 74);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(75, 23);
            this.StopButton.TabIndex = 4;
            this.StopButton.Text = "Stop";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // MinimizeCheckbox
            // 
            this.MinimizeCheckbox.AutoSize = true;
            this.MinimizeCheckbox.Checked = true;
            this.MinimizeCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MinimizeCheckbox.Location = new System.Drawing.Point(338, 47);
            this.MinimizeCheckbox.Name = "MinimizeCheckbox";
            this.MinimizeCheckbox.Size = new System.Drawing.Size(164, 17);
            this.MinimizeCheckbox.TabIndex = 5;
            this.MinimizeCheckbox.Text = "Minimize this window on Start";
            this.MinimizeCheckbox.UseVisualStyleBackColor = true;
            // 
            // RefreshButton
            // 
            this.RefreshButton.AccessibleDescription = "Refresh";
            this.RefreshButton.AccessibleName = "Refresh";
            this.RefreshButton.BackColor = System.Drawing.Color.Transparent;
            this.RefreshButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.RefreshButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.RefreshButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.RefreshButton.Image = global::SideBySideLogCompareHelper.Properties.Resources.refresh32bpp;
            this.RefreshButton.Location = new System.Drawing.Point(638, 16);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(28, 28);
            this.RefreshButton.TabIndex = 2;
            this.RefreshButton.UseVisualStyleBackColor = false;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // helpButton
            // 
            this.helpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helpButton.Location = new System.Drawing.Point(638, 70);
            this.helpButton.Name = "helpButton";
            this.helpButton.Size = new System.Drawing.Size(28, 28);
            this.helpButton.TabIndex = 6;
            this.helpButton.Text = "?";
            this.helpButton.UseVisualStyleBackColor = true;
            this.helpButton.Click += new System.EventHandler(this.helpButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 111);
            this.Controls.Add(this.helpButton);
            this.Controls.Add(this.MinimizeCheckbox);
            this.Controls.Add(this.StopButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.ProcessListComboBox);
            this.Controls.Add(this.SelectAppLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(690, 150);
            this.MinimumSize = new System.Drawing.Size(690, 150);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SideBySideLogCompareHelper v1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SelectAppLabel;
        private System.Windows.Forms.ComboBox ProcessListComboBox;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.CheckBox MinimizeCheckbox;
        private System.Windows.Forms.Button helpButton;
    }
}

