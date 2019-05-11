using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Controls;

namespace SoftwareTechnikProjekt
{
    internal class ModuleController
    {
        private static ModuleController _instance;
        private static readonly object _padLock = new object();

        public ModuleController()
        {
        }

        public static ModuleController Instance
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ModuleController();
                    }
                    return _instance;
                }
            }
        }

        internal List<CollegeModule> GetAllModules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SoftwareTechnikProjekt.modules.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<CollegeModule>>(result);
            }
        }

        internal void MoveToList(object selectedItem, ListBox originalBox, ListBox newBox)
        {
            newBox.Items.Add(selectedItem);
            originalBox.Items.Remove(selectedItem);
        }

        internal void SetupEvents()
        {
            MainWindow.AppWindow.OnModuleMoved += OnModuleHasBeenMoved;
        }

        private void OnModuleHasBeenMoved(object selectedModule, ListBox FromList, ListBox ToList)
        {
            ToList.Items.Add(selectedModule);
            FromList.Items.Remove(selectedModule);
        }
    }
}