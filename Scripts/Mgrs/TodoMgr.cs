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
        public LogEnum AddTodo(string content, List<string> tags = null)
        {
            if (!string.IsNullOrEmpty(content))
            {
                Todos.Add(CreateTodo(content, tags));
                return LogEnum.AddSuccess;
            }
            return LogEnum.NoParameter;
        }
        public LogEnum DelTodo(int index)
        {
            if (index >= 0 && index < Todos.Count)
            {
                Todos.RemoveAt(index);
                return LogEnum.DelSuccess;
            }
            return LogEnum.ParamerterIllegal;
        }
        public void DelTodo(Todo todo)
        {
            Todos.Remove(todo);
        }
        public LogEnum AddTags(int index, List<string> tags)
        {
            if (index >= 0 && index < Todos.Count)
            {
                return Todos[index].AddTags(tags);
            }
            return LogEnum.ParamerterIllegal;
        }
        public LogEnum DelTags(int index, List<string> tags)
        {
            if (index >= 0 && index < Todos.Count)
            {
                return Todos[index].DelTags(tags);
            }
            return LogEnum.ParamerterIllegal;
        }
        public LogEnum DoneTodo(int index)
        {
            var todos = SearchTodo(SpecialTag.Done, false);
            if (index >= 0 && index < todos.Count)
            {
                todos[index].DoneTodo();
            }
            return LogEnum.None;
        }
        public LogEnum ShowList(string tag, bool has)
        {
            var searchTodos = SearchTodo(tag, has);
            if (searchTodos.Count > 0)
            {
                string todoTemp = @"{0}. {1}";
                for (int i = 0; i < searchTodos.Count; i++)
                {
                    LogMgr.Instance.Log(string.Format(todoTemp, i, searchTodos[i]));
                }
                return LogEnum.None;
            }
            return LogEnum.ListAllDone;
        }
        public LogEnum ClearTodo(List<string> tags)
        {
            if (tags.Count != 0)
            {
                foreach (var tag in tags)
                {
                    var todos = SearchTodo(tag, true);
                    foreach (var todo in todos)
                    {
                        DelTodo(todo);
                    }
                }
            }
            else
            {
                Todos.Clear();
            }
            return LogEnum.None;
        }
        public string[] ToFileString()
        {
            string[] result = new string[Todos.Count];
            for (int i = 0; i < Todos.Count; i++)
            {
                result[i] = Todos[i].ToFileString();
            }
            return result;
        }
        public List<Todo> SearchTodo(string tag, bool has = true)
        {
            return Todos.FindAll(todo =>
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
        }
    }
}