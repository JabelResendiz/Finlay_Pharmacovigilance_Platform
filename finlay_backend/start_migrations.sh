dotnet ef migrations add InitialMigration99090 --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet ef database update --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet run --project .\Finlay.PharmaVigilance.API\