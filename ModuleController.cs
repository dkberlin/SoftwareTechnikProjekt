using Newtonsoft.Json;
using SoftwareTechnikProjekt.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls;

namespace SoftwareTechnikProjekt
{
    internal class ModuleController
    {
        private CourseDataHandler _dataHandler;

        public event ModuleAddedToFinished OnFinishedModulesChange;
        public delegate void ModuleAddedToFinished(object selectedModule, bool addedToList);

        public ModuleController(CourseDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        internal List<CollegeModule> FetchAllModules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SoftwareTechnikProjekt.modules.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1")))
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
            ToList.Items.Add(selectedModule);

            var moduleTitle = selectedModule.ToString();
            var moduleData = GetCollegeModuleByTitle(moduleTitle);

            CheckModuleForDependencies(moduleData, selectedModule, ToList);

            if (ToList == MainWindow.AppWindow.finishedModules)
            {
                OnFinishedModulesChange?.Invoke(selectedModule, true);
            }
            else if (FromList == MainWindow.AppWindow.finishedModules)
            {
                OnFinishedModulesChange?.Invoke(selectedModule, false);
            }

            FromList.Items.Remove(selectedModule);
        }

        private void CheckModuleForDependencies(CollegeModule movedModuleData, object movedModuleElement, ListBox ToList)
        {
            if (movedModuleData.DependandModules == null)
            {
                return;
            }

            var finishedModules = MainWindow.AppWindow.finishedModules.Items;

            if (ToList == MainWindow.AppWindow.plannedModules)
            {
                if (!finishedModules.Contains(movedModuleElement))
                {
                    foreach (var finishedModule in finishedModules)
                    {
                        var finishedModuleData = GetCollegeModuleByTitle(finishedModule.ToString());

                        foreach (var dependandID in movedModuleData.DependandModules)
                        {
                            if (finishedModuleData.ID == dependandID)
                            {
                                return;
                            }
                        }
                    }

                    MainWindow.AppWindow.SetAlertLabelForModule(movedModuleElement.ToString(), true);
                }
            }

            else if (ToList == MainWindow.AppWindow.openModules ||
                ToList == MainWindow.AppWindow.finishedModules)
            {
                MainWindow.AppWindow.SetAlertLabelForModule(movedModuleElement.ToString(), false);
            }
        }

        public CollegeModule GetCollegeModuleByTitle(string moduleTitle)
        {
            List<CollegeModule> modules = GetAllModules();
            CollegeModule currentModule = modules.First(m => m.Title == moduleTitle);

            return currentModule;
        }

        public List<CollegeModule> GetAllModules()
        {
            return _dataHandler.CollegeModules;
        }

        internal void UnloadAllEvents()
        {
            MainWindow.AppWindow.OnModuleMoved -= OnModuleHasBeenMoved;
        }
    }
}