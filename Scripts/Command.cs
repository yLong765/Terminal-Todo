using System.Windows.Input;
using System.Collections.Generic;

namespace Todo
{
    public class CommandBase
    {
        private List<string> param;

        private bool nilTags = true;
        private bool hasTags = false;
        private List<string> tags;

        private bool nilContent = true;
        private bool hasContent = false;
        private string content;

        private bool nilIndex = true;
        private bool hasIndex = false;
        private int index;

        public List<string> Tags
        {
            get
            {
                if (hasTags)
                {
                    return tags;
                }
                if (!nilTags)
                {
                    LogMgr.Instance.SystemLog(LogEnum.NoTag);
                }
                return new List<string>();
            }
        }
        public int Index
        {
            get
            {
                if (hasIndex)
                {
                    return index;
                }
                if (!nilIndex)
                {
                    LogMgr.Instance.SystemLog(LogEnum.ParamerterIllegal);
                }
                return -1;
            }
        }
        public string Content
        {
            get
            {
                if (hasContent)
                {
                    return content;
                }
                if (!nilContent)
                {
                    LogMgr.Instance.SystemLog(LogEnum.NoParameter);
                }
                return "";
            }
        }

        public bool InitCommand(List<string> param)
        {
            this.param = param;
            if (CheckParam())
            {
                tags = GetTags();
                index = GetIndex();
                content = GetContent();
                return true;
            }
            return false;
        }

        public void WriteConfig()
        {
            if (NeedWrite())
            {
                SaveMgr.Instance.WriteTodoFile();
            }
        }

        private bool CheckParam()
        {
            if (param == null || param.Count == 0)
            {
                //LogMgr.Instance.SystemLog(LogEnum.NoParameter);
                return false;
            }
            return true;
        }

        private List<string> GetTags()
        {
            List<string> tags = new List<string>();
            for (int i = 0; i < param.Count; i++)
            {
                if (param[i].StartsWith('[') && param[i].EndsWith(']'))
                {
                    hasTags = true;
                    tags.Add(param[i].Substring(1, param[i].Length - 2));
                    param.RemoveAt(i);
                }
            }
            return tags;
        }

        private int GetIndex()
        {
            for (int i = 0; i < param.Count; i++)
            {
                int result;
                if (int.TryParse(param[i], out result))
                {
                    hasIndex = true;
                    param.RemoveAt(i);
                    return result;
                }
            }
            return -1;
        }

        private string GetContent()
        {
            if (param.Count == 1)
            {
                hasContent = true;
                return param[0];
            }
            return "";
        }

        public virtual void Execute() { }
        public virtual string HelpTips() { return ""; }
        public virtual bool NeedWrite() { return false; }
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
        DoneTodo = 8,
    }
    #region 工具类
    // public static class CommandUtility
    // {
    //     public static bool CheckHasParams(List<string> param)
    //     {
    //         if (param == null || param.Count == 0)
    //         {
    //             LogMgr.Instance.SystemLog(LogEnum.NoParameter);
    //             return false;
    //         }
    //         return true;
    //     }

    //     public static List<string> GetTags(List<string> param)
    //     {
    //         List<string> tags = new List<string>();
    //         for (int i = 0; i < param.Count; i++)
    //         {
    //             if (param[i].StartsWith('[') && param[i].EndsWith(']'))
    //             {
    //                 tags.Add(param[i].Substring(1, param[i].Length - 2));
    //                 param.RemoveAt(i);
    //             }
    //         }
    //         return tags;
    //     }

    //     public static string GetContent(List<string> param)
    //     {
    //         for (int i = 0; i < param.Count; i++)
    //         {
    //             if (!param[i].StartsWith('[') || !param[i].EndsWith(']'))
    //             {
    //                 return param[i];
    //             }
    //         }
    //         return "";
    //     }

    //     public static int GetIndex(List<string> param)
    //     {
    //         for (int i = 0; i < param.Count; i++)
    //         {
    //             int result;
    //             if (int.TryParse(param[i], out result))
    //             {
    //                 return result;
    //             }
    //         }
    //         return -1;
    //     }
    // }
    #endregion

    #region Commands
    public class SetSaveDirectoryCommand : CommandBase
    {
        public override void Execute()
        {

        }

        public override string HelpTips()
        {
            return "设置存储路径";
        }
    }

    public class AddTodoCommand : CommandBase
    {
        public override void Execute()
        {
            TodoMgr.Instance.AddTodo(Content, Tags);
        }

        public override string HelpTips()
        {
            return "添加新Todo";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }

    public class DelTodoCommand : CommandBase
    {
        public override void Execute()
        {
            TodoMgr.Instance.DelTodo(Index);
        }

        public override string HelpTips()
        {
            return "删除对应Id的Todo";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }

    public class ShowTodosCommand : CommandBase
    {
        public override void Execute()
        {
            if (Tags.Count == 0)
            {
                TodoMgr.Instance.SearchTodo();
            }
            else if (Tags.Count == 1)
            {
                TodoMgr.Instance.SearchTodo(Tags[0], true);
            }
        }

        public override string HelpTips()
        {
            return "展示全部Todo";
        }
    }

    public class HelpCommand : CommandBase
    {
        public override void Execute()
        {
            var comStrs = CommandMgr.Instance.GetCommandList();
            LogMgr.Instance.Log("示例：<命令> [<参数>(可多个参数)]");
            LogMgr.Instance.Log("可用命令(无视大小写)：\n");
            for (int i = 0; i < comStrs.Count; i++)
            {
                LogMgr.Instance.Log(comStrs[i].PadRight(15) + CommandMgr.Instance.GetHelpTips(comStrs[i]));
            }
        }

        public override string HelpTips()
        {
            return "显示全部命令";
        }
    }

    public class ExitCommand : CommandBase
    {
        public override void Execute()
        {

        }

        public override string HelpTips()
        {
            return "";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }

    public class AddTagsCommand : CommandBase
    {
        public override void Execute()
        {
            TodoMgr.Instance.AddTags(Index, Tags);
        }

        public override string HelpTips()
        {
            return "添加标签";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }

    public class DelTagsCommand : CommandBase
    {
        public override void Execute()
        {
            TodoMgr.Instance.DelTags(Index, Tags);
        }

        public override string HelpTips()
        {
            return "删除标签";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }

    public class DoneTodoCommand : CommandBase
    {
        public override void Execute()
        {
            TodoMgr.Instance.DoneTodo(Index);
        }

        public override string HelpTips()
        {
            return "设置存储路径";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }
    #endregion
}
