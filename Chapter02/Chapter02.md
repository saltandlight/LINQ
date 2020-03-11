# Chapter02. 개선된 C#과 VB.NET의 언어상 기능
- 프로그래밍 언어에 잘 내장된 상태로 질의할 수 이씨게 해주는 언어 확장에 대해서 설명할 것
- LINQ는 C#과 VB.NET을 새로운 구조로 확장함
- LINQ가 가능하게 하기 위해 C#과 vB.NET이 어떻게 확장되고 개선되었는지를 살펴볼 것

## c2.1 새로운 언어적 측면에서의 개선점
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
### 2.4.2 익명 메소드
### 2.4.3 람다 표현식의 소개

### 2.5 확장 메소드
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

- 확장 메소드의 추가적인 장점: 연ㅅ나을 연달아서 묶음으로 처리하기 쉽다
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
### 2.6.1 익명형을 이용하여 데이터를 객체로 그룹하하기
### 2.6.2 이름이 없어도 형은 형이다
### 2.6.3 익명형을 사용하여 예제 개선시키기
### 2.6.4 제약사항

## 2.7 요약