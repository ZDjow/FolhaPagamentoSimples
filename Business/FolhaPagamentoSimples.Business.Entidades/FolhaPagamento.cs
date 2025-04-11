namespace FolhaPagamentoSimples.Business.Entidades
{
    public class FolhaPagamento
    {
        public Empregado Empregado { get; set; }
        public decimal BaseCalculoINSS { get; set; }
        public decimal BaseCalculoIR { get; set; }
        public decimal DescontoDependentes { get; set; }
        public decimal DescontoINSS { get; set; }
        public decimal DescontoIR { get; set; }
        public decimal ValorLiquido { get; set; }
    }
}
