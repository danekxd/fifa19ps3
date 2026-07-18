# FIFA 19 PS3 Blaze Private Server — Step-by-Step Research and Implementation Roadmap

**Target captured from your setup**

- Game: FIFA 19 Legacy Edition, PS3, `BLES02258`
- PS3 address on the Windows hotspot: `192.168.137.211`
- Server PC/hotspot address: `192.168.137.1`
- First EA hostname observed: `winter15.gosredirector.ea.com`
- First EA TCP port observed: `42230`
- Transport observed: TLS followed by encrypted Blaze application data

This document is a practical development roadmap. It does **not** assume that an NHL or Skate server can simply be renamed to FIFA. The repositories provide the Blaze transport, TDF serialization, common components, PS3 ticket parsing, matchmaking patterns, relay patterns, and Ultimate Team architecture. FIFA-specific component definitions and responses still have to be discovered and implemented.

---

## 1. Pick the correct foundation

Use the repositories in these roles:

| Repository | Role in the FIFA project |
|---|---|
| `ZamboniDevelopment/Zamboni3` | Main starting server application. It already wires together a redirector, Blaze core server, PS3 ticket login, common EA Sports components, game management, reports, leagues, OSDK components, database support, and an Ultimate Team module. |
| `ZamboniDevelopment/BlazeSDK` | Blaze 3 server framework, FireFrame packet handling, TDF encoder/decoder, generated component bases, and old-protocol TLS support. Use the `nhl14legacy-compatability` branch first because that is the branch used by the Zamboni installation guide. |
| `Aim4kill/PSN` | Parses PS3 NP tickets supplied to the Blaze login request. It is used to create a local account identity without requesting the player's PSN password. |
| `ZamboniDevelopment/ZamboniCommonComponents` | EA Sports/OSDK structures and shared component implementations used by Zamboni3. |
| `ZamboniDevelopment/ZamboniUltimateTeam` | Reference architecture for an Ultimate Team backend. It is NHL/HUT-specific and is **not** a FIFA FUT implementation. |
| `ZamboniDevelopment/RelayProtocol` | Messages exchanged between the matchmaking server and relay servers. Defer this until login and lobby access work. |
| `skate6743/Skate3BlazeServer` | Secondary reference for a complete community server, local hosting, HTTP content, game creation, matchmaking, relay handling, and debugging patterns. Do not merge it wholesale into Zamboni3. |
| `Aim4kill/Bug_OldProtoSSL` | TLS compatibility research. Treat it as an experiment, not a guaranteed FIFA 19 solution. |

Repository addresses:

```text
https://github.com/ZamboniDevelopment/Zamboni3
https://github.com/ZamboniDevelopment/BlazeSDK
https://github.com/Aim4kill/PSN
https://github.com/ZamboniDevelopment/ZamboniCommonComponents
https://github.com/ZamboniDevelopment/ZamboniUltimateTeam
https://github.com/ZamboniDevelopment/RelayProtocol
https://github.com/skate6743/Skate3BlazeServer
https://github.com/Aim4kill/Bug_OldProtoSSL
```

### Recommended architecture

```text
FIFA 19 PS3
    |
    | DNS query for winter15.gosredirector.ea.com
    v
Local DNS on 192.168.137.1
    |
    | returns 192.168.137.1
    v
FIFA redirector listener :42230 (TLS)
    |
    | returns local Blaze core address
    v
FIFA Blaze core listener :16767 (initial development choice)
    |
    +-- authentication / PS3 ticket
    +-- user sessions
    +-- utility configuration
    +-- matchmaking and game manager
    +-- stats / clubs / seasons
    +-- later: FUT, HTTP content, QoS, relay servers
```

Keep Sony/PSN hostnames untouched. Redirect only the EA/FIFA hostnames that are observed in your captures.

---

## 2. Install the Windows development tools

Install:

1. Git for Windows.
2. .NET 8 SDK.
3. Visual Studio 2022 Community with the **.NET desktop development** workload, or Visual Studio Code with the C# extension.
4. Wireshark.
5. A local DNS server capable of a single hostname override and normal upstream forwarding.
6. PostgreSQL only when you reach persistence or Ultimate Team work. Zamboni3 can start without a working database; it disables database-backed functions when the connection fails.

Verify the tools in PowerShell:

```powershell
git --version
dotnet --info
```

Create the project directory:

```powershell
mkdir C:\fifa19-server
cd C:\fifa19-server
```

---

## 3. Clone the repositories as sibling folders

The folder names matter because `Zamboni3` uses relative project references such as `..\BlazeSDK`, `..\PSN`, and `..\ZamboniUltimateTeam`.

Run:

```powershell
cd C:\fifa19-server

git clone https://github.com/ZamboniDevelopment/Zamboni3.git
git clone --branch nhl14legacy-compatability https://github.com/ZamboniDevelopment/BlazeSDK.git
git clone https://github.com/Aim4kill/PSN.git
git clone https://github.com/ZamboniDevelopment/RelayProtocol.git
git clone https://github.com/ZamboniDevelopment/ZamboniCommonComponents.git
git clone https://github.com/ZamboniDevelopment/ZamboniUltimateTeam.git
git clone https://github.com/skate6743/Skate3BlazeServer.git
```

The resulting layout must resemble:

```text
C:\fifa19-server\
    BlazeSDK\
    PSN\
    RelayProtocol\
    Skate3BlazeServer\
    Zamboni3\
    ZamboniCommonComponents\
    ZamboniUltimateTeam\
```

Create your own branch before changing code:

```powershell
cd C:\fifa19-server\Zamboni3
git switch -c fifa19-bootstrap
```

---

## 4. Confirm the untouched server builds

From the Zamboni3 directory:

```powershell
dotnet restore
dotnet build -c Debug
```

Do not begin FIFA modifications until the base solution builds.

### Typical build failures

**A sibling project is missing**

Example:

```text
The project file ..\ZamboniCommonComponents\ZamboniCommonComponents.csproj was not found
```

Fix the directory layout rather than editing out the reference.

**BlazeSDK API mismatch**

Make sure the BlazeSDK checkout is on the compatibility branch:

```powershell
cd C:\fifa19-server\BlazeSDK
git branch --show-current
```

Expected initially:

```text
nhl14legacy-compatability
```

**Wrong .NET SDK**

The Zamboni3 project targets `net8.0`. Confirm .NET 8 appears in:

```powershell
dotnet --list-sdks
```

---

## 5. Make a minimal FIFA bootstrap fork

Open `C:\fifa19-server\Zamboni3\Program.cs`.

### 5.1 Rename the server

Change:

```csharp
public const string Name = "Zamboni14Legacy 1.1";
```

To:

```csharp
public const string Name = "FIFA19Blaze Bootstrap 0.1";
```

### 5.2 Change the redirector port to the captured FIFA port

Change:

```csharp
public const int RedirectorPort = 42127;
```

To:

```csharp
public const int RedirectorPort = 42230;
```

Do not copy the NHL redirector port or Skate's port. Your own capture showed FIFA connecting to `42230`.

### 5.3 Leave the first core port at 16767

`ZamboniConfig.cs` defaults the game/core server to `16767`. That port is only the address returned by your local redirector. It does not need to match EA's original core port during a local experiment.

Later, change it only if FIFA refuses the returned address or expects a secure core endpoint.

### 5.4 Disable nonessential modules during the first boot

For the first successful redirector test, reduce variables. In `StartCoreServer()`, temporarily comment out components that can introduce NHL-specific behavior, leaving the minimum framework:

```csharp
core.AddComponent<UtilComponent>();
core.AddComponent<AuthenticationComponent>();
core.AddComponent<UserSessionsComponent>();
```

Keep the other component registrations saved in Git; you will add them back when FIFA asks for them.

A more useful second-stage set is:

```csharp
core.AddComponent<UtilComponent>();
core.AddComponent<AuthenticationComponent>();
core.AddComponent<UserSessionsComponent>();
core.AddComponent<MessagingComponent>();
core.AddComponent<AssociationListsComponent>();
core.AddComponent<StatsComponent>();
core.AddComponent<GameManager>();
```

Do not enable `CardHouseComponent` merely because FIFA has FUT. Zamboni's CardHouse implementation is designed around NHL/HUT data and commands.

---

## 6. Create the first local configuration

Run the server once after it builds. Zamboni3 creates `zamboni-config.yml` when the file is absent.

Then edit it to:

```yaml
GameServerIp: "192.168.137.1"
GameServerPort: 16767
LogLevel: "Trace"
DatabaseConnectionString: "Host=127.0.0.1;Port=5432;Username=unused;Password=unused;Database=unused"
HostRedirectorInstance: true
ApiServerIdentifier: "fifa19"
ApiServerPort: "8082"
TargetProtocol: "FIFA19_PS3"
UseRelayServerImplementation: false
Relays: {}
Config: {}
TopologyOverride: 0
```

A failed database connection is acceptable during bootstrap. Zamboni3 catches it, sets `isEnabled` to false, and continues with fallback behavior.

The important values are:

```text
GameServerIp      192.168.137.1
GameServerPort    16767
LogLevel          Trace
HostRedirector    true
```

---

## 7. Solve TLS before attempting protocol work

This is the first serious blocker.

Zamboni3's redirector currently loads:

```csharp
var certBytes = File.ReadAllBytes("gosredirector_mod.pfx");
X509Certificate cert = new X509Certificate2(certBytes, "123456");
```

Therefore the process will not start its redirector without a compatible `gosredirector_mod.pfx` in the working directory.

### What not to assume

- A normal self-signed Windows certificate will probably be rejected by the game.
- Fiddler's `DO_NOT_TRUST_FiddlerRoot` certificate does not automatically become trusted by FIFA or the PS3.
- The `Bug_OldProtoSSL` technique only applies to vulnerable older ProtoSSL implementations. Its own documentation says newer versions fixed the issue.
- A minimal SSLv3 server used by older EA revivals is not automatically suitable for the TLS 1.2 connection observed in your FIFA capture.

### Development decision tree

**Test A — existing emulator certificate**

Place the known emulator `gosredirector_mod.pfx` beside the Zamboni executable, start the server, redirect DNS, and watch whether FIFA completes the TLS handshake.

**Test B — TLS alert immediately after ServerHello/certificate**

The client rejected the certificate or protocol parameters. Record the TLS alert in Wireshark. At that point you need one of these for your own client:

1. A certificate format accepted by the specific FIFA ProtoSSL version.
2. A FIFA EBOOT patch that changes certificate validation or trusts your development certificate.
3. A controlled PS3 trust-store modification.
4. RPCS3 development first, where a game patch and host override are easier to iterate.

Do not continue writing Blaze handlers until the server can decrypt and log the first request.

---

## 8. Add the DNS override

Run a DNS server on the Windows hotspot PC. Configure one local record:

```text
winter15.gosredirector.ea.com  A  192.168.137.1
```

All other requests must forward to a normal upstream DNS resolver so PSN still works.

Set the PS3 network configuration to:

```text
IP address: automatic
DNS: manual
Primary DNS: 192.168.137.1
Secondary DNS: leave blank or use the same local DNS if required
Proxy: do not use
```

Do **not** override:

```text
auth.np.ac.playstation.net
playstation.net
sonyentertainmentnetwork.com
```

Verify from the PC:

```powershell
nslookup winter15.gosredirector.ea.com 192.168.137.1
```

Expected answer:

```text
Address: 192.168.137.1
```

---

## 9. Open only the initial Windows Firewall ports

Run PowerShell as Administrator:

```powershell
New-NetFirewallRule -DisplayName "FIFA19 Local DNS UDP" -Direction Inbound -Protocol UDP -LocalPort 53 -Action Allow -Profile Private
New-NetFirewallRule -DisplayName "FIFA19 Local DNS TCP" -Direction Inbound -Protocol TCP -LocalPort 53 -Action Allow -Profile Private
New-NetFirewallRule -DisplayName "FIFA19 Blaze Redirector" -Direction Inbound -Protocol TCP -LocalPort 42230 -Action Allow -Profile Private
New-NetFirewallRule -DisplayName "FIFA19 Blaze Core" -Direction Inbound -Protocol TCP -LocalPort 16767 -Action Allow -Profile Private
```

Do not open the NHL or Skate relay port ranges yet. Add ports only when a capture proves FIFA needs them.

Confirm listeners after starting the server:

```powershell
Get-NetTCPConnection -State Listen | Where-Object LocalPort -in 42230,16767,8082
```

---

## 10. Run the first redirector test

Start Wireshark on both:

- `Local Area Connection* 10` / the hotspot virtual adapter
- the upstream Wi-Fi interface

Use this filter:

```wireshark
ip.addr == 192.168.137.211 && tcp.port == 42230
```

Start the server:

```powershell
cd C:\fifa19-server\Zamboni3
dotnet run
```

Then on the PS3:

1. Sign into PSN normally.
2. Launch FIFA 19.
3. Select an online feature.
4. Wait until an error or a menu change occurs.
5. Stop the Wireshark capture.

### Interpret the result

| Observation | Meaning | Next action |
|---|---|---|
| No DNS request | PS3 is not using your local DNS | Recheck PS3 DNS and DNS firewall rules |
| DNS returns the public EA IP | Override is not active | Correct the local DNS record |
| SYN to `192.168.137.1:42230`, no SYN-ACK | Server not listening or firewall blocked | Check listener and firewall |
| TCP connects, TLS alert follows | Certificate/protocol mismatch | Work on TLS compatibility |
| TLS completes, server logs a Blaze request | First major milestone reached | Inspect component, command, and decoded TDF |
| Redirector responds, then PS3 opens `192.168.137.1:16767` | Redirector works | Begin core component implementation |

---

## 11. Adapt the redirector response

Open:

```text
Zamboni3\Components\Blaze\RedirectorComponent.cs
```

Zamboni's redirector is designed to return `Program.GameServerIp` and `ZamboniConfig.GameServerPort` in a `ServerInstanceInfo` response.

For the first test, make sure the returned values are:

```text
Host/IP: 192.168.137.1
Port: 16767
```

The `secure` field is game-specific:

- Start with the existing Zamboni value only as a test.
- Watch the next connection in Wireshark.
- If the core connection starts with a TLS ClientHello, configure the core as secure.
- If it starts directly with a Blaze/FireFrame packet, keep the core plaintext.

Do not guess permanently. Let the first post-redirect packet decide.

---

## 12. Turn the server into a protocol logger

Your first objective is not a successful login. It is a precise log of every FIFA request.

For each request, save:

```text
Timestamp
Connection ID
Component ID and name
Command ID and name
Request/message ID
Raw frame bytes
Decoded TDF object
Response or error sent
What FIFA did next
```

Set `LogLevel: "Trace"`.

Search the BlazeSDK and Zamboni code for the request dispatch path:

```powershell
cd C:\fifa19-server
Get-ChildItem -Recurse -Filter *.cs | Select-String -Pattern "Unknown component|Unknown command|BlazeRpcException|FireFrame|MessageType"
```

Add logging immediately before the framework returns an unknown-component or unknown-command error. Preserve the raw payload before decoding fails.

Create a protocol notebook such as `fifa19-protocol.csv`:

```csv
step,component_id,command_id,request_type,response_type,result,next_client_action
1,?, ?,redirector request,server instance info,accepted,connected to core
2,?, ?,pre-auth,?,pending,retried
```

This notebook becomes the real FIFA server specification.

---

## 13. Implement components in the order FIFA requests them

Do not implement every component in advance. Follow the actual request sequence.

A likely order for an EA Sports Blaze client is:

### Stage A — bootstrap/utilities

Start in `UtilComponent.cs`.

Typical responsibilities include:

- pre-authentication configuration
- post-authentication configuration
- client configuration key/value maps
- ping/keepalive behavior
- telemetry addresses
- QoS addresses
- ticker or messaging configuration

Copying NHL values blindly can make FIFA loop or reject the session. Log the request fields and return the smallest structurally valid response first.

### Stage B — authentication

Use `AuthenticationComponent.cs` as the model.

Zamboni's PS3 login path parses the NP ticket carried by the Blaze request and maps the ticket identity to a local server user. Preserve this approach:

- never request the player's PSN password
- never proxy or store Sony credentials
- use the NP ticket already supplied by the game
- create your own local FIFA account ID and persona record

FIFA may expect entitlement, product, persona, locale, and legal-document fields that differ from NHL. Log every requested field and add only what the client demonstrates it requires.

### Stage C — user sessions

Implement enough of `UserSessionsComponent` for:

- local user/session creation
- persona lookup
- extended data updates
- presence and network information
- server notifications expected after login

A successful RPC response may still be insufficient if FIFA expects a notification immediately afterward.

### Stage D — social and menu components

Add only when requested:

- association lists/friends
- messaging
- rooms
- census data
- clubs
- stats
- league/seasons
- OSDK settings, ticker, online-pass, dynamic messaging

Zamboni3 already registers many of these components, making it a useful structural reference, but the payload values are NHL-specific.

### Stage E — game manager and matchmaking

Use `GameManager.cs`, `ServerManager.cs`, `ServerGame.cs`, and Skate3's game management code as references for:

- create game
- join game
- matchmaking scenario/session
- player state changes
- game state notifications
- host migration
- disconnect handling

The goal here is first to make two clients see the same local lobby. Actual gameplay transport comes later.

### Stage F — game reporting

Only after a match starts, implement FIFA report structures. Do not reuse NHL report structs as final schemas.

### Stage G — FUT

Treat FUT as a separate subproject after ordinary online menus and matchmaking work.

Use `ZamboniUltimateTeam` to understand:

- module boundaries
- database initialization
- card/item models
- configuration reloads
- how a Blaze component is connected to a persistent economy

Do not copy HUT item definitions, pack contents, salary logic, or component payloads into FIFA.

---

## 14. Discover FIFA-specific component definitions

BlazeSDK's generated component bases are based largely on other Blaze 3 titles. FIFA may use component/command IDs or TDF structures not linked to the existing generated classes.

When the logger reports an unknown component or command:

1. Record its numeric component ID and command ID.
2. Save the raw TDF payload.
3. Check whether an existing BlazeSDK JSON definition contains the IDs.
4. Check ZamboniCommonComponents and OSDK components.
5. Check Skate3BlazeServer for the same numeric IDs.
6. Search the FIFA EBOOT and game files for component names, command strings, service names, configuration keys, and TDF labels.
7. Add a FIFA-specific component definition rather than changing an unrelated NHL/Battlefield definition.

BlazeSDK warns that generated component base `.cs` files should be produced from their JSON definitions. When modifying a generated component, edit the source definition and regenerate where practical instead of permanently editing generated output.

Organize FIFA additions separately:

```text
Zamboni3\Components\Fifa19\
    FifaUtilComponent.cs
    FifaAuthenticationComponent.cs
    FifaGameManagerComponent.cs
    FifaStatsComponent.cs
    FifaSeasonsComponent.cs
    FifaUltimateTeamComponent.cs

ZamboniCommonComponents\Fifa19\
    Requests\
    Responses\
    Notifications\
    Enums\
```

This prevents NHL assumptions from becoming hidden inside FIFA code.

---

## 15. Add HTTP, QoS, and relay services only when observed

The Skate server contains HTTP/content and relay implementations. Zamboni uses a separate QoS service and relay protocol. These are references, not proof that FIFA uses identical endpoints or ports.

### HTTP/content service

Add an HTTP server when Wireshark shows FIFA resolving or connecting to an HTTP hostname. Log:

- hostname
- path
- method
- query string
- request headers/body
- expected content type

Return minimal static test responses before creating a full content system.

### QoS

Add a QoS service when the client asks for QoS configuration or contacts a measured UDP/TCP endpoint. Copy the transport pattern, not NHL's hardcoded response values.

### Relay/game traffic

Wait until matchmaking reaches game setup. Then capture:

```wireshark
ip.addr == 192.168.137.211 && udp
```

Determine whether FIFA uses:

- direct peer-to-peer traffic
- a dedicated relay
- host-authoritative traffic
- NAT traversal and probes

Open only the observed ports. Do not begin with Skate's or NHL's complete relay ranges.

---

## 16. Test with milestones, not “does online work?”

Use these milestones:

### M0 — build

`dotnet build` succeeds with all sibling projects.

### M1 — DNS

The PS3 resolves `winter15.gosredirector.ea.com` to `192.168.137.1`.

### M2 — transport

The PS3 completes TCP and TLS to the local listener on `42230`.

### M3 — redirector protocol

The server decodes the first Blaze redirector request and FIFA accepts the response.

### M4 — core connection

FIFA connects to the returned local core address and port.

### M5 — pre-auth

Utility/pre-auth requests stop looping and progress to authentication.

### M6 — authentication

The PS3 NP ticket is parsed and FIFA accepts a local session/persona.

### M7 — online menu

The game reaches an online menu without EA's original servers.

### M8 — two-client lobby

Two clients authenticate and see/join the same game session.

### M9 — match transport

A match starts and gameplay packets flow between peers or through a relay.

### M10 — persistence

Stats, clubs, reports, or FUT data survive server restarts.

Do not work on M8 while M3 is unstable.

---

## 17. Troubleshooting map

| Symptom | Likely cause |
|---|---|
| FIFA still contacts `159.153.51.20` | DNS override is not being used |
| Local server receives SYN but no TLS ClientHello | Wrong port, immediate client abort, or network filtering |
| TLS alert after certificate | Certificate validation, protocol, or cipher mismatch |
| Redirector logs request but FIFA reconnects repeatedly | Response fields or secure flag are wrong |
| Redirector succeeds but no core connection appears | Returned IP/port is unreachable or response schema is rejected |
| Core connects then closes before a decoded request | Wrong framing/TDF version or secure/plaintext mismatch |
| Same Blaze request repeats | The response is malformed, incomplete, or missing a required notification |
| Unknown component | FIFA-specific component definition is missing |
| Known component but unknown command | Existing generated base is from a different Blaze title/version |
| Authentication says invalid ticket | PS3 ticket parser/version mismatch or wrong request structure |
| Login succeeds but menu hangs | Missing utility/config/social notifications or a separate HTTP service |
| Matchmaking succeeds but game never starts | NAT/QoS/relay/game-state notification work remains |

---

## 18. What to read first in each repository

### Zamboni3

Read in this order:

1. `Program.cs`
2. `ZamboniConfig.cs`
3. `Components/Blaze/RedirectorComponent.cs`
4. `ZamboniCoreServer.cs`
5. `Components/Blaze/UtilComponent.cs`
6. `Components/Blaze/AuthenticationComponent.cs`
7. `Components/Blaze/UserSessionsComponent.cs`
8. `Components/Blaze/GameManager.cs`
9. `ServerManager.cs`, `ServerPlayer.cs`, `ServerGame.cs`
10. `GameReportingComponent.cs`
11. `RelayCommunicator.cs`
12. `Database.cs`

### BlazeSDK

Read:

1. `ExampleBlazeRedirectorServer`
2. `BlazeCommon` server and connection classes
3. FireFrame/header parsing
4. component dispatch logic
5. `Tdf` encoder and decoder
6. generated component JSON definitions and bases
7. `FixedSsl`

### Aim4kill/PSN

Read the NP ticket parser and determine exactly which fields Zamboni's authentication handler consumes.

### ZamboniCommonComponents

Locate OSDK structures, EA Sports-specific request/response types, and shared notifications.

### ZamboniUltimateTeam

Read initialization and storage boundaries only after normal authentication works.

### Skate3BlazeServer

Read:

1. main server startup
2. redirector/router flow
3. authentication and user sessions
4. game manager and matchmaking
5. relay setup
6. HTTP server and static content
7. logging of unknown messages

Use it to compare architectural solutions, not to copy Skate-specific payloads.

---

## 19. First-week execution plan

### Day 1

- Install tools.
- Clone all sibling repositories.
- Build unmodified Zamboni3.
- Create the `fifa19-bootstrap` branch.

### Day 2

- Change redirector port to `42230`.
- Configure `192.168.137.1` and core port `16767`.
- Set Trace logging.
- Add Windows Firewall rules.

### Day 3

- Configure local DNS override.
- Confirm FIFA contacts the PC rather than EA.
- Test TLS compatibility.

### Day 4

- Instrument redirector request/response logs.
- Get a decoded redirector request.
- Make FIFA attempt the local core connection.

### Day 5

- Instrument raw core dispatch.
- Record the first 10 component/command requests.
- Create FIFA-specific component folders and protocol notebook.

### Day 6–7

- Implement the first utility/pre-auth response.
- Repeat until the client advances to authentication.
- Commit each behavioral advance separately.

Suggested commit pattern:

```text
bootstrap: build Zamboni dependencies on Windows
network: listen on FIFA 19 redirector port 42230
logging: dump unknown Blaze component and command IDs
redirector: return local core endpoint
util: satisfy first FIFA pre-auth request
```

---

## 20. The honest stopping point of the repositories

The repositories give you a strong protocol and architecture base, but they do not contain FIFA 19's original backend behavior. Your first difficult unknowns are:

1. Whether FIFA 19 accepts the emulator TLS certificate on a physical PS3.
2. The exact Blaze version/framing and generated definitions used by FIFA 19.
3. The redirector response fields FIFA requires.
4. FIFA-specific utility configuration and authentication responses.
5. Which online modes use Blaze versus separate HTTP services.
6. FIFA matchmaking attributes and game-state notifications.
7. FUT's independent schemas, economy, inventory, catalog, and content services.

The correct workflow is therefore:

```text
redirect one hostname
→ terminate TLS
→ decode one request
→ send one structurally valid response
→ observe the next request
→ document it
→ implement it
→ repeat
```

That loop—not the Wireshark DNS packet alone—is how the private server is built.
