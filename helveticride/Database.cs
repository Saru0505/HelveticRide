
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
      }

      string sqlFilePath = "create_tables.sql";
      if (!File.Exists(sqlFilePath))
        throw new FileNotFoundException("SQL-Datei zur Tabellenerstellung nicht gefunden.");

      string sqlScript = File.ReadAllText(sqlFilePath);
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      using var cmd = new SQLiteCommand(sqlScript, conn);
      cmd.ExecuteNonQuery();
    }

    // USER ----------------------------------

    public void AddUser(string username, string passwordHash)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@Username", username);
      cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
      cmd.ExecuteNonQuery();
    }

    public bool ValidateUser(string username, string passwordHash)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@Username", username);
      cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
      return (long)cmd.ExecuteScalar() > 0;
    }

    public int GetUserId(string username)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "SELECT UserId FROM Users WHERE Username = @Username";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@Username", username);
      var result = cmd.ExecuteScalar();
      return result != null ? Convert.ToInt32(result) : -1;
    }

    public List<User> GetAllUsers()
    {
      var users = new List<User>();
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "SELECT * FROM Users";
      using var cmd = new SQLiteCommand(sql, conn);
      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        users.Add(new User
        {
          UserId = Convert.ToInt32(reader["UserId"]),
          Username = reader["Username"].ToString(),
          PasswordHash = reader["PasswordHash"].ToString(),
          CreatedAt = reader["CreatedAt"].ToString()
        });
      }
      return users;
    }

    // ROUTES -------------------------------

    public void SaveRoute(string start, string end, string waypoints, string distance, string duration)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "INSERT INTO Routes (Start, End, Waypoints, IsFavorite, CreatedAt, Distance, Duration) VALUES (@Start, @End, @Waypoints, 0, @CreatedAt, @Distance, @Duration)";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@Start", start);
      cmd.Parameters.AddWithValue("@End", end);
      cmd.Parameters.AddWithValue("@Waypoints", waypoints);
      cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
      cmd.Parameters.AddWithValue("@Distance", distance);
      cmd.Parameters.AddWithValue("@Duration", duration);
      cmd.ExecuteNonQuery();
    }

    public List<Route> GetRouteList()
    {
      var routes = new List<Route>();
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "SELECT * FROM Routes";
      using var cmd = new SQLiteCommand(sql, conn);
      using var reader = cmd.ExecuteReader();
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
      return routes;
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

    // FEEDBACK -----------------------------

    public void AddFeedback(int userId, string message)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "INSERT INTO Feedback (UserId, Message) VALUES (@UserId, @Message)";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@UserId", userId);
      cmd.Parameters.AddWithValue("@Message", message);
      cmd.ExecuteNonQuery();
    }

    public List<Feedback> GetAllFeedback()
    {
      var feedbackList = new List<Feedback>();
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "SELECT * FROM Feedback ORDER BY CreatedAt DESC";
      using var cmd = new SQLiteCommand(sql, conn);
      using var reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        feedbackList.Add(new Feedback
        {
          Id = Convert.ToInt32(reader["Id"]),
          UserId = Convert.ToInt32(reader["UserId"]),
          Message = reader["Message"].ToString(),
          CreatedAt = reader["CreatedAt"].ToString()
        });
      }
      return feedbackList;
    }

    // USERROUTES ---------------------------

    public void SaveUserRoute(int userId, int routeId, bool isFavorite = false)
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "INSERT INTO UserRoutes (UserId, RouteId, IsFavorite) VALUES (@UserId, @RouteId, @IsFavorite)";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@UserId", userId);
      cmd.Parameters.AddWithValue("@RouteId", routeId);
      cmd.Parameters.AddWithValue("@IsFavorite", isFavorite ? 1 : 0);
      cmd.ExecuteNonQuery();
    }

    public int GetLastInsertedRouteId()
    {
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      string sql = "SELECT MAX(Id) FROM Routes";
      using var cmd = new SQLiteCommand(sql, conn);
      object result = cmd.ExecuteScalar();
      return result != null && result != DBNull.Value ? Convert.ToInt32(result) : -1;
    }

    public List<UserRoute> GetRoutesForUser(int userId)
    {
      var userRoutes = new List<UserRoute>();
      using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
      conn.Open();
      var sql = "SELECT * FROM UserRoutes WHERE UserId = @UserId";
      using var cmd = new SQLiteCommand(sql, conn);
      cmd.Parameters.AddWithValue("@UserId", userId);
      using var reader = cmd.ExecuteReader();
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
      return userRoutes;
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
  }

  public class User
  {
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string CreatedAt { get; set; }
  }

  public class Feedback
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public string CreatedAt { get; set; }
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
