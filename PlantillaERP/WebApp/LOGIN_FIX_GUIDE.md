# 🔧 LOGIN TROUBLESHOOTING GUIDE

## ✅ Changes Made

1. **Removed duplicate seeders** - Only using `SeedInitialData.Initialize()` now
2. **Added proper database migration** - Migrations are now applied automatically on startup
3. **Removed conflicting seed methods** - AdminSeeder and SeedService calls removed

## 🔑 Login Credentials

After these changes, use these credentials:

```
Username: admin
Password: Admin@123456
```

## 🚀 Steps to Fix Login Issues

### Option 1: Reset Database (Recommended)

1. Run the reset script:
   ```powershell
   cd WebApp
   .\ResetDatabase.ps1
   ```

2. Start your application
   - The database will be recreated with migrations
   - Seed data will be populated automatically

3. Try logging in with: `admin` / `Admin@123456`

### Option 2: Manual Database Reset

1. Open SQL Server Object Explorer in Visual Studio
2. Find `(localdb)\mssqllocaldb`
3. Delete the `PlantillaERP_UserIdentity` database
4. Run the application - it will recreate everything

### Option 3: Using EF Core Tools

Run these commands in Package Manager Console:

```powershell
# Navigate to the Identity project
cd ..\UserRoles.Identity

# Drop and recreate database
Drop-Database -Context AppDbContext -Force
Update-Database -Context AppDbContext
```

Then run the application to seed data.

## 🔍 Verification Steps

After starting the application, check the console output for:

```
🔄 Applying database migrations...
✅ Database migrations applied successfully
✅ Rol 'Admin' creado
✅ Usuario admin creado (usuario: admin, contraseña: Admin@123456)
✅ Datos iniciales creados correctamente
```

## ❌ Common Issues

### Issue: "Invalid login attempt"

**Causes:**
- Old user data with different password in database
- Database not reset after code changes
- Migrations not applied

**Solution:**
- Reset the database using Option 1 or 2 above

### Issue: "Connection string error"

**Check:** `appsettings.json` has:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PlantillaERP_UserIdentity;Trusted_Connection=true;"
```

### Issue: User exists but password doesn't work

**Solution:**
```sql
-- Connect to the database and run:
DELETE FROM AspNetUsers WHERE Email = 'admin@erp.com';
DELETE FROM AspNetUserRoles;
```

Then restart the application to reseed.

## 📝 What Was Fixed

### Before:
- 3 different seeders running (AdminSeeder, SeedInitialData, SeedService)
- Conflicting passwords and usernames
- Database might not have had migrations applied
- `EnsureCreatedAsync()` doesn't apply migrations

### After:
- Single seeder (SeedInitialData)
- Consistent credentials: `admin` / `Admin@123456`
- Migrations properly applied with `MigrateAsync()`
- Clean startup sequence

## 🎯 Test Your Login

1. Start the application
2. Navigate to: `/Account/Login`
3. Enter:
   - Username: `admin`
   - Password: `Admin@123456`
4. Click "Iniciar Sesión"
5. You should be redirected to `/Dashboard`

If you still have issues, check the console output for any red error messages (❌) and share them for further troubleshooting.
