using System;
using System.Collections.Generic;

namespace Todo
{
    public interface ICommand
    {
        public void Execute(params object[] param) { }
        //public string HelpTips() { return ""; }
    }

    public enum CommandType
    {
        SetSaveDirectory = 1,
        AddTodo = 2,
        DelTodo = 3,
        ShowTodos = 4,
        Help = 5,
    }

    public class CommandMgr
    {
        public static CommandMgr Instance { get { return _instance; } }
        private static CommandMgr _instance = new CommandMgr();

        private Dictionary<CommandType, ICommand> _commandDir = new Dictionary<CommandType, ICommand>();
        private Dictionary<string, CommandType> _strToCommand = new Dictionary<string, CommandType>();

        private CommandMgr()
        {
            _commandDir.Add(CommandType.SetSaveDirectory, new SetSaveDirectoryCommand());
            _commandDir.Add(CommandType.AddTodo, new AddTodoCommand());
            _commandDir.Add(CommandType.DelTodo, new DelTodoCommand());
            _commandDir.Add(CommandType.ShowTodos, new ShowTodosCommand());
            _strToCommand.Add("setsavedirectory", CommandType.SetSaveDirectory);
            _strToCommand.Add("add", CommandType.AddTodo);
            _strToCommand.Add("del", CommandType.DelTodo);
            _strToCommand.Add("list", CommandType.ShowTodos);
        }

        public void ExecuteCommand(params object[] param)
        {
            var commandStr = param[0] as string;
            if (_strToCommand.ContainsKey(commandStr))
            {
                object[] p = new object[param.Length - 1];
                if (p.Length != 0)
                {
                    param.CopyTo(p, 1);
                }
                ExecuteCommand(_strToCommand[commandStr], p);
            }
        }

        private void ExecuteCommand(CommandType type, params object[] param)
        {
            if (_commandDir.ContainsKey(type))
            {
                ICommand command = _commandDir[type];
                if (command != null)
                {
                    command.Execute(param);
                }
            }
            else
            {
                Console.WriteLine("  命令不存在");
            }
        }
    }

    public class SetSaveDirectoryCommand : ICommand
    {
        public void Execute(params object[] param)
        {
            string path = param[0] as string;
            if (!string.IsNullOrEmpty(path))
            {
                Blackboard.SavePath = path;
                Console.WriteLine("  设置路径成功");
            }
        }
    }

    public class AddTodoCommand : ICommand
    {
        public void Execute(params object[] param)
        {
            string todoStr = param[0] as string;
            if (!string.IsNullOrEmpty(todoStr))
            {
                Blackboard.ToDos.Add(todoStr);
                Console.WriteLine("  添加成功");
            }
        }
    }

    public class DelTodoCommand : ICommand
    {
        public void Execute(params object[] param)
        {
            int index;
            bool b = int.TryParse(param[0] as string, out index);
            if (b)
            {
                if (index >= 0 && index < Blackboard.ToDos.Count)
                {
                    Blackboard.ToDos.RemoveAt(index);
                    Console.WriteLine("  删除成功");
                }
                else
                {
                    Console.WriteLine("  删除失败, 错误：序号不存在");
                }
            }
            else
            {
                Console.WriteLine("  删除失败, 错误：参数不为数字");
            }
        }
    }

    public class ShowTodosCommand : ICommand
    {
        public void Execute(params object[] param)
        {
            if (Blackboard.ToDos.Count > 0)
            {
                string todoTemp = @"  {0}.  {1}";
                for (int i = 0; i < Blackboard.ToDos.Count; i++)
                {
                    Console.WriteLine(string.Format(todoTemp, i, Blackboard.ToDos[i]));
                }
            }
            else
            {
                Console.WriteLine("  恭喜！Todo全部完成");
            }
        }
    }
}
