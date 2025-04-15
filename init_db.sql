-- Create Users Table
CREATE TABLE Users (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Email VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    IsActive BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create Roles Table
CREATE TABLE Roles (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create Permissions Table
CREATE TABLE Permissions (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE,
    Description VARCHAR(255),
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW()
);

-- Create UserRoles Table (Many-to-Many between Users and Roles)
CREATE TABLE UserRoles (
    UserId INT NOT NULL,
    RoleId INT NOT NULL,
    AssignedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE
);

-- Create RolePermissions Table (Many-to-Many between Roles and Permissions)
CREATE TABLE RolePermissions (
    RoleId INT NOT NULL,
    PermissionId INT NOT NULL,
    AssignedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    PRIMARY KEY (RoleId, PermissionId),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE,
    FOREIGN KEY (PermissionId) REFERENCES Permissions(Id) ON DELETE CASCADE
);

-- Create Projects Table
CREATE TABLE Projects (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description VARCHAR(1024),
    StartDate DATE NOT NULL,
    EndDate DATE,
    CreatedBy INT NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id) ON DELETE SET NULL
);

-- Create ProjectUsers Table (Many-to-Many between Projects and Users)
CREATE TABLE ProjectUsers (
    ProjectId INT NOT NULL,
    UserId INT NOT NULL,
    RoleId INT NOT NULL, -- Role within the project
    AssignedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    PRIMARY KEY (ProjectId, UserId),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE SET NULL
);

-- Create Tasks Table
CREATE TABLE Tasks (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    ProjectId INT NOT NULL,
    Title VARCHAR(100) NOT NULL,
    Description VARCHAR(1024),
    DueDate DATE,
    Priority VARCHAR(20) CHECK (Priority IN ('Low', 'Medium', 'High')),
    Status VARCHAR(20) CHECK (Status IN ('To Do', 'In Progress', 'Completed')) DEFAULT 'To Do',
    AssigneeId INT,
    CreatedBy INT NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (AssigneeId) REFERENCES Users(Id) ON DELETE SET NULL,
    FOREIGN KEY (CreatedBy) REFERENCES Users(Id) ON DELETE SET NULL
);

-- Create TaskComments Table
CREATE TABLE TaskComments (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    TaskId INT NOT NULL,
    UserId INT NOT NULL,
    Comment VARCHAR(1024) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE SET NULL
);

-- Create Files Table
CREATE TABLE Files (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    FileName VARCHAR(255) NOT NULL,
    FilePath VARCHAR(500) NOT NULL,
    UploadedBy INT NOT NULL,
    UploadedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    ProjectId INT NULL,
    TaskId INT NULL,
    FOREIGN KEY (UploadedBy) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE,
    CHECK (
        (ProjectId IS NOT NULL AND TaskId IS NULL) OR
        (ProjectId IS NULL AND TaskId IS NOT NULL)
    )
);

-- Create Notifications Table
CREATE TABLE Notifications (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    Content VARCHAR(1024) NOT NULL,
    IsRead BOOLEAN NOT NULL DEFAULT FALSE,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create RefreshTokens Table
CREATE TABLE RefreshTokens (
    Id INT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    UserId INT NOT NULL,
    Token VARCHAR(500) NOT NULL UNIQUE,
    ExpiresAt TIMESTAMP NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create Indexes for Performance Optimization

-- Index on Users.Email for quick lookup
CREATE INDEX IDX_Users_Email ON Users(Email);

-- Index on Users.Username for quick lookup
CREATE INDEX IDX_Users_Username ON Users(Username);

-- Index on Projects.Name for searching
CREATE INDEX IDX_Projects_Name ON Projects(Name);

-- Index on Tasks.Status for filtering
CREATE INDEX IDX_Tasks_Status ON Tasks(Status);

-- Index on Tasks.Priority for filtering
CREATE INDEX IDX_Tasks_Priority ON Tasks(Priority);

-- Index on TaskComments.TaskId for retrieving comments by task
CREATE INDEX IDX_TaskComments_TaskId ON TaskComments(TaskId);

-- Index on Files.ProjectId for retrieving files by project
CREATE INDEX IDX_Files_ProjectId ON Files(ProjectId);

-- Index on Files.TaskId for retrieving files by task
CREATE INDEX IDX_Files_TaskId ON Files(TaskId);

-- Index on Notifications.UserId for retrieving user-specific notifications
CREATE INDEX IDX_Notifications_UserId ON Notifications(UserId);

-- Index on RefreshTokens.UserId for managing user tokens
CREATE INDEX IDX_RefreshTokens_UserId ON RefreshTokens(UserId);
