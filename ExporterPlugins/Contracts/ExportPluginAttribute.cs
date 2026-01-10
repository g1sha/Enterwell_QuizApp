using System.ComponentModel.Composition;

namespace ExporterPlugins.Contracts;

/// This attribute decorates exporter classes so MEF can automatically discover them.
/// When you add this on a class like [ExportPlugin("csv", ".scv", ...)], it tells the plugin loader
/// that it's an export plugin and how to use it.

[MetadataAttribute]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ExportPluginAttribute : ExportAttribute, IExportPluginMetadata
{
    public string Format { get; }
    public string FileExtension { get; }
    public string ContentType { get; }
    public string DisplayName { get; }

    public ExportPluginAttribute(string format, string fileExtension, string contentType, string displayName)
        : base(typeof(IExportPlugin))
    {
        Format = format;
        FileExtension = fileExtension;
        ContentType = contentType;
        DisplayName = displayName;
    }
}