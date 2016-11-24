namespace SigmakokiSTageTest
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
        protected override void Dispose( bool disposing ) {
            if ( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent( ) {
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnsetMove = new System.Windows.Forms.Button();
            this.btnorigin = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnGetStatus = new System.Windows.Forms.Button();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.txbMovePos = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(118, 58);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnsetMove
            // 
            this.btnsetMove.Location = new System.Drawing.Point(12, 140);
            this.btnsetMove.Name = "btnsetMove";
            this.btnsetMove.Size = new System.Drawing.Size(118, 58);
            this.btnsetMove.TabIndex = 2;
            this.btnsetMove.Text = "SetMovePos";
            this.btnsetMove.UseVisualStyleBackColor = true;
            this.btnsetMove.Click += new System.EventHandler(this.btnsetMove_Click);
            // 
            // btnorigin
            // 
            this.btnorigin.Location = new System.Drawing.Point(12, 76);
            this.btnorigin.Name = "btnorigin";
            this.btnorigin.Size = new System.Drawing.Size(118, 58);
            this.btnorigin.TabIndex = 3;
            this.btnorigin.Text = "Origin";
            this.btnorigin.UseVisualStyleBackColor = true;
            this.btnorigin.Click += new System.EventHandler(this.btnorigin_Click);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(12, 204);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(118, 58);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnGetStatus
            // 
            this.btnGetStatus.Location = new System.Drawing.Point(12, 268);
            this.btnGetStatus.Name = "btnGetStatus";
            this.btnGetStatus.Size = new System.Drawing.Size(118, 58);
            this.btnGetStatus.TabIndex = 5;
            this.btnGetStatus.Text = "Get Status";
            this.btnGetStatus.UseVisualStyleBackColor = true;
            this.btnGetStatus.Click += new System.EventHandler(this.btnGetStatus_Click);
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(156, 32);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(100, 20);
            this.txbPort.TabIndex = 6;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(183, 291);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "label1";
            // 
            // txbMovePos
            // 
            this.txbMovePos.Location = new System.Drawing.Point(156, 160);
            this.txbMovePos.Name = "txbMovePos";
            this.txbMovePos.Size = new System.Drawing.Size(100, 20);
            this.txbMovePos.TabIndex = 8;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(306, 12);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(118, 58);
            this.btnDisconnect.TabIndex = 9;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 333);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.txbMovePos);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.btnGetStatus);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnorigin);
            this.Controls.Add(this.btnsetMove);
            this.Controls.Add(this.btnConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.CursorChanged += new System.EventHandler(this.f);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnsetMove;
        private System.Windows.Forms.Button btnorigin;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnGetStatus;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox txbMovePos;
        private System.Windows.Forms.Button btnDisconnect;
    }
}

