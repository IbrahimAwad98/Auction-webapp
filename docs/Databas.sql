-- Kör som root i MySQL Workbench. Byt ut <lösenord> och lägg samma i user-secrets:
--   dotnet user-secrets set "ConnectionStrings:AuctionDbConnection"  "server=localhost;port=3306;database=auctiondb;user=auctionapp;password=<lösenord>;"
--   dotnet user-secrets set "ConnectionStrings:IdentityDbConnection" "server=localhost;port=3306;database=identitydb;user=auctionapp;password=<lösenord>;"
CREATE DATABASE IF NOT EXISTS auctiondb;
CREATE DATABASE IF NOT EXISTS identitydb;
CREATE USER IF NOT EXISTS 'auctionapp'@'localhost';
ALTER USER 'auctionapp'@'localhost' IDENTIFIED BY 'ibrahim123';
GRANT ALL PRIVILEGES ON auctiondb.* TO 'auctionapp'@'localhost';
GRANT ALL PRIVILEGES ON identitydb.* TO 'auctionapp'@'localhost';
FLUSH PRIVILEGES;