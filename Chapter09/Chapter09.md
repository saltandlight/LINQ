# Chapter09. LINQ to XML의 소개
- LINQ to XML은 LINQ가 제공하는 강력한 질의기능을 XML 데이터에 대해서도 사용 가능하게 함
- 개발자들에게 새로운 XML 프로그래밍 API를 제공해줌
- LINQ to XML API를 잘 다루려면 LINQ to XML이 설계당시에 어떤 목적과 사상에 바탕을 두고 있었는지 자세히 알아볼 필요가 있음
- 이 장에서는 설계상의 원칙이나 LINQ to XML의 핵심을 구성하는 개념들을 소개함
- MS사가 왜 LINQ to XML이라는 개념을 설계하고 제품화하게 되었는지를 완벽하게 이해하고 공감할 수 있게 된 다음에 LINQ to XML의 클래스 구조를 살펴볼 것
- 클래스 구조를 살펴보면서, LINQ to XML API에서 사용하는 몇몇 주요 생성자 및 메소드들에 대해 알아봄
- LINQ to XML이 제공하는 클래스들의 구조에 대해 이해하고 난 후에는 XML을 이용하는 애플리케이션을 작성하면서 LINQ를 사용하여 읽기, 해석하기, 수정하기, 삭제하기, 저장하기 등의 조작을 XML에 가하는 방법에 대해 알아봄

## 9.1. XML API란 무엇인가?
- XML API는 개발자들에게 XML 데이터를 다룰 수 이는 프로그래밍 인터페이스를 제공함
- XML API를 사용함 -> XML을 사용하는 애플리케이션 작성 가능
- 다음의 예시를 통해 그러한 API를 왜 필요로 하는 지 살펴보자
- [웹 사이트 주소를 담고 있는 예시 XML 파일]
```XML
<links>
    <link>
        <url>http://linqaction.net</url>
        <name>LINQ in Action</name>
    </link>
    <link>
        <url>http://hookedonlinq.com</url>
        <name>Hooked on LINQ</name>
    </link>
    <link>
        <url>http://msdn.micorosoft.com/data/linq/</url>
        <name>The LINQ Project</name>
    </link>
</links>
```
- XML을 다루기 위해서라면 API를 이용하는 것이 나음
- 만약 XML을 매우 원시적인 수준에서 다루고 싶다면 XmlTextReader 클래스를 이용하는 것이 좋음
- 만일 매우 큰 문서를 다룬다면 XmlReader와 같은 스트리밍 API를 이용하는 것이 좋음
- 만약 XML을 자유롭게 훑어보고 트리구조를 탐색하고 싶다면 XmlNode 클래스를 사용하거나 XPath 표현식을 이용하여 XML 트리구조를 탐색할 수 있게 해주는 XPathNavigator를 통해 DOM을 이용하는 것이 좋음
- 각각의 API는 고유의 장단점을 가지고 있음
- 그러나.. 이런 API들이 가진 공통점: 개발자들이 그들의 애플리케이션에서 XML을 활용하기 쉽게 해준다는 점
- 왜 또다른 새로운 XML 프로그래밍 API를 필요로 했는지 알아보기

## 9.2. 왜 XML을 처리하는 새로운 방법이 필요한가?
- 현재 존재하는 API들을 활용시 개발자들은 상당히 많은 것들을 염두에 두어야 함
    - 어떤 것을 사용할 지 선택, 각각의 API들의 미묘한 차이점 이해, 완전히 다른 관점에서 설계되고 구현된 모델들에 대해 배워야 함
- 대다수의 개발자들에게 XML이라는 기술이 가진 넓은 범위와 깊이, XML을 사용하면서 해야하는 수많은 기술적 선택들은 엄청난 부담이 됨

- LINQ to XML은 평이한 많은 개발자들이 간단하고 강력하게 사용 가능한 새롱누 XML 프로그래밍 API를 제공함으로 이런 문제를 해결하려고 했음
- LINQ는 XQuery와 XPath가 가진 질의 및 변형의 강력함을 .NET 프로그램이 언어 속에 잘 녹아들게 했음
- 메모리 내의 데이터 접근 API 또한 제공하는 형태로 XML 데이터를 매우 일관적이고 예측 가능한 형태로 다룰 수 있게 했음
- nullable형이나 함수형 생성기능은 매우 보편적으로 이용됨
- LINQ는 확장 메소드 및 익명형, 람다 표현식 등으로 스스로 언어적 진보를 이루고 있음

- LINQ to XML의 핵심 설계원칙들을 살펴보면서 기존의 .NET XML API들과 LINQ 가 어떤 차이를 갖고 있는지 이해해보기

## 9.3. LINQ to XML의 설계철학
- 일반적인 XML 프로그래머들이 XML에 의한 작업을 더 생성적이고 덜 지루하게 할 수 있도록 MS사는 LINQ to XML의 설계에 완전 새로운 접근방법을 택함
- LINQ는 개념적으로나 실제 성능 및 메모리 사용 측면에서 매우 경량의 플랫폼으로 기획되었음
- LINQ to XML 데이터 모델은 W3C Information Set을 매우 신중하게 참고하여 설계됨

- 논의할 설계철학이 XML을 다룰 떄 어떤 차이를 가져올지 확실하게 평가하기 위해서 현존하는 가장 유명한 XML 프로그래밍 ZPI인 DOM을 이용하여 간단한 XML 문서를 만든 후에 동일한 작업을 LINQ to XML을 이용하여 해보겠음
- 이 예제의 목표: LINQBooks에 포함되어 있는 책에 관한 상세 정보드릉ㄹ 추출해서 XML 문서로 만드는 것
- [누군가의 서가에 있는 책 중 가장 중요한 책]
```xml
<books>
    <title>LINQ in Action</title>
    <author>Fabrice Marguerie</author>
    <author>Steve Eichert</author>
    <author>Jim Wooley</author>
    <publisher>Manning</publisher>
</books>
``` 
- 이 문서를 위해 작성해야 하는 코드는 다음과 같음
- [DOM을 이용하여 XML 문서 만들기]
```C#
XmlDocumnet doc = new XmlDocument();
XmlElement books = doc.CreateElement("books");
XmlElement author1 = doc.CreateElement("author");
author1.InnerText = "Fabrice Marguerie";
XmlElement author2 = doc.CreateElement("author");
author2.InnerText = "Steve Eichert";
XmlElement author3 = doc.CreateElement("author");
author3.InnerText = "Jim Wooley";
XmlElement title = doc.CreateElement("title");
title.Innertext = "LINQ in Action";
XmlElement book = doc.CreateElement("book");
book.AppendChild(author1);
book.AppendChild(author2);
book.AppendChild(author3);
book.AppendChild(title);
books.AppendChild(book);
doc.AppendChild(books);
```
- DOM을 이용해서 XML 문서를 만들려면 강제로 명령형 생성 모델을 이용하도로고 되어 있음
- 명령형 생성 모델의 결과물은 만들어지는 XML과 전혀 다른 모습을 하고 있음
- XML과 같은 계층구조를 갖지 않고 모든 코드는 하나의 계층을 갖는 매우 밋밋한 형태로 되어 있음 
- 추가적으로, 매우 많은 임시 변수들을 만들어서 우리가 작성하는 개체들을 가지고 있어야 함
- 이런 특성상 코드의 가독성은 매우 떨어지며 디버깅이나 유지 보수가 어려움
- 작성한 코드의 구조가 궁극적으로 만들고 싶은 XML 문서의 구조와 매우 많이 다름
- LINQ to XML을 이용하여 작성하는 경우와의 차이점 알아보기
- [LINQ to XML을 이용하여 XML 문서를 생성하기]
```C#
new XElement("books",
  new XElement("book",
    new XElement("author", "Fabrice Marguerie"),
    new XElement("author", "Steve Eichert"),
    new XElement("author", "Jim Wooley"),
    new XElement("title", "LINQ in Action"),
    new XElement("publisher", "Manning")
  )
);
```
- 부모 문서의 컨텍스트에 맞게 개체를 생성하는 것에 고민하지 않아도 됨
- XML 최종문서와 매우 유사한 형태의 구조를 이용하여 XML을 작성 가능함

### 9.3.1. 핵심개념: 함수형 생성
- LINQ to XML은 **함수형 생성**이라는 XML 개체들을 생성하는 매우 강력한 접근방법을 제공함
- 이런 함수형 생성에 의해 완전한 XML 트리를 하나의 문장으로 작성 가능
- 명령형으로 각 노드마다 임시변수를 이용하여 XML 문서를 작성하는 방법보다 좀 더 함수형의 접근방법을 사용해서 결과 XML과 매우 유사한 형태로 코드 작성 가능함.
- 함수형 생성의 목적: 개발자들이 자신이 생각한 바대로 XML을 구현할 수 있게 해주는 데 있음

### 9.3.2. 핵심개념: 컨텍스트에서 자유로운 XML 생성
- DOM을 이용하여 XML 생성 시 모든 것은 부모가되는 문서의 컨텍스트 하에서 진행되어야 함
- 이런 방법은 가독성이 떨어지고 디버깅이 어려운 코드를 가져올 수 밖에...
- LINQ to XML을 이용하면 개체와 속성들은 종속된 개체로서 뿐만 아니라 독립된 개체로서 대우받을 수 있음
- 개체나 속성은 문서나 부모 개체의 컨텍스트와 무관한 상태에서 생성 가능, 이런 특성을 바탕으로 개발자들은 자유로운 방법으로 XML 활용 가능
- 정형의 함수들을 통해 개체와 속성들을 생성하기보다 XElement 또는 XAttribute 클래스가 제공하는 조합 생성자들을 이용할 수 있게 됨

- 코드 작성이 더 쉬워짐!
- 문서들은 LINQ to XML에서 예전의 주도적 위치를 상실했지만, 그들의 역할과 위치는 남아있음
- XML 선언, 문서형태 정의, XML 처리법 정의 등을 포함하는 완전한 형태의 XML 문서를 작성하는 경우에 대비해서 LINQ to XML은 XDocument라는 클래스를 지원하고 있음

### 9.3.3. 핵심개념: 간소화된 명명법

## 9.4. LINQ to XML 클래스 계층구조

## 9.5. LINQ를 이용하여 XML을 다루기
### 9.5.1 XML을 읽어들이기
### 9.5.2. XML을 해석하기
### 9.5.3. XML을 생성하기 
### 9.5.4. Visual Basic의 XML 리터럴을 이용하여 해석하기
### 9.5.5. XML 문서를 생성하기 
### 9.5.6. XML에 내용을 추가하기
### 9.5.7. XML에서 내용을 삭제하기
### 9.5.8. XML의 내용을 수정하기
### 9.5.9. 속성을 가지고 작업하기
### 9.5.10. XML을 저장하기
