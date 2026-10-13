# Auction web app

A simplified auction system built with **ASP.NET Core 8 MVC**, **Entity Framework Core** and **ASP.NET Core Identity**, following a **three-tier architecture** with interfaces and dependency injection between the layers.

Lab 2 in the course *HI1031 Distributed Information Systems* at KTH Royal Institute of Technology.

## Features

### Grade 3
- Register, log in and log out
- Create an auction
- Edit the description of your own auction
- List all active auctions, sorted by end date
- View details of an active auction, with bids sorted highest first
- Place a bid on someone else's active auction (must be higher than the current price)
- List active auctions you have bid on
- List ended auctions you have won

## Architecture

Each layer only calls the layer directly below it, through an interface.

<img src="docs/architecture.png" alt="Architecture" width="852">

## Project structure

```
Auction-webapp/
├── docs/                       Architecture and database diagrams, database setup script
├── AuctionApp/
│   ├── Areas/Identity/Pages/   Layout shim so the Identity UI pages use the app layout
│   ├── Controllers/            AuctionsController, HomeController
│   ├── Core/                   Auction, Bid, AuctionException, AuctionService
│   │   └── Interfaces/         IAuctionService, IAuctionPersistence
│   ├── Data/                   AuctionDb, BidDb, AuctionDbContext, AuctionPersistence, AppIdentityDbContext
│   │   └── Migrations/Identity EF Core migrations for the Identity database
│   ├── Migrations/             EF Core migrations for the auction database
│   ├── Models/Auctions/        View models: AuctionVm, AuctionDetailsVm, CreateAuctionVm, ...
│   ├── Views/                  Auctions (Index, Details, Create, Edit, MyBids, Won), Home, Shared
│   ├── wwwroot/css/site.css    Styling on top of Bootstrap
│   ├── appsettings.json        Connection strings for the two databases (password in user secrets)
│   └── Program.cs              Dependency injection and request pipeline
└── AuctionApp.sln
```

## Database

<img src="docs/schema.png" alt="Database schema" width="852">

Two separate MySQL databases:

- **Auction database** (`auctiondb`): auctions and bids, where each bid has a foreign key to its auction
- **Identity database** (`identitydb`): users and roles, managed by ASP.NET Core Identity

### Setup

1. Run `docs/Databas.sql` as root in MySQL Workbench. It creates both databases and the `auctionapp` user. Replace `<lösenord>` with a password of your choice.
2. Store the connection strings with that password in user secrets, from the `AuctionApp/AuctionApp` folder:
   ```
   dotnet user-secrets set "ConnectionStrings:AuctionDbConnection" "server=localhost;port=3306;database=auctiondb;user=auctionapp;password=<lösenord>;"
   dotnet user-secrets set "ConnectionStrings:IdentityDbConnection" "server=localhost;port=3306;database=identitydb;user=auctionapp;password=<lösenord>;"
   ```
3. Create the tables:
   ```
   dotnet ef database update --context AuctionDbContext
   dotnet ef database update --context AppIdentityDbContext
   ```

## Contributors

- Ibrahim Awad, iawad@kth.se
- Wajeed Abid, wajeeda@kth.se  

## License

This Lab is licensed under the MIT License. See LICENSE for details.

## Course

HI1031 Distributed Information Systems, KTH Royal Institute of Technology, 2026.