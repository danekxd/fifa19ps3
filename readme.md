# FIFA 19 PS3 Blaze Private Server — Step-by-Step Research and Implementation Roadmap

> **Updated for the current LAN setup.** The earlier hotspot-only instructions have been corrected. Zamboni, DNS, and packet capture are separate parts of the setup.

## Current tested setup

- Game: FIFA 19 Legacy Edition, PS3, `BLES02258`
- Server PC LAN address: `192.168.1.83`  
  Verify this with `ipconfig`; update every example if the address changes.
- PS3: connected to the same normal router as the PC, with an automatic IP address
- Windows Mobile Hotspot: **off** while a DNS server runs on this PC
- EA hostname observed: `winter15.gosredirector.ea.com`
- EA redirector TCP port observed: `42230`
- Local Blaze core port: `16767`
- Local HTTP/API port: `8082`
- Transport observed: TLS followed by encrypted Blaze application data
- Zamboni startup has been confirmed on ports `16767`, `8082`, and `42230`
- A bootstrap self-signed `gosredirector_mod.pfx` can start the redirector, but this does **not** prove FIFA will trust it

### Current network flow

```text
FIFA 19 PS3
    |
    | DNS query: winter15.gosredirector.ea.com
    v
Separate local DNS service on 192.168.1.83:53
    |
    | special answer: 192.168.1.83
    | all other names: forwarded normally
    v
Zamboni redirector on 192.168.1.83:42230 (TLS)
    |
    | returns the local Blaze core endpoint
    v
Zamboni core on 192.168.1.83:16767
```

**Important:** Zamboni does not include a DNS server. Turning off Mobile Hotspot only frees DNS port `53`; it does not create the hostname override.

Wireshark does not require Mobile Hotspot for this test. When FIFA is redirected to the PC, the traffic is addressed directly to the PC, so Wireshark can capture it on the PC's active Wi-Fi or Ethernet adapter.

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
    | same normal router/LAN
    v
Local DNS service on the server PC: 192.168.1.83:53
    |
    | winter15.gosredirector.ea.com -> 192.168.1.83
    v
FIFA redirector listener: 192.168.1.83:42230 (TLS)
    |
    | returns 192.168.1.83:16767
    v
FIFA Blaze core listener: 192.168.1.83:16767
    |
    +-- authentication / PS3 ticket
    +-- user sessions
    +-- utility configuration
    +-- matchmaking and game manager
    +-- stats / clubs / seasons
    +-- later: FUT, HTTP content, QoS, relay servers
```

Keep Sony/PSN hostnames untouched. Redirect only the EA/FIFA hostnames observed in captures.

A Windows hotspot is optional, not required. If it is enabled, Internet Connection Sharing (`SharedAccess`) commonly owns port `53`; in that topology, run the custom DNS service on another device or stop using the hotspot for the test.

---

## 2. Install the Windows development tools

Install:

1. Git for Windows.
2. .NET 8 SDK.
3. Visual Studio 2022 Community with the **.NET desktop development** workload, or Visual Studio Code with the C# extension.
4. Wireshark.
5. A DNS solution capable of one hostname override plus normal upstream forwarding. Zamboni does not provide DNS. This can be a local DNS application, a router with custom DNS records, or another LAN device.
6. PostgreSQL only when you reach persistence or Ultimate Team work. Zamboni3 can start without a working database; it disables database-backed functions when the connection fails.

Verify the tools in PowerShell:

```powershell
git --version
dotnet --info
```

Create the project directory:

```powershell
mkdir C:\Users\Administrator\Desktop\fifa19
cd C:\Users\Administrator\Desktop\fifa19
```

---

## 3. Clone the repositories as sibling folders

The folder names matter because `Zamboni3` uses relative project references such as `..\BlazeSDK`, `..\PSN`, and `..\ZamboniUltimateTeam`.

Run:

```powershell
cd C:\Users\Administrator\Desktop\fifa19

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
C:\Users\Administrator\Desktop\fifa19\
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
cd C:\Users\Administrator\Desktop\fifa19\Zamboni3
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
cd C:\Users\Administrator\Desktop\fifa19\BlazeSDK
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


**`CS1024: Preprocessor directive expected` after editing `Program.cs`**

C# comments begin with `//`, not `#`.

Wrong:

```csharp
# core.AddComponent<UtilComponent>();
```

Correct:

```csharp
// core.AddComponent<UtilComponent>();
```

Check the exact reported line numbers and replace accidental `#` characters before rebuilding.

---

## 5. Make a minimal FIFA bootstrap fork

Open `C:\Users\Administrator\Desktop\fifa19\Zamboni3\Program.cs`.

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

Run the server once from the `Zamboni3` working directory after a successful build:

```powershell
cd C:\Users\Administrator\Desktop\fifa19\Zamboni3
dotnet run --project .\Zamboni14Legacy.csproj -c Release
```

When `zamboni-config.yml` is absent, Zamboni creates it in the current working directory. Stop the process with `Ctrl+C`, then edit the generated file.

For the current normal-router setup:

```yaml
GameServerIp: "192.168.1.83"
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

A failed database connection is acceptable during bootstrap. The expected warning is similar to:

```text
Database is not accessible. Gamedata wont be saved
```

The process should continue. A successful startup currently resembles:

```text
Starting insecure ProtoFireServer(CoreServer) on port 16767...
Now listening on: http://0.0.0.0:8082
Starting secure ProtoFireServer(RedirectorServer) on port 42230...
FIFA19Blaze Bootstrap 0.1 started
```

The important values are:

```text
GameServerIp       192.168.1.83
GameServerPort     16767
LogLevel           Trace
HostRedirector     true
Redirector port    42230
```

If the PC's LAN address changes, update all three places together:

```text
zamboni-config.yml GameServerIp
DNS override target
PS3 primary/secondary DNS
```

Do not use the old hotspot address `192.168.137.1` unless the PS3 is actually connected through that hotspot.

## 7. Solve TLS before attempting protocol work

This is the first serious protocol blocker.

Zamboni3's redirector currently loads:

```csharp
var certBytes = File.ReadAllBytes("gosredirector_mod.pfx");
X509Certificate cert = new X509Certificate2(certBytes, "123456");
```

The process therefore needs `gosredirector_mod.pfx` in its **working directory** and expects the password `123456`.

### Bootstrap-only self-signed PFX

A self-signed PFX is useful for proving that Zamboni can start its secure redirector. From the `Zamboni3` directory, run:

```powershell
$cert = New-SelfSignedCertificate `
    -DnsName "winter15.gosredirector.ea.com" `
    -CertStoreLocation "Cert:\CurrentUser\My" `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -HashAlgorithm SHA256 `
    -KeyExportPolicy Exportable `
    -NotAfter (Get-Date).AddYears(5)

$password = ConvertTo-SecureString "123456" -AsPlainText -Force

Export-PfxCertificate `
    -Cert $cert `
    -FilePath ".\gosredirector_mod.pfx" `
    -Password $password
```

Confirm:

```powershell
Get-Item .\gosredirector_mod.pfx
```

This proves only:

```text
Zamboni can load the PFX
→ the redirector can listen on port 42230
```

It does **not** prove:

```text
FIFA trusts the certificate
→ the TLS handshake completes
```

### What not to assume

- A normal self-signed Windows certificate will probably be rejected by the game.
- Fiddler's `DO_NOT_TRUST_FiddlerRoot` certificate is trusted only where it is installed; it does not automatically become trusted by FIFA or the PS3.
- Installing Fiddler's root on Windows does not make the PS3 trust it.
- The `Bug_OldProtoSSL` technique only applies to vulnerable older ProtoSSL implementations. Newer versions fixed the issue.
- A minimal SSLv3 server used by older EA revivals is not automatically suitable for the TLS 1.2 connection observed in the FIFA capture.

### Development decision tree

**Test A — bootstrap self-signed certificate**

Start Zamboni and confirm the redirector listener starts. Redirect DNS and capture the handshake.

**Test B — known emulator certificate**

If a certificate known to work with this exact FIFA/ProtoSSL version exists, place it beside the Zamboni executable, start the server, redirect DNS, and test again.

**Test C — TLS alert immediately after ServerHello/certificate**

The client rejected the certificate or protocol parameters. Record the TLS alert in Wireshark. Possible development routes for your own client are:

1. A certificate and chain accepted by the specific FIFA ProtoSSL version.
2. A FIFA EBOOT development patch that changes certificate validation or trusts your development certificate.
3. A controlled PS3 trust-store modification.
4. RPCS3 development first, where host overrides, executable patches, logs, and rollback are easier.

Do not continue implementing Blaze handlers until the server can decrypt and log the first request.

## 8. Add the DNS override

DNS is a separate service from Zamboni.

The required record for the current LAN setup is:

```text
winter15.gosredirector.ea.com  A  192.168.1.83
```

All other requests must resolve normally through an upstream DNS resolver so PSN and unrelated services continue to work.

### 8.1 Use the correct network topology

Recommended:

```text
Router
├── PC/Zamboni/DNS: 192.168.1.83
└── PS3:             192.168.1.x
```

Both devices connect directly to the same router. Mobile Hotspot is **off**.

Why: Windows Internet Connection Sharing (`SharedAccess`) commonly binds UDP/TCP port `53` on `0.0.0.0`. While that service owns port `53`, a custom DNS server on the same PC cannot bind normally.

Check the current PC address:

```powershell
ipconfig
```

Check whether anything already owns DNS port `53`:

```powershell
Get-NetUDPEndpoint -LocalPort 53 -ErrorAction SilentlyContinue |
    Select-Object LocalAddress,LocalPort,OwningProcess

Get-NetTCPConnection -LocalPort 53 -State Listen -ErrorAction SilentlyContinue |
    Select-Object LocalAddress,LocalPort,OwningProcess
```

If Mobile Hotspot is off but `SharedAccess` is still running, stop it from an Administrator PowerShell:

```powershell
Stop-Service SharedAccess -Force
```

Turning off the hotspot or stopping `SharedAccess` only frees port `53`. It does **not** start a DNS server.

### 8.2 Choose one DNS implementation

Use exactly one of these:

1. **A local DNS application on the PC** that can create a local A record and forward every other query upstream.
2. **A router feature** for local DNS/host overrides, if the router supports per-host records.
3. **Another device on the LAN** running DNS, such as another PC or Raspberry Pi.

The roadmap does not require a particular DNS program. Whatever implementation is chosen must:

- listen on the IP used as the PS3's DNS server;
- answer UDP port `53` and ideally TCP port `53`;
- return `192.168.1.83` only for `winter15.gosredirector.ea.com`;
- forward all other names normally.

If Windows Mobile Hotspot must remain enabled, run the custom DNS server on another hotspot client rather than on `192.168.137.1`, because ICS owns DNS on the hotspot host.

### 8.3 Configure the record

Create:

```text
Name:    winter15.gosredirector.ea.com
Type:    A
Address: 192.168.1.83
TTL:     60 or another low development value
```

Do **not** override:

```text
auth.np.ac.playstation.net
playstation.net
sonyentertainmentnetwork.com
```

### 8.4 Verify DNS before touching the PS3

A DNS server must be actively running before this command can work:

```powershell
nslookup winter15.gosredirector.ea.com 192.168.1.83
```

Required answer:

```text
Name:    winter15.gosredirector.ea.com
Address: 192.168.1.83
```

Also verify forwarding:

```powershell
nslookup playstation.net 192.168.1.83
```

That should return public addresses, not `192.168.1.83`.

Interpretation:

| Result | Meaning |
|---|---|
| `Address: 192.168.1.83` | Override works |
| A public EA address such as `159.153.x.x` | DNS is running, but the override is missing or bypassed |
| `No response from server` / timeout | No DNS service is listening, it is bound to the wrong interface, or the firewall blocks port `53` |
| `Server: UnKnown` | Usually harmless; it only means there is no reverse-DNS name for the local DNS IP |

### 8.5 Configure the PS3

Use:

```text
IP address:    Automatic
DNS:           Manual
Primary DNS:   192.168.1.83
Secondary DNS: 192.168.1.83
Proxy:         Do Not Use
MTU:           Automatic
UPnP:          Enable
```

Using the same local DNS address twice prevents the PS3 from bypassing the override through a public secondary resolver.

## 9. Open only the initial Windows Firewall ports

Run PowerShell as Administrator:

```powershell
New-NetFirewallRule -DisplayName "FIFA19 Local DNS UDP" -Direction Inbound -Protocol UDP -LocalPort 53 -Action Allow -Profile Any
New-NetFirewallRule -DisplayName "FIFA19 Local DNS TCP" -Direction Inbound -Protocol TCP -LocalPort 53 -Action Allow -Profile Any
New-NetFirewallRule -DisplayName "FIFA19 Blaze Redirector" -Direction Inbound -Protocol TCP -LocalPort 42230 -Action Allow -Profile Any
New-NetFirewallRule -DisplayName "FIFA19 Blaze Core" -Direction Inbound -Protocol TCP -LocalPort 16767 -Action Allow -Profile Any
New-NetFirewallRule -DisplayName "FIFA19 HTTP API" -Direction Inbound -Protocol TCP -LocalPort 8082 -Action Allow -Profile Any
```

Do not open NHL or Skate relay ranges yet. Add ports only when a capture proves FIFA needs them.

Confirm Zamboni listeners after starting the server:

```powershell
Get-NetTCPConnection -State Listen |
    Where-Object LocalPort -in 42230,16767,8082 |
    Select-Object LocalAddress,LocalPort,State
```

Confirm the separate DNS service:

```powershell
Get-NetUDPEndpoint -LocalPort 53 -ErrorAction SilentlyContinue
Get-NetTCPConnection -LocalPort 53 -State Listen -ErrorAction SilentlyContinue
```

Zamboni listening on `42230`, `16767`, and `8082` does not imply that DNS port `53` is available.

## 10. Run the first redirector test

### 10.1 Prove that the PS3 can reach the PC

Keep Zamboni running and open this from the PS3 browser:

```text
http://192.168.1.83:8082
```

A page, API response, or even an HTTP error generated by the local service proves the PS3 reached the PC.

For a dedicated basic HTTP test, optionally run from the PC:

```powershell
cd C:\Users\Administrator\Desktop\fifa19
py -m http.server 8000 --bind 0.0.0.0
```

Then browse on the PS3 to:

```text
http://192.168.1.83:8000
```

This tests only LAN reachability. It does not test DNS, TLS, or Blaze.

### 10.2 Start the server

```powershell
cd C:\Users\Administrator\Desktop\fifa19\Zamboni3
dotnet run --project .\Zamboni14Legacy.csproj -c Release
```

Keep the console open. Expected listeners:

```text
16767  CoreServer
8082   HTTP/API
42230  RedirectorServer
```

### 10.3 Capture with Wireshark on the normal LAN

Mobile Hotspot is not needed.

1. Find the PS3's current IP in PS3 network information or the router's client list.
2. Start Wireshark on the PC's active Wi-Fi or Ethernet adapter.
3. Replace `<PS3_IP>` in this display filter:

```wireshark
ip.addr == <PS3_IP> && (
    udp.port == 53 ||
    tcp.port == 42230 ||
    tcp.port == 16767 ||
    tcp.port == 8082
)
```

Example:

```wireshark
ip.addr == 192.168.1.90 && (udp.port == 53 || tcp.port == 42230 || tcp.port == 16767)
```

On a switched home network, the PC generally cannot see arbitrary PS3-to-Internet traffic. That is not a blocker here: after the DNS override, the FIFA connection is addressed directly to the PC, so Wireshark sees it.

### 10.4 Launch the test

On the PS3:

1. Sign into PSN normally.
2. Launch FIFA 19.
3. Select an online feature.
4. Wait for an error, menu change, or new Zamboni log entry.
5. Stop the Wireshark capture.

### Interpret the result

| Observation | Meaning | Next action |
|---|---|---|
| No DNS request reaches the configured DNS server | PS3 is not using that DNS address | Recheck PS3 DNS and network settings |
| DNS returns a public EA IP | Override is not active | Correct the local DNS record |
| DNS returns `192.168.1.83`, but no TCP SYN follows | FIFA did not attempt the endpoint or cached another result | Restart the game, clear/retest DNS, inspect other requested hostnames |
| SYN to `192.168.1.83:42230`, no SYN-ACK | Server not listening or firewall blocked | Check the listener and firewall |
| TCP connects and a TLS ClientHello appears | PS3 successfully reached the local redirector | Inspect the server reply |
| TLS alert follows ServerHello/certificate | Certificate, protocol, cipher, or signature mismatch | Record the alert and continue TLS research |
| TLS completes and Zamboni logs a Blaze request | First major milestone reached | Inspect component ID, command ID, and decoded TDF |
| Redirector responds, then PS3 opens `192.168.1.83:16767` | Redirector works | Begin core component implementation |

If no traffic appears, verify that `nslookup` returns `192.168.1.83` before changing Blaze code.

## 11. Adapt the redirector response

Open:

```text
Zamboni3\Components\Blaze\RedirectorComponent.cs
```

Zamboni's redirector is designed to return `Program.GameServerIp` and `ZamboniConfig.GameServerPort` in a `ServerInstanceInfo` response.

For the first test, make sure the returned values are:

```text
Host/IP: 192.168.1.83
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
cd C:\Users\Administrator\Desktop\fifa19
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
ip.addr == <PS3_IP> && udp
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

The PS3 resolves `winter15.gosredirector.ea.com` to `192.168.1.83`.

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
| `nslookup ... 192.168.1.83` says `No response from server` | No separate DNS server is listening on the PC, it is bound incorrectly, or the firewall blocks port `53` |
| DNS returns `159.153.x.x` or another public EA address | The DNS server is forwarding normally but the local override is missing or not selected |
| Mobile Hotspot is enabled and custom DNS cannot bind to port `53` | Windows ICS/`SharedAccess` owns DNS; disable the hotspot or run DNS on another device |
| FIFA still contacts a public EA address | PS3 is bypassing the local DNS, using a secondary resolver, or using a different hostname |
| PS3 browser cannot open `http://192.168.1.83:8082` | Basic LAN reachability, firewall, or PC IP is wrong |
| Local server receives SYN but no TLS ClientHello | Wrong port, immediate client abort, or network filtering |
| TLS alert after certificate | Certificate validation, protocol, cipher, signature, or hostname mismatch |
| Redirector logs request but FIFA reconnects repeatedly | Response fields or secure flag are wrong |
| Redirector succeeds but no core connection appears | Returned IP/port is unreachable or response schema is rejected |
| Core connects then closes before a decoded request | Wrong framing/TDF version or secure/plaintext mismatch |
| Same Blaze request repeats | Response is malformed, incomplete, or missing a required notification |
| Unknown component | FIFA-specific component definition is missing |
| Known component but unknown command | Existing generated base is from another Blaze title/version |
| Authentication says invalid ticket | PS3 ticket parser/version mismatch or wrong request structure |
| Login succeeds but menu hangs | Missing utility/config/social notifications or a separate HTTP service |
| Matchmaking succeeds but game never starts | NAT/QoS/relay/game-state notification work remains |
| `CS1024: Preprocessor directive expected` | A C# line was commented with `#`; use `//` |
| Database warning appears but server continues | Persistence is unavailable; acceptable for early transport tests |

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
- Configure the current PC LAN address, for example `192.168.1.83`, and core port `16767`.
- Generate or obtain the bootstrap PFX.
- Set Trace logging.
- Add Windows Firewall rules.
- Confirm Zamboni starts on `16767`, `8082`, and `42230`.

### Day 3

- Keep PC and PS3 on the same normal router.
- Disable Mobile Hotspot/ICS on the DNS-hosting PC.
- Configure a separate local DNS service and the single override.
- Confirm `nslookup` returns the PC LAN address.
- Confirm PS3-to-PC HTTP reachability.
- Capture the first redirector TLS attempt in Wireshark.

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
