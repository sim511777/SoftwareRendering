namespace SoftwareRendering {
    partial class FormMain {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent() {
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkFillCircles = new System.Windows.Forms.CheckBox();
            this.pbxDraw = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDraw)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkFillCircles);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(600, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 450);
            this.panel1.TabIndex = 0;
            // 
            // chkFillCircles
            // 
            this.chkFillCircles.AutoSize = true;
            this.chkFillCircles.Checked = true;
            this.chkFillCircles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFillCircles.Location = new System.Drawing.Point(6, 12);
            this.chkFillCircles.Name = "chkFillCircles";
            this.chkFillCircles.Size = new System.Drawing.Size(118, 16);
            this.chkFillCircles.TabIndex = 0;
            this.chkFillCircles.Text = "Fill 10000 Circles";
            this.chkFillCircles.UseVisualStyleBackColor = true;
            // 
            // pbxDraw
            // 
            this.pbxDraw.BackColor = System.Drawing.Color.White;
            this.pbxDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxDraw.Location = new System.Drawing.Point(0, 0);
            this.pbxDraw.Name = "pbxDraw";
            this.pbxDraw.Size = new System.Drawing.Size(600, 450);
            this.pbxDraw.TabIndex = 0;
            this.pbxDraw.TabStop = false;
            this.pbxDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxDraw_Paint);
            this.pbxDraw.Layout += new System.Windows.Forms.LayoutEventHandler(this.pbxDraw_Layout);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbxDraw);
            this.Controls.Add(this.panel1);
            this.Name = "FormMain";
            this.Text = "Software Rendering";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDraw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbxDraw;
        private System.Windows.Forms.CheckBox chkFillCircles;
    }
}

