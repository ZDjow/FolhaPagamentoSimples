namespace FolhaPagamentoSimples.Presentation
{
    partial class FmGUI
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnCarregarArquivo;
        private System.Windows.Forms.DataGridView dgFolhas;

        private string FormatarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                return cpf; 

            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnCarregarArquivo = new Button();
            dgFolhas = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgFolhas).BeginInit();
            SuspendLayout();
            // 
            // btnCarregarArquivo
            // 
            btnCarregarArquivo.Location = new Point(15, 493);
            btnCarregarArquivo.Margin = new Padding(4, 3, 4, 3);
            btnCarregarArquivo.Name = "btnCarregarArquivo";
            btnCarregarArquivo.Size = new Size(117, 27);
            btnCarregarArquivo.TabIndex = 0;
            btnCarregarArquivo.Text = "Carregar Arquivo";
            btnCarregarArquivo.UseVisualStyleBackColor = true;
            btnCarregarArquivo.Click += btnCarregarArquivoClick;
            // 
            // dgFolhas
            // 
            dgFolhas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgFolhas.Location = new Point(15, 12);
            dgFolhas.Margin = new Padding(4, 3, 4, 3);
            dgFolhas.Name = "dgFolhas";
            dgFolhas.Size = new Size(887, 462);
            dgFolhas.TabIndex = 1;
            // 
            // FmGUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(915, 532);
            Controls.Add(dgFolhas);
            Controls.Add(btnCarregarArquivo);
            Margin = new Padding(4, 3, 4, 3);
            Name = "FmGUI";
            Text = "Apurador de Folha de Pagamento Simples";
            ((System.ComponentModel.ISupportInitialize)dgFolhas).EndInit();
            ResumeLayout(false);
        }
    }
}
