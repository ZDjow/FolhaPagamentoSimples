using FolhaPagamentoSimples.Business.Entidades;

namespace FolhaPagamentoSimples.Business.Regras
{
    public class CSVLayout
    {
        public List<Rubrica> ProcessarArquivo(string arquivo)
        {
            var rubricas = new List<Rubrica>();
            Empregado empregadoAtual = null;

            foreach (var registro in File.ReadAllLines(arquivo))
            {
                if (string.IsNullOrWhiteSpace(registro))
                    continue;

                var coluna = registro.Split(';');
                if (coluna.Length == 0)
                    continue;

                var tipoRegistro = coluna[0].Trim().ToLower(); // Caixa baixa no indicador para comparação.

                if (tipoRegistro == "emp") // Usa o indicador para descobrir registro de empregado.
                {
                    if (coluna.Length < 4)
                        throw new Exception("Registro de empregado inválido.");

                    var cpf = coluna[1].Trim();
                    var nome = coluna[2].Trim();
                    var qtdDependentes = int.Parse(coluna[3]);

                    empregadoAtual = new Empregado
                    {
                        CPF = cpf,
                        Nome = nome,
                        QtdDependentes = qtdDependentes
                    };
                }
                else if (tipoRegistro == "rub") // Usa o indicador para descobrir registro de rubrica.
                {
                    if (coluna.Length < 5)
                        throw new Exception("Registro de rubrica inválido.");

                    if (empregadoAtual == null)
                        throw new Exception("Rubrica encontrada antes de qualquer registro de empregado.");

                    var codigo = int.Parse(coluna[1].Trim());
                    var descricao = coluna[2].Trim();
                    var tipo = coluna[3].Trim();
                    var valor = decimal.Parse(coluna[4].Trim());

                    var rubrica = new Rubrica
                    {
                        Empregado = empregadoAtual,
                        Codigo = codigo,
                        Descricao = descricao,
                        Tipo = tipo,
                        Valor = valor
                    };

                    rubricas.Add(rubrica);
                }
            }

            return rubricas;
        }
    }
}
