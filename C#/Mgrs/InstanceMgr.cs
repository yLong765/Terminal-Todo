namespace Todo
{
    public class InstanceMgr<T> where T : new()
    {
        private static T _instance;
        public static T Instance { get { return _instance; } }
        public static void Create()
        {
            _instance = new T();
        }
    }
}