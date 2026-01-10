using ExporterPlugins;
using ExporterPlugins.Contracts;

namespace Infrastructure.Services;

public interface IExportServiceFactory : IDisposable
{
    IExportPluginLoader GetLoader();
    IEnumerable<ExportFormatInfo> GetAllAvailableFormats();
}

public class ExportServiceFactory : IExportServiceFactory
{
    private readonly string _pluginDirectory;
    private readonly Lazy<ExportPluginLoader> _loader;

    public ExportServiceFactory(string pluginDirectory)
    {
        _pluginDirectory = pluginDirectory;
        _loader = new Lazy<ExportPluginLoader>(() => new ExportPluginLoader(_pluginDirectory));
    }

    // Returns the export plugin loader instance, creating it on first call
    public IExportPluginLoader GetLoader()
    {
        return _loader.Value;
    }

    public IEnumerable<ExportFormatInfo> GetAllAvailableFormats()
    {
        return _loader.Value.GetAvailableFormats();
    }
    
    // cleanup for the loader when the factory is disposed
    public void Dispose()
    {
        if (_loader.IsValueCreated)
        {
            _loader.Value.Dispose();
        }
    }
}