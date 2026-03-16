Alter table UserMst  add  [Address] nvarchar(max) NULL;
	Alter table userMst  add [Pincode] nvarchar(8)  NULL;
INSERT INTO UserMst (
    UserId, Fullname, Email, Gender, ProfilePic, 
    Password, ConfirmPassword, Contact, RoleId, 
    DepartmentId, Designation, AadharNumber, 
    BankName, BankNumber, CreatedBy, UpdatedBy, 
    CreatedAt, UpdatedAt, IsActive, IsDelete
)
VALUES (
    'ADMIN001', 
    'System Admin', 
    'admin@gmail.com', 
    'Other', 
    NULL, 
    'Admin@123', 
    'Admin@123', 
    '0000000000', 
    1, -- RoleId
    1, -- DepartmentId
    1, -- Designation
    '0000-0000-0000', 
    'System Bank', 
    '00000000', 
    1, -- CreatedBy (Self)
    1, -- UpdatedBy (Self)
    GETDATE(), 
    GETDATE(), 
    1, -- IsActive (True)
    0  -- IsDelete (False)

);INSERT INTO RoleMst
(
    RoleName,
    CreatedBy,
    UpdatedBy,
    CreatedAt,
    UpdatedAt,
    IsActive,
    IsDelete
)
VALUES
('Admin',    1, 1, GETDATE(), GETDATE(), 1, 0),
('Employee', 1, 1, GETDATE(), GETDATE(), 1, 0),
('HR',       1, 1, GETDATE(), GETDATE(), 1, 0),
('Manager',  1, 1, GETDATE(), GETDATE(), 1, 0),
('User',  1, 1, GETDATE(), GETDATE(), 1, 0);


INSERT INTO DepartmentMst
(
    DepartmentName,
    CreatedBy,
    UpdatedBy,
    CreatedAt,
    UpdatedAt,
    IsActive,
    IsDelete
)
VALUES
('Sales',       1, 1, GETDATE(), GETDATE(), 1, 0),
('ITEngineer', 1, 1, GETDATE(), GETDATE(), 1, 0),
('Developer',   1, 1, GETDATE(), GETDATE(), 1, 0),
('Support',     1, 1, GETDATE(), GETDATE(), 1, 0),
('HR',          1, 1, GETDATE(), GETDATE(), 1, 0),
('Recruiter',   1, 1, GETDATE(), GETDATE(), 1, 0);


INSERT INTO DesignationMst
(DesignationName, DepartmentId, CreatedBy, UpdatedBy, CreatedAt, UpdatedAt, IsActive, IsDelete)
SELECT 'IT Support', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'ITEngineer';

INSERT INTO DesignationMst
SELECT 'Junior .NET Developer', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Developer';

INSERT INTO DesignationMst
SELECT 'Senior React Developer', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Developer';

INSERT INTO DesignationMst
SELECT 'Trainee', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Developer';

INSERT INTO DesignationMst
SELECT 'Salesforce Developer', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Developer';

INSERT INTO DesignationMst
SELECT 'Angular Developer', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Developer';

INSERT INTO DesignationMst
SELECT 'Project Manager', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Developer';

INSERT INTO DesignationMst
SELECT 'Senior Human Resource', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'HR';

INSERT INTO DesignationMst
SELECT 'Junior HR', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'HR';

INSERT INTO DesignationMst
SELECT 'Junior Sales', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Sales';

INSERT INTO DesignationMst
SELECT 'Senior Sales', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Sales';

INSERT INTO DesignationMst
SELECT 'Sales Team Lead', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Sales';

INSERT INTO DesignationMst
SELECT 'Product Support', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Support';

INSERT INTO DesignationMst
SELECT 'Support Engineer', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Support';

INSERT INTO DesignationMst
SELECT 'Support Team Lead', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Support';

INSERT INTO DesignationMst
SELECT 'Recruiter Lead', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Recruiter';

INSERT INTO DesignationMst
SELECT 'Junior Recruiter', Id, 1, 1, GETDATE(), GETDATE(), 1, 0
FROM DepartmentMst WHERE DepartmentName = 'Recruiter';


INSERT INTO PermissionMst
(
    PermissionName,
    PermissionDescription,
    CreatedBy,
    UpdatedBy,
    CreatedAt,
    UpdatedAt,
    IsActive,
    IsDelete
)
VALUES
('AddUser',        'Add User',        1, 1, GETDATE(), GETDATE(), 1, 0),
('EditUser',       'Edit User',       1, 1, GETDATE(), GETDATE(), 1, 0),
('DeleteUser',     'Delete User',     1, 1, GETDATE(), GETDATE(), 1, 0),
('ViewUser',       'View User',       1, 1, GETDATE(), GETDATE(), 1, 0),
('ViewPermission', 'View Permission', 1, 1, GETDATE(), GETDATE(), 1, 0),
('EditPermission', 'Edit Permission', 1, 1, GETDATE(), GETDATE(), 1, 0),
('AddTicket', 'Add Ticket', 1, 1, GETDATE(), GETDATE(), 1, 0),
('EditTicket', 'Edit Ticket', 1, 1, GETDATE(), GETDATE(), 1, 0),
('ViewTicket', 'View Ticket', 1, 1, GETDATE(), GETDATE(), 1, 0),
('DeleteTicket', 'Delete Ticket', 1, 1, GETDATE(), GETDATE(), 1, 0),
('UpdateTicketStatus', 'Update Ticket Status', 1, 1, GETDATE(), GETDATE(), 1, 0),
('UpdateLeaveStatus', 'Update Leave Status', 1, 1, GETDATE(), GETDATE(), 1, 0),
('AddLeave', 'Add Leave', 1, 1, GETDATE(), GETDATE(), 1, 0),
('EditLeave', 'Edit Leave', 1, 1, GETDATE(), GETDATE(), 1, 0),
('AddLeaveType', 'Add LeaveType', 1, 1, GETDATE(), GETDATE(), 1, 0),
('EditLeaveType', 'Edit LeaveType', 1, 1, GETDATE(), GETDATE(), 1, 0),
('ViewLeaveType', 'View LeaveType', 1, 1, GETDATE(), GETDATE(), 1, 0),
('DeleteLeaveType', 'Delete LeaveType', 1, 1, GETDATE(), GETDATE(), 1, 0),
('ViewLeavePolicy', 'View LeavePolicy', 1, 1, GETDATE(), GETDATE(), 1, 0),
('AddLeavePolicy', 'Add LeavePolicy', 1, 1, GETDATE(), GETDATE(), 1, 0),
('EditLeavePolicy', 'Edit LeavePolicy', 1, 1, GETDATE(), GETDATE(), 1, 0);



;


INSERT INTO RolePermissionMst
(
    RoleId,
    PermissionId,
    CreatedBy,
    UpdatedBy,
    CreatedAt,
    UpdatedAt,
    IsActive,
    IsDelete
)
SELECT 
    r.Id,
    p.Id,
    1,
    1,
    GETDATE(),
    GETDATE(),
    1,
    0
FROM RoleMst r
CROSS JOIN PermissionMst p
WHERE r.RoleName = 'Admin'
  AND r.IsDelete = 0
  AND p.IsDelete = 0;


  INSERT INTO RolePermissionMst
(
    RoleId,
    PermissionId,
    CreatedBy,
    UpdatedBy,
    CreatedAt,
    UpdatedAt,
    IsActive,
    IsDelete
)
SELECT 
    r.Id,
    p.Id,
    1,
    1,
    GETDATE(),
    GETDATE(),
    1,
    0
FROM RoleMst r
JOIN PermissionMst p
    ON p.PermissionName IN 
       ('AddUser', 'EditUser', 'ViewUser', 'ViewPermission')
WHERE r.RoleName = 'HR'
  AND r.IsDelete = 0
  AND p.IsDelete = 0;


  
  INSERT INTO RolePermissionMst
(
    RoleId,
    PermissionId,
    CreatedBy,
    UpdatedBy,
    CreatedAt,
    UpdatedAt,
    IsActive,
    IsDelete
)
SELECT 
    r.Id,
    p.Id,
    1,
    1,
    GETDATE(),
    GETDATE(),
    1,
    0
FROM RoleMst r
JOIN PermissionMst p
    ON p.PermissionName IN 
       ( 'AddTicket','EditTicket','DeleteTicket','UpdateTicketStatus')
WHERE r.RoleName = 'Manager'
  AND r.IsDelete = 0
  AND p.IsDelete = 0;

   INSERT INTO RolePermissionMst
(
    RoleId,
    PermissionId,
    CreatedBy,
    UpdatedBy,
    CreatedAt,
    UpdatedAt,
    IsActive,
    IsDelete
)
SELECT 
    r.Id,
    p.Id,
    1,
    1,
    GETDATE(),
    GETDATE(),
    1,
    0
FROM RoleMst r
JOIN PermissionMst p
    ON p.PermissionName IN 
       ( 'EditTicket','UpdateTicketStatus')
WHERE r.RoleName = 'Manager'
  AND r.IsDelete = 0
  AND p.IsDelete = 0;

  INSERT INTO Holidays (HolidayName, HolidayDate, Description)
VALUES
('Republic Day', '2026-01-26', 'National Holiday'),

('Mahashivratri', '2026-02-15', 'Hindu Festival'),

('Holi', '2026-03-14', 'Festival of Colors'),

('Ram Navami', '2026-03-30', 'Hindu Festival'),

('Independence Day', '2026-08-15', 'National Holiday'),

('Gandhi Jayanti', '2026-10-02', 'Birth Anniversary of Mahatma Gandhi');