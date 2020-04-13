# Chapter10. LINQ to XML을 이용한 XML 질의 및 변환🦢
- **알아볼 내용:**
    - LINQ to XML이 제공하는 질의기능과 변환 기능
    - LINQ를 이용하여 XML 데이터를 어떻게 질의하는지
    - XML API 중 LINQ to XML 축 메소드
        - 축 메소드는 LINQ to XML API 내에서 XML 트리 내의 특정 개체, 속성, 노드에 선택적으로 접근시켜 줌
    - LINQ to XML 축 메소드 확인 후에는 축 메소드들을 표준 질의 연산자나 LINQ 질의 표현식과 조합하여 개발자가 원하는 LINQ의 강력한 질의능력을 어떻게 추출할 수 있는지 알아볼 것
    - XPAth를 이용하여 LINQ to XML 객체를 질의하는 것도 살펴볼 것임
    - 방향을 틀어 어떻게 LINQ to XML 을 이용하여 XML을 다른 형식으로 변환할 수 있는지
    - 모든 것이 완벽하다는 가정 하에 애플리케이션이 받는 XML 데이터는 사용자에게 보여주기 좋은, 필요로 하는 형태로 주어질지도 모름
    - LINQ to XML이 제공하는 강력한 질의기능과 함수형 생성 및 XML 리터럴을 조합하면 개발자는 쉽고 간편하게 XML 문서를 다른 형태로 변환 가능함

## 10.1 LINQ to XML 축 메소드
- LINQ 프레임워크가 제공하는 표준 질의 연산자에 의해 C#에서 가장 기초적인 수준의 구조체가 될 수 있음
- 표준 질의 연산자들은 IEnumerable<T>나 IEnumerable<T>를 구현하는 객체에 수행되었을 경우 일련의 객체 집합을 대상으로 작동 가능함
- LINQ to XML의 경우에 'I'에 해당하느 형은 보통 XElement, XAttribute, XNode일 가능성이 매우 높음

- 표준 질의 연산자를 XML 데이터에 대해 사용하기 위해 XML을 표준 질의 연산자로 질의가 가능한 일련의 객체 집합의 형태로 변환할 필요 있음
- LINQ to XML 축 메소드는 XML에 대해 이런 변환과정을 수행하여 개체, 속성, 노드 등을 검색할 수 있게 해줌
- 축 메소드에 대해 알아보기 위해 사용하게 될 샘플 XML 파일을 한 번 확인해보기
- 다음 예제의 XML은 LINQ Books 카탈로그 내에 있는 책들의 부분집합을 나타냄
- [XML의 트리형태의 구조를 잘 보여주는 예제 XML 파일]
```XML
<category name="Technical">
    <category name=".NET">
        <books>
            <book>CLR via C#</book>
            <book>Essential .NET</book>
        </books>
    </category>
    <category name="Design">
        <books>
            <book>Refactoring</book>
            <book>Domain Driven Design</book>
            <book>Patterns of Enterprise Application Architecture</book>
        </books>
    </category>
        <books>
            <book>Extreme Programming Explained</book>
            <book>Programatic Unit Testing with C#</book>
            <book>Head First Design Patterns</book>
        </books>
</category>
```
- LINQ to XML 축 메소드를 이용하면 관심을 가질만한 개체와 속성들을 추출 가능함
- 어떤 결과가 나올지 이해하고 예측하기 위해서는 XML 트리 내에서 현재 위치가 어디인지 알아야 함

- LINQ to XML 축 메소드에 대해서 알아보자
- LINQ to XML 축 메소드가 어떤 기능을 제공하는지 살펴보기 위해 먼저 축 메소드의 도움을 받아 다음과 같은 결과가 나오도록 해보자
    - .NET
    - - CLR via C#
    - - Essential .NET
- 이런 결과를 만들기 위해서는 Element, Attribute, Elements라는 세 가지 축 메소드에 대해서 알아야 함
- 이 세 가지 축 메소드에 대해서 잘 이해하고 나면 Descendants나 Ancesotrs와 같은 몇몇 다른 축 메소드에 대해서도 알아보도록 하겠음
- 어떻게 Element 축 메소드를 사요하여 목표를 쉽게 달성할 수 있는지 알아보기

### 10.1.1 Element
- 목표로 하는 결과를 얻기 위해 가장 먼저 해야 하는 일: XML에서 .NET 분류를 별도로 추출해내는 것
- Element 축 메소드는 이름을 통해 하나의 XML 개체를 추출하도록 함
- [Element 질의 축 메소드를 이용하여 일므을 바탕으로 개체를 선택하기]
```C#
XElement root = XElement.Load("categorizedBooks.xml");
XElement dotNetCategory = root.Element("category");
Console.WriteLine(dotNetCategory);
```
- 정적인 Load 메소드를 이용하여 XML을 XElement부터 먼저 불러들임
- XML을 다 불러들이면 Element 축 메소드를 가장 상위의 XElement에 대해서 분류를 매개변수로 하여 호출함
- Element 축 메소드는 XNAme을 매개변수로 받아들여 **맨 먼저 들어맞은 현재 개체**의 자식인 XElement를 반환함
- XName에 대해 정의된 오버로딩 연산자는 new XName("category") 대신 분류 이름을 문자열로 전달할 수 있게 해줌
- 해당 오버로딩된 연산자는 "category"라는 문자열을 자동적으로 완전한 XName으로 전환시켜 줌
- 위 예제코드는 다음과 같은 결과를 콘솔창에 출력해줌
```XML
<category name=".NET">
    <books>
        <book>CLR via C#</book>
        <book>Essential .NET</book>
    </books>
</category>
```
- 만약 Element 축 메소드에 전달된 이름으로 개체가 존재하지 않는다면 null이 반환될 것임
- ".NET"이라는 분류에 해당하는 XElement를 별도로 갖고 있으므로 전체 XML 조각 대신 분류의 이름을 출력해야 함
- 분류의 이름은 name 속성에 저장되어 있음

### 10.1.2 Attribute
- XElement 형의 .NET 분류 개체를 메모리상에 가지게 되었으므로 XElement에 대해 name 속성의 값에 대한 질의를 수행할 것임
- name 속성을 가져오기 위해 축 메소드인 XAttribute를 가져올 것임

- Attribute는 제시된 XName을 바탕으로 들어맞는 첫 번째 속성을 반환함
- 이 경우에는 XElement에 한 가지 속성만 정의되어 있지만 항상 그런 것은 아님
- name 속성에 관심이 있으므로 name을 매개변수로 해서 다음 코드와 같이 Attribnute 축 메소드를 호출해보쟝
- [Attribute 메소드를 이용하여 XML 개체에서 속성을 가져오기]
```C#
XElement root = XElement.Load("categorizeBooks.xml");
XElement dotNetCategory = root.Element("category");
XAttribute name = dotNetCategory.Attribute("name");
```
- Element 축 메소드처럼 Attribute는 주어진 XName을 가진 첫 번째 속성을 반환함
- 만약 주어진 XName을 가진 속성이 없다면 null이 반환됨
- XAttribute를 반환받아서 가지고 있음으로 분류의 제목을 다음과 같이 XAttribute를 문자열로 캐스팅하는 형태로 출력 가능함
- `Console.WriteLine((string)name);`
- 다음과 같은 결과를 출력함
```C#
.NET
```

- 일단 XElement를 확보하게 되면 분류의 이름을 Attribute  질의 축 메소드의 도움을 받아 출력이 가능
- 그 다음, 분류 XElement를 질의해서 그 질의에 담긴 모든 책 개체를 가져와야 함
- 불행히도... 복수의 개체를 가져와야 하므로 Element 메소드를 사용 불가능
- Elements 축 메소드를 배울 때가 되었음

### 10.1.3 Elements
- Elements 축 메소드는 Element 질의 축 메소드와 비슷한 일을 함
- **중요한 차이점:**
    - 맨 처음 들어맞는 Element를 반환하는 대신, Elements는 들어맞는 모든 개체를 반환함
    - 이런 측면에서, Elements가 XElement 객체의 IEnumerable을 반환한다는 것이 그리 놀랍지는 않음
    - Element처럼 Elements는 XName을 매개변수로 받아들임

- 이 경우에는 모든 <book> 객체를 찾고 있으므로 책을 Elements에 매개변수로 전달할 것임
- <book> 개체들이 앞에서 선택했던 분류를 나타내는 XElement의 바로 아래에 있는 개체들이 아니므로 Element query 축 메소드를 이용하여 <books> 개체를 선택해야 함
- [Elements 질의 축 메소드를 이용하여 모든 자식 책 개체들을 가져오기]
```C#
XElement root = XElement.Load("categorizedBooks.xml");
XElement dotNetCategory = root.Element("category");
XAttribute name = dotNetCategory.Attribute("name");

XElement books = dotNetCategory.Element("books");
IEnumerable<XElement> bookElements = books.Elements("book");

Console.WriteLine((string) name);
foreach(XElement bookElement in bookElements)
{
    Console.WriteLine(" - "+(string)bookElement);
}
```
- 수행 결과는 다음과 같음
```C#
.NET
 - CLR via C#
 - Essential .NET
```
- Elements 메소드는 매개변수를 받아들이지 않는 오버로딩된 버전을 갖고 있음
- 이 버전은 XElement의 모든 자식 개체를 가져오기 위해 사용 가능함
- 예제 코드에서는 어차피 <books>는 <book> 개체들만 자식 개체로 갖고 있으므로 매개변수를 갖지 않는 버전의 Elements를 호출함

- Elements가 XElement의 바로 하위 자식 개체들만 탐색한다는 것은 반드시 알아둬야 함
- 바로 아래 계층의 자식 개체들보다는 현재 개체보다 하위 계층에 있는 모든 개체들에 대하여 탐색하기 원하는 경우가 많음
- 이런 경우 LINQ to XML에서는 Descendants라는 축 메소드를 지원함

### 10.1.4 Decendants
- Decendants 축 메소드는 Elements 메소드와 동일한 방식으로 동작함
- 그러나 반환되는 개체를 현재 개체의 모든 하위 개체로 해서 반환함
- Decendants 축 메소드는 특정한 XName을 가진 모든 개체들을 가져오고 싶지만 어디쯤 있는지 잘 모를 경우에 유용함
- Decendants 축 메소드에는 두 가지 형태가 있음
    - 1. 매개변수로 xName을 받아 해당 XName을 가진 현재 개체 아래의 모든 개체를 반환
    - 2. 매개변수 없이 호출되어 XName과 관계없이 하위의 모든 개체를 반환해줌
- 이제는 앞에 있었던 XML을 재활용할 것
- 하나의 분류에 속한 모든 책들을 검색하는 대신 분류와 관계없이 모든 책들을 반환하고자 함
- 책 개체들이 XML의 여러 다른 계층에 존잭가능하므로 Elements를 사용하는 것은 불가능함
- 대신 당연히 Decendants 축 메소드를 이용하게 될 것임
- XML 내의 모든 책들을 가져오려면 다음과 같은 형태로 코드를 작성하면 됨
- [Decendants 메소드를 이용하여  XML 내의 모든 책들을 가져오기]
```C#
XElement books = XElement.Load("categorizedBooks.xml");
foreach (XElement bookElement in books.Descendants("book"))
{
    Console.WriteLine((string)bookElement);
}
```
- [결과]
- ![](cap1.PNG)
- Descendants 축 메소드는 자기 자신을 검색된 개체의 트리에 포함시키지 않는다는 것이 중요함
- 현재 개체를 포함시키고자 한다면 DesendatnsAndSElf 축 메소드를 사용하면 됨
- Descendants 축 메소드처럼 DescendantsAndSelf 메소드 또한 XElement객체들의 IEnumerable 컬렉션을 반환함
- 유일한 차이점: DescendantsAndSelf가 자기 자신을 반환하는 XElement 객체에 포함시킨다는 것
- [앞에서 사용해왔던 XML]
```XML
<category name="Technical">
  <category name=".NET">
    <books>
      <book>CLR via C#</book>
      <book>Essential .NET</book>
    </books>
  </category>
  <category name="Design">
    <books>
      <book>Refactoring</book>
      <book>Domain Driven Design</book>
      <book>Patterns of Enterprise Application Architecture</book>
    </books>
  </category>
  <books>
    <book>Extreme Programming Explained</book>
    <book>Programatic Unit Testing with C#</book>
    <book>Head First Design Patterns</book>
  </books>
</category>
```
- 이제 다음과 같은 코드를 바탕으로 Descendants와 DescendantsAndSelf 메소드를 비교해보자
- [Descendants와 DescendantsAndSelf 축 메소드의 비교]
```C#
XElement root = XElement.Load("categorizedBooks.xml");
IEnumerable<XElement> categories = root.Descendants("category");

Console.WriteLine("Descendants");
foreach (XElement categoryElement in categories)
{
    Console.WriteLine(" - "+(string)categoryElement.Attribute("name"));
}

categories = root.DescendantsAndSelf("category");
Console.WriteLine("DescendantsAndSelf");
foreach (XElement categoryElement in categories)
{
    Console.WriteLine(" - "+(string)categoryElement.Attribute("name"));
}
```
- [실행 결과]
- ![](cap3.PNG)
- 코드에서 알 수 있듯이 Descendants와 DescendantsAndSelf의 호출방법은 완전히 동일함
- 실행 결과를 살펴보면 DescendantsAndSElf는 상위 분류(Technical)을 출력에 포함시켰음을 알 수 있음

- Descendants와 DescendantsAndSelf를 이용해 간편하게 하나의 XML 트리내에서 관심있는 모든 개체들이 현재 노드 아래에 있을 경우, 하위 개체들을 손쉽게 가져올 수 있음
- XML에 대해 질의할 때, Element, Elements, Attribute, Descendants는 XML 트리내에서 관심 있는 개체와 속성들을 찾아내는 가장 핵심적인 축 메소드임을 뜻함

- Elements와 Descendants가 IEnumerable<XElement>를 반환해줌 -> 그 결과물은 표준 질의 연산자나 질의 표현식과 매우 잘 연동되어 사용 가능함
- [XML에 대해 질의하기 위해 LINQ 질의 표현식 문법을 이용하기]
```C#
XElement root = XElement.Load("categorizedBooks.xml");
var books = from book in root.Descendants("book")
            select (string)book;

foreach (string book in books)
{
    Console.WriteLine(book);
}
```
- [실행결과]
- ![](cap4.PNG)
- 여기서 확인 가능하듯이 Descendants 축 메소드를 이용하면 LINQ to XML을 통해 객체와 관계형 데이터에 대한 질의를 하는 것과 동일한 문법으로 XML 데이터에 대해 질의 가능함

### 10.1.5 Ancestors
- Ancestors 축 메소드는 탐색의 방향만 반대일 뿐 나머지는 모두 Descendants 메소드와 동일하게 동작함
- 동일한 메소드 시그너처를 가지고 있고 AncesotrsAndSelf, AncestorNodes와 같이 유사한 아류 메소드들을 갖고 있음
- Ancestors는 XML 트리상에서 현재 노드 위에 존재하는 개체들을 탐색함
- 지금까지는 어떻게 분류 개체내에 있는 책들의 목록을 Element와 Elements를 이용하여 가져오는지, 그리고 어떻게 XML 내의 모든 책들을 DEscendants로 가져오는지에 대해 살펴봄
- 이 절에서는 어떻게 Ancestors를 이용해 특정 책이 속한 모든 분류의 목록을 얻어낼 수 있는지 알아볼 것
- 분류는 다음과 같은 형태로 나타남
- `Domain Driven Design is in the: Technical/Design category.`

- 가장 먼저 해야할 일: 관심을 가지고 있는 책들을 선택하는 것
- XML에 Descendants 축 메소드를 이용해서 모든 책들을 가져와야 함
- 모든 책들을 가져오고나면 책의 목록에 대해 다음처럼 Where와 First같은 표준 질의 연산자를 이용하여 관심 있는 하나의 책으로 좁혀나감
```C#
XElement root = XElement.Load("categorizedBooks.xml");
XElement dddBook = root.Descendants("book")
                        .Where(book =>
                              (string)book == "Domain Driven Design"
                        ).First();
```
- Domain Driven Design이라는 책 개체를 골라냄
- 책 개체를 갖게 되면 Ancestors 축 메소드를 호출하여 책 개체가 속한 분류의 목록을 얻어올 수 있음
- 분류의 목록에 Reverse와 String.Join을 활용하여 처리해주면 원하는 규격으로 분류정보를 받아올 수 있음
```C#
XElement root = XElement.Load("categorizedBooks.xml");
XElement dddBook = root.Descendants("book")
                        .Where(book =>
                        (string)book == "Domain Driven Design"
                        ).First();

IEnumerable<XElement> ancestors = dddBook.Ancestors("category").Reverse();

string categoryPath =
        String.Join("/", ancestors.Select(e =>
        (string)e.Attribute("name")).ToArray());

Console.WriteLine((string)dddBook + " is in the: "+categoryPath+" category.");
```
- [결과]
- ![](cap5.PNG)

### 10.1.6 ElementsAfterSelf, NodesAfterSelf, ElementsBeforeSelf, NodesBeforeSelf
- ElementsAfterSelf, ElementsBeforeSelf, NodesAfterSelf, NodesBEforeSelf 메소드는 현재 개체의 앞뒤에 존재하는 모든 개체를 받아올 수 있는 매우 쉬운 방법을 제공
- ElementsBeforeSelf: 현재 개체보다 앞에 있는 모든 XElement 개체 반환
- ElementsAfterSelf: 현재 개체보다 뒤에 있는 모든 XElement 개체 반환
- 유사하게 개체만 받아오지 않고 모든 노드를 받아오고 싶다면 NodesBeforeSelf와 NodesAfterSelf 메소드를 사용 가능함
- **중요한 점:**
    - Ancestors나 Descendants 축 메소드랑 달리 ElementsBeforeSelf, ElementsAfterSelf, NodesBeforeSelf, NodesAfterSelf 메소드들은 현재 개체와 동일한 계층에 있는 개체들만 탐색한다는 것임
- [특정 개체와 동일한 계층에 있는 모든 개체 노드들을 ElementsBEforeSelf로 찾아내기]
```C#
XElement root = XElement.Load("categorizedBooks.xml");
XElement Book = root.Descendants("book")
                    .Where(book =>
                           (string)book == "Domain Driven Design"
                           ).First();

IEnumerable<XElement> beforeSelf = Book.ElementsBeforeSelf();
foreach (XElement element in beforeSelf)
{
    Console.WriteLine((string) element);
}
```
- [실행결과]
- ![](cap6.PNG)

- 출력된 결과로 알 수 있듯이 ElementsBeforeSElf는 현재 노드와 같은 계층에 있는 개체들들로 탐색의 범위가 제한됨
- Ancestor나 Descendants 축 메소드와 같이 계층간을 오가며 탐색하지 않는다는 뜻임

## 10.2 표준 질의 연산자
### 10.2.1 Select로 사영하기

### 10.2.2 Where로 필터링하기

### 10.2.3 정렬과 그룹화

## 10.3 XPath를 이용하여 LINQ to XML 객체에 대해 질의하기

## 10.4 XML 변환하기

### 10.4.1 LINQ to XML 변환

### 10.4.2 LINQ to XML 객체를 XSLT를 이용하여 변환하기