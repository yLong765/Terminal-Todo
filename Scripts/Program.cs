using System.Linq;
using System;
using System.Collections.Generic;

namespace Todo
{
    class Program
    {
        static void Main(string[] args)
        {
            InitAllMgr();
            Test();
            string command = "nb!!!";
            while ((command = GetInput()) != "exit")
            {
                CommandMgr.Instance.ExecuteCommand(command);
            }
        }

        static string GetInput()
        {
            Console.Write("Command> ");
            return Console.ReadLine().ToLower();
        }

        static void InitAllMgr()
        {
            LogMgr.Create();
            TodoMgr.Create();
            CommandMgr.Create();
            SaveMgr.Create();
            Console.WriteLine("Loading File...");
            SaveMgr.Instance.LoadFile();
            Console.WriteLine("Done! can use");
        }

        static void Test()
        {
            CommandMgr.Instance.ExecuteCommand("add [p1] [p2] 123");
            CommandMgr.Instance.ExecuteCommand("list");
            //CommandMgr.Instance.ExecuteCommand("addt [nb] 0");
            //CommandMgr.Instance.ExecuteCommand("list");
        }
    }
}
