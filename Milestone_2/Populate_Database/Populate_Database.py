import json
import psycopg2


def cleanStr4SQL(s):
    return s.replace("'", "`").replace("\n", " ")


def int2BoolStr(value):
    if value == 0:
        return  0
    else:
        return 1


def insert2BusinessTable():
    """
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
    """

    # reading the JSON file
    with open('./yelp_business.JSON', 'r') as f:
        outfile = open('./yelp_business.SQL', 'w')
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)

            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            sql_str = ("INSERT INTO Business (businessID, bName, bStreet, bCity, bState, bPostal_Code, bLatitude, bLongitude, bnum_Tips, bCheckins, bIs_Open, bNum_Stars)\n"
                       + "VALUES ('"
                       + cleanStr4SQL(data['business_id'])
                       + "','"
                       + cleanStr4SQL(data["name"])
                       + "','"
                       + cleanStr4SQL(data["address"])
                       + "','"
                       + cleanStr4SQL(data["city"])
                       + "','"
                       + cleanStr4SQL(data["state"])
                       + "',"
                       + str(data["postal_code"])
                       + ","
                       + str(data["latitude"])
                       + ","
                       + str(data["longitude"])
                       + ", 0 , 0 ,"
                       + str(int2BoolStr(data["is_open"]))
                       + ","
                       + str(data["stars"])
                       + ");")

            outfile.write(sql_str)
            outfile.write("\n\n")

            line = f.readline()
            count_line += 1

    outfile.close()
    f.close()
    return count_line


def insert2CheckinTable():
    """
    --Creates Checkin Relationship
    CREATE TABLE Checkin(
    cBusinessID VARCHAR NOT NULL,
    cDate DATE NOT NULL,
    cTime TIME NOT NULL,
    CONSTRAINT pk_Checkin PRIMARY KEY(cBusinessID, cDate, cTime),
    CONSTRAINT fk_Checkin FOREIGN KEY(cBusinessID) REFERENCES Business(businessID)
    );
    """
    with open('./yelp_checkin.JSON', 'r') as f:
        outfile = open('./yelp_checkin.SQL', 'w')
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)

            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            date_data = data["date"].split(",")
            for date in date_data:
                sql_str = ("INSERT INTO Checkin (cBusinessID, cDate, cTime)\n"
                           + "VALUES ('"
                           + cleanStr4SQL(data['business_id'])
                           + "',"
                           + str(date.split(" ")[0].lstrip("'").rstrip("'"))
                           + ","
                           + str(date.split(" ")[1].lstrip("'").rstrip("'"))
                           + ");")

                outfile.write(sql_str)
                outfile.write("\n\n")

            line = f.readline()
            count_line += 1

    outfile.close()
    f.close()
    return count_line


def insert2FriendsTable():
    """
    --Create Friends Relationship
    CREATE TABLE Friends(
    userID1 VARCHAR NOT NULL,
    userID2 VARCHAR NOT NULL,
    CONSTRAINT pk_Friends PRIMARY KEY(userID1, userID2),
    CONSTRAINT fk_Friend1 FOREIGN KEY(userID1) REFERENCES yelpUser(userID),
    CONSTRAINT fk_Friend2 FOREIGN KEY(userID2) REFERENCES yelpUser(userID), 
    );
    """
    with open('./yelp_user.JSON', 'r') as f:
        outfile = open('./yelp_friends.SQL', 'w')
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)

            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            friend_data = data["friends"]
            for friend in friend_data:
                sql_str = ("INSERT INTO Friends (userID1, userID2)\n"
                           + "VALUES ('"
                           + cleanStr4SQL(data['user_id'])
                           + "','"
                           + cleanStr4SQL(friend)
                           + "'"
                           + ");")

                outfile.write(sql_str)
                outfile.write("\n\n")

            line = f.readline()
            count_line += 1

    outfile.close()
    f.close()
    return count_line


def insert2HoursTable():
    """
    --Create Hours Weak Relation
    CREATE TABLE Hours_Open(
    hBusinessID VARCHAR NOT NULL,
    hDay VARCHAR(10),
    hHours VARCHAR(20),
    CONSTRAINT pk_Hours PRIMARY KEY(hBusinessID, hDay),
    CONSTRAINT fk_Hours FOREIGN KEY(hBusinessID) REFERENCES Business(businessID)
    );
    """
    with open('./yelp_business.JSON', 'r') as f:
        outfile = open('./yelp_hours.SQL', 'w')
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)

            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            hours_data = data["hours"]
            for hour in hours_data.keys():
                sql_str = ("INSERT INTO Hours_Open (hBusinessID, hDay, hHours)\n"
                           + "VALUES ('"
                           + cleanStr4SQL(data['business_id'])
                           + "','"
                           + cleanStr4SQL(hour)
                           + "','"
                           + cleanStr4SQL(hours_data[hour])
                           + "'"
                           + ");")

                outfile.write(sql_str)
                outfile.write("\n\n")

            line = f.readline()
            count_line += 1

    outfile.close()
    f.close()
    return count_line


def insert2Makes_TipsTable():
    """
    --Create Makes_Tips Relationship
    CREATE TABLE Makes_Tips(
    mUserID VARCHAR NOT NULL,
    mBusinessID VARCHAR NOT NULL,
    CONSTRAINT pk_Makes_Tips PRIMARY KEY(mUserID, mBusinessID),
    CONSTRAINT fk_mUser FOREIGN KEY(mUserID) REFERENCES yelpUser(userID),
    CONSTRAINT fk_mBusiness FOREIGN KEY(mBusinessID) REFERENCES Business(businessID)
    );

    """
    with open('./yelp_tip.JSON', 'r') as f:
        outfile = open('./yelp_makes_tips.SQL', 'w')
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)

            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            sql_str = ("INSERT INTO Hours_Open (mUserID, mBusinessID)\n"
                       + "VALUES ('"
                       + cleanStr4SQL(data["user_id"])
                       + "','"
                       + cleanStr4SQL(data["business_id"])
                       + "'"
                       + ");")

            outfile.write(sql_str)
            outfile.write("\n\n")

            line = f.readline()
            count_line += 1

    outfile.close()
    f.close()
    return count_line


def insert2TipTable():
    """
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
    """
    with open('./yelp_tip.JSON', 'r') as f:
        outfile = open('./yelp_tip.SQL', 'w')
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)

            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            sql_str = ("INSERT INTO Tips (tBusinessID, tUserID, tNum_Likes, tPosted_Date, tPosted_Time, tText)\n"
                       + "VALUES ('"
                       + cleanStr4SQL(data['business_id'])
                       + "','"
                       + cleanStr4SQL(data["user_id"])
                       + "', 0,"
                       + data["date"].split(" ")[0].lstrip("'").rstrip("'")
                       + ","
                       + data["date"].split(" ")[1].lstrip("'").rstrip("'")
                       + ",'"
                       + cleanStr4SQL(data["text"])
                       + "'"
                       + ");")

            outfile.write(sql_str)
            outfile.write("\n\n")

            line = f.readline()
            count_line += 1

    outfile.close()
    f.close()
    return count_line


def insert2UserTable():
    """
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
    """
    with open('./yelp_user.JSON', 'r') as f:
        outfile = open('./yelp_user.SQL', 'w')
        line = f.readline()
        count_line = 0

        while line:
            data = json.loads(line)

            # Generate the INSERT statement for the cussent business
            # include values for all businessTable attributes
            sql_str = ("INSERT INTO yelpUser (UserID, uName, uNum_Fans, uNum_Funny, uNum_Cool, uNum_Useful, uTips_Count, uTotal_Likes, uAvg_Stars, uLatitude, uLongitude, uDate_Joined, uTime_Joined)\n"
                       + "VALUES ('"
                       + cleanStr4SQL(data["user_id"])
                       + "','"
                       + cleanStr4SQL(data["name"])
                       + "',"
                       + str(data["fans"])
                       + ","
                       + str(data["funny"])
                       + ","
                       + str(data["cool"])
                       + ","
                       + str(data["useful"])
                       + ","
                       + str(data["tipcount"])
                       + ","
                       + str("0, ")
                       + ","
                       + str(data["average_stars"])
                       + ","
                       + str(0.0)
                       + ","
                       + str(0.0)
                       + ","
                       + cleanStr4SQL(data["yelping_since"].split(" ")[0])
                       + ","
                       + cleanStr4SQL(data["yelping_since"].split(" ")[1])
                       + ");")

            outfile.write(sql_str)
            outfile.write("\n\n")

            line = f.readline()
            count_line += 1

    outfile.close()
    f.close()
    return count_line


def main():

    lines_in_tables = [19983, 17978, 189298, 19983, 287288, 287288, 189298]
    table_names = ["business", "checkin",
                   "friends", "hours", "makes_tips", "tip", "user"]
    read_in_tables = [insert2BusinessTable(), insert2CheckinTable(
    ), insert2FriendsTable(), insert2HoursTable(), insert2Makes_TipsTable(), insert2TipTable(), insert2UserTable()]

    for x in range(len(table_names)):
        if lines_in_tables[x] != read_in_tables[x]:
            print("Did not read in all lines in " + table_names[x] + "!")


if __name__ == "__main__":
    main()
