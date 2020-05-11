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
            this.btnE1M1 = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.chkDrawEntity = new System.Windows.Forms.CheckBox();
            this.chkNoVis = new System.Windows.Forms.CheckBox();
            this.rdoLightmap = new System.Windows.Forms.RadioButton();
            this.rdoTexture = new System.Windows.Forms.RadioButton();
            this.lblFace = new System.Windows.Forms.Label();
            this.lblFps = new System.Windows.Forms.Label();
            this.pbxDraw = new System.Windows.Forms.Panel();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnE1M1);
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.chkDrawEntity);
            this.panel1.Controls.Add(this.chkNoVis);
            this.panel1.Controls.Add(this.rdoLightmap);
            this.panel1.Controls.Add(this.rdoTexture);
            this.panel1.Controls.Add(this.lblFace);
            this.panel1.Controls.Add(this.lblFps);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(197, 551);
            this.panel1.TabIndex = 0;
            // 
            // btnE1M1
            // 
            this.btnE1M1.Location = new System.Drawing.Point(12, 98);
            this.btnE1M1.Name = "btnE1M1";
            this.btnE1M1.Size = new System.Drawing.Size(75, 23);
            this.btnE1M1.TabIndex = 8;
            this.btnE1M1.Text = "E1M1";
            this.btnE1M1.UseVisualStyleBackColor = true;
            this.btnE1M1.Click += new System.EventHandler(this.btnE1M1_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(93, 69);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 69);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkDrawEntity
            // 
            this.chkDrawEntity.AutoSize = true;
            this.chkDrawEntity.Location = new System.Drawing.Point(96, 47);
            this.chkDrawEntity.Name = "chkDrawEntity";
            this.chkDrawEntity.Size = new System.Drawing.Size(84, 16);
            this.chkDrawEntity.TabIndex = 5;
            this.chkDrawEntity.Text = "DrawEntity";
            this.chkDrawEntity.UseVisualStyleBackColor = true;
            // 
            // chkNoVis
            // 
            this.chkNoVis.AutoSize = true;
            this.chkNoVis.Location = new System.Drawing.Point(13, 47);
            this.chkNoVis.Name = "chkNoVis";
            this.chkNoVis.Size = new System.Drawing.Size(58, 16);
            this.chkNoVis.TabIndex = 4;
            this.chkNoVis.Text = "NoVis";
            this.chkNoVis.UseVisualStyleBackColor = true;
            // 
            // rdoLightmap
            // 
            this.rdoLightmap.AutoSize = true;
            this.rdoLightmap.Location = new System.Drawing.Point(96, 25);
            this.rdoLightmap.Name = "rdoLightmap";
            this.rdoLightmap.Size = new System.Drawing.Size(75, 16);
            this.rdoLightmap.TabIndex = 3;
            this.rdoLightmap.Text = "Lightmap";
            this.rdoLightmap.UseVisualStyleBackColor = true;
            // 
            // rdoTexture
            // 
            this.rdoTexture.AutoSize = true;
            this.rdoTexture.Checked = true;
            this.rdoTexture.Location = new System.Drawing.Point(14, 25);
            this.rdoTexture.Name = "rdoTexture";
            this.rdoTexture.Size = new System.Drawing.Size(66, 16);
            this.rdoTexture.TabIndex = 2;
            this.rdoTexture.TabStop = true;
            this.rdoTexture.Text = "Texture";
            this.rdoTexture.UseVisualStyleBackColor = true;
            // 
            // lblFace
            // 
            this.lblFace.AutoSize = true;
            this.lblFace.Location = new System.Drawing.Point(94, 9);
            this.lblFace.Name = "lblFace";
            this.lblFace.Size = new System.Drawing.Size(57, 12);
            this.lblFace.TabIndex = 1;
            this.lblFace.Text = "face:2000";
            // 
            // lblFps
            // 
            this.lblFps.AutoSize = true;
            this.lblFps.Location = new System.Drawing.Point(12, 9);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(44, 12);
            this.lblFps.TabIndex = 0;
            this.lblFps.Text = "FPS:60";
            // 
            // pbxDraw
            // 
            this.pbxDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxDraw.Location = new System.Drawing.Point(197, 0);
            this.pbxDraw.Name = "pbxDraw";
            this.pbxDraw.Size = new System.Drawing.Size(710, 551);
            this.pbxDraw.TabIndex = 1;
            this.pbxDraw.Layout += new System.Windows.Forms.LayoutEventHandler(this.pbxDraw_Layout);
            // 
            // dlgOpen
            // 
            this.dlgOpen.Filter = "Quake BSP files (*.bsp)|*.bsp";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 551);
            this.Controls.Add(this.pbxDraw);
            this.Controls.Add(this.panel1);
            this.Name = "FormMain";
            this.Text = "Software Rendering";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pbxDraw;
        private System.Windows.Forms.Button btnE1M1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox chkDrawEntity;
        private System.Windows.Forms.CheckBox chkNoVis;
        private System.Windows.Forms.RadioButton rdoLightmap;
        private System.Windows.Forms.RadioButton rdoTexture;
        private System.Windows.Forms.Label lblFace;
        private System.Windows.Forms.Label lblFps;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
    }
}

