using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GRAO.Core.Extensions
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(params string[] extensions)
        {
            _extensions = extensions.Select(e => e.ToLower()).ToArray();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not List<IFormFile> files || files.Count == 0)
                return ValidationResult.Success;

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(
                        $"Arquivo inválido. Permitidos: {string.Join(", ", _extensions)}");
                }
            }

            return ValidationResult.Success;
        }
    }
}
