namespace DangKyKhamTuDong
{
    partial class frm_Face
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Face));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ib_face = new Emgu.CV.UI.ImageBox();
            this.btn_OK = new DevExpress.XtraEditors.SimpleButton();
            this.btn_TryAgain = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Start = new DevExpress.XtraEditors.SimpleButton();
            this.imageBoxFrameGrabber = new Emgu.CV.UI.ImageBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ib_face)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ib_face);
            this.panel1.Controls.Add(this.btn_OK);
            this.panel1.Controls.Add(this.btn_TryAgain);
            this.panel1.Location = new System.Drawing.Point(394, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(327, 316);
            this.panel1.TabIndex = 1;
            // 
            // ib_face
            // 
            this.ib_face.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ib_face.Location = new System.Drawing.Point(73, 11);
            this.ib_face.Name = "ib_face";
            this.ib_face.Size = new System.Drawing.Size(183, 168);
            this.ib_face.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ib_face.TabIndex = 5;
            this.ib_face.TabStop = false;
            // 
            // btn_OK
            // 
            this.btn_OK.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.Appearance.Options.UseFont = true;
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Image = ((System.Drawing.Image)(resources.GetObject("btn_OK.Image")));
            this.btn_OK.Location = new System.Drawing.Point(174, 194);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(128, 32);
            this.btn_OK.TabIndex = 3;
            this.btn_OK.Text = "Chọn ảnh";
            // 
            // btn_TryAgain
            // 
            this.btn_TryAgain.Appearance.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_TryAgain.Appearance.Options.UseFont = true;
            this.btn_TryAgain.Image = ((System.Drawing.Image)(resources.GetObject("btn_TryAgain.Image")));
            this.btn_TryAgain.Location = new System.Drawing.Point(39, 194);
            this.btn_TryAgain.Name = "btn_TryAgain";
            this.btn_TryAgain.Size = new System.Drawing.Size(129, 32);
            this.btn_TryAgain.TabIndex = 3;
            this.btn_TryAgain.Text = "Chụp lại";
            this.btn_TryAgain.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(38, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 33);
            this.label1.TabIndex = 2;
            this.label1.Text = "1. Bấm nút \'Bắt đầu thực hiện\' ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(38, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(574, 33);
            this.label2.TabIndex = 2;
            this.label2.Text = "2. Điều chỉnh để khuôn mặt vào trong khung hình";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(41, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(521, 33);
            this.label3.TabIndex = 2;
            this.label3.Text = "Vui lòng không dùng: Mũ, khẩu trang, kính...";
            // 
            // btn_Start
            // 
            this.btn_Start.Appearance.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Start.Appearance.Options.UseFont = true;
            this.btn_Start.Image = ((System.Drawing.Image)(resources.GetObject("btn_Start.Image")));
            this.btn_Start.Location = new System.Drawing.Point(47, 451);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(265, 34);
            this.btn_Start.TabIndex = 3;
            this.btn_Start.Text = "Bắt đầu thực hiện";
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // imageBoxFrameGrabber
            // 
            this.imageBoxFrameGrabber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageBoxFrameGrabber.Location = new System.Drawing.Point(10, 7);
            this.imageBoxFrameGrabber.Name = "imageBoxFrameGrabber";
            this.imageBoxFrameGrabber.Size = new System.Drawing.Size(359, 301);
            this.imageBoxFrameGrabber.TabIndex = 5;
            this.imageBoxFrameGrabber.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Controls.Add(this.imageBoxFrameGrabber);
            this.panel2.Location = new System.Drawing.Point(12, 129);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(376, 316);
            this.panel2.TabIndex = 4;
            // 
            // frm_Face
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(761, 496);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_Face";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ib_face)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBoxFrameGrabber)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SimpleButton btn_Start;
        private DevExpress.XtraEditors.SimpleButton btn_OK;
        private DevExpress.XtraEditors.SimpleButton btn_TryAgain;
        private Emgu.CV.UI.ImageBox imageBoxFrameGrabber;
        private System.Windows.Forms.Panel panel2;
        private Emgu.CV.UI.ImageBox ib_face;
    }
}

