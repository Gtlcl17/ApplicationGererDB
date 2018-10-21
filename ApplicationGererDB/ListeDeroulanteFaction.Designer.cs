namespace ApplicationGererDB
{
    partial class ListeDeroulanteFaction
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

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboFaction = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboFaction
            // 
            this.comboFaction.FormattingEnabled = true;
            this.comboFaction.Location = new System.Drawing.Point(4, 4);
            this.comboFaction.Name = "comboFaction";
            this.comboFaction.Size = new System.Drawing.Size(355, 24);
            this.comboFaction.TabIndex = 0;
            // 
            // ListeDeroulanteFaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboFaction);
            this.Name = "ListeDeroulanteFaction";
            this.Size = new System.Drawing.Size(362, 36);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboFaction;
    }
}
