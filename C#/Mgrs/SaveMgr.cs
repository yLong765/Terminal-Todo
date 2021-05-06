using System.IO;
namespace Todo
{
    public class SaveMgr : InstanceMgr<SaveMgr>
    {
        public static readonly string savePath = "todo.conf";
        public void WriteTodoFile()
        {
            string fullPath = Path.GetFullPath(savePath);
            File.WriteAllLines(fullPath, TodoMgr.Instance.ToFileString());
        }

        public string[] ReadTodoFile()
        {
            string fullPath = Path.GetFullPath(savePath);
            if (File.Exists(fullPath))
            {
                return File.ReadAllLines(fullPath);
            }
            return null;
        }

        public void LoadFile()
        {
            var todos = ReadTodoFile();
            if (todos != null)
            {
                for (int i = 0; i < todos.Length; i++)
                {
                    CommandMgr.Instance.ExecuteCommand("add " + todos[i]);
                }
            }
        }
    }
}