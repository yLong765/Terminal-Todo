using System.Collections.Generic;

namespace Todo
{
    public static class Blackboard
    {
        public static string SavePath = "/config.conf";
        public static List<string> ToDos = new List<string>();
    }
}