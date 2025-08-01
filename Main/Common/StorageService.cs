using System.IO;
using System.Text.Json;

namespace Main.Common
{
    internal class StorageService
    {
        private static readonly string FolderPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "WpfOtpAuthenicator");

        private static readonly string FilePath = Path.Combine(FolderPath, "otps.json");

        public static List<OtpAccount> Load()
        {
            if (!File.Exists(FilePath))
            {
                return [];
            }
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<OtpAccount>>(json) ?? [];
        }

        public static void Save(List<OtpAccount> otps)
        {
            Directory.CreateDirectory(FolderPath);
            var json = JsonSerializer.Serialize(otps, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
