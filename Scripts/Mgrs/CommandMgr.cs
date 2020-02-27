using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Todo
{
    public class CommandMgr : InstanceMgr<CommandMgr>
    {
        private Dictionary<CommandType, ICommand> _commandDir = new Dictionary<CommandType, ICommand>()
        {
            {CommandType.SetSaveDirectory, new SetSaveDirectoryCommand()},
            {CommandType.AddTodo, new AddTodoCommand()},
            {CommandType.DelTodo, new DelTodoCommand()},
            {CommandType.ShowTodos, new ShowTodosCommand()},
            {CommandType.Help, new HelpCommand()},
            {CommandType.AddTags, new AddTagsCommand()},
            {CommandType.DelTags, new DelTagsCommand()},
        };
        private Dictionary<string, CommandType> _strToCommand = new Dictionary<string, CommandType>()
        {
            {"setpath", CommandType.SetSaveDirectory},
            {"add", CommandType.AddTodo},
            {"del", CommandType.DelTodo},
            {"list", CommandType.ShowTodos},
            {"help", CommandType.Help},
            {"addt", CommandType.AddTags},
            {"delt", CommandType.DelTags},
        };
        public List<string> GetCommandList()
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
            return _commandDir[_strToCommand[comStr]].HelpTips();
        }

        public void ExecuteCommand(string commandStr)
        {
            string command;
            List<string> param;
            bool can = CommandAnalyze(commandStr, out command, out param);
            if (can)
            {
                ExecuteCommand(_strToCommand[command], param.ToList());
            }
            else
            {
                LogMgr.Instance.SystemLog(LogEnum.CommandNotHave);
            }
        }

        private void ExecuteCommand(CommandType type, List<string> param = null)
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

        private bool CommandAnalyze(string commandStr, out string command, out List<string> param)
        {
            command = "";
            param = new List<string>();
            List<string> commands = commandStr.Split(' ').ToList();
            for (int i = 0; i < commands.Count; i++)
            {
                if (_strToCommand.ContainsKey(commands[i]))
                {
                    command = commands[i];
                    commands.RemoveAt(i);
                    param = commands;
                    return true;
                }
            }
            return false;
        }
    }
}