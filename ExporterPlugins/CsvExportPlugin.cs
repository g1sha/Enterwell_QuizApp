using System.Reflection;
using System.Text;
using ExporterPlugins.Contracts;


namespace ExporterPlugins;

// CSV exporter plugin converts data into CSV format
// This class gets auto-discovered by MEF thanks to the [ExportPlugin] attribute,
// so when someone requests format="csv" in the API, this plugin handles the conversion.

[ExportPlugin("csv", ".csv", "text/csv", "CSV Export")]
public class CsvExportPlugin : IExportPlugin
{
    public string Format => "csv";
    public string FileExtension => ".csv";
    public string ContentType => "text/csv";
    public string DisplayName => "CSV Export";

    // Takes a list of objects and converts them to CSV format as bytes.
    // Builds a header row from property names, then adds each object as a data row.
    public byte[] Export(IEnumerable<object> data, Type itemType)
    {
        var sb = new StringBuilder();
        var properties = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        sb.AppendLine(string.Join(",", properties.Select(p => EscapeCsvField(p.Name))));
        
        foreach (var item in data)
        {
            var values = properties.Select(p => EscapeCsvField(p.GetValue(item)?.ToString() ?? ""));
            sb.AppendLine(string.Join(",", values));
        }
        
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}