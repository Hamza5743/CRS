use CRS

DELETE FROM Comments;
DELETE FROM [Image];
DELETE FROM Complaints;
DELETE FROM Feedback;
DELETE FROM department;
DELETE FROM Users;

GO
DECLARE @output NVARCHAR(50)

EXECUTE user_signup @email = 'test@lhr.nu.edu.pk', @fname = 'test', @lname = 'user', @password = 'testpass', @OutputMsg = @output OUTPUT

EXECUTE Add_Department @Dept_Name = 'Computer Science', @Admin_Email = 'admin1@nu.edu.pk', @Admin_Fname = 'admin', @Admin_Lname = '1', @Admin_Password = 'admin1pass', @OutputMsg = @output OUTPUT

EXECUTE Add_Department @Dept_Name = 'Electrical Engineering', @Admin_Email = 'admin2@nu.edu.pk', @Admin_Fname = 'admin', @Admin_Lname = '2', @Admin_Password = 'admin2pass', @OutputMsg = @output OUTPUT

EXECUTE Add_Department @Dept_Name = 'Civil Engineering', @Admin_Email = 'admin3@nu.edu.pk', @Admin_Fname = 'admin', @Admin_Lname = '3', @Admin_Password = 'admin3pass', @OutputMsg = @output OUTPUT

EXECUTE Add_Department @Dept_Name = 'Mechanical Engineering', @Admin_Email = 'admin4@nu.edu.pk', @Admin_Fname = 'admin', @Admin_Lname = '4', @Admin_Password = 'admin4pass', @OutputMsg = @output OUTPUT

EXECUTE Add_Suggestion @filer = 'test@lhr.nu.edu.pk', @content = 'sugg1', @deptId = 1, @OutputMsg = @output OUTPUT

EXECUTE Add_Suggestion @filer = 'test@lhr.nu.edu.pk', @content = 'sugg2', @deptId = 1, @OutputMsg = @output OUTPUT

EXECUTE Add_Suggestion @filer = 'test@lhr.nu.edu.pk', @content = 'sugg3', @deptId = 1, @OutputMsg = @output OUTPUT

EXECUTE Add_Suggestion @filer = 'test@lhr.nu.edu.pk', @content = 'sugg4', @deptId = 1, @OutputMsg = @output OUTPUT

EXECUTE Add_Suggestion @filer = 'test@lhr.nu.edu.pk', @content = 'sugg6', @deptId = 1, @OutputMsg = @output OUTPUT

EXECUTE Add_Complaint @filer = 'test@lhr.nu.edu.pk', @content = 'comp1', @deptId = 2, @deadline = '06/10/2020', @priority = 'Low', @OutputMsg = @output OUTPUT

EXECUTE Add_Complaint @filer = 'test@lhr.nu.edu.pk', @content = 'comp2', @deptId = 2, @deadline = '06/11/2020', @priority = 'Medium', @OutputMsg = @output OUTPUT

EXECUTE Add_Complaint @filer = 'test@lhr.nu.edu.pk', @content = 'comp3', @deptId = 2, @deadline = '06/12/2020', @priority = 'High', @OutputMsg = @output OUTPUT

EXECUTE Add_Complaint @filer = 'test@lhr.nu.edu.pk', @content = 'comp4', @deptId = 2, @deadline = '06/13/2020', @priority = 'Done', @OutputMsg = @output OUTPUT

EXECUTE Add_Complaint @filer = 'test@lhr.nu.edu.pk', @content = 'comp5', @deptId = 2, @deadline = '06/14/2020', @priority = 'Rejected', @OutputMsg = @output OUTPUT

DECLARE @output NVARCHAR(50)
EXECUTE Add_Comment @complaintId = 6, @commentorEmail = 'admin1@nu.edu.pk', @content = 'com1', @OutputMsg = @output OUTPUT

DECLARE @output NVARCHAR(50)
EXECUTE Add_Comment @complaintId = 6, @commentorEmail = 'test@lhr.nu.edu.pk', @content = 'com2', @OutputMsg = @output OUTPUT

DECLARE @output NVARCHAR(50)
EXECUTE Add_Comment @complaintId = 6, @commentorEmail = 'admin1@nu.edu.pk', @content = 'com3', @OutputMsg = @output OUTPUT

DECLARE @output NVARCHAR(50)
EXECUTE Add_Comment @complaintId = 6, @commentorEmail = 'test@lhr.nu.edu.pk', @content = 'com4', @OutputMsg = @output OUTPUT

DECLARE @output NVARCHAR(50)
EXECUTE Add_Comment @complaintId = 6, @commentorEmail = 'admin1@nu.edu.pk', @content = 'com5', @OutputMsg = @output OUTPUT
GO

SELECT * FROM Users;
SELECT * FROM department;
SELECT * FROM Feedback;
SELECT * FROM Complaints;
SELECT * FROM Comments;
SELECT * FROM [Image];

drop database CRS