using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedLinesDotterGUI
{
    internal static class JsonWriter
    {
        internal static void Write(LablelParams lablel)
        {
            var path = GetPath();
            var json = GetJson(lablel);

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] array = new byte[json.Length];
                array = Encoding.UTF8.GetBytes(json);
                stream.Write(array, 0, array.Length);
            }
        }

        private static string GetPath()
        {
            var directory = Directory.GetCurrentDirectory();
            var fileName = "LabelParametres.Json";
            var path = Path.Combine(directory, fileName);

            return path;
        }

        private static string GetJson(LablelParams lablel)
        {
            return JsonConvert.SerializeObject(lablel);
        }
    }
}
