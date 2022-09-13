using System.Globalization;

namespace SanyaLorik
{
    public static class NamingEditor
    {
        public static string Edit(string name) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.Replace('_', char.MinValue));
    }
}