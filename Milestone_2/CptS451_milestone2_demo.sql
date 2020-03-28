
--GLOSSARY
--table names
businesstable
usertable
tipstable
friendstable
checkin
businesscategory
businessattribute
businesshours

--some attribute names
zipcode
business_id
city  (business city)
name   (business name)
user_id
friend_id
numtips
numCheckins

user_id
tipcount  (user)
totallikes (user)

tipdate
tiptext
likes  (tip)

checkinyear
checkinmonth
checkinday
checkintime


--1.
SELECT COUNT(*) 
FROM  Business;
SELECT COUNT(*) 
FROM  yelpUser;
SELECT COUNT(*) 
FROM  Tips;
SELECT COUNT(*) 
FROM  Friends;
SELECT COUNT(*) 
FROM  Checkin;
SELECT COUNT(*) 
FROM  businesscategory;
SELECT COUNT(*) 
FROM  Hours_Open;



--2. Run the following queries on your business table, checkin table and review table. Make sure to change the attribute names based on your schema. 

SELECT bPostal_Code, count(businessID) 
FROM Business
GROUP BY bPostal_Code
HAVING count(businessID) > 500
ORDER BY bPostal_Code;

SELECT bPostal_Code, COUNT(distinct C.category)
FROM businesstable as B, businesscategory as C
WHERE B.businessID = C.businessID
GROUP BY bPostal_Code
HAVING count(distinct C.category)>300
ORDER BY bPostal_Code;

SELECT zipcode, COUNT(distinct A.attribute)
FROM businesstable as B, businessattribute as A
WHERE B.business_id = A.business_id
GROUP BY zipcode
HAVING count(distinct A.attribute)>65;


--3. Run the following queries on your business table, checkin table and tips table. Make sure to change the attribute names based on your schema. 

SELECT yelpUser.userID, count(userID1)
FROM yelpUser, Friends
WHERE yelpUser.userID = Friends.userID1 AND 
      yelpUser.userID = 'zvQ7B3KZuFOX7pYLsOxhpA'
GROUP BY yelpUser.userID;


SELECT BusinessID, bname, bcity, bnum_tips, bCheckins 
FROM Business
WHERE businessID ='UvF68aNDfzCWQbxO6-647g' ;

SELECT userID, uname, utip_count, utotal_likes
FROM yelpUser
WHERE userID = 'i3bLA4sEdFk8j3Pq6tx8wQ'

-----------

SELECT COUNT(*) 
FROM checkin
WHERE businessID ='UvF68aNDfzCWQbxO6-647g';

SELECT count(*)
FROM tips
WHERE  businessID = 'UvF68aNDfzCWQbxO6-647g';



--4. 
--Type the following statements. Make sure to change the attribute names based on your schema. 

SELECT COUNT(*) 
FROM checkin
WHERE businessID ='M007_bAIM34x1yd138zhSQ';

SELECT businessID, bname, bcity, bCheckins, bnum_tips
FROM business
WHERE businessID ='M007_bAIM34x1yd138zhSQ';

INSERT INTO checkin (cBusinessID, cDate,cTime)
VALUES ('M007_bAIM34x1yd138zhSQ','2020/03/27','15:00');


--5.
--Type the following statements. Make sure to change the attribute names based on your schema.  

SELECT businessID, bname, bcity, bCheckins, bnum_tips
FROM business 
WHERE businessID ='M007_bAIM34x1yd138zhSQ';

SELECT userID, uname, utips_count, utotallikes
FROM yelpUser
WHERE userID = 'rRrFcSEZOTw6iZagsIwTFQ'


INSERT INTO tips (tBusinessID, tUserID, tNum_Likes, tipdate, tiptext,likes)  
VALUES ('M007_bAIM34x1yd138zhSQ', 'rRrFcSEZOTw6iZagsIwTFQ', 0, '2020-03-27 13:00','EVERYTHING IS AWESOME');

UPDATE tips
SET likes = likes+1
WHERE userID = 'rRrFcSEZOTw6iZagsIwTFQ' AND 
      businessID = 'M007_bAIM34x1yd138zhSQ' AND 
      tPosted_Datetime ='2020-03-27 13:00'
      
