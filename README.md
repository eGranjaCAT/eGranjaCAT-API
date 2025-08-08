# eGranjaCAT
## About

The eGranjaCAT API is developed with .NET 8 and C#, providing an integrated solution to modernize pig farm management in Catalonia. It uses a single-tenant-per-database architecture to ensure data isolation and client-specific customization. 

This RESTful API integrates official systems such as Gestió Telemàtica Ramadera (GTR), Spain’s Ministerio de Agricultura, Pesca y Alimentación (MAPA), the PresVet antibiotic prescription surveillance system, and offers native support for generating and digitally signing veterinary electronic prescriptions without relying on third-party software (some features still in progress). 
Additional capabilities include secure JWT authentication with role- and policy-based access control, scheduled background jobs, SMTP email support, and PDF/XLSX document generation, and more. All aimed at simplifying and automating daily farm operations.


## License
**© 2025 Felix Montragull Kruse.All rights reserved.**

This project and its source code are the exclusive property of Felix Montragull Kruse. No part of this software may be copied, modified, distributed, or used without explicit permission.
 

 

### Notes:

Add Migration:
```dotnet ef migrations add {Migration Name} --project eGranjaCAT.Infrastructure --startup-project eGranjaCAT.API --context ApplicationDbContext --output-dir Persistence/Migrations```

Update Database:
```dotnet ef database update --project eGranjaCAT.Infrastructure --startup-project eGranjaCAT.API --context ApplicationDbContext```
