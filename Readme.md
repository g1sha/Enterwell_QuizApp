
### Docker url: http://localhost:5000/swagger/index.html
### IDE Debug url: https://localhost:5001/swagger/index.html

### Accounts for testing
```bash
username:password

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