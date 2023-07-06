namespace MyCustomLauncher
{
    partial class AccountForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            flAccounts = new FlowLayoutPanel();
            btnNewAccount = new Button();
            btnSetting = new Button();
            lbNoAccountInfo = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(449, 32);
            label1.TabIndex = 0;
            label1.Text = "CmlLib.Core Custom Minecraft Launcher";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 84);
            label2.Name = "label2";
            label2.Size = new Size(64, 15);
            label2.TabIndex = 1;
            label2.Text = "Accounts: ";
            // 
            // flAccounts
            // 
            flAccounts.Location = new Point(12, 102);
            flAccounts.Name = "flAccounts";
            flAccounts.Size = new Size(450, 297);
            flAccounts.TabIndex = 2;
            // 
            // btnNewAccount
            // 
            btnNewAccount.Location = new Point(12, 405);
            btnNewAccount.Name = "btnNewAccount";
            btnNewAccount.Size = new Size(449, 65);
            btnNewAccount.TabIndex = 3;
            btnNewAccount.Text = "Add New Account";
            btnNewAccount.UseVisualStyleBackColor = true;
            btnNewAccount.Click += btnNewAccount_Click;
            // 
            // btnSetting
            // 
            btnSetting.Location = new Point(387, 73);
            btnSetting.Name = "btnSetting";
            btnSetting.Size = new Size(75, 23);
            btnSetting.TabIndex = 4;
            btnSetting.Text = "Setting";
            btnSetting.UseVisualStyleBackColor = true;
            // 
            // lbNoAccountInfo
            // 
            lbNoAccountInfo.AutoSize = true;
            lbNoAccountInfo.Location = new Point(152, 224);
            lbNoAccountInfo.Name = "lbNoAccountInfo";
            lbNoAccountInfo.Size = new Size(188, 15);
            lbNoAccountInfo.TabIndex = 5;
            lbNoAccountInfo.Text = "Click 'Add New Account' to login";
            // 
            // AccountForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(474, 481);
            Controls.Add(lbNoAccountInfo);
            Controls.Add(btnSetting);
            Controls.Add(btnNewAccount);
            Controls.Add(flAccounts);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "AccountForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            FormClosing += AccountForm_FormClosing;
            Load += AccountForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private FlowLayoutPanel flAccounts;
        private Button btnNewAccount;
        private Button btnSetting;
        private Label lbNoAccountInfo;
    }
}