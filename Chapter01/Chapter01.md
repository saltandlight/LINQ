# Chapter01. LINQ의 소개🦝

## 1.1 What is LINQ?
- .NET 프로그래밍 언어를 통해 데이터베이스를 이용하는 과정에서 발생될 수 있는 개념적, 기술적 어려움을 극복하기 위해 개발됨
- MS 사는 LINQ를 통해 객체-관계 매핑의 한계를 극복하고 객체와 DB 간의 소통을 편하게 하고자 하는 목표를 가졌음
- But 이보다 더 나아간 LINQ
- LINQ는 관계형 DB뿐만 아니라 메모리 내부의 객체나 XML 문서, 플랫 파일과 같은 다양한 데이터 저장소의 데이터에 대해 질의하는 용도로 사용 가능함
- 약간 먼치킨같은 존재

### 1.1.1 개론
- LINQ는 데이터 저장소에 관계없이 자유롭게 사용 간으한 통합된 데이터 접근방법을 제공함
- LINQ는 관계형 데이터베이스에서 보편적으로 사용되는 SQL 구문처럼 데이터에 대한 질의 및 집합연산에도 대응함
- .NET 언어의 확장 형태를 보이며 코드 내에 질의를 바로 내장시킬 수 있도록 했음(이것이 특징)
- LINQ = LanguageINtegrated Query의 약어(`언어 내장형 질의`)

- LINQ는 다른 여러 세계를 결합하는 접착제 역할을 함(장애물을 제거해줌)
- LINQ의 주된 장점: 
    - 어떤 형태나 어떤 저장소에 있는 데이터와 사용하더라도 자연스럽게 일관된 프로그래밍 모델을 제공할 수 있도록 설계됨
    - **LINQ가 데이텨의 형을 엄격하게 지킨다.**
        - LINQ가 애플리케이션이 컴파일되는 시점에 문법적인 오류를 체크할 수 있다는 점
        - Visual Studio의 IntelliSense(자동완성 및 제안) 기능을 활용 가능함

- LINQ의 구문과 개념은 매우 보편적 -> 배열이나 컬렉션같은 메모리 내의 객체 내 데이터에 LINQ를 사용하는 방법 안다면
                                    관계형 DB나 XML 같은 다른 매체에서도 LINQ를 사용하는 방법을 안다는 것과 같은 의미

### 1.1.2 도구로서의 LINQ
- 세 가지 형태의 LINQ provider: 
    - LINQ to Object
    - LINQ to SQL
    - LINQ to XML
- 세 가지 LINQ provider는 독립적으로 단독으로 사용될 수 있고, 복합적으로 사용될 때는 더 강력한 솔루션 제공 가능함
- 이 프로바이더들은 하나의 공통된 LINQ foundation에서 파생된 변종들
- LINQ foundation: 질의 연산자, 질의 표현식, 표현식 트리같은 기초적인 시발점을 제공 
-> LINQ toolset은 더욱 자유로운 확장성 가질 수 있게 됨.
- 어떤 데이터 출처나 API도 LINQ와 결합 가능함

### 1.1.3 언어 확장으로서의 LINQ
- LINQ는 규격화된 질의문법을 통해 다양한 종류의 데이터 저장소에 접근 가능하게 해줌
- LINQ처럼 선언적인 접근을 하는 경우는 코드가 간결해짐~!
[LINQ를 사용한 예시]
```C#
var contacts =
    from customer in db.Customers
    where customer.Name.StartsWith("A") && customer.Orders.Count > 0
    orderby customer.Name
    select new { customer.Name, customer.Phone };

var xml =  new XElement("contacts",
               from contact in contacts
                select new XElement("contact",
                new XAttribute("name", contact.Name),
                new XAttribute("phone", contact.Phone)
                )
            );
```
- LINQ를 사용하지 않을 떄보다 훨씬~~!! 간결하다
- LINQ는 .NET 언어가 SQL, XSL처럼 저장소에 따라 바뀌는 다양한 형식의 언어로 인해 복잡하게 얽히는 것을 막아줌

## 1.2 왜 LINQ가 필요한가?
- 예전에는 SQL이나 DB를 프로그래밍 언어와 이어주는 API 역시 습득해야 완전한 애플리케이션을 작성 가능했음
(마치 DB와 java 사이에 jsp, php를 사용하는 것처럼)
- .NET API 이용해서 데이터 액세스하는 코드의 일부분을 찾아볼 것임 
    -> 문제점을 발견할 수 있을 것(DB와 프로그래밍 언어의 구조상의 불일치)
### 1.2.1 일반적인 문제점
### 1.2.2 패러다임 간의 불일치에 대한 고찰
### 1.2.3 해결사로서의 LINQ
  


## LINQ to Objects LINQ to XML, LINQ to SQL과의 첫 만남