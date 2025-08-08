# eGranjaCAT

Notes:

Add Migration:
```dotnet ef migrations add {Migration Name} --project eGranjaCAT.Infrastructure --startup-project eGranjaCAT.API --context ApplicationDbContext --output-dir Persistence/Migrations```

Update Database:
```dotnet ef database update --project eGranjaCAT.Infrastructure --startup-project eGranjaCAT.API --context ApplicationDbContext```

