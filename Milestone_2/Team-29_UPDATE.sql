-- numTips
UPDATE business
SET bNum_Tips = 
(
    SELECT COUNT(Tips.tBusinessID)
    FROM Tips
    WHERE business.businessID = Tips.tBusinessID
);


-- numCheckins in Business from data in Checkin
UPDATE business
SET bCheckins = 
(
    SELECT COUNT(Checkin.cBusinessID)
    FROM Checkin
    WHERE business.businessID = Checkin.cBusinessID
);


-- totalLikes in yelpUser from data in Tips
UPDATE yelpUser
SET uTotal_Likes =
(
    SELECT SUM(Tips.tNum_Likes)
    FROM Tips
    WHERE yelpUser.userID = Tips.tUserID
);


-- tipCount in yelpUser from data in Tips
UPDATE yelpUser
SET uTips_Count =
(
    SELECT COUNT(Tips.tUserID)
    FROM Tips
    WHERE yelpUser.userID = Tips.tUserID
);