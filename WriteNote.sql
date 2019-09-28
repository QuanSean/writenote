USE master
DROP DATABASE WriteNote
GO
CREATE DATABASE WriteNote
GO
USE WriteNote

GO
CREATE TABLE tb_Users (
	ID_USER INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	USERNAME VARCHAR(20) NULL,
	PASSWORD VARCHAR(255) NOT NULL,
	EMAIL VARCHAR(255) NOT NULL,
	FULLNAME NVARCHAR(255) NULL,
	DATETIME_CREATE DATETIME DEFAULT GETDATE() NOT NULL,
	FORGOT_PASSWORD VARCHAR(255) NULL,
	ID_SUBSCRIPTION INT DEFAULT 1 NOT NULL,
	ID_ROLE INT DEFAULT 2 NOT NULL
)

CREATE TABLE tb_Notebooks (
	ID_NOTEBOOK INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	ID_USER INT NOT NULL,
	TITLE NVARCHAR(255) NOT NULL,
	DATETIME_CREATE DATETIME DEFAULT GETDATE() NOT NULL
)

CREATE TABLE tb_Notes (
	ID_NOTE INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	ID_USER INT NOT NULL,
	TITLE NVARCHAR(255) NULL,
	BODY TEXT NOT NULL,
	ID_NOTEBOOK INT NOT NULL,
	IS_DELETE INT DEFAULT 0 NOT NULL,
	DATETIME_CREATE DATETIME DEFAULT GETDATE() NOT NULL,
	DATETIME_UPDATE DATETIME DEFAULT GETDATE() NOT NULL
)

CREATE TABLE tb_NoteDeletes (
	ID_NOTEDELETE INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	ID_USER INT NOT NULL,
	ID_NOTE INT NOT NULL,
	DATETIME_DELETE DATETIME DEFAULT GETDATE() NOT NULL
)

CREATE TABLE tb_Roles (
	ID_ROLE INT NOT NULL PRIMARY KEY,
	NAME_ROLE NVARCHAR(255) NOT NULL
)

CREATE TABLE tb_Subscriptions (
	ID_SUBSCRIPTION INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	NAME_SUBSCRIPTION NVARCHAR(255) NOT NULL,
	PRICE INT NOT NULL
)

CREATE TABLE tb_Buys (
	ID_BUY INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	ID_USER INT NOT NULL,
	ID_SUBSCRIPTION INT NOT NULL,
	PRICE INT NOT NULL,
	COUNT_MONTH INT NOT NULL,
	DATETIME_BOUGHT DATETIME DEFAULT GETDATE() NOT NULL
)

CREATE TABLE tb_Logs (
	ID_LOG INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
	ID_USER INT NOT NULL,
	ID_NOTE INT NOT NULL,
	DATETIME_CREATE DATETIME DEFAULT GETDATE() NOT NULL
)

/* ADD FOREIGN KEY TO TABLE tb_Notes */
ALTER TABLE tb_Notes ADD FOREIGN KEY (ID_USER) REFERENCES tb_Users(ID_USER)
ALTER TABLE tb_Notes ADD FOREIGN KEY (ID_NOTEBOOK) REFERENCES tb_Notebooks(ID_NOTEBOOK)

/* ADD FOREIGN KEY TO TABLE tb_Notebooks */
ALTER TABLE tb_Notebooks ADD FOREIGN KEY (ID_USER) REFERENCES tb_Users(ID_USER)

/* ADD FOREIGN KEY TO TABLE tb_NoteDeletes */
ALTER TABLE tb_NoteDeletes ADD FOREIGN KEY (ID_USER) REFERENCES tb_Users(ID_USER)
ALTER TABLE tb_NoteDeletes ADD FOREIGN KEY (ID_NOTE) REFERENCES tb_Notes(ID_NOTE)

/* ADD FOREIGN KEY TO TABLE tb_Roles */
ALTER TABLE tb_Users ADD FOREIGN KEY (ID_ROLE) REFERENCES tb_Roles(ID_ROLE)
ALTER TABLE tb_Users ADD FOREIGN KEY (ID_SUBSCRIPTION) REFERENCES tb_Subscriptions(ID_SUBSCRIPTION)

/* INSERT DATA TO TABLE tb_Roles */
INSERT INTO tb_Roles VALUES (1, N'Admin')
INSERT INTO tb_Roles VALUES (2, N'User')
INSERT INTO tb_Roles VALUES (3, N'Mod')

/* INSERT DATA TO TABLE tb_Subcriptions */
INSERT INTO tb_Subscriptions VALUES (N'GÓI CƠ BẢN', 0)
INSERT INTO tb_Subscriptions VALUES (N'GÓI NÂNG CAO', 50000)
INSERT INTO tb_Subscriptions VALUES (N'GÓI CAO CẤP',100000)

INSERT INTO tb_Users VALUES ('admin', '25f9e794323b453885f5181f1b624d0b', 'phutruongck@gmail.com', N'Trương Phú Trường', '2019/04/02', '', 1, 1)

INSERT INTO tb_Notebooks VALUES (1, N'Không có tiêu đề', GETDATE())

SELECT * FROM tb_Users
SELECT * FROM tb_Notes
SELECT * FROM tb_Notebooks
SELECT * FROM tb_Subscriptions
SELECT * FROM tb_NoteDeletes
SELECT * FROM tb_Buys
SELECT * FROM tb_Logs