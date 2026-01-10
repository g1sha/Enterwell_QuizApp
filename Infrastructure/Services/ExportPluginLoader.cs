using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using ExporterPlugins;
using ExporterPlugins.Contracts;

namespace Infrastructure.Services;

// Interface for the plugin loader service lets other parts of the app get info on
// available export formats and perform exports without needing to know plugin details.

public interface IExportPluginLoader : IDisposable
{
    IEnumerable<ExportFormatInfo> GetAvailableFormats();
    byte[] Export<T>(string format, IEnumerable<T> data);
}

// The workhorse that discovers and loads all the export plugin DLLs from a folder.
// Uses MEF to automatically find and wire up any class marked with [ExportPlugin].
// When you ask it to export data, it finds the right plugin and tells it to do the job.

public class ExportPluginLoader : IExportPluginLoader
{
    private readonly CompositionContainer _container;
    private readonly AggregateCatalog _catalog;

    // This gets populated by MEF with all the IExportPlugin implementations it can find
    // Stored as Lazy type so the actual plugin classes aren't instantiated until we need them
    [ImportMany(typeof(IExportPlugin))]
    private IEnumerable<Lazy<IExportPlugin, IExportPluginMetadata>>? _plugins;

    // Constructor takes the directory path where plugin DLLs are located
    public ExportPluginLoader(string pluginDirectory)
    {
        _catalog = new AggregateCatalog();
        
        if (Directory.Exists(pluginDirectory))
        {
            _catalog.Catalogs.Add(new DirectoryCatalog(pluginDirectory, "*.dll"));
        }

        _container = new CompositionContainer(_catalog);

        try
        {
            _container.ComposeParts(this);
        }
        catch (CompositionException ex)
        {
            throw new InvalidOperationException("Failed to load export plugins", ex);
        }
    }
    
    // Returns a list of all available export formats that were discovered.
    public IEnumerable<ExportFormatInfo> GetAvailableFormats()
    {
        return _plugins?.Select(p => new ExportFormatInfo(
            p.Metadata.Format,
            p.Metadata.FileExtension,
            p.Metadata.ContentType,
            p.Metadata.DisplayName
        )) ?? Enumerable.Empty<ExportFormatInfo>();
    }

    // Finds the plugin that matches the requested format and tells it to export the data
    // Throws ex if no plugin is found for that format
    public byte[] Export<T>(string format, IEnumerable<T> data)
    {
        var plugin = _plugins?.FirstOrDefault(p =>
            p.Metadata.Format.Equals(format, StringComparison.OrdinalIgnoreCase));

        if (plugin == null)
        {
            throw new ArgumentException($"Export format '{format}' is not supported");
        }

        // Convert to IEnumerable<object> and pass the type info
        var objectData = data.Cast<object>();
        return plugin.Value.Export(objectData, typeof(T));
    }

    // Cleans up MEF resources
    public void Dispose()
    {
        _container.Dispose();
        _catalog.Dispose();
    }
}