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
            ModuleController.Instance.OnModuleAddedToFinished += OnModuleAddedToFinished;
        }

        private void OnModuleAddedToFinished(object selectedModule)
        {
            //update progress bar
            var moduleCount = ModuleController.Instance.GetAllModules().Count;
            float relativeModuleAmount = 100f / moduleCount;
            var finishedModulesAmount = MainWindow.AppWindow.finishedModules.Items.Count;

            MainWindow.AppWindow.CompletedProgressBar.Value = finishedModulesAmount * relativeModuleAmount;
        }
    }
}