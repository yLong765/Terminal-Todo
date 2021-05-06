using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Todo
{
    public static class SpecialTag
    {
        public static readonly string Task = "任务";
        public static readonly string Done = "已完成";
    }

    public class Todo
    {
        public List<string> tags = new List<string>();
        public string content;
        public Todo(string content, List<string> tags = null)
        {
            this.content = content;
            AddTags(tags);
            if (!tags.Contains(SpecialTag.Done))
            {
                AddTag(SpecialTag.Task);
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
        public LogEnum AddTags(List<string> tags)
        {
            if (tags != null && tags.Count != 0)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    AddTag(tags[i]);
                }
                return LogEnum.AddTagsSuccess;
            }
            return LogEnum.NoTag;
        }
        public LogEnum DelTags(List<string> tags)
        {
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    DelTag(tags[i]);
                }
                return LogEnum.DelTagsSuccess;
            }
            return LogEnum.NoTag;
        }
        public void DoneTodo()
        {
            DelTag(SpecialTag.Task);
            AddTag(SpecialTag.Done);
        }
        public string ToFileString()
        {
            string str = "";
            foreach (var tag in tags)
            {
                if (tag != SpecialTag.Task)
                {
                    str += "[" + tag + "] ";
                }
            }
            return str + content;
        }
        public override string ToString()
        {
            string str = "";
            foreach (var tag in tags)
            {
                if (tag != SpecialTag.Task && tag != SpecialTag.Done)
                {
                    str += "[" + tag + "] ";
                }
            }
            return str + content;
        }
    }
}