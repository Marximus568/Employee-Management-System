using Application.DTOs.Employee;
using Application.Interfaces.PDF;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Services.PDF;

public class PdfService : IPdfService
{
    public PdfService()
    {
        // License setup (Community License for QuestPDF)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateEmployeeResume(EmployeeDto employee)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text($"Resume: {employee.FirstName} {employee.LastName}")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(x =>
                    {
                        x.Spacing(20);

                        // Personal Info Section
                        x.Item().Text("Personal Information").Bold().FontSize(14).FontColor(Colors.Grey.Darken2);
                        x.Item().Row(row => 
                        {
                            row.RelativeItem().Column(c => 
                            {
                                c.Item().Text(text => { text.Span("ID: ").Bold(); text.Span(employee.Document); });
                                c.Item().Text(text => { text.Span("Email: ").Bold(); text.Span(employee.Email); });
                                c.Item().Text(text => { text.Span("Phone: ").Bold(); text.Span(employee.Phone); });
                            });
                            row.RelativeItem().Column(c => 
                            {
                                c.Item().Text(text => { text.Span("Address: ").Bold(); text.Span(employee.Address); });
                                c.Item().Text(text => { text.Span("Birth Date: ").Bold(); text.Span(employee.BirthDate.ToString("yyyy-MM-dd")); });
                                c.Item().Text(text => { text.Span("Age: ").Bold(); text.Span((DateTime.Now.Year - employee.BirthDate.Year).ToString()); });
                            });
                        });

                        x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        // Work Info Section
                        x.Item().Text("Employment Details").Bold().FontSize(14).FontColor(Colors.Grey.Darken2);
                        x.Item().Row(row => 
                        {
                            row.RelativeItem().Column(c => 
                            {
                                c.Item().Text(text => { text.Span("Position: ").Bold(); text.Span(employee.Position); });
                                c.Item().Text(text => { text.Span("Department: ").Bold(); text.Span(employee.DepartmentName); });
                            });
                            row.RelativeItem().Column(c => 
                            {
                                c.Item().Text(text => { text.Span("Hire Date: ").Bold(); text.Span(employee.HireDate.ToString("yyyy-MM-dd")); });
                                c.Item().Text(text => { text.Span("Status: ").Bold(); text.Span(employee.Status); });
                                c.Item().Text(text => { text.Span("Salary: ").Bold(); text.Span(employee.Salary.ToString("C")); });
                            });
                        });

                        x.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                        // Professional Profile
                        if (!string.IsNullOrWhiteSpace(employee.ProfessionalProfile))
                        {
                            x.Item().Text("Professional Profile").Bold().FontSize(14).FontColor(Colors.Grey.Darken2);
                            x.Item().Text(employee.ProfessionalProfile).Justify();
                            x.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        }

                        // Education Section
                        if (employee.EducationRecords != null && employee.EducationRecords.Any())
                        {
                            x.Item().Text("Education").Bold().FontSize(14).FontColor(Colors.Grey.Darken2);
                            foreach (var method in employee.EducationRecords)
                            {
                                x.Item().Row(r => 
                                {
                                    r.RelativeItem().Column(c => 
                                    {
                                        c.Item().Text(method.Institution).Bold();
                                        c.Item().Text($"{method.Degree} - {method.City}").FontSize(10);
                                    });
                                    r.AutoItem().Text($"{method.StartDate:yyyy} - {(method.EndDate.HasValue ? method.EndDate.Value.ToString("yyyy") : "Present")}");
                                });
                            }
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Generated by Express Firmeza System - ");
                        x.CurrentPageNumber();
                    });
            });
        });

        return document.GeneratePdf();
    }
}
