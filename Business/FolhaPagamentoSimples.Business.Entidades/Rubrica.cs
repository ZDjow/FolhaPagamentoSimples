namespace FolhaPagamentoSimples.Business.Entidades
{
    public class Rubrica
    {
        public Empregado Empregado { get; set; }
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
    }
}
