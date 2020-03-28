
--Create User
CREATE TABLE yelpUser(
    userID VARCHAR NOT NULL,
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

--Create Business
CREATE TABLE Business(
    businessID VARCHAR NOT NULL,
    bName VARCHAR(100),
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


--Create Tips
CREATE TABLE Tips(
    --Not certain for Primary key
    tBusinessID VARCHAR NOT NULL,
    tUserID VARCHAR NOT NULL,
    --May change primary key later

    tNum_Likes INT,
    tPosted_Datetime TIMESTAMP,
    tText TEXT,
    CONSTRAINT pk_Tips PRIMARY KEY(tUserID, tBusinessID, tPosted_Datetime),
    CONSTRAINT fk_User FOREIGN KEY(tUserID) REFERENCES yelpUser(userID),
    CONSTRAINT fk_Business FOREIGN KEY(tBusinessID) REFERENCES Business(businessID)
);


--Create Friends Relationship
CREATE TABLE Friends(
    userID1 VARCHAR NOT NULL,
    userID2 VARCHAR NOT NULL,
    CONSTRAINT pk_Friends PRIMARY KEY(userID1, userID2),
    CONSTRAINT fk_Friend1 FOREIGN KEY(userID1) REFERENCES yelpUser(userID),
    CONSTRAINT fk_Friend2 FOREIGN KEY(userID2) REFERENCES yelpUser(userID), 
);

--Create Makes_Tips Relationship
CREATE TABLE Makes_Tips(
    mUserID VARCHAR NOT NULL,
    mBusinessID VARCHAR NOT NULL,
    CONSTRAINT pk_Makes_Tips PRIMARY KEY(mUserID, mBusinessID),
    CONSTRAINT fk_mUser FOREIGN KEY(mUserID) REFERENCES yelpUser(userID),
    CONSTRAINT fk_mBusiness FOREIGN KEY(mBusinessID) REFERENCES Business(businessID)
);

--Creates Tips_About Relationship
CREATE TABLE Tips_About(
    aUserID VARCHAR NOT NULL,
    aBusinessID VARCHAR NOT NULL,
    CONSTRAINT pk_Tipes_About PRIMARY KEY(aUserID, aBusinessID),
    CONSTRAINT fk_aUser FOREIGN KEY(aUserID) REFERENCES yelpUser(userID),
    CONSTRAINT fk_aBusiness FOREIGN KEY(aBusinessID) REFERENCES Business(businessID)
);

--Creates Checkin Relationship
CREATE TABLE Checkin(
    cBusinessID VARCHAR NOT NULL,
    cDate DATE NOT NULL,
    cTime TIME NOT NULL,
    CONSTRAINT pk_Checkin PRIMARY KEY(cBusinessID, cDate, cTime),
    CONSTRAINT fk_Checkin FOREIGN KEY(cBusinessID) REFERENCES Business(businessID)
);

--Create Hours Weak Relation
CREATE TABLE Hours_Open(
    hBusinessID VARCHAR NOT NULL,
    hDay VARCHAR(10),
    hHours VARCHAR(20),
    CONSTRAINT pk_Hours PRIMARY KEY(hBusinessID, hDay),
    CONSTRAINT fk_Hours FOREIGN KEY(hBusinessID) REFERENCES Business(businessID)
);




