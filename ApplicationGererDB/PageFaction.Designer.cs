

namespace ApplicationGererDB
{
    partial class PageFaction
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
            this.listeDeroulanteFaction1 = new ApplicationGererDB.ListeDeroulanteFaction();
            this.label1 = new System.Windows.Forms.Label();
            this.ListeSubFaction = new ApplicationGererDB.ListeDeroulanteFaction();
            this.SuspendLayout();
            // 
            // listeDeroulanteFaction1
            // 
            this.listeDeroulanteFaction1.Location = new System.Drawing.Point(0, 35);
            this.listeDeroulanteFaction1.Name = "listeDeroulanteFaction1";
            this.listeDeroulanteFaction1.Size = new System.Drawing.Size(362, 36);
            this.listeDeroulanteFaction1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Liste des factions:";
            // 
            // ListeSubFaction
            // 
            this.ListeSubFaction.Location = new System.Drawing.Point(3, 77);
            this.ListeSubFaction.Name = "ListeSubFaction";
            this.ListeSubFaction.Size = new System.Drawing.Size(362, 36);
            this.ListeSubFaction.TabIndex = 2;
            // 
            // PageFaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ListeSubFaction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listeDeroulanteFaction1);
            this.Name = "PageFaction";
            this.Size = new System.Drawing.Size(373, 367);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListeDeroulanteFaction listeDeroulanteFaction1;
        private System.Windows.Forms.Label label1;
        private ListeDeroulanteFaction ListeSubFaction;
    }
}
