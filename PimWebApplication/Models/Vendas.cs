namespace PimWebApplication.Models
{
    public class Vendas
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public DateTime DataVenda { get; set; }
        public int QtdVendida { get; set; }
        public double PrecoUnitario { get;set; }
        public string Cliente { get; set; }
        public double PrecoTotal { get; set; }
    }
}
