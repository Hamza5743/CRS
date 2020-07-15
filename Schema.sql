--Complaint Registration System

CREATE database CRS;

use CRS;

--Schema
CREATE TABLE Users
(
	email NVARCHAR(40) NOT NULL UNIQUE CHECK (email LIKE '%@lhr.nu.edu.pk' OR email LIKE '%@nu.edu.pk'),
	fname NVARCHAR(20) NOT NULL,
	lname NVARCHAR(20) NOT NULL,
	cnic NVARCHAR(15) CHECK (cnic LIKE '_____-_______-_'),
	[type] NVARCHAR(10) NOT NULL CHECK ([type] = 'User' OR [type] = 'Admin' OR [type] = 'MainAdmin'),
    [password] NVARCHAR(20) NOT NULL CHECK (LEN([password]) >= 8),
	PRIMARY KEY(email),
);

CREATE TABLE department
(
	id INT IDENTITY(1,1) NOT NULL UNIQUE,
	[name] NVARCHAR(60) NOT NULL,
	AdminEmail NVARCHAR(40) UNIQUE,
	PRIMARY KEY(id),
	FOREIGN KEY(AdminEmail) REFERENCES Users(email) ON UPDATE CASCADE ON DELETE CASCADE,
);

CREATE TABLE Feedback
(
	id INT IDENTITY(1,1) NOT NULL UNIQUE,
	filer NVARCHAR(40),
	content NVARCHAR(max) NOT NULL,
	deptId INT NOT NULL,
	[type] NVARCHAR(15) NOT NULL CHECK ([type] = 'Complaint' OR [type] = 'Suggestion'),
	PRIMARY KEY(id),
	FOREIGN KEY(filer) REFERENCES Users(email) ON DELETE SET NULL ON UPDATE CASCADE,
);

CREATE TABLE Complaints
(
	id INT NOT NULL UNIQUE,
	deadline DATE NOT NULL CHECK (deadline >= GETDATE()),
	[priority] NVARCHAR(10) NOT NULL CHECK ([priority] = 'High' OR [priority] = 'Medium' OR [priority] = 'Low' OR [priority] = 'Done' OR [priority] = 'Rejected'),
	PRIMARY KEY(id),
	FOREIGN KEY(id) REFERENCES Feedback(id) ON DELETE CASCADE ON UPDATE CASCADE,
);

CREATE TABLE [Image]
(
	id INT IDENTITY(1,1) NOT NULL UNIQUE,
	imageData VARBINARY(max),
	[type] NVARCHAR(5) NOT NULL CHECK ([type] = '.jpg' OR [type] = '.png' OR [type] = '.gif' OR [type] = '.jpeg'),
	feedbackId INT NOT NULL,
	PRIMARY KEY(id),
	FOREIGN KEY(FeedbackId) REFERENCES Feedback(id) ON DELETE CASCADE ON UPDATE CASCADE,
);

CREATE TABLE Comments
(
	complaintId INT NOT NULL,
	commentorEmail NVARCHAR(40),
	content NVARCHAR(max) NOT NULL,
	commTimeStamp DATETIME NOT NULL,
	PRIMARY KEY(complaintId,commTimeStamp),
	FOREIGN KEY(complaintId) REFERENCES Complaints(id) ON UPDATE CASCADE ON DELETE CASCADE,
);

--TRIGGER to check if deptId in Feedback actually exists
GO
CREATE TRIGGER Feedback_Logic ON Feedback
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @id INT, @filer NVARCHAR(40), @content NVARCHAR(max), @deptId INT, @type NVARCHAR(15)
	SELECT @id = id, @filer = filer, @content = content, @deptId = deptId, @type = [type]
	FROM INSERTED

	IF @deptid IN (SELECT id FROM Department)
	BEGIN
		INSERT INTO Feedback(filer, content, deptId, [type]) VALUES (@filer, @content, @deptId, @type)
	END
END;
GO

--TRIGGER to check if commentorEmail exists in Users
GO
CREATE TRIGGER Comment_Logic ON Comments
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @complaintId INT, @commentorEmail NVARCHAR(40),	@content NVARCHAR(max),	@commTimeStamp DATETIME
	SELECT @complaintId = complaintId, @commentorEmail = commentorEmail, @content = content, @commTimeStamp = commTimeStamp
	FROM INSERTED

	IF @commentorEmail in (SELECT email FROM Users)
	BEGIN
		INSERT INTO Comments(complaintId, commentorEmail, content, commTimeStamp) VALUES (@complaintId, @commentorEmail, @content, GETDATE())
	END
END;
GO

--PROCEDURE for Sign-Up
GO
CREATE PROCEDURE user_signup
(
	@email NVARCHAR(40),
	@fname NVARCHAR(20),
	@lname NVARCHAR(20),
	@cnic NVARCHAR(15) = null,
    @password NVARCHAR(20),
	@OutputMsg NVARCHAR(50) Output
)
AS 
BEGIN
	IF @email in (SELECT email FROM Users)
	BEGIN
		Set @OutputMsg = 'Email Already Exists!'
	End
	ELSE
	BEGIN
		IF Len(@password) < 8
		BEGIN
			Set @OutputMsg = 'Password too Short!'
		End
		ELSE
		BEGIN
			IF @email NOT LIKE '%@lhr.nu.edu.pk' AND @email NOT LIKE '%@nu.edu.pk'
			BEGIN
				Set @OutputMsg = 'Wrong Email Format!'
			END
			ELSE
			BEGIN
				IF @cnic IS NOT NULL AND @cnic NOT LIKE '_____-_______-_'
				BEGIN
					Set @OutputMsg = 'Wrong CNIC Format!'
				END
				ELSE
				BEGIN
					Set @OutputMsg = 'User Created!'
					Insert Into Users (email,fname,lname,cnic,[type],[password]) Values (@email,@fname,@lname,@cnic,'User',@password);
				END
			END
		End
	End
End;
GO

--PROCEDURE for login
GO
CREATE PROCEDURE user_login
(
	@username NVARCHAR(40),
	@pass NVARCHAR(20),
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	DECLARE @DBUsername NVARCHAR(40), @DBPassword NVARCHAR(20), @DBType NVARCHAR(10)
	SELECT @DBUsername = email, @DBPassword = [password], @DBType = [type] FROM Users WHERE @username = email
	IF @username = @DBUsername AND @pass = @DBPassword
	BEGIN
		Set @OutputMsg = @DBType
	End
	ELSE
	BEGIN
		Set @OutputMsg = 'Username or Password is Incorrect!'
	End
END;
GO

--PROCEDURE to delete a user FROM Database
GO
CREATE PROCEDURE Del_User
(
	@username NVARCHAR(40),
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @username in (SELECT commentorEmail FROM Comments)
	BEGIN
		UPDATE Comments Set commentorEmail = NULL WHERE commentorEmail = @username
	End

	Delete FROM [Users] WHERE email = @username
	SET @OutputMsg = 'User Deleted'
END;
GO

--Change User Password
GO
CREATE PROCEDURE Change_Password
(
	@username NVARCHAR(40),
	@password NVARCHAR(20),
	@OutputMsg NVARCHAR(50) OUTPUT
)
AS
BEGIN
	IF @username in (SELECT email FROM Users)
	BEGIN
		IF LEN(@password) < 8
		BEGIN
			SET @OutputMsg = 'Password is Too Short!'
		END
		ELSE
		BEGIN
			UPDATE Users SET [password] = @password WHERE email = @username
			SET @OutputMsg = 'Password Updated!'
		END
	END
	ELSE
	BEGIN
		SET @OutputMsg = 'User Does Not Exist!'
	END
END;
GO

--PROCEDURE to add a suggestion
GO
CREATE PROCEDURE Add_Suggestion
(
	@filer NVARCHAR(40),
	@content NVARCHAR(max),
	@deptId INT,
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @filer in (SELECT [email] FROM Users)
	BEGIN
		IF @deptid in (SELECT id FROM Department)
		BEGIN
			INSERT INTO Feedback(filer, content, deptId, [type]) VALUES (@filer, @content, @deptId, 'Suggestion')
			SET @OutputMsg = 'Suggestion Added!'
		END
		ELSE
		BEGIN
			SET @OutputMsg = 'Department Does Not Exist!'
		END
	END
	ELSE
	BEGIN
		SET @OutputMsg = 'Filer Does Not Exist!'
	END
END;
GO

--PROCEDURE to add a complaint
GO
CREATE PROCEDURE Add_Complaint
(
	@filer NVARCHAR(40),
	@content NVARCHAR(max),
	@deptId INT,
	@deadline DATE,
	@priority NVARCHAR(10),
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @filer in (SELECT [email] FROM Users)
	BEGIN
		IF @deptid in (SELECT id FROM Department)
		BEGIN
			IF @deadline > GETDATE()
			BEGIN
				IF @priority = 'High' OR @priority = 'Medium' OR @priority = 'Low'
				BEGIN
					INSERT INTO Feedback(filer, content, deptId, [type]) VALUES (@filer, @content, @deptId, 'Complaint')
					INSERT INTO Complaints(id, [deadline], priority) VALUES(@@IDENTITY, @deadline, @priority)
					SET @OutputMsg = 'Complaint Registered!'
				END
				ELSE
				BEGIN
					SET @OutputMsg = 'Wrong Priority Level!'
				END
			END
			ELSE
			BEGIN
				SET @OutputMsg = 'Deadline Date Has Already Passed!'
			END
		END
		ELSE
		BEGIN
			SET @OutputMsg = 'Department Does Not Exist!'
		END
	END
	ELSE
	BEGIN
		SET @OutputMsg = 'Filer Does Not Exist!'
	END
END;
GO

--PROCEDURE to add a comment to a complaint
GO
CREATE PROCEDURE Add_Comment
(
	@complaintId INT,
	@commentorEmail NVARCHAR(40),
	@content NVARCHAR(max),
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @complaintId in (SELECT id FROM Complaints)
	BEGIN
		IF @commentorEmail in (SELECT email FROM Users)
		BEGIN
			INSERT INTO Comments(complaintId, commentorEmail, content, commTimeStamp) VALUES (@complaintId, @commentorEmail, @content, GETDATE())
			SET @OutputMsg = 'Comment Added!'
		END
		ELSE
		BEGIN
			SET @OutputMsg = 'Commentor Does not Exist!'
		END
	END
	ELSE
	BEGIN
		SET @OutputMsg = 'Complaint Does Not Exist!'
	END
END
GO

--PROCEDURE to add an image evidence to complaint
GO
CREATE PROCEDURE Add_Image
(
	@imageData VARBINARY(max),
	@type NVARCHAR(5),
	@feedbackId INT,
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @type = '.jpg' OR @type = '.png' OR @type = '.gif' OR @type = '.jpeg'
	BEGIN
		IF @feedbackId in (SELECT id FROM Feedback)
		BEGIN
			INSERT INTO [Image](imageData, [type], feedbackId) VALUES (@imageData, @type, @feedbackId)
			SET @OutputMsg = 'Image Added!'
		END
		ELSE
		BEGIN
			SET @OutputMsg = 'No Such Feedback Exists!'
		END
	END
	ELSE
	BEGIN
		SET @OutputMsg = 'Invalid Image Type!'
	END
END;
GO

--PROCEDURE to UPDATE deadline of a complaint
GO
CREATE PROCEDURE Set_Deadline
(
	@deadline DATE,
	@comid INT,
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @deadline < GETDATE()
	BEGIN
		SET @OutputMsg = 'Date Already Passed!'
	End
	ELSE
	BEGIN
		DECLARE @DBPriority NVARCHAR(10)
		SELECT @DBPriority = [priority] FROM Complaints WHERE @comid = id
		IF @DBPriority IS NULL
		BEGIN
			SET @OutputMsg = 'Complaint Does Not Exist!'
		END
		ELSE
		BEGIN
			IF @DBPriority = 'Done' OR @DBPriority = 'Rejected'
			BEGIN
				SET @OutputMsg = 'Complaint Already Processed!'
			END
			ELSE
			BEGIN
				UPDATE Complaints SET deadline = @deadline WHERE @comid = id
				SET @OutputMsg = 'Deadline Updated!'
			END
		END
	END
END;
GO

--PROCEDURE to set a User defined priority
GO
CREATE PROCEDURE Set_Priority
(
	@priority NVARCHAR(10),
	@comid INT,
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	DECLARE @DBPriority NVARCHAR(10)
	SELECT @DBPriority = [priority] FROM Complaints WHERE @comid = id
	IF @DBPriority IS NULL
	BEGIN
		SET @OutputMsg = 'Complaint Does Not Exist!'
	END
	ELSE
	BEGIN
		IF @DBPriority = 'Done' OR @DBPriority = 'Rejected'
		BEGIN
			SET @OutputMsg = 'Complaint Already Processed!'
		END
		ELSE
		BEGIN
			IF @priority = 'Low' or @priority = 'Medium' or @priority = 'High'
			BEGIN
				UPDATE Complaints SET [priority] = @priority WHERE @comid = id
				SET @OutputMsg = 'Priority Updated!'
			END
			ELSE
			BEGIN
				SET @OutputMsg = 'Wrong Priority Level!'
			END
		END
	END
END;
GO

--PROCEDURE to mark a complaint as resolved
GO
CREATE PROCEDURE Complaint_Resolved
(
	@cid INT,
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @cid in (SELECT id FROM Complaints)
	BEGIN
		UPDATE Complaints SET [priority] = 'Done' WHERE id = @cid
		SET @OutputMsg = 'Complaint Resolved!'
	END
	ELSE
	BEGIN
		SET @OutputMsg = 'Complaint Does not Exist!'
	END
END;
GO

--PROCEDURE to mark a complaint as rejected
GO
CREATE PROCEDURE Complaint_Rejected
(
	@cid INT,
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @cid in (SELECT id FROM Complaints)
	BEGIN
		UPDATE Complaints SET [priority] = 'Rejected' WHERE id = @cid
		SET @OutputMsg = 'Complaint Rejected!'
	END
	ELSE
	BEGIN
		SET @OutputMsg = 'Complaint Does not Exist!'
	END
END;
GO

--PROCEDURE to Add a Department
GO
CREATE PROCEDURE Add_Department
(
	@Dept_Name NVARCHAR(60),
	@Admin_Email NVARCHAR(40),
	@Admin_Fname NVARCHAR(20),
	@Admin_Lname NVARCHAR(20),
	@Admin_Cnic NVARCHAR(15) = NULL,
	@Admin_Password NVARCHAR(20),
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @Dept_Name IN (SELECT [name] FROM Department)
	BEGIN
		SET @OutputMsg = 'Department Already Exists!'
	END
	ELSE
	BEGIN
		IF @Admin_Email in (SELECT email FROM Users)
		BEGIN
			Set @OutputMsg = 'Email Already Exists!'
		End
		ELSE
		BEGIN
			IF Len(@Admin_Password) < 8
			BEGIN
				Set @OutputMsg = 'Password too Short!'
			End
			ELSE
			BEGIN
				IF @Admin_Email NOT LIKE '%@nu.edu.pk'
				BEGIN
					Set @OutputMsg = 'Wrong Email Format!'
				END
				ELSE
				BEGIN
					IF @Admin_Cnic IS NOT NULL AND @Admin_Cnic NOT LIKE '_____-_______-_'
					BEGIN
						Set @OutputMsg = 'Wrong CNIC Format!'
					END
					ELSE
					BEGIN
						Set @OutputMsg = 'Department Created!'
						Insert Into Users (email,fname,lname,cnic,[type],[password]) Values (@Admin_Email,@Admin_Fname,@Admin_Lname,@Admin_Cnic,'Admin',@Admin_Password);
						INSERT INTO Department ([name], AdminEmail) VALUES (@Dept_Name, @Admin_Email)
					END
				END
			End
		End
	END
END;
GO

--PROCEDURE to delete a department
GO
CREATE PROCEDURE Del_Dept
(
	@Dept_Id INT,
	@OutputMsg NVARCHAR(50) OUTPUT
)
AS
BEGIN
	if @Dept_Id in (SELECT id FROM Department)
	BEGIN
		DELETE FROM Users WHERE email = (SELECT AdminEmail FROM Department WHERE id = @Dept_Id)
		DELETE FROM Feedback WHERE deptId = @Dept_Id
		Set @OutputMsg = 'Department Deleted!'
	END
	ELSE
	BEGIN
		Set @OutputMsg = 'Department Does Not Exist!'
	END
END;
GO

--PROCEDURE to change an admin of a department
GO
CREATE PROCEDURE Change_Admin
(
	@Dept_Id NVARCHAR(60),
	@Admin_Email NVARCHAR(40),
	@Admin_Fname NVARCHAR(20),
	@Admin_Lname NVARCHAR(20),
	@Admin_Cnic NVARCHAR(15) = NULL,
	@Admin_Password NVARCHAR(20),
	@OutputMsg NVARCHAR(50) Output
)
AS
BEGIN
	IF @Dept_Id IN (SELECT id FROM Department)
	BEGIN
		IF @Admin_Email in (SELECT email FROM Users)
		BEGIN
			Set @OutputMsg = 'Email Already Exists!'
		End
		ELSE
		BEGIN
			IF Len(@Admin_Password) < 8
			BEGIN
				Set @OutputMsg = 'Password too Short!'
			End
			ELSE
			BEGIN
				IF @Admin_Email NOT LIKE '%@nu.edu.pk'
				BEGIN
					Set @OutputMsg = 'Wrong Email Format!'
				END
				ELSE
				BEGIN
					IF @Admin_Cnic IS NOT NULL AND @Admin_Cnic NOT LIKE '_____-_______-_'
					BEGIN
						Set @OutputMsg = 'Wrong CNIC Format!'
					END
					ELSE
					BEGIN
						DECLARE @OldEmail NVARCHAR(40)
						SELECT @OldEmail = AdminEmail FROM Department WHERE @Dept_Id = id
						INSERT INTO Users (email,fname,lname,cnic,[type],[password]) VALUES (@Admin_Email,@Admin_Fname,@Admin_Lname,@Admin_Cnic,'Admin',@Admin_Password);
						UPDATE Department SET AdminEmail = @Admin_Email WHERE id = @Dept_Id
						DELETE FROM Users WHERE email = @OldEmail
						Set @OutputMsg = 'Admin Changed!'
					END
				END
			End
		End
	END
	ELSE
	BEGIN
		Set @OutputMsg = 'Department Does not Exist!'
	END
END;
GO

--PROCEDURE to UPDATE All priorities
GO
CREATE PROCEDURE UPDATE_Priority
AS
BEGIN
	DECLARE @currentid INT, @deadline DATE, @priority NVARCHAR(10),@totComplaints INT
	SELECT @currentid = MIN(id) FROM Complaints
	SELECT @totComplaints = MAX(id) FROM Complaints
	WHILE @currentid <= @totComplaints
	BEGIN
		SELECT @deadline = deadline, @priority = [priority] FROM Complaints WHERE id = @currentid
		IF @priority != 'Done' AND @priority != 'Rejected'
		BEGIN
			IF @priority = 'Low' OR @priority = 'Medium'
			BEGIN
				IF DATEDIFF(DAY, GETDATE(), @deadline) >= 4 AND DATEDIFF(DAY, GETDATE(), @deadline) <= 7
				BEGIN
					UPDATE Complaints SET priority = 'Medium' WHERE id = @currentid
				END
				ELSE
				IF DATEDIFF(DAY, GETDATE(), @deadline) < 4
				BEGIN
					UPDATE Complaints SET priority = 'High' WHERE id = @currentid
				END
			END
		END
		SET @currentid = @currentid + 1
	END
END;
GO

--Inserting MainAdmin data
INSERT INTO Users(email, fname, lname, [cnic], [type], [password]) VALUES ('main@nu.edu.pk', 'Main', 'Admin', '12345-6789123-4', 'MainAdmin', 'MainAdmin')

--Creating a schedule so priorities are UPDATEd every day
USE msdb;

EXECUTE dbo.sp_add_job
	@job_name = N'Daily ComplaINT Priority';

EXECUTE sp_add_jobstep
    @job_name = N'Daily ComplaINT Priority',
    @step_name = N'Set priorities of complaINTs',
    @subsystem = N'TSQL',
    @command = N'EXECUTE UPDATE_Priority',
    @retry_attempts = 5,
	@retry_INTerval = 5;
 
EXECUTE sp_add_schedule
    @schedule_name = N'DailyRoutine',
    @freq_type = 4,
    @freq_INTerval = 1,
    @active_start_time = 010000;
 
EXECUTE sp_attach_schedule
   @job_name = N'Daily ComplaINT Priority',
   @schedule_name = N'DailyRoutine';

EXECUTE dbo.sp_add_jobserver
    @job_name = N'Daily ComplaINT Priority';