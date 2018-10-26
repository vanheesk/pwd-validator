# pwd-validator
A password validator service based on the HaveIBeenPwned datadump.

Features that should eventually be included:
* Validate the SHA1 hash of a string against the list of breached passwords
* Notify you how many occurrences have been found of the password
* Track the unsafe passwords requested against the store.
* Allow downloading a report with (unsafe) requests made

## Technologies
* .NET
* Firebird DB
* NPOI
* Microsoft Extensions CommandLineUtils

## State of Project
Currently, nothing more then a POC.
