using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace TiaV21ProjectCreator
{
    public partial class MainWindow : Window
    {
        private readonly TiaProjectService _tiaProjectService = new TiaProjectService();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Proje Klasörünü Seçin (Dosya adı önemli değil)",
                FileName = "KlasorSec",
                Filter = "Klasör Seçimi|*.none"
            };

            if (dialog.ShowDialog() == true)
            {
                string selectedFolder = System.IO.Path.GetDirectoryName(dialog.FileName);
                if (!string.IsNullOrEmpty(selectedFolder))
                {
                    TxtTargetPath.Text = selectedFolder;
                }
            }
        }

        private async void BtnCreateProject_Click(object sender, RoutedEventArgs e)
        {
            string projectName = TxtProjectName.Text.Trim();
            string targetPath = TxtTargetPath.Text.Trim();

            BtnCreateProject.IsEnabled = false;
            TxtStatus.Text = "TIA Portal başlatılıyor ve proje oluşturuluyor...";

            try
            {
                string createdPath = await Task.Run(() =>
                    _tiaProjectService.CreateNewProject(projectName, targetPath, openWithUi: true));

                TxtStatus.Text = $"Proje başarıyla oluşturuldu:\n{createdPath}";
            }
            catch (Exception ex)
            {
                TxtStatus.Text = $"Hata: {ex.Message}";
            }
            finally
            {
                BtnCreateProject.IsEnabled = true;
            }
        }
    }
}