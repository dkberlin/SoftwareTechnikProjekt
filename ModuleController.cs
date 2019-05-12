using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

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

        internal void SetupEvents()
        {
            MainWindow.AppWindow.OnModuleMoved += OnModuleHasBeenMoved;
        }

        private void OnModuleHasBeenMoved(object selectedModule, ListBox FromList, ListBox ToList)
        {
            FromList.Items.Remove(selectedModule);
            ToList.Items.Add(selectedModule);

            var moduleTitle = selectedModule.ToString();
            var moduleData = GetCollegeModuleByTitle(moduleTitle);

            CheckModuleForDependencies(moduleData, selectedModule, ToList);
        }

        private void CheckModuleForDependencies(CollegeModule moduleData, object selectedModuleElement, ListBox ToList)
        {
            if (moduleData.DependandModules == null)
            {
                return;
            }

            if (ToList == MainWindow.AppWindow.plannedModules)
            {
                if (!MainWindow.AppWindow.finishedModules.Items.Contains(selectedModuleElement))
                {
                    MainWindow.AppWindow.SetAlertLabelForModule(selectedModuleElement.ToString(), true);
                }
            }
            else if (ToList == MainWindow.AppWindow.openModules ||
                ToList == MainWindow.AppWindow.finishedModules)
            {
                MainWindow.AppWindow.SetAlertLabelForModule(selectedModuleElement.ToString(), false);
            }
        }

        private CollegeModule GetCollegeModuleByTitle(string moduleTitle)
        {
            List<CollegeModule> modules = GetAllModules();
            CollegeModule currentModule = modules.First(m => m.Title == moduleTitle);

            return currentModule;
        }
    }
}