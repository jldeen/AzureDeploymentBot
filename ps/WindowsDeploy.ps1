$Location="WestUs"
$RGName="TestDeploy"
$DepName="TestDeploy"

$TempFile="https://raw.githubusercontent.com/Azure/azure-quickstart-templates/master/101-vm-simple-windows/azuredeploy.json"

#Add-AzureRmAccount

New-AzureRmResourceGroup $RGName $Location

New-AzureRmResourceGroupDeployment -Name $DepName -ResourceGroupName $RGName -TemplateUri $TempFile 