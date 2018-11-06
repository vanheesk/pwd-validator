# pwd-validator
A password validator service based on the HaveIBeenPwned datadump using an embedded Firebird as datasource.
Important note: this is not meant for production purposes! 

## Included features
* Validate the SHA1 hash of a string against the list of breached passwords
* Notify you how many occurrences have been found of the password

## Planned features
* Track the unsafe passwords requested against the store.
* Allow downloading a report with (unsafe) requests made

## Technologies
* .NET
* Firebird DB
* NPOI
* Microsoft Extensions CommandLineUtils

## State of Project
Working, but too slow to be of use in a real production environment. 
(e.g. with 10 million rows inserted, a lookup takes around 17 seconds to complete)
