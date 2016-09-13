#Run the following to sign into your Azure account
Add-AzureRmAccount

#Positional Parameters
Param(
  [string]$Location,
  [string]$Resource,
  [string]$Name
  )

<# $Location="WestUs"
$RGName="TestDeploy3"
$DepName="TestDeploy3" #>

$TempFile="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/101-vm-simple-windows/azuredeploy.json"

New-AzureRmResourceGroup $Resource $Location

New-AzureRmResourceGroupDeployment -Name $Name -ResourceGroupName $Resource -TemplateUri $TempFile 

