using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces.Excel;

public interface IExcelService
{
    Task ImportEmployeesAsync(Stream fileStream);
}
