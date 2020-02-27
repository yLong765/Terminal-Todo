using System.Linq;
using System.Collections.Generic;

namespace Todo
{
    public class TodoMgr : InstanceMgr<TodoMgr>
    {
        private List<Todo> Todos = new List<Todo>();
        public Todo CreateTodo(string content, List<string> tags = null)
        {
            return new Todo(content, tags);
        }
        public void AddTodo(string content, List<string> tags = null)
        {
            if (!string.IsNullOrEmpty(content))
            {
                Todos.Add(CreateTodo(content, tags));
                LogMgr.Instance.SystemLog(LogEnum.AddSuccess);
            }
        }
        public void DelTodo(int index)
        {
            if (index >= 0 && index < Todos.Count)
            {
                Todos.RemoveAt(index);
                LogMgr.Instance.SystemLog(LogEnum.DelSuccess);
            }
            else
            {
                LogMgr.Instance.SystemLog(LogEnum.ParamerterIllegal);
            }
        }
        public void AddTags(int index, List<string> tags)
        {
            if (index >= 0 && index < Todos.Count)
            {
                Todos[index].AddTags(tags);
            }
            else
            {
                LogMgr.Instance.SystemLog(LogEnum.ParamerterIllegal);
            }
        }
        public void DelTags(int index, List<string> tags)
        {
            if (index >= 0 && index < Todos.Count)
            {
                Todos[index].DelTags(tags);
            }
            else
            {
                LogMgr.Instance.SystemLog(LogEnum.ParamerterIllegal);
            }
        }
        public void SearchTodo(string tag = "完成", bool has = false)
        {
            List<Todo> searchTodos = Todos.FindAll(todo =>
            {
                if (has)
                {
                    return todo.tags.Contains(tag);
                }
                else
                {
                    return !todo.tags.Contains(tag);
                }
            });
            if (searchTodos.Count > 0)
            {
                string todoTemp = @"{0}. {1}";
                for (int i = 0; i < searchTodos.Count; i++)
                {
                    LogMgr.Instance.Log(string.Format(todoTemp, i, searchTodos[i]));
                }
            }
            else
            {
                LogMgr.Instance.SystemLog(LogEnum.ListAllDone);
            }
        }
        public string[] ToFileString()
        {
            string[] result = new string[Todos.Count];
            for (int i = 0; i < Todos.Count; i++)
            {
                result[i] = Todos[i].ToString();
            }
            return result;
        }
        public void LoadFile()
        {
            string[] todos = SaveMgr.Instance.ReadTodoFile();
            if (todos != null)
            {
                for (int i = 0; i < todos.Length; i++)
                {
                    var commands = todos[i].Split(' ').ToList();
                    var tags = CommandUtility.GetTags(commands);
                    var content = CommandUtility.GetContent(commands);
                    AddTodo(content, tags);
                }
            }
        }
    }
}