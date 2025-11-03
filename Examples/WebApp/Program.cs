using BogusExample.Apps;
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
var server = new Server();
#if DEBUG
server.UseHotReload();
#endif
server.AddAppsFromAssembly();
server.AddConnectionsFromAssembly();
var customHeader = Layout.Vertical().Gap(2)
    |new Embed("https://github.com/codespaces/new?hide_repo_select=true&ref=main&repo=Ivy-Interactive%2FIvy-Examples&machine=standardLinux32gb&devcontainer_path=.devcontainer%2Fbogus%2Fdevcontainer.json&location=EuropeWest");
var chromeSettings = new ChromeSettings()
    .DefaultApp<BogusApp>()
    .UseTabs(preventDuplicates: true)
    .Header(customHeader);
server.UseChrome(chromeSettings);
await server.RunAsync();