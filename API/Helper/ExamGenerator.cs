using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace API.Helper
{
    public class ExamGenerator
    {
        public static byte[] GenerateExams(List<string> exams)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            using (var stream = new MemoryStream())
            {
                Document.Create(container =>
                {
                    foreach (string exam in exams) {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(2, Unit.Centimetre);
                            page.Content().Text(exam).FontSize(11);
                        });
                    }
                }).GeneratePdf(stream);
                return stream.ToArray();
            }
        }
    }
}