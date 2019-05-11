using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SoftwareTechnikProjekt
{
    public partial class MainWindow : Window
    {
        //private static MainWindow _instance;
        public static MainWindow AppWindow { get; private set; }
        private static readonly object _padLock = new object();

        public event ModuleMoved OnModuleMoved;
        public delegate void ModuleMoved(object selectedModule, ListBox FromList, ListBox ToList);

        //public static MainWindow Instance
        //{
        //    get
        //    {
        //        lock (_padLock)
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new MainWindow();
        //            }
        //            return _instance;
        //        }
        //    }
        //}

        public MainWindow()
        {
            AppWindow = this;
            InitializeComponent();
            SetupOnStartApplication();
        }

        private void SetupOnStartApplication()
        {
            //TODO: check for save
            ModuleController.Instance.SetupEvents();
            var modules = ModuleController.Instance.GetAllModules();
            foreach(var module in modules)
            {
                openModules.Items.Add(module.Title);
            }
        }

        private void AddToPlannedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = openModules.SelectedItem;
            //ModuleController.MoveToList(selectedItem, openModules, plannedModules);
            OnModuleMoved?.Invoke(selectedItem, openModules, plannedModules);
        }

        private void RemoveFromPlanned_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = plannedModules.SelectedItem;
            //ModuleController.Instance.MoveToList(selectedItem, plannedModules, openModules);
            OnModuleMoved?.Invoke(selectedItem, plannedModules, openModules);
        }
    }
}
