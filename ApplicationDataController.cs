using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SoftwareTechnikProjekt.Data
{
    class ApplicationDataController
    {
        private static ApplicationDataController _instance;
        private static readonly object _padLock = new object();

        public ApplicationDataController()
        {
        }

        public static ApplicationDataController Instance
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationDataController();
                    }
                    return _instance;
                }
            }
        }

        class ModuleListSaveData
        {
            public IEnumerable<CollegeModule> modules;
            public string containingListBox;
        }

        public void SaveModulesStatus(IEnumerable<CollegeModule> open, IEnumerable<CollegeModule> planned, IEnumerable<CollegeModule> finished)
        {
            var openModules = new ModuleListSaveData { modules = open, containingListBox = ApplicationConstants.ModuleContainer.Open.ToString() };
            var plannedModules = new ModuleListSaveData { modules = planned, containingListBox = ApplicationConstants.ModuleContainer.Planned.ToString() };
            var finishedModules = new ModuleListSaveData { modules = finished, containingListBox = ApplicationConstants.ModuleContainer.Finished.ToString() };

            List<ModuleListSaveData> saveData = new List<ModuleListSaveData>();

            saveData.Add(openModules);
            saveData.Add(plannedModules);
            saveData.Add(finishedModules);

            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Studiendokumentation.json");

            using (StreamWriter file = File.CreateText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, saveData);
            }
        }
    }
}
