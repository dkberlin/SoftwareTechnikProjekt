using SoftwareTechnikProjekt.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SoftwareTechnikProjekt
{
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow { get; private set; }
        private static readonly object _padLock = new object();

        public event ModuleMoved OnModuleMoved;
        public delegate void ModuleMoved(ListBoxItem selectedModule, ListBox FromList, ListBox ToList);

        public event SaveButtonClicked OnSaveButtonClicked;
        public delegate void SaveButtonClicked(ListBox open, ListBox planned, ListBox finished);

        public event LoadButtonClicked OnLoadButtonClicked;
        public delegate void LoadButtonClicked(ListBox open, ListBox planned, ListBox finished);

        public MainWindow()
        {
            AppWindow = this;
            InitializeComponent();
            SetupOnStartApplication();
        }

        private void SetupOnStartApplication()
        {
            AlertLabel.Visibility = Visibility.Hidden;
            ModuleInfoLabel.Visibility = Visibility.Hidden;
            ApplicationManager.Instance.SetupApplication();
            //ModuleController.Instance.SetupEvents();
            //CourseDataHandler.Instance.SetupEvents();
            //var modules = ModuleController.Instance.FetchAllModules();

            //foreach(var module in modules)
            //{
            //    openModules.Items.Add(module.Title);
            //    CourseDataHandler.Instance.AddModule(module);
            //}
        }

        #region BUTTONS
        private void AddToPlannedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = openModules.SelectedItem as ListBoxItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, openModules, plannedModules);
            }
        }

        private void RemoveFromPlanned_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = plannedModules.SelectedItem as ListBoxItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, plannedModules, openModules);
            }
        }

        private void AddToFinishedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = plannedModules.SelectedItem as ListBoxItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, plannedModules, finishedModules);
            }
        }

        private void RemoveFromFinished_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = finishedModules.SelectedItem as ListBoxItem;

            if (selectedItem != null)
            {
                OnModuleMoved?.Invoke(selectedItem, finishedModules, plannedModules);
            }
        }
        #endregion

        internal void UpdateGradeForModule(CollegeModule currentModule, double chosenGrade, bool shouldAddGrade)
        {
            object itemToUpdate = null;

            foreach (var item in finishedModules.Items)
            {
                var listBoxItem = item as ListBoxItem;

                if (listBoxItem.Content == currentModule.Title)
                {
                    itemToUpdate = item;
                    break;
                }
            }

            var listIndexToUpdate = finishedModules.Items.IndexOf(itemToUpdate);

            if (shouldAddGrade)
            {
                finishedModulesGrades.Items.Add(chosenGrade);
            }
            else
            {
                finishedModulesGrades.Items.RemoveAt(listIndexToUpdate);
            }
        }

        internal void SetAlertLabelForModule(ListBoxItem module, bool shouldShow)
        {
            var plannedModulesItemIndex = plannedModules.Items.IndexOf(module);
            var finishedModulesItemIndex = finishedModules.Items.IndexOf(module);
            var openModulesItemIndex = openModules.Items.IndexOf(module);

            var moduleData = ApplicationManager.Instance.DataHandler.ModuleController.GetCollegeModuleByTitle(module.Content.ToString());
            var dependandModules = new List<CollegeModule>();

            foreach (var dep in moduleData.DependandModules)
            {
                dependandModules.Add(ApplicationManager.Instance.DataHandler.ModuleController.GetCollegeModuleById(dep));
            }

            var depModString = "";

            foreach (var mod in dependandModules)
            {
                depModString += mod.Title +"\n";
            }

            if (plannedModulesItemIndex >= 0)
            {
                AlertLabel.Content = $"! {module.Content} \nbaut auf nicht beendeten Modulen auf:\n{depModString}";

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
            ApplicationManager.Instance.UnloadAllEvents();
            //CourseDataHandler.Instance.UnloadAllEvents();
            //ModuleController.Instance.UnloadAllEvents();
            Application.Current.Shutdown();
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveButtonClicked != null)
            {
                OnSaveButtonClicked.Invoke(openModules, plannedModules, finishedModules);
            }
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnLoadButtonClicked != null)
            {
                OnLoadButtonClicked.Invoke(openModules, plannedModules, finishedModules);
            }
        }
    }
}
