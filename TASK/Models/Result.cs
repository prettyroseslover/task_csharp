using System.Text.Json.Serialization;

namespace TASK.Models;

public class Result
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int AllTime { get; set; } //  Все время (максимальное значение времени минус минимальное значение времени)
    public DateTime MinDateTime { get; set; } //  Минимальное дата и время, как момент запуска первой операции
    public double AverageTime { get; set; } //	Среднее время выполнения
    public double Average { get; set; } //	Среднее значение по показателям
    public double Median { get; set; } //	Медина по показателям
    public double Max { get; set; } //	Максимальное значение показателя
    public double Min { get; set; } //  Минимальное значение показателя
    public int CountString { get; set; } //  Количество строк
    [JsonIgnore]
    public List<Value> Values { get; set; } = new List<Value>(); // один результат из нескольких строк исходника 

}