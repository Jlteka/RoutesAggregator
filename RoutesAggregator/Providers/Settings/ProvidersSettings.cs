namespace RoutesAggregator.Providers.Settings;
public class ProvidersSettings
{ 
    public ProviderSettings[] ProviderSettings { get; set; }
}

public class ProviderSettings
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
}