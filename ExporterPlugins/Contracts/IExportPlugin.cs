namespace ExporterPlugins.Contracts;

/// Interface that all export plugins must implement.
/// Any new exporter needs to follow this contract
/// so the plugin loader knows how to use it.

public interface IExportPlugin
{
    string Format { get; }
    string FileExtension { get; }
    string ContentType { get; }
    string DisplayName { get; }
    byte[] Export(IEnumerable<object> data, Type itemType);
}