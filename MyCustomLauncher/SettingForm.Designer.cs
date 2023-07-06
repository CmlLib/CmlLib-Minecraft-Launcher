namespace MyCustomLauncher
{
    partial class SettingForm
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
            btnChangeGamePath = new Button();
            txtGamePath = new TextBox();
            btnChangeAccount = new Button();
            btnLicense = new Button();
            SuspendLayout();
            // 
            // btnChangeGamePath
            // 
            btnChangeGamePath.Location = new Point(12, 45);
            btnChangeGamePath.Name = "btnChangeGamePath";
            btnChangeGamePath.Size = new Size(548, 23);
            btnChangeGamePath.TabIndex = 0;
            btnChangeGamePath.Text = "Change Minecraft Path";
            btnChangeGamePath.UseVisualStyleBackColor = true;
            btnChangeGamePath.Click += btnChangeGamePath_Click;
            // 
            // txtGamePath
            // 
            txtGamePath.Location = new Point(12, 16);
            txtGamePath.Name = "txtGamePath";
            txtGamePath.Size = new Size(548, 23);
            txtGamePath.TabIndex = 1;
            // 
            // btnChangeAccount
            // 
            btnChangeAccount.Location = new Point(12, 72);
            btnChangeAccount.Name = "btnChangeAccount";
            btnChangeAccount.Size = new Size(548, 58);
            btnChangeAccount.TabIndex = 2;
            btnChangeAccount.Text = "Change Account";
            btnChangeAccount.UseVisualStyleBackColor = true;
            btnChangeAccount.Click += btnChangeAccount_Click;
            // 
            // btnLicense
            // 
            btnLicense.Location = new Point(12, 169);
            btnLicense.Name = "btnLicense";
            btnLicense.Size = new Size(75, 23);
            btnLicense.TabIndex = 3;
            btnLicense.Text = "License";
            btnLicense.UseVisualStyleBackColor = true;
            btnLicense.Click += btnLicense_Click;
            // 
            // SettingForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(572, 204);
            Controls.Add(btnLicense);
            Controls.Add(btnChangeAccount);
            Controls.Add(txtGamePath);
            Controls.Add(btnChangeGamePath);
            Name = "SettingForm";
            Text = "SettingForm";
            Load += SettingForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnChangeGamePath;
        private TextBox txtGamePath;
        private Button btnChangeAccount;
        private Button btnLicense;
    }
}