using System;
using System.Collections.Generic;

namespace Todo
{
    public class Todo
    {
        public HashSet<string> tags = new HashSet<string>();
        public string content;
        public Todo(string content, params string[] tags)
        {
            this.content = content;
            for (int i = 0; i < tags.Length; i++)
            {
                AddTag(tags[i]);
            }
        }
        public void AddTag(string tag)
        {
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
            }
        }
        public override string ToString()
        {
            string str = "";
            foreach (var tag in tags)
            {
                str += "[" + tag +"] ";
            }
            return str + content;
        }
    }

    public class TodoMgr
    {
        private static List<Todo> Todos = new List<Todo>();
        public static Todo CreateTodo(string content, params string[] tags)
        {
            return new Todo(content, tags);
        }
        public static void AddTodo(string content, params string[] tags)
        {
            Todos.Add(CreateTodo(content, tags));
            LogMgr.SystemLog(LogEnum.AddSuccess);
        }
        public static void DelTodo(int index)
        {
            if (index >= 0 && index < Todos.Count)
            {
                Todos.RemoveAt(index);
                LogMgr.SystemLog(LogEnum.DelSuccess);
            }
            else
            {
                LogMgr.SystemLog(LogEnum.DelFailed1);
            }
        }
        public static void ShowAllTodo()
        {
            if (Todos.Count > 0)
            {
                string todoTemp = @"{0}. {1}";
                for (int i = 0; i < Todos.Count; i++)
                {
                    LogMgr.Log(string.Format(todoTemp, i, Todos[i]));
                }
            }
            else
            {
                LogMgr.SystemLog(LogEnum.ListAllDone);
            }
        }
    }
}