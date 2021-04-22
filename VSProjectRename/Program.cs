using System;
using System.IO;
using System.Linq;
using System.Text;

namespace VSProjectRename
{
    class Program
    {
        static void Main(string[] args)
        {

            //请先备份项目
            Console.WriteLine("本工具会将代码中、路径中旧项目名关键字转换为新项目名");

            Console.WriteLine("注意：请先备份原项目！");
            Console.WriteLine("注意：请先备份原项目！");
            Console.WriteLine("注意：请先备份原项目！");
            Console.WriteLine("注意：请先备份原项目！");
            Console.WriteLine("注意：请先备份原项目！");

            Console.WriteLine("注意：请自行将目录中的.git .vs .vscode等目录先移除");

            //请先备份项目
            Console.WriteLine(@"请输入项目sln所在路径（例:D:\git\KanAuto）：");
            string projectPath = Console.ReadLine();

            Console.WriteLine(@"请输入旧项目名：");
            string projectOldName = Console.ReadLine();

            Console.WriteLine(@"请输入新项目名：");
            string projectNewName = Console.ReadLine();

            Console.WriteLine("是否开始执行(y/n)：");
            if (Console.ReadLine() != "y")
            {
                Console.WriteLine(@"结束执行");
                return;
            }

            if (!Directory.Exists(projectPath))
            {
                Console.WriteLine(@"目录不存在");
                return;
            }

            byte[] projectOldNameBytes = System.Text.Encoding.UTF8.GetBytes(projectOldName);
            byte[] projectNewNameBytes = System.Text.Encoding.UTF8.GetBytes(projectNewName);

            Console.WriteLine("文件内容更改中...");
            //枚举文件，先将文件内的关键字修改
            string[] files = Directory.GetFiles(projectPath, "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                byte[] bytes = File.ReadAllBytes(file);
                byte[] newBytes = ByteExtension.ByteReplace(bytes, (projectOldNameBytes, projectNewNameBytes)).ToArray();

                //var byteReplaceResults = Encoding.ASCII.GetString(ByteExtension.ByteReplace(bytes, (projectOldNameBytes, projectNewNameBytes)).ToArray());

                File.WriteAllBytes(file, newBytes);
            }

            Console.WriteLine("文件名更改中...");
            foreach (string file in files)
            {
                if (file.Contains(projectOldName))
                {
                    try
                    {
                        Console.WriteLine($"修改:{file}");
                        File.Move(file, file.Replace(projectOldName, projectNewName));
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }

            //将文件夹名字修改，包含名字的都算在内
            PathRename(projectPath, projectOldName, projectNewName);

        }

        static void PathRename(string projectPath, string projectOldName, string projectNewName)
        {
            Console.WriteLine("目录名更改中...");
            string[] paths = Directory.GetDirectories(projectPath, "*", SearchOption.AllDirectories);
            bool hasMove = false;
            foreach (string path in paths)
            {
                if (path.Contains(projectOldName))
                {
                    try
                    {
                        Console.WriteLine($"修改:{path}");
                        Directory.Move(path, path.Replace(projectOldName, projectNewName));
                        hasMove = true;
                    }
                    catch (System.Exception ex)
                    {
                    	
                    }
                }
            }
            Console.WriteLine(paths);

            if (hasMove)
            {
                PathRename(projectPath, projectOldName, projectNewName);
            }
        }

    }
}
