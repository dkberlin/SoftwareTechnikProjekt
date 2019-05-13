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
            CourseDataHandler.Instance.SetupEvents();
            var modules = ModuleController.Instance.GetAllModules();
            foreach(var module in modules)
            {
                openModules.Items.Add(module.Title);
            }
        }

        private void AddToPlannedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = openModules.SelectedItem;
            OnModuleMoved?.Invoke(selectedItem, openModules, plannedModules);
        }

        private void RemoveFromPlanned_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = plannedModules.SelectedItem;
            OnModuleMoved?.Invoke(selectedItem, plannedModules, openModules);
        }

        internal void SetAlertLabelForModule(string moduleTitle, bool shouldShow)
        {
            var plannedModulesItemIndex = plannedModules.Items.IndexOf(moduleTitle);
            var finishedModulesItemIndex = finishedModules.Items.IndexOf(moduleTitle);
            var openModulesItemIndex = openModules.Items.IndexOf(moduleTitle);

            if (plannedModulesItemIndex >= 0)
            {
                AlertLabel.Content = $"! {moduleTitle} \nbaut auf einem nicht beendetem Modul auf.";
                AlertLabel.Visibility = shouldShow ? Visibility.Visible : Visibility.Hidden;
            }
            else if (finishedModulesItemIndex >= 0)
            {
                AlertLabel.Visibility = shouldShow ? Visibility.Visible : Visibility.Hidden;
            }
            else if (openModulesItemIndex >= 0)
            {
                AlertLabel.Content = "";
                AlertLabel.Visibility = shouldShow ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void AddToFinishedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = plannedModules.SelectedItem;
            OnModuleMoved?.Invoke(selectedItem, plannedModules, finishedModules);
        }

        private void RemoveFromFinished_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = finishedModules.SelectedItem;
            OnModuleMoved?.Invoke(selectedItem, finishedModules, plannedModules);
        }

        private void QuitAppButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
