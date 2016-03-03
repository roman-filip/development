cd c:\Hudson_Support\SupermodulCompatibilityBreak

powershell .\MakeSuperModuleMagic.ps1 -Branch %1 -Release %2 -WorkingDir %3
exit %errorlevel%