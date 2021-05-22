using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Resources;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using Microsoft.Build;
using System.Reflection;

namespace NPVC
{
    public class NPVCElement
    {
        /// <summary>
        /// MSBuild application address.
        /// </summary>
        private string MSBuildApplicationPath { get { return $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{nameof(NPVCResource.MSBuild)}\15.0\Bin\MSBuild.exe"; } }


        /// <summary>
        /// Directory where the program versions is storeaged.
        /// </summary>
        public string ProgramsDirectoryPath { get; private set; }
        /// <summary>
        /// Folder name of reference version.
        /// </summary>
        public string ReferenceVersionFolderName { get; private set; }
        /// <summary>
        /// Folder name of new version.
        /// </summary>
        public string NewVersionFolderName { get; private set; }
        /// <summary>
        /// LOG path for storeage each information from compilation process.
        /// </summary>
        public string CompileProcessLOGFilePath { get; private set; }
        /// <summary>
        /// Directory where the reference version Dll's is storeaged.
        /// </summary>
        public string ReferenceVersionDllsDirectoryPath { get; private set; }
        /// <summary>
        /// Directory where the new version Dll's is storeaged.
        /// </summary>
        public string NewVersionDllsDirectoryPath { get; private set; }
        /// <summary>
        /// Directory where the new version resources is storeaged.
        /// </summary>
        public string NewVersionResourcesDirectoryPath { get; private set; }

        /// <summary>
        /// <see cref="List{T}"/> with each program with the reference version folder name.
        /// </summary>
        public List<string> ProgramsWithReferenceVersionFolderName { get; private set; }
        /// <summary>
        /// <see cref="List{T}"/> with each program with the new version folder name.
        /// </summary>
        public List<string> ProgramsWithNewVersionFolderName { get; private set; }

        /// <summary>
        /// <see cref="List{T}"/> with each Dll file address of the reference version.
        /// </summary>
        public List<string> ReferenceVersionDllFiles { get; private set; }
        /// <summary>
        /// <see cref="List{T}"/> with each Dll file address of the new version.
        /// </summary>
        public List<string> NewVersionDllFiles { get; private set; }
        /// <summary>
        /// <see cref="List{T}"/> with each resource file address of the new version.
        /// </summary>
        public List<string> NewVersionResourceFiles { get; private set; }
        
        /// <summary>
        /// Creat a new programa version creator instance.
        /// </summary>
        /// <param name="programsDirectoryPath">Directory where the program versions is storeaged.</param>
        /// <param name="referenceVersionFolderName">Folder name of reference version.</param>
        /// <param name="newVersionFolderName">Folder name of new version.</param>
        /// <param name="compileProcessLOGFilePath">LOG path for storeage each information from compilation process.</param>
        /// <param name="referenceVersionDllsDirectoryPath">Directory where the reference version Dll's is storeaged.</param>
        /// <param name="newVersionDllsDirectoryPath">Directory where the new version Dll's is storeaged.</param>
        /// <param name="newVersionResourcesDirectoryPath">Directory where the new version resources is storeaged.</param>
        public NPVCElement(string programsDirectoryPath, string referenceVersionFolderName, string newVersionFolderName, string compileProcessLOGFilePath, string referenceVersionDllsDirectoryPath, string newVersionDllsDirectoryPath, string newVersionResourcesDirectoryPath)
        {
            ProgramsDirectoryPath = programsDirectoryPath;
            ReferenceVersionFolderName = referenceVersionFolderName;
            NewVersionFolderName = newVersionFolderName;
            CompileProcessLOGFilePath = compileProcessLOGFilePath;
            ReferenceVersionDllsDirectoryPath = referenceVersionDllsDirectoryPath;
            NewVersionDllsDirectoryPath = newVersionDllsDirectoryPath;
            NewVersionResourcesDirectoryPath = newVersionResourcesDirectoryPath;

            GetAllReferenceVersionDirectories();
            GetAllNewVersionDirectories();

            GetAllReferenceVersionDllFiles();
            GetAllNewVersionDllFiles();

            GetAllNewVersionResourceFiles();
        }

        /// <summary>
        /// Get all reference version directories.
        /// </summary>
        void GetAllReferenceVersionDirectories()
        {
            if (string.IsNullOrEmpty(ProgramsDirectoryPath) || string.IsNullOrWhiteSpace(ProgramsDirectoryPath) || !Directory.Exists(ProgramsDirectoryPath))
            {
                ProgramsWithReferenceVersionFolderName = new List<string>();
                return;
            }
            List<string> directories = new List<string>(Directory.GetDirectories(ProgramsDirectoryPath));
            ProgramsWithReferenceVersionFolderName = new List<string>();
            while (directories.Count > 0)
            {
                if (directories.First().Split(Path.DirectorySeparatorChar).Last() == ReferenceVersionFolderName)
                {
                    ProgramsWithReferenceVersionFolderName.Add(directories.First());
                }
                else
                {
                    directories.AddRange(Directory.GetDirectories(directories.First()));
                }
                directories.Remove(directories.First());
            }
        }
        /// <summary>
        /// Get all new version directories.
        /// </summary>
        void GetAllNewVersionDirectories()
        {
            if (string.IsNullOrEmpty(ProgramsDirectoryPath) || string.IsNullOrWhiteSpace(ProgramsDirectoryPath) || !Directory.Exists(ProgramsDirectoryPath))
            {
                ProgramsWithNewVersionFolderName = new List<string>();
                return;
            }
            List<string> directories = new List<string>(Directory.GetDirectories(ProgramsDirectoryPath));
            ProgramsWithNewVersionFolderName = new List<string>();
            while (directories.Count > 0)
            {

                if (directories.First().Split(Path.DirectorySeparatorChar).Last() == NewVersionFolderName)
                {
                    ProgramsWithNewVersionFolderName.Add(directories.First());
                }
                else
                {
                    directories.AddRange(Directory.GetDirectories(directories.First()));
                }
                directories.Remove(directories.First());

            }
        }

        /// <summary>
        /// Get all reference version Dll files.
        /// </summary>
        void GetAllReferenceVersionDllFiles()
        {
            if (string.IsNullOrEmpty(ReferenceVersionDllsDirectoryPath) || string.IsNullOrWhiteSpace(ReferenceVersionDllsDirectoryPath) || !Directory.Exists(ReferenceVersionDllsDirectoryPath))
            {
                ReferenceVersionDllFiles = new List<string>();
                return;
            }
            List<string> directories = new List<string>(Directory.GetDirectories(ReferenceVersionDllsDirectoryPath));
            ReferenceVersionDllFiles = new List<string>(Directory.GetFiles(ReferenceVersionDllsDirectoryPath));
            while (directories.Count > 0)
            {
                directories.AddRange(Directory.GetDirectories(directories.First()));
                ReferenceVersionDllFiles.AddRange(Directory.GetFiles(directories.First()));
                directories.RemoveAt(0);
            }
        }
        /// <summary>
        /// Get all new version Dll files.
        /// </summary>
        void GetAllNewVersionDllFiles()
        {
            if (string.IsNullOrEmpty(NewVersionDllsDirectoryPath) || string.IsNullOrWhiteSpace(NewVersionDllsDirectoryPath) || !Directory.Exists(NewVersionDllsDirectoryPath))
            {
                NewVersionDllFiles = new List<string>();
                return;
            }
            List<string> directories = new List<string>(Directory.GetDirectories(NewVersionDllsDirectoryPath));
            NewVersionDllFiles = new List<string>(Directory.GetFiles(NewVersionDllsDirectoryPath));
            while (directories.Count > 0)
            {
                directories.AddRange(Directory.GetDirectories(directories.First()));
                NewVersionDllFiles.AddRange(Directory.GetFiles(directories.First()));
                directories.RemoveAt(0);
            }
        }

        /// <summary>
        /// Get all new version resource files.
        /// </summary>
        void GetAllNewVersionResourceFiles()
        {
            if (string.IsNullOrEmpty(NewVersionResourcesDirectoryPath) || string.IsNullOrWhiteSpace(NewVersionResourcesDirectoryPath) || !Directory.Exists(NewVersionResourcesDirectoryPath))
            {
                NewVersionResourceFiles = new List<string>();
                return;
            }
            List<string> directories = new List<string>(Directory.GetDirectories(NewVersionResourcesDirectoryPath));
            NewVersionResourceFiles = new List<string>(Directory.GetFiles(NewVersionResourcesDirectoryPath));
            while (directories.Count > 0)
            {
                directories.AddRange(Directory.GetDirectories(directories.First()));
                NewVersionResourceFiles.AddRange(Directory.GetFiles(directories.First()));
                directories.RemoveAt(0);
            }
        }

        /// <summary>
        /// Creat a new programa version using a reference program.
        /// </summary>
        public void CreatNewProgramVersion()
        {
            foreach (string referenceVersionPath in ProgramsWithReferenceVersionFolderName)
            {
                string newVersionPath = $@"{referenceVersionPath}\".Replace($"{Path.DirectorySeparatorChar}{ReferenceVersionFolderName}{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}{NewVersionFolderName}");
                if (!Directory.Exists(newVersionPath))
                {
                    Directory.CreateDirectory(newVersionPath);
                    ProgramsWithNewVersionFolderName.Add(newVersionPath);
                }
                while (!Directory.Exists(newVersionPath)) { Thread.Sleep(10); }

                foreach (string path in GetAllDirectories(referenceVersionPath))
                {
                    string newPath = path.Replace($"{Path.DirectorySeparatorChar}{ReferenceVersionFolderName}{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}{NewVersionFolderName}{Path.DirectorySeparatorChar}");
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    while (!Directory.Exists(newPath)) { Thread.Sleep(10); }
                }

                foreach (string path in GetAllFiles(referenceVersionPath))
                {
                    string newPath = path.Replace($"{Path.DirectorySeparatorChar}{ReferenceVersionFolderName}{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}{NewVersionFolderName}{Path.DirectorySeparatorChar}");
                    if (!File.Exists(newPath))
                    {
                        File.Copy(path, newPath);
                    }
                    while (!File.Exists(newPath)) { Thread.Sleep(10); }
                }
            }

        }
        /// <summary>
        /// Change Dll references of new program version.
        /// </summary>
        public void ChangeDllReferencesOfNewVersion()
        {
            string csprojExtension = ".csproj";
            Dictionary<string, string> referencesXML = new Dictionary<string, string>();

            foreach (string dllPath in NewVersionDllFiles)
            {
                string dllName = Path.GetFileNameWithoutExtension(dllPath);
                string modifiedPath = string.Empty;
                string[] terms = dllPath.Split(new char[] { Path.DirectorySeparatorChar }, int.MaxValue, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < terms.Length; i++)
                {
                    if (i == terms.Length - 1)
                        modifiedPath += terms[i];
                    else
                        modifiedPath += $@"{terms[i]}\";
                }
                try
                {
                    referencesXML.Add
                        (
                            dllName,
                            $@"<Reference Include={"\""}{dllName}{"\""}>" +
                            $@"{"\n"}   <Private>False</Private>" +
                            $@"{"\n"}   <HintPath>..\..\..\..\{modifiedPath}</HintPath>" +
                            $@"{"\n"}   <Private>False</Private>" +
                            $@"{"\n"}</Reference>"
                        );
                }
                catch { }
            }
            foreach (string path in ProgramsWithNewVersionFolderName)
            {
                foreach (string csprojFile in GetAllFiles(path).Where(file => Path.GetExtension(file) == csprojExtension))
                {
                    List<string> xmlDocument = new List<string>();
                    foreach (string line in File.ReadAllLines(csprojFile))
                    {
                        xmlDocument.Add(line);
                    }
                    foreach (KeyValuePair<string, string> referenceXML in referencesXML)
                    {
                        if (xmlDocument.Any(line => line.Contains($"<Reference Include=\"{referenceXML.Key},")))
                        {
                            int referenceIndex = xmlDocument.FindIndex(line => line.Contains($"<Reference Include=\"{referenceXML.Key},"));
                            string t = string.Empty;
                            for (int i = referenceIndex; i < xmlDocument.Count;)
                            {
                                if (xmlDocument[i].Contains("</Reference>"))
                                {
                                    xmlDocument.RemoveAt(i);
                                    break;
                                }
                                else
                                    xmlDocument.RemoveAt(i);
                            }

                            if (referenceIndex < xmlDocument.Count)
                                xmlDocument.Insert(referenceIndex, referenceXML.Value);
                            else
                                xmlDocument.Add(referenceXML.Value);
                        }
                    }
                    File.SetAttributes(csprojFile, File.GetAttributes(csprojFile) & ~FileAttributes.ReadOnly);
                    File.WriteAllLines(csprojFile, xmlDocument);
                    File.SetAttributes(csprojFile, File.GetAttributes(csprojFile) | FileAttributes.ReadOnly);
                }
            }
        }
        /// <summary>
        /// Change resources of new program version.
        /// </summary>
        public void ChangeResourcesOfNewVersion()
        {
            List<string> files = GetAllFiles(ProgramsWithNewVersionFolderName);
            foreach (string resourceFile in NewVersionResourceFiles)
            {
                foreach(string file in files.Where(file => Path.GetFileName(file) == Path.GetFileName(resourceFile)))
                {
                    File.Copy(resourceFile, file,true);
                }
            }
        }
        /// <summary>
        /// Compile each new program version
        /// <para>*Advice: Process only avaliable for Microsoft Visual Studio Projects</para>
        /// </summary>
        public void CompileNewVersionPrograms()
        {
            CreatFile(CompileProcessLOGFilePath);
            Tuple<bool, Exception> mSBuildStatus = CreatMSBuildComponents();
            if (mSBuildStatus.Item1)
            {
                string slnExtension = ".sln";
                foreach (string path in ProgramsWithNewVersionFolderName)
                {
                    foreach (string slnFile in GetAllFiles(path).Where(file => Path.GetExtension(file) == slnExtension))
                    {
                        string slnDirectory = Path.GetDirectoryName(slnFile);
                        string logFile = $@"{slnDirectory}\MSBuildLog.log";
                        string[] arguments = new string[]
                        {
                            $@"cd /d ""{slnDirectory}""",
                            $@"""{MSBuildApplicationPath}"" -flp:logfile=""{logFile}"";",                            
                        };
                        ProcessStartInfo processInfo = new ProcessStartInfo("CMD.exe")
                        {
                            CreateNoWindow = false,
                            Arguments = "/C " + string.Join(" & ", arguments),
                            UseShellExecute = false,
                        };
                        Process.Start(processInfo).WaitForExit();

                        string[] compilationData = File.ReadAllLines(logFile);
                        string[] compilationResultInf = new string[]
                        {
                            $"{DateTime.Now}",
                            slnDirectory,
                            compilationData[compilationData.Length - 3],
                            compilationData[compilationData.Length - 4],
                            string.Empty,
                        };
                        File.AppendAllLines(CompileProcessLOGFilePath, compilationResultInf);
                    }
                }
            }    
        }

        Tuple<bool, Exception> CreatMSBuildComponents()
        {
            try
            {
                if (!File.Exists($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{nameof(NPVCResource.MSBuild)}.zip"))                
                    File.WriteAllBytes($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{nameof(NPVCResource.MSBuild)}.zip", NPVCResource.MSBuild);                   
                if(!Directory.Exists($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\"))
                    ZipFile.ExtractToDirectory($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{nameof(NPVCResource.MSBuild)}.zip", $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\");
                if (!File.Exists($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{nameof(NPVCResource.MSBuild)}.zip"))
                    File.Delete($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\{nameof(NPVCResource.MSBuild)}.zip");

                return new Tuple<bool, Exception>(true, null);
            }
            catch (Exception error)
            {
                return new Tuple<bool, Exception>(false, error);
            }
        }

        public void CreatFile(string path)
        {
            string[] terms = path.Split(Path.DirectorySeparatorChar);
            string address = string.Empty;
            foreach(string term in terms)
            {                
                address += string.IsNullOrEmpty(address) ? $@"{term}" : $@"\{term}";                
                if (Path.HasExtension(address))
                {
                    if (!File.Exists(address))
                        File.Create(address).Close();
                }
                else
                {
                    if (!Directory.Exists(address))
                        Directory.CreateDirectory(address);
                }
            }
        }

        public List<string> GetAllDirectories(string path)
        {
            List<string> directories = new List<string>(Directory.GetDirectories(path));
            for (int i = 0; i < directories.Count; i++)
            {
                directories.AddRange(Directory.GetDirectories(directories[i]));
            }
            return directories;
        }
        public List<string> GetAllDirectories(List<string> paths)
        {
            List<string> directories = new List<string>();
            foreach (string path in paths)
            {
                directories.AddRange(Directory.GetDirectories(path));
                for (int i = 0; i < directories.Count; i++)
                {
                    directories.AddRange(Directory.GetDirectories(directories[i]));
                }
            }
            return directories;
        }
        public List<string> GetAllFiles(string path)
        {
            List<string> directories = new List<string>(Directory.GetDirectories(path));
            List<string> paths = new List<string>(Directory.GetFiles(path));
            while (directories.Count > 0)
            {
                directories.AddRange(Directory.GetDirectories(directories.First()));
                paths.AddRange(Directory.GetFiles(directories.First()));
                directories.RemoveAt(0);
            }
            return paths;
        }
        public List<string> GetAllFiles(List<string> paths)
        {
            List<string> directories = new List<string>();
            List<string> _paths = new List<string>();
            foreach (string path in paths)
            {                
                directories.AddRange(Directory.GetDirectories(path));
                _paths.AddRange(Directory.GetFiles(path));
                while (directories.Count > 0)
                {
                    directories.AddRange(Directory.GetDirectories(directories.First()));
                    _paths.AddRange(Directory.GetFiles(directories.First()));
                    directories.RemoveAt(0);
                }
            }
            return _paths;
        }
    }
}

