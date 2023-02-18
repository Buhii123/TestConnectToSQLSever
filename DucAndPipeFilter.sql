Create Database MEP
Go

Use MEP
Go
--Duct
Create Table MEP_Duct
(
	STT INTEGER IDENTITY(1,1) PRIMARY KEY not null,
	ElementID NVARCHAR(255),
	NameTypeDuct NVARCHAR(255),
	[Length] FLOAT,
	Height FLOAT,
	Width FLOAT,
	SystemClass NVARCHAR(255),
	Comments NVARCHAR(255)
)
Go


Create Table MEP_DuctFiting
(
	STT INTEGER IDENTITY(1,1) PRIMARY KEY Not null,
	ElementID NVARCHAR(255),
	NameTypeDuct NVARCHAR(255),
	
	Size NVARCHAR(255),
	SystemClass NVARCHAR(255),
	Comments NVARCHAR(255)
)
Go


--Pipe

Create Table MEP_Pipe
(
	STT INTEGER IDENTITY(1,1) PRIMARY KEY Not null,
	ElementID NVARCHAR(255),
	NameTypePipe NVARCHAR(255),
	[Length] FLOAT,
	Size NVARCHAR(255),
	SystemClass NVARCHAR(255),
	Comments NVARCHAR(255)
)
Go



Create Table MEP_PipeFiting
(
	STT INTEGER IDENTITY(1,1) PRIMARY KEY Not null,
	ElementID NVARCHAR(255),
	NameTypePipe NVARCHAR(255),
	
	Size NVARCHAR(255),
	SystemClass NVARCHAR(255),
	Comments NVARCHAR(255)
)
Go



Create Table MEP_PipeAccessories
(
	STT INTEGER IDENTITY(1,1) PRIMARY KEY Not null,
	ElementID NVARCHAR(255),
	NameTypePipe NVARCHAR(255),
	
	Size NVARCHAR(255),
	SystemClass NVARCHAR(255),
	Comments NVARCHAR(255)
)

Go

