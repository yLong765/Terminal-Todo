using System;

namespace Todo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading...");
            string com = "del 0";
            while ((com = Console.ReadLine()) != "exit")
            {
                string[] coms = com.Split(' ');
                CommandMgr.Instance.ExecuteCommand(coms);
            }
        }
    }
}
