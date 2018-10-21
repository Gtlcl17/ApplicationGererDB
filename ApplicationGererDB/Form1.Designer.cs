namespace ApplicationGererDB
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
            this.pageFaction1 = new ApplicationGererDB.PageFaction();
            this.pageFaction2 = new ApplicationGererDB.PageFaction();
            this.SuspendLayout();
            // 
            // pageFaction1
            // 
            this.pageFaction1.Location = new System.Drawing.Point(55, 42);
            this.pageFaction1.Name = "pageFaction1";
            this.pageFaction1.Size = new System.Drawing.Size(502, 294);
            this.pageFaction1.TabIndex = 0;
            // 
            // pageFaction2
            // 
            this.pageFaction2.Location = new System.Drawing.Point(325, 213);
            this.pageFaction2.Name = "pageFaction2";
            this.pageFaction2.Size = new System.Drawing.Size(440, 8);
            this.pageFaction2.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 403);
            this.Controls.Add(this.pageFaction1);
            this.Controls.Add(this.pageFaction2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

		}

        #endregion

        private PageFaction pageFaction1;
        private PageFaction pageFaction2;
    }
}

