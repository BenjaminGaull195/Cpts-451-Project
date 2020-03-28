

-- update numTips and tipCount when a tip is added
CREATE OR REPLACE FUNCTION updateNumTips() RETURNS trigger AS '
BEGIN 
    UPDATE business
    SET bNum_Tips = (bNum_Tips + 1)
    WHERE NEW.tBusinessID = business.businessID;
    RETURN NEW;
END
' LANGUAGE plpgsql; 

CREATE OR REPLACE FUNCTION updateTipCount() RETURNS trigger AS '
BEGIN 
    UPDATE yelpUser
    SET uTips_Count = (uTips_Count + 1)
    WHERE NEW.tUserID = yelpUser.UserID;
    RETURN NEW;
END
' LANGUAGE plpgsql; 


CREATE TRIGGER numTipsTrigger 
AFTER INSERT ON Tips
EXECUTE PROCEDURE updateNumTips();

CREATE TRIGGER TipCountTrigger 
AFTER INSERT ON Tips
EXECUTE PROCEDURE updateTipCount();

-- Test
SELECT * FROM business WHERE business.businessID = 'voZnDQs6Hs3YpNcS-9TALg';
SELECT * FROM yelpuser WHERE yelpUser.UserID = 'jRyO2V1pA4CdVVqCIOPc1Q';

INSERT INTO Tips (tBusinessID, tUserID, tNum_Likes, tPosted_Datetime, tText)
VALUES ('voZnDQs6Hs3YpNcS-9TALg','jRyO2V1pA4CdVVqCIOPc1Q', 0,'2011-12-26 01:46:17', 'TEST');

SELECT * FROM business WHERE business.businessID = 'voZnDQs6Hs3YpNcS-9TALg';
SELECT * FROM yelpuser WHERE yelpUser.UserID = 'jRyO2V1pA4CdVVqCIOPc1Q';

-- clean
DROP TRIGGER numTipsTrigger ON Tips;
DROP TRIGGER TipCountTrigger ON Tips;
DROP FUNCTION updateNumTips();
DROP FUNCTION updateTipCount();
DELETE FROM Tips WHERE Tips.tbusinessID = 'voZnDQs6Hs3YpNcS-9TALg' AND Tips.tUserID = 'jRyO2V1pA4CdVVqCIOPc1Q';