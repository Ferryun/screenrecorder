Function SetLogFilePerm() 
	Dim installDir   
	installDir = Session.Property("CustomActionData")
	Dim logFile
	logFile = installDir & "application.log"
	Dim objShell
	Set objShell = CreateObject ("WScript.shell")
	objShell.Run "cmd.exe /c echo y|cacls """ + logFile + """ /E /G Users:w", 0, true
	Set objShell = Nothing        
	SetLogFilePerm = 0
End Function