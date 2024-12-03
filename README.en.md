
# LJSON

A simple library for converting an object to an Ljson string format and back.

___

# What is LJSON

___

The main principle of this format is: store only what is necessary and nothing more.

We usually store data as a string, and storing data as a string requires only two things:
 - The data itself
 - A data delimiter

In Ljson, the data delimiter can be any character the user deems appropriate.  
The data can be stored in any order defined by the user.


## Advantages of Ljson

---


 - ### Flexibility
The order in which data is stored and how it is delimited can be determined by the user independently.
 - ### Few restrictions
There are no restrictions on allowed characters or string format precision. It is harder to "break" during conversion.
 - ### Space efficiency
Stores only the minimum data.
 - ### Speed
Honestly, it hasn’t been tested for speed, but it should be very fast because the conversion strategies are very straightforward.

## Disadvantages of Ljson

---


 - ### A lot of manual work
The order of the data and how it will be assigned back during conversions is written manually by the user, which can be quite lengthy and inconvenient.
 - ### Lack of standardization
Different **ljson** strings can have different delimiters and methods of separation. The format is mainly designed to work "with itself" (own server-client or file storage) rather than to interact with other interfaces. To convert an unknown **ljson**, you need the same method and the same converter class.

# How to use

---


## Regular Class

---

Suppose we have a `Person` class like this:

```c#
class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public void Print()
        {
            Console.WriteLine($"\tName: {Name}\n" +
                              $"\tSurname: {Surname}\n" +
                              $"\tAge: {Age}\n");
        }
    }
```

If we want to convert it to an **ljson** string, we create a converter for it.  
Here, we inherit from `LjsonDefaultConvert`.  
We assign the conversion strategy (explained later) and specify the list of values to be stored and how they should be read.

```c#
class PersonConverter: LjsonDefaultConvert<Person>
    {
        public PersonConverter()
        {
            ConvertStrategy = new SimpleStrategy()
            {
                Char = '.'
            };
        }
        public override IList<object> ExtractValues(Person obj)
        {
            return [obj.Name, obj.Surname, obj.Age];
        }

        public override void AssignValues(Person obj, IList<string> values)
        {
            obj.Name = values[0];
            obj.Surname = values[1];
            obj.Age = int.Parse(values[2]);
        }
    }
```
Convert to a string and back. Note that the conversion does not create a new object but only assigns values.

```c#
var person = new Person()
{
    Name = "Mike",
    Surname = "Johnson",
    Age = 30
};
var converter = new PersonConverter();
var ljson = converter.ToLjson(person);
Console.WriteLine("LJson: " + ljson + "\n");
//LJson: Mike.Johnson.30

var person2 = new Person();
converter.FromLjson(person2, ljson);
person2.Print();

/*
    Name: Mike
    Surname: Johnson
    Age: 30
*/
```

---

## ILjsonable

---

It is possible to avoid defining a custom converter if the class itself knows the order in which to save or read the data. For this, we have the `ILjsonable` interface.

Let’s say we have a `Person2` class with private fields.  
We don’t have access to them, so we can’t use a regular converter.  
We use the `ILjsonable` interface.

```c#
public class Person2: ILjsonable
{
    private string name;
    private int age;

    public Person2() { }
    
    public void GenerateFields()
    {
        name = "Andrew";
        age = 50;
    }

    public void Print()
    {
        Console.WriteLine($"\tName: {name}\n" +
                          $"\tAge: {age}\n");
    }
    
    public void AssignValues(IList<string> values)
    {
        name = values[0];
        age = int.Parse(values[1]);
    }

    public IList<object> ExtractValues()
    {
        return [name, age];
    }
}
```
Now, just create this converter, set the strategy, and convert.

```c#
var person = new Person2();
person.GenerateFields();
var converter = new LjsonILjsonConvert<Person2>()
{
    ConvertStrategy = new SimpleStrategy()
    {
        Char = '/'
    }
};
var ljson = converter.ToLjson(person);
Console.WriteLine("LJson: " + ljson + "\n");
// LJson: Andrew/50

var person2 = new Person2();
converter.FromLjson(person2, ljson);
person2.Print();
/*
    Name: Andrew
    Age: 50
*/
```
---

## Converters

---

### LjsonDefaultConverter

This is an abstract class for inheritance.  
You only need to override the methods `IList<object> ExtractValues(T obj)` to retrieve values as a list and `void AssignValues(T obj, IList<string> values)` to assign fields with read values.

### LjsonILjsonConverter

This is a ready-to-use class. You don’t need to do anything with it—just create and use it if you want to work with the `ILjsonable` interface.

### ILjsonAssigner and BaseLjsonAssigner

The first is an abstract class interface `BaseLjsonAssigner`. You can use it to define your own basic converters.  
`ILjsonAssigner` is for conversion methods to and from ljson, and `BaseLjsonAssigner` is for assignment and retrieval of data to and from the value list.

___

## Conversion Strategies

---

These are classes that contain methods for converting a data list to a string and vice versa. Here is where the main conversion logic resides.  
To define your own strategies, use the `IConvertStringStrategy` interface.

`SimpleStrategy` uses a single character for separation. The implementation is very simple and suitable for many trivial cases.

```c#
public class SimpleStrategy: IConvertStringStrategy
{
    public char Char { get; set; }
    public IList<string> LjsonToList(string ljson) =>
        ljson.Split(Char);
    public string ListToLjson(IList<object> values) =>
        string.Join(Char.ToString(), values);
}
```

`DoubleCharStrategy` uses two characters for separation:
 - One to begin a data field
 - Another to end a data field

Using the same `Person2` and `LjsonIljsonConvert` as an example, let’s use `DoubleCharStrategy`.

```c#
var person = new Person2();
person.GenerateFields();
var converter = new LjsonILjsonConvert<Person2>()
{
    ConvertStrategy = new DoubleCharStrategy()
    {
        FirstChar = '[',
        LastChar = ']'
    }
};
var ljson = converter.ToLjson(person);
Console.WriteLine("LJson: " + ljson + "\n");
// LJson: [Andrew][50]

var person2 = new Person2();
converter.FromLjson(person2, ljson);
person2.Print();
/*
    Name: Andrew
    Age: 50
*/
```
This can be useful for nested data.

## License

Note that the use of the library code, the library itself, as well as the Readme texts, is covered by the NIN license (available in the project repository). Acceptance is mandatory.
