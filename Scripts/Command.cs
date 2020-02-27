using System.Windows.Input;
using System.Collections.Generic;

namespace Todo
{
    public interface ICommand
    {
        public void Execute(List<string> param) { }
        public string HelpTips() { return ""; }
    }

    public enum CommandType
    {
        SetSaveDirectory = 1,
        AddTodo = 2,
        DelTodo = 3,
        ShowTodos = 4,
        Help = 5,
        AddTags = 6,
        DelTags = 7,
    }

    public static class CommandUtility
    {
        public static bool CheckHasParams(List<string> param)
        {
            if (param == null || param.Count == 0)
            {
                LogMgr.Instance.SystemLog(LogEnum.NoParameter);
                return false;
            }
            return true;
        }

        public static List<string> GetTags(List<string> param)
        {
            List<string> tags = new List<string>();
            for (int i = 0; i < param.Count; i++)
            {
                if (param[i].StartsWith('[') && param[i].EndsWith(']'))
                {
                    tags.Add(param[i].Substring(1, param[i].Length - 2));
                    param.RemoveAt(i);
                }
            }
            return tags;
        }

        public static string GetContent(List<string> param)
        {
            for (int i = 0; i < param.Count; i++)
            {
                if (!param[i].StartsWith('[') || !param[i].EndsWith(']'))
                {
                    return param[i];
                }
            }
            return "";
        }

        public static int GetIndex(List<string> param)
        {
            for (int i = 0; i < param.Count; i++)
            {
                int result;
                if (int.TryParse(param[i], out result))
                {
                    return result;
                }
            }
            return -1;
        }
    }

    public class SetSaveDirectoryCommand : ICommand
    {
        public void Execute(List<string> param)
        {

        }

        public string HelpTips()
        {
            return "设置存储路径";
        }
    }

    public class AddTodoCommand : ICommand
    {
        public void Execute(List<string> param)
        {
            if (CommandUtility.CheckHasParams(param))
            {
                var content = CommandUtility.GetContent(param);
                var tags = CommandUtility.GetTags(param);
                TodoMgr.Instance.AddTodo(content, tags);
            }
            SaveMgr.Instance.WriteTodoFile();
        }

        public string HelpTips()
        {
            return "添加新Todo";
        }
    }

    public class DelTodoCommand : ICommand
    {
        public void Execute(List<string> param)
        {
            if (CommandUtility.CheckHasParams(param))
            {
                var index = CommandUtility.GetIndex(param);
                TodoMgr.Instance.DelTodo(index);
            }
            SaveMgr.Instance.WriteTodoFile();
        }

        public string HelpTips()
        {
            return "删除对应Id的Todo";
        }
    }

    public class ShowTodosCommand : ICommand
    {
        public void Execute(List<string> param)
        {
            var tags = CommandUtility.GetTags(param);
            if (tags.Count == 0)
            {
                TodoMgr.Instance.SearchTodo();
            }
            else if (tags.Count == 1)
            {
                TodoMgr.Instance.SearchTodo(tags[0], true);
            }
        }

        public string HelpTips()
        {
            return "展示全部Todo";
        }
    }

    public class HelpCommand : ICommand
    {
        public void Execute(List<string> param)
        {
            var comStrs = CommandMgr.Instance.GetCommandList();
            LogMgr.Instance.Log("示例：<命令> [<参数>(可多个参数)]");
            LogMgr.Instance.Log("可用命令(无视大小写)：\n");
            for (int i = 0; i < comStrs.Count; i++)
            {
                LogMgr.Instance.Log(comStrs[i].PadRight(15) + CommandMgr.Instance.GetHelpTips(comStrs[i]));
            }
        }

        public string HelpTips()
        {
            return "显示全部命令";
        }
    }

    public class ExitCommand : ICommand
    {
        public void Execute(List<string> param)
        {

        }

        public string HelpTips()
        {
            return "";
        }
    }

    public class AddTagsCommand : ICommand
    {
        public void Execute(List<string> param)
        {
            if (CommandUtility.CheckHasParams(param))
            {
                var index = CommandUtility.GetIndex(param);
                var tags = CommandUtility.GetTags(param);
                TodoMgr.Instance.AddTags(index, tags);
            }
            SaveMgr.Instance.WriteTodoFile();
        }

        public string HelpTips()
        {
            return "添加标签";
        }
    }

    public class DelTagsCommand : ICommand
    {
        public void Execute(List<string> param)
        {
            if (CommandUtility.CheckHasParams(param))
            {
                var index = CommandUtility.GetIndex(param);
                var tags = CommandUtility.GetTags(param);
                TodoMgr.Instance.DelTags(index, tags);
            }
            SaveMgr.Instance.WriteTodoFile();
        }

        public string HelpTips()
        {
            return "删除标签";
        }
    }
}
