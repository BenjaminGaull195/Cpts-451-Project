
--Create User
CREATE TABLE User(
    userID INT NOT NULL,
    uName VARCHAR(32),
    uNum_Fans INT,
    --Num Votes
    uNum_Funny INT,
    uNum_Cool INT,
    uNum_Useful INT,
    uTips_Count INT,
    uTotal_Tip_Likes INT,
    --Average Stars
    uTotal_Stars INT,
    uTotal_Ratings INT,
    --Loacation
    uLatitude FLOAT,
    uLongitude FLOAT,
    --Date Joined
    uDate_Joined DATE,
    uTime_Joined TIME,
    CONSTRAINT pk_User PRIMARY KEY(userID)
);

--Create Tips
CREATE TABLE Tips(
    --Not certain for Primary key

    tNum_Likes INT,
    tPosted_Date DATE,
    tPosted_Time TIME,
    tText TEXT,
    CONSTRAINT pk_Tips PRIMARY KEY()
);

--Create Business
CREATE TABLE Business(
    businessID INT NOT NULL,
    bName VARCHAR(32),
    --Address
    bStreet VARCHAR(40),
    bCity VARCHAR(20),
    bState CHAR(2),
    bPostal_Code INT,
    --Location
    bLatitude FLOAT,
    bLongitude FLOAT,
    CONSTRAINT pk_Business PRIMARY KEY(businessID)
);

--Create Friends Relationship
CREATE TABLE Friends(

);

--Create Makes_Tips Relationship
CREATE TABLE Makes_Tips(

);

--Creates Tips_About Relationship
CREATE TABLE Tips_About(

);

--Creates Checkin Relationship
CREATE TABLE Checkin(

);

--Create Hours Weak Relation
CREATE TABLE Hours_Open(

);


