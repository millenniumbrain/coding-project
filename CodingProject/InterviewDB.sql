USE interview;
-- While it is convention to name id [tablename]ID name it id
-- to reflect the id object property for both tables
CREATE TABLE people (
	id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	username NVARCHAR(50) NOT NULL,
	firstName NVARCHAR(100) NOT NULL,
	lastName NVARCHAR(100) NOT NULL,
	dob NVARCHAR(50) NOT NULL
);

CREATE TABLE addresses (
	id INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	streetOne VARCHAR(200) NOT NULL,
	streetTwo NVARCHAR(200) NOT NULL,
	city NVARCHAR(50) NOT NULL,
	state NVARCHAR(100) NOT NULL,
	zipCode NVARCHAR(50) NOT NULL,
	personID INT,
	CONSTRAINT fk_address_people FOREIGN KEY (personID) REFERENCES dbo.people (id)
);