# Chapter04. LINQ to Objects와 친해지기

## 4.1 앞으로 사용하게 될 예제 코드에 대한 설명
### 4.1.1 목표
- **예제를 활용하기 위해 기본적으로 요구되는 사항들:**
    - 다양한 LINQ 질의를 받아들이기 위해 객체 모델은 매우 잘 구성되어 있어야 함
    - 예제 코드는 메모리 내의 객체와 XML 문서, 관계형 데이터 등을 독립적으로나 조합하여 다룰 수 있어야 함
    - 예제 코드는 Windows Forms 애플리케이션뿐만 아니라 ASP.NET 웹 어플리케이션도 포함해야 함
    - 예제 코드는 로컬 환경의 데이터 저장소뿐만 아닌 공개 웹 서비스와 같은 외부의 원격지 데이터 저장소에 대해서도 질의할 수 있어야 함
    
### 4.1.2 구현할 기능
- LinqBooks의 주요 기능은 다음과 같은 것들을 포함해야 함
    - 어떤 책을 소장하고 있는지
    - 그런 책에 대해서 생각하는 바를 기록할 수 있어야 함
    - 책에 대해 더 많은 정보를 가져올 수 있어야 함
    - 책목록과 서평목록을 출력해줄 수 있어야 함
- 이 책에서 구현할 기술적인 기능에 포함되어야 하는 사항
    - 로컬 데이터베이스에 대해 질의하고, 삽입하고, 업데이트하는 기능
    - 로컬 저장소의 카탈로그 또는 서드파티의 데이터에도 검색기능을 제공하는 기능
    - 웹 사이트에서 책에 대한 정보를 불러 읽어들이는 기능
    - XML 문서에서 데이터를 불러오고 저장하는 기능
    - 추천하는 책으로 RSS 피드를 작성하는 기능
- 이런 기능들을 구현하기 위해 하나의 그룹의 비즈니스 개체들을 이용할 것임

### 4.1.3 비즈니스 개체
- 사용하게 될 객체 모델은 다음과 같은 클래스들로 이루어져 있음
Book, Author, Publisher, Subject, Review, User

### 4.1.4 데이터베이스 스키마
- 데이터베이스를 사용하여 애플리케이션들이 다루고 있는 정보를 저장하고 읽어올 것

### 4.1.5 예시 데이터
- LINQ to Objects를 시연하기 위해 메모리 내의 데이터를 이용할 것임
## 4.2 LINQ를 메모리 내의 컬렉션과 함께 사용하기
- LINQ to Objects는 메모리 내의 객체 컬렉션에 대해 동작하는 LINQ 변종
- 이 아이가 지원하는 컬렉션에는 어떤 것들이 있는가?
- 이런 컬렉션들에 대해 어떤 연산을 가할 수 있는가?

### 4.2.1 어떤 것들에 대해 질의할 수 있는가?
- LINQ to Objects가 모든 것을 대상으로 할 수 있는 것은 아님
- LINQ를 통한 질의가 가능하기 위한 첫 번째 전제조건: 대상 객체가 컬렉션이어야 함
- 컬렉션이 LINQ to Objects로 질의가 가능하기 위해서는 IEnumerable<T> 인터페이스를 구현하기만 함녀 됨
- **시퀀스**: 
    - LINQ의 세계에서 IEnumerable<T>를 구현하는 객체들
    
##### 배열

- 모든 종류의 배열을 지원하고 있음

- 형이 정해지지 않은 배열을 LINQ to Objects를 이용하여 질의하기

  ```C#
  using System;
  using System.Linq;
  
  static class TestArray
  {
      static void Main()
      {
          Object[] array = {"String", 12, true, 'a'};
          var types = 
              array
              	.Select(item => item.GetType().Name)
              	.OrderBy(type => type);
          ObjectDumper.Write(types);
      }
  }
  ```

  - 결과물

    ```C#
    Boolean
    Char
    Int32
    String
    ```

- 질의들은 사용자 정의 객체의 배열에 대해서도 수행 가능함

- Book 객체의 배열에 대한 질의를 수행

- 형을 가진 배열에 대해 LINQ to Objects 로 질의하기

  ```C#
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using LinqInAction.LinqBooks.Common;
  
  static class TestArray
  {
      static void Main()
      {
          Book[] books = 
          {
              new Book{Title = "LINQ in Action"},
              new Book{Title="LINQ for Fun"},
              new Book{Title = "Extreme LINQ"}
          };
          
          var titles = 
              books
              .Where(book => book.Title.Contains("Action"))
              .Select(book => book.Title);
          
          Object.Dumper.Write(titles);
      }
  }
  ```

- LINQ to Objects 질의는 어떤 데이터형의 질의에도 사용 가능함

##### 제너릭 리스트

- LINQ to Objects 는 List<T>를 비롯하여 다른 제네릭 리스트형에도 적용이 가능함

- 주요 제너릭 리스트형의 목록

  - System.Collections.Generic.List<T>
  - System.Collections.Generic.LinkedList<T>
  - System.Collections.Generic.Queue<T>
  - System.Collections.Generic.Stack<T>
  - System.Collections.Generic.HashSet<T>
  - System.Collections.ObjectModel.Collection<T>
  - System.ComponentModel.BindingList<T>

- 제너릭 리스트에 대해 LINQ to Objects로 질의하기 

  ```C#
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using LinqInAction.LinqBooks.Common;
  
  static class TestList
  {
      static void Main()
      {
          List<Book> books = new List<Book>(){
              new Book {Title = "LINQ in Action"},
              new Book {Title = "LINQ for Fun"},
              new Book {Title = "Extreme LINQ"}
          };
          
          var titles = 
              books
              .Where(book => book.Title.Contains("Action"))
              .Select(book => book.Title);
          
          ObjectDumper.Write(titles);
      }
  }
  ```

- 배열과 리스트 모두 IEnumerable<Book> 이라는 공통의 인터페이스를 구현하고 있음 
- 질의에는 아무 변화가 없다. 

##### 제너릭 딕셔너리
- 모든 제네릭 딕셔너리도 LINQ to Objects를 이용하여 질의 가능함
    - System.Collections.Generic.Dictionary<TKey, TValue>
    - System.Collections.Generic.SortedDictionary<TKey, TValue>
    - System.Collections.Generic.SortedList<TKey, TValue>
- 제네릭 딕셔너리는 IENumerable<KeyValuePair<TKey, TValue>>를 구현함
- KeyValue-Pair 구조는 형을 갖는 Key와 Value 프로퍼티를 갖고 있음

[제네릭 딕셔너리에 대해 LINQ to Objects를 이용하여 질의 수행하기]
```C#
static class GenericDictionary
    {
        static void Main()
        {
            Dictionary<int, string> frenchNumbers;
            frenchNumbers = new Dictionary<int, string>();
            frenchNumbers.Add(0, "zero");
            frenchNumbers.Add(1, "un");
            frenchNumbers.Add(2, "deux");
            frenchNumbers.Add(3, "trois");
            frenchNumbers.Add(4, "quatre");

            var evenFrenchNumbers =
                from entry in frenchNumbers
                where (entry.Key % 2) == 0
                select entry.Value;

            foreach (var num in evenFrenchNumbers)
            {
                Console.WriteLine(num.ToString());
            }

            Console.ReadKey();
        }
    }
```
[수행 결과]
```C#
zero
deux
quatre
```
##### 문자열
- INumerable<Char>를 구현한 컬렉션 객체
- 문자열 객체들도 LINQ to Objects를 이용하여 질의할 수 있음
```C#
static void Main()
{
    var count =
                "Non-letter characters in this string: 8"
                .Where(c => !Char.IsLetter(c))
                .Count();

    Console.Write(count);
    Console.ReadKey();
}
```
- 실행결과는 8
##### 기타 컬렉션 객체
- IEnumerable<T> 를 구현하는 다른 임의의 형의 객체를 대상으로도 LINQ to Objects를 사용 가능함<br>
-> LINQ to Objects가 자신이 직접 정의한 컬렉션 객체나 다른 프레임워크에서 가져온 객체들에 대해서도 동작할 것임

- 이와 관련하여 접할 수 있는 문제들: 
    - 모든 .NET 컬렉션들이 반드시 IEnumerable<T>를 구현하는 것은 아님
    - 실제로 엄격하게 형이 정의된 컬렉션만 이 인터페이스를 구현함
    - 배열, 제네릭 리스트 및 제네릭 딕셔너리는 엄격하게 형이 정의되어 있음
    - 정수의 배열이나 문자열의 리스트, Book 객체의 딕셔너리 등에 대해서도 자유롭게 LINQ to Objects를 사용 가능함

- 제네릭이 아닌 컬렉션들은 IEnumerable<T>를 구현하지 않지만 IEnumerable을 구현함
- **이게 LINQ를 DataSet이나 ArrayList와 같은 객체에 대해 사용할 수 없다는 의미?**
- Cast와 OfType 연산자의 도움을 받아 비제네릭 컬렉션에 대해 질의를 수행할 수 있는 방법이 존재함

### 4.2.2 지원되는 여러 가지 연산
- 앞에서 열거했던 형들에 대해 행할 수 있는 연산들은 표준 질의 연산자들이 지원하는 것들
- LINQ는 시퀀스를 조정하고 질의를 작성할 떄 유용하게 사용 가능한 몇 가지 연산자를 포함함

- 표준 연산자가 지원하는 연산:
    - 제한(restriction)
    - 사영(projection)
    - 분할(partitioning)
    - 조인(join)
    - 순차정렬(ordering)
    - 그룹화(grouping)
    - 집합연산(set)
    - 변환(conversion)
    - 등치성(equality)
    - 개체선택(element)
    - 생성(generation)
    - 계량화(quantifiers)
    - 누적연산(aggregation)

- 질의 연산자들이 System.Linq.Enumerable을 통해 IEnumerable<T>를 구현하는 형태의 확장 메소드라는 것에 주목!

- 이런 연산자들은 표준 질의 연산자라고 불림
- 스스로 직접 정의한 질의 연산자들을 제공 가능함
- 이는 표준 질의 연산자들에 의해 지원하지는 않음
- LINQ를 설계한 사람들이 범용 연산들을 처리가능하도록 함 

## 4.4 주요 표준 질의 연산자에 대하여 알아보기
- 표준 질의 연산자: 질의를 구성하는 기본 구성단위

  [종류별로 정리된 표준 질의 연산자]

  | 종류               | 질의 연산자                                                  |
  | ------------------ | :----------------------------------------------------------- |
  | 선별(필터링, 추출) | OfType, **Where**                                            |
  | 사영               | **Select, SelectMany**                                       |
  | 분할               | **Skip**, SkipWhile, **Take**, TakeWhile                     |
  | 조인               | **GroupJoin, Join**                                          |
  | 병합               | Concat                                                       |
  | 순차정렬           | **OrderBy, OrderByDescending**, Reverse, **ThenBy**, **ThenByDescending** |
  | 그룹화             | **GroupBy**, ToLookup                                        |
  | 집합연산           | **Distinct**, Except, Intersect, Union                       |
  | 변환               | AsEnumerable, AsQueryable, Cast, **ToArray**, **ToDictionary, ToList** |
  | 등치성             | SequenceEqual                                                |
  | 개체 선택          | ElementAt, ElementAtOrDefault, First, FirstOrDefault, Last, LastOrDefault, SIngle, SingleOrDefault |
  | 생성               | **DefaultIfEmpty,** Empty, Rangle, Repeat                    |
  | 계량화             | All, Any, Contains                                           |
  | 누적연산           | Aggregate, Average, **Count**, LongCount, **Max, Min, Sum**  |

### 4.4.1 제한 연산자 "Where"
- 체와 같이 Where 연산자는 특정한 분류기준을 통해 값의 시퀀스를 필터링해냄
- Where는 원본 시퀀스를 열거하여 미리 지정한 조건을 만족시키는 값들만 남기게 됨

[Where 사용 예시]
```C#
public static IEnumerable<T> Where<T>(
    this IEnumerable<T> source,
    Func<T,bool> predicate);
```
- 조건함수의 첫 번 째 매개변수 = 심사할 개체
- 이 함수는 조건의 만족 여부를 나타내는 Boolean 값을 리턴함

[가격이 155이상인 책들의 목록 생성하는 예시]
```C#
IEnumerable<Book> books = 
    SampleData.Books.Where(book => book.Price >= 15);
```
- 질의 표현식 내에서, where 절은 Where 연산자에 대한 호출로 변환됨
- 아래 예시는 이전 예시와 같은 결과물을 냄
```C#
var books =
    from book in SampleData.Books
    where book.Price >= 15
    select book;
```
- 다음은 원본 시퀀스에 대해 개체의 인덱스로 동작 가능학 하는 Where 연산자의 재정의 형태
```C#
public static IEnumerable<T> Where<T>(
    this IEnumerable<T> source,
    Func<T, int, bool> predicate);
```
- 조건 함수의 두 번째 매개변수가 만약 존재한다면, 이것은 원본 시퀀스에서 개체의 순서를 나타내는 인덱스 번호임

[책의 컬렉션에서 가격이 15이상이고, 홀수 번째인 경우만 선별해내는 예시]
```C#
IEnumerable<Book> books =
    SampleData.Books.Where(
        (book, index) => (book.Price >= 15) && ((index&1) == 1));
```
- Where는 제한을 가하는 연산자로, 간단하기 때문에 시퀀스 내의 개체들을 필터링하기 위해 자주 사용할 것임

### 4.4.2 사영 연산자 이용하기
#### Select
- Select 연산자는 연산자에 넘겨진 매개변수를 바탕으로 시퀀스에 대하여 사영을 수행하기 위해 이용됨
- Select는 다음과 같이 선언됨
```C#
public static IEnumerable<S> Select<T, S>(
    this IEnumerable<T> source,
    Func<T, S> selector);
```
- Select 연산자는 각각의 대상 객체에 대해 selection 함수를 적용하고 난 후의 결과를 담은 열거형을 할당하고 반환함
[모든 책의 제목을 담은 시퀀스]
```c#
IEnumerbale<String> titles = 
    SampleData.Books.Select(book => book.Title);
```
- 질의 표현식 내에서 Select절은 Select에 대한 호출로 변환됨
- 다음의 질의 표현식은 전의 예로 변환이 됨
```C#
var titles = 
    from book in SampleData.Books
    select book.Title;
```
- 이 질의는 전에 반환되어썯ㄴ "책"이라는 객체의 시퀀스를 문자열값의 시퀀스로 범위를 축소해줌
- 객체를 반환받을 수도 있고 개체를 선택 가능함
- 다음은 책과 연관된 Publisher 객체를 어떻게 반환받는지 보여줌
```C#
var publishers = 
    from book in SampleData.Books
    select book.Publisher;
```
- Select를 사용했을 때 반환되는 컬렉션은 새로운 객체의 항목들의 조합이거나 원본 객체를 그대로 보존한 형태일 수도 있음
- 다음은 익명형이 이용되어 필요한 항목들을 즉석에서 객체화하여 결과를 반환해주고 있음
```C#
var books =
    from book in SampleData.Books
    select new {book.Title, book.Publisher.Name, book.Authors};
```
- 이러한 종류의 코드는 데이터를 사영해줌
#### SelectMany
- 사영 계열의 두 번쨰 연산자
- 이 연산자의 선언은 Select의 선언과 매우 유사함
- 그러나 이 연산자의 내부 함수는 시퀀스를 반환한다는 차이가 있음
```C#
public static IEnumerable<S> SelectMany<T, S>(
    this IEnumerable<T> Source,
    Func<T, IEnumerable<S>> selector);
```
- SelectMany 연산자는 selector 함수에서 반환된 각각의 시퀀스 내의 개체를 새로운 시퀀스로 매핑해준 다음 결과를 이어 붙여줌
- Select 의 동작과 비교해보자
```C#
IEnumerable<IEnumerable<Autor>> tmp = 
    SampleData.Books
    .Select(book => book.Authors);
foreach(var authors in tmp)
{
    foreach(Author author in authors)
    {
        Console.WriteLine(author.LastName);
    }
}
```
- 같은 일을 하는 SelectMany를 통해 구현된 코드
```C#
IEnumerable<Author> authors = 
    SampleData.Books
    .SelectMany(book => book.Authors);
foreach(Author author in authors)
{
    Console.WRiteLine(ahotr.LastName);
}
```
- 훨씬 짧다
- 책의 저자들을 여기서 열거하려고 함
- Book 객체의 Authors 프로퍼티는 Author 객체들의 배열
- 그래서 Select 연산자는 이드 ㄹ배열이 열거된 형태를 그대로 반환함
- 그러나...! SelectMany는 이 배열의 개체들을 펼쳐놓고 Author 객체의 시퀀스로 재조합해줌
[SelectMany가 호출되는 부분에서 사용 가능한 질의식]
```C#
from book in SampleData.Books
from author in book.Authors
select author.LastName
```
- from 절이 연결될 떄마다 SelectMany 사영이 유용하게 이용되고 있음
- Select와 SelectMany 연산자는 인덱스들과 함께 동작하는 오버로딩된 형태도 제공함

#### 인덱스를 선택하기
- Select와 SelectMany 연산자는 시퀀스 내의 각각의 개체들의 인덱스를 가져오는 데 사용될 수 있음
- 컬렉션 내의 책을 알파벳순으로 정렬하기 전에 인덱스를 보여준다고 가정해보자
```C#
index=3     Title=All your base are belong to us
index=4     Title=Bonjour monAmour
index=2     Title=C# on Rails
index=0     Title=Funny Stories
index=1     Title=LINQ rules
```
- 다음은 Select문으로 문제를 해결하는 것을 보여줌
[인덱스와 함께 Select 질의 연산자를 사용한 예제]
```C#
var books=
  SampleData.Books
  .Select((book, index) => new {index, book.Title})
  .OrderBy(book => book.Title);
```
- 질의 표현식 구문을 이용할 수 없음 
- 왜? 인덱스를 제공하는 Select 연산자의 변종이 이와 동등한 구문을 제공하지 않았음
- 이 버전의 Select 메소드가 람다 표현식에서 사용 가능한 인덱스 변수를 제공한다는 사실에 주목하자
- 컴파일러는 자동적으로 인덱스 매개변수의 유무를 통해 어떤 버전의 Select 연산자가 이요오디고 있는지 정함
- OrderBy를 수행하기 전에 Select를 수행한다는 것을 확인 가능함
- 책이 정렬되기 전에 인덱스를 가져오는 것은 매우 중요함
### 4.4.3 Distinct 이용하기
- 질의 결과에는 중복이 발생하곤 함
[책을 쓴 저자의 목록을 반환하는 예시]
```C#
var authors = 
    SampleData.Books
        .SelectMany(book => book.Authors)
        .Select(author => author.FirstName+" "+author.LastName);
```
- 그러나... 동일한 작가의 이름이 한 번 이상 등장하는 경우가 있음
- 같은 작가가 책을 여러 권 집필했을 수도 있어서 발생하는 문제
- 이러한 문제를 피하기 위해 Distinct 연산자를 사용 가능함
- Distinct 연산자: 어떤 시퀀스 내의 중복된 개체들을 삭제해줌
- 개체들을 비교하기 위해, Distinct 연산자는 개체가 만약 IEquaTable<T>를 구현하고 있다면, IEquaTable<T>.Equals() 메소드를 호출함
- 그렇지 않으면 별도로 구현해놓은 Object.Equals 메소드를 사용함

[Distinct 질의 연산자를 사용하여 저자목록 받아오기]
```C#
var authors = 
    SampleData.Books
        .SelectMany(book => book.Authors)
        .Distinct()
        .Select(author => author.FirstName+" "+author.LastName); 
```
- Distinct 에 해당하는 동등한 키워드는 C#질의 표현식 구문에 존재하지 않음
- C#에서 Distinct는 메소드 호출로만 이용될 수 있음
- 다음에 알아보는 연산자들은 C#이나 VB.NET 모두 대응될 만한 키워드를 갖고 있지 않음
- 이런 연산자들은 시퀀스를 표준적인 컬렉션 객체로 바꿀 수 있도록 해줌
### 4.4.4 변환 연산자 이용하기
- LINQ는 시퀀스를 손쉽게 다른 컬렉션 형태로 변환 가능한 연산자들을 기본적으로 제공함
- 예를 들어, ToArray와 ToList 연산자는 시퀀스를 배열이나 리스트의 형태로 변환시켜줌
- 이런 연산자들은 기존의 코드 라이브러리들과 LINQ를 통합하는 과정에서 매우 유용하게 사용 가능함
- 이런 변환 연산자들은 배열이나 객체 목록을 받기를 기대하고 있는 메소드들을 다시 활용 가능하게 해줌

- 기본적으로 질의들은 IEnumerable<T>를 구현하는 컬렉션인 시퀀스를 반환함
```C#
IEnumerable<String> titles = 
    SampleData.Books.Select(book => book.Title);
```
- 다음은 어떻게 그런 결과가 배열이나 리스트로 변환될 수 있는지 보여줌
```C#
String[] array = titles.ToArray();
List<String> list = titles.ToList();
```
- ToArray나 ToList는 질의의 결과를 임시로 저장해두거나 명령과 동시에 질의가 수행되기를 요청할 떄 유용하게 사용 가능함
- 호출되었을 때, 이 연산자들은 전체 시퀀스를 나열한 후 이 시퀀스에 의해 반환된 개체들의 복제물을 생성함

- 질의는 여러 번 수행 시마다 다른 결과를 반환 가능함
- 어떤 시퀀스의 특정 시점에서의 스냅샷을 원한다면 ToArray나 ToList를 이용하면 됨
- 이런 연산자들은 모든 결과 개체들을 호출될 때마다 새로운 배열이나 리스트로 복사할 것임
- -> 대규모의 시퀀스에 대해 지나친 사용은 자제해야 함

- 실수하기 쉬운 용례: using 블록 내에서 생성된 일회용 객체에 대해 질의를 할 떄, 블록을 벗어나면 객체는 일찍 사라지게 됨
    - 해결하는 방법: ToList로 결과를 복사한 후 블록을 떠나는 것
```C#
IEnumerable<Book> results;

using(var db = new LinqBooksDataContext())
{
    results = db.Books.Where(...).ToList();
}

foreach(var book in resutls)
{
    DoSomething(book);
    yield return book;
}
```
- 재미있는 변환 연산자: ToDictionary
    - 배열이나 리스트를 생성하는 대신 이 연산자는 딕셔너리를 하나 작성하여 키를 이용하여 데이터를 처리함
    ```C#
        Dictionary<String, Book> isbnRef =
            SampleData.Books.ToDictionary(book => book.Isbn);
    ```
    - 여기서는 각각의 책의 ISBN으로 인덱싱된 책의 딕셔너리를 생성
    - 이런 종류의 변수는 ISBN을 기반으로 책을 검색하는 작업을 쉽게 해줌
    `Book linqRules = isbnRef["0-111-77777-2"];`
### 4.4.5 누적 연산자 이용하기
- 누적 연산자는 데이터에 몇 가지 수학 함수를 적용하기 위한 표준 질의 연산자들
    - Count 연산자는 시퀀스 내 개체의 개수를 계산함
    - Sum은 시퀀스 내 수치값들의 합을 나타냄
    - Min과 Max는 시퀀스 내의 수치값 중 최저값과 최고값을 찾아줌

- 이렇게 활용이 가능함
```C#
var minPrice = SampleData.Books.Min(book => book.Price);
// 표현식을 만족하는 시퀀스에게만 적용
var maxPrice = SampleData.Books.Select(book => book.Price).Max();
// 모든 객체에 대해 적용됨
var totalPrice = SampleData.Books.Sum(book => book.Price);
var nbCheapBooks = 
    SampleData.Books.Where(book => book.Price < 30).Count(); 
```
- 모든 누적 연산자들은 selector를 매개변수로 받을 수 있음. 
- 어떤 오버로딩된 연산자를 사용할지는 사전제한된 시퀀스를 사용하는지 여부에 달림

## 4.5 메모리 내의 객체 그래프에 대한 뷰 작성하기
### 4.5.1 정렬
- 데이터를 내가 원하는 순서로 정렬해서 보고싶을 때 사용
- 질의 표현식은 이런 경우에 orderby절을 이용할 수 있게 해줌
```C#
from book in SampleData.Books
 orderby book.Publisher.Name, book.Price descending, book.Title
 select new { Publisher=book.Publisher.Name,
              book.Price,
              book.Title};
```
- orderby 키워드는 복수의 정렬 기준을 정해주기 위해 사용 가능함
- 기본적으로 정렬은 오름차순 그러나 내림차순을 원하면 각 기준별로 하나하나 descending 키워드를 추가하여 설정 가능함

- 질의 표현식의 orderby절은 내부적으로 OrderBy, ThenBy, OrderByDescending, ThenByDescending 연산자에 대한 호출로 전환되어 수행됨
[질의 연산자로 표현된 예제 코드]
```C#
SampleData.Books
    .OrderBy(book => book.Publisher.Name)
    .ThenByDescending(book => book.Price)
    .ThenBy(book => book.Title)
    .Select(book => new { Publisher = book.Publisher.Name, 
                          book.Price,
                          book.Title });
```
### 4.5.2 중첩 질의
- 중첩 질의를 활용하여 출판사의 이름이 중복되는 경우 없게 할 것임
```C#
from publisher in SampleData.Publishers
select publisher
```
- 출판사의 이름과 책을 만약 모두 원한다면?
- Publisher 객체 전체를 반환하는 것이 아니라, Publisher와 Books 두 객체의 정보를 조합하여 가지게 되는 익명형을 반환하여 이 문제를 해결해볼 것임
```C#
from publhser in SampleData.Publishers
select new {Publisher = publisher.Name, Books = ...}
```
- 흥미로운 부분: "어떻게 특정 출판사의 책을 골라낼 수 있을까?"
- 예제 데이터 속에서 책은 Publisher 프로퍼티를 통해 출판사 정보와 연계되어 있음
- Publisher 객체에서는 이와 유사하게 Book 객체를 찾을 수 있는 고리가 없다
- 다행히, LINQ는 이런 문제를 해결해줌
- 첫번째 질의 속에 포함된 간단한 질의를 통해 이런 문제 해결 가능!
```C#
from publisher in SampleData.Publishers
select new {
    Publisher = publisher.Name, 
    Books = 
        from  book in SampleData.Books
        where book.Publisher.Name == publisher.Name
        select book
}
```
### 4.5.3 그룹화하기
- 그룹화 기능을 통해서 동일한 결과를 낼 수 있음
```C#
GridView1.DataSource = 
    from book in SamplData.Books
    group book by book.Publisher into publisherBooks
    select new {  Publisher=publisherBooks.Key.Name, 
                  Books=publisherBooks };
```
- 출판사별로 그룹화된 책의 목록을 받아보고 싶어함
- 특정한 출판사에 소속된 책은 publisherBooks라는 하나의 그룹 안에 모아질 것임
- publisherBooks 그룹은 IGrouping<Key, T> 인터페이스의 인스턴스임
- 다음과 같이 정의됨
```C#
public interface IGrouping<TKey, T>:IEnumerable<T>
{
    TKey Key {get; }
}
```
- IGrouping  제네릭 인터페이스를 구현하는 객체는 엄격하게 형을 가지는 키를 갖고 있고, 엄격하게 형을 가진 열거형임
- 이 경우에 키는 Publisher 객체, 열거형은 IEnumerable<Book>형임

- 질의는 출판사의 이름(그룹의 키값)과 책의 사영을 반환함
- 중첩 질의를 이용한 이전의 예제에서 일어나는 일과 동일함

- 중첩 질의 대신 이전 예제처럼 그룹화 연산자를 이용하는 것은 두 가지의 장점을 가짐
    - 질의가 더욱 짧아지고 그룹에 이름 지정이 가능
- 그룹이 무엇으로 구성되었는지 쉽게 알 수 있게 해줌
```C#
from book in SampleData.Books
group book by book.Publisher into publisherBooks
select new {
    Publisher = publisherBooks.Key.Name, 
    Books = publisherBooks,
    Count = publisherBooks.Count()};
```
- 그룹화는 SQL에서 누적 연산자와 함께 흔히 사용됨
- Count, Sum, Min,Max 같은 누적 연산자들을 종종 사용하게 될 것
### 4.5.4 조인 사용하기
- 중첩 질의와 그룹화 연산자를 이용하여 데이터를 그룹화하는 것과 유사한 결과를 얻는 방법
- join 연산자는 사영, 중첩 질의, 그룹화 등과 동일한 종류의 연산을 수행 가능하도록 함
- join 연산자는 SQL과 유사한 문법구조를 가짐

#### 그룹 조인
[join...into 절을 이용하여 책을 출판사별로 그룹화하기]
```C#
from publisher in SampleData.Publishers
join book in SampleData.Books
  on publisher equals book.Publisher into publisherBooks
select new { Publisher = publisher.Name, Books = publisherBooks };
```
- 각 출판사의 책을 publisherBooks라고 명명된 시퀀스로 묶음
- 이 새로운 질의는 group절을 이용하여 작성했던 질의와 동일한 기능을 수행함

#### 내부 조인
- 두 시퀀스의 교집합을 찾아내는 것
- 내부 조인을 사용하면 두 시퀀스의 개체 중 특정한 매치 조건을 충족하는 경우 하나의 시퀀스를 형성하게 됨

- join 연산자는 개체에서 추출된 매치키를 바탕으로 두 시퀀스의 내부 조인을 수행함
- 시퀀스는 각각의 책에 대한 개체를 가지고 있음
- 예제 데이터에서 하나의 출판사는 어떤 책과도 연관 없음
- 이런 출팥사는 관심대상에서 제외됨
- 이런 join 연산 = 내부 조인
- 시퀀스에서 가져온 개체 중 최소한 하나의 맞는 개체가 다른 시퀀스내에 있으면 유지됨
- 어떻게 왼쪽 외부 조인과 상관 있는지 알아볼 것
[join 절을 이용하여 책을 출판사별로 그룹화하기]
```C#
from publisher in SampleData.Publishers
join book in SampleData.Books on publisher equals book.Publisher
select new { Publisher=publisher.Name, Book = book.Title };
```
- 이 질의를 join 질의 연산자를 이요앟여 어떻게 작성할 수 있는가?
[Join 연산자를 사용하여 출판사별로 책 그룹화하기]
```C#
SampleData.Publishers
.Join(SampleData.Books,  //니부 시퀀스
publishers => publisher, //외부키 선택자
book => book.Publisher,  //내부키 선택자
(publisher, book) => new {Publisher=publisher.Name,  //결과 선택지
                          Book=book.Title});
```
- SQL과 유사한 문법을 바탕으로 질의 표현식은 몇몇 질의 연산자의 복잡함을 피할 수 있도록 해줌
#### 왼쪽 외부 조인(left outer join)
- 내부 조인을 사용하면 두 조인된 시퀀스에 모두 포함된 개체들만 살아남음
- 만약 외부 시퀀스의 모든 개체들을 내부 시퀀스에 맞는 개체가 있는지 유무에 관계없티 유지하고 싶으면 왼쪽 외부 조인을 이용해야 함

- 내부 조인과 유사하지만 모든 좌변의 개체들이 우변에 맞는 값이 없더라도 최소한 한 번 이상 포함된다
- 책이 없는 출판하들을 결과에 포함시키고 싶을 떄도 사용됨
[왼쪽 외부 조인을 하기 위해 사용된 질의]
```C#
from publisher in SampleData.Publishers
join book in SampleData.Books
  on publisher equals book.Publisher into publisherBooks
from book in publisherBooks.DefaultIfEmpty()
select new {
    Publisher = publisher.Name, 
    Book = book == default(Book) ?"(no books)": book.Title
};
```
- DefaultIfEmpty 연산자는 빈 시퀀스를 위해 기본 개체를 제공함
- DefaultIfEmpty는 제네릭 형태의 default 키워드를 사용함
- 이는 참조형을 위해 null을 반환하고 수치값을 위해 0을 반환함
- 구조체에 대해서는 각각의 멤버가 값인지 참조인지 여부에 따라 0과 null로 초기화된 인스턴스를 반환함

- 예제의 경우에 기본값은 null임 그러나 여기서는 book이라는 특성에 맞춰서 잘 했음

#### 카테시안 조인(cross join)
- 두 시퀀스의 모든 개체들의 카테시안 곱을 계산함
- 결과: 첫 번째 시퀀스의 각각의 개체와 두 번째 시퀀스들의 개체들의 곱으 ㅣ시퀀스가 됨
- -> 결과 시퀀스의 개체수 = 각각의 시퀀스의 개체수 

- **LINQ에서 카테시안 조인은 Join 연산자에 의해 이루어지지 않음**
- LINQ에서는 카테시안 조인을 사영이라고 함
- SelectMany 연산자를 이용하거나 from 절을 질의 표현식에서 수정하는 방법으로 가능함
[카테시안 조인을 수행하는 질의]
```C#
from publisher in SampleData.Publishers
from book in SampleData.Books
select new {
    Correct = (publisher == book.Publisher),
    Publisher = publisher.Name,
    Book = book.Title
};
```
- 동일한 작업을 질의 표현식을 사용하지 않고 selectMany와 Select 연산자를 이용해서 알 수 있는지 보여줌
```C#
SampleData.Publishers.SelectMany(
    publisher => SampleData.Books.Select(
        book => new {
            Correct = (publisher == book.Publisher),
            Publisher = publisher.Name,
            Book = book.Title}));
```
### 4.5.5 분할
#### Skip과 Take
- 시퀀스에서 반환된 데이터의 일부 범위를 가져오고 싶다면, Skip과 Take라는 두 가지의 분할 질의 연산자를 사용 가능함
- Skip 연산자는 주어진 개수만큼 시퀀스에서 개체를 건너뛴 다음 나머지 부분을 반환함
- Take 연산자는 지정된 위치에서 주어진 개수만큼의 개체를 반환하고 나머지를 버림
- 따라서 한 페이지의 크기인 pageSize가 주어졌을 때 페이지 번호인 n을 반환하는 일반적인 표현식은 이렇다
```C#
Sequence.Skip(n*pageSize).Take(pageSize).
```
