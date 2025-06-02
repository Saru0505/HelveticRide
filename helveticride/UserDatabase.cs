
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace helveticride
{
  public class UserDatabase
  {
    private readonly string _dbPath = "Routes.db";

    public UserDatabase()
    {
      if (!File.Exists(_dbPath))
      {
        SQLiteConnection.CreateFile(_dbPath);
      }

      CreateUserTable();
    }
    public int GetUserId(string username)
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "SELECT UserId FROM Users WHERE Username = @Username";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@Username", username);
          object result = cmd.ExecuteScalar();
          return result != null ? Convert.ToInt32(result) : -1;
        }
      }
    }



    private void CreateUserTable()
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = @"
          CREATE TABLE IF NOT EXISTS Users (
            UserId INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT NOT NULL UNIQUE,
            PasswordHash TEXT NOT NULL,
            CreatedAt TEXT DEFAULT (datetime('now'))
          )";

        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.ExecuteNonQuery();
        }
      }
    }

    public void AddUser(string username, string password)
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash)";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@Username", username);
          cmd.Parameters.AddWithValue("@PasswordHash", password);
          cmd.ExecuteNonQuery();
        }
      }
    }

    public bool ValidateUser(string username, string password)
    {
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          cmd.Parameters.AddWithValue("@Username", username);
          cmd.Parameters.AddWithValue("@PasswordHash", password);
          long count = (long)cmd.ExecuteScalar();
          return count > 0;
        }
      }
    }

    public List<User> GetAllUsers()
    {
      var users = new List<User>();
      using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
      {
        conn.Open();
        string sql = "SELECT * FROM Users";
        using (var cmd = new SQLiteCommand(sql, conn))
        {
          using (var reader = cmd.ExecuteReader())
          {
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
          }
        } 
      }
      return users;
    }
  }

  public class User
  {
    public int UserId { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string CreatedAt { get; set; }
  }
}
