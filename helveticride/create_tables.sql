
-- Tabelle: Users
CREATE TABLE IF NOT EXISTS Users (
    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    CreatedAt TEXT DEFAULT (datetime('now'))
);

-- Tabelle: Routes
CREATE TABLE IF NOT EXISTS Routes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Start TEXT NOT NULL,
    End TEXT NOT NULL,
    Waypoints TEXT,
    IsFavorite INTEGER DEFAULT 0,
    CreatedAt TEXT,
    Distance TEXT,
    Duration TEXT
);

-- Tabelle: Feedback
CREATE TABLE IF NOT EXISTS Feedback (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    Message TEXT NOT NULL,
    CreatedAt TEXT DEFAULT (datetime('now')),
    FOREIGN KEY(UserId) REFERENCES Users(UserId)
);

-- Tabelle: UserRoutes
CREATE TABLE IF NOT EXISTS UserRoutes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    UserId INTEGER NOT NULL,
    RouteId INTEGER NOT NULL,
    IsFavorite INTEGER DEFAULT 0,
    SavedAt TEXT DEFAULT (datetime('now')),
    FOREIGN KEY(UserId) REFERENCES Users(UserId),
    FOREIGN KEY(RouteId) REFERENCES Routes(Id)
);
