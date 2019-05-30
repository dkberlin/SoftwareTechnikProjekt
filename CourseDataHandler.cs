using SoftwareTechnikProjekt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace SoftwareTechnikProjekt.Data
{
    internal class CourseDataHandler
    {
        private static CourseDataHandler _instance;
        private static readonly object _padLock = new object();
        private List<CollegeModule> _collegeModules;
        public List<CollegeModule> CollegeModules { get => _collegeModules; }



        public CourseDataHandler()
        {
        }

        public static CourseDataHandler Instance
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new CourseDataHandler();
                    }
                    return _instance;
                }
            }
        }

        internal void SetupEvents()
        {
            ModuleController.Instance.OnFinishedModulesChange += OnFinishedModulesChange;
            MainWindow.AppWindow.OnSaveButtonClicked += PrepareModuleListsForSaving;
        }

        internal void AddModule(CollegeModule module)
        {
            if (_collegeModules == null)
            {
                _collegeModules = new List<CollegeModule>();
            }

            _collegeModules.Add(module);
        }

        private void PrepareModuleListsForSaving(ListBox open, ListBox planned, ListBox finished)
        {
            List<CollegeModule> openModules = new List<CollegeModule>();
            List<CollegeModule> plannedModules = new List<CollegeModule>();
            List<CollegeModule> finishedModules = new List<CollegeModule>();

            foreach (var entry in open.Items)
            {
                var moduleData = ModuleController.Instance.GetCollegeModuleByTitle(entry.ToString());
                openModules.Add(moduleData);
            }

            foreach (var entry in planned.Items)
            {
                var moduleData = ModuleController.Instance.GetCollegeModuleByTitle(entry.ToString());
                plannedModules.Add(moduleData);
            }

            foreach (var entry in finished.Items)
            {
                var moduleData = ModuleController.Instance.GetCollegeModuleByTitle(entry.ToString());
                finishedModules.Add(moduleData);
            }

            ApplicationDataController.Instance.SaveModulesStatus(openModules, plannedModules, finishedModules);
        }

        private void OnFinishedModulesChange(object selectedModule, bool moduleAddedToList)
        {
            //update progress bar
            var moduleCount = _collegeModules.Count;
            float relativeModuleAmount = 100f / moduleCount;
            var finishedModulesAmount = MainWindow.AppWindow.finishedModules.Items.Count;

            MainWindow.AppWindow.CompletedProgressBar.Value = finishedModulesAmount * relativeModuleAmount;

            var moduleData = ModuleController.Instance.GetCollegeModuleByTitle(selectedModule.ToString());

            if (moduleAddedToList)
            {
                var popUp = new GradePopUp();
                popUp.SetupAndShow(moduleData);
            }
            else
            {
                MainWindow.AppWindow.UpdateGradeForModule(moduleData, 0, false);
            }
        }

        internal void UpdateModuleGrade(CollegeModule currentModule, double chosenGrade)
        {
            var updatedModule = _collegeModules.First(m => m.Title == currentModule.Title);
            updatedModule.Grade = chosenGrade;

            MainWindow.AppWindow.UpdateGradeForModule(currentModule, chosenGrade, true);
        }
    }
}