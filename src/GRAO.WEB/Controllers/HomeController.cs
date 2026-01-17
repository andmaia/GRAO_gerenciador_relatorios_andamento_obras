using GRAO.Core;
using GRAO.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace GRAO.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(PdfRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    ));
            }

            if (model.Images == null || model.Images.Count == 0)
            {
                return BadRequest(new
                {
                    Images = new[] { "Nenhuma imagem foi enviada." }
                });
            }

            try
            {
                var imagesBytes = new List<byte[]>(model.Images.Count);

                foreach (var file in model.Images)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    imagesBytes.Add(ms.ToArray());
                }

                var pdfBytes = PdfGenerator.CreatePdf(
                    model.Description,
                    model.CountFirst,
                    imagesBytes);

                return File(pdfBytes, "application/pdf", "resultado.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao gerar o PDF");

                return StatusCode(500, new
                {
                    error = "Erro interno ao gerar o PDF."
                });
            }
        }


    }
}
