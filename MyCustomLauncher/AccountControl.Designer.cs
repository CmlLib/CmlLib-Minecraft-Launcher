namespace MyCustomLauncher
{
    partial class AccountControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            pbAvatar = new PictureBox();
            lbUsername = new Label();
            lbIdentifier = new Label();
            btnLogin = new Button();
            btnRemove = new Button();
            ((System.ComponentModel.ISupportInitialize)pbAvatar).BeginInit();
            SuspendLayout();
            // 
            // pbAvatar
            // 
            pbAvatar.Location = new Point(12, 8);
            pbAvatar.Name = "pbAvatar";
            pbAvatar.Size = new Size(85, 85);
            pbAvatar.TabIndex = 0;
            pbAvatar.TabStop = false;
            // 
            // lbUsername
            // 
            lbUsername.AutoSize = true;
            lbUsername.Font = new Font("맑은 고딕", 18F, FontStyle.Regular, GraphicsUnit.Point);
            lbUsername.Location = new Point(104, 18);
            lbUsername.Name = "lbUsername";
            lbUsername.Size = new Size(122, 32);
            lbUsername.TabIndex = 1;
            lbUsername.Text = "Username";
            // 
            // lbIdentifier
            // 
            lbIdentifier.AutoSize = true;
            lbIdentifier.Font = new Font("맑은 고딕", 12F, FontStyle.Regular, GraphicsUnit.Point);
            lbIdentifier.Location = new Point(104, 50);
            lbIdentifier.Name = "lbIdentifier";
            lbIdentifier.Size = new Size(76, 21);
            lbIdentifier.TabIndex = 2;
            lbIdentifier.Text = "Identifier";
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(309, 28);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(75, 23);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRemove
            // 
            btnRemove.Location = new Point(309, 57);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(75, 23);
            btnRemove.TabIndex = 4;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // AccountControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnRemove);
            Controls.Add(btnLogin);
            Controls.Add(lbIdentifier);
            Controls.Add(lbUsername);
            Controls.Add(pbAvatar);
            Name = "AccountControl";
            Size = new Size(400, 100);
            ((System.ComponentModel.ISupportInitialize)pbAvatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbAvatar;
        private Label lbUsername;
        private Label lbIdentifier;
        private Button btnLogin;
        private Button btnRemove;
    }
}
