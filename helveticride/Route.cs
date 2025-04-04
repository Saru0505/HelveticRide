using SQLite;

namespace helveticride
{
  public class Route
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string Waypoints { get; set; }
  }
}
