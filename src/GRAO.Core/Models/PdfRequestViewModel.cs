using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GRAO.Core.Extensions;
using Microsoft.AspNetCore.Http;

namespace GRAO.Core.Models
{
    public class PdfRequestViewModel
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [MinLength(1, ErrorMessage = "A descrição não pode ser vazia.")]
        public string Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O número inicial não pode ser negativo.")]
        public int CountFirst { get; set; }

        [Required(ErrorMessage = "É necessário enviar ao menos uma imagem.")]
        [AllowedExtensions(".png", ".jpg", ".jpeg")]
        public List<IFormFile> Images { get; set; }
    }
}
