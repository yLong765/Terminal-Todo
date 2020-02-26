using System;
using System.Collections.Generic;

namespace Todo
{
    public enum LogType
    {
        Normal,
        Warning,
        Error,
    }
    public enum LogEnum : uint
    {
        None = 0,
        // Command
        CommandNotHave = 501,
        NoParameter = 502,
        // SetSavePathCommand
        SetSavePathSuccess = 1001,
        // AddCommand
        AddSuccess = 1010,
        // DelCommand
        DelSuccess = 1020,
        DelFailed1 = 1021,
        DelFailed2 = 1022,
        // ListCommand
        ListAllDone = 1030,
    }

    public class Log
    {
        public LogType type;
        public string content;
        public override string ToString()
        {
            string chief = "";
            switch (type)
            {
                case LogType.Warning:
                    chief = "  警告：";
                    break;
                case LogType.Error:
                    chief = "  错误：";
                    break;
            }
            return chief + content;
        }
    }

    public static class LogMgr
    {
        private static readonly string _logFilePath = "debug.log";
        private static Dictionary<LogEnum, Log> _logStr = new Dictionary<LogEnum, Log>()
        {
            {LogEnum.None, new Log {type = LogType.Normal, content = ""}},
            {LogEnum.CommandNotHave, new Log {type = LogType.Warning, content = "命令不存在，请输入help来查看全部命令"}},
            {LogEnum.NoParameter, new Log {type = LogType.Error, content = "无参数"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Normal, content = "设置路径成功"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Normal, content = "添加成功"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Normal, content = "删除成功"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Error, content = "删除失败, 序号不存在"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Error, content = "删除失败, 参数不为数字"}},
            {LogEnum.SetSavePathSuccess, new Log {type = LogType.Normal, content = "恭喜！Todo全部完成"}},
        };

        private static void WriteInLogFile(string log)
        {
            Console.WriteLine(log);
        }

        public static void SystemLog(LogEnum id = LogEnum.None)
        {
            WriteInLogFile(_logStr[id].ToString());
        }

        public static void Error(string log)
        {
            log = "  错误：" + log;
            WriteInLogFile(log);
        }

        public static void Warning(string log)
        {
            log = "  警告：" + log;
            WriteInLogFile(log);
        }

        public static void Log(string log)
        {
            log = "  " + log;
            WriteInLogFile(log);
        }

    }
}