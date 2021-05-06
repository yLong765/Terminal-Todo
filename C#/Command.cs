using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;

namespace Todo
{
    public class CommandBase
    {
        private List<string> param;

        private List<string> tags;
        private string content;
        private List<int> indexs;

        public List<string> Tags { get { return tags; } }
        public List<int> Indexs { get { return indexs; } }
        public int Index0 { get { if (indexs.Count >= 1) return indexs[0]; return -1; } }
        public int Index1 { get { if (indexs.Count >= 2) return indexs[1]; return -1; } }
        public string Content { get { return content; } }

        public bool InitCommand(List<string> param)
        {
            this.param = param;
            this.tags = new List<string>();
            this.indexs = new List<int>();
            this.content = "";
            if (CheckParam())
            {
                tags = GetTags();
                indexs = GetIndexs();
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
            List<int> removeTags = new List<int>();
            for (int i = 0; i < param.Count; i++)
            {
                if (param[i].StartsWith('[') && param[i].EndsWith(']'))
                {
                    tags.Add(param[i].Substring(1, param[i].Length - 2));
                    removeTags.Add(i);
                }
            }
            for (int i = removeTags.Count - 1; i >= 0; i--)
            {
                param.RemoveAt(removeTags[i]);
            }
            return tags;
        }

        private List<int> GetIndexs()
        {
            List<int> indexs = new List<int>();
            for (int i = 0; i < param.Count; i++)
            {
                int result;
                if (int.TryParse(param[i], out result))
                {
                    indexs.Add(result);
                }
            }
            return indexs;
        }

        private string GetContent()
        {
            if (param.Count == 1)
            {
                return param[0];
            }
            return "";
        }

        public virtual LogEnum Execute() { return LogEnum.None; }
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
        ClearTodo = 9,
        Restore = 10,
        Sort = 11,
        Swap = 12,
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
        public override string HelpTips()
        {
            return "设置存储路径";
        }
    }

    public class AddTodoCommand : CommandBase
    {
        public override LogEnum Execute()
        {
            TodoMgr.Instance.AddTodo(Content, Tags);
            CommandMgr.Instance.ExecuteCommand(CommandType.ShowTodos);
            return LogEnum.None;
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
        public override LogEnum Execute()
        {
            TodoMgr.Instance.DelTodo(Index0);
            CommandMgr.Instance.ExecuteCommand(CommandType.ShowTodos);
            return LogEnum.None;
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
        public override LogEnum Execute()
        {
            LogEnum log = LogEnum.None;
            if (Content == "done")
            {
                log = TodoMgr.Instance.ShowDone();
            }
            else if (Tags == null || Tags.Count == 0)
            {
                log = TodoMgr.Instance.ShowTask();
            }
            else if (Tags.Count == 1)
            {
                log = TodoMgr.Instance.ShowHasTag(Tags[0]);
            }
            return log;
        }

        public override string HelpTips()
        {
            return "展示全部Todo";
        }
    }

    public class HelpCommand : CommandBase
    {
        public override LogEnum Execute()
        {
            var comStrs = CommandMgr.Instance.GetCommandList();
            LogMgr.Instance.Log("示例：<命令> [<参数>(可多个参数)]");
            LogMgr.Instance.Log("可用命令(无视大小写)：\n");
            for (int i = 0; i < comStrs.Count; i++)
            {
                LogMgr.Instance.Log(comStrs[i].PadRight(15) + CommandMgr.Instance.GetHelpTips(comStrs[i]));
            }
            return LogEnum.None;
        }

        public override string HelpTips()
        {
            return "显示全部命令";
        }
    }

    public class AddTagsCommand : CommandBase
    {
        public override LogEnum Execute()
        {
            TodoMgr.Instance.AddTags(Index0, Tags);
            CommandMgr.Instance.ExecuteCommand(CommandType.ShowTodos);
            return LogEnum.None;
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
        public override LogEnum Execute()
        {
            TodoMgr.Instance.DelTags(Index0, Tags);
            CommandMgr.Instance.ExecuteCommand(CommandType.ShowTodos);
            return LogEnum.None;
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
        public override LogEnum Execute()
        {
            TodoMgr.Instance.DoneTodo(Index0);
            CommandMgr.Instance.ExecuteCommand(CommandType.ShowTodos);
            return LogEnum.None;
        }

        public override string HelpTips()
        {
            return "完成Todo(添加'已完成'标签)";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }

    public class ClearTodoCommand : CommandBase
    {
        public override LogEnum Execute()
        {
            if (Content == "done")
            {
                TodoMgr.Instance.ClearTodo(new List<string> { SpecialTag.Done });
            }
            else
            {
                TodoMgr.Instance.ClearTodo(Tags);
            }
            CommandMgr.Instance.ExecuteCommand(CommandType.ShowTodos);
            return LogEnum.None;
        }

        public override string HelpTips()
        {
            return "清除对应标签的Todo(无标签则全部清除)";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }

    public class SortCommand : CommandBase
    {
        public override LogEnum Execute()
        {
            return LogEnum.None;
        }

        public override string HelpTips()
        {
            return "排序Todo";
        }

        public override bool NeedWrite()
        {
            return true;
        }
    }
    #endregion
}
