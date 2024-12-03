# LJSON

Проста бібліотека для конвертування об'єкта у формат рядка Ljson і назад.

___

# Що таке LJSON

___

Основним принципом цього формату є: зберігаємо лише те, що потрібно, і нічого більше.

Зберігаємо ми дані зазвичай як рядок, а для збереження даних як рядок потрібні всього дві речі:
 - Самі дані
 - Роздільник даних

В Ljson роздільником даних може бути будь-який символ, який користувач вважає доречним.
Дані ж можуть зберігатися у визначеному користувачу порядку.


## Переваги Ljson

---


 - ### Гнучкість
Те, в якому порядку зберігаються дані, і як саме вони розділяються, може визначати користувач самостійно
 - ### Відсутність багатьох обмежень
Немає обмежень на дозволені символи, на чіткість формату рядка. Його важче "зламати" при конвертації
 - ### Економія місця
Зберігає, по суті, лише мінімум даних
 - ### Швидкість
Якщо чесно, не тестувалося на швидкість, але має бути дуже швидким, адже стратегії конвертації дуже тривіальні

## Недоліки Ljson

---


 - ### Багато ручної роботи
Порядок даних і те, як вони будуть присвоюватися назад при конвертаціях, пишеться власноруч користувачем, що може бути доволі довго і незручно.
 - ### Відсутність стандарту
Різні рядки **ljson** можуть мати різні розділювачі та методи розділення. Формат розрахований переважно на роботу "з собою"
(власні сервер-клієнт, збереження у файл), а не для взаємодії з іншими інтерфейсами. Для конвертації невідомого **ljson**
потрібно мати той же метод, той же клас конвертера.

# Як використовувати

---


## Звичайний клас

---

Нехай у нас є такий клас Person

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

Якщо ми хочемо привести його до рядка **ljson**, створимо для цього конвертер.
Тут ми наслідуємося від `LjsonDefaultConvert`.
Ми присвоюємо стратегію конвертації (про це пізніше) і задаємо список значень,
що має зберігатися, і те, як вони мають зчитуватися.

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
Конвертуємо у рядок і назад. Варто зазначити, що конвертація не створює новий об'єкт, а лише присвоює значення.

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

Можна обійтися без визначення власного конвертера, якщо клас сам знає,
у якому порядку потрібно зберегти або зчитати дані. Для цього є інтерфейс `ILjonable`

Нехай у нас є клас `Person2`, у якого є приватні поля. 
До них ми не маємо доступу, тому не зможемо використати звичайний конвертер.
Використаємо інтерфейс `ILjsonable`.

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
Тепер просто створимо цей конвертер, вкажемо стратегію і конвертуємо.

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

## Конвертери

---

### LjsonDefaultConverter

Це абстрактний клас для наслідування. Все, що потрібно перевизначити,
це методи `IList<object> ExtractValues(T obj)` для отримання значень у вигляді списку та
`void AssignValues(T obj, IList<string> values)` для призначення полям зчитаних значень.

### LjsonILjsonConverter

Це вже готовий клас, нічого з ним робити не потрібно, просто створіть і використовуйте його,
якщо хочете працювати з інтерфейсом `ILjsonable`

### ILjsonAssigner та BaseLjsonAssigner
Перший це інтерфейс абстрактного класу `BaseLjsonAssigner`. Можете використати для визначення власних базових конвертерів.
`ILjsonAssigner` - для методів конвертації з і до ljson,
`BaseLjsonAssigner` - для методів присвоєння й отримання даних з і до списку значень

___

## Стратегії конвертації

---

Це класи, що містять методи конвертації списку даних у рядок і навпаки. Тут зосереджена основна логіка конвертації.
Для визначення власних стратегій використовуйте інтерфейс `IConvertStringStrategy`.

`SimpleStrategy` використовує один символ для розділення. Реалізація дуже проста і підходить для багатьох тривіальних випадків
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
`DoubleCharStrategy` використовує два символи для розділення:
 - один для початку поля даних
 - інший в кінці поля даних

На прикладі того ж `Person2` та `LjsonIljsonConvert` використаємо `DoubleCharStrategy`
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
Це може бути корисно для вкладених даних.

## Ліцензія
Увага, на використання коду бібліотеки, самої бібліотеки, а також текстів Readme
розповсюджується ліцензія NIN (є в репозиторії проєкту). Прийняття є обов'язковим.

