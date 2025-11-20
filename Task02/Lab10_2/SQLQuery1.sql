CREATE TABLE Department (
    DeptID INT PRIMARY KEY IDENTITY,
    DeptName NVARCHAR(100),
    DeptChair NVARCHAR(100)
);

CREATE TABLE Student (
    StudentID INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Major NVARCHAR(50),
    DeptID INT FOREIGN KEY REFERENCES Department(DeptID)
);
INSERT INTO Department (DeptName, DeptChair)
VALUES ('Computer Science', 'Engr. Raheela'),
       ('Mathematics', 'Dr. Adeel'),
       ('Engineering', 'Dr. Joddat');

INSERT INTO Student (FirstName, LastName, Major, DeptID)
VALUES ('Siawish', 'Malik', 'CS', 1),
       ('Dani', 'Qur', 'Math', 2),
       ('Talha', 'Naveed', 'Engineering', 3);
