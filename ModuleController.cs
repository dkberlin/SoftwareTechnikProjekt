using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace SoftwareTechnikProjekt
{
    internal class ModuleController
    {
        internal static void GetAllModules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SoftwareTechnikProjekt.modules.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                CollegeModule module = JsonConvert.DeserializeObject<CollegeModule>(result);

                System.Console.WriteLine("Title is : " + module.Title);
            }
        }
    }
}