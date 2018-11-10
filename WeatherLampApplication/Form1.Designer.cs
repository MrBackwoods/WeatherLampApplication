namespace WeatherLampApplication
{
    partial class WeatherLampApplicationForm
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
            this.LampToggleButton = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // LampToggleButton
            // 
            this.LampToggleButton.BackColor = System.Drawing.Color.PaleGreen;
            this.LampToggleButton.Font = new System.Drawing.Font("Built Titling Rg", 50F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LampToggleButton.Location = new System.Drawing.Point(12, 12);
            this.LampToggleButton.Name = "LampToggleButton";
            this.LampToggleButton.Size = new System.Drawing.Size(406, 93);
            this.LampToggleButton.TabIndex = 0;
            this.LampToggleButton.Text = "Enable";
            this.LampToggleButton.UseVisualStyleBackColor = false;
            this.LampToggleButton.Click += new System.EventHandler(this.LampToggleButton_Click);
            // 
            // LogBox
            // 
            this.LogBox.FormattingEnabled = true;
            this.LogBox.Location = new System.Drawing.Point(12, 122);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(406, 199);
            this.LogBox.TabIndex = 1;
            // 
            // WeatherLampApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 339);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.LampToggleButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(448, 378);
            this.MinimumSize = new System.Drawing.Size(448, 378);
            this.Name = "WeatherLampApplicationForm";
            this.Text = "WeatherLampApplication";
            this.Load += new System.EventHandler(this.WeatherLampApplicationForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LampToggleButton;
        private System.Windows.Forms.ListBox LogBox;
    }
}

