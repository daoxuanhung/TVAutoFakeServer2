
namespace TVAutoFakeServer2
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnInstallNic = new System.Windows.Forms.Button();
            this.btnHTTPServer = new System.Windows.Forms.Button();
            this.btnServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInstallNic
            // 
            this.btnInstallNic.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInstallNic.Location = new System.Drawing.Point(12, 12);
            this.btnInstallNic.Name = "btnInstallNic";
            this.btnInstallNic.Size = new System.Drawing.Size(123, 54);
            this.btnInstallNic.TabIndex = 0;
            this.btnInstallNic.Text = "Cài card loopback";
            this.btnInstallNic.UseVisualStyleBackColor = true;
            this.btnInstallNic.Click += new System.EventHandler(this.btnInstallNic_Click);
            // 
            // btnHTTPServer
            // 
            this.btnHTTPServer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHTTPServer.Location = new System.Drawing.Point(141, 13);
            this.btnHTTPServer.Name = "btnHTTPServer";
            this.btnHTTPServer.Size = new System.Drawing.Size(123, 53);
            this.btnHTTPServer.TabIndex = 1;
            this.btnHTTPServer.Text = "Bật HTTP Server";
            this.btnHTTPServer.UseVisualStyleBackColor = true;
            this.btnHTTPServer.Click += new System.EventHandler(this.btnHTTPServer_Click);
            // 
            // btnServer
            // 
            this.btnServer.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnServer.Location = new System.Drawing.Point(270, 14);
            this.btnServer.Name = "btnServer";
            this.btnServer.Size = new System.Drawing.Size(123, 51);
            this.btnServer.TabIndex = 2;
            this.btnServer.Text = "Bật server";
            this.btnServer.UseVisualStyleBackColor = true;
            this.btnServer.Click += new System.EventHandler(this.btnServer_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 78);
            this.Controls.Add(this.btnServer);
            this.Controls.Add(this.btnHTTPServer);
            this.Controls.Add(this.btnInstallNic);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TVAuto Fake Server";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInstallNic;
        private System.Windows.Forms.Button btnHTTPServer;
        private System.Windows.Forms.Button btnServer;
    }
}

