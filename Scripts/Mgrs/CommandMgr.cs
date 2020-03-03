using System.Linq;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Todo
{
    public class CommandMgr : InstanceMgr<CommandMgr>
    {
        private Dictionary<CommandType, CommandBase> _commandDir = new Dictionary<CommandType, CommandBase>()
        {
            {CommandType.SetSaveDirectory, new SetSaveDirectoryCommand()},
            {CommandType.AddTodo, new AddTodoCommand()},
            {CommandType.DelTodo, new DelTodoCommand()},
            {CommandType.ShowTodos, new ShowTodosCommand()},
            {CommandType.Help, new HelpCommand()},
            {CommandType.AddTags, new AddTagsCommand()},
            {CommandType.DelTags, new DelTagsCommand()},
            {CommandType.DoneTodo, new DoneTodoCommand()},
            {CommandType.ClearTodo, new ClearTodoCommand()},
            //{CommandType.Sort, new ClearTodoCommand()},
            //{CommandType.Swap, new ClearTodoCommand()},
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
            {"done", CommandType.DoneTodo},
            {"clear", CommandType.ClearTodo},
            //{"sort", CommandType.Sort},
            //{"swap", CommandType.Swap},
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

        public void ExecuteCommand(CommandType type, List<string> param = null)
        {
            if (_commandDir.ContainsKey(type))
            {
                CommandBase command = _commandDir[type];
                if (command != null)
                {
                    command.InitCommand(param);
                    var log = command.Execute();
                    LogMgr.Instance.SystemLog(log);
                    command.WriteConfig();
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