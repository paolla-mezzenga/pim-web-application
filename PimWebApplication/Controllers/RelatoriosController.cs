using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using PimWebApplication.Data;
using PimWebApplication.Models;
using System.IO;
using System.Linq;

namespace PimWebApplication.Controllers
{
    public class RelatoriosController : Controller
    {
        private readonly AppDbContext _context;

        public RelatoriosController(AppDbContext context)
        {
            _context = context;
        }

        // Action para exibir a página de filtro
        [HttpGet]
        public IActionResult FiltroRelatorio()
        {
            var tiposRelatorio = new[]
            {
                new { Id = 1, Nome = "Vendas" },
                new { Id = 2, Nome = "Produção" }
            };

            ViewBag.TiposRelatorio = tiposRelatorio;

            return View();
        }

        // Action para gerar o relatório (PDF)
        [HttpPost]
        public IActionResult GerarRelatorioPDF(DateTime dataInicio, DateTime dataFim, int tipoRelatorio)
        {
            MemoryStream workStream = new MemoryStream();
            var document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;

            document.Open();

            switch (tipoRelatorio)
            {
                case 1: // Relatório de Vendas
                    GerarRelatorioVendas(document, dataInicio, dataFim);
                    break;
                case 2: // Relatório de Produção
                    GerarRelatorioProducao(document, dataInicio, dataFim);
                    break;
                default:
                    document.Add(new Paragraph("Tipo de relatório inválido."));
                    break;
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Position = 0;

            return File(workStream, "application/pdf", "Relatorio.pdf");
        }

        // Função para gerar o relatório de Vendas
        private void GerarRelatorioVendas(Document document, DateTime dataInicio, DateTime dataFim)
        {
            var vendas = _context.Vendas.Where(v => v.DataVenda >= dataInicio && v.DataVenda <= dataFim).ToList();

            document.Add(new Paragraph("Relatório de Vendas", FontFactory.GetFont("Arial", 18, Font.BOLD)));
            document.Add(new Paragraph($"Período: {dataInicio.ToShortDateString()} até {dataFim.ToShortDateString()}", FontFactory.GetFont("Arial", 12)));
            document.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(5); // 5 colunas: NomeProduto, DataVenda, Quantidade, Preço Unitário, Preço Total
            table.AddCell("Nome Produto");
            table.AddCell("Data Venda");
            table.AddCell("Qtd Vendida");
            table.AddCell("Preço Unitário");
            table.AddCell("Preço Total");

            foreach (var venda in vendas)
            {
                table.AddCell(venda.NomeProduto);
                table.AddCell(venda.DataVenda.ToShortDateString());
                table.AddCell(venda.QtdVendida.ToString());
                table.AddCell(venda.PrecoUnitario.ToString("C"));
                table.AddCell(venda.PrecoTotal.ToString("C"));
            }

            document.Add(table);
        }

        // Função para gerar o relatório de Produção
        private void GerarRelatorioProducao(Document document, DateTime dataInicio, DateTime dataFim)
        {
            var producao = _context.Producao.Where(p => p.DataCultivo >= dataInicio && p.DataCultivo <= dataFim).ToList();

            document.Add(new Paragraph("Relatório de Produção", FontFactory.GetFont("Arial", 18, Font.BOLD)));
            document.Add(new Paragraph($"Período: {dataInicio.ToShortDateString()} até {dataFim.ToShortDateString()}", FontFactory.GetFont("Arial", 12)));
            document.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(4); // 4 colunas: Nome, Data Cultivo, Data Colheita, Observações
            table.AddCell("Nome");
            table.AddCell("Data Cultivo");
            table.AddCell("Data Colheita");
            table.AddCell("Observações");

            foreach (var producaoItem in producao)
            {
                table.AddCell(producaoItem.Nome);
                table.AddCell(producaoItem.DataCultivo.ToShortDateString());
                table.AddCell(producaoItem.DataColheita.ToShortDateString());
                table.AddCell(producaoItem.Observacoes);
            }

            document.Add(table);
        }
    }
}
