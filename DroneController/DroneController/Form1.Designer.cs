namespace DroneController
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.startBtn = new System.Windows.Forms.Button();
            this.markupBtn = new System.Windows.Forms.Button();
            this.txtDroneCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPickStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWaterSpread = new System.Windows.Forms.TextBox();
            this.btnDrawPolygon = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCursorX = new System.Windows.Forms.Label();
            this.lblCursorY = new System.Windows.Forms.Label();
            this.drawTimer = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.lblSquare = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(886, 487);
            this.panel1.TabIndex = 0;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(914, 502);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(146, 55);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // markupBtn
            // 
            this.markupBtn.Location = new System.Drawing.Point(914, 209);
            this.markupBtn.Name = "markupBtn";
            this.markupBtn.Size = new System.Drawing.Size(146, 55);
            this.markupBtn.TabIndex = 2;
            this.markupBtn.Text = "Create Paths";
            this.markupBtn.UseVisualStyleBackColor = true;
            // 
            // txtDroneCount
            // 
            this.txtDroneCount.Location = new System.Drawing.Point(852, 22);
            this.txtDroneCount.Name = "txtDroneCount";
            this.txtDroneCount.Size = new System.Drawing.Size(65, 20);
            this.txtDroneCount.TabIndex = 3;
            this.txtDroneCount.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(782, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Drone Count";
            // 
            // btnPickStart
            // 
            this.btnPickStart.Location = new System.Drawing.Point(914, 148);
            this.btnPickStart.Name = "btnPickStart";
            this.btnPickStart.Size = new System.Drawing.Size(146, 55);
            this.btnPickStart.TabIndex = 5;
            this.btnPickStart.Text = "Pick start point";
            this.btnPickStart.UseVisualStyleBackColor = true;
            this.btnPickStart.Click += new System.EventHandler(this.btnPickStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(947, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Water Spread (m2)";
            // 
            // txtWaterSpread
            // 
            this.txtWaterSpread.Location = new System.Drawing.Point(1049, 22);
            this.txtWaterSpread.Name = "txtWaterSpread";
            this.txtWaterSpread.Size = new System.Drawing.Size(65, 20);
            this.txtWaterSpread.TabIndex = 6;
            this.txtWaterSpread.Text = "1";
            // 
            // btnDrawPolygon
            // 
            this.btnDrawPolygon.Location = new System.Drawing.Point(914, 70);
            this.btnDrawPolygon.Name = "btnDrawPolygon";
            this.btnDrawPolygon.Size = new System.Drawing.Size(146, 55);
            this.btnDrawPolygon.TabIndex = 8;
            this.btnDrawPolygon.Text = "Draw Polygon";
            this.btnDrawPolygon.UseVisualStyleBackColor = true;
            this.btnDrawPolygon.Click += new System.EventHandler(this.btnDrawPolygon_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Cursor Position:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "X:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Y:";
            // 
            // lblCursorX
            // 
            this.lblCursorX.AutoSize = true;
            this.lblCursorX.Location = new System.Drawing.Point(54, 29);
            this.lblCursorX.Name = "lblCursorX";
            this.lblCursorX.Size = new System.Drawing.Size(13, 13);
            this.lblCursorX.TabIndex = 12;
            this.lblCursorX.Text = "0";
            // 
            // lblCursorY
            // 
            this.lblCursorY.AutoSize = true;
            this.lblCursorY.Location = new System.Drawing.Point(54, 49);
            this.lblCursorY.Name = "lblCursorY";
            this.lblCursorY.Size = new System.Drawing.Size(13, 13);
            this.lblCursorY.TabIndex = 13;
            this.lblCursorY.Text = "0";
            // 
            // drawTimer
            // 
            this.drawTimer.Enabled = true;
            this.drawTimer.Tick += new System.EventHandler(this.drawTimer_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Result Square:";
            // 
            // lblSquare
            // 
            this.lblSquare.AutoSize = true;
            this.lblSquare.Location = new System.Drawing.Point(264, 9);
            this.lblSquare.Name = "lblSquare";
            this.lblSquare.Size = new System.Drawing.Size(13, 13);
            this.lblSquare.TabIndex = 15;
            this.lblSquare.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 591);
            this.Controls.Add(this.lblSquare);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblCursorY);
            this.Controls.Add(this.lblCursorX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDrawPolygon);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtWaterSpread);
            this.Controls.Add(this.btnPickStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDroneCount);
            this.Controls.Add(this.markupBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button markupBtn;
        private System.Windows.Forms.TextBox txtDroneCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPickStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtWaterSpread;
        private System.Windows.Forms.Button btnDrawPolygon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCursorX;
        private System.Windows.Forms.Label lblCursorY;
        private System.Windows.Forms.Timer drawTimer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSquare;
    }
}

