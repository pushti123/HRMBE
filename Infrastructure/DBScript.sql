

Create database HRMDB;

Create table TokenMst(
  Id int Primary key not null Identity(1,1),
  Token nvarchar(max) not null,
  RefreshToken nvarchar(max) not null,
  TokenExpiryDate datetime not null,
  RefreshTokenExpiryDate datetime not null,
  CreatedBy int not null,
  UpdatedBy int not null,
  CreatedAt datetime not null,
  UpdatedAt datetime not null,
  UserId int not null,
  IsActive bit not null,
  IsDelete bit not null,
)


Create table RoleMst(
 Id int Primary key not null Identity(1,1),
 RoleName varchar(50) not null,
  CreatedBy int not null,
  UpdatedBy int not null,
  CreatedAt datetime not null,
 UpdatedAt datetime not null,
  IsActive bit not null,
  IsDelete bit not null,
 )

 Create table DepartmentMst(
 Id int Primary key not null Identity(1,1),
 DepartmentName varchar(50) not null,
  CreatedBy int not null,
  UpdatedBy int not null,
  CreatedAt datetime not null,
 UpdatedAt datetime not null,
  IsActive bit not null,
  IsDelete bit not null,
 )

 Create table DesignationMst(
 Id int Primary key not null Identity(1,1),
 DesignationName varchar(50) not null,
 DepartmentId int not null,
  CreatedBy int not null,
  UpdatedBy int not null,
  CreatedAt datetime not null,
 UpdatedAt datetime not null,
  IsActive bit not null,
  IsDelete bit not null,
 )

 Create table PermissionMst(
 Id int Primary key not null Identity(1,1),
 PermissionName varchar(50) not null,
 PermissionDescription varchar(500) not null,
  CreatedBy int not null,
  UpdatedBy int not null,
  CreatedAt datetime not null,
 UpdatedAt datetime not null,
  IsActive bit not null,
  IsDelete bit not null,
 )

 Create table RolePermissionMst(
 Id int Primary key not null Identity(1,1),
 RoleId int not null,
 PermissionId int not null,
  CreatedBy int not null,
  UpdatedBy int not null,
  CreatedAt datetime not null,
  UpdatedAt datetime not null,
  IsActive bit not null,
  IsDelete bit not null,
 )

 Create table UserMst(
 Id int Primary key not null Identity(1,1),
 UserId varchar(100) not null,
 Fullname varchar(500) not null,
 Email varchar(100) not null,
 Gender varchar(20) not null,
 ProfilePic nvarchar(max) null,
 Password nvarchar(max) not null,
 ConfirmPassword nvarchar (max) not null,
 Contact varchar(20) not null,
 RoleId int not null,
 DepartmentId int not null,
 Designation int not null,
 AadharNumber varchar(30) not null,
 BankName VARCHAR(MAX) NOT NULL,
 BankNumber varchar(30) not null,
  CreatedBy int not null,
  UpdatedBy int not null,
  CreatedAt datetime not null,
  UpdatedAt datetime not null,
  IsActive bit not null,
  IsDelete bit not null,
 )

 Alter table UserMst  alter column [RoleId] [int]  NULL;
	Alter table UserMst  alter column [DepartmentId] [int]  NULL;
	Alter table UserMst  alter column [Designation] [int]  NULL;
	Alter table UserMst  alter column [AadharNumber] [varchar](30) NOT NULL;
	Alter table UserMst  alter column [BankName] [varchar](max)  NULL;
	Alter table UserMst  alter column [BankNumber] [varchar](30)  NULL;


	 Create table TicketMst(
 Id int Primary key not null Identity(1,1),
 UserId int not null,
 Subject varchar(100) not null,
 Description nvarchar(max) not null,
 Status varchar(50) not null,
Comment nvarchar(max) not null,
 Isdeleted bit not null,
 CreatedBy int not null,
 UpdatedBy int not null,
 CreatedAt datetime not null,
 UpdatedAt datetime not null,
 Comments nvarchar(max) null,
 )

 CREATE TABLE LeaveTypes
(
    LeaveTypeId INT IDENTITY(1,1) PRIMARY KEY,
    LeaveName VARCHAR(50) NOT NULL,
    LeaveCode VARCHAR(20) NULL,
    Description VARCHAR(200) NULL,
    IsActive BIT DEFAULT 1,
    IsAutoAssign BIT,
    CreatedDate DATETIME DEFAULT GETDATE(),
	UpdatedDate DATETIME DEFAULT GETDATE(),
	IsDelete Bit Default 0,
);

CREATE TABLE LeavePolicies
(
    LeavePolicyId INT IDENTITY(1,1) PRIMARY KEY,

    LeaveTypeId INT NOT NULL,

    IsProbationApplicable BIT DEFAULT 0,
    ProbationLeaveDays INT NULL,

    LeaveCreditType VARCHAR(10) NULL, -- Monthly / Yearly
    LeaveDays INT NULL,

    IsCarryForward BIT DEFAULT 0,
    MaxCarryForwardDays INT NULL,

    IsSandwichApplicable BIT DEFAULT 0,

	IsHolidaySandwichApplicable BIT Default 0,
	IsWeelOffSandwichApplicable BIT Default 0,
    MaxLeavePerRequest INT NULL,

    IsHalfDayAllowed BIT DEFAULT 0,

    IsActive BIT DEFAULT 1,
	IsDelete Bit Default 0,
    CreatedDate DATETIME DEFAULT GETDATE(),
	 UpdatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_LeavePolicy_LeaveType
    FOREIGN KEY (LeaveTypeId) REFERENCES LeaveTypes(LeaveTypeId)
);

CREATE TABLE Holidays
(
    HolidayId INT IDENTITY(1,1) PRIMARY KEY,
    HolidayName VARCHAR(100) NOT NULL,
    HolidayDate DATE NOT NULL,
    Description VARCHAR(200) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
	UpdatedDate DATETIME DEFAULT GETDATE(),
	 IsActive BIT DEFAULT 1,
	IsDelete Bit Default 0,

);

CREATE TABLE EmployeeLeaveBalances
(
    BalanceId INT IDENTITY(1,1) PRIMARY KEY,

    EmployeeId INT NOT NULL,

    LeaveTypeId INT NOT NULL,

    Year INT NOT NULL,

    AllocatedDays INT NOT NULL,

    UsedDays INT DEFAULT 0,

    RemainingDays INT NOT NULL,

    CreatedDate DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_Balance_LeaveType
    FOREIGN KEY (LeaveTypeId) REFERENCES LeaveTypes(LeaveTypeId)
);

CREATE TABLE LeaveApplications
(
    LeaveApplicationId INT IDENTITY(1,1) PRIMARY KEY,

    EmployeeId INT NOT NULL,

    LeaveTypeId INT NOT NULL,

    FromDate DATE NOT NULL,

    ToDate DATE NOT NULL,

    TotalDays DECIMAL(4,1) NOT NULL,
    PaidLeaveDays DECIMAL(5,2),
    LwpDays DECIMAL(5,2),

    Reason VARCHAR(500) NULL,

    Status VARCHAR(20) DEFAULT 'Pending',

    AppliedDate DATETIME DEFAULT GETDATE(),

    ApprovedBy INT NULL,

    ApprovedDate DATETIME NULL,

    CONSTRAINT FK_LeaveApplication_LeaveType
    FOREIGN KEY (LeaveTypeId) REFERENCES LeaveTypes(LeaveTypeId)
);

CREATE TABLE LeaveApplicationDetails
(
    LeaveDetailId INT IDENTITY(1,1) PRIMARY KEY,

    LeaveApplicationId INT NOT NULL,

    LeaveDate DATE NOT NULL,

    LeaveDayType VARCHAR(20) NOT NULL,  
    -- FullDay / FirstHalf / SecondHalf

    CreatedDate DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_LeaveApplicationDetails
    FOREIGN KEY (LeaveApplicationId)
    REFERENCES LeaveApplications(LeaveApplicationId)
);

--Pending
--Approved
--Rejected
--Cancelled

--PaidLeaveDays DECIMAL(5,2),
--    LwpDays DECIMAL(5,2),

--	alter table LeaveApplications add PaidLeaveDays DECIMAL(5,2);
--		alter table LeaveApplications add  LwpDays DECIMAL(5,2);


CREATE TABLE Candidates
(
    CandidateId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(100),
    LastName VARCHAR(100),
    Email VARCHAR(150),
    CountryCode VARCHAR(10),
    Mobile VARCHAR(20),
    IsActive BIT DEFAULT 0,
    FinalSubmitted BIT DEFAULT 0,
	ResumeParserUserId int not null,
    CreatedDate DATETIME DEFAULT GETDATE(),
)


CREATE TABLE CandidateResumes
(
    ResumeId INT Identity(1,1) PRIMARY KEY,
    CandidateId INT not null,
	ResumeParserUserId int not null,
    FileName VARCHAR(200),
    FilePath VARCHAR(500),
    ParsedJson NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE(),
	JobId INT not null,
)

CREATE TABLE JobApplications
(
    JobApplicationId INT Identity(1,1) PRIMARY KEY,
    CandidateId INT not null,
    ResumeId INT not null,
    JobId INT not null,
	ResumeParserUserId int not null,
	JobTitle varchar(100),
    CreatedDate DATETIME DEFAULT GETDATE(),
	IsActive BIT DEFAULT 0,
    FinalSubmitted BIT DEFAULT 0
)

CREATE TABLE CandidateWorkExperiences
(
    WorkId INT Identity(1,1) PRIMARY KEY,
	JobId INT not null,
	CandidateId int not null,
    CompanyName VARCHAR(200),
    CandidateWorkExperiencesJobTitle VARCHAR(200),
    StartDate DATE,
    EndDate DATE,
    IsCurrentCompany BIT,
	    JobApplicationId INT,
    Description NVARCHAR(MAX)
)

CREATE TABLE CandidateEducations
(
    EducationId INT Identity(1,1) PRIMARY KEY,
    JobApplicationId INT,
    CollegeName VARCHAR(200),
    Degree VARCHAR(100),
    FieldOfStudy VARCHAR(100),
    StartYear INT,
    EndYear INT,
	CandidateId int not null,
)

CREATE TABLE CandidateSkills
(
 SkillId INT Identity(1,1) PRIMARY KEY,
JobApplicationId INT,
SkillName VARCHAR(100),
	CandidateId int not null,

)

CREATE TABLE CandidateLanguages
(
    LanguageId INT Identity(1,1) PRIMARY KEY,

    JobApplicationId INT,
		CandidateId int not null,

    LanguageName VARCHAR(100)
)

CREATE TABLE ApplicationQuestions
(
    QuestionId INT Identity(1,1) PRIMARY KEY,

    JobApplicationId INT,
		CandidateId int not null,

    ExpectedCTC DECIMAL(10,2),

    NoticePeriod INT,
	IsTeferral bit,
    Source VARCHAR(100),
    ReferralName VARCHAR(100)
)

CREATE TABLE ApplicationDeclarations
(
    DeclarationId INT Identity(1,1) PRIMARY KEY,

    JobApplicationId INT,
			CandidateId int not null,

    AcceptedTerms BIT,

    SubmittedDate DATETIME
)

create table ResumeParserUser(

ResumeParserId int identity(1,1) primary key,
Email varchar(50) not null,
Password nvarchar(50) not null
)