using TASK.Models;
using MathNet.Numerics.Statistics;

namespace TASK.Counters;

public class Counter
{
    private List<DateTime> _date;
    private List<int> _time;
    private List<double> _val;

    private string _name;
    
    private readonly ApplicationContext _context;
    
    private int AllTime { get; set; } //  Все время (максимальное значение времени минус минимальное значение времени)
    private DateTime MinDateTime { get; set; } //  Минимальное дата и время, как момент запуска первой операции
    private double AverageTime { get; set; } //	Среднее время выполнения
    private double Average { get; set; } //	Среднее значение по показателям
    private double Median { get; set; } //	Медина по показателям
    private double Max { get; set; } //	Максимальное значение показателя
    private double Min { get; set; } //  Минимальное значение показателя
    private int CountString { get; set; } //  Количество строк

    public Counter(List<DateTime> date, List<int> time, List<double> val, string fileName, ApplicationContext context)
    {
        _date = date;
        _time = time;
        _val = val;
        _name = fileName;
        _context = context;
    }

    public void countInformation()
    {
        // Все время (максимальное значение времени минус минимальное значение времени)
        AllTime = _time.Max() - _time.Min();
        // Минимальное дата и время, как момент запуска первой операции
        MinDateTime = _date.Min();
        // Среднее время выполнения
        AverageTime = _time.Average();
        // Среднее значение по показателям
        Average = _val.Average();
        // Медина по показателям
        Median = _val.Median();
        // Максимальное значение показателя
        Max = _val.Max();
        // Минимальное значение показателя
        Min = _val.Min();
        // Количество строк
        CountString = _date.Count;
    }
    
    public void addInformation()
    {
        var r1 = _context.Results.FirstOrDefault(r => r.Name == _name);

        if (r1 != null)
        {
            // Если файл существует, то апдейтим значения в таблицах
            var r1Id = r1.Id;
            
            r1.AllTime = AllTime;
            r1.MinDateTime = MinDateTime;
            r1.AverageTime = AverageTime;
            r1.Average = Average;
            r1.Median = Median;
            r1.Max = Max;
            r1.Min = Min;
            r1.CountString = CountString;

            var v1 = _context.Values.Where(r => r.ResultId == r1Id).ToList();
            _context.Values.RemoveRange(v1);
            
            for (int i = 0; i < CountString; i++)
            {
                var value = new Value
                {
                    TimeDate = _date[i],
                    Time = _time[i],
                    Values = _val[i],
                    Result = r1
                };
                _context.Values.Add(value);
            }
            _context.SaveChanges();
        }
        else
        {
            var result = new Result
            {
                Name = _name,
                AllTime = AllTime,
                MinDateTime = MinDateTime,
                AverageTime = AverageTime,
                Average = Average,
                Median = Median,
                Max = Max,
                Min = Min,
                CountString = CountString
            };
            
            _context.Results.Add(result);
            
            // Добавить таблицу Values
            for (int i = 0; i < CountString; i++)
            {
                var value = new Value
                {
                    TimeDate = _date[i],
                    Time = _time[i],
                    Values = _val[i],
                    Result = result
                };
                _context.Values.Add(value);
            }
            _context.SaveChanges();
        }
    }
}