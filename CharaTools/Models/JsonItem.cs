using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace CharaTools.Models
{
    internal class JsonItem
    {
        #region Variables
        private JToken jToken = null;
        #endregion

        #region Properties
        public string Name { get; private set; } = string.Empty;

        public string Value { get; private set; } = string.Empty;

        public bool HasChildren { get; private set; }

        public int Level { get; private set; } = 0;

        public JTokenType ValueType { get; private set; } = JTokenType.Null;

        public bool IsArray
        {
            get => ValueType == JTokenType.Array;
        }

        public bool IsObject
        {
            get => ValueType == JTokenType.Object;
        }

        public bool IsValue
        {
            get => ValueType != JTokenType.Array && ValueType != JTokenType.Object;
        }

        public bool IsNull
        {
            get => ValueType == JTokenType.Null || ValueType == JTokenType.Undefined || ValueType == JTokenType.None;
        }

        public bool IsString
        {
            get => ValueType == JTokenType.String || ValueType == JTokenType.Uri || ValueType == JTokenType.Guid
                            || ValueType == JTokenType.Date || ValueType == JTokenType.TimeSpan;
        }

        public bool IsPrimitive
        {
            get => ValueType == JTokenType.Boolean || ValueType == JTokenType.Float || ValueType == JTokenType.Integer;
        }

        public bool IsNormal
        {
            get => !IsNull && !IsString && !IsPrimitive;
        }

        public Thickness LevelPadding
        {
            get
            {
                var level = Level - 1;
                return new Thickness(level * 19, 0, 0, 0);
            }
        }
        #endregion

        #region Constructor
        public JsonItem(string name, JToken item, int level)
        {
            jToken = item;

            string value = string.Empty;

            switch (item.Type)
            {
                case JTokenType.None:
                case JTokenType.Null:
                case JTokenType.Undefined:
                    value = "null";
                    break;
                case JTokenType.String:
                case JTokenType.Date:
                case JTokenType.Guid:
                case JTokenType.Uri:
                case JTokenType.TimeSpan:
                    value = $"\"{item}\"";
                    break;
                case JTokenType.Array:
                    value = $"[{item.Count()}]";
                    break;
                case JTokenType.Object:
                    break;
                case JTokenType.Integer:
                case JTokenType.Float:
                case JTokenType.Boolean:
                case JTokenType.Raw:
                case JTokenType.Bytes:
                    value = item.ToString();
                    break;
                default:
                    break;
            }

            Name = name;
            Value = string.IsNullOrEmpty(value) ? string.Empty : value;
            ValueType = item.Type;
            Level = level;
        }
        #endregion

        #region Methods
        public List<JsonItem> GetChildren()
        {
            var children = new List<JsonItem>();

            if (jToken.Type == JTokenType.Array)
                LoadArray(jToken, Level + 1, ref children);
            else if (jToken.Type == JTokenType.Object)
                LoadObject(jToken as JObject, Level + 1, ref children);

            return children;
        }

        private void LoadArray(JToken value, int level, ref List<JsonItem> list)
        {
            int i = 0;
            foreach (var item in value)
            {
                var name = $"#{i}";
                var child = new JsonItem(name, item, level);
                list.Add(child);

                if (item.Type == JTokenType.Array || item.Type == JTokenType.Object)
                {
                    var children = child.GetChildren();
                    child.HasChildren = children.Count > 0;
                    list.AddRange(children);
                }

                i++;
            }
        }

        private void LoadObject(JObject obj, int level, ref List<JsonItem> list)
        {
            foreach (var item in obj)
            {
                var name = item.Key;
                var child = new JsonItem(name, item.Value, level);
                list.Add(child);

                if (item.Value.Type == JTokenType.Array || item.Value.Type == JTokenType.Object)
                {
                    var children = child.GetChildren();
                    child.HasChildren = children.Count > 0;
                    list.AddRange(children);
                }
            }
        }
        #endregion
    }
}
