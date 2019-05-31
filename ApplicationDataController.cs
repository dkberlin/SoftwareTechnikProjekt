using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace SoftwareTechnikProjekt.Data
{
    class ApplicationDataController
    {
        private CourseDataHandler _courseDataHandler;

        public CourseDataHandler CourseDataHandler { get => _courseDataHandler; internal set => _courseDataHandler = value; }

        public ApplicationDataController()
        {
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

        internal void LoadModulesStatus(ListBox open, ListBox planned, ListBox finished)
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Studiendokumentation.json");
            List<ModuleListSaveData> savedData = new List<ModuleListSaveData>();

            using (StreamReader file = File.OpenText(fileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                savedData = (List<ModuleListSaveData>)serializer.Deserialize(file, typeof(List<ModuleListSaveData>));
            }

            var openModulesData = savedData.Where(d => d.containingListBox == ApplicationConstants.ModuleContainer.Open.ToString());
            var plannedModulesData = savedData.Where(d => d.containingListBox == ApplicationConstants.ModuleContainer.Planned.ToString());
            var finishedModulesData = savedData.Where(d => d.containingListBox == ApplicationConstants.ModuleContainer.Finished.ToString());

            AddLoadedModulesToListbox(open, openModulesData);
            AddLoadedModulesToListbox(planned, plannedModulesData);
            AddLoadedModulesToListbox(finished, finishedModulesData, true);
        }

        private void AddLoadedModulesToListbox(ListBox containingListBox, IEnumerable<ModuleListSaveData> modulesData, bool shouldAddGrade = false)
        {
            foreach (var data in modulesData)
            {
                foreach (var module in data.modules)
                {
                    containingListBox.Items.Add(module.Title);

                    if (shouldAddGrade)
                    {
                        _courseDataHandler.UpdateModuleGrade(module, module.Grade);
                        _courseDataHandler.UpdateProgressBar(true);
                    }
                }
            }
        }
    }
}
