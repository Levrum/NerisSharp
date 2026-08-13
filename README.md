# NerisSharp

A .NET client library for the [NERIS](https://neris.fsri.org/) API (National Emergency Response
Information System), the FSRI-hosted data platform for fire and EMS reporting.

The library wraps the REST endpoints at `api.neris.fsri.org` in typed models and async methods, so a
consuming application can read and write Entities, Incidents, Stations, and Units without hand-rolling
HTTP requests, OAuth token refresh, or the snake_case JSON conventions the API expects.

`NerisSharp` targets `netstandard2.1`, so it can be referenced from .NET Framework 4.8, .NET Core 3.x,
and modern .NET.

## Installation

A Nuget package is available.

Install through your IDE's Nuget package manager by searching for Levrum.NerisSharp or from command line using:
```bash
dotnet add package Levrum.NerisSharp
```

```powershell
Install-Package Levrum.NerisSharp
```

To use the source code, clone the repository and add a project
> reference to `NerisSharp/NerisSharp.csproj`, or build the DLL and reference it directly.

## Quick start

```csharp
using NerisSharp;
using NerisSharp.Models;

// HttpClient is owned by the caller and should be long-lived, not created per call.
var httpClient = new HttpClient();

//Create a config object with your credentials and pass to NerisBase. No explicit login step.
var config = Config.CreateClientCredentialConfig(clientId, clientSecret, UrlType.Test);
var neris = new NerisBase(config, httpClient);

var request = new IncidentRequestModel
{
    Neris_Id_Entity = "FD01234567",
    Call_Create_Start = DateTimeOffset.UtcNow.AddDays(-7),
    Call_Create_End = DateTimeOffset.UtcNow,
};

List<IncidentModel> incidents = await neris.GetAllIncidents(request);
```
## Repository layout

| Path | Purpose |
| --- | --- |
| `NerisSharp/` | The class library that models NERIS data and allows for interaction with the NERIS API |
| `NerisSharpTest/` | Test Suite with Xunit. |
| `TestProject/` | Console harness for manual testing against the live or test API. |

Inside `NerisSharp/`:

- `NerisBase.cs` - constructor, authentication, route building, shared `_call` helper, error
  handling, and logging.
- `QueryMethods/` - `EntityMethods.cs`, `IncidentMethods.cs`, `StationMethods.cs`, `UnitMethods.cs`.
  These are all parts of the same `partial class NerisBase`, split by resource, containing methods for interacting with NERIS API.
- `Models/ElementModels/` - the API's data objects (`EntityModel`, `IncidentModel`, `StationModel`,
  `UnitModel`, and the incident modules under `Incident/Modules/`).
- `Models/RequestModels/` - search/filter models (`IncidentRequestModel`, `EntityRequestModel`).
- `Models/Config.cs` - connection settings and endpoint selection.
- `Utils/` - serialization, URI composition, enum helpers, and the sync bridge.
- `Exceptions/` - Custom Errors this Library may throw. 

## Usage

### Configuration and authentication

Client Credential and Username/Password Authentication are both supported. 

#### Client Credential Authentication
Client credential authentication is preferred to Username/Password as Username/Password authentication requires completing an MFA challenge.
Supply a Client Id and Secret from NERIS. View NERIS documentation to learn how to create client credentials, this process may differ depending on if you are a
vendor/integration partner or fire service professional. 


```csharp
// UrlType.Live  -> https://api.neris.fsri.org/v1/
// UrlType.Test  -> https://api-test.neris.fsri.org/v1/
var config = Config.CreateClientCredentialConfig(clientId, clientSecret, UrlType.Live);
```
After supplying Client credentials, the library will handle all authentication automatically.


#### Username Password Authentication
Supply your NERIS username and password to the config. You will have to manually login and supply your MFA challenge code.

```csharp
// UrlType.Live  -> https://api.neris.fsri.org/v1/
// UrlType.Test  -> https://api-test.neris.fsri.org/v1/
var config = Config.CreatePasswordConfig(username, password, UrlType.Live);
NerisBase neris = new NerisBase(config, httpClient);
await neris.Login();
if (neris.RequiresChallengeResponse)
{
    //supply challenge code. Example with Console.
    string code = Console.ReadLine();
    neris.LoginChallenge(code);
}
```

If you attempt to access the API while an MFA challenge is required, the library will throw an `MFARequiredException` to inform you. 

#### General Config
`NerisBase` takes the config, an `HttpClient`, and optionally an `ILogger<NerisBase>`. Without a logger,
messages go to the console.

```csharp
var neris = new NerisBase(config, httpClient, logger);
```

### Write protection

Pass `denyWriteActions: true` to build a read-only client. Every POST/PUT/PATCH/DELETE method then throws
`NoAccessException` before issuing a request. The incident `Validate` endpoint is exempt, since it does
not modify data.

```csharp
var readOnly = new NerisBase(config, httpClient, logger, denyWriteActions: true);
```

### Model Paths:

All methods that interact with the API for a piece of data require you to supply the Id for the parent data.
For instance, all Incident methods require you to submit the Id of the parent Entity that owns the incident. 
For query methods, the id is placed into the RequestModel object.
This is also true for stations and units, where for a given unit you must supply the station that owns the unit, and the entity that owns the station.
Most methods have two overloads: one taking a full model (from which the `Neris_Id` is extracted) and one
taking the raw id string.

### Queries

Search endpoints take a request model. `Validate()` runs before the request and throws
`ValidationException` on bad input. Individual searches take a specific Id.

```csharp
var entities = await neris.GetEntities(new EntityRequestModel { State = "OR", Name = "Portland " });
var entity   = await neris.GetEntity("FD01234567");
```

### Pagination

Incident search endpoints return a `IncidentPageSet` carrying `Next_Cursor` and `Prev_Cursor`. Either walk the cursor
yourself with `GetIncidentsPage`, or let `GetAllIncidents` do it. The `IncidentRequestModel` class contains a field for the current cursor. 

```csharp
IncidentPageSet page = await neris.GetIncidentsPage(request);    // one page
request.Cursor = page.Next_Cursor;
IncidentPageSet pageTwo = await neris.GetIncidentsPage(request); // next page
List<IncidentModel> all = await neris.GetAllIncidents(request);  // every page
```

Entity Requests are slightly different, the `EntityPageSet` contains info on the `Page_Size`, `Page_Count`, and current `Page_Number`.
Increment the `Page_Number` on the EntityRequestModel to walk pages of Entities.


### Writes

Incidents are submitted as an `IncidentModelPayload`. A payload can be constructed from a retrieved
`IncidentModel`. Incident Post and Put methods are supported, Patch methods are not currently supported for Incidents. 

```csharp
var payload = new IncidentModelPayload(existingIncident);
payload.Base.People_Present = true; //modify the incident

if (await neris.ValidateIncident(entityId, payload))
{
    string nerisId = await neris.PutIncident(entityId, payload);
}
```


Entities, stations and units do not have their own Payload object and work off their base Model object.

```csharp
string stationId = await neris.PostStation(entityId, newStation);
string unitId    = await neris.PostUnit(entityId, stationId, newUnit);
```

For PUT and PATCH, server-managed fields (`neris_id`, and on incidents `submitter_account_type` and
`incident_status`) are stripped before sending. PATCH methods accept an optional `fieldsToNull` set:
snake_cased field names to explicitly null out on the server, since omitted fields are left unchanged.

```csharp
await neris.PatchStation(entityId, stationUpdate, new HashSet<string> { "address_line_2" });
```

### Synchronous callers

The library is async-first. Where a synchronous consumer needs it, a wrapper is provided over
`Utils/SyncRunner`:

```csharp
List<IncidentModel> incidents = SyncRunner.RunSync(() => GetAllIncidents(requestModel));
```
Prefer the async methods where you have the choice. The SyncRunner has not been extensively validated and tested.

### Errors

- `ValidationException` — a request model failed `Validate()`.
- `NoAccessException` — a write was attempted on a client constructed with `denyWriteActions: true`.
- `AuthorizationException` — authentication failed.
- `HttpRequestException` — the API returned a non-2xx status. The response body is included in the
  message.
- `ArgumentNullException` / `ArgumentException` — null or whitespace ids, checked before the request is
  built.
- `MFARequiredException` - the API was accessed while an MFA challenge is still hanging. Use the LoginChallenge() method to complete the challenge before attempting to access the API.

## Building

```bash
dotnet build NerisSharp.sln
dotnet test NerisSharpTest/NerisSharpTest.csproj
dotnet run --project TestProject        # manual harness; needs credentials
```

`TestProject` is a scratch harness rather than a test suite — it exercises methods against the live or
test API and expects credentials to be supplied in `Program.cs`.
