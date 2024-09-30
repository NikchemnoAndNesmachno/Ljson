using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ljson.ConvertStringsStrategy
{
    public class DoubleCharStrategy: IConvertStringStrategy
    {
        public char FirstChar { get; set; }
        public char LastChar { get; set; }
        public IList<string> LjsonToList(string ljson)
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
                    isInside = true;
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
                        throw new FormatException($"Use ' {LastChar} ' only for closing a message!");
                    
                    if (isInside)
                    {
                        partBuilder.Append(c);
                        continue;
                    }
                }
                partBuilder.Append(c);

            }
            if (isInside)
                throw new FormatException($"I am inside of ljson. So we don't have a ' {LastChar} ' to close the ' {FirstChar} ', do we?");
            return values;
        }

        public string ListToLjson(IList<object> values)
        {
            var stringBuilder = new StringBuilder();
            foreach (var value in values)
            {
                stringBuilder.Append(FirstChar);
                stringBuilder.Append(value);
                stringBuilder.Append(LastChar);
            }
            return stringBuilder.ToString();
        }
    }
}