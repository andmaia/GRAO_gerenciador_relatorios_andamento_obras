using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace GRAO.Core
{
    public static class PdfGenerator
    {
        public static byte[] CreatePdf(
            string description,
            int countFirst,
            IEnumerable<byte[]> imageBytes)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            using var ms = new MemoryStream();

            Document.Create(container =>
            {
                int currentIndex = countFirst;
                var buffer = new List<(byte[] Image, int Index)>(6);

                foreach (var img in imageBytes)
                {
                    buffer.Add((img, currentIndex++));

                    if (buffer.Count == 6)
                    {
                        AddPage(container, buffer, description);
                        buffer.Clear();
                    }
                }

                if (buffer.Count > 0)
                    AddPage(container, buffer, description);

            }).GeneratePdf(ms);

            return ms.ToArray();
        }

        private static void AddPage(
            IDocumentContainer container,
            List<(byte[] Image, int Index)> images,
            string description)
        {
            container.Page(page =>
            {
                page.Margin(20);

                page.Content().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Spacing(10);

                    foreach (var item in images)
                    {
                        grid.Item().Column(col =>
                        {
                            col.Item()
                               .PaddingBottom(4)
                               .Image(item.Image, ImageScaling.FitArea);

                            col.Item()
                               .AlignCenter()
                               .Text($"{description} {item.Index}")
                               .FontSize(10);
                        });
                    }
                });
            });
        }
    }
}
