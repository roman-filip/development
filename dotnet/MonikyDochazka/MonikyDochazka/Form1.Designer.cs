namespace MonikyDochazka
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
            this.btnWorkStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnWorkStart
            // 
            this.btnWorkStart.Location = new System.Drawing.Point(12, 12);
            this.btnWorkStart.Name = "btnWorkStart";
            this.btnWorkStart.Size = new System.Drawing.Size(122, 23);
            this.btnWorkStart.TabIndex = 0;
            this.btnWorkStart.Text = "Prichod do rachoty";
            this.btnWorkStart.UseVisualStyleBackColor = true;
            this.btnWorkStart.Click += new System.EventHandler(this.btnWorkStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 403);
            this.Controls.Add(this.btnWorkStart);
            this.Name = "Form1";
            this.Text = "Moniky dochazka";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnWorkStart;
    }
}

