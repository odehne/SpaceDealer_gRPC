using System;
using System.IO;

namespace SpaceDealerService
{
	public class GamePersistor
	{
        public static bool SaveFile(string content, string filename)
        {
            try
            {
                using var txt = new StreamWriter(filename);
                txt.WriteLine(content);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to save custom parameters file [{filename}]. {e.Message}");
                return false;
            }
            return true;
        }

        public static string LoadFile(string filename)
        {
            if (!File.Exists(filename))
                return string.Empty;
            try
            {
                using var txt = new StreamReader(filename);
                return txt.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
