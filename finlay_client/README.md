# Finlay PharmaVigilance Client

Proyecto de consola .NET que consume la API de Finlay PharmaVigilance.

## Uso

1. Iniciar la API:
   ```bash
   cd finlay_backend
   dotnet run --project ./Finlay.PharmaVigilance.API/
   ```
2. Iniciar el cliente:
   ```bash
   cd ../finlay_client
   dotnet run --project Finlay.PharmaVigilance.Client.csproj
   ```
3. El cliente ofrece un menú para:
   - registrar administradores
   - iniciar sesión
   - registrar vacunas
   - registrar síntomas
   - poblar la base de datos con datos de prueba

## Seed de prueba

Ejecuta un solo comando para poblar la base de datos con un administrador, vacunas y síntomas de ejemplo:

```bash
cd ../finlay_client
dotnet run --project Finlay.PharmaVigilance.Client.csproj -- seed
```

Si tu API corre en otra URL, añade la base URL después de `seed`:

```bash
dotnet run --project Finlay.PharmaVigilance.Client.csproj -- seed http://localhost:5137/api
```

## Configuración

El cliente asume `http://localhost:5137/api` por defecto. Puede pasar otra URL base como primer argumento:

```bash
dotnet run --project Finlay.PharmaVigilance.Client.csproj -- "http://localhost:5137/api"
```
