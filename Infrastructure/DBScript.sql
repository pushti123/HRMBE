

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