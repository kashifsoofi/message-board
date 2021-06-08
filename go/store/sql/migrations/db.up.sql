CREATE TABLE IF NOT EXISTS Users (
    ClusteredId             BIGINT AUTO_INCREMENT PRIMARY KEY,
    Id                      CHAR(36) NOT NULL,
    Email                   NVARCHAR(256) NULL,
    EmailConfirmed          BIT NOT NULL,
    PasswordHash            NVARCHAR(MAX) NULL,
    SecurityStamp           NVARCHAR(MAX) NULL,
    PhoneNumber             NVARCHAR(MAX) NULL,
    PhoneNumberConfirmed    BIT NOT NULL,
    TwoFactorEnabled        BIT NOT NULL,
    LockoutEndDateUtc       DATETIME NULL,
    LockoutEnabled          BIT NOT NULL,
    AccessFailedCount       INT NOT NULL,
    UserName                NVARCHAR(256) NOT NULL,
    INDEX Users_Id_Index(Id),
    INDEX 
);

CREATE TABLE IF NOT EXISTS Roles (
    ClusteredId     BIGINT AUTO_INCREMENT PRIMARY KEY,
    Id              CHAR(36) NOT NULL,
    Name            NVARCHAR(256) NOT NULL,
);

CREATE TABLE IF NOT EXISTS UserRoles (
    UserId  CHAR(36) NOT NULL,
    RoleId  CHAR(36) NOT NULL,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

CREATE TABLE IF NOT EXISTS UserLogins (
    LoginProvider   NVARCHAR(450) NOT NULL,
    ProviderKey     NVARCHAR(450) NOT NULL,
    UserId          CHAR(36) NOT NULL,
    PRIMARY KEY (LoginProvider, ProviderKey, UserId),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE IF NOT EXISTS UserClaims (
    ClusteredId     BIGINT AUTO_INCREMENT PRIMARY KEY,
    UserId          CHAR(36) NOT NULL,
    ClaimType       NVARCHAR(MAX) NOT NULL,
    ClaimValue      NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE IF NOT EXISTS UserTokens (
    UserId          CHAR(36) NOT NULL,
    LoginProvider   NVARCHAR(450) NOT NULL,
    Name            NVARCHAR(450) NOT NULL,
    Value           NVARCHAR(MAX) NOT NULL,
    PRIMARY KEY (UserId, LoginProvider, Name),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

