namespace ExporterPlugins.Contracts;

/// Metadata interface for MEF holds info about an exporter without needing to create an instance of it.
/// Used so the plugin loader can read plugin details before actually using the plugin.

public interface IExportPluginMetadata
{
    string Format { get; }
    string FileExtension { get; }
    string ContentType { get; }
    string DisplayName { get; }
}