using FolhaPagamentoSimples.Business.Entidades;

namespace FolhaPagamentoSimples.Business.Utils
{
    public class ExportarDados
    {
        // Exporta as folhas de pagamento em um arquivo CSV.
        public static void Exportar(string diretorio, List<FolhaPagamento> folhasPagamento)
        {
            using (var gravar = new StreamWriter(diretorio))
            {                
                gravar.WriteLine("CPF;Nome;Base INSS;Base IR;Desconto INSS;Desconto IR;Desconto Dependentes;Valor Liquido");

                foreach (var folha in folhasPagamento)
                {
                    gravar.WriteLine($"{folha.Empregado.CPF};{folha.Empregado.Nome};{folha.BaseCalculoINSS:F2};{folha.BaseCalculoIR:F2};{folha.DescontoINSS:F2};{folha.DescontoIR:F2};{folha.DescontoDependentes:F2};{folha.ValorLiquido:F2}");
                }
            }
        }
    }
}
