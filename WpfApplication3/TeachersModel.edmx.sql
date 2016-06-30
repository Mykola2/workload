
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/11/2016 17:25:11
-- Generated from EDMX file: C:\Users\nicko\Documents\Visual Studio 2015\Projects\WpfApplication3\WpfApplication3\TeachersModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FacultyDepartment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentSet] DROP CONSTRAINT [FK_FacultyDepartment];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupSet] DROP CONSTRAINT [FK_DepartmentGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_StudyActionsModuleStudyAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ModuleStudyActionSet] DROP CONSTRAINT [FK_StudyActionsModuleStudyAction];
GO
IF OBJECT_ID(N'[dbo].[FK_ModuleModuleStudyAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ModuleStudyActionSet] DROP CONSTRAINT [FK_ModuleModuleStudyAction];
GO
IF OBJECT_ID(N'[dbo].[FK_TeacherModuleStudyAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ModuleStudyActionSet] DROP CONSTRAINT [FK_TeacherModuleStudyAction];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupModuleStudyAction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ModuleStudyActionSet] DROP CONSTRAINT [FK_GroupModuleStudyAction];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentTeacher]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeacherSet] DROP CONSTRAINT [FK_DepartmentTeacher];
GO
IF OBJECT_ID(N'[dbo].[FK_StudyActionsFormulas]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FormulasSet] DROP CONSTRAINT [FK_StudyActionsFormulas];
GO
IF OBJECT_ID(N'[dbo].[FK_StudyCycleModule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ModuleSet] DROP CONSTRAINT [FK_StudyCycleModule];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[StudyCycleSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudyCycleSet];
GO
IF OBJECT_ID(N'[dbo].[DepartmentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentSet];
GO
IF OBJECT_ID(N'[dbo].[FacultySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FacultySet];
GO
IF OBJECT_ID(N'[dbo].[GroupSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupSet];
GO
IF OBJECT_ID(N'[dbo].[StudyActionsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StudyActionsSet];
GO
IF OBJECT_ID(N'[dbo].[ModuleSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ModuleSet];
GO
IF OBJECT_ID(N'[dbo].[ModuleStudyActionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ModuleStudyActionSet];
GO
IF OBJECT_ID(N'[dbo].[TeacherSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeacherSet];
GO
IF OBJECT_ID(N'[dbo].[FormulasSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FormulasSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'StudyCycleSet'
CREATE TABLE [dbo].[StudyCycleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DepartmentSet'
CREATE TABLE [dbo].[DepartmentSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Faculty_Id] int  NOT NULL
);
GO

-- Creating table 'FacultySet'
CREATE TABLE [dbo].[FacultySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'GroupSet'
CREATE TABLE [dbo].[GroupSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Budget_students] int  NOT NULL,
    [Contract_students] int  NOT NULL,
    [Year] int  NOT NULL,
    [Course] nvarchar(max)  NOT NULL,
    [Department_Id] int  NOT NULL
);
GO

-- Creating table 'StudyActionsSet'
CREATE TABLE [dbo].[StudyActionsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsIndividual] bit  NOT NULL
);
GO

-- Creating table 'ModuleSet'
CREATE TABLE [dbo].[ModuleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Year] int  NOT NULL,
    [Semester] nvarchar(max)  NOT NULL,
    [Credits] nvarchar(max)  NOT NULL,
    [Lections] int  NULL,
    [Practices] nvarchar(max)  NULL,
    [Labs] nvarchar(max)  NULL,
    [Self] nvarchar(max)  NULL,
    [Exam] nvarchar(max)  NULL,
    [ZALIK] nvarchar(max)  NULL,
    [Module_tests] nvarchar(max)  NULL,
    [Course_work] nvarchar(max)  NULL,
    [RGR] nvarchar(max)  NULL,
    [DKR] nvarchar(max)  NULL,
    [Referat] nvarchar(max)  NULL,
    [StudyCycle_Id] int  NOT NULL
);
GO

-- Creating table 'ModuleStudyActionSet'
CREATE TABLE [dbo].[ModuleStudyActionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Hours] int  NOT NULL,
    [Year] nvarchar(max)  NOT NULL,
    [StudyActions_Id] int  NOT NULL,
    [Module_Id] int  NOT NULL,
    [Teacher_Id] int  NOT NULL,
    [Group_Id] int  NOT NULL
);
GO

-- Creating table 'TeacherSet'
CREATE TABLE [dbo].[TeacherSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Rank] nvarchar(max)  NOT NULL,
    [TeacherType] nvarchar(max)  NOT NULL,
    [Position] nvarchar(max)  NOT NULL,
    [Degree] nvarchar(max)  NOT NULL,
    [Department_Id] int  NOT NULL
);
GO

-- Creating table 'FormulasSet'
CREATE TABLE [dbo].[FormulasSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Formula] nvarchar(max)  NOT NULL,
    [StudyActions_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'StudyCycleSet'
ALTER TABLE [dbo].[StudyCycleSet]
ADD CONSTRAINT [PK_StudyCycleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DepartmentSet'
ALTER TABLE [dbo].[DepartmentSet]
ADD CONSTRAINT [PK_DepartmentSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FacultySet'
ALTER TABLE [dbo].[FacultySet]
ADD CONSTRAINT [PK_FacultySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GroupSet'
ALTER TABLE [dbo].[GroupSet]
ADD CONSTRAINT [PK_GroupSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StudyActionsSet'
ALTER TABLE [dbo].[StudyActionsSet]
ADD CONSTRAINT [PK_StudyActionsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ModuleSet'
ALTER TABLE [dbo].[ModuleSet]
ADD CONSTRAINT [PK_ModuleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ModuleStudyActionSet'
ALTER TABLE [dbo].[ModuleStudyActionSet]
ADD CONSTRAINT [PK_ModuleStudyActionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TeacherSet'
ALTER TABLE [dbo].[TeacherSet]
ADD CONSTRAINT [PK_TeacherSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FormulasSet'
ALTER TABLE [dbo].[FormulasSet]
ADD CONSTRAINT [PK_FormulasSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Faculty_Id] in table 'DepartmentSet'
ALTER TABLE [dbo].[DepartmentSet]
ADD CONSTRAINT [FK_FacultyDepartment]
    FOREIGN KEY ([Faculty_Id])
    REFERENCES [dbo].[FacultySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FacultyDepartment'
CREATE INDEX [IX_FK_FacultyDepartment]
ON [dbo].[DepartmentSet]
    ([Faculty_Id]);
GO

-- Creating foreign key on [Department_Id] in table 'GroupSet'
ALTER TABLE [dbo].[GroupSet]
ADD CONSTRAINT [FK_DepartmentGroup]
    FOREIGN KEY ([Department_Id])
    REFERENCES [dbo].[DepartmentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentGroup'
CREATE INDEX [IX_FK_DepartmentGroup]
ON [dbo].[GroupSet]
    ([Department_Id]);
GO

-- Creating foreign key on [StudyActions_Id] in table 'ModuleStudyActionSet'
ALTER TABLE [dbo].[ModuleStudyActionSet]
ADD CONSTRAINT [FK_StudyActionsModuleStudyAction]
    FOREIGN KEY ([StudyActions_Id])
    REFERENCES [dbo].[StudyActionsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudyActionsModuleStudyAction'
CREATE INDEX [IX_FK_StudyActionsModuleStudyAction]
ON [dbo].[ModuleStudyActionSet]
    ([StudyActions_Id]);
GO

-- Creating foreign key on [Module_Id] in table 'ModuleStudyActionSet'
ALTER TABLE [dbo].[ModuleStudyActionSet]
ADD CONSTRAINT [FK_ModuleModuleStudyAction]
    FOREIGN KEY ([Module_Id])
    REFERENCES [dbo].[ModuleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ModuleModuleStudyAction'
CREATE INDEX [IX_FK_ModuleModuleStudyAction]
ON [dbo].[ModuleStudyActionSet]
    ([Module_Id]);
GO

-- Creating foreign key on [Teacher_Id] in table 'ModuleStudyActionSet'
ALTER TABLE [dbo].[ModuleStudyActionSet]
ADD CONSTRAINT [FK_TeacherModuleStudyAction]
    FOREIGN KEY ([Teacher_Id])
    REFERENCES [dbo].[TeacherSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeacherModuleStudyAction'
CREATE INDEX [IX_FK_TeacherModuleStudyAction]
ON [dbo].[ModuleStudyActionSet]
    ([Teacher_Id]);
GO

-- Creating foreign key on [Group_Id] in table 'ModuleStudyActionSet'
ALTER TABLE [dbo].[ModuleStudyActionSet]
ADD CONSTRAINT [FK_GroupModuleStudyAction]
    FOREIGN KEY ([Group_Id])
    REFERENCES [dbo].[GroupSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupModuleStudyAction'
CREATE INDEX [IX_FK_GroupModuleStudyAction]
ON [dbo].[ModuleStudyActionSet]
    ([Group_Id]);
GO

-- Creating foreign key on [Department_Id] in table 'TeacherSet'
ALTER TABLE [dbo].[TeacherSet]
ADD CONSTRAINT [FK_DepartmentTeacher]
    FOREIGN KEY ([Department_Id])
    REFERENCES [dbo].[DepartmentSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentTeacher'
CREATE INDEX [IX_FK_DepartmentTeacher]
ON [dbo].[TeacherSet]
    ([Department_Id]);
GO

-- Creating foreign key on [StudyActions_Id] in table 'FormulasSet'
ALTER TABLE [dbo].[FormulasSet]
ADD CONSTRAINT [FK_StudyActionsFormulas]
    FOREIGN KEY ([StudyActions_Id])
    REFERENCES [dbo].[StudyActionsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudyActionsFormulas'
CREATE INDEX [IX_FK_StudyActionsFormulas]
ON [dbo].[FormulasSet]
    ([StudyActions_Id]);
GO

-- Creating foreign key on [StudyCycle_Id] in table 'ModuleSet'
ALTER TABLE [dbo].[ModuleSet]
ADD CONSTRAINT [FK_StudyCycleModule]
    FOREIGN KEY ([StudyCycle_Id])
    REFERENCES [dbo].[StudyCycleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StudyCycleModule'
CREATE INDEX [IX_FK_StudyCycleModule]
ON [dbo].[ModuleSet]
    ([StudyCycle_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------