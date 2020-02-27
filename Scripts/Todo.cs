using System.Collections.Generic;

namespace Todo
{
    public class Todo
    {
        public HashSet<string> tags = new HashSet<string>();
        public string content;
        public Todo(string content, List<string> tags = null)
        {
            this.content = content;
            if (tags == null || tags.Count == 0)
            {
                AddTag("收集箱"); // 默认tag
            }
            else
            {
                AddTags(tags);
            }
        }
        public void AddTag(string tag)
        {
            if (!tags.Contains(tag))
            {
                tags.Add(tag);
            }
        }
        public void DelTag(string tag)
        {
            if (tags.Contains(tag))
            {
                tags.Remove(tag);
            }
        }
        public void AddTags(List<string> tags)
        {
            if (tags != null && tags.Count != 0)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    AddTag(tags[i]);
                }
                LogMgr.Instance.SystemLog(LogEnum.AddTagsSuccess);
            }
            else
            {
                LogMgr.Instance.SystemLog(LogEnum.NoTag);
            }
        }
        public void DelTags(List<string> tags)
        {
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    DelTag(tags[i]);
                }
                LogMgr.Instance.SystemLog(LogEnum.DelTagsSuccess);
            }
            else
            {
                LogMgr.Instance.SystemLog(LogEnum.NoTag);
            }
        }
        public override string ToString()
        {
            string str = "";
            foreach (var tag in tags)
            {
                if (tag != "收集箱")
                {
                    str += "[" + tag + "] ";
                }
            }
            return str + content;
        }
    }
}