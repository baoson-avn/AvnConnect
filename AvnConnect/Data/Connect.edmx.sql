
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/22/2018 09:23:40
-- Generated from EDMX file: D:\Documents\Visual Studio 2015\Projects\AvnConnect\AvnConnect\Data\Connect.edmx
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

IF OBJECT_ID(N'[dbo].[Staffs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Staffs];
GO
IF OBJECT_ID(N'[dbo].[JobTitles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JobTitles];
GO
IF OBJECT_ID(N'[dbo].[Departments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Departments];
GO
IF OBJECT_ID(N'[dbo].[Educations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Educations];
GO
IF OBJECT_ID(N'[dbo].[ForeignLanguages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ForeignLanguages];
GO
IF OBJECT_ID(N'[dbo].[PracticingLicenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PracticingLicenses];
GO
IF OBJECT_ID(N'[dbo].[WorkingExperiences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkingExperiences];
GO
IF OBJECT_ID(N'[dbo].[ProfesstionalAreas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfesstionalAreas];
GO
IF OBJECT_ID(N'[dbo].[Permissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Permissions];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[ProjectStaffs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectStaffs];
GO
IF OBJECT_ID(N'[dbo].[UserProjectPermissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserProjectPermissions];
GO
IF OBJECT_ID(N'[dbo].[ProjectActivities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectActivities];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[ProjectTaskLists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectTaskLists];
GO

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
    [Email2] nvarchar(max)  NULL,
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
    [ModifiedOn] datetime  NOT NULL,
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
    [FromYear] datetime  NULL,
    [ToYear] datetime  NULL,
    [IsLearning] bit  NULL,
    [EducationKey] nvarchar(max)  NOT NULL
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
    [WritingLevel] tinyint  NOT NULL,
    [Key] nvarchar(max)  NOT NULL
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
    [ProfessionalArea] nvarchar(max)  NULL,
    [Key] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WorkingExperiences'
CREATE TABLE [dbo].[WorkingExperiences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StaffKey] nvarchar(max)  NOT NULL,
    [NameOfCompany] nvarchar(max)  NOT NULL,
    [FromTime] datetime  NULL,
    [ToTime] datetime  NULL,
    [Jobtitle] nvarchar(max)  NOT NULL,
    [ResignationReason] nvarchar(max)  NULL,
    [Key] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProfesstionalAreas'
CREATE TABLE [dbo].[ProfesstionalAreas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Key] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Permissions'
CREATE TABLE [dbo].[Permissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [StaffKey] nvarchar(max)  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [CanAddProject] bit  NOT NULL,
    [CanManageStaff] bit  NOT NULL,
    [GiveFutureAccess] bit  NOT NULL,
    [ManageDepartment] bit  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Color] nvarchar(max)  NOT NULL,
    [ParentKey] nvarchar(max)  NOT NULL,
    [AddedBy] nvarchar(max)  NOT NULL,
    [AddedOn] datetime  NOT NULL,
    [ModifiedBy] nvarchar(max)  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [Level] int  NOT NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [BindToCustomer] bit  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [AddedBy] nvarchar(max)  NOT NULL,
    [AddedOn] datetime  NOT NULL,
    [ModifiedBy] nvarchar(max)  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [Category] nvarchar(max)  NULL,
    [Tag] nvarchar(max)  NULL,
    [ProjectOwner] nvarchar(max)  NOT NULL,
    [IsStared] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [IsArchived] bit  NOT NULL,
    [ArchivedBy] nvarchar(max)  NULL,
    [ArchivedOn] datetime  NULL,
    [IsCompleted] bit  NOT NULL,
    [CompletedOn] datetime  NULL,
    [CompletedBy] nvarchar(max)  NULL
);
GO

-- Creating table 'ProjectStaffs'
CREATE TABLE [dbo].[ProjectStaffs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Key] nvarchar(max)  NOT NULL,
    [ProjectKey] nvarchar(max)  NOT NULL,
    [StaffKey] nvarchar(max)  NOT NULL,
    [AddedBy] nvarchar(max)  NOT NULL,
    [AddedOn] nvarchar(max)  NOT NULL,
    [PermissionKey] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserProjectPermissions'
CREATE TABLE [dbo].[UserProjectPermissions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectStaffKey] nvarchar(max)  NOT NULL,
    [IsAdmin] bit  NOT NULL,
    [CanViewUpdate] bit  NOT NULL,
    [CanAddUpdate] bit  NOT NULL,
    [CanViewTask] bit  NOT NULL,
    [CanViewEstimatedTime] bit  NOT NULL,
    [CanCreateTask] bit  NOT NULL,
    [CanUpdateAllTask] bit  NOT NULL,
    [CanViewMessageAndFile] bit  NOT NULL,
    [CanUpdateMessageAndFile] bit  NOT NULL,
    [CanViewNoteBook] bit  NOT NULL,
    [CanUpdateNoteBook] bit  NOT NULL,
    [CanViewLinks] bit  NOT NULL,
    [CanAddLinks] bit  NOT NULL,
    [CanViewRisk] bit  NOT NULL,
    [CanUpdateRisk] bit  NOT NULL
);
GO

-- Creating table 'ProjectActivities'
CREATE TABLE [dbo].[ProjectActivities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectKey] nvarchar(max)  NOT NULL,
    [ByStaff] nvarchar(max)  NOT NULL,
    [HappenedOn] datetime  NOT NULL,
    [Message] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TagKey] nvarchar(max)  NOT NULL,
    [TagName] nvarchar(max)  NOT NULL,
    [Color] nvarchar(max)  NOT NULL,
    [AddedBy] nvarchar(max)  NOT NULL,
    [AddOn] datetime  NOT NULL
);
GO

-- Creating table 'ProjectTaskLists'
CREATE TABLE [dbo].[ProjectTaskLists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ListName] nvarchar(max)  NOT NULL,
    [CreatedBy] nvarchar(max)  NOT NULL,
    [CreatedOn] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [ModifiedBy] nvarchar(max)  NULL,
    [ModifiedOn] datetime  NULL,
    [Key] nvarchar(max)  NOT NULL,
    [ProjectKey] nvarchar(max)  NOT NULL
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

-- Creating primary key on [Id] in table 'ProfesstionalAreas'
ALTER TABLE [dbo].[ProfesstionalAreas]
ADD CONSTRAINT [PK_ProfesstionalAreas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Permissions'
ALTER TABLE [dbo].[Permissions]
ADD CONSTRAINT [PK_Permissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectStaffs'
ALTER TABLE [dbo].[ProjectStaffs]
ADD CONSTRAINT [PK_ProjectStaffs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserProjectPermissions'
ALTER TABLE [dbo].[UserProjectPermissions]
ADD CONSTRAINT [PK_UserProjectPermissions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectActivities'
ALTER TABLE [dbo].[ProjectActivities]
ADD CONSTRAINT [PK_ProjectActivities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectTaskLists'
ALTER TABLE [dbo].[ProjectTaskLists]
ADD CONSTRAINT [PK_ProjectTaskLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------