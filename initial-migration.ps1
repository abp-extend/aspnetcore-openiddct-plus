param(
    [string]$MigrationName = "InitialMigration",
    [string]$OutputDirectory = "./Migrations"
)

Write-Host "Adding migration '$MigrationName' to directory '$OutputDirectory'..." -ForegroundColor Green

dotnet ef migrations add $MigrationName -o $OutputDirectory

if ($?) {
    Write-Host "Migration '$MigrationName' added successfully!" -ForegroundColor Green
} else {
    Write-Host "Failed to add migration '$MigrationName'." -ForegroundColor Red
}
