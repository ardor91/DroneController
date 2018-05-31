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
            this.label7 = new System.Windows.Forms.Label();
            this.lblAngle = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lblLessCount = new System.Windows.Forms.Label();
            this.lblMoreCount = new System.Windows.Forms.Label();
            this.lblUpCount = new System.Windows.Forms.Label();
            this.lblDownCount = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.nudAngle = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.nudWaterSpread = new System.Windows.Forms.NumericUpDown();
            this.showTemp = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaterSpread)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(712, 712);
            this.panel1.TabIndex = 0;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
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
            this.markupBtn.Click += new System.EventHandler(this.markupBtn_Click);
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
            this.txtWaterSpread.Text = "20";
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(181, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Result Angle:";
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Location = new System.Drawing.Point(264, 29);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(13, 13);
            this.lblAngle.TabIndex = 17;
            this.lblAngle.Text = "0";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1066, 70);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 55);
            this.button1.TabIndex = 18;
            this.button1.Text = "Draw Rect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(358, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(184, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Points:  LESS   MORE   UP    DOWN";
            // 
            // lblLessCount
            // 
            this.lblLessCount.AutoSize = true;
            this.lblLessCount.Location = new System.Drawing.Point(407, 29);
            this.lblLessCount.Name = "lblLessCount";
            this.lblLessCount.Size = new System.Drawing.Size(13, 13);
            this.lblLessCount.TabIndex = 20;
            this.lblLessCount.Text = "0";
            // 
            // lblMoreCount
            // 
            this.lblMoreCount.AutoSize = true;
            this.lblMoreCount.Location = new System.Drawing.Point(446, 29);
            this.lblMoreCount.Name = "lblMoreCount";
            this.lblMoreCount.Size = new System.Drawing.Size(13, 13);
            this.lblMoreCount.TabIndex = 21;
            this.lblMoreCount.Text = "0";
            // 
            // lblUpCount
            // 
            this.lblUpCount.AutoSize = true;
            this.lblUpCount.Location = new System.Drawing.Point(478, 29);
            this.lblUpCount.Name = "lblUpCount";
            this.lblUpCount.Size = new System.Drawing.Size(13, 13);
            this.lblUpCount.TabIndex = 22;
            this.lblUpCount.Text = "0";
            // 
            // lblDownCount
            // 
            this.lblDownCount.AutoSize = true;
            this.lblDownCount.Location = new System.Drawing.Point(512, 29);
            this.lblDownCount.Name = "lblDownCount";
            this.lblDownCount.Size = new System.Drawing.Size(13, 13);
            this.lblDownCount.TabIndex = 23;
            this.lblDownCount.Text = "0";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1066, 148);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 55);
            this.button2.TabIndex = 24;
            this.button2.Text = "Clear Debug";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1066, 209);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(146, 55);
            this.button3.TabIndex = 25;
            this.button3.Text = "Clear Lines";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1066, 270);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(146, 55);
            this.button4.TabIndex = 26;
            this.button4.Text = "Stop Timer";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(571, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 27;
            this.label9.Text = "Angle";
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(611, 6);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(65, 20);
            this.txtAngle.TabIndex = 28;
            this.txtAngle.Text = "0";
            // 
            // nudAngle
            // 
            this.nudAngle.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudAngle.Location = new System.Drawing.Point(682, 7);
            this.nudAngle.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.nudAngle.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.nudAngle.Name = "nudAngle";
            this.nudAngle.Size = new System.Drawing.Size(42, 20);
            this.nudAngle.TabIndex = 29;
            this.nudAngle.ValueChanged += new System.EventHandler(this.nudAngle_ValueChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(1073, 349);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(86, 17);
            this.checkBox1.TabIndex = 30;
            this.checkBox1.Text = "showOriginal";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // nudWaterSpread
            // 
            this.nudWaterSpread.Location = new System.Drawing.Point(1120, 22);
            this.nudWaterSpread.Maximum = new decimal(new int[] {
            3650,
            0,
            0,
            0});
            this.nudWaterSpread.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWaterSpread.Name = "nudWaterSpread";
            this.nudWaterSpread.Size = new System.Drawing.Size(42, 20);
            this.nudWaterSpread.TabIndex = 31;
            this.nudWaterSpread.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudWaterSpread.ValueChanged += new System.EventHandler(this.nudWaterSpread_ValueChanged);
            // 
            // showTemp
            // 
            this.showTemp.AutoSize = true;
            this.showTemp.Location = new System.Drawing.Point(1073, 372);
            this.showTemp.Name = "showTemp";
            this.showTemp.Size = new System.Drawing.Size(74, 17);
            this.showTemp.TabIndex = 32;
            this.showTemp.Text = "showBuild";
            this.showTemp.UseVisualStyleBackColor = true;
            this.showTemp.CheckedChanged += new System.EventHandler(this.showTemp_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 795);
            this.Controls.Add(this.showTemp);
            this.Controls.Add(this.nudWaterSpread);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.nudAngle);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblDownCount);
            this.Controls.Add(this.lblUpCount);
            this.Controls.Add(this.lblMoreCount);
            this.Controls.Add(this.lblLessCount);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.label7);
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
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaterSpread)).EndInit();
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblAngle;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblLessCount;
        private System.Windows.Forms.Label lblMoreCount;
        private System.Windows.Forms.Label lblUpCount;
        private System.Windows.Forms.Label lblDownCount;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.NumericUpDown nudAngle;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.NumericUpDown nudWaterSpread;
        private System.Windows.Forms.CheckBox showTemp;
    }
}

