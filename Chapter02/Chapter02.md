# Chapter02. 개선된 C#과 VB.NET의 언어상 기능🥨
- 프로그래밍 언어에 잘 내장된 상태로 질의할 수 이씨게 해주는 언어 확장에 대해서 설명할 것
- LINQ는 C#과 VB.NET을 새로운 구조로 확장함
- LINQ가 가능하게 하기 위해 C#과 vB.NET이 어떻게 확장되고 개선되었는지를 살펴볼 것

## 2.1 새로운 언어적 측면에서의 개선점
- .NET 2.9은 몇몇 중요한 언어 및 프레임워크 측면의 개선사항을 반영함
- LINQ가 목표로 하고 있는 심도 있는 수준에서의 데이터 통합을 이루려면 매개변수화될 수 있는 형이 필요함

- C# 2.0에서는 익명 메소드와 반복자(iterator)가 추가됨
-  -> 한 차원 높은 수준의 데이터와 프로그래밍 언어의 통합을 가져오는 주춧돌이 됨
- LINQ가 C#이나 VB.NET과 같은 언어에 깔끔하게 내장된 형태의 질의문법을 제공하기 위해서는 더 많은 사항들이 요구됨
- C#과 VB.NET 9.0은 LINQ를 지원하기 위해 제네릭, 익명 메소드, 반복자 등을 필요로 했음

- 필요한 기능들
    - 암시적으로 형이 선언된(implicitly typed) 로컬 변수: 
        - 로컬 변수들의 형이 이를 초기화하는 표현식에 의해 정해질 수 있게 함
    - 객체 초기화 함수(object initializer):
        - 객체의 생성과 초기화를 쉽게 해줌
    - 람다 표현식(lambda expression):
        - 익명 메소드의 진화형태로 향상된 형(type) 추론기능과 대리자형이나 표현식 트리로의 변환이 가능함
    - 확장 메소드(extension method):
        - 현존하는 형들이나 새로 정의된 형들을 추가적인 메소드를 통해 확장 가능하게 해줌 
        - 확장 메소드에서 형 자체는 확장된 것이 아님
    - 익명형(anonymous type): 
        - 객체 초기화 함수에 의해 자동저긍로 추론되어 생성된 형

### 2.1.1 동작중인 프로세스의 목록 생성하기
- 컴퓨터에서 돌아가고 있는 모든 프로세스의 목록을 얻으려고 하는 상황
- System.Diagnostic.Process.GetProcesses API를 사용하면 쉽게 구할 수 있음
[프로세스의 목록을 열거하는 간단한 .NET 2.0기반의 코드]
```C#
class LanguageFeatures
{
        static void DisplayPrcoesses()
        {
            List<String> processes = new List<String>();
            foreach (Process process in Process.GetProcesses())
            {
                processes.Add(process.ProcessName);
                Console.WriteLine(process.ProcessName);
            }

            
        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
}
```
- 여기서는 제너릭 리스트 List<T>를 기반으로 한 목록을 이용함
- 제너릭은 .NET 2.0 에 추가된 중요한 내용임
- 프로세스의 이름 외에 프로세스에 대한 정보를 더 알고 싶다면 어떻게 해야 하나?

### 2.1.2 결과를 클래스 형태로 조합하기
- 리스트에 프로세스의 ID, 이름, 메모리 사용량 등을 포함시키려고 함
- 이 작업을 수행하기 위해서는 새로운 클래스나 구조를 만들어 프로세스에 대해서 알고 싶은 정보들을 그룹화하여 다룰 수 있어야 함
```C#
 class LanguageFeatures2
{
        class ProcessData
        {
            public Int32 Id;
            public Int64 Memory;
            public String Name;

            public override string ToString()
            {
               return "Id=" + Id + ",  Name=" + Name + ",  Memory=" + Memory;
            }
        }

        static void DisplayPrcoesses()
        {
            List<ProcessData> processes = new List<ProcessData>();
            foreach (Process process in Process.GetProcesses())
            {
                ProcessData data = new ProcessData();
                data.Id = process.Id;
                data.Name = process.ProcessName;
                data.Memory = process.WorkingSet64;
                processes.Add(data);
                Console.WriteLine(data.ToString());
            }

        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
}
```
- 코드가 원하는 것을 출력해주지만 아직 중복된 정보가 있음
- 객체의 형이 두 번 명시되어 있음 
    - 변수의 정의를 위해서
    - 생성자를 호출하기 위해서
- 앞으로는 코드를 더 간결하게, 불필요한 중복을 피할 수 있게 할 것임

## 2.2 암시적으로 형이 정의된 로컬 변수
`var i = 5;`
- C# 3.0은 명시적으로 형 지정하지 않고 로컬 변수를 선언 가능한 var이라는 새로운 키워드를 제공함
- 로컬 변수를 선언하면서 var 키워드 사용하면 컴파일러는 그 변수를 초기화할 때, 사용하는 식에서 형을 추론하여 설정함.
- 예제 코드를 이 새로운 키워드를 사용해서 수정하자!

### 2.2.1 문법
- var 키워드는 사용하기가 쉬움
- 이것을 사용하기 위해서는 생성하려는 로컬 변수 이름 앞에 var 키워드를 붙여주고 초기화 식으로 초기화하기만 하면 됨

- 명시적으로 형을 선언한 변수들을 사용한 코드와 명시적으로 형을 선언하지 않은 변수를 사용한 코드를 비교해보자!
1.
```C#
var i = 12;
var s = "Hello";
var d= 1.0;
var numbers = new int[] {1,2,3};
var process = new ProcessData();
var processes = 
    new Dictionary<int, ProcessData>();
```

2.
```C#
int i = 12;
string s = "Hello";
double d= 1.0;
int[] numbers = new int[] {1,2,3};
ProcessData process = new ProcessData();
Dictionary<int, ProcessData> processes = 
    new Dictionary<int, ProcessData>();
```
- 명시적으로 형을 선언하지는 않았지만, var 키워드를 이용해서 생성한 변수는 필수적으로 형을 갖게 됨

### 2.2.2 앞의 에제를 암시적으로 형을 정의한 로컬 변수를 이용하여 개선시키기
```C#
class LanguageFeatures3
    {
        class ProcessData
        {
            public Int32 Id { get; set;}
            public Int64 Memory { get; set; }
            public String Name { get; set; }

            public override string ToString()
            {
                return "Id=" + Id + ",  Name=" + Name + ",  Memory=" + Memory;
            }
        }

        static void DisplayPrcoesses()
        {
            var processes = new List<ProcessData>();
            foreach (var process in Process.GetProcesses())
            {
                var data = new ProcessData();
                data.Id = process.Id;
                data.Name = process.ProcessName;
                data.Memory = process.WorkingSet64;
                processes.Add(data);
                Console.WriteLine(data.ToString());
            }

        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
    }
```
- 이 예제 코드는 앞선 코드와 완벽히 동일하게 동작함
- 간단하고 간소한 문법을 사용하면서도 엄격하고 명시적으로 형을 선언한 것처럼 컴파일 시 validation이나 IntelliSense의 혜택을 받을 수 있음

- 꼭 필요한 곳에만 사용하도록 주의해아 함
- 예제를 조금 더 발전시켜보자~!(ProcessData 객체를 초기화하는 것이 매우 길고 번잡한 코드를 필요로 함-> 개선시켜보자)

## 2.3 객체와 컬렉션 초기화 함수
`new Point{X=1, Y=2}`
- 객체와 컬렉션을 초기화하는 방법에 대한 소개로부터 시작할 것!
- 새롭게 배운 객체 초기화 함수를 통해 예제 코드를 한 번 더 수정할 것!

### 2.3.1 객체 초기화 함수의 필요성
- 객체 초기화 함수는 어떤 객체의 하나 이상의 속성값에 단 한줄의 명령만으로 값을 설정 가능하게 함
- 이 도구는 모든 종류의 객체에 대해 선언적으로 초기화가 가능하게 해줌
- 지금까지는 기본형이나 배열형의 객체에 대해 다음과 같은 형태로 초기화가 가능했었음
```C#
int i = 12;
string s = "abc";
string[] names = {"LLINQ", "In", "Action"};
```
- 그러나 다른 형태의 객체에 이러한 간단하 방법으로 초기홯할 수 있는 방법은 없었음
- 그렇게 하기 위해서 이런 구질구질한 코드를 작성해야 했었음
```C#
ProcessData data = new ProcessData();
data.Id = 123;
data.Name = "MyProcess";
data.Memory = 123456;
```
- C# 3.0 을 출발점으로 해서 간편해진 초기화 함수를 이용하여 모든 개체를 초기화할 수 있게 됨
```C#
var data = new ProcessData { Id=123, Name="MyProcess", Memory=123456};
```
- 생성자를 사용하는 것이 필요하거나 유용한 상황에서도 이런 객체 초기화 함수를 사용 가능함.
- 다음 예에서는 객체 초기화 함수를 함께 사용하여 객체를 생성하고 있음

```C#
throw new Exception("message") {Source = "LINQ in Action"};
```
- 여기서는 Message(생성자 통해서), Source(객체 초기화 도구를 이용해서) 두 개의 프로퍼티를 한 줄의 코드로 초기화함
- 만약 새로운 문법 활용하지 않았다면 다음의 코드처럼 되었을 것
```C#
var exception = new Exception("message");
exception.Source = "LINQ in Action";
throw exception;
```

### 2.3.2 컬렉션 초기화 함수
- 컬렉션 초기화 함수라는 다른 종류의 초기화 함수가 추가됨
- 이 문법은 다양한 여러 종류의 컬렉션이 System.Collections.IEnumerable만 제대로 구현하고 있다면 Add 메소드를 사용한 것과 같은 결과를 얻을 수 있게 해줌

`var digits = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};`

- 예전에는 아래와 같은 코드를 작성해야 했음
- 이제는 위의 코드를 작성하면 컴파일러가 알아서 아래와 같은 코드로 변환시킨 후 컴파일해줌

```C#
List<int> digits = new List<int>();
digits.Add(0);
digits.Add(1);
digits.Add(2);
...
digits.Add(9);
```
- 객체와 컬렉션 초기화 도구를 사용한 코드와 그렇지 않은 코드의 차이를 보자!
[컬렉션 초기화 도구를 사용한 코드]
```C#
var processes = new List<ProcessData>{
    new ProcessData {Id=123, Name="devenv"},
    new ProcessData {Id=456, Name="firefox"}
}
```
[컬렉션 초기화 도구를 사용하지 않은 코드]
```C#
ProcessData tmp;
var processes = new List<ProcessData>();
tmp = new ProcessData();
tmp.Id=123;
tmp.Name="devenv";
processes.Add(tmp);
tmp = new ProcessData();
tmp.Id=456;
tmp.Name="firefox";
processes.Add(tmp);
```
- IEnumerable 인터페이스를 구현하는 클래스로 표현됨
- 여기서는 {x,y,z} 같은 형태의 문법을 사용해서 Add 메소드에 넣고자 했던 매개변수들을 설정 가능함
- 이는 많은 프레임워크내 기존 컬렉션 클래스들고 서드파티 라이브릴들의 컬렉션 클래스들을 활용 가능하게 해줌
- 이런 일반화는 다음과 같은 문법을 가진 딕셔너리를 초기화할 수 있게 해줌

`new Dictionary<int, string>{{1, "one"}, {2, "two"}, {3, "three"}}`

### 2.3.3 객체 초기화 함수를 이용하여 예제 개선시키기
- ProcessData 객체를 생성하기 위해서는 여러 줄의 코드와 하나의 임시 변수를 이용해야 했음
```C#
ProcessData data = new ProcessData();
data.Id=process.Id;
data.Name=process.ProcessName;
data.Memory = process.WorkingSet64;
processes.Add(data);
```
- 하나의 생성자를 추가하는 방법으로 이런 객체를 단 한번에 초기화 가능함
```C#
class LanguageFeatures4
    {
        class ProcessData
        {
            private string processName;
            private long workingSet64;

            public ProcessData(int id, string processName, long workingSet64)
            {
                Id = id;
                this.processName = processName;
                this.workingSet64 = workingSet64;
            }

            public Int32 Id { get; set; }
            public Int64 Memory { get; set; }
            public String Name { get; set; }

            public override string ToString()
            {
                return "Id=" + Id + ",  Name=" + Name + ",  Memory=" + Memory;
            }
        }

        static void DisplayPrcoesses()
        {
            var processes = new List<ProcessData>();
            foreach (var process in Process.GetProcesses())
            {
                processes.Add( new ProcessData(process.Id, process.ProcessName, process.WorkingSet64));
                System.Console.WriteLine(process.ToString());
            }

        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
    }
```
- 생성자를 추가하는 과정은 ProcessData형에 코드를 추가하는 과정이 수반됨
- 이렇게 추가된 생성자는 앞으로의 용법에 적합하지 않을 수 있음
- 이를 위한 새로운 방법: **객체 초기화** 문법을 이용하는 것
```C#
class LanguageFeatures5
{
        class ProcessData
        {
            public Int32 Id { get; set; }
            public Int64 Memory { get; set; }
            public String Name { get; set; }

            public override string ToString()
            {
                return "Id=" + Id + ",  Name=" + Name + ",  Memory=" + Memory;
            }
        }

        static void DisplayPrcoesses()
        {
            var processes = new List<ProcessData>();
            foreach (var process in Process.GetProcesses())
            {
                processes.Add(new ProcessData { Id = process.Id, Name = process.ProcessName, Memory = process.WorkingSet64 });
                System.Console.WriteLine(process.ToString());
            }

        }
        static void Main(string[] args)
        {
            DisplayPrcoesses();
            Console.ReadKey();
        }
}
```
- 이런 경우 새로운 생성자를 새롭게 정의하지 않아도 됨

- 객체 초기화 도구의 장점들
    - 하나의 명령문만으로 객체 초기화 가능
    - 객체들을 초기화하기 위해 별도로 생성자를 정의할 필요 없음
    - 객체의 다른 속성들을 초기화하기 위해 복수의 생성자를 작성할 필요가 없음
    
## 2.4 람다 표현식
`address => address.City == "Paris"`
- **람다 표현식**: 
    - 람다 계산법의 세계로부터 탄생한 개념
    - 함수형 언어들은 함수를 정의하기 위해 람다 표현법을 사용함
    - C#이나 VB.NET 같은 큰 범주에서 람다 표현식 도입 -> LINQ가 함수형 언어의 장점을 흡수할 수 있게 됨

- 예제에 필터링 기능을 추가하기 위해서는 어떤 메소드를 다른 메소드에 매개변수로 넘겨줄 수 있도록 하는 대리자(delegate)라는 아이를 사용 가능함

### 2.4.1 대리자에 대한 복습
```C#
static void DisplayPrcoesses()
{
    var processes = new List<ProcessData>();
    foreach (var process in Process.GetProcesses())
    {
        if (process.WorkingSet64 >= 20 * 1024 * 1024)
        {
            processes.Add(new ProcessData(process.Id, process.ProcessName, process.WorkingSet64));
            System.Console.WriteLine(process.ToString());
        }
    }
}
```
- WorkingSet64는 연관된 프로세스에 할당된 물리적 메모리의 양을 나타냄
- 20메가바이트 이상의 메모리를 할당받은 프로세스를 검색하고 있음

- 다음과 같은 코드의 필터링 메소드는 하나의 Process 객체를 매개변수로 받아들여 어떤 프로세스가 특정 조건에 적합한지 알려주는 Boolean 값을 반환함
`delegate Boolean FilterDelegate(Process process);`

- 이런 방법 대신 .NET 2.0이 제공하는 Predicate<T> 형을 이용 가능함
`delegate Boolean Predicate<T>(T obj);`

- Predicate<T> 대리자 형식은 입력에 따라 true와 false를 반환하는 메소드를 표현함
- 이 형이 일반적이기 때문에 이것이 Process 객체에 동작한다는 것을 알려주어야 함
- 여기서 사용하는 정확한 대리자 형식은 Predicate<Process>임.

- DisplayProcess 메소드가 서술어(predicate)를 매개변수로 받아들이는 데 사용되는 용법을 보여줌
```C#
static void DisplayPrcoesses(Predicate<Process> match)
{
    var processes = new List<ProcessData>();
    foreach (var process in Process.GetProcesses())
    {
        if (match(process))
        {
            processes.Add(new ProcessData { Id = process.Id, Name = process.ProcessName, Memory = process.WorkingSet64 });
            System.Console.WriteLine(process.ToString());
        }
    }
}
```
- 예제 코드에 있는 것처럼, DisplayProcesses가 수정된 것을 보면 이제 어떤 필터도 사용 가능하게 됨
- 이 경우 필터링 메소드는 조건에 맞으면 true를 반환하게 되어있음
```C#
static Boolean Filter(Process process)
{
    return process.WorkingSet64 >= 20*1024*1024;
}
```
- 메소드를 사용하기 위해서 메소드를 DisplayProcesses 메소드에 매개변수로 넘겨줘야 함
`DisplayProcesses(Filter);`

### 2.4.2 익명 메소드
- 익명 메소드를 사용하게 되면 좀 더 간결한 코드를 작성할 수 있도록 명시적으로 지정한 메소드를 사용하지 않아도 됨
- 익명 메소드 덕분에 Filter와 같은 메소드를 선언하지 않고 다음과 같이 DisplayProcesses에 곧바로 코드를 집어넣을 수 있음
```C#
DisplayProcesses( delegate (Process process)
{ return process.WorkingSet64 >= 20*1024*1024; });
```
- 익명 메소드는 함수 객체와 유사하게 멋지게 한 줄의 코드로 컬렉션 내의 항목들을 수정하는 용도로 사용 가능함
- .NET 2.0은 System.Collection.Generic.List<T>와 System.Array에 익명 메소드를 사용할 수 있도록 특화되어 설계된 몇 개의 메소드를 포함시켰음
- 이런 메소드에는 ForEach, Find, FindAll과 같은 메소드가 있음
- 이런 메소드는 리스트나 배열에 대해 상대적으로 적은 코드를 가지고 많은 일을 할 수 있게 해줌

### 2.4.3 람다 표현식의 소개
- 다음의 코드는 이전의 코드와 완벽하게 동일한 결과를 가져옴
`DisplayProcesses(process => process.WorkingSet64 >= 20*1024*1024);`

- 어떤 프로세스가 주어졌을 떄 만약 그 프로세스가 20MB 이상의 메모리를 소비하고 있다면 true를 반환하라
- 람다 표현식을 사용할 경우, 매개변수의 형이 무엇인지 알려줄 필요가 없음
- C# 컴파일러는 매개변수의 형을 메소드 시그너처로부터 유추 가능함

#### 람다 표현식과 익명 메소드의 비교
- C# 2.0은 익명 메소드라는 방식으로 코드 블록을 대리자가 예상되는 곳에 집어넣을 수 있는 새로운 기능을 도입함
- 익명 메소드 문법은 비교적 복잡하고 명령적인 형태를 띄고 있음
- 이와 반대로 람다 표현식은 좀 더 간결하고 함수형 프로그래밍 언어의 형태를 띄고 있어서 이해하기 쉽다는 장점이 있음

- 람다 표현식은 기능상 다음과 같은 추가기능을 제공하는 익명 메소드의 상위 집합임
    - 람다 표현식은 매개변수의 형을 추론할 수 있으므로 생략 가능함
    - 람다 표현식은 명령문 또는 표현식을 그 내용으로 할 수 있으므로 문법상 명령문의 형태만을 띨 수 있는 익명 메소드보다 간결함
    - 람다 표현식은 매개변수로 주어졌을 때 형 매개변수를 추론 가능, 메소드 오버로드 분석에 사용될 수 있음
    - 표현식을 그 내용으로 하는 람다 표현식은 표현식 트리로 변환될 수 있음

#### 람다 표현식을 나타내는 방법
- C#에서 람다 표현식은 매개변수를 나열한 후 "=>"를 쓰고 표현식이나 선언문으로 다음처럼 나타낼 수 있음
```C#
process         =>              process.WorkingSet64 >= 20*1024*1024;
입력 매개변수   람다연산자          표현식 또는 문장 블록
```
- 람다 연산자는 "goes to"의 의미임
- 연산자의 좌변은 입력 매개변수
- 연산자의 우변은 해석될 표현식이나 문장

- 람다 표현식의 종류
    - 표현식 람다: 우변에 표현식을 갖고 있는 람다 표현식
    - 명령문 람다: 우변에 중괄호로 묶인 가변 개수의 문장이 올 수 있음

[C#으로 작성된 람다 표현식의 예]
```C#
//암묵적으로 형을 갖는 표현식
x => x+1; 
//암묵적으로 형을 갖는 명령문
x => {return x+1;}
//명시적으로 형을 갖는 표현식
(int x) => x+1;
//명시적으로 형을 갖는 명령문
(int x) => {return x+1;}
//복수의 매개변수
(x, y) => x*y;
//매개변수가 없는 표현식
() => 1
//매개변수가 없는 명령문
() => Console.WriteLine() customer => customer.Name
person => person.City == "Paris"
(person, minAge) => person.Age >= minAge
```

- 람다 표현식은 대리자와 매우 유사한 점이 많음
- 람다 표현식이 대리자와 얼마나 잘 어울리는지 살펴보자!(몇 가지 대리자형을 이용해보자)

- System.Action<T>, System.Converter<TInput, TOutput>, System.Predicate<T>와 가은 일반 대리자형들은 .NET 2.0에서 도입됨
```C#
    delegate void Action<T>(T obj);
    delegate TOutput Converter<TInput, TOutput>(TInput input);
    delegate Boolean Predicate<T>(T obj);
```
- 이보다 이전 버전의 .NET에서 도입된 대리자형은 MethodInvoker
- 이 형은 매개변수를 받아들이지 않고 결과도 내놓지 않는 형태의 아무 메소드나 나타낼 수 있음
`   delegate void MethodInvoker();`
- MethodInvoker는 Window Forms 형태의 애플리케이션이 아니더라도 유용하게 사용될 수 있음
- 그러나... System.Windows.Forms 네임스페이스 내에 정의된 것은 매우 유감스러운 것...
- 매개변수를 받아들이지 않는 새로운 Action이라는 대리자형이 System.Core.dll 어셈블리에 포함되어 System 네임스페이스에 추가될 것으로 보임
`delegate void Action();`

- 상당히 많은 종류의 새로운 대리자형이 System.Core.dll 속에 담겨 System 네임스페이스에 추가되었음
```C#
    delegate void Action<T1, T2>(T1 arg1, T2 arg2);
    delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
    delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    delegate TResult Func<TResult>();
    delegate TResult Func<T, TResult>(T arg);
    delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2);
    delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
    delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4); 
```

- 람다 표현식은 다음 같은 조건들을 충족시킬 경우 대리자와의 호환성을 가짐
    - 람다는 대리자형과 동수의 매개변수들을 가져야 함
    - 각각의 입력 매개변수들은 상응하는 대리자의 매개변수로 암묵적으로 변환이 가능해야 함
    - 람다의 반환값이 있다면 대리자의 반환형으로 암묵적으로 변환이 가능해야 함

[C#에서 대리자로 선언된 람다 표현식]
```C#
//매개변수가 없는 경우
Func<DateTime> getDateTime = () => DateTime.Now;
//암묵적으로 형을 갖는 문자열 매개변수
Action<string> printImplicit = s => Console.WriteLine(s);
//명시적으로 형을 갖는 문자열 매개변수
Action<string> printExplicit = (string s) => Console.WriteLine(s);
//두 개의 암묵적으로 형을 갖는 매개변수
Func<int, int, int> sumInts = (x, y) => x+y;
//동등하지만 호환이 안 되는 경우
Predicate<int> equalsOne1 = x => x == 1;
Func<int, bool> equalsOne2 = x => x ==1;
//같은 람다 표현식이지만 다른 대리자형을 갖는 경우
Func<int, int> incInt = x => x+1;
Func<int, double> incIntAsDouble = x => x+1;
//명령문과 명시적인 형을 갖는 매개변수
Func<int, int, int> comparer = (int x, int y) =>
{
    if(x>y) return 1;
    if(x<y) return -1;
    return 0;
};
```

## 2.5 확장 메소드

`static void Dump(this object o);`

- 어떤 형이 정의된 후 어떻게 그 형에 메소드를 추가시킬 수 있는지 살펴볼 것

#### 2.5.1 확장 메소드 작성해보기
[예제 코드: 표준 정적 메소드로 구현된 TotalMemory 메소드]
```C#
static Int64 TotalMemory(IEnumerable<ProcessData> processes)
{
    Int64 result = 0;

    foreach(Var process in processes)
        result += process.Memory;
    
    return result;
}
```
- 이 메소드는 다음 처럼 활용 가능함
```C#
Console.WriteLine("Total memory: {0} MB", TotalMemory(processes)/1024/1024);
```
- 예제 코드를 개선하기 위해 시도할 수 있는 것 중 하나: 정적 메소드를 확장 메소드로 전환해보는 것

### 2.5.2 C#에서 확장 메소드 선언하기
- 예제의 메소드를 확장 메소드로 변환시키기 위해서는 첫 번째 매개변수에 `this` 키워드만 하나 넣어주면 됨
```C#
static Int64 TotalMemory(this IEnumerable<ProcessData> processes)
{
    Int64 result = 0;

    foreach(Var process in processes)
        result += process.Memory;
    
    return result;
}
```
- This 키워드는 컴파일러에게 메소드를 확장 메소드로 다룰 것을 요청함
- 이 키워드로 인해 컴파일러는 이 메소드가 IEnumerable<ProcessData> 형의 객체를 확장하는 메소드라는 것을 확실히 알 수 있고 그에 맞게 처리하게 됨
- 이제는 TotalMemory 메소드를 processes 의 객체에 정의된 인스턴스 메소드처럼 사용 가능하게 됨
```C#
Console.WriteLine("Total Memory: {0} MB",
    processes.TotalMemory()/1024/1024);
```
- 확장 메소드가 지원하는 형이라면 IntelliSense 의 도움을 받으며 TotalMemory 메소드를 사용 가능함.
- 확장 메소드는 예전의 정적 헬퍼 메소드들에 비해 IntelliSense 의 기능을 활용 가능하다는 매우 큰 장점을 가짐

- 확장 메소드의 추가적인 장점:  연달아서 묶음으로 처리하기 쉽다
- 예시) 다음과 같은 연산을 수행한다고 가정하자~~
    1. 헬퍼 메소드를 이용하여 ProcessData 객체들의 컬렉션에서 몇 개의 프로세스를 선별해냄
    2. TotalMemory를 이용하여 전체 메모리 사용량을 계산함
    3. 메모리 사용량을 다른 헬퍼 메소드를 이용하여 메가바이트 단위로 환산함
- 헬퍼 메소드 사용 시 다음처럼 코드가 작성됨
```C#
 BytesToMegaBytes(TotalMEmory(FilterOutSomeProcesses(processes)));
```
- 이러한 코드의 문제점: 연산들이 수행 순서와 반대로 나열되어 있음
    - 읽기도 어렵고 작성하기 어려운 부자연스러운 형태가 됨

- 반면, 세 개의 확장 메소드 형태의 헬퍼 메소드들이 정의되었다면 다음처럼 작성되었을 것임
```C#
processes
    .FilterOutSomeProcesses()
    .TotalMemory()
    .BytesToMegaBytes();
```
- 자신들이 수행되는 순서에 맞게 연산들이 나열되고 있음
- 직관적이고 가독성이 높음
- 줄줄이 호출하는 방식 -> LINQ 질의의 핵심기능

#### VB.NET에서 확장 메소드 선언하기
- VB.NET에서 확장 메소드는 사용자 정의 프로퍼티인 System.Runtime.CompilerServices.ExtensionAttribute로 치장된 인스턴스 메소드 구문으로 호출 가능한 공유 메소드
- 확장 메소드는 Sub 형태 또는 Procedure 형태의 프로시저가 될 수 있음
    - 새롭게 추가된 System.Core.dll 어셈블리에 의해 지원됨
- VB.NET 확장 메소드의 첫 번째 매개변수에는 메소드가 어떤 데이터형을 확장하는지 명시함
- 메소드 실행 시, 첫 번째 매개변수는 메소드가 적용된 데이터형의 인스턴스에 바인딩됨

- 다음 코드는 TotalMemory 확장 메소드를 VB.NET에서 어떻게 정의할 지 보여줌
[VB.NET으로 작성된 확장 메소드의 예]
```C#
<System.Runtime.CompilerServices.Extension()> _
Public Function TotalMemory(
  ByVal processes As IEnumerable(Of ProcessData)) _
   As Int64
   Dim result As Int64 = 0
   For Each process in processes
    result += process.Memory
   Next
   Return Result
end Function
)
```
### 2.5.2 LINQ의 표준 질의 연산자들을 사용한 예
**OrderByDescending**
- 메모리 사용량을 기반으로 프로세스의 목록을 메모리를 많이 차지하는 순으로 정렬하고 싶다~~
- System.Linq.Enumerable에 정의된 OrderByDescending 확장 메소드 사용 가능
- 확장 메소드는 네임스페이스를 명시하는 형태로 불러들일 수 있음
- 이런 식으로 코드의 맨 앞에 추가시켜야 함
`using System.Linq`
- 이제는 프로세스를 정렬하기 위해 OrderByDescending을 사용 가능함
```C#
ObjectDumper.Write(
    processes.OrderByDescending(process => process.Memory));
```
- 매개변수로 던져준 내용을 살펴보면 프로세스들을 메모리 사용량을 바탕으로 정렬하고자 한다는 의도를 알 수 있음
- **코드를 간소화하기 위해 자동으로 알아서 형을 유추함**
- 비록 OrderByDescending이 제네릭 메소드로 구현되었음 그러나 다루려는 형을 명시적으로 선언해줄 필요가 없음
- C# 컴파일러가 알아서 OrderByDescending이 Process 객체에 동작하여 Int64 객체를 반환하는 메소드라는 것을 추론해냄
- 형을 선언하지 않고, 컴파일러가 알아서 추론하게 하는 것은 제너릭 메소드를 호출하는 구문을 좀 더 유연하고 효율적이게 해줌
    - 프로그래머는 매번 형에 관한 정보를 명시해주는 수고를 하지 않아도 됨
```C#
public static IOrderedSequence<TSource>
  OrderByDescending<TSource, TKey>(
      this IEnumerable<TSource> source,
      Func<TSource, TKey> keySelector)
```
-  형 추론기능을 사용할 수 없다면...?
```C#
processes.OrderByDescendig<Process, Int64>(
    (Process process) => process.Memory));
```
- 만약 형 추론을 사용 불가능하고 LINQ 질의의 여기저기에 형을 명시해야 한다면 코드의 가독성은 떨어질 것

**Take**
- 만약 가장 메모리를 많이 차지하는 두 개의 프로세스를 찾아내는 것에만 관심 있다면 Take 확장 메소드 이용 가능함
```C#
ObjectDumper.Write(
    processes
        .OrderByDescending(process => process.Memory)
        .Take(2));
```
- Take 메소드는 어떤 객체의 집합에서 처음 n개의 객체를 선발하여 반환함

**Sum**
- 만약 두 개의 프로세스에서 사용되는 메모리량의 합을 구하고 싶다면 다른 표준확장 메소드인 Sum을 활용 가능
- 예제에서 사용한 확장 메소드인 TotalMemory 대신 사용 가능함
```C#
 ObjectDumper.Wirte(
    processes
        .OrderByDescending(process => process.Memory)
        .Take(2)
        .Sum(process => process.Memory)/1024/1024);
```
### 2.5.3 예제에서 실제로 사용되는 확장 메소드
- 지금까지의 수정사항을 모두 반영한 다음의 코드(DisplayProcess)
[확장 메소드를 이용하여 구현한 DisplayProcesses 메소드]
```C#
static void DisplayProcesses(Func<Process, Boolean> match)
{
    var processes = new List<ProcessData>();
    foreach(var process in PRocess.GetProcesses())
    {
        if(match(process))
        {
            processes.Add(new ProcessData
            {
                Id = process.Id,
                Name = process.ProcessName, 
                Memory = process.WorkingSet64
            });
        }
    }

    Console.WriteLine("Total memory: {0} MB",
        processes.TotalMemory()/1024/1024);
    
    var top2Memory =
        processes
            .OrderByDescending(process => process.Memory)
            .Take(2)
            .Sum(process => process.Memory)/1014/1024;
    Console.WriteLine(
        "Memory consumed by the two most hungry processes: {0} MB", top2Memory);

    ObjectDumper.Write(processes);
}
```
[정적 메소드를 이용한 방법]
```C#
var top2Memory =
    Enumerable.Sum(
        Enumerable.Take(
            Enumerable.OrderByDescending(processes, process => process.Memory),
            2),
            process => process.Memory)/1024/1024;
```
[확장 메소드를 이용한 방법]
```C#
var top2Memory =
  processes.
    .OrderByDescending(process => process.Memory)
    .Take(2)
    .Sum(process => process.Memory)/1024/1024;
```
- 점으로 연결된 표현방식을 통해 줄줄히 연결된 형태의 호출을 사용하기에 편리하게 되어 있음
- Unix의 파이프 기능과 견줄 수 있는 형태의 구조임

- 두 번째 형태를 따라 확장 메소드 이용 시 가독성 높아지고 이해하기 쉬워짐
- 연산과정은 정확히 묘사되어 있음
- 프로세스들을 메모리 사용량으로 정렬, 최초의 두 개를 선택, 메모리 사용량을 합산하려고 함
- 첫 번째 코드에서는 이런 의도가 명확히 표현되지 않음 <- 첫 번째 경우에는 괄호에 둘러싸인 복잡한 구조의 메소드 호출이 주를 이루고 있어서!

### 2.5.4 주의사항
- 만약 확장 메소드가 다른 인스턴스 메소드와 충돌한다면...?
- 확장 메소드가 어떤 순서대로 호출되고 어떤 우선순위를 갖는지 확인하는 것은 매우 중요함
    - 확장 메소드는 인스턴스 메소드들에 비해 찾기가 매우 어려움
- 확장 메소드들이 항상 좀 더 낮은 우선순위를 갖고 있음
- 확장 메소드는 인스턴스 메소드보다 먼저 호출되지 않음

[확장 메소드의 검색 용이성을 보여주는 예제 코드]
```C#
using System;

class Class1
{
}

class Class2
{
    public void Method1(string s)
    {
        Console.WriteLine("Class2.Method1");
    }
}

class Class3
{
    public void Method1(object o)
    {
        Console.WriteLine("Class3.Method1");
    }
}

class Class4
{
    public void Method1(int I)
    {
        Console.WriteLine("Class4.Method1");
    }
}

static class Extensions
{
    static public void Method1(this object o, int I)
    {
        Console.WriteLine("Extensions.Method1");
    }
}

static void Main()
{
    new Class1().Method1(12);
    new Class2().Method1(12);
    new Class3().Method1(12);
    new Class4().Method1(12);
}
```
[수행 결과]
```C#
Extensions.Method1
Extensions.Method1
Class3.Method1
Class4.Method1
```
- 정확히 맞는 매개변수형을 가진 인스턴스 메소드를 발견 시 그 메소드가 수행됨
- 정확히 들어맞는 매개변수형을 가진 인스턴스 메소드가 발견되지 않을 경우에만 확장 메소드가 수행됨

- 확장 메소드들은 인스턴스 메소드들에 비해 기능적 제약이 더 큼
- 확장 메소드들은 멤버가 public이 아니면 접근 불가능함
- 언제 확장 메소드를 사용하는지 모르게 빈번히 사용 -> 코드의 가독성 저해 가능성 있음
- 확장 메소드를 사용하는 것이 유리한 상황일 떄만 쓰는 것이 좋다.

- 확장 메소드 기능을 사용하면 예제코드를 아주 효율적으로 작성 가능함
## 2.6 익명형
- 객체 초기화 도구와 유사한 문법을 사용하면서 익명형을 생성할 수 있음
- 익명형이 실제 형인지 알아보고 이 방식의 한계점을 알아볼 것

### 2.6.1 익명형을 이용하여 데이터를 객체로 그룹하하기
- 예를 들어, 연산의 결과물을 모두 함께 그룹화하려는 경우, 이 정보를 객체화하여 담아두려고 한다고 하자
- 임시 저장 목적을 위해 특정한 형을 정의하고 사용하는 것은 번거로운 일...!

[C#에서 익명형을 이용할 수 있는 방법]
```C#
var results = new {
    TotalMemory = processes.TotalMemory()/1024/1024,
    Top2Memory = top2Memory,
    Processes = processes
};
```
### 2.6.2 이름이 없어도 형은 형이다
- 익명형은 이름이 없지만 분형히 형이다
- 컴파일러가 동적으로 실제의 형을 추론하여 생성@
- 예제를 살펴보면 프로퍼티들의 형은 초기화 도구에 의해 추론되고 있음

- 컴파일러는 같은 프로그램 내에서 같은 이름과 형을 프로퍼티로 가진 익명형이 있는 경우, 두 형이 같다고 판단할 수 있다는 점에 주의해야 함
- 이 두 줄을 수행시킬 경우 컴파일러는 하나의 형만을 자동으로 생성하게 됨
```c#
    var v1 = new { Person = "Suzie", Age=32, CanCode = true}
    var v2 = new { Person = "Barney", Age=29, CanCode = false}
```
- 이 때 다음과 같은 세 번째 줄을 추가하면 프로퍼티의 순서가 바뀌어서 v3을 위해 새로운 형이 생성됨
`var v3 = new {Age=17,  Person="Bill", CanCode=false}`

### 2.6.3 익명형을 사용하여 예제 개선시키기
- ProcessData 객체 제거가 가능함
- ProcessData 클래스 대신에 익명형을 이용하는 DisplayProcesses 메소드를 보여줌
```C#
class LlanguageFeatures7
    {
        
        static void DisplayPrcoesses(Predicate<Process> match)
        {
            var processes = new List<Object>();
            foreach (var process in Process.GetProcesses())
            {
                if (match(process))
                {
                    processes.Add(new {
                        process.Id,
                        process.ProcessName,
                        process.WorkingSet64 });
                    System.Console.WriteLine(process.ToString());
                }
            }
        }

        static Boolean Filter(Process process)
        {
            return process.WorkingSet64 >= 20 * 1024 * 1024;
        }

        static void Main(string[] args)
        {
            DisplayPrcoesses(Filter);
            Console.ReadKey();
        }
    }
```
- ProcessData 클래스를 선언할 필요가 없어짐
- 그러나... 익명형에도 수많은 제약사항이 있음

### 2.6.4 제약사항
- TotalMemory 메소드는 ProcessData 객체들과 사용할 수 있도록 정의되어 있음
- ProcessData 클래스를 없앤다면 곤란한 상황에 직면하게 됨
- 익명형을 이용하게 될 경우, 메소드가 정의된 영역 밖에서 객체를 엄격하게 고정된 형을 가진 형태로 다루는 것이 불가능해짐
- 메소드가 object를 매개변수로 받기를 기대하고 있을 때만 익명형을 메소드에 전달할 수 있다는 것
- 또한 좀 더 자세한 형을 원하고 있다면 불가능할 수도 있음
- Reflection은 메소드가 생성된 곳 외부에서 익명으로 작업하는 유일한 방법
**Reflection:**
- 어셈블리, 모듈 및 형식을 설명하는 객체를 제공함 

- 익명형은 메소드의 반환형이 object가 아니라면 메소드 결과물로 이용될 수 없음
- 이것이 익명형이 임시 데이터의 저장에만 이용되고 일반형과 같이 메소드 시그니처에서 사용되면 안 되는 이유

- 그러나... 완전 맞는 말은 아님
- 익명형을 제너릭 메소드의 메소드 결과물로 이용 가능함
- 다음 메소드를 고려해보자!
```C#
public static TResult ReturnAGeneric<TResult>(Func<TResult> creator)
{
    return creator();
}
```
- ReturnAGeneric 메소드의 반환형은 제너릭
- TResult형 매개변수에 형을 암시하지 않고 호출할 경우 creator에서 자동적으로 추론됨
- ReturnAGeneric을 호출하는 코드들을 확인해보자!
```C#
var obj = ReturnAGeneric(
    () => new {Time = DateTime.Now, AString = "abc"});
```
- 매개변수로 주어진 creator 함수는 익명형의 인스턴스를 반환하기 때문에 ReturnAGeneric은 그 인스턴스를 반환함
- 그래서 ReturnAGeneric은 그 인스턴스를 반환함
- ReturnAGeneric은 객체가 아닌 일반형만을 반환하도록 정의되어 있음
- 이런 이유로 obj 변수는 엄격하게 형을 적용받는 것
- 이는 DateTime형의 Time 프로퍼티와 String 형의 AString 프로퍼티를 갖고 있음

- 익명형에 대해 한 가지 더 기억해둘 것!: **C#에서 익명형의 인스턴스들은 바꾸지 않는 형태로 고정됨**
- 익명형의 인스턴스를 생성할 경우 이의 항목과 프로퍼티값들은 영원히 고정됨
- 프로퍼티와 항목들에 값을 지정하는 유일한 방법: 생성자를 통해서임
- 만약 문법을 이용하여 익명형 형태의 인스턴스를 초기화하려고 하면, 그 형의 생성자는 즉시 호출되고 값은 영원히 고정되는 형태로 설정됨

- 왜 C#의 익명형이 변경이 불가능하도록 설계되었는가?
    - 의도적인 것임
    - 함수형 언어들의 유연성이 가져올 수 있는 예기치 못한 부수효과를 억제하면서 안정적인 코드작성이 가능하도록 해줌
    - 값이 변하는 것이 허용되지 않은 객체를 이용하면 훨씬 더 안정적으로 작업이 가능
    - 변하지 않는 익명형은 .NET을 좀 더 함수형 언어에 가깝게 해주어 상태를 이용하면서도 부작용이 없는 코드를 작성할 수 있게 해줌

## 2.7 요약
**우리가 알아본 내용들**
- 암시적으로 형이 선언된 로컬 변수
- 객체와 컬렉션 초기화 함수
- 람다 표현식
- 확장 메소드
- 익명형

[복습코드]
```C#
static class LlanguageFeatures8
{
        public static object ObjectDumper { get; private set; }

        internal class ProcessData
        {
            public Int32 Id { get; set; }
            public Int64 Memory { get; set; }
            public String Name { get; set; }
        }
        static void DisplayPrcoesses(Predicate<Process> match)
        {
            var processes = new List<ProcessData>();
            foreach (var process in Process.GetProcesses())
            {
                if (match(process))
                {
                    processes.Add(new ProcessData
                    {
                        Id = process.Id,
                        Name = process.ProcessName,
                        Memory = process.WorkingSet64
                    });
                }
            }
            Console.WriteLine("Total memory: {0} MB", processes.TotalMemory() / 1024 / 1024);

            var top2Memory =
                processes
                .OrderByDescending(process => process.Memory)
                .Take(2)
                .Sum(process => process.Memory) / 1024 / 1024;
            Console.WriteLine(
                "Memory consumed by the two most hungry processes: {0} MB", top2Memory);

            var results = new
            {
                TotalMemory = processes.TotalMemory() / 1024 / 1024,
                Top2Memory = top2Memory,
                Processes = processes
            };
            ObjectDumper.Write(results, 1);
            ObjectDumper.Write(processes);
        }0

        static Int64 TotalMemory(this IEnumerable<ProcessData> processes)
        {
            Int64 result = 0;

            foreach (var process in processes)
                result += process.Memory;

            return result;
        }

        static void Main(string[] args)
        {
            
            Console.ReadKey();
        }
}
```