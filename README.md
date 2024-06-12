# A simple library for encoding objects into an <b>ljson</b> string.

It is like json, but L (!) json. Try it.

The string contains only the object fields in the order specified by the user.

To separate the fields, a separate symbol is used at the beginning of the word and a separate one at the end (they must be different).

These characters are rare for human writing.

# How to use

## Simple object

Let us have this class: 

```c#
internal class TestObject
{
    public string StrProp { get; set; } = "String Value";
    public int IntProp { get; set; } = 1123;
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public override string ToString()
    {
        return $"StrProp: {StrProp}\nIntProp: {IntProp}\nDate: {Date}";
    }
}
```

So we must have a corresponding converter. For example:

```c#
internal class TestObjectConverter : LjsonConvert<TestObject>
{
    public override string[] GetValues(TestObject obj)
    {
        return [
            obj.StrProp, obj.IntProp.ToString(),
            $"{obj.Date.Day}&{obj.Date.Month}"
            ];
    }

    public override void SetValues(TestObject obj, string[] values)
    {
        obj.StrProp = values[0];
        obj.IntProp = int.Parse(values[1]);
        var dateValues = values[2].Split("&");
        obj.Date = new DateOnly(DateTime.Now.Year, int.Parse(dateValues[1]), int.Parse(dateValues[0]));
    }
}
```
We must define the GetValues method that returns a string array with the data representing object's fields. The string array will be converted to ljson.

We must define the SetValues method to set our properties from string array got from ljson.


To launch it:

```c#
internal class Program
{

    static void Main(string[] args)
    {
        var testObject = new TestObject();
        var converter = new TestObjectConverter();
        var ljson = converter.ToLjson(testObject);
        Console.WriteLine(ljson + "\n"); // outputs: ╥String Value╛╥1123╛╥12&6╛

        var testObject2 = converter.FromLjson(ljson);
        Console.WriteLine(testObject2);
        /*
         StrProp: String Value
         IntProp: 1123
         Date: 12.06.2024
         */
    }
}
```

## Private fields

If you have the class with private fields that must be included in ljson, you can use the Iljsonable interface.


```c#
internal class PrivateTestObject : ILjsonable
{
    private int IntField = 100;
    private string StringField = "str field";
    private string[] strings = ["str1", "str2", "str3", "str4"];

    public string[] GetValues()
    {
        var arr = new string[1 + 1 + strings.Length];
        arr[0] = IntField.ToString();
        arr[1] = StringField;
        for(int i = 0; i < strings.Length; i++)
        {
            arr[2 + i] = strings[i];
        }
        return arr;
    }

    public void SetValues(string[] values)
    {
        IntField = int.Parse(values[0]);
        StringField = values[1];
        strings = values[2..];
    }

    public override string ToString()
    {
        return $"String field: {StringField}\nInt: {IntField}\nStrings: {string.Join("[]", strings)}";
    }
}
```
And your converter can use its methods (in future versions its behaviour will be added to the package):

```c#
internal class PrivateTestObjectConverter : LjsonConvert<PrivateTestObject>
{
    public override string[] GetValues(PrivateTestObject obj)
    {
        return obj.GetValues();
    }

    public override void SetValues(PrivateTestObject obj, string[] values)
    {
        obj.SetValues(values);
    }
}
```

And use it:

```c#
namespace Ljson.Examples;

internal class Program
{

    static void Main(string[] args)
    {
        var privateObject = new PrivateTestObject();
        var privateConverter = new PrivateTestObjectConverter();
        var privateLjson = privateConverter.ToLjson(privateObject);
        Console.WriteLine(privateLjson + "\n"); // ╥100╛╥str field╛╥str1╛╥str2╛╥str3╛╥str4╛


        var privateObject2 = privateConverter.FromLjson(privateLjson);
        Console.WriteLine(privateObject2);

        /*
        String field: str field
        Int: 100
        Strings: str1[]str2[]str3[]str4
         */
    }
}

```
