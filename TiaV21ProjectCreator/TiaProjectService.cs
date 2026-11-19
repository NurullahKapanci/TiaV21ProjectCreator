using System;
using System.IO;
using Siemens.Engineering;

namespace TiaV21ProjectCreator
{
    public class TiaProjectService
    {
        public string CreateNewProject(string projectName, string targetDirectory, bool openWithUi = true)
        {
            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("Proje adı boş olamaz.");

            if (string.IsNullOrWhiteSpace(targetDirectory))
                throw new ArgumentException("Hedef klasör boş olamaz.");

            Directory.CreateDirectory(targetDirectory);

            // TIA Portal hedef klasörün içinde proje adıyla alt klasör oluşturur;
            // aynı isimde klasör varsa Create çakışabilir.
            string projectFolder = Path.Combine(targetDirectory, projectName);
            if (Directory.Exists(projectFolder))
                throw new InvalidOperationException($"Bu isimde bir proje klasörü zaten mevcut: {projectFolder}");

            TiaPortalMode mode = openWithUi
                ? TiaPortalMode.WithUserInterface
                : TiaPortalMode.WithoutUserInterface;

            using (TiaPortal tiaPortal = new TiaPortal(mode))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(targetDirectory);
                Project project = tiaPortal.Projects.Create(dirInfo, projectName);
                string savedPath = project.Path.FullName;
                project.Save();

                // UI açık moddaysa kullanıcı TIA Portal penceresinde projeyi görebilir
                if (!openWithUi)
                {
                    project.Close();
                }

                return savedPath;
            }
        }
    }
}