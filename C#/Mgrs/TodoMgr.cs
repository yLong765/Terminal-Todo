using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Todo
{
    public class TodoMgr : InstanceMgr<TodoMgr>
    {
        private List<Todo> _todos = new List<Todo>();
        #region cacheTodos
        private Dictionary<string, List<Todo>> _cacheTodos = new Dictionary<string, List<Todo>>();
        private void AddCache(Todo todo, string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                if (!_cacheTodos.ContainsKey(tag))
                {
                    _cacheTodos.Add(tag, new List<Todo>());
                }
                if (!_cacheTodos[tag].Contains(todo))
                {
                    _cacheTodos[tag].Add(todo);
                }
            }
        }
        private void AddCaches(Todo todo, List<string> tags)
        {
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    AddCache(todo, tag);
                }
            }
        }
        private void DelCache(Todo todo, string tag)
        {
            if (!string.IsNullOrEmpty(tag))
            {
                if (_cacheTodos.ContainsKey(tag))
                {
                    _cacheTodos[tag].Remove(todo);
                }
            }
        }
        private void DelCaches(Todo todo, List<string> tags)
        {
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    DelCache(todo, tag);
                }
            }
        }
        #endregion
        public Todo CreateTodo(string content, List<string> tags = null)
        {
            return new Todo(content, tags);
        }
        public LogEnum AddTodo(string content, List<string> tags = null)
        {
            if (!string.IsNullOrEmpty(content))
            {
                var todo = CreateTodo(content, tags);
                _todos.Add(todo);
                AddCaches(todo, todo.tags);
                return LogEnum.AddSuccess;
            }
            return LogEnum.NoParameter;
        }
        public LogEnum DelTodo(int index)
        {
            if (_cacheTodos.ContainsKey(SpecialTag.Task))
            {
                if (index >= 0 && index < _cacheTodos[SpecialTag.Task].Count)
                {
                    var todo = _cacheTodos[SpecialTag.Task][index];
                    _todos.Remove(todo);
                    DelCaches(todo, todo.tags);
                    return LogEnum.DelSuccess;
                }
            }
            return LogEnum.ParamerterIllegal;
        }
        public LogEnum AddTags(int index, List<string> tags)
        {
            if (_cacheTodos.ContainsKey(SpecialTag.Task))
            {
                if (index >= 0 && index < _cacheTodos[SpecialTag.Task].Count)
                {
                    var todo = _cacheTodos[SpecialTag.Task][index];
                    AddCaches(todo, tags);
                    return todo.AddTags(tags);
                }
            }
            return LogEnum.ParamerterIllegal;
        }
        public LogEnum DelTags(int index, List<string> tags)
        {
            if (_cacheTodos.ContainsKey(SpecialTag.Task))
            {
                if (index >= 0 && index < _cacheTodos[SpecialTag.Task].Count)
                {
                    var todo = _cacheTodos[SpecialTag.Task][index];
                    DelCaches(todo, tags);
                    return todo.DelTags(tags);
                }
            }
            return LogEnum.ParamerterIllegal;
        }
        public LogEnum DoneTodo(int index)
        {
            if (_cacheTodos.ContainsKey(SpecialTag.Task))
            {
                if (index >= 0 && index < _cacheTodos[SpecialTag.Task].Count)
                {
                    var todo = _cacheTodos[SpecialTag.Task][index];
                    DelCache(todo, SpecialTag.Task);
                    AddCache(todo, SpecialTag.Done);
                    todo.DoneTodo();
                }
            }
            return LogEnum.None;
        }
        public LogEnum ShowList(string tag, bool has)
        {
            if (_cacheTodos.ContainsKey(tag))
            {
                string todoTemp = @"{0}. {1}";
                if (_cacheTodos[tag].Count != 0)
                {
                    for (int i = 0; i < _cacheTodos[tag].Count; i++)
                    {
                        LogMgr.Instance.Log(string.Format(todoTemp, i, _cacheTodos[tag][i]));
                    }
                    return LogEnum.None;
                }
                return LogEnum.ListAllDone;
            }
            return LogEnum.ListAllDone;
        }
        public LogEnum ShowTask()
        {
            return ShowList(SpecialTag.Task, true);
        }
        public LogEnum ShowDone()
        {
            return ShowList(SpecialTag.Done, true);
        }
        public LogEnum ShowHasTag(string tag)
        {
            return ShowList(tag, true);
        }
        public LogEnum ClearTodo(List<string> tags)
        {
            if (tags.Count != 0)
            {
                foreach (var tag in tags)
                {
                    if (_cacheTodos.ContainsKey(tag))
                    {
                        foreach(var todo in _cacheTodos[tag])
                        {
                            _todos.Remove(todo);
                        }
                        _cacheTodos[tag].Clear();
                    }
                }
            }
            else
            {
                _todos.Clear();
                _cacheTodos.Clear();
            }
            return LogEnum.None;
        }
        public string[] ToFileString()
        {
            string[] result = new string[_todos.Count];
            for (int i = 0; i < _todos.Count; i++)
            {
                result[i] = _todos[i].ToFileString();
            }
            return result;
        }
    }
}