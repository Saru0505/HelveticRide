using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace helveticride
{
  public class Database
  {
    private readonly string _dbPath = "Routes.db";

    public Database()
    {
      if (!File.Exists(_dbPath))
      {
        SQLiteConnection.CreateFile(_dbPath);
        CreateRoutesTable();
      }
    }

    private void CreateRoutesTable()
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = @"
            CREATE TABLE IF NOT EXISTS Routes (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Start TEXT NOT NULL,
                End TEXT NOT NULL,
                Waypoints TEXT
            )";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void SaveRoute(string start, string end, string waypoints)
    {
      try
      {
        using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
        {
          conn.Open();
          string sql = "INSERT INTO Routes (Start, End, Waypoints) VALUES (@Start, @End, @Waypoints)";
          using (var cmd = new SQLiteCommand(sql, conn))
          {
            cmd.Parameters.AddWithValue("@Start", start);
            cmd.Parameters.AddWithValue("@End", end);
            cmd.Parameters.AddWithValue("@Waypoints", waypoints);
            cmd.ExecuteNonQuery();
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Fehler beim Speichern in der Datenbank: " + ex.Message);
      }
    }

    public string GetAllRoutes()
    {
      string result = "";
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "SELECT * FROM Routes";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              result += $"Start: {reader["Start"]}, End: {reader["End"]}, Waypoints: {reader["Waypoints"]}\n";
            }
          }
        }
      }
      return result;
    }

    public List<Route> GetRouteList()
    {
      var routes = new List<Route>();
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "SELECT * FROM Routes";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              routes.Add(new Route
              {
                Id = Convert.ToInt32(reader["Id"]),
                Start = reader["Start"].ToString(),
                End = reader["End"].ToString(),
                Waypoints = reader["Waypoints"].ToString()
              });
            }
          }
        }
      }
      return routes;
    }
  }

  public class Route
  {
    public int Id { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string Waypoints { get; set; }

    public override string ToString()
    {
      return $"Start: {Start}, End: {End}, Waypoints: {Waypoints}";
    }
  }
}
