namespace PimWebApplication.Models
{
    public class Producao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataCultivo { get; set; }
        public DateTime DataColheita { get; set; }
        public string Observacoes { get; set; }
    }
}
