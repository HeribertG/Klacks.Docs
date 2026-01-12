namespace Klacks.Docs;

public static class DocsReader
{
    private static readonly Dictionary<string, string> AvailableDocs = new()
    {
        ["general"] = "Allgemeine Hilfe und Systemübersicht",
        ["clients"] = "Mitarbeiterverwaltung",
        ["shifts"] = "Schichtplanung",
        ["identity-providers"] = "LDAP/OAuth2 Konfiguration",
        ["macros"] = "Makro-Scripting System"
    };

    public static IReadOnlyDictionary<string, string> GetAvailableDocs() => AvailableDocs;

    public static bool DocExists(string docName) => AvailableDocs.ContainsKey(docName);

    public static string? GetDocDescription(string docName) =>
        AvailableDocs.TryGetValue(docName, out var desc) ? desc : null;

    public static async Task<string> ReadDocAsync(string docName)
    {
        var assembly = typeof(DocsReader).Assembly;
        var resourceName = $"Klacks.Docs.Resources.Docs.{docName}.md";

        await using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    public static string ReadDoc(string docName)
    {
        var assembly = typeof(DocsReader).Assembly;
        var resourceName = $"Klacks.Docs.Resources.Docs.{docName}.md";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            return string.Empty;
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
