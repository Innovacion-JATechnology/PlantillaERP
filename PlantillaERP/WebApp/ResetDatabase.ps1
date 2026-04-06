# Script to reset the database
Write-Host "🗑️  Resetting database..." -ForegroundColor Yellow

# Stop all SQL Server Express instances that might be using the database
Write-Host "Stopping SQL Server instances..." -ForegroundColor Cyan
Stop-Process -Name "sqlservr" -Force -ErrorAction SilentlyContinue

# Delete the localdb database
Write-Host "Deleting LocalDB database..." -ForegroundColor Cyan
sqllocaldb stop mssqllocaldb
sqllocaldb delete mssqllocaldb

# Create new instance
Write-Host "Creating new LocalDB instance..." -ForegroundColor Cyan
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb

Write-Host "✅ Database reset complete!" -ForegroundColor Green
Write-Host "Now run your application to recreate the database with seed data." -ForegroundColor Yellow
Write-Host ""
Write-Host "Login credentials:" -ForegroundColor Cyan
Write-Host "  Username: admin" -ForegroundColor White
Write-Host "  Password: Admin@123456" -ForegroundColor White
