param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

# Set the command to execute
$command = "dotnet ef migrations add $MigrationName --startup-project ..\GoalManager.Web\ --context AppDbContext --output-dir Data\Migrations"

# Execute the command
Invoke-Expression $command