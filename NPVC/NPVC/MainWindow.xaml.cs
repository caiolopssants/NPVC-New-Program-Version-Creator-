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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Reflection;





namespace NewProgramVersionCreator
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string,string> TextBox_InitalText_Dictionary { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            SetInitialText();
            SetControlsEvents();

            //SetTempTextBoxText();
        }

        #region Main Window Set Methods
        private void SetInitialText()
        {
            TextBox_InitalText_Dictionary = new Dictionary<string, string>
            {
                { allPrograms_DirectoryPath_TextBox.Name, allPrograms_DirectoryPath_TextBox.Text },
                { referenceVersion_FolderName_TextBox.Name, referenceVersion_FolderName_TextBox.Text },
                { newVersion_FolderName_TextBox.Name, newVersion_FolderName_TextBox.Text },
                { lOG_FilePath_TextBox.Name, lOG_FilePath_TextBox.Text },
                { referenceVersion_Dlls_DirectoryPath_TextBox.Name, referenceVersion_Dlls_DirectoryPath_TextBox.Text },
                { newVersion_Dlls_DirectoryPath_TextBox.Name, newVersion_Dlls_DirectoryPath_TextBox.Text },
                { newVersion_ResourceElements_DirectoryPath_TextBox.Name, newVersion_ResourceElements_DirectoryPath_TextBox.Text }
            };
        }
        private void SetControlsEvents()
        {
            allPrograms_DirectoryPath_TextBox.MouseDoubleClick += new MouseButtonEventHandler(GetDirectoryPath_TextBox_MouseDoubleClick);
            allPrograms_DirectoryPath_TextBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            allPrograms_DirectoryPath_TextBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);
            allPrograms_DirectoryPath_TextBox.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
            allPrograms_DirectoryPath_TextBox.KeyDown += new KeyEventHandler(GetDirectoryPath_TextBox_KeyDown);

            referenceVersion_FolderName_TextBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            referenceVersion_FolderName_TextBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);
            referenceVersion_FolderName_TextBox.GotFocus += new RoutedEventHandler(TextBox_GotFocus);

            newVersion_FolderName_TextBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            newVersion_FolderName_TextBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);
            newVersion_FolderName_TextBox.GotFocus += new RoutedEventHandler(TextBox_GotFocus);

            lOG_FilePath_TextBox.MouseDoubleClick += new MouseButtonEventHandler(GetFilePath_TextBox_MouseDoubleClick);
            lOG_FilePath_TextBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            lOG_FilePath_TextBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);
            lOG_FilePath_TextBox.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
            lOG_FilePath_TextBox.KeyDown += new KeyEventHandler(GetFilePath_TextBox_KeyDown);

            referenceVersion_Dlls_DirectoryPath_TextBox.MouseDoubleClick += new MouseButtonEventHandler(GetDirectoryPath_TextBox_MouseDoubleClick);
            referenceVersion_Dlls_DirectoryPath_TextBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            referenceVersion_Dlls_DirectoryPath_TextBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);
            referenceVersion_Dlls_DirectoryPath_TextBox.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
            referenceVersion_Dlls_DirectoryPath_TextBox.KeyDown += new KeyEventHandler(GetDirectoryPath_TextBox_KeyDown);

            newVersion_Dlls_DirectoryPath_TextBox.MouseDoubleClick += new MouseButtonEventHandler(GetDirectoryPath_TextBox_MouseDoubleClick);
            newVersion_Dlls_DirectoryPath_TextBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            newVersion_Dlls_DirectoryPath_TextBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);
            newVersion_Dlls_DirectoryPath_TextBox.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
            newVersion_Dlls_DirectoryPath_TextBox.KeyDown += new KeyEventHandler(GetDirectoryPath_TextBox_KeyDown);

            newVersion_ResourceElements_DirectoryPath_TextBox.MouseDoubleClick += new MouseButtonEventHandler(GetDirectoryPath_TextBox_MouseDoubleClick);
            newVersion_ResourceElements_DirectoryPath_TextBox.MouseEnter += new MouseEventHandler(TextBox_MouseEnter);
            newVersion_ResourceElements_DirectoryPath_TextBox.MouseLeave += new MouseEventHandler(TextBox_MouseLeave);
            newVersion_ResourceElements_DirectoryPath_TextBox.GotFocus += new RoutedEventHandler(TextBox_GotFocus);
            newVersion_ResourceElements_DirectoryPath_TextBox.KeyDown += new KeyEventHandler(GetDirectoryPath_TextBox_KeyDown);

            generateNewVersion_Button.Click += new RoutedEventHandler(CreatNewVersion_Button_Click);
        }

        private void SetTempTextBoxText()
        {
            allPrograms_DirectoryPath_TextBox.Text = $@"G:\programas alheio\CreatNewProgramVersion\CreatNewProgramVersion\CONJUNTO_TESTE";
            referenceVersion_FolderName_TextBox.Text = $@"21";
            newVersion_FolderName_TextBox.Text = $@"22";
            lOG_FilePath_TextBox.Text = $@"G:\programas alheio\CreatNewProgramVersion\CreatNewProgramVersion\CONJUNTO_TESTE\__Erros de compilação__\22\apps com erro de compilação 2022 07_04_2021.txt";
            referenceVersion_Dlls_DirectoryPath_TextBox.Text = $@"G:\programas alheio\CreatNewProgramVersion\CreatNewProgramVersion\dll's Revit_R\21";
            newVersion_Dlls_DirectoryPath_TextBox.Text = $@"G:\programas alheio\CreatNewProgramVersion\CreatNewProgramVersion\dll's Revit_R\22";
            newVersion_ResourceElements_DirectoryPath_TextBox.Text = string.Empty;
        }
        #endregion

        #region Main Window Events
        private void GetDirectoryPath_TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog oFD = new OpenFileDialog()
            {
                FileName = "Press the button -Open- when reach the directory _ if necessary select a random file inside of directory path",
                Multiselect = false,
                Title = "Search desired directory path",
                CheckFileExists = false,
                CheckPathExists = true
            };
            
            bool? result = oFD.ShowDialog();
            if (result != null && result.GetValueOrDefault())
            {
                ((TextBox)sender).Text = $@"{System.IO.Path.GetDirectoryName(oFD.FileName)}";
            }
        }
        private void GetFilePath_TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string version = (TextBox_InitalText_Dictionary[((TextBox)sender).Name]!=((TextBox)sender).Text) ? ((TextBox)sender).Text : "???";
            SaveFileDialog sFD = new SaveFileDialog()
            {   
                FileName = $"apps with compilation error {version} {DateTime.Now.ToShortDateString().Replace(@"\","_")}",                
                Title = "Creating/ Chosing file address to save",
                CheckFileExists = false,
                CheckPathExists = true,
                Filter = "Text File|*.txt|All|*",                
            };

            bool? result = sFD.ShowDialog();
            if (result != null && result.GetValueOrDefault())
            {
                ((TextBox)sender).Text = sFD.FileName;
            }
        }
        private void GetDirectoryPath_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetDirectoryPath_TextBox_MouseDoubleClick(sender, null);
            }
        }
        private void GetFilePath_TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GetFilePath_TextBox_MouseDoubleClick(sender, null);
            }
        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if(TextBox_InitalText_Dictionary[((TextBox)sender).Name] == ((TextBox)sender).Text)
            {
                ((TextBox)sender).Text = string.Empty;
                ((TextBox)sender).Focus();
            }
        }
        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrEmpty(((TextBox)sender).Text) || string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                ((TextBox)sender).Text = TextBox_InitalText_Dictionary[((TextBox)sender).Name];
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {            
            if (TextBox_InitalText_Dictionary[((TextBox)sender).Name] == ((TextBox)sender).Text)
            {
                ((TextBox)sender).Text = string.Empty;                
            }
        }
        
        private void CreatNewVersion_Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime startTime = DateTime.Now;

            NPVC.NPVCElement element = new NPVC.NPVCElement
                (
                    allPrograms_DirectoryPath_TextBox.Text,
                    referenceVersion_FolderName_TextBox.Text,
                    newVersion_FolderName_TextBox.Text,
                    lOG_FilePath_TextBox.Text,
                    referenceVersion_Dlls_DirectoryPath_TextBox.Text,
                    newVersion_Dlls_DirectoryPath_TextBox.Text,
                    newVersion_ResourceElements_DirectoryPath_TextBox.Text
                );          
            
            element.CreatNewProgramVersion();
            element.ChangeDllReferencesOfNewVersion();
            element.ChangeResourcesOfNewVersion();
            element.CompileNewVersionPrograms();

            DateTime endTime = DateTime.Now;                      
            MessageBox.Show($"Processo concluido.\n\nInício: {startTime.ToString()}\nFim: {endTime.ToString()}", "Processo Concluido", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
        #endregion
    }




}
