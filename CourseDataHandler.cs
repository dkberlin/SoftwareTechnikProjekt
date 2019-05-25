using System;

namespace SoftwareTechnikProjekt
{
    internal class CourseDataHandler
    {
        private static CourseDataHandler _instance;
        private static readonly object _padLock = new object();

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
            //update progress bar
            var moduleCount = ModuleController.Instance.GetAllModules().Count;
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
            currentModule.Grade = chosenGrade;
            MainWindow.AppWindow.UpdateGradeForModule(currentModule, chosenGrade, true);
        }
    }
}