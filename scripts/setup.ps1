# Setup script to bootstrap local.settings.json file.
# Note: For non-local development, secrets and connectionstrings should be in key vault.-
az login
$sub = Read-Host "Enter the name of your azure subscription"
if($null -eq $sub)
{
    throw "You must provide the name of your azure subscription."
}
az account set --subscription $sub

$name = Read-Host "Enter the namespace name of your service bus"
if($null -eq $name)
{
    throw "You must provide the namespace of your service bus."
}
$rg = Read-Host "Enter the resource group where your service bus is located"
if($null -eq $rg)
{
    throw "You must provide a resource group."
}
Write-Host "Creating audit queue"
az servicebus queue create --resource-group $rg --namespace-name $name --name cleanfunc.application.audit.messages.auditmessage
Write-Host "Generate local.settings.json file"

$connectionString=az servicebus namespace authorization-rule keys list --resource-group $rg --namespace-name $name --name RootManageSharedAccessKey --query primaryConnectionString --output tsv

$jsonObject = @{
    IsEncrypted=$false
    Values= @{
        AzureWebJobsStorage = "UseDevelopmentStorage=true"
        ServiceBusConnectionString = $connectionString
        CosmosDBEndpoint = "https://localhost:8081"
        CosmosDBAccountKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="
        UseInMemoryDatabase = $true
    }

$json = $jsonObject | ConvertTo-Json
Set-Content -Path ..\src\FunctionApp\local.settings.json -Value $json
