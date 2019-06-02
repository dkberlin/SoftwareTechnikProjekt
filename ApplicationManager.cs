using SoftwareTechnikProjekt.Data;

namespace SoftwareTechnikProjekt
{
    class ApplicationManager
    {
        private static ApplicationManager _instance;
        private static readonly object _padLock = new object();
        private CourseDataHandler _dataHandler;
        private ModuleController _moduleController;
        public CourseDataHandler DataHandler { get => _dataHandler; }

        public ApplicationManager()
        {
        }

        public static ApplicationManager Instance
        {
            get
            {
                lock (_padLock)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationManager();
                    }
                    return _instance;
                }
            }
        }

        internal void SetupApplication()
        {
            CourseDataHandler dataHandler = new CourseDataHandler();
            ModuleController moduleController = new ModuleController(dataHandler);
            ApplicationDataController dataController = new ApplicationDataController(moduleController);
            dataHandler.ApplicationDataController = dataController;
            dataHandler.ModuleController = moduleController;
            dataController.CourseDataHandler = dataHandler;

            moduleController.SetupEvents();
            dataHandler.SetupEvents();

            var modules = moduleController.FetchAllModules();

            foreach (var module in modules)
            {
                var openModulesListbox = MainWindow.AppWindow.openModules;
                var moduleListBoxItem = moduleController.GenerateListBoxItemByModule(module);
                moduleController.AddModuleToListBox(moduleListBoxItem, openModulesListbox);
                //MainWindow.AppWindow.openModules.Items.Add(module.Title);
                dataHandler.AddModule(module);
            }

            _dataHandler = dataHandler;
            _moduleController = moduleController;
        }

        internal void UnloadAllEvents()
        {
            _dataHandler.UnloadAllEvents();
            _moduleController.UnloadAllEvents();
        }
    }
}
