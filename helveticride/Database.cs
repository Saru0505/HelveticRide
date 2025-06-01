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
      else
      {
        EnsureColumnExists("IsFavorite", "INTEGER DEFAULT 0");
        EnsureColumnExists("CreatedAt", "TEXT");
        EnsureColumnExists("Distance", "TEXT");
        EnsureColumnExists("Duration", "TEXT");

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
            Waypoints TEXT,
            IsFavorite INTEGER DEFAULT 0,
            CreatedAt TEXT
            Distance TEXT,
            Duration TEXT

          )";

        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    private void EnsureColumnExists(string columnName, string columnDefinition)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();

      using var cmd = new SQLiteCommand("PRAGMA table_info(Routes);", conn);
      using var reader = cmd.ExecuteReader();

      bool exists = false;
      while (reader.Read())
      {
        if (reader["name"].ToString().Equals(columnName, StringComparison.OrdinalIgnoreCase))
        {
          exists = true;
          break;
        }
      }

      if (!exists)
      {
        using var alterCmd = new SQLiteCommand($"ALTER TABLE Routes ADD COLUMN {columnName} {columnDefinition};", conn);
        alterCmd.ExecuteNonQuery();
      }
    }

    public void SaveRoute(string start, string end, string waypoints, string distance, string duration)

    {
      try
      {
        using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
        {
          conn.Open();
          string sql = "INSERT INTO Routes (Start, End, Waypoints, IsFavorite, CreatedAt, Distance, Duration) VALUES (@Start, @End, @Waypoints, 0, @CreatedAt, @Distance, @Duration)";
          ;
          using (var cmd = new SQLiteCommand(sql, conn))
          {
            cmd.Parameters.AddWithValue("@Start", start);
            cmd.Parameters.AddWithValue("@End", end);
            cmd.Parameters.AddWithValue("@Waypoints", waypoints);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@Distance", distance);
            cmd.Parameters.AddWithValue("@Duration", duration);

            cmd.ExecuteNonQuery();
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Fehler beim Speichern in der Datenbank: " + ex.Message);
      }
    }

    public void UpdateFavoriteStatus(int routeId, bool isFavorite)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "UPDATE Routes SET IsFavorite = @IsFavorite WHERE Id = @Id";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@IsFavorite", isFavorite ? 1 : 0);
      cmd.Parameters.AddWithValue("@Id", routeId);
      cmd.ExecuteNonQuery();
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
              result += $"Start: {reader["Start"]}, End: {reader["End"]}, Waypoints: {reader["Waypoints"]}, Favorit: {reader["IsFavorite"]}, Erstellt am: {reader["CreatedAt"]}\n";
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
                Waypoints = reader["Waypoints"].ToString(),
                IsFavorite = Convert.ToInt32(reader["IsFavorite"]) == 1,
                CreatedAt = reader["CreatedAt"].ToString(),
                Distance = reader["Distance"].ToString(),
                Duration = reader["Duration"].ToString()

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
    public bool IsFavorite { get; set; }
    public string CreatedAt { get; set; }
    public string Distance { get; set; }
    public string Duration { get; set; }


    public override string ToString()
    {
      return $"Start: {Start}, End: {End}, Waypoints: {Waypoints}, Favorit: {IsFavorite}, Erstellt am: {CreatedAt}";
    }
  }
}
