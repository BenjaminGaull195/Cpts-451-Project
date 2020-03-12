
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
    uTotal_Likes INT,
    --Average Stars
    uAvg_Stars FLOAT,
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
    tBusinessID INT NOT NULL,
    tUserID INT NOT NULL,
    --May change primary key later

    tNum_Likes INT,
    tPosted_Datetime DATETIME,
    tText TEXT,
    CONSTRAINT pk_Tips PRIMARY KEY(tUserID, tBusinessID, tPosted_Datetime),
    CONSTRAINT fk_User FOREIGN KEY(tUserID) REFERENCES User(userID),
    CONSTRAINT fk_Business FOREIGN KEY(tBusinessID) REFERENCES Business(businessID)
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
    bNum_Tips INT,
    bCheckins INT,
    bIs_Open INT,
    bNum_Stars FLOAT,
    CONSTRAINT pk_Business PRIMARY KEY(businessID)
);

--Create Friends Relationship
CREATE TABLE Friends(
    userID1 INT NOT NULL,
    userID2 INT NOT NULL,
    CONSTRAINT pk_Friends PRIMARY KEY(userID1, userID2),
    CONSTRAINT fk_Friend1 FOREIGN KEY(userID1) REFERENCES User(userID),
    CONSTRAINT fk_Friend2 FOREIGN KEY(userID2) REFERENCES User(userID), 
);

--Create Makes_Tips Relationship
CREATE TABLE Makes_Tips(
    mUserID INT NOT NULL,
    mBusinessID INT NOT NULL,
    CONSTRAINT pk_Makes_Tips PRIMARY KEY(mUserID, mBusinessID),
    CONSTRAINT fk_mUser FOREIGN KEY(mUserID) REFERENCES User(userID),
    CONSTRAINT fk_mBusiness FOREIGN KEY(mBusinessID) REFERENCES Business(businessID)
);

--Creates Tips_About Relationship
CREATE TABLE Tips_About(
    aUserID INT NOT NULL,
    aBusinessID INT NOT NULL,
    CONSTRAINT pk_Tipes_About PRIMARY KEY(aUserID, aBusinessID),
    CONSTRAINT fk_aUser FOREIGN KEY(aUserID) REFERENCES User(userID),
    CONSTRAINT fk_aBusiness FOREIGN KEY(aBusinessID) REFERENCES Business(businessID)
);

--Creates Checkin Relationship
CREATE TABLE Checkin(
    cBusinessID INT NOT NULL,
    cDate DATE NOT NULL,
    cTime TIME NOT NULL,
    CONSTRAINT pk_Checkin PRIMARY KEY(cBusinessID, cDate, cTime),
    CONSTRAINT fk_Checkin FOREIGN KEY(cBusinessID) REFERENCES Business(businessID)
);

--Create Hours Weak Relation
CREATE TABLE Hours_Open(
    hBusinessID INT NOT NULL,
    hDay VARCHAR(10),
    hHours VARCHAR(20),
    CONSTRAINT pk_Hours PRIMARY KEY(hBusinessID, hDay),
    CONSTRAINT fk_Hours FOREIGN KEY(hBusinessID) REFERENCES Business(businessID)
);




