using FolhaPagamentoSimples.Business.Base;
using FolhaPagamentoSimples.Business.Regras;

namespace FolhaPagamentoSimples.Presentation
{
    public partial class FmGUI : Form
    {
        private readonly Comuns comunsParametros;
        private readonly ApurarFolhaPagamento apurarFolha;
        private readonly CSVLayout CSV;

        public FmGUI()
        {
            InitializeComponent();

            try
            {
                string caminhoParametros = Path.Combine(AppContext.BaseDirectory, @"..\Parametros.json");
                comunsParametros = ProcessaParametros.Processar(caminhoParametros);
                apurarFolha = new ApurarFolhaPagamento(comunsParametros);
                CSV = new CSVLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar o sistema: {ex.Message}",
                                 "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void btnCarregarArquivoClick(object sender, EventArgs e)
        {
            using (var arquivosSelecionados = new OpenFileDialog())
            {
                arquivosSelecionados.Filter = "CSV files (*.csv)|*.csv";
                if (arquivosSelecionados.ShowDialog() == DialogResult.OK)
                {
                    ProcessarArquivos(arquivosSelecionados.FileName);
                }
            }
        }

        private void ProcessarArquivos(string diretorio)
        {
            try
            {
                var rubricas = CSV.ProcessarArquivo(diretorio);

                // Agrupa as rubricas por cada empregado
                var folhas = rubricas
                    .GroupBy(r => r.Empregado)
                    .Select(g => new
                    {
                        Empregado = g.Key,
                        Folha = apurarFolha.CalcularRubricas(g.ToList())
                    })
                    .OrderBy(r => r.Empregado.CPF)
                    .ToList();

                // Prepara os dados para exibição
                var exibirFolhas = folhas.Select(r => new
                {
                    CPF = FormatarCPF(r.Empregado.CPF),
                    r.Empregado.Nome,                    
                    r.Folha.BaseCalculoINSS,
                    r.Folha.BaseCalculoIR,
                    r.Folha.DescontoINSS,
                    r.Folha.DescontoIR,
                    r.Folha.DescontoDependentes,
                    r.Folha.ValorLiquido
                }).ToList();

                dgFolhas.DataSource = exibirFolhas;

                // Formata o cabeçalho das colunas
                dgFolhas.Columns["BaseCalculoINSS"].HeaderText = "Base de Cálculo INSS";
                dgFolhas.Columns["BaseCalculoIR"].HeaderText = "Base de Cálculo IR";
                dgFolhas.Columns["DescontoINSS"].HeaderText = "Desconto INSS";
                dgFolhas.Columns["DescontoIR"].HeaderText = "Desconto IR";
                dgFolhas.Columns["DescontoDependentes"].HeaderText = "Desconto Dependentes";
                dgFolhas.Columns["ValorLiquido"].HeaderText = "Valor Líquido";
                
                // Formata as colunas numéricas para ficarem como moeda
                dgFolhas.Columns["BaseCalculoINSS"].DefaultCellStyle.Format = "C2";
                dgFolhas.Columns["BaseCalculoIR"].DefaultCellStyle.Format = "C2";
                dgFolhas.Columns["DescontoINSS"].DefaultCellStyle.Format = "C2";
                dgFolhas.Columns["DescontoIR"].DefaultCellStyle.Format = "C2";
                dgFolhas.Columns["DescontoDependentes"].DefaultCellStyle.Format = "C2";
                dgFolhas.Columns["ValorLiquido"].DefaultCellStyle.Format = "C2";


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao processar o arquivo: {ex.Message}",
                                 "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}