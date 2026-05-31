# Aboriginal Art Gallery API

SIT331 5.2HD

## What this is

A backend REST API for the Aboriginal Art Gallery of Australia. Built with ASP.NET Core and MongoDB. I am responsible for the Artists, Artworks, Tribes, and ArtTypes bounded contexts.

## Note on group work

This project was originally scoped as a two-person group submission. My teammate withdrew partway through development. I completed all four bounded contexts independently. The Git commit history reflects a gap during that period, followed by a focused solo session to complete Tribes and ArtTypes.

## Prerequisites

- .NET 10 SDK
- MongoDB running locally

## How to run

1. Start MongoDB locally
2. Run the API:


dotnet run --project AboriginalArtGallery.API


3. Open Swagger UI at: http://localhost:{port}/swagger

The database and collections are created automatically on first run. No migrations needed.

## How to run the tests


dotnet test


All 33 tests should pass.

## Project structure

- AboriginalArtGallery.Domain - entities and value objects (Artist, Artwork, Tribe, ArtType, ArtistName, Dimensions, etc.)
- AboriginalArtGallery.Application - service layer and repository interfaces
- AboriginalArtGallery.Infrastructure - MongoDB repositories and index setup
- AboriginalArtGallery.API - controllers, Swagger, global exception middleware
- AboriginalArtGallery.Tests - xUnit tests (value object tests + service layer tests with Moq)

## API endpoints

Artists
- GET /api/artists - paginated list, supports ?search=, ?tribeId=, ?isVerified=
- GET /api/artists/{id}
- GET /api/artists/{id}/artworks
- GET /api/artists/{id}/summary - artwork count, year range, media used
- POST /api/artists
- PUT /api/artists/{id}
- DELETE /api/artists/{id} - soft delete

Artworks
- GET /api/artworks - paginated list, supports ?artistId=, ?onDisplay=, ?medium=, ?yearFrom=, ?yearTo=
- GET /api/artworks/{id}
- POST /api/artworks
- PUT /api/artworks/{id}
- DELETE /api/artworks/{id} - soft delete
- PATCH /api/artworks/{id}/display-status - toggle is_on_display

Tribes (reference data)
- GET /api/tribes
- GET /api/tribes/{id}
- POST /api/tribes
- PUT /api/tribes/{id}
- DELETE /api/tribes/{id}

Art Types (reference data)
- GET /api/art-types - supports ?category= filter
- GET /api/art-types/{id}
- POST /api/art-types
- PUT /api/art-types/{id}
- DELETE /api/art-types/{id}

Other
- GET /api/health

## Key design decisions

- All deletes are soft deletes - records have is_active=false, nothing is removed from the database
- Artist.TribeId is a Guid FK, not a navigation property, because Tribe is a separate bounded context
- Value objects (ArtistName, Dimensions, etc.) are C# records that throw DomainException on invalid input
- MongoDB indexes are created on startup in MongoDbInitializer, no EF migrations
- All PKs are Guid, never int

## MongoDB connection

Default connection string: mongodb://localhost:27017
Database name: aboriginal_art_gallery

These are set in AboriginalArtGallery.API/appsettings.json.

