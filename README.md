# Math App in MVC with SQL DB and FirebaseAuth

The purpose of this repo is to outline the steps needed to build a dotnet app that interacts with a SQL DB for basic read/write functionality.

## Important Note
This repo has been build to assist with understanding various aspects in this course, and I am certain it can be made even better.

**If you notice any errors or need to suggest improvements, please reach out to me!! I will be grateful**

## Basic Features
* User can enter two numbers, select an option, and click calculate. Once calculated, the result is to be shown to the user and written to the SQL DB
* User can review previous calculations stored in the DB (history)
* User can clear previous calculations stored in the DB
* Each user has their own history
* Authentication with logging of errors
* Ensuring that divide by 0 does not happen

## Pre-Requisites
* VS or VS Code with Dotnet 8.0 or 9.0
* MS SQL Server installed
* A browser to run everything

## Steps

It is highly recommended that you follow these steps in order:
1. [Setting up the DB](/Guides/SettingUpDB.md)
1. [Building the DB Context in MVC](/Guides/BuildingDBContext.md)
1. [Building the Calculate Functionality](/Guides/BuildingCalculate.md)
1. [Building the History Functionality](/Guides/BuildingHistory.md)