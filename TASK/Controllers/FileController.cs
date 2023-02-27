using Microsoft.AspNetCore.Mvc;
using TASK.Models;
using TASK.Parsers;
using TASK.Counters;

namespace TASK.Controllers;


[ApiController]
[Route("[controller]")]
public class FileController : Controller
{
    private readonly ApplicationContext _context;
    
    public FileController(ApplicationContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Загрузка необходимого csv-файла
    /// </summary>
    /// <remarks>  csv с каждой строкой в формате: {Дата и время в формате ГГГГ-ММ-ДД_чч-мм-сс};{Целочисленное значение времени в секундах};{Показатель в виде числа с плавающей запятой} </remarks>
    [HttpPost]
    [Route("method1")]
    public IActionResult PostFile(IFormFile uploadedFile)
    {
        List<DateTime> date;
        List<int> time;
        List<double> val;
        
        var newParser = new Parser(uploadedFile);
        (var success, var reason) = newParser.csvParse(out date, out time, out val);
        if (!success)
        {
            return new ObjectResult(reason) {StatusCode = 400};
        }

        var newCounter = new Counter(date, time, val, uploadedFile.FileName, _context);
        newCounter.countInformation();
        newCounter.addInformation();
        
        return Ok("Файл принят!");
    }
    
    /// <summary>
    /// Возвращает значения из таблицы Results по заданным параметрам
    /// </summary>
    /// <remarks> Требования к параметрам: &#13;&#10;
    /// filenameCSV - имя файла + расширение (filename.csv)&#13;&#10;
    /// Время запуска первой операции от timeFrom до timeTo&#13;&#10;
    /// Средний показатель в диапозоне от avgFrom до avgTo&#13;&#10;
    /// Среднее время в диапозоне от avgTimeFrom до avgTimeTo </remarks>
    [HttpGet]
    [Route("method2")]
    public List<Result> GetResult(string? filenameCsv, DateTime? timeFrom, DateTime? timeTo, double? avgFrom, double? avgTo, double? avgTimeFrom, double? avgTimeTo)
    {
        IQueryable<Result> fin = _context.Results;
        if (filenameCsv != null)
        {
            // По имени файла
            fin = fin.Where(r => r.Name == filenameCsv);
        }

        if (timeFrom != null && timeTo != null && timeFrom < timeTo)
        {
            // По времени запуска первой операции (от, до)
            fin = fin.Where(r => r.MinDateTime > timeFrom && r.MinDateTime < timeTo);
        }
        
        if (avgFrom != null && avgTo != null && avgFrom < avgTo)
        {
            // По среднему показателю (в диапазоне)
            fin = fin.Where(r => r.Average > avgFrom && r.Average < avgTo);
        }

        if (avgTimeFrom != null && avgTimeTo != null && avgTimeFrom < avgTimeTo)
        {
            // По среднему времени (в диапазоне)
            fin = fin.Where(r => r.AverageTime > avgTimeFrom && r.AverageTime < avgTimeTo);
        }
        
        return fin.ToList();
    }
    
    /// <summary>
    /// Возвращает значения из таблицы Values по имени файла
    /// </summary>
    /// <remarks>Имя файла вводить в формате filename.csv</remarks>
    [HttpGet]
    [Route("method3/{filenameCsv}")]
    public List<Value> GetValuesByName(string filenameCsv)
    {
        // Ожидаем, что имя введут в формате filename.csv
        var values = _context.Values.Where(v => v.Result.Name == filenameCsv).ToList();
        return values;
    }
    
}
