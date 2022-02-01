
namespace PS
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
            this.bttn_Tallozas = new System.Windows.Forms.Button();
            this.originalImage = new System.Windows.Forms.PictureBox();
            this.newImage = new System.Windows.Forms.PictureBox();
            this.bttn_Negalas = new System.Windows.Forms.Button();
            this.bttn_GammaTranszformacio = new System.Windows.Forms.Button();
            this.bttn_LogaritmikusTranszformacio = new System.Windows.Forms.Button();
            this.bttn_Szurkites = new System.Windows.Forms.Button();
            this.bttn_HisztorgramKeszites = new System.Windows.Forms.Button();
            this.bttn_HisztogramKiegyenlites = new System.Windows.Forms.Button();
            this.bttn_AtlagoloSzuro = new System.Windows.Forms.Button();
            this.bttn_GaussSzuro = new System.Windows.Forms.Button();
            this.bttn_SobelEldetektor = new System.Windows.Forms.Button();
            this.bttn_LaplaceEldetektor = new System.Windows.Forms.Button();
            this.bttn_JellemzoPontok = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.originalImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newImage)).BeginInit();
            this.SuspendLayout();
            // 
            // bttn_Tallozas
            // 
            this.bttn_Tallozas.Location = new System.Drawing.Point(418, 117);
            this.bttn_Tallozas.Name = "bttn_Tallozas";
            this.bttn_Tallozas.Size = new System.Drawing.Size(148, 23);
            this.bttn_Tallozas.TabIndex = 0;
            this.bttn_Tallozas.Text = "Tallózás";
            this.bttn_Tallozas.UseVisualStyleBackColor = true;
            this.bttn_Tallozas.Click += new System.EventHandler(this.bttn_Tallozas_Click);
            // 
            // originalImage
            // 
            this.originalImage.Location = new System.Drawing.Point(12, 117);
            this.originalImage.Name = "originalImage";
            this.originalImage.Size = new System.Drawing.Size(400, 400);
            this.originalImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalImage.TabIndex = 1;
            this.originalImage.TabStop = false;
            // 
            // newImage
            // 
            this.newImage.Location = new System.Drawing.Point(572, 117);
            this.newImage.Name = "newImage";
            this.newImage.Size = new System.Drawing.Size(400, 400);
            this.newImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.newImage.TabIndex = 2;
            this.newImage.TabStop = false;
            // 
            // bttn_Negalas
            // 
            this.bttn_Negalas.Location = new System.Drawing.Point(418, 202);
            this.bttn_Negalas.Name = "bttn_Negalas";
            this.bttn_Negalas.Size = new System.Drawing.Size(148, 23);
            this.bttn_Negalas.TabIndex = 3;
            this.bttn_Negalas.Text = "Negálás";
            this.bttn_Negalas.UseVisualStyleBackColor = true;
            this.bttn_Negalas.Click += new System.EventHandler(this.bttn_Negalas_Click);
            // 
            // bttn_GammaTranszformacio
            // 
            this.bttn_GammaTranszformacio.Location = new System.Drawing.Point(418, 231);
            this.bttn_GammaTranszformacio.Name = "bttn_GammaTranszformacio";
            this.bttn_GammaTranszformacio.Size = new System.Drawing.Size(148, 23);
            this.bttn_GammaTranszformacio.TabIndex = 4;
            this.bttn_GammaTranszformacio.Text = "Gamma transzformáció";
            this.bttn_GammaTranszformacio.UseVisualStyleBackColor = true;
            this.bttn_GammaTranszformacio.Click += new System.EventHandler(this.bttn_GammaTranszformacio_Click);
            // 
            // bttn_LogaritmikusTranszformacio
            // 
            this.bttn_LogaritmikusTranszformacio.Location = new System.Drawing.Point(418, 260);
            this.bttn_LogaritmikusTranszformacio.Name = "bttn_LogaritmikusTranszformacio";
            this.bttn_LogaritmikusTranszformacio.Size = new System.Drawing.Size(148, 23);
            this.bttn_LogaritmikusTranszformacio.TabIndex = 5;
            this.bttn_LogaritmikusTranszformacio.Text = "Logaritmikus transzformáció";
            this.bttn_LogaritmikusTranszformacio.UseVisualStyleBackColor = true;
            this.bttn_LogaritmikusTranszformacio.Click += new System.EventHandler(this.bttn_LogaritmikusTranszformacio_Click);
            // 
            // bttn_Szurkites
            // 
            this.bttn_Szurkites.Location = new System.Drawing.Point(418, 289);
            this.bttn_Szurkites.Name = "bttn_Szurkites";
            this.bttn_Szurkites.Size = new System.Drawing.Size(148, 23);
            this.bttn_Szurkites.TabIndex = 6;
            this.bttn_Szurkites.Text = "Szűrkítés";
            this.bttn_Szurkites.UseVisualStyleBackColor = true;
            this.bttn_Szurkites.Click += new System.EventHandler(this.bttn_Szurkites_Click);
            // 
            // bttn_HisztorgramKeszites
            // 
            this.bttn_HisztorgramKeszites.Location = new System.Drawing.Point(418, 318);
            this.bttn_HisztorgramKeszites.Name = "bttn_HisztorgramKeszites";
            this.bttn_HisztorgramKeszites.Size = new System.Drawing.Size(148, 23);
            this.bttn_HisztorgramKeszites.TabIndex = 7;
            this.bttn_HisztorgramKeszites.Text = "Hisztogram készítés";
            this.bttn_HisztorgramKeszites.UseVisualStyleBackColor = true;
            this.bttn_HisztorgramKeszites.Click += new System.EventHandler(this.bttn_HisztorgramKeszites_Click);
            // 
            // bttn_HisztogramKiegyenlites
            // 
            this.bttn_HisztogramKiegyenlites.Location = new System.Drawing.Point(418, 349);
            this.bttn_HisztogramKiegyenlites.Name = "bttn_HisztogramKiegyenlites";
            this.bttn_HisztogramKiegyenlites.Size = new System.Drawing.Size(148, 23);
            this.bttn_HisztogramKiegyenlites.TabIndex = 8;
            this.bttn_HisztogramKiegyenlites.Text = "Hisztogram kiegyenlítés";
            this.bttn_HisztogramKiegyenlites.UseVisualStyleBackColor = true;
            this.bttn_HisztogramKiegyenlites.Click += new System.EventHandler(this.bttn_HisztogramKiegyenlites_Click);
            // 
            // bttn_AtlagoloSzuro
            // 
            this.bttn_AtlagoloSzuro.Location = new System.Drawing.Point(418, 378);
            this.bttn_AtlagoloSzuro.Name = "bttn_AtlagoloSzuro";
            this.bttn_AtlagoloSzuro.Size = new System.Drawing.Size(148, 23);
            this.bttn_AtlagoloSzuro.TabIndex = 9;
            this.bttn_AtlagoloSzuro.Text = "Átlagoló szűrő";
            this.bttn_AtlagoloSzuro.UseVisualStyleBackColor = true;
            this.bttn_AtlagoloSzuro.Click += new System.EventHandler(this.bttn_AtlagoloSzuro_Click);
            // 
            // bttn_GaussSzuro
            // 
            this.bttn_GaussSzuro.Location = new System.Drawing.Point(418, 407);
            this.bttn_GaussSzuro.Name = "bttn_GaussSzuro";
            this.bttn_GaussSzuro.Size = new System.Drawing.Size(148, 23);
            this.bttn_GaussSzuro.TabIndex = 10;
            this.bttn_GaussSzuro.Text = "Gauss szűrő";
            this.bttn_GaussSzuro.UseVisualStyleBackColor = true;
            this.bttn_GaussSzuro.Click += new System.EventHandler(this.bttn_GaussSzuro_Click);
            // 
            // bttn_SobelEldetektor
            // 
            this.bttn_SobelEldetektor.Location = new System.Drawing.Point(418, 436);
            this.bttn_SobelEldetektor.Name = "bttn_SobelEldetektor";
            this.bttn_SobelEldetektor.Size = new System.Drawing.Size(148, 23);
            this.bttn_SobelEldetektor.TabIndex = 11;
            this.bttn_SobelEldetektor.Text = "Sobel éldetektor";
            this.bttn_SobelEldetektor.UseVisualStyleBackColor = true;
            this.bttn_SobelEldetektor.Click += new System.EventHandler(this.bttn_SobelEldetektor_Click);
            // 
            // bttn_LaplaceEldetektor
            // 
            this.bttn_LaplaceEldetektor.Location = new System.Drawing.Point(418, 465);
            this.bttn_LaplaceEldetektor.Name = "bttn_LaplaceEldetektor";
            this.bttn_LaplaceEldetektor.Size = new System.Drawing.Size(148, 23);
            this.bttn_LaplaceEldetektor.TabIndex = 12;
            this.bttn_LaplaceEldetektor.Text = "Laplace éldetektor";
            this.bttn_LaplaceEldetektor.UseVisualStyleBackColor = true;
            this.bttn_LaplaceEldetektor.Click += new System.EventHandler(this.bttn_LaplaceEldetektor_Click);
            // 
            // bttn_JellemzoPontok
            // 
            this.bttn_JellemzoPontok.Location = new System.Drawing.Point(418, 494);
            this.bttn_JellemzoPontok.Name = "bttn_JellemzoPontok";
            this.bttn_JellemzoPontok.Size = new System.Drawing.Size(148, 23);
            this.bttn_JellemzoPontok.TabIndex = 13;
            this.bttn_JellemzoPontok.Text = "Jellemző pontok detektálása";
            this.bttn_JellemzoPontok.UseVisualStyleBackColor = true;
            this.bttn_JellemzoPontok.Click += new System.EventHandler(this.bttn_JellemzoPontok_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.bttn_JellemzoPontok);
            this.Controls.Add(this.bttn_LaplaceEldetektor);
            this.Controls.Add(this.bttn_SobelEldetektor);
            this.Controls.Add(this.bttn_GaussSzuro);
            this.Controls.Add(this.bttn_AtlagoloSzuro);
            this.Controls.Add(this.bttn_HisztogramKiegyenlites);
            this.Controls.Add(this.bttn_HisztorgramKeszites);
            this.Controls.Add(this.bttn_Szurkites);
            this.Controls.Add(this.bttn_LogaritmikusTranszformacio);
            this.Controls.Add(this.bttn_GammaTranszformacio);
            this.Controls.Add(this.bttn_Negalas);
            this.Controls.Add(this.newImage);
            this.Controls.Add(this.originalImage);
            this.Controls.Add(this.bttn_Tallozas);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.originalImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bttn_Tallozas;
        private System.Windows.Forms.PictureBox originalImage;
        private System.Windows.Forms.PictureBox newImage;
        private System.Windows.Forms.Button bttn_Negalas;
        private System.Windows.Forms.Button bttn_GammaTranszformacio;
        private System.Windows.Forms.Button bttn_LogaritmikusTranszformacio;
        private System.Windows.Forms.Button bttn_Szurkites;
        private System.Windows.Forms.Button bttn_HisztorgramKeszites;
        private System.Windows.Forms.Button bttn_HisztogramKiegyenlites;
        private System.Windows.Forms.Button bttn_AtlagoloSzuro;
        private System.Windows.Forms.Button bttn_GaussSzuro;
        private System.Windows.Forms.Button bttn_SobelEldetektor;
        private System.Windows.Forms.Button bttn_LaplaceEldetektor;
        private System.Windows.Forms.Button bttn_JellemzoPontok;
    }
}

