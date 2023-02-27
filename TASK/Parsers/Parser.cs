using System.Text;

namespace TASK.Parsers;

public class Parser
{
    private IFormFile _uploadedFile;

    public Parser(IFormFile uploadedFile)
    {
        _uploadedFile = uploadedFile;
    }

    public (bool, string) csvParse(out List<DateTime> date, out List<int> time, out List<double> val)
    {
        date = new List<DateTime>();
        time = new List<int>();
        val = new List<double>();
        // проверяем расширение файла
        string ext = Path.GetExtension(_uploadedFile.FileName);
        if (ext != ".csv")
        {
            return (false, "Не CSV-файл");
        }

        var sb = new StringBuilder();

        using (var reader = new StreamReader(_uploadedFile.OpenReadStream()))
        {
            if (reader.Peek() < 0)
            {
                {
                    return (false, "Пустой файл?");
                }
            }
            while (reader.Peek() >= 0)
            {
                sb.AppendLine(reader.ReadLine());
            }
        }
        
        string[] res = sb.ToString()
            .Replace("\"", "")
            .Replace("\n", ";")
            .TrimEnd(';')
            .Split(";");
        
        for (var i = 0; i < res.Length; i++)
        {
            if (i % 3 == 0)
            {
                // Дата не может быть позже текущей и раньше 01.01.2000
                try
                {
                    DateTime enteredDate = DateTime.ParseExact(res[i], "yyyy-M-d_HH-mm-ss", null);
                    DateTime limit = new DateTime(2000, 1, 1, 0, 0, 0);
                    if (enteredDate > DateTime.Now || enteredDate < limit)
                    {
                        return (false, "Некорректная дата");
                    }
                    else
                    {
                        date.Add(enteredDate);
                    }
                }
                catch (FormatException)
                {
                    return (false, "Некорректный файл");
                }
            }
            else if (i % 3 == 1)
            {
                // Время не может быть меньше 0
                int temp;
                bool success = int.TryParse(res[i], out temp);
                if (success && (temp >= 0))
                {
                    time.Add(temp);
                }
                else
                {
                    return (false, "Некорректное время");  
                }

            }
            else
            {
                double temp;
                bool success = double.TryParse(res[i], out temp);
                if (success && (temp >= 0))
                {
                    val.Add(temp);
                }
                else
                {
                    return (false, "Некорректный показатель");  
                }
            }

        }
        
        // Количество строк не может быть меньше 1 и больше 10 000
        if (date.Count < 1 || date.Count > 10000)
        {
            return (false, "Кол-во строк в файл не удовлетворяет условию");
        }
        
        return (true, "ОК");
    }
}