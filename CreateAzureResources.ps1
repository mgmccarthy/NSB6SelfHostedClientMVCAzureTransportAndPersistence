$resourceGroupName = ""
$storageAccountName = ""
$sku = ""
$location = ""

az group create --name $resourceGroupName --location $location
az storage account create --location eastus --name $storageAccountName --resource-group $resourceGroupName --sku $sku
az storage account show-connection-string --name $storageAccountName --resource-group $resourceGroupName