# Enterwell Quiz App - [PDF zadatka](dotnet-zadatak.pdf)

## Pregled projekta

Aplikacija razvijena u .NET 8 koristi Clean Architecture pristup s jasnom separacijom odgovornosti između slojeva. Aplikacija omogućuje upravljanje kvizovima i pitanjima putem REST API-ja s JWT autentifikacijom.

---

## Struktura rješenja

### 1. Core

Sloj koji sadrži poslovnu logiku i ne ovisi o vanjskim bibliotekama.

**Sadržaj:**
- **Entities/** - Domain modeli (`Quiz`, `Question`, `QuizQuestion`, `User`)
- **DTOs/** - Data Transfer Objects za komunikaciju između slojeva
- **Interfaces/** - Interfejsi servisa (`IQuizService`, `IQuestionService`)
- **Constants/** - Hardcode validacijske i error poruke (`ValidationMessages`)

Bazna klasa `BaseEntity` implementira audit polja (`CreatedAt`, `UpdatedAt`, `CreatedBy`, `UpdatedBy`) koja nasljeđuju sve entity klase.

---

### 2. Infrastructure

Sloj za pristup podacima i eksterne servise.

**Sadržaj:**
- **Data/** - Entity Framework konfiguracija
  - `QuizContext` - DbContext koji nasljeđuje `IdentityDbContext<User>` za integraciju ASP.NET Identity
  - `InitialSeed` - Početni podaci za bazu (korisnici, pitanja, kvizovi)
- **Services/** - Implementacije interfejsa iz Core sloja
  - `QuizService` - CRUD operacije nad kvizovima
  - `QuestionService` - CRUD operacije nad pitanjima
  - `AuthService` - Registracija, prijava, JWT generiranje, refresh token
  - `ExportPluginLoader` - MEF plugin za export fajlova
- **Extensions/** - `PaginationExtensions` za paginaciju IQueryable objekata
- **Migrations/** - EF Core migracije

---

### 3. API

Prezentacijski sloj - ASP.NET Core Web API.

**Sadržaj:**
- **Controllers/**
  - `QuizzesController` - Endpointi za kvizove (CRUD, dodavanje pitanja, export)
  - `QuestionsController` - Endpointi za pitanja (CRUD s filtriranjem)
  - `AuthController` - Autentifikacija (register, login, refresh token)
- **Extensions/** - Konfiguracije
  - `SwaggerConfig` - Swagger postavke
  - `IdentityConfig` - ASP.NET Identity i JWT Bearer konfiguracija
- `Program.cs` - Dependency injection, middleware pipeline, seed podataka

---

### 4. ExporterPlugins

Zasebna biblioteka za export funkcionalnost korištenjem MEF (Managed Extensibility Framework).

**Sadržaj:**
- **Contracts/** - Interfejsi i atributi za plugin sistem
  - `IExportPlugin` - Interfejs koji implementiraju svi exporteri
  - `ExportPluginAttribute` - MEF atribut za auto-discovery
- `CsvExportPlugin` - CSV export implementacija

Plugin sistem omogućuje dodavanje novih export formata bez izmjene postojećeg koda.

---

### 5. QuizApp.Tests

xUnit testovi za poslovnu logiku. Koristi InMemory bazu i Moq biblioteku za mock-anje dependancija.

---

## Arhitektura

Korištena je **Clean Architecture** s tri sloja:

```
API (Prezentacijski sloj) -> Infrastructure (Data Access) -> Core (Poslovna logika)
```

**Prednosti:**
- Neovisnost Core sloja od framework-a i baze
- Zamjenjivost implementacija bez utjecaja na poslovnu logiku

---

## Baza podataka

SQL Server s Entity Framework Core.

---

## Autentifikacija

JWT Bearer token s refresh token mehanizmom.

---

### Docker url: http://localhost:5000/swagger/index.html
### IDE Debug url: https://localhost:5001/swagger/index.html

## Setup

### URL-ovi

| Okruženje | URL |
|-----------|-----|
| Docker | http://localhost:5000/swagger/index.html |
| IDE Debug | https://localhost:5001/swagger/index.html |

### Testni korisnici

```
rejd@enterwell.net:Password123!
igor@enterwell.net:Password123!
```

### Connection string za SSMS
```bash
Server=localhost,1433
Server=sqlserver;Database=QuizAppDb;User Id=sa;Password=@Passw0rd1;TrustServerCertificate=True;
```

### Pokretanje cijelog rješenja sa bazom (API + SQL Server)
```bash
docker-compose up -d
```

### Pokretanje samo SQL Servera
```bash
docker-compose up -d sqlserver
```

### Pokretanje samo API-ja
```bash
docker-compose up -d api
```

### Brisanje volume-a - fresh start
```bash
docker-compose down -v
```

### Reset svega
```bash
docker-compose down -v
docker-compose up -d --build
```
