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
        private List<CollegeModule> _finishedModules;
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
            var moduleData = ModuleController.Instance.GetCollegeModuleByTitle(selectedModule.ToString());
            //update progress bar
            var moduleCount = _collegeModules.Count;
            float relativeModuleAmount = 100f / moduleCount;
            var finishedModulesAmount = MainWindow.AppWindow.finishedModules.Items.Count;

            UpdateProgressBar(moduleAddedToList);
            ShowGradePopupIfNecessary(moduleData, moduleAddedToList);

            if (!moduleAddedToList)
            {
                UpdateAvgGrade(moduleData, moduleAddedToList);
            }
        }

        private void UpdateAvgGrade(CollegeModule moduleData, bool shouldAddGrade)
        {
            if (_finishedModules == null)
            {
                _finishedModules = new List<CollegeModule>();
            }

            if (shouldAddGrade)
            {
                _finishedModules.Add(moduleData);
            }
            else
            {
                var moduleToRemove = _finishedModules.First(e => e.Title == moduleData.Title);
                _finishedModules.Remove(moduleToRemove);
            }

            List<double> gradesList = new List<double>();

            foreach (var module in _finishedModules)
            {
                var amountOfEntries = module.Credits == 15 ? 3 : 1;

                for (int i = 0; i < amountOfEntries; i++)
                {
                    gradesList.Add(module.Grade);
                }
            }

            var avgGrade = gradesList.Sum(x => Convert.ToDouble(x)) / gradesList.Count;
            MainWindow.AppWindow.avgGradeLabel.Content = Math.Round(avgGrade, 1);
        }

        private static void ShowGradePopupIfNecessary(CollegeModule selectedModule, bool moduleAddedToList)
        {
            if (moduleAddedToList)
            {
                var popUp = new GradePopUp();
                popUp.SetupAndShow(selectedModule);
            }
            else
            {
                MainWindow.AppWindow.UpdateGradeForModule(selectedModule, 0, false);
            }
        }

        private static void UpdateProgressBar(bool moduleAddedToList)
        {
            var moduleCount = ModuleController.Instance.GetAllModules().Count;
            float relativeModuleAmount = 100f / moduleCount;
            var finishedModulesAmount = MainWindow.AppWindow.finishedModules.Items.Count;

            if (!moduleAddedToList)
            {
                finishedModulesAmount--;
            }

            MainWindow.AppWindow.CompletedProgressBar.Value = finishedModulesAmount * relativeModuleAmount;
        }

        internal void UpdateModuleGrade(CollegeModule currentModule, double chosenGrade)
        {
            currentModule.Grade = chosenGrade;
            UpdateAvgGrade(currentModule, true);
            var updatedModule = _collegeModules.First(m => m.Title == currentModule.Title);
            updatedModule.Grade = chosenGrade;

            MainWindow.AppWindow.UpdateGradeForModule(currentModule, chosenGrade, true);
        }
    }
}