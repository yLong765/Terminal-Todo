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
        //private static readonly string _logFilePath = "debug.log";
        private Dictionary<LogEnum, Log> _logs = new Dictionary<LogEnum, Log>()
        {
            {LogEnum.None, new Log {type = LogType.Normal, content = ""}},
            {LogEnum.CommandNotHave, new Log {type = LogType.Error, content = "命令不存在，请输入help来查看全部命令"}},
            {LogEnum.NoParameter, new Log {type = LogType.Warning, content = "无参数"}},
            {LogEnum.ParamerterIllegal, new Log {type = LogType.Error, content = "参数非法"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Normal, content = "设置路径成功"}},
            {LogEnum.AddSuccess, new Log {type = LogType.Normal, content = "添加成功"}},
            {LogEnum.DelSuccess, new Log {type = LogType.Normal, content = "删除成功"}},
            {LogEnum.ListAllDone, new Log {type = LogType.Normal, content = "恭喜！Todo全部完成"}},
            {LogEnum.AddTagsSuccess, new Log {type = LogType.Normal, content = "添加标签成功"}},
            {LogEnum.DelTagsSuccess, new Log {type = LogType.Normal, content = "删除标签成功"}},
            {LogEnum.NoTag, new Log {type = LogType.Warning, content = "无标签"}},
        };

        private void WriteInLogFile(string log)
        {
            Console.WriteLine(log);
        }

        public void SystemLog(LogEnum id = LogEnum.None)
        {
            WriteInLogFile(_logs[id].ToString());
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