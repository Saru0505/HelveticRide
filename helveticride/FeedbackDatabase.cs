
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace helveticride
{
  public class FeedbackDatabase
  {
    private readonly string _dbPath = "Routes.db";

    public FeedbackDatabase()
    {
      if (!File.Exists(_dbPath))
      {
        SQLiteConnection.CreateFile(_dbPath);
      }

      CreateFeedbackTable();
    }

    private void CreateFeedbackTable()
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = @"
          CREATE TABLE IF NOT EXISTS Feedback (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL,
            Message TEXT NOT NULL,
            CreatedAt TEXT DEFAULT (datetime('now')),
            FOREIGN KEY(UserId) REFERENCES Users(UserId)
          )";

        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void AddFeedback(int userId, string message)
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "INSERT INTO Feedback (UserId, Message) VALUES (@UserId, @Message)";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@UserId", userId);
          cmd.Parameters.AddWithValue("@Message", message);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public List<Feedback> GetAllFeedback()
    {
      var feedbackList = new List<Feedback>();
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "SELECT * FROM Feedback ORDER BY CreatedAt DESC";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          using (var reader = cmd.ExecuteReader())
          {
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
          }
        }
      }
      return feedbackList;
    }
  }

  public class Feedback
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public string CreatedAt { get; set; }
  }
}
