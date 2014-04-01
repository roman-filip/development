$username = "manager"
$password = "manager1"
$secstr = New-Object -TypeName System.Security.SecureString
$password.ToCharArray() | ForEach-Object {$secstr.AppendChar($_)}
$credentials = new-object -typename System.Management.Automation.PSCredential -argumentlist $username, $secstr
 
function CheckEnvironment($environment)
{
    $response = Invoke-WebRequest -uri "https://homer-menu-test.ws.homecredit.net/homer-menu/console/testMenuItems.do?code=$environment" -Credential $credentials

    for ($i=1; $i -le $response.AllElements.Count; $i++)
    {
        if ($response.AllElements[$i].value -ieq $environment)
        {
            Write-Output $environment
            Write-Output "******"
            Write-Output "HTTP Status: " $response.AllElements[$i + 4].outerText
            Write-Output "Login Status:" $response.AllElements[$i + 5].outerText
            Write-Output "Version:     " $response.AllElements[$i + 6].outerText
        }
    }
}

# ************************************** Start **************************************

$environment = $args[0].ToUpper()
CheckEnvironment($environment)
