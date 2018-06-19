
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/19/2018 09:00:10
-- Generated from EDMX file: C:\Users\avn1\documents\visual studio 2015\Projects\AvnConnect\AvnConnect\Data\Connect.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [AvnConnect];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Staffs'
CREATE TABLE [dbo].[Staffs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Surname] nvarchar(max)  NOT NULL,
    [Firstname] nvarchar(max)  NOT NULL,
    [Birthday] datetime  NULL,
    [PlaceOfBirth] nvarchar(max)  NULL,
    [FirstDayAtWork] datetime  NULL,
    [LastDayAtWork] datetime  NULL,
    [JobTitle] nvarchar(max)  NOT NULL,
    [Department] nvarchar(max)  NOT NULL,
    [Nationality] nvarchar(max)  NULL,
    [Gender] bit  NULL,
    [HomePhone] nvarchar(max)  NULL,
    [MobilePhone] nvarchar(max)  NULL,
    [MaritalStatus] nvarchar(max)  NULL,
    [IDNumber] nvarchar(max)  NULL,
    [DateOf_ID_Issue] datetime  NULL,
    [PlaceOf_ID_Issue] nvarchar(max)  NULL,
    [PassportNumber] nvarchar(max)  NULL,
    [DateOfPassportIssue] datetime  NULL,
    [PlaceOfPassportIssue] nvarchar(max)  NULL,
    [EmailAddress] nvarchar(max)  NOT NULL,
    [PermanentResidence] nvarchar(max)  NULL,
    [CurrentAddress] nvarchar(max)  NULL,
    [SocialInsuranceNumber] nvarchar(max)  NULL,
    [PIT_Code] nvarchar(max)  NULL,
    [PIT_Deduction] nvarchar(max)  NULL,
    [VCBAccount] nvarchar(max)  NULL,
    [OtherBankAccount] nvarchar(max)  NULL,
    [OtherBankAccountSubsidiary] nvarchar(max)  NULL,
    [EmergencyContactName] nvarchar(max)  NULL,
    [EmergencyContactPhone] nvarchar(max)  NULL,
    [EmergencyContactRelationship] nvarchar(max)  NULL,
    [Key] nchar(16)  NOT NULL,
    [Avatar] nvarchar(max)  NULL,
    [AddedOn] datetime  NOT NULL,
    [AddedBy] nvarchar(max)  NOT NULL,
    [ModifiedOn] nvarchar(max)  NOT NULL,
    [ModifiedBy] nvarchar(max)  NOT NULL,
    [Password] nchar(32)  NOT NULL
);
GO

-- Creating table 'JobTitles'
CREATE TABLE [dbo].[JobTitles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Key] nchar(16)  NOT NULL
);
GO

-- Creating table 'Departments'
CREATE TABLE [dbo].[Departments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DepartmentName] nvarchar(max)  NOT NULL,
    [Key] nchar(16)  NOT NULL
);
GO

-- Creating table 'Educations'
CREATE TABLE [dbo].[Educations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StaffKey] nchar(16)  NOT NULL,
    [EducationDegree] nvarchar(max)  NOT NULL,
    [NameOfSchool] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [Speciality] nvarchar(max)  NOT NULL,
    [FromYear] datetime  NOT NULL,
    [ToYear] datetime  NULL,
    [IsLearning] bit  NULL
);
GO

-- Creating table 'ForeignLanguages'
CREATE TABLE [dbo].[ForeignLanguages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StaffKey] nchar(16)  NOT NULL,
    [Language] nvarchar(max)  NOT NULL,
    [SpeakingLevel] tinyint  NOT NULL,
    [ListeningLevel] tinyint  NOT NULL,
    [ReadingLevel] tinyint  NOT NULL,
    [WritingLevel] tinyint  NOT NULL
);
GO

-- Creating table 'PracticingLicenses'
CREATE TABLE [dbo].[PracticingLicenses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StaffKey] nchar(16)  NOT NULL,
    [LicenseNumber] nvarchar(max)  NOT NULL,
    [DateOfIssue] datetime  NULL,
    [Status] nvarchar(max)  NULL,
    [PlaceOfIssue] nvarchar(max)  NULL,
    [ProfessionalArea] nvarchar(max)  NULL
);
GO

-- Creating table 'WorkingExperiences'
CREATE TABLE [dbo].[WorkingExperiences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StaffKey] nvarchar(max)  NOT NULL,
    [NameOfCompany] nvarchar(max)  NOT NULL,
    [FromTime] nvarchar(max)  NOT NULL,
    [ToTime] nvarchar(max)  NOT NULL,
    [CurrentlyWorking] nvarchar(max)  NOT NULL,
    [Jobtitle] nvarchar(max)  NOT NULL,
    [ResignationReason] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Staffs'
ALTER TABLE [dbo].[Staffs]
ADD CONSTRAINT [PK_Staffs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JobTitles'
ALTER TABLE [dbo].[JobTitles]
ADD CONSTRAINT [PK_JobTitles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Departments'
ALTER TABLE [dbo].[Departments]
ADD CONSTRAINT [PK_Departments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Educations'
ALTER TABLE [dbo].[Educations]
ADD CONSTRAINT [PK_Educations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ForeignLanguages'
ALTER TABLE [dbo].[ForeignLanguages]
ADD CONSTRAINT [PK_ForeignLanguages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PracticingLicenses'
ALTER TABLE [dbo].[PracticingLicenses]
ADD CONSTRAINT [PK_PracticingLicenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'WorkingExperiences'
ALTER TABLE [dbo].[WorkingExperiences]
ADD CONSTRAINT [PK_WorkingExperiences]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------