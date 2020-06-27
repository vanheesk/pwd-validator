# pwd-validator

A sample password validator service based on the HaveIBeenPwned datadump using a SQLServer or SQLite as datasource for storing the data.

## Included features

* Validate the SHA1 hash of a string against the list of breached passwords
* Notify you how many occurrences have been found of the password

## Planned features

* Track the unsafe passwords requested against the store.
* Allow downloading an overview of (unsafe) requests made

## Technologies

* .NET
* RepoDB (SQLServer, SQLite)
* DbUp
* NPOI
* Microsoft Extensions CommandLineUtils
* Serilog

## State of Project

In development

## Usage

Use -h or --help to know about possible commands and options for running the application

To create an initial (empty) database use the following run configuration:
`setup`

To populate the database based on records read from the HaveIBeenPwned use this: 
`populate`

To run as a REST API service: 
`service`

## Prerequisites

- A datadump file as provided by HaveIBeenPwned website (https://haveibeenpwned.com)
