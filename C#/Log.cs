namespace Todo
{
    public enum LogType
    {
        Normal,
        Warning,
        Error,
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
                default:
                    chief = "  ";
                    break;
            }
            return chief + content;
        }
    }
}