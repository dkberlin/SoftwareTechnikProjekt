﻿using Newtonsoft.Json;
using SoftwareTechnikProjekt.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SoftwareTechnikProjekt
{
    internal class ModuleController
    {
        private CourseDataHandler _dataHandler;

        public event ModuleAddedToFinished OnFinishedModulesChange;
        public delegate void ModuleAddedToFinished(ListBoxItem selectedModule, bool addedToList);

        public ModuleController(CourseDataHandler dataHandler)
        {
            _dataHandler = dataHandler;
        }

        internal List<CollegeModule> FetchAllModules()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SoftwareTechnikProjekt.modules.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1")))
            {
                string result = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<CollegeModule>>(result);
            }
        }

        internal void SetupEvents()
        {
            MainWindow.AppWindow.OnModuleMoved += OnModuleHasBeenMoved;
        }

        private void OnModuleHasBeenMoved(ListBoxItem selectedModule, ListBox FromList, ListBox ToList)
        {
            //ToList.Items.Add(selectedModule);
            if (ToList == MainWindow.AppWindow.finishedModules)
            {
                OnFinishedModulesChange?.Invoke(selectedModule, true);
            }
            else if (FromList == MainWindow.AppWindow.finishedModules)
            {
                OnFinishedModulesChange?.Invoke(selectedModule, false);
            }

            var moduleData = GetCollegeModuleByListBoxItem(selectedModule);

            FromList.Items.Remove(selectedModule);
            AddModuleToListBox(selectedModule, ToList);

            CheckModuleForDependencies(moduleData, selectedModule, ToList);

            OnModuleUnselected();
        }

        public string GetModuleTitleFromListBoxItem(ListBoxItem selectedModule)
        {
            var title = selectedModule.ToString();
            var split = title.Split(':');
            return split[1].Substring(1);
        }

        internal void AddModuleToListBox(ListBoxItem module, ListBox listBox)
        {
            var moduleTitle = GetModuleTitleFromListBoxItem(module);
            var lbi = new ListBoxItem();

            lbi.Content = new TextBlock { Text = moduleTitle, TextWrapping = TextWrapping.Wrap };

            if (listBox == MainWindow.AppWindow.openModules)
            {
                lbi.Selected += OnModuleSelected;
                MainWindow.AppWindow.openModules.Items.Add(lbi);
            }
            else if (listBox == MainWindow.AppWindow.plannedModules)
            {
                MainWindow.AppWindow.plannedModules.Items.Add(lbi);
            }
            else
            {
                MainWindow.AppWindow.finishedModules.Items.Add(lbi);
            }
        }

        private void OnModuleUnselected()
        {
            MainWindow.AppWindow.ModuleInfoLabel.Visibility = Visibility.Hidden;
            MainWindow.AppWindow.ModuleInfoLabel.Text = String.Empty;
        }

        private void OnModuleSelected(object sender, RoutedEventArgs e)
        {
            var selectedItem = e.Source as ListBoxItem;

            var itemModuleInfo = GetCollegeModuleByListBoxItem(selectedItem);

            MainWindow.AppWindow.ModuleInfoLabel.Visibility = Visibility.Visible;
            MainWindow.AppWindow.ModuleInfoLabel.Text = itemModuleInfo.FurtherInfo;
        }

        private void CheckModuleForDependencies(CollegeModule movedModuleData, ListBoxItem movedModuleElement, ListBox ToList)
        {
            if (movedModuleData.DependandModules == null)
            {
                return;
            }

            var finishedModules = MainWindow.AppWindow.finishedModules.Items;

            if (ToList == MainWindow.AppWindow.plannedModules)
            {
                if (!finishedModules.Contains(movedModuleElement as object))
                {
                    foreach (var finishedModule in finishedModules)
                    {
                        var moduleListBoxItem = finishedModule as ListBoxItem;
                        var finishedModuleData = GetCollegeModuleByListBoxItem(moduleListBoxItem);

                        foreach (var dependandID in movedModuleData.DependandModules)
                        {
                            if (finishedModuleData.ID == dependandID)
                            {
                                return;
                            }
                        }
                    }

                    MainWindow.AppWindow.SetAlertLabelForModule(movedModuleElement, true);
                }
            }

            else if (ToList == MainWindow.AppWindow.openModules ||
                ToList == MainWindow.AppWindow.finishedModules)
            {
                MainWindow.AppWindow.SetAlertLabelForModule(movedModuleElement, false);
            }
        }

        internal ListBoxItem GenerateListBoxItemByModule(CollegeModule module)
        {
            return new ListBoxItem { Content = module.Title };
        }

        public CollegeModule GetCollegeModuleByListBoxItem(ListBoxItem module)
        {
            var moduleTitle = GetModuleTitleFromListBoxItem(module);

            List<CollegeModule> modules = GetAllModules();
            CollegeModule currentModule = modules.First(m => m.Title == moduleTitle);

            return currentModule;
        }

        public CollegeModule GetCollegeModuleById(int ID)
        {
            List<CollegeModule> modules = GetAllModules();
            CollegeModule currentModule = modules.First(m => m.ID == ID);

            return currentModule;
        }

        public List<CollegeModule> GetAllModules()
        {
            return _dataHandler.CollegeModules;
        }

        internal void UnloadAllEvents()
        {
            MainWindow.AppWindow.OnModuleMoved -= OnModuleHasBeenMoved;
        }
    }
}