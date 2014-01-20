namespace FourSquareData
{
    partial class WebCredentials
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
            this.wb4Sq = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // wb4Sq
            // 
            this.wb4Sq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb4Sq.Location = new System.Drawing.Point(0, 0);
            this.wb4Sq.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb4Sq.Name = "wb4Sq";
            this.wb4Sq.ScriptErrorsSuppressed = true;
            this.wb4Sq.Size = new System.Drawing.Size(749, 520);
            this.wb4Sq.TabIndex = 0;
            this.wb4Sq.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wb4Sq_DocumentCompleted);
            // 
            // WebCredentials
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 520);
            this.ControlBox = false;
            this.Controls.Add(this.wb4Sq);
            this.Name = "WebCredentials";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Four Square";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser wb4Sq;
    }
}