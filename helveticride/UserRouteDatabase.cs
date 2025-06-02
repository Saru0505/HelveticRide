
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace helveticride
{
  public class UserRouteDatabase
  {
    private readonly string _dbPath = "Routes.db";

    public UserRouteDatabase()
    {
      if (!File.Exists(_dbPath))
      {
        SQLiteConnection.CreateFile(_dbPath);
      }

      CreateUserRouteTable();
    }

    private void CreateUserRouteTable()
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = @"
          CREATE TABLE IF NOT EXISTS UserRoutes (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            RouteId INTEGER NOT NULL,
            IsFavorite INTEGER DEFAULT 0,
            SavedAt TEXT DEFAULT (datetime('now')),
            FOREIGN KEY(UserId) REFERENCES Users(UserId),
            FOREIGN KEY(RouteId) REFERENCES Routes(Id)
          )";

        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void SaveUserRoute(int userId, int routeId, bool isFavorite = false)
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = @"
          INSERT INTO UserRoutes (UserId, RouteId, IsFavorite)
          VALUES (@UserId, @RouteId, @IsFavorite)";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@UserId", userId);
          cmd.Parameters.AddWithValue("@RouteId", routeId);
          cmd.Parameters.AddWithValue("@IsFavorite", isFavorite ? 1 : 0);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public List<UserRoute> GetRoutesForUser(int userId)
    {
      var userRoutes = new List<UserRoute>();
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "SELECT * FROM UserRoutes WHERE UserId = @UserId";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@UserId", userId);
          using (var reader = cmd.ExecuteReader())
          {
            while (reader.Read())
            {
              userRoutes.Add(new UserRoute
              {
                Id = Convert.ToInt32(reader["Id"]),
                UserId = Convert.ToInt32(reader["UserId"]),
                RouteId = Convert.ToInt32(reader["RouteId"]),
                IsFavorite = Convert.ToInt32(reader["IsFavorite"]) == 1,
                SavedAt = reader["SavedAt"].ToString()
              });
            }
          }
        }
      }
      return userRoutes;
    }
  }

  public class UserRoute
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RouteId { get; set; }
    public bool IsFavorite { get; set; }
    public string SavedAt { get; set; }
  }
}
