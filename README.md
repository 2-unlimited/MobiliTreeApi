# Introducing MobiliTree

MobiliTree is a new startup in the parking industry that wants to provide a consistent framework of organizing a parking business. 
Their goal is helping parking managers and landlords to monitor their cash flow and manage their parking sessions.

You are part of their development team and your current goal is to produce an API which will be capable of interacting with both the hardware in the parking facilities and the customers.

## Data
For now, the API and the tests use the same fake data put in place for the alpha testing team (4 customers and two parking facilities).

### Customers
- John - Id: "c001", has a contract for parking facility "pf001"
- Sarah - Id: "c002", has a contract for parking facilities "pf001" and "pf002"
- Andrea - Id: "c003", has a contract for parking facility "pf002"
- Peter - Id: "c004", is a known customer but does not have any active contracts

### Parking facilities
The two parking facilities ("pf001", "pf002") have similar pricing setups. 

A TimeslotPrice object represents the price per hour of parking in a specific time interval during the day.
```javascript
{
	StartHour: 3,		// starting from 03:00 AM 
	EndHour: 13,		// until 01:00 PM
	PricePerHour: 1.5	// the price is 1.5 EUR/hour
}
```

Both customers that have a contract with a specific parking facility and those who don't are allowed to park there. Of course, the ones with a contract will get preferential rates.

The ServiceProfile object describes the price breakdown per week days vs weekend days and per customers with a contract vs customers without a contract.

```javascript
{
	WeekDaysPrices: [],		// list of TimeslotPrices applied for customers with active contracts during week days
	WeekendPrices: [],		// list of TimeslotPrices applied for customers with active contracts during weekend days	
	OverrunWeekDaysPrices: [],	// list of TimeslotPrices applied for customers without active contracts during week days	
	OverrunWeekendPrices: [],	// list of TimeslotPrices applied for customers without active contracts during week days
}
```
### Sessions
A Session object represents a parking session of a specific customer in a specific parking facility. Based on all his/her sessions, the invoice amount will be calculated for each customer.

```javascript
{
    ParkingFacilityId: <ParkingFacilityId>,
    CustomerId: <CustomerId>,
    StartDateTime: <StartDateTime>,
    EndDateTime: <EndDateTime>
}
```

## API

Below is a list of API endpoints with their respective input and output.

```
POST
/sessions
- adds a session

GET
/sessions/{parkingFacilityId}
- gets a list of sessions corresponding to the parking facility

GET
/invoices/{parkingFacilityId}
- gets a list of invoices (one for each customer with parking sessions) corresponding to the parking facility

```

## Requirements

 - .NET9

## Compatible IDEs

Tested on:
- Visual Studio Professional 2022 on Windows (17.5.0)

## Build

```console
$ dotnet build
```

## Test

```console
$ dotnet test MobiliTreeApi.Tests
```

## Run

```console
$ dotnet run --project MobiliTreeApi
```
