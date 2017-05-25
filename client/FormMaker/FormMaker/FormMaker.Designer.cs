namespace FormMaker
{
    partial class FormMaker
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
            this.bNew = new System.Windows.Forms.Button();
            this.bGen = new System.Windows.Forms.Button();
            this.questions = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.pQuestions = new System.Windows.Forms.Panel();
            this.lResultLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.questions)).BeginInit();
            this.SuspendLayout();
            // 
            // bNew
            // 
            this.bNew.Location = new System.Drawing.Point(23, 14);
            this.bNew.Name = "bNew";
            this.bNew.Size = new System.Drawing.Size(77, 23);
            this.bNew.TabIndex = 0;
            this.bNew.Text = "Nowy arkusz";
            this.bNew.UseVisualStyleBackColor = true;
            this.bNew.Click += new System.EventHandler(this.bNew_Click);
            // 
            // bGen
            // 
            this.bGen.Location = new System.Drawing.Point(106, 14);
            this.bGen.Name = "bGen";
            this.bGen.Size = new System.Drawing.Size(149, 23);
            this.bGen.TabIndex = 3;
            this.bGen.Text = "Generacja";
            this.bGen.UseVisualStyleBackColor = true;
            this.bGen.Click += new System.EventHandler(this.bGen_Click);
            // 
            // questions
            // 
            this.questions.Location = new System.Drawing.Point(106, 50);
            this.questions.Name = "questions";
            this.questions.Size = new System.Drawing.Size(149, 20);
            this.questions.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Ilość pytań";
            // 
            // pQuestions
            // 
            this.pQuestions.AutoScroll = true;
            this.pQuestions.Location = new System.Drawing.Point(26, 117);
            this.pQuestions.Name = "pQuestions";
            this.pQuestions.Size = new System.Drawing.Size(229, 378);
            this.pQuestions.TabIndex = 8;
            // 
            // lResultLabel
            // 
            this.lResultLabel.AutoSize = true;
            this.lResultLabel.Location = new System.Drawing.Point(37, 90);
            this.lResultLabel.Name = "lResultLabel";
            this.lResultLabel.Size = new System.Drawing.Size(0, 13);
            this.lResultLabel.TabIndex = 9;
            // 
            // FormMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 507);
            this.Controls.Add(this.lResultLabel);
            this.Controls.Add(this.pQuestions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.questions);
            this.Controls.Add(this.bGen);
            this.Controls.Add(this.bNew);
            this.Name = "FormMaker";
            this.Text = "Generator arkuszy";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.questions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bNew;
        private System.Windows.Forms.Button bGen;
        private System.Windows.Forms.NumericUpDown questions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pQuestions;
        private System.Windows.Forms.Label lResultLabel;
    }
}

