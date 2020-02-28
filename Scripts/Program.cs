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
            LogMgr.Instance.CanOutPutLog = false;
            TodoMgr.Create();
            CommandMgr.Create();
            SaveMgr.Create();
            Console.WriteLine("Loading File...");
            SaveMgr.Instance.LoadFile();
            LogMgr.Instance.CanOutPutLog = true;
            Console.WriteLine("Done! can use");
            CommandMgr.Instance.ExecuteCommand("list");
        }

        static void Test()
        {
            //CommandMgr.Instance.ExecuteCommand("list");
            //CommandMgr.Instance.ExecuteCommand("done 2");
            //CommandMgr.Instance.ExecuteCommand("done 2");
            //CommandMgr.Instance.ExecuteCommand("addt [nb] 0");
            //CommandMgr.Instance.ExecuteCommand("list");
        }
    }
}
