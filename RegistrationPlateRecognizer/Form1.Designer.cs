namespace RegistrationPlateRecognizer
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
            this.recognize = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cannyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // recognize
            // 
            this.recognize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.recognize.Location = new System.Drawing.Point(101, 86);
            this.recognize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.recognize.Name = "recognize";
            this.recognize.Size = new System.Drawing.Size(164, 117);
            this.recognize.TabIndex = 0;
            this.recognize.Text = "recognize";
            this.recognize.UseVisualStyleBackColor = true;
            this.recognize.Click += new System.EventHandler(this.recognize_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 297);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // cannyButton
            // 
            this.cannyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cannyButton.Location = new System.Drawing.Point(304, 86);
            this.cannyButton.Margin = new System.Windows.Forms.Padding(4);
            this.cannyButton.Name = "cannyButton";
            this.cannyButton.Size = new System.Drawing.Size(164, 117);
            this.cannyButton.TabIndex = 2;
            this.cannyButton.Text = "Canny";
            this.cannyButton.UseVisualStyleBackColor = true;
            this.cannyButton.Click += new System.EventHandler(this.cannyButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 596);
            this.Controls.Add(this.cannyButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.recognize);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button recognize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cannyButton;
    }
}

