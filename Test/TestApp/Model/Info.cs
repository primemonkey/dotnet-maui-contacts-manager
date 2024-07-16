using System.Text.Json.Serialization;

namespace TestApp.Model;

public class Info
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Occupation { get; set; }
    public string Image { get; set; }
    public string City { get; set; }
}

[JsonSerializable(typeof(List<Info>))]
internal sealed partial class InfoContext : JsonSerializerContext{

}