
>&2 echo "Populating Business Tables"
sudo -u postgres psql -d milestone2 -bq -f yelp_business.SQL > /dev/null

>&2 echo "Populating User Tables"
sudo -u postgres psql -d milestone2 -bq -f yelp_user.SQL > /dev/null

>&2 echo "Populating Checkin Tables"
sudo -u postgres psql -d milestone2 -bq -f yelp_checkin.SQL > /dev/null

>&2 echo "Populating Friend Tables"
sudo -u postgres psql -d milestone2 -bq -f yelp_friends.SQL > /dev/null

>&2 echo "Populating Hour Tables"
sudo -u postgres psql -d milestone2 -bq -f yelp_hours.SQL > /dev/null

>&2 echo "Populating Makes Tips Tables"
sudo -u postgres psql -d milestone2 -bq -f yelp_makes_tips.SQL > /dev/null

>&2 echo "Populating Tip Tables"
sudo -u postgres psql -d milestone2 -bq -f yelp_tip.SQL  > /dev/null