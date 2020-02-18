# Chapter02. 개선된 C#과 VB.NET의 언어상 기능
## 2. 람다 표현식
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

**Take**
**Sum**
