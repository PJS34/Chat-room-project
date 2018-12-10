namespace Graphics
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.sayingbox = new System.Windows.Forms.TextBox();
            this.Chat = new System.Windows.Forms.TextBox();
            this.sendbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sayingbox
            // 
            this.sayingbox.Location = new System.Drawing.Point(131, 329);
            this.sayingbox.Name = "sayingbox";
            this.sayingbox.Size = new System.Drawing.Size(624, 26);
            this.sayingbox.TabIndex = 0;
            this.sayingbox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Chat
            // 
            this.Chat.Location = new System.Drawing.Point(131, 30);
            this.Chat.Multiline = true;
            this.Chat.Name = "Chat";
            this.Chat.ReadOnly = true;
            this.Chat.Size = new System.Drawing.Size(623, 287);
            this.Chat.TabIndex = 1;
            // 
            // sendbutton
            // 
            this.sendbutton.Location = new System.Drawing.Point(698, 361);
            this.sendbutton.Name = "sendbutton";
            this.sendbutton.Size = new System.Drawing.Size(75, 23);
            this.sendbutton.TabIndex = 2;
            this.sendbutton.Text = "send";
            this.sendbutton.UseVisualStyleBackColor = true;
            this.sendbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sendbutton);
            this.Controls.Add(this.Chat);
            this.Controls.Add(this.sayingbox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sayingbox;
        private System.Windows.Forms.TextBox Chat;
        private System.Windows.Forms.Button sendbutton;
    }
}

