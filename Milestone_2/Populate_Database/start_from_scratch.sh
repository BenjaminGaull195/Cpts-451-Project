>&2 echo "Clearing Tables"
sh drop_tables.sh

>&2 echo "Creating Databse"
sh create_database.sh

>&2 echo "Populating Database"
sh Populate_Database.sh