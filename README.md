# pwd-validator-firebird

A sample password validator service based on the HaveIBeenPwned datadump using an embedded Firebird as datasource.

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

## Features to be added

* Entity Framework, Dabber, DbUp
* Serilog

## Usage

Use -h or --help to know about possible commands and options for running the application

To create an initial (empty) database use the following run configuration:
`setup`

To populate the database based on records read from the HaveIBeenPwned use this: 
`populate`

To run as a REST API service: 
`service`

## Developer Setup

1. Download the latest version of Firebird Embedded SQL from: https://www.firebirdsql.org/manual/ufb-cs-embedded.html.
2. Unzip and store the files in the Firebird folder found within the PwdValidator.Service/Services folder
3. Make sure the contents of the folder are added to the .gitignore file (this should already have been the case)
