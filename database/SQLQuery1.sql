CREATE TABLE Employee_Service (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT,  
    ServiceId INT,  
    FOREIGN KEY (EmployeeId) REFERENCES employee_information(Id),
    FOREIGN KEY (ServiceId) REFERENCES Manned_Guarding(Id)
);
select * from Booking
CREATE TABLE Booking (
    id INT PRIMARY KEY IDENTITY(1,1),  
    name VARCHAR(255) NOT NULL,        
    email VARCHAR(255) NOT NULL,        
    employee_id INT NOT NULL,           
    service_id INT NOT NULL,            
    booking_datetime DATETIME NOT NULL, 
    FOREIGN KEY (employee_id) REFERENCES employee_information(Id),
    FOREIGN KEY (service_id) REFERENCES Manned_Guarding(Id)
);

select * from Booking

create table Registeration_User	(
id int primary key identity (1,1),
name varchar (255),
Email varchar (255),
Password varchar (255)
)

create table Contact(
id int primary key identity (1,1),
name varchar (255),
email varchar (255),
phonenumber varchar (255),
subject varchar (255),
message varchar (255)
)

select * from Registeration_user

UPDATE Registeration_User 
SET Name = 'Unknown', Email = 'unknown@example.com', Password = 'default123' 
WHERE Name IS NULL OR Email IS NULL OR Password IS NULL;


CREATE TABLE admin_registeration_role(
Id INT PRIMARY KEY IDENTITY(1,1),
Role VARCHAR(255)
)

INSERT INTO admin_registeration_role values ('customer'),('admin');

CREATE TABLE admin_registeration(
Id INT PRIMARY KEY IDENTITY(1,1),
Name VARCHAR(255),
Email VARCHAR(255),
Password VARCHAR(255),
Role INT,
FOREIGN KEY (Role) REFERENCES admin_registeration_role(Id)
)


select * from admin_registeration_role
select * from admin_registeration

ALTER TABLE Manned_Guarding
ADD items VARCHAR(255);

drop table Manned_Guarding

create table Manned_Guarding
(
Id int primary key identity (1,1),
Title varchar(250),
Description varchar (250),
Image_Path varchar (250)
)

update Manned_Guarding set Description='involves professionally trained security personnel providing surveillance, access control, and protection for businesses, events, or private properties to ensure safety and prevent unauthorized activities.' where Id=1;

select * from Manned_Guarding


select * from employee_information
drop table employee_information

create table employee_information(
id int primary key identity (1,1),
name varchar (255),
address varchar (255),
phonenumber varchar (255),
qualification varchar (255),
role int,
grade varchar (255),
client varchar (255),
achievements varchar (255),
    FOREIGN KEY (role) REFERENCES Manned_Guarding(Id)
);




ALTER TABLE employee_information
ADD email VARCHAR(255);
EXEC sp_rename 'employee_information.Role', 'ServiceId', 'COLUMN';


ALTER TABLE employee_information
ADD action VARCHAR(255);

SELECT * FROM Manned_Guarding;
SELECT * FROM Employee_Information WHERE Action = 'Accepted';

SELECT * FROM employee_information WHERE Id = 1;
SELECT * FROM Manned_Guarding WHERE Id = @ServiceId;

create table Network
(
Id int primary key identity (1,1),
Title varchar(250),
Cell varchar (250),
Location varchar (250),
Email varchar (250)
)

INSERT INTO Network (Title, Cell, Location, Email) VALUES
('Naran', '+92 997 1234567', 'Naran, KPK, Pakistan', 'naran@starsecurity.com'),
('Bahawalpur', '+92 300 9876543', 'Bahawalpur, Punjab, Pakistan', 'bahawalpur@starsecurity.com'),
('Gilgit', '+92 300 1234567', 'Gilgit, Gilgit-Baltistan, Pakistan', 'gilgit@starsecurity.com'),
('Hunza', '+92 300 6543210', 'Hunza, Gilgit-Baltistan, Pakistan', 'hunza@starsecurity.com'),
('Kaghan', '+92 300 5432109', 'Kaghan, KPK, Pakistan', 'kaghan@starsecurity.com'),
('Chitral', '+92 943 1234567', 'Chitral, KPK, Pakistan', 'chitral@starsecurity.com'),
('Karachi', '+92 21 1234567', 'Karachi, Sindh, Pakistan', 'karachi@starsecurity.com'),
('Nawabshah', '+92 300 1234567', 'Nawabshah, Sindh, Pakistan', 'nawabshah@starsecurity.com'),
('Sialkot', '+92 300 9876543', 'Sialkot, Punjab, Pakistan', 'sialkot@starsecurity.com'),
('Gwadar', '+92 400 1234567', 'Gwadar, Balochistan, Pakistan', 'gwadar@starsecurity.com'),
('Multan', '+92 300 1234567', 'Multan, Punjab, Pakistan', 'multan@starsecurity.com'),

INSERT INTO Network (Title, Cell, Location, Email) VALUES
('Mirpurkhas', '+92 300 1234567', 'Mirpurkhas, Sindh, Pakistan', 'mirpurkhas@starsecurity.com');
