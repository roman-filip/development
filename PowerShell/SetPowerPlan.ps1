#Sets the machine powerplan to one of three settings:
# High performance
# Balanced
# Power saver

PARAM
(
    [Parameter(Mandatory=$true)][ValidateSet("High performance", "Balanced", "Power saver")]
    [string]$PreferredPlan
)

$filter = "ElementName = '$PreferredPlan'"
$powerPlan = gwmi -NS root\cimv2\power -Class win32_PowerPlan -Filter $filter
$powerPlan.Activate()
