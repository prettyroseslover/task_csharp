using System.Text.Json.Serialization;

namespace TASK.Models;

public class Value
{
    public int Id { get; set; }
    
    public int ResultId { get; set; }
    
    public DateTime TimeDate { get; set; }
    public int Time { get; set; }
    public double Values { get; set; }
    
    [JsonIgnore]
    public Result Result { get; set; }
    
}