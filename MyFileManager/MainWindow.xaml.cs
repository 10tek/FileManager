using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MyFileManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private string filePath = string.Empty;
        private string fileName = string.Empty;
        private bool isFile = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFileBtnClick(object sender, RoutedEventArgs e)
        {
            var res = openFileDialog.ShowDialog();
            if (!res.Value)
                return;
            filePath = openFileDialog.FileName;
            fileName = System.IO.Path.GetFileName(filePath);
            infoL.Content = "Файл выбран";
            isFile = true;
        }

        private void SelectFolderBtnClick(object sender, RoutedEventArgs e)
        {
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = folderDialog.ShowDialog();
                if (result.ToString() == "OK")
                {
                    filePath = folderDialog.SelectedPath;
                    infoL.Content = "Каталог выбрана";
                }
            }
            fileName = System.IO.Path.GetFileName(filePath);
            isFile = false;
        }

        private void MoveBtnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                infoL.Content = "Выберите каталог или файл";
                return;
            }
            infoL.Content = "Куда хотите переместить?";
            var destFilePath = string.Empty;
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = folderDialog.ShowDialog();
                if (result.ToString() == "OK")
                    destFilePath = folderDialog.SelectedPath;
            }
            try
            {
                Directory.Move(filePath, $"{destFilePath}\\{fileName}");
                infoL.Content = "Успешно перемещен";
            }
            catch (Exception ex)
            {
                infoL.Content = "Произошла ошибка!";
                MessageBox.Show(ex.Message);
            }

        }

        private void CopyBtnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                infoL.Content = "Выберите каталог или файл";
                return;
            }
            infoL.Content = "Куда хотите копировать?";
            var destFilePath = string.Empty;
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = folderDialog.ShowDialog();
                if (result.ToString() == "OK")
                    destFilePath = folderDialog.SelectedPath;
            }
            try
            {
                if (isFile)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(filePath, $"{destFilePath}\\{fileName}");
                }
                else
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(filePath, $"{destFilePath}\\{fileName}");
                }
                infoL.Content = "Успешно cкопирован";
            }
            catch (Exception ex)
            {
                infoL.Content = "Произошла ошибка!";
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBtnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                infoL.Content = "Выберите каталог или файл";
                return;
            }
            try
            {
                if (isFile)
                    File.Delete(filePath);
                else
                    Directory.Delete(filePath, true);
                infoL.Content = (isFile ? "Файл" : "Каталог") + " удален";
            }
            catch (Exception ex)
            {
                infoL.Content = "Произошла ошибка!";
                MessageBox.Show(ex.Message);
            }
        }
    }
}
