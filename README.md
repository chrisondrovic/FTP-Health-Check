FTP Health Check Readme

For monitoring
Date	Time	Message	Source	Event ID	Computer
9/13/2014	19:44:02	[Error] The remote name could not be resolved: '<server>' 	FTP Health Check	<eventID>	<computername>

Source: FTP Health Check
Event ID: 24267
Message: The error detected
This will upload a test file to the Kohl’s FTP then remove it if an error is detected it will email the following groups as well as write the above EventLog Entry

Email Groups:
Support Private Cloud SupportPrivateCloud@inin.com
CaaS NOC Engineering CaasNOCEngineering@inin.com

To send email it uses our own smtp replay’s emails will come from no-replay@inin.com with a subject line of “FTP Health Check <Date>”
