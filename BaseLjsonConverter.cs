using System;
using System.Collections.Generic;
using System.Text;

namespace Ljson
{
    public abstract class BaseLjsonConverter<T>
    {
        public char FirstChar = '╥';
        public char LastChar = '╛';

        public string Cover(string value)
        {
            return $"{FirstChar}{value}{LastChar}";
        }

        public string ToLjson(T obj)
        {
            var stringBuilder = new StringBuilder();

            var values = GetValues(obj);
            foreach (var value in values)
            {
                stringBuilder.Append(FirstChar);
                stringBuilder.Append(value);
                stringBuilder.Append(LastChar);
            }
            return stringBuilder.ToString();
        }

        protected string[] GetStringValues(string ljson)
        {
            var values = new List<string>();
            var partBuilder = new StringBuilder();
            int firstCharCount = 0;
            bool isInside = false;
            foreach (char c in ljson)
            {
                if (c == FirstChar)
                {
                    firstCharCount++;
                    if (isInside)
                    {
                        partBuilder.Append(c);
                        continue;
                    }
                    else
                    {
                        isInside = true;
                    }
                    continue;
                }
                if (c == LastChar)
                {
                    firstCharCount--;
                    if (firstCharCount == 0)
                    {
                        values.Add(partBuilder.ToString());
                        partBuilder.Clear();
                        isInside = false;
                        continue;
                    }
                    if (firstCharCount < 0)
                    {
                        throw new FormatException($"Use ' {LastChar} ' only for closing a message!");
                    }
                    if (isInside)
                    {
                        partBuilder.Append(c);
                        continue;
                    }
                }
                partBuilder.Append(c);

            }
            if (isInside)
            {
                throw new FormatException($"I am inside of ljson. So we don't have a ' {LastChar} ' to close the ' {FirstChar} ', do we?");
            }
            return values.ToArray();
        }

        public string[] GetValues(ILjsonable obj)
        {
            return obj.GetValues();
        }
        public void SetValues(ILjsonable obj, string[] values)
        {
            obj.SetValues(values);
        }
        public abstract T FromLjson(string ljson);
        public abstract string[] GetValues(T obj);
        public abstract void SetValues(T obj, string[] values);
    }
}
