using FolhaPagamentoSimples.Business.Base;
using FolhaPagamentoSimples.Business.Entidades;
using FolhaPagamentoSimples.Business.Regras;
using FolhaPagamentoSimples.Business.Utils;

namespace FolhaPagamentoSimples.Console
{
    class ConsoleWK
    {
        static void Main(string[] args)
        {
            try
            {
                System.Console.Write("Insira o diretório do arquivo Parametros.json ou pressione 'Enter' para usar o padrão: ");
                string entradaCaminho = System.Console.ReadLine();

                string caminhoParametros = string.IsNullOrWhiteSpace(entradaCaminho)
                    ? Path.Combine(AppContext.BaseDirectory, @"..\Parametros.json")
                    : entradaCaminho;

                if (!File.Exists(caminhoParametros))
                {
                    System.Console.WriteLine($"Arquivo com os parâmetros para os cálculos não encontrado: {caminhoParametros}");
                    return;
                }

                System.Console.Write("Insira o(s) diretório(s) do(s) arquivo(s) CSV separados por vírgula: ");
                string entradaConsole = System.Console.ReadLine();
                List<string> diretoriosCSV = entradaConsole.Split(',').Select(p => p.Trim()).ToList();

                if (!File.Exists(caminhoParametros))
                {
                    System.Console.WriteLine($"Arquivo com os parâmetros para os cálculos não encontrado: {caminhoParametros}");
                    return;
                }

                Comuns parametros = ProcessaParametros.Processar(caminhoParametros);
                ApurarFolhaPagamento apurarFolha = new(parametros);
                CSVLayout CSV = new();

                var listaFolhas = new List<Rubrica>();

                //Procura todos os diretórios informados e processa cada arquivo.
                foreach (var diretorio in diretoriosCSV)
                {
                    if (!File.Exists(diretorio))
                    {
                        System.Console.WriteLine($"Arquivo não encontrado: {diretorio}");
                        continue;
                    }

                    var listasRubricas = CSV.ProcessarArquivo(diretorio);

                    if (listasRubricas == null || listasRubricas.Count == 0)
                    {
                        System.Console.WriteLine($"Nenhuma dado encontrado no arquivo: {diretorio}");
                        continue;
                    }

                    listaFolhas.AddRange(listasRubricas);
                }

                if (listaFolhas.Count == 0)
                {
                    System.Console.WriteLine("Nenhuma rubrica encontrada.");
                    return;
                }

                // Agrupa as rubricas por empregado e calcula a folha de pagamento.
                var folhas = listaFolhas
                    .GroupBy(r => r.Empregado)
                    .Select(g => apurarFolha.CalcularRubricas(g.ToList()))
                    .OrderBy(f => f.Empregado.CPF)
                    .ToList();

                ExibirFolhasPagamento(folhas);
                ExportarFolhasPagamento(folhas);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Erro durante a execução: {ex.Message}");
            }
        }

        private static string FormatarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                return cpf;

            return Convert.ToUInt64(cpf).ToString(@"000\.000\.000\-00");
        }

        private static void ExibirFolhasPagamento(List<FolhaPagamento> folhas)
        {
            System.Console.WriteLine("\nFolha(s) apurada(s):\n");
            System.Console.WriteLine("CPF | Nome | Base INSS | Base IR | Desconto INSS | Desconto IR | Desconto Dependentes | Valor Líquido");

            foreach (var folha in folhas)
            {
                System.Console.WriteLine(
                    $"{FormatarCPF(folha.Empregado.CPF)} | " +
                    $"{folha.Empregado.Nome} | " +
                    $"{folha.BaseCalculoINSS:C2} | " +
                    $"{folha.BaseCalculoIR:C2} | " +
                    $"{folha.DescontoINSS:C2} | " +
                    $"{folha.DescontoIR:C2} | " +
                    $"{folha.DescontoDependentes:C2} | " +
                    $"{folha.ValorLiquido:C2}");
            }
        }

        private static void ExportarFolhasPagamento(List<FolhaPagamento> folhasProcessadas)
        {
            string arquivoExportado = "FolhaPagamento.csv";
            ExportarDados.Exportar(arquivoExportado, folhasProcessadas);
            System.Console.WriteLine($"\nFolha(s) de pagamento simples salva(s) em: {Path.GetFullPath(arquivoExportado)}");
        }
    }
}