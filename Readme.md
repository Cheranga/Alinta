# Alinta API

## API Functionalities
- [x] Create Customer
- [x] Update Customer
- [x] Delete Customer
- [x] Search Customer
* The API allows to search by customer name. It will match either the first name or the last name regardless of case sensitivity.

## Documentation

The documentation of the API can be found in,
* https://alintaapi.azurewebsites.net/index.html
* https://alintadockerapi.azurewebsites.net/index.html

## Design and Architecture

In here clean/onion architecture based design is used.

* Project Structure

![alt text](https://github.com/Cheranga/Alinta/blob/master/Images/Dependencies%20Graph.png "Project Structure")

## Tests
You can find the unit tests in the below mentioned projects,

* Alinta.Services.UnitTests

This contains the unit tests which are specific to the services used.

* Alinta.DataAccess.EntityFramework.UnitTests

This contains the unit tests which are specific to the data access layer. As instructed in here it's using entity framework with an in-memory database.

## CI and CD
- [x] The solution is hosted in GitHub.
- [x] Automatic CI/CD pipelines have been set to the `master` branch through `Azure DevOps`

## Deployments

- [x] Azure Web App

The API has been deployed in `Azure Web App` and, can be seen by visiting https://alintaapi.azurewebsites.net/index.html

- [x] Using Docker

A docker image has been created for this API and it's also deployed as an `Azure Web App for Containers`. 
The docker image can be found by visiting think [link]("https://hub.docker.com/r/cheranga/alintaapi")

This can be seen by visiting https://alintadockerapi.azurewebsites.net/index.html

## Notes
* Case insensitive search in EF core answer was found in [here]("https://stackoverflow.com/questions/43277868/entity-framework-core-contains-is-case-sensitive-or-case-insensitive")