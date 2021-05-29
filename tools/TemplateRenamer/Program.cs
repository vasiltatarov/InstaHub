using System;
using System.IO;
using System.Text;

namespace TemplateRenamer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("Template Renamer");
            Console.WriteLine(new string('=', 40));

            Console.WriteLine("Write absolute path to directory would you like to rename?");
            Console.WriteLine();
            Console.WriteLine("Example: D:\\SoftUni\\src");

            Console.WriteLine(new string('-', 40));
            Console.Write("Path: ");

            // example D:\SoftUni\src
            var renameDirectory = Console.ReadLine();

            Console.WriteLine(new string('-', 40));
            Console.WriteLine("Working in: " + renameDirectory);
            Console.WriteLine(new string('=', 40));
            Console.WriteLine();

            var oldName = string.Empty;
            if (string.IsNullOrWhiteSpace(oldName))
            {
                Console.Write("What is your (file, directory, project's) old name for replace ([A-Z][a-z]): ");
                oldName = Console.ReadLine();
            }

            var newName = string.Empty;
            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.Write("What is your (file, directory, project's) new name to replace ([A-Z][a-z]): ");
                newName = Console.ReadLine();
            }

            Console.WriteLine("Renaming directories...");
            RenameDirectories(renameDirectory, oldName, newName);
            Console.WriteLine("Directories renamed.");

            Console.WriteLine("Renaming files...");
            RenameFiles(renameDirectory, oldName, newName);
            Console.WriteLine("Files renamed.");

            Console.WriteLine("Renaming file contents...");
            RenameFileContents(renameDirectory, oldName, newName);
            Console.WriteLine("File contents renamed.");

            Console.WriteLine("Done!");
        }

        private static void RenameFileContents(string currentDirectory, string originalName, string newName)
        {
            var files = Directory.GetFiles(currentDirectory);
            foreach (var file in files)
            {
                if (!file.EndsWith(".exe") && !file.EndsWith(".dll") && !file.EndsWith(".runtimeconfig.json"))
                {
                    var contents = File.ReadAllText(file);
                    contents = contents.Replace(originalName, newName);
                    File.WriteAllText(file, contents, Encoding.UTF8);
                }
            }

            var subDirectories = Directory.GetDirectories(currentDirectory);
            foreach (var directory in subDirectories)
            {
                RenameFileContents(directory, originalName, newName);
            }
        }

        private static void RenameFiles(string currentDirectory, string originalName, string newName)
        {
            var files = Directory.GetFiles(currentDirectory);
            foreach (var file in files)
            {
                var newFileName = file.Replace(originalName, newName);
                if (newFileName != file)
                {
                    Directory.Move(file, newFileName);
                }
            }

            var subDirectories = Directory.GetDirectories(currentDirectory);
            foreach (var directory in subDirectories)
            {
                RenameFiles(directory, originalName, newName);
            }
        }

        private static void RenameDirectories(string currentDirectory, string originalName, string newName)
        {
            var directories = Directory.GetDirectories(currentDirectory);
            foreach (var directory in directories)
            {
                var newDirectoryName = directory.Replace(originalName, newName);
                if (newDirectoryName != directory)
                {
                    Directory.Move(directory, newDirectoryName);
                }
            }

            directories = Directory.GetDirectories(currentDirectory);
            foreach (var directory in directories)
            {
                RenameDirectories(directory, originalName, newName);
            }
        }
    }
}
