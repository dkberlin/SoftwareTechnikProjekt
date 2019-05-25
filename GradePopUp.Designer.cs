namespace SoftwareTechnikProjekt
{
    partial class GradePopUp
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
            this.label1 = new System.Windows.Forms.Label();
            this.GradeDropDown = new System.Windows.Forms.ComboBox();
            this.GradeSubmitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please Choose Your Grade";
            // 
            // GradeDropDown
            // 
            this.GradeDropDown.FormattingEnabled = true;
            this.GradeDropDown.Location = new System.Drawing.Point(106, 53);
            this.GradeDropDown.Name = "GradeDropDown";
            this.GradeDropDown.Size = new System.Drawing.Size(121, 21);
            this.GradeDropDown.TabIndex = 1;
            // 
            // GradeSubmitButton
            // 
            this.GradeSubmitButton.Location = new System.Drawing.Point(128, 96);
            this.GradeSubmitButton.Name = "GradeSubmitButton";
            this.GradeSubmitButton.Size = new System.Drawing.Size(75, 23);
            this.GradeSubmitButton.TabIndex = 2;
            this.GradeSubmitButton.Text = "Submit";
            this.GradeSubmitButton.UseVisualStyleBackColor = true;
            this.GradeSubmitButton.Click += new System.EventHandler(this.GradeSubmitButton_Click);
            // 
            // GradePopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 134);
            this.Controls.Add(this.GradeSubmitButton);
            this.Controls.Add(this.GradeDropDown);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GradePopUp";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose Your Grade";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GradePopUp_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox GradeDropDown;
        private System.Windows.Forms.Button GradeSubmitButton;
    }
}