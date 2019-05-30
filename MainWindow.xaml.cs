using SoftwareTechnikProjekt.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SoftwareTechnikProjekt
{
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow { get; private set; }
        private static readonly object _padLock = new object();

        public event ModuleMoved OnModuleMoved;
        public delegate void ModuleMoved(object selectedModule, ListBox FromList, ListBox ToList);

        public event SaveButtonClicked OnSaveButtonClicked;
        public delegate void SaveButtonClicked(ListBox open, ListBox planned, ListBox finished);

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
            var modules = ModuleController.Instance.FetchAllModules();

            foreach(var module in modules)
            {
                openModules.Items.Add(module.Title);
                CourseDataHandler.Instance.AddModule(module);
            }
        }

        #region BUTTONS
        private void AddToPlannedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = openModules.SelectedItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, openModules, plannedModules);
            }
        }

        private void RemoveFromPlanned_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = plannedModules.SelectedItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, plannedModules, openModules);
            }
        }

        private void AddToFinishedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = plannedModules.SelectedItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, plannedModules, finishedModules);
            }
        }

        private void RemoveFromFinished_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = finishedModules.SelectedItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, finishedModules, plannedModules);
            }
        }
        #endregion

        internal void UpdateGradeForModule(CollegeModule currentModule, double chosenGrade, bool shouldAddGrade)
        {
            var listIndexToUpdate = finishedModules.Items.IndexOf(currentModule.Title);

            if (shouldAddGrade)
            {
                finishedModulesGrades.Items.Add(chosenGrade);
            }
            else
            {
                finishedModulesGrades.Items.RemoveAt(listIndexToUpdate);
            }
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

        private void QuitAppButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveButtonClicked != null)
            {
                OnSaveButtonClicked.Invoke(openModules, plannedModules, finishedModules);
            }
        }
    }
}
