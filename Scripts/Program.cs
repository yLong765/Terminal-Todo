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
            //Test();
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
            Console.WriteLine("Init System...");
            LogMgr.Create();
            TodoMgr.Create();
            CommandMgr.Create();
            SaveMgr.Create();
            Console.WriteLine("Init Done...");
            Console.WriteLine("Loading...");
            TodoMgr.Instance.LoadFile();
            Console.WriteLine("Done! can use");
        }

        static void Test()
        {
            CommandMgr.Instance.ExecuteCommand("add 123");
            CommandMgr.Instance.ExecuteCommand("list");
            CommandMgr.Instance.ExecuteCommand("addt [nb] 0");
            CommandMgr.Instance.ExecuteCommand("list");
        }
    }
}
