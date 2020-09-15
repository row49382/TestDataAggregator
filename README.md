## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Config](#Config)
* [Setup](#setup)

## General info
This application aggregates properties from the TestCase, TestResult and WorkItems Azure Devops Analytics apis into a csv to maintain better testing traceability of your Azure DevOps project.

## Technologies
Project is created with .NET Core version: 3.1

## Config
The application contains a Config.json file to control project-specific and report customization variables. The file looks like this:
```json
{
  "organization": "",
  "project": "",
  "personalAccessToken": "",
  "outputDirectory": "",
  "builderOptions": {
    "FilterNotApplicableTestResults": true,
    "FilterClosedTestCases": true,
    "KeepMostRecentTestResults": true,
    "AddTestCasesWithoutTestResults": true
  },
  "odataQueries": {
    "TestResults": "",
    "WorkItems": "",
    "TestCases": "",
    "WorkItemRevisions": ""
  }
}
```
Below is the description of each item:
  * The following properties are required to run the application:
    * organization: The Azure DevOps Organization
    * project: The Azure DevOps Project
    * personalAccessToken: The token (PAT) needed to authorize your requests using Basic auth for the api calls. Follow the steps here to generate your own PAT:
  https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=preview-page.
  Make sure you are authorized to Query the Analytics api.
  * The following properties are not required to run the application:
    * outputDirectory: The output directory the report will be written to. Defaults to the users Desktop location
    * builderOptions: The builder options that can optionally be ommitted when generating the report. If the fields are absent from the config, the value is defaulted to true
    * odataQueries: The odataQueries which can be applied to the apis used to refine your data documented in the report
	
## Setup

After The required properties are set in the Config.json file, navigate to csproj folder and execute `dotnet run`

```
$ cd TestDataAggregator
$ dotnet run
```

Navigate to your desktop location (or outputDirectory location set in Config.json) to find the file. The naming is setup in this format: 

`"{organization}{project}TestReport_{DateTime.Now:yyyy-dd-M--HH-mm-ss}.csv"`.
