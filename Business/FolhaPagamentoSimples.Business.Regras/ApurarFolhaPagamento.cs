using FolhaPagamentoSimples.Business.Base;
using FolhaPagamentoSimples.Business.Entidades;

namespace FolhaPagamentoSimples.Business.Regras
{
    public class ApurarFolhaPagamento
    {
        private readonly Comuns comunsParametros;

        public ApurarFolhaPagamento(Comuns parametros)
        {
            comunsParametros = parametros;
        }

        public FolhaPagamento CalcularRubricas(List<Rubrica> rubricas)
        {
            if (rubricas == null || !rubricas.Any())
                throw new ArgumentException("Não foi encontrada nenhuma rubrica");

            var empregado = rubricas.First().Empregado; //Pega o primeiro empregado da rubrica para evitar retrabalho

            //Soma tanto os proventos quanto os descontos
            decimal totalProventos = rubricas
                .Where(r => r.Tipo.Equals("P", StringComparison.OrdinalIgnoreCase))
                .Sum(r => r.Valor);

            decimal totalDescontos = rubricas
                .Where(r => r.Tipo.Equals("D", StringComparison.OrdinalIgnoreCase))
                .Sum(r => r.Valor);

            var folha = new FolhaPagamento
            {
                Empregado = empregado,
                BaseCalculoINSS = totalProventos - totalDescontos
            };

            // Cálculo de INSS
            folha.DescontoINSS = CalcularINSS(folha.BaseCalculoINSS);

            // Cálculo de desconto de dependentes
            folha.DescontoDependentes = empregado.QtdDependentes * (comunsParametros.SalarioMinimo * 0.05m);

            // Base de cálculo do IR (aqui eu já vou tirar os descontos de dependentes e INSS).
            folha.BaseCalculoIR = folha.BaseCalculoINSS - folha.DescontoDependentes - folha.DescontoINSS;

            // Cálculo do IR
            folha.DescontoIR = CalcularIR(folha.BaseCalculoIR);

            // Valor líquido
            folha.ValorLiquido = totalProventos - totalDescontos - folha.DescontoINSS - folha.DescontoIR - folha.DescontoDependentes;

            return folha;
        }

        private decimal CalcularINSS(decimal baseINSS)
        {
            if (baseINSS <= 0)
                return 0;

            decimal total = 0;
            decimal valorRestante = baseINSS;
            decimal limiteInferior = 0;

            var faixasOrdenadas = comunsParametros.FaixasINSS.OrderBy(f => f.Faixa).ToList();

            foreach (var faixa in faixasOrdenadas)
            {
                if (valorRestante <= 0)
                    break;

                decimal valorTributavel = Math.Min(valorRestante, faixa.Faixa - limiteInferior);
                total += valorTributavel * faixa.Aliquota;
                valorRestante -= valorTributavel;
                limiteInferior = faixa.Faixa;
            }

            // Se ainda tem valor acima da última faixa ele vai aplicar a última alíquota
            if (valorRestante > 0)
            {
                total += valorRestante * faixasOrdenadas.Last().Aliquota;
            }

            return total;
        }

        private decimal CalcularIR(decimal baseIR)
        {
            if (baseIR <= 0)
                return 0;

            decimal baseEmSalarios = baseIR / comunsParametros.SalarioMinimo;
            var faixasOrdenadas = comunsParametros.FaixasIR.OrderBy(f => f.Faixa).ToList();

            // Encontra a faixa certa de acordo com a soma dos proventos
            foreach (var faixa in faixasOrdenadas)
            {
                if (baseEmSalarios <= faixa.Faixa)
                {
                    return baseIR * faixa.Aliquota;
                }
            }

            // Se ultrapassou todas as faixas vai usar a última alíquota
            return baseIR * faixasOrdenadas.Last().Aliquota;
        }
    }
}