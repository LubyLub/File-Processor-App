﻿using System;
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
using File_Processor.Controllers;

namespace File_Processor.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        //private CategoryController categoryController;
        //private CategoryClassificationController categoryClassificationController;
        public Page1()
        {
            InitializeComponent();
            //categoryController = new CategoryController(this);
        }

        //private void Add_Category(object sender, RoutedEventArgs e)
        //{
        //    string givenPath = categoryFilePath.Text; // Grabs Text in a TextBox named manualFilePath"
        //    string category = categoryName.Text;
        //    // Adding Category
        //    int result = categoryController.addCategory(givenPath, category);
        //    if (result == 1) { MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Successfuly Added"); }
        //    else if (result == 2) { MessageBox.Show("Category \'" + category + ":" + givenPath + "\' Updated"); }
        //    else { MessageBox.Show("Category not added"); }
        //}

        private void Change_To_Setting_Page(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow) Window.GetWindow(this);
            mainWindow.MainFrame.Source = new Uri("Page2.xaml", UriKind.Relative);
            //NavigationService.Navigate(new Uri("Page2.xaml", UriKind.Relative));
        }
    }
}
