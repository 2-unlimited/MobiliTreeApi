(Invoke-RestMethod -Method Post -Uri http://localhost:5400/sessions -Body (ConvertTo-Json @{
	ParkingFacilityId = "pf001"; 
	CustomerId = "c001"; 
	StartDateTime="2018-12-15T12:25:00";
	EndDateTime="2018-12-15T14:05:00"
}) -ContentType "application/json")