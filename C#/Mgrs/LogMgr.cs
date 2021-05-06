using System;
using System.Collections.Generic;

namespace Todo
{
    public enum LogEnum : uint
    {
        None = 0,
        // Command
        CommandNotHave = 501,
        NoParameter = 502,
        ParamerterIllegal = 503,
        // SetSavePathCommand
        SetSavePathSuccess = 1001,
        // AddCommand
        AddSuccess = 1010,
        // DelCommand
        DelSuccess = 1020,
        // ListCommand
        ListAllDone = 1030,
        // AddTagsCommand
        AddTagsSuccess = 1040,
        DelTagsSuccess = 1041,
        NoTag = 1042,
    }

    public class LogMgr : InstanceMgr<LogMgr>
    {
        public bool CanOutPutLog = true;
        //private static readonly string _logFilePath = "debug.log";
        private Dictionary<LogEnum, Log> _logs = new Dictionary<LogEnum, Log>()
        {
            {LogEnum.None, new Log {type = LogType.Normal, content = ""}},
            {LogEnum.CommandNotHave, new Log {type = LogType.Error, content = "命令不存在，请输入help来查看全部命令"}},
            {LogEnum.NoParameter, new Log {type = LogType.Warning, content = "No Oaram"}},
            {LogEnum.ParamerterIllegal, new Log {type = LogType.Error, content = "Param Illegal"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Normal, content = "设置路径成功"}},
            {LogEnum.AddSuccess, new Log {type = LogType.Normal, content = "Add Todo Success"}},
            {LogEnum.DelSuccess, new Log {type = LogType.Normal, content = "Del Todo Success"}},
            {LogEnum.ListAllDone, new Log {type = LogType.Normal, content = "No Todo, Enjory!"}},
            {LogEnum.AddTagsSuccess, new Log {type = LogType.Normal, content = "Add Tags Success"}},
            {LogEnum.DelTagsSuccess, new Log {type = LogType.Normal, content = "Del Tags Success"}},
            {LogEnum.NoTag, new Log {type = LogType.Warning, content = "No Tag"}},
        };

        private void WriteInLogFile(string log)
        {
            if (CanOutPutLog)
            {
                Console.WriteLine(log);
            }
        }

        public void SystemLog(LogEnum id = LogEnum.None)
        {
            if (id != LogEnum.None)
            {
                WriteInLogFile(_logs[id].ToString());
            }
        }

        public void Error(string log)
        {
            log = "  错误：" + log;
            WriteInLogFile(log);
        }

        public void Warning(string log)
        {
            log = "  警告：" + log;
            WriteInLogFile(log);
        }

        public void Log(string log)
        {
            log = "  " + log;
            WriteInLogFile(log);
        }

    }
}