namespace FolhaPagamentoSimples.Business.Base
{
    public class Faixas
    {
        public decimal Faixa { get; set; }
        public decimal Aliquota { get; set; }
    }

    // Classe para manipular os parâmetros do sistema como salário mínimo e faixas do INSS e IR que serão informados pelo arquivo Parametros.json.
    public class Comuns
    {
        public decimal SalarioMinimo { get; set; }
        public List<Faixas> FaixasINSS { get; set; }
        public List<Faixas> FaixasIR { get; set; }
    }
}
