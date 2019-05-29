using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareTechnikProjekt
{
    internal class CourseDataHandler
    {
        private static CourseDataHandler _instance;
        private static readonly object _padLock = new object();
        private List<CollegeModule> _finishedModules;



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
        }

        private void OnFinishedModulesChange(object selectedModule, bool moduleAddedToList)
        {
            var moduleData = ModuleController.Instance.GetCollegeModuleByTitle(selectedModule.ToString());

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
            MainWindow.AppWindow.UpdateGradeForModule(currentModule, chosenGrade, true);
        }
    }
}