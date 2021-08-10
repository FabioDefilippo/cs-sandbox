using System.IO;
using System.Security;
using System;
using System.Diagnostics;
using sandbox.Properties;
using System.Security.AccessControl;

namespace sandbox
{
    class Program
    {
        private static readonly string SandBoxFolder = @"C:\Temp\";
        private static readonly string[] VARS = new string[] {"OS", "PATHEXT", "PROCESSOR_ARCHITECTURE", "PROCESSOR_IDENTIFIER", "PROCESSOR_LEVEL",
                            "LOCALAPPDATA", "NUMBER_OF_PROCESSORS", "LOGONSERVER", "COMPUTERNAME","APPDATA", "HOMEDRIVE", "PUBLIC", "HOMEPATH",
                            "SystemDrive","SystemRoot","USERNAME", "USERPROFILE", "USERDOMAIN", "USERDOMAIN_ROAMINGPROFILE", "TMP", "TEMP"  };
        private static readonly TextWriter errout = Console.Error;
        static void Main(string[] args)
        {
            if (args.Length.Equals(0) || args.Equals(null))
            {
                errout.WriteLine(Resources.titolo);
                errout.WriteLine(Resources.arg1);
                errout.WriteLine(Resources.arg2);
                errout.WriteLine(Resources.arg3);
                errout.WriteLine(Resources.arg4);
                errout.WriteLine(Resources.arg5);
            }
            else
            {
                errout.WriteLine(Resources.load);
                try
                {
                    int LUN = args.Length;
                    if (LUN.Equals(5))
                    {
                        if (!args[0].Equals(String.Empty) && !args[1].Equals(String.Empty) && !args[3].Equals(String.Empty))
                        {
                            DirectoryInfo di = new DirectoryInfo(SandBoxFolder);
                            if(!di.Exists)
                            {
                                di.Create();
                            }
                            string PROGRAMMA = args[3];
                            string DOMINIO = args[0];
                            string UTENTE = args[1];
                            string PASSWD = args[2];
                            string ARGOMENTI = args[4];
                            errout.WriteLine(Resources.calc);
                            DirectorySecurity ds = new DirectorySecurity(SandBoxFolder, AccessControlSections.All);
                            ds.SetAccessRule(new FileSystemAccessRule(DOMINIO + Resources.slash + UTENTE, FileSystemRights.FullControl, AccessControlType.Allow));
                            di.SetAccessControl(ds);
                            if (File.Exists(PROGRAMMA))
                            {
                                errout.WriteLine(Resources.ver);
                                SecureString SS = new SecureString();
                                if (PASSWD.Length > 0)
                                {
                                    foreach (char CARA in PASSWD.ToCharArray())
                                    {
                                        SS.AppendChar(CARA);
                                    }
                                }
                                errout.WriteLine(Resources.set);
                                ProcessStartInfo PSI;
                                PSI = new ProcessStartInfo(PROGRAMMA, ARGOMENTI);
                                PSI.WindowStyle = ProcessWindowStyle.Maximized;
                                PSI.WorkingDirectory = SandBoxFolder;
                                PSI.UseShellExecute = false;
                                PSI.UserName = UTENTE;
                                PSI.Domain = DOMINIO;
                                PSI.Password = SS;
                                foreach (string VAR in VARS)
                                {
                                    PSI.Environment.Remove(VAR);
                                }
                                PSI.Environment.Add(Resources.PAT, Resources.EXE);
                                Process.Start(PSI);
                                errout.WriteLine(Resources.done);

                            }
                            else
                            {
                                errout.WriteLine(PROGRAMMA + Resources.noexist);
                            }
                        }
                        else
                        {
                            errout.WriteLine(Resources.err1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errout.WriteLine(ex.Message);
                }
            }
        }
    }
}