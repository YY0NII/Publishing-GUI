//******************************************************
// File: MainWindow.xaml.cs
//
// Purpose: To establish the data context of the xaml GUI
// and house the code for the many processes of the GUI
//
// Written By: Jonathon Carrera
//
// Compiler: Visual Studio 2019
//
//****************************************************** 
using Microsoft.Win32;
using Publishing_Solution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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

namespace Publishing_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Publisher publisher;
        Book book;
        public MainWindow()
        {
            InitializeComponent();
            
            publisher = new Publisher();
            DataContext = publisher;
        }

        #region findBook Method
        //****************************************************
        // Method: findBook
        //
        // Purpose: To call the FindBook method on the publisher
        // object that serves as the DataContext of the window
        // and use the book object returned by that method to 
        // set the DataContext of Book_Title, Book_Price, and
        // List_Authors.
        //**************************************************** 
        private void findBook(object sender, RoutedEventArgs e)
        {
            book = publisher.FindBook(Target_Title.Text);

            //****************************************************
            // Here we are setting the individual DataContext of 
            // each of these controls to the book object that we
            // populated above. What information they display is
            // handled by databinding to properties of the book.
            //**************************************************** 
            Book_Title.DataContext = book;
            Book_Price.DataContext = book;
            List_Authors.DataContext = book;

        }
        #endregion

        #region openFile Method
        //****************************************************
        // Method: openFile
        //
        // Purpose: To find a json file that contains a publisher
        // object than use that object to populate the relevant
        // controls of the GUI; while clearing away the old data
        //**************************************************** 
        private void openFile(object sender, RoutedEventArgs e)
        {
            string filename = "";

            #region resources used to figure out open file dialog
            //****************************************************
            // Here are the resources I used to figure out how to
            // use the openFileDialog and some of it's properties
            //
            // https://stackoverflow.com/questions/10315188/open-file-dialog-and-select-a-file-using-wpf-controls-and-c-sharp
            // https://www.wpf-tutorial.com/dialogs/the-openfiledialog/
            // I used both this stackoverflow post and this tutorial 
            // to set up the OpenFileDialog obj set the filter, set
            // the initial directory, open, and use the dialog to 
            // let the user choose a file from the file explorer.
            //
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.filedialog?view=netcore-3.1
            // https://docs.microsoft.com/en-us/dotnet/api/system.io.directory.getcurrentdirectory?view=netcore-3.1
            // I used both of these microsoft documentation resources
            // to figure out how to set the initial directory of
            // my program to the bin/debug folder.
            //**************************************************** 
            #endregion

            #region Setting up open file dialog
            // Instantiates an OpenFileDialog object
            OpenFileDialog openFileDialog = new OpenFileDialog();
              
            // Sets the title of the explorer window
            openFileDialog.Title = "Open Publisher From JSON";
            
            // Filters out any file that is not a JSON file
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            
            //**************************************************** 
            // Sets the initial directory to the current directory 
            // of the app (Debug in this case)
            //**************************************************** 
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            #endregion

            #region using open file dialog
            //**************************************************** 
            // The way I understand this line is that if the user
            // finds and opens a file in the explorer then true is 
            // returned by the showDialog() which then allows us to
            // access the file name, store it and then read from it
            //****************************************************
            if (openFileDialog.ShowDialog() == true) 
            {
                filename = openFileDialog.FileName;
                File_Name.Text = filename;

                FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read);

                DataContractJsonSerializer inputSerializer;
                inputSerializer = new DataContractJsonSerializer(typeof(Publisher));

                publisher = (Publisher)inputSerializer.ReadObject(reader);
                reader.Dispose();

                DataContext = publisher;

                // Clears the old data of these controls if new data is found
                Target_Title.Clear();
                Book_Title.Clear();
                Book_Price.Clear();
                List_Authors.DataContext = null;
            }
            #endregion
        }
        #endregion
    }
}
