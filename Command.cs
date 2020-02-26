using System;
using System.Collections.Generic;

namespace Todo
{
    public interface ICommand
    {
        public void Execute(params string[] param) { }
        public string HelpTips() { return ""; }
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
        public List<string> GetCommandStrs()
        {
            List<string> comStrs = new List<string>();
            foreach (var stc in _strToCommand)
            {
                comStrs.Add(stc.Key);
            }
            return comStrs;
        }
        public string GetHelpTips(string comStr)
        {
            return _commandDir[_strToCommand[comStr.ToLower()]].HelpTips();
        }

        private CommandMgr()
        {
            _commandDir.Add(CommandType.SetSaveDirectory, new SetSaveDirectoryCommand());
            _commandDir.Add(CommandType.AddTodo, new AddTodoCommand());
            _commandDir.Add(CommandType.DelTodo, new DelTodoCommand());
            _commandDir.Add(CommandType.ShowTodos, new ShowTodosCommand());
            _commandDir.Add(CommandType.Help, new HelpCommand());
            _strToCommand.Add("setpath", CommandType.SetSaveDirectory);
            _strToCommand.Add("add", CommandType.AddTodo);
            _strToCommand.Add("del", CommandType.DelTodo);
            _strToCommand.Add("list", CommandType.ShowTodos);
            _strToCommand.Add("help", CommandType.Help);
        }

        public void ExecuteCommand(params string[] param)
        {
            var commandStr = param[0] as string;
            commandStr = commandStr.ToLower();
            if (_strToCommand.ContainsKey(commandStr))
            {
                string[] p = new string[param.Length - 1];
                if (param.Length > 1) Array.Copy(param, 1, p, 0, param.Length - 1);
                ExecuteCommand(_strToCommand[commandStr], p);
            }
            else
            {
                LogMgr.SystemLog(LogEnum.CommandNotHave);
            }
        }

        private void ExecuteCommand(CommandType type, params string[] param)
        {
            if (_commandDir.ContainsKey(type))
            {
                ICommand command = _commandDir[type];
                if (command != null)
                {
                    command.Execute(param);
                }
            }
        }
    }

    public class SetSaveDirectoryCommand : ICommand
    {
        public void Execute(params string[] param)
        {
            string path = param[0];
            if (!string.IsNullOrEmpty(path))
            {
                Blackboard.SavePath = path;
                LogMgr.SystemLog(LogEnum.SetSavePathSuccess);
            }
        }

        public string HelpTips()
        {
            return "设置存储路径";
        }
    }

    public class AddTodoCommand : ICommand
    {
        public void Execute(params string[] param)
        {
            if (param.Length > 0)
            {
                if (param.Length > 1)
                for (int i = 0; i < param.Length - 1; i++)
                {
                    
                }
                string content = param[param.Length - 1];
                if (!string.IsNullOrEmpty(content))
                {
                    TodoMgr.AddTodo(content);
                }
            }
            else
            {
                LogMgr.SystemLog(LogEnum.NoParameter);
            }
        }

        public string HelpTips()
        {
            return "添加新Todo";
        }
    }

    public class DelTodoCommand : ICommand
    {
        public void Execute(params string[] param)
        {
            if (param.Length > 0)
            {
                int index;
                bool b = int.TryParse(param[0], out index);
                if (b)
                {
                    TodoMgr.DelTodo(index);
                }
                else
                {
                    LogMgr.SystemLog(LogEnum.DelFailed2); //Console.WriteLine("  删除失败, 错误：参数不为数字");
                }
            }
            else
            {
                LogMgr.SystemLog(LogEnum.NoParameter);
            }
        }

        public string HelpTips()
        {
            return "删除对应Id的Todo";
        }
    }

    public class ShowTodosCommand : ICommand
    {
        public void Execute(params string[] param)
        {
            TodoMgr.ShowAllTodo();
        }

        public string HelpTips()
        {
            return "展示全部Todo";
        }
    }

    public class HelpCommand : ICommand
    {
        public void Execute(params string[] param)
        {
            var comStrs = CommandMgr.Instance.GetCommandStrs();
            LogMgr.Log("示例：<命令> [<参数>(可多个参数)]");
            LogMgr.Log("可用命令(无视大小写)：\n");
            for (int i = 0; i < comStrs.Count; i++)
            {
                LogMgr.Log(comStrs[i].PadRight(15) + CommandMgr.Instance.GetHelpTips(comStrs[i]));
            }
        }

        public string HelpTips()
        {
            return "显示全部命令";
        }
    }
}
