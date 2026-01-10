namespace ExporterPlugins.Contracts;

/// Simple data container that holds all the info about an export format.

public record ExportFormatInfo(
    string Format,
    string FileExtension,
    string ContentType,
    string DisplayName
);