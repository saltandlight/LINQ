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
- 애플리케이션에서 DB에 접근할 일이 많기 떄문에 .NET 프레임워크가 저장된 데이터에 접근하기 쉬운 API를 내장할 것이 요구됨
- .NET 프레임워크 클래스 라이브러리(FCL)은 ADO.NET 이라는 관계형 데이터에 접근, 관계형 데이터를 메모리내에서 나타내는 API를 통해 해법을 제시해왔음
- API는 SqlConnection, SqlCommand, SqlReader, DataSet, DataTable 과 같은 클래스 등으로 구성됨
- 문제점: 개발자들이 테이블이나 레코드, 열과같은 관계형 모델의 개념으로서만 메서드를 작성할 수 밖에 없음

- 객체지향 패러다임이 SW 개발의 대세로 자리잡고 있음 -> 개발자는 관계형 DB나 XML처럼 다른 형태로 추상화된 데이터를 객체 모델로 매핑하기 위해 많은 비효율과 비용이 듬
- LINQ가 목표하던  바: 그러한 압박으로부터 개발자들을 해방시키는 것
[예제 1.2: 지금까지의 .NET의 데이터 접근 코드]
```C#
using (SqlConnection connection = new SqlConnection("..."))
{
    connection.Open();
    SqlCommand command = connection.CreateCommand();
    command.CommandText = 
    @"SELECT Name, Country
      FROM Customers
      WHERE City = @City";
    command.Parameters.AddWithValue("@City", "Paris");
    using(SqlReader reader = command.ExecuteReader())
    {
        while(reader.Read())
        {
            string name = reader.GetString(0);
            string country = reader.GetString(1);
            ...
        }
    }
}
```
- 코드의 한계점:
    - 몇 가지 과정은 필수 -> 불필요하게 코드가 길어짐
    
    - 질의가 따옴표 속의 문자열로 표현됨 -> 컴파일 시 검증이 안 됨
    
    - while 문 안의 내용이 너무 느슨하게 정의되어 있음(열이 예상하는 형으로 되어있을까? 올바른 개수의 매개변수를 사용한다고 할 수 있을까? 매개변수의 이름이 확실한가?)
    
    - 사용하는 클래스들이 SQL Server와의 소통에만 사용 가능, 다른 데이터베이스 서버와는 사용하기 힘듬
    
     -> DbConnection 을 사용해서 해결함
     -> 절반만 해결된 상황... 
    
     -> SQL은 배포판이나 판매하는 회사에 따라 언어에 추가된 사항이 다르고 데이텨형에도 차이가 있음 
      => 어떤 DB를 위해 작성된 코드가 다른 종류의 DB에서는 
      잘 안 돌아갈 수 있음
- LINQ의 도입의 두 가지 측면:
    - MS사가 데이터 매핑 솔루션을 제공하지 못했던 상황에서 LINQ가 질의를 프로그래밍 언어에 내장시킬 수 있는 좋은 방법으로 인식됨
    - 예제코드 1.2의 한계점을 상당 부분 극복해냄
    - **중요한 점:**
      
        - LINQ 사용 시, 다음의 문법으로 어떤 종류의 데이터 저장소에 대한 질의도 본인이 늘 사용하여 익숙하게 다룰 수 있는 프로그래밍 언어 내에서 수행 가능함.
        ```C#
        from customer in customers
        where customer.Name.StartsWith("A") && customer.Orders.Count>0
        orderby customer.Name
        select new {customer.Name, customer.Orders}
        ```
    - 데이터는 메모리 내에서 존재 가능함, DB나 XML 문서 속에 있을 수도 있음
    - But... 저장소의 위치와 형태에 무관하게 질의의 문법은 전혀 바뀌지 않음

### 1.2.2 패러다임 간의 불일치에 대한 고찰
- 보통 개발자들에게 요구되는 능력:
    - 기술이나 언어들을 독립적으로 다루고 적용할 수 있는 능력
    - 기술과 언어를 요구에 맞게 조합하여 잘 맞물려 동작하는 결과물을 만들어낼 수 있는 능력
- 그런데... DB 모델, XML, OOP 는 상호동작을 고려하지 않고 설계됨<br>
    -> 함께 잘 동작하지 않음
    - 서로 잘 호환되지 않는 다른 패러다임을 대표함
#### 사람들이 말하는 "상호불일치(Impedance Mismatch)"란?
- 데이터는 C#, VB.NET, Java, Delphi, C++ 등의 객체지향 언어로 작성된 애플리케이션에 의해 다뤄짐
- 그러나... 객체의 상관관계를 DB의 레코드로 나타내는 것은 짜증나고 지루한 코딩을 요구할 때가 있음

- MS 사는 LINQ가 해결해주는 문제들을 `"데이터 != 객체"`라고 설명함 
    - LINQ to SQL: `"관계형 데이터 != 객체"`
    - LINQ to XML: `"XML 데이터 != 객체"`
    - 결론: `"XML 데이터 != 객체"`

- 상호불일치:
    - 시스템 간의 호환성의 결여나 어떤 시스템이 다른 시스템으로부터 데이터를 받아오지 못하는 경우를 묘사하기 위해 사용됨

#### 객체-관계 매핑
- 객체지향 패러다임과 관계형 패러다임을 봤을 때, 두 패러다임 간 상호불일치는 여러 계층에서 존재함

##### 관계형 데이터베이스와 객체지향 언어는 동일한 집합의 기본형을 지원하지 않는다.
- DB에서 문자열은 특정한 길이 갖지만 C#이나 VB.NET에서는 길이가 자유로움 
- 프로그래밍 언어에서는 Boolean 값을 흔히 이용하는 반면 대부분의 DB는 Boolean 형을 기본으로 지원하지 않음

##### OOP와 관계형 이론들은 각각 다른 데이터 모델을 갖고 있음
- 내제적인 구조상의 이유와 성능상의 이유로 RDBMS는 보통 일반화되어 있음
- 일반화는 중복을 제거, 데이터를 효율적으로 정렬, 예기치 못한 데이터 무결성을 방지해주는 역할함
- 일반화는 관계형 데이터 모델에 맞게 데이터를 조정해줌
- 테이블과 레코드가 객체나 컬렉션과 곧바로 매핑되는 것을 방지함
[여기서부터 계속 해야함]

##### 프로그래밍 모델상
- SQL에서는 질의를 작성함 -> 필요로 하는 데이터를 명시하는 고급의 선언적 방법을 가지고 있음
- C#과 VB.NET은 명령형 프로그래밍 언어에서는 반복문과 조건문을 잘 활용해야 함

##### 캡슐화 
- 객체들은 잘 짜여진 컨테이너 안에 담겨 있음
- 데이터와 행동을 모두 포함하고 있음 
- DB에서 데이터 레코드는 단순한 데이터에 불과할 뿐 행동에 관한 정보를 포함하지 않음
- -> SQL질의나 저장된 프로시저만으로는 DB 레코드의 행동을 제어할 수 없음
- 관계형 DB에서 코드와 데이터는 명확히 분리되어 있음

- 이런 불일치는 관계형 DB와 일반적인 객체지향 클래스 계층구조 간의 차이에서 나옴 

- 상속 or 조합같은 개념은 관계형 DB에서 곧바로 지원되는 것은 아님
- 두 가지 모델에서 공통된 방법으로 데이터를 표현할 수 없음

- 객체 모델을 새로운 관계형 DB에 저장하려고 해도 직접적 매핑을 사용하기애는 어려움
- 성능상의 문제와 중복의 문제가 있음<br>
 -> 하나의 테이블만 DB에 만드는 것이 훨씬 나을 것...! 그러나 그런 경우, DB 테이블에서 가져온 데이터를 이용하여 쉽게 메모리 내에 다시 객체 그래프를 만들어내기 어렵다는 단점이 있음 

 - 두 모델 간의 상호불일치는 DB 스키마나 객체 모델을 잘 설계함으로써 어느 정도는 해소할 수 있을 것
- 그러나 두 패러다임 간의 태생적인 차이 때문에 차이점을 완전히 극복하기는 힘듬
- DB와 프로그램을 잘 융합시키는 것은 단순히 데이터 저장소에서 데이터를 읽고 쓰는 것 이상의 매우 복잡한 문제
- 객체지향 언어를 이용하여 프로그래밍할 경우, 애플리케이션 내부의 로직들이 비즈니스 영역에서 정의된 객체 모델을 따르는 것을 원함, 관계형 구조를 따르기 원하지 않음
- 어느 시점에서는 두 모델이 잘 연동될 수 있도록 해야함

- **객체-관계 매핑**:
    - 객체지향 언어와 관계형 데이터베이스를 잘 이어주는 일반적인 해법
    - 양방향 매핑을 의미함
    - 매핑: 객체와 그들간의 관계가 관계형 DB의 같은 저장소에 저장될 수 있도록 지정해주는 행위라고 정의 가능함

 - DB는 자연적으로 객체 모델과 매핑되지는 않음
 - 객체-관계 매핑 도구는 언어 간의 불일치를 해결해주는 자동화 도구
 - -> 객체-관계 매핑 도구에 클래스, DB, 매핑 설정 파일만 제공해주면 매핑 도구가 알아서 매핑을 해줌
 - 알아서 SQL 질의 생성, 객체를 DB에서 가져온 데이터로 채워주기도 함 
 - 반대로 DB에 객체의 내용을 저장해주기도 하는 등 대부분의 지루하고 복잡한 일들을 매핑 도구가 대신해줌   

 - 그러나 당연히 매핑 도구들도 몇 가지 기능상의 제약을 안고 있음
    - 도구에 대해 충분히 배워야 함
    - 최적화된 활용을 위해서 여전히 관계형 DB에 대한 해박한 이해가 필요함
    - 매핑 도구는 항상 효율적이지만은 않음
    - 모든 도구가 컴파일시 체크를 지원해주는 것은 아님
#### Object-XML 매핑
- 객체와 XML 간에도 상호불일치가 존재함
- XML 스키마 정의에 포함되어 있는 형(type) 시스템은 .NET 프레임워크의 형 시스템과 일대일 대응의 관계에 있지 않음
- But... System.Xml에 이를 다루는 API들이 있고 객체의 직렬화를 지원하는 기능이 포함되어 있음<br>
-> .NET 애플리케이션에서 XML을 사용하는 것은 큰 문제가 아님

- But... 여전히 매우 간단한 작업 시에도 아주 지루한 코드를 작성할 경우가 있음
- 각 영역 간의 갭의 예시
    - 관계형 DB는 관계대수로 표현되어 있음, 테이블, 행, 열, SQL을 다룸
    - XML은 문서, 개체, 속성, 계층구조, XPath 에 관한 것들임
    - 객체지향 범용언어들과 .NET은 클래스, 메소드, 프로퍼티, 상속, 반복문 등의 세계에서 놀고 있음
 - 많은 개념들이 다른 영역으로의 직관적인 매핑을 제공하지 않음
 - C# 과 VB.NET 같은 .NET 기반의 언어는 개발자들이 IntelliSense, 엄격한 형의 코드, 컴파일시의 체크 등 여러 가지 유용한 기능을 사용가능하게 함
 - 이런 기능들은 컴파일러에 의해 체크되지 않는 SQL문이나 XML 조각들을 실수로 코드내에 끼워넣을 때 쉽게 오동작 일으킬 수 있음

- 이런 문제의 완벽한 해결을 위해 서로 다른 기술 간의 갭을 해소하고 메모리내 객체와 저장매체 간의 상호불일치를 해결할 필요가 있음
- 이러한 문제를 해결하기 위해 .NET과 다른 데이터 저장소 개체 간의 문제를 해결해야 함

    - 근본적인 기술 간의 차이
    - 여러 종류의 데이터 저장소가 가능한 일과 가능하지 않은 일의 차이
    - 각각의 기술에 대한 소유권과 기술적 권고사항의 차이
    - 다른 모델링과 설계원칙

### 1.2.3 해결사로서의 LINQ
- 메모리내 객체와 관계형 DB의 데이터를 함께 잘 혼용하기 위해 두 가지 데이터 저장소의 특성과 차이 및 패러다임을 이해하고 나서 그 지식을 바탕으로 적절한 선택을 해야 함
- LINQ와 LINQ to SQL을 이용하면 그 한계와 선택에 대한 걱정을 줄일 수 있음
- 상호불일치는 둘 중 어느 한 쪽을 "primary"한 것으로 선택해야 하는 과정을 요구함
- MS사는 LINQ를 사용하여 프로그래밍 언어쪽을 선택함
    - 그들 입장에서는 C#이나 VB.NET 언어의 개념을 변화시키는 것이 더 쉬웠기 때문!
- LINQ의 주된 목표: 데이터 질의와 조작능력을 프로그래밍 언어에 부여하는 것

- LINQ는 객체와 DB, XML 간에 존재하는 많은 장벽을 허뭄
- 각각의 패러다임을 동일한 언어 내장형 기능을 이용하여 작업 가능함<br>


[하나의 질의 속에서 관계형 데이터와 XML을 동시에 다루기]
```C#
var database = new RssDB("server=.; initial catalog=RssDB");

XElement rss = new XElement("rss",
  new XAttribute("version", "2.0"),
  new XElement("channel",
    new XElement("title", "LINQ in Action RSS Feed"),
    new XElement("link", "http://LinqInAcation.net"),
    new XElement("description","The RSS feed for this book"),
    from post in database.Posts
    orderby post.CreationDate descending
    select new XElement("item",
      new XElement("title", post.Title),
      new XElement("link", "posts.aspx?id="+post.ID),
      new XElement("description", post.Description),
      from category in post.Categories
      select new XElement("category", category.Description)
    )
  )
);
```
## 1.3 LINQ의 기원과 설계상의 목표
- MS사가 LINQ를 통해서 얻고자 했던 것은 무엇이었나?
- LINQ 프로젝트의 설계상 목표가 무엇인지 알아보는 것부터 시작!
- LINQ가 MS사의 다른 프로젝트들과 어떤 관계를 갖고 있는지를 살펴본 후, 
LINQ가 C#이나 ObjectSpaces, WinFS, XQuery 지원 등과 어떤 상호 관계를 가지는 지 살펴보자

### 1.3.1 LINQ 프로젝트의 목표
- LINQ의 기능 중 최고는 다음과 같은 여러 가지 종류의 데이터형과 저장소에 대응할 수 있는 능력

[LINQ의 설계상 목표와 동기]

| 목표                                                        | 동기                                                         |
| ----------------------------------------------------------- | ------------------------------------------------------------ |
| 객체나 관계형 데이터, XML의 통합                            | 데이터 저장소와 프로그래밍 언어에 구애받지 않고 통합된 질의 문법을 제공함, 저장소나 메모리 내의 표현형태와 무관한 단일 데이터 처리 모델을 확립함 |
| C#과 VB.NET 에서 사용 가능한 SQL이나 XQuery와 유사한 강력함 | 프로그래밍 언어에 바로 질의기능을 내장함                     |
| 언어를 위한 확장 모델                                       | 다른 언어에서도 구현할 수 있도록 확장함                      |
| 형 안정성( type safety)                                     | 컴파일 시의 형(type)에 대한 확인을 바탕으로 런타임에서만 발견되던 질의에 관련된 버그들을 조기에 탐지함 |
| 광폭적인 IntelliSense 지원(엄격한 형 구조 유지)             | 개발자들이 더 나은 생산성으로 질의를 작성하고 새로운 문법에 쉽게 적응할 수 있도록 해줌 |
| 디버거 지원                                                 | 개발자들이 풍부한 디버깅 정보로 차근차근 LINQ 질의를 디버그할 수 있게 해줌 |
| C# 1.0, C# 2.0, VB.NET 7.0, VB.NET 8.0의 기초에 기반한 발전 | 언어의 이전 버전에서 구현된 기능을 그대로 활용가능하게 해줌  |
| .NET 2.0 CLR에서의 복귀                                     | 새로운 런타임을 사용해서 불필요한 배포상의 문제점을 야기시키지 않음 |
| 100% 하위 호환성의 유지                                     | 표준과 제네릭 컬렉션을 사용 가능함, 데이터 바인딩이나 현재의 표현계층을 그대로 활용가능하게 함 |

- LINQ는 처음부터 일반적인 객체 컬렉션, DB, 개체, XML 등에 대해 자유롭게 질의할 수 있는 기능을 내장하고 있음
- 이와 함께  LINQ는 강력한 확장 기능을 제공함 -> 원한다면 쉽게 다른 데이터 저장소나 프로바이더와 연동하여 사용 가능함
- LINQ의 중요한 기능 특징: 엄격한 형(type)을 유지함
- 형을 유지할 경우의 장점:
  - 컴파일 시 질의에 대해 문법적, 논리적 체크가 가능해짐
    - 컴파일 시에 작성한 코드에 오류가 없는지 확인 가능함
    - 직접적 혜택: 개발과정에서 발생 가능한 예상치 못한 오류를 줄일 수 있음
- LINQ 질의를 작성 시 Visual Studio에서 제공하는 강력한 IntelliSense 기능을 이용 가능함.
  - 간단하거나 복잡한 컬렉션 또는 데이터 객체 모델을 직관적으로 다룰 수 있게 해줌

### 1.3.2 역사에 관한 설명
- 프로그래밍 언어의 진화 및 데이터 접근방법의 개선 등을 목표로 한 프로젝트 
-> LINQ to Objects, LINQ to XML, LINQ to SQL의 기반이 되는 중요한 연구들이었음

#### Cw(C-오메가 언어)
- Cw는 C#을 다양한 방면으로 확장하기 위해 MS 리서치가 추진한 프로젝트였음
- Cw 프로젝트의 목적:  
    - 비동기적인 전역 동기화를 위한 제어 흐름의 확장
    - XML과 DB 조작을 쉽게 하는 데이터형의 확장
- 현재 LINQ 기술이라고 총칭하는 것들의 대부분은 Cw에 포함되어 있던 것들임
- Cw 프로젝트는 언어 내장형 질의, C#과 SQL의 조합, C#과 XQuery 의 조합 등에 대한 연구로 진행됨

- Cw는 프리뷰의 형태로 공개되었음
- Cw가 .NET과 C#의 형 시스템을 확장하려 한 시도는 SQL 형태의 질의와 질의 결과 집합, XML 내용을 언어의 일원으로 아우르려는 첫 번재 시도였음
- Cw는 .NET 2.0 의 System.Collections.Generic.IEnumerable<T>와 비견되는 스트림형(stream type)을 도입함
- Cw가 지원한 다른 한 부분은 VB.NET에서 볼 수 있는 내장된 형태의 XML임

#### ObjectSpaces
- 사실 LINQ to SQL은 MS사가 객체-관계 매핑 연구로서 최초로 시도한 것은 아님
- ObjectSpaces라는 프로젝트가 있었음
- ObjectSpaces는 데이터 접근 API들의 집합, 데이터 저장소의 종류 및 위치와 무관하게 데이터를 객체로 다루는 기능이 있었음
    - OPath라는 객체 질의 언어를 지원하고 있었음

#### XQuery 구현
- ObjectSpaces와 같은 시기, MS사는 XQuery 처리기에 대한 작업에 착수
- XQuery의 한 가지 문제점: 사용자가 XML 만을 다루기 위해 또 다른 언어를 배워야 함

- 왜 MS사는 이런 시도들을 중단했는가?
    - LINQ로 질의가 불가능한 것이 거의 없기 떄문에 ObjectSpaces나 XQuery를 클라이언트에 지원할 이유가 사라짐

## 1.4  LINQ to Objects의 첫걸음: 메모리 내의 컬렉션 객체에 대한 질의
### 1.4.1 코드를 작성하기에 앞서 알아두어야 할 사항
**컴파일러 및 .NET 프레임워크 지원과 필요한 소프트웨어**
- 이 버전 중 하나의 Visual Studio를 설치해야 함
    - Visual C# 2008 Express Edition
    - Visual Basic 2008 Express Edition
    - Visual Web Developer 2008 Express Edition
    - Visual Studio 2008 Standard Edition 또는 그 이상
- LINQ to SQL 예제들을 돌려보려면 다음 중 하나를 사용 가능해야 함
    - SQL Server 2005 Express Edition 또는 SQL Server 2005 Compact Edition
    - SQL Server 2005
    - SQL Server 2000a
    - 이보다 높은 버전의 SQL Server

**언어에 대한 이해**
- C# 프로그래밍 언어의 문법을 알고 있다면 충분하오

### 1.4.2 Hello LINQ to Objects
[예제: C#으로 작성한 Hello World]
```C#
using System;
using System.Linq;

static class HelloWorld
{
    static void Main()
    {
        string[] words = 
        {"hello", "wonderful", "linq", "beautiful", "world" };

        var shortWords =
          from word in words
          where word.Length <= 5
          select word;
        
        foreach(var word in shortWords)
          Console.WriteLine(Word);
    }
}
```

[예제: VB.NET으로 작성한 Hello LINQ]
```VB.NET
Module HelloWorld
  Sub Main()
    Dim words As String() =
      {"hello", "wonderful", "linq", "beautiful", "world"};

    Dim shortWords = _
      From word In words _
      Where word.Length <= 5 _
      Select word

    For Each word In shortWords
      Console.WriteLine(word)
    Next
  EndSub
End Module
```
[실행 결과]
hello
linq
world

[예제: 고전적인 방식으로 구현한 Hello LINQ]
```C#
using System;

static class HelloWorld
{
    static void Main()
    {
        string[] words = new string[]
        {"hello", "wonderful", "linq", "beautiful", "world"};

        foreach(string word in words)
        {
            if(word.Length <= 5)
              Console.WriteLine(word);
        }
    }
}
```
- 이 코드가 더 간결하고 이해하기 쉽다....
- 그러나 LINQ가 제공하는 장점들은 무궁무진함...!
- 만약 그룹화 기능과 정렬 기능을 추가한다면...?
```C#
 class Program
    {
        static void Main(string[] args)
        {
            string[] words =
                {"hello", "wonderful", "linq", "beautiful", "world" };

            var groups =
                from word in words
                orderby word ascending
                group word by word.Length into lengthGroups
                orderby lengthGroups.Key descending
                select new { Length = lengthGroups.Key, Words = lengthGroups };

            foreach (var group in groups)
            {
                Console.WriteLine("Words of length " + group.Length);
                foreach (string word in group.Words)
                {
                    Console.WriteLine(" " + word);
                }
            }

            Console.ReadKey();
        }
    }
```
```VB.NET
Module HelloWorld
      Sub Main ()
            string[] words =
                {"hello", "wonderful", "linq", "beautiful", "world" };

            Dim groups = _
                From word In words_
                Order By word Ascending _
                Group By word.Length Into TheWords = Group _
                Order By Length Descending
            
            For Each group in groups
                Console.WriteLine("Words of length " + _
                    group.Length.ToString())
                For Each word in group.TheWords
                    Console.WriteLine(" "+Word)
                Next
             Next
        End Sub
End Module
```
- "단어 목록 중 알파벳순으로 단어를 정렬한 후 그 길이에 따라 내림차순으로 그룹화하라"
는 질의를 하나의 질의로 표현함
- 이것을 LINQ로 하지 않는다면 훨씬 많은 코드와 컬렉션을 다루어야 함
- -> LINQ의 장점; 풍부한 표현력
- SQL과 유사한 질의기능 외에도 LINQ가 제공하는 함수들: Sum, Min Max, Average등
- 
## 1.5 LINQ to XML의 첫걸음: XML 문서 질의하기
- LINQ to XML은 LINQ 프레임워크의 장점 이용 -> XML을 질의하고 변환하는 기능을 .NET 프로그래밍 언어의 확장형태로 제공
- LINQ to XML:
  - 완벽히 구현된 하나의 XML API
  - 메모리 내의 XML문서나 개체 트리뿐 아니라 스트림으로 들어온 문서도 처리 가능함
  - LINQ to XML로 지금까지 사용해왔던 System.Xml의 고전적인 XML API를 대신해서 대부분의 XML 처리작업을 더 쉽게 할 수 있다는 것을 의미함

### 1.5.1 LINQ to XML은 왜 필요한가?
- XML:
  - 상당히 범용적으로 사용됨
  - 애플리케이션 간에 데이터를 주고받기 위해 사용됨
  - 정보를 저장하는 매체로 사용됨
  - 임시적인 정보를 영구적으로 정리
  - 웹 페이지나 리포트를 작성하기 위해서도 사용됨
- 최근까지만 해도 XML은 대부분의 프로그래밍 언어에서 기본적으로 지원되지 않았음
- -> XML을 다루기 위해서는 API들을 배워야만 했음
- 이런 API에는 XSLT, SAX, XQuery 등을 위해 구현된 XmlDocument, XmlReader, XPathNavigator, XslTransform 등이 존재함
- 그러나 이런 API들은 손수비게 프로그래밍 언어와 융합하여 사용하기가 어려움
- 가끔 간단한 결과를 얻을 때도 지루한 코딩의 과정을 거쳐야 하는 경우가 있음

- LINQ to XML은 XML을 지원하기 위해 LINQ가 제공하는 언어에 내장된 질의기능을 확장함
- XML에 대한 지원을 위해 XPath와 XQuery의 강력함이 필요함
- 형을 가진 IntelliSense가 사용 가능한 프로그래밍 언어를 선택할 것
- LINQ to XML은 DOM에서 구현된 것들을 바탕으로 개발자들이 DOM에 내장된 제약을 뛰어넘을 수 있게 해줌

[LINQ to XML을 XML DOM과 비교하여 LINQ to XML이 얼마나 더 나은 선택인지 보여주는 자료]

| LINQ to XML의 특성                                           | XML DOM의 특성                                |
| ------------------------------------------------------------ | --------------------------------------------- |
| 개체를 중심으로 구성                                         | 문서구조를 중심으로 구성                      |
| 선언적 모델                                                  | 명령적 모델                                   |
| LINQ to XML 코드는 XML 문서의 구조화된 계층구조와 비슷한 레이아웃을 보여줌 | 코드와 문서 간의 구조적 유사성이 없음         |
| 언어 내장형 질의를 사용 가능                                 | 언어 내장형 질의를 사용 불가능                |
| 개체와 속성을 만드는 것은 하나의 명령으로 처리 가능. 텍스트 노드들은 그냥 문자열 형식 | 간단한 것들도 매우 많은 코드를 필요로 함      |
| 간소화된 XML 네임스페이스 처리방식을 지원함                  | 네임스페이스 관리자들과 접두사를 잘 다뤄야 함 |
| 빠르고 가벼움                                                | 매우 무겁고 메모리 사용량 많음                |
| 스트리밍을 지원함                                            | 모든 것이 메모리에 로딩됨                     |
| 개체와 속성의 API가 유사함                                   | XML 문서의 구성요소들마다 다루는 방법이 다름  |
- LINQ to XML은 DOM의 문서 중심의 접근법 대신 **개체 중심의 접근법**을 실현 가능하게 함

- .NET 프레임워크가 제공하는 두 개의 클래스는 XmlReader와 XmlWriter
- 이 클래스들은 XML 텍스트를 그대로 처리 가능하게 함, LINQ to XML에 비해 저급한 방식으로 XML을 취급함
- LINQ to XML은 기술적으로 완전히 새로운 XML API를 이용하지는 않음
    - 내부적으로 XmlReader와 XmlWriter 클래스를 이용하고 있음
    - -> LINQ to XML은 XmlReader 및 XmlWriter와의 호환성을 유지하고 있음

- LINQ to XML은 좀 더 직접적인 방법으로 문서를 생성 가능하게 함, XML 문서의 내용을 질의하는 것도 더 쉽게 해줌
- LINQ 기술을 그대로 확장하고 있으므로 다른 데이터 저장소에서 가져온 데이터와 함께 사용해야 할 때 아주 좋은 선택이 될 것

### 1.5.2 Hello LINQ to XML
[Book 클래스가 C#에서 정의되는 예]
```C#
 class Book"
    {
        public string Publisher;
        public string Title;
        public int Year;

        public Book(string title, string publisher, int year)
        {
            Title = title;
            Publisher = publisher;
            Year = year;
        }
    }
```

[VB.NET에서의 코드]
```VB.NET
Public Class Book
 Public Publisher As String
 Public Title As String
 Public Year As Integer

 Public Sub New( _
    ByVal title As String, _
    ByVal publisher As String, _
    ByVal Year As Integer)
  Me.Title = title
  Me.Publisher = publisher
  Me.Year = year
 End Sub
End Class
```
- 다음과 같은 책의 컬렉션을 가지고 있다고 하자
```C#
Book[] books = new Book[] {
  new Book("Ajax in Action", "Manning", 2005),
  new Book("Windows Forms in Action", "Manning", 2006),
  new Book("RSS and Atom in Action", "Manning", 2006)
};
```
- 2006년에 출간된 책을 XML 형식으로 받고 싶다면...
[LINQ to XML을 사용한 방법]
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace StudyLINQ_ch1
{
    class Book
    {
        public string Publisher;
        public string Title;
        public int Year;

        public Book(string title, string publisher, int year)
        {
            Title = title;
            Publisher = publisher;
            Year = year;
        }
    }

    static class HelloLinqToXml
    {
        static void Main()
        {
            Book[] books = new Book[] {
              new Book("Ajax in Action", "Manning", 2005),
              new Book("Windows Forms in Action", "Manning", 2006),
              new Book("RSS and Atom in Action", "Manning", 2006)
            };
            XElement xml = new XElement("books",
                from book in books
                where book.Year == 2006
                select new XElement("book",
                  new XAttribute("title", book.Title),
                  new XElement("publisher", book.Publisher)
                )
            );

            Console.WriteLine(xml);
        }
    }
}
```
[XML DOM을 이용한 방법]
```C#
static class HelloLinqToXml
{
    static void Main()
     {
            Book[] books = new Book[] {
              new Book("Ajax in Action", "Manning", 2005),
              new Book("Windows Forms in Action", "Manning", 2006),
              new Book("RSS and Atom in Action", "Manning", 2006)
            };

            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("books");
            foreach (Book book in books)
            {
                if (book.Year == 2006)
                {
                    XmlElement element = doc.CreateElement("book");
                    element.SetAttribute("title", book.Title);

                    XmlElement publisher = doc.CreateElement("publisher");
                    publisher.InnerText = book.Publisher;
                    element.AppendChild(publisher);

                    root.AppendChild(element);
                }
            }

            doc.AppendChild(root);
            doc.Save(Console.Out);
    }
}
```
- LINQ to XML이 DOM에 비해 훨씬 시각적으로 친숙함
- XML 조각을 얻어내기 위해 작성하는 코드의 구조 = 궁극적으로 만들고자 하는 문서의 모습과 매우 유사함
- 이러한 형태의 접근법 = **함수형 생성 패턴**

## 1.6 LINQ to SQL의 첫걸음: 관계형 DB에 대해 질의하기
- LINQ to SQL은 자동으로 객체에 대한 변화를 추적함
- 동적인 SQL 질의나 저장된 프로시저를 통해 데이터베이스를 적절히 관리함
- LINQ는 질의라는 개념 자체를 프로그래밍 언어에 포함시킴 -> 매우 자연스러운 일부분으로 만드는 것을 목표로 함
- 이 정신을 이어받아 개발자들이 LINQ to Object나 LINQ to XML에서 접했던 구문과 동일한 구문을 사용
    - -> 관계형 데이터베이스를 질의할 수 있게 해줌

### 1.6.1 LINQ to SQL의 기능들
- LINQ to SQL는 LINQ의 확장 매커니즘을 통해 언어 내장형의 데이터 접근기능을 제공함
- .NET 사용자 정의 프로퍼티에 암호화되어 저장된 매핑 정보나 XML 문서를 이용함
- 이 정보는 자동적으로 관계형 DB에 저장된 객체들의 영속성을 다루기 위해 사용됨

### 1.6.2 Hello LINQ to SQL
- 객체의 컬렉션에 대해 질의 작성 가능함
- 다음 코드는 메모리 내의 연락처 정보의 집합을 도시에 따라서 필터링함
```C#
from contact in contacts
where contact.City == "Paris"
select contact;
```
- LINQ to SQL 덕분에 데이터베이스에서 LINQ 질의가 가능한 메모리내 객체로 데이터를 옮기는 과정 생략 가능
- 곧바로 관계형 데이터베이스에 매우 유사한 LINQ 질의를 직접 보내는 게 가능

```C#
from contact in db.GetTable<Contact>()
where contact.City == "Paris"
select contact;
```

- 질의가 동작하는 대상 객체만 다를 뿐, 질의문법 구조 자체는 완벽하게 똑같다!
- -> 어떻게 여러 종류의 데이터에 동일한 방법으로 접근할 수 있는지 보여주는 좋은 예시임
- SQL 질의를 자동생성하게 되는 LINQ 질의임
- LINQ to SQL의 경우: 실제로 데이터를 처리하는 동작은 데이터베이스 서버에서 이루어짐
    - 프로그래밍 언어 속에 잘 들어맞음, SQL 질의와 다르게 컴파일시에 검증까지 이루어지는 매우 정형화된 엄격한 형을 가진 질의 API

#### 개체 클래스
- LINQ to SQL 애플리케이션을 작성하는 첫 번째 단계: 애플리케이션 데이터를 표현하게 될 개체 클래스를 선언하는 것

- 마이크로소프트사가 LINQ 코드 예제와 함께 제공하는 Northwind 샘플 데이터베이스의 Contacts테이블에 연결할 것
- 이 과정을 위해 해야 할 일: 사용자 정의 프로퍼티 하나만 클래스에 적용
```C#
[Table(Name="Contacts")]
class Contact
{
    public int ContactID;
    public string Name;
    public string City;
}
```
- Table 프로퍼티는 System.Data.Linq.Mapping 네임스페이스를 통해 LINQ to SQL을 제공
- 데이터베이스 테이블의 이름을 지정할 수 있도록 Name 프로퍼티를 가지고 있음

- 개체 클래스를 테이블과 연관짓는 것 + 테이블의 열과 관련된 각각의 항목이나 프로퍼티가 무엇인지 별도로 표시해둬야 함
- Column 프로퍼티를 통해 이런 일을 할 수 있음

```C#
[Table(Name="Contacts")]
class Contact
{
    [Column(IsPrimaryKey=true)]
    public int ContactID;
    [Column(Name="ContactName")]
    public string Name;
    [Column]
    public string City;
}
```
- ContactName이라고 표시한 곳을 보면 다른 열의 이름이나 열의 형을 지정해주지 않음
- 이 경우 LINQ to SQL이 알아서 클래스의 항목 중에서 추측해냄~!

#### DataContext   
- 프로그래밍 언어에 내장된 질의(LINQ)를 이용하기 위해 준비해야 하는 것은 System.Data.Linq.DataContext 객체
- DataContext의 목적: 객체에 대한 요청을 데이터베이스가 알아듣는 SQL 질의로 변환, 반대로 결과로 나오는 값들을 다시 객체로 만들어 표출하는 것

- 이것을 위해 NorthWind.mdf 데이터베이스를 사용할 것
- 이 데이터베이스는 Data 디렉토리 안에 들어있으며 DataContext 객체의 생성과정은 다음과 같음
```C#
string path = Path.GetFullPath(@"..\..\..\..\Data\northwind.mdf");
DataContext db = new DataContext(path);
```
- DataContext 클래스의 생성자는 connection string을 매개변수로 받아들임

- DataContext는 데이터베이스 내의 테이블에 접근 가능하게 함
[Contact 클래스에 매핑된 Contacts 테이블에 접근하는 방법]
`Table<Contact> contacts = db.GetTable<Contact>()`
- DataContext.GetTable은 엄격한 형을 가지고 작업할 수 있게 해주는 제너릭 메소드
- 이것이 LINQ 질의를 사용 가능하게 해줌!

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StudyLINQ_ch1
{
    class HelloLinqToSql
    {
        [Table(Name = "Contacts")]
        class Contact
        {
            [Column(IsPrimaryKey=true)]
            public int ContactID { get; set; }
            [Column(Name="ContactName")]
            public string Name { get; set; }
            [Column]
            public string City { get; set; }

        }

        static void Main()
        {
            string path =
                System.IO.Path.GetFullPath(@"..\..\..\..\Data\northwnd.mdf");
            DataContext db = new DataContext(path);
            var contacts =
                from contact in db.GetTable<Contact>()
                where contact.City == "Paris"
                select contact;

            foreach (var contact in contacts)
            {
                Console.WriteLine("Bonjour " + contact.Name);
            }
         }
    }
}
```
- 나도 모르게 SQL Server에 보내진 SQL문
```SQL
SELECT [t0].[ContactID], [t0].[ContactName] AS [Name], [t0].[City]
FROM [Contacts] AS [t0]
WHERE [t0].City = @p0
```
- LINQ를 사용해보면 얼마나 쉽게 엄격한 형을 유지하여 데이터베이스에 접근할 수 있는지 알 수 있음

- LINQ to SQL이 자동으로 개발자 대신 해주는 일
    - 데이터베이스에 대한 연결을 여는 일
    - 데이터베이스가 알아들을 수 있는 SQL 질의를 작성하는 일
    - 데이터베이스에 대해 그 질의를 수행시키는 일
    - 테이블 형태로 반환된 결과를 사용하기 쉬운 객체형태로 전환시켜 주는 일

- 고리타분한 방식의 코드와 LINQ to SQL 비교 시 확인 가능한 것들
    - SQL로 작성된 따옴표 속에 들어 있는 질의
    - 컴파일시 오류항목을 체크하지 못함
    - 느슨하게 바인딩된 매개변수
    - 느슨하게 형이 적용된 반환 결과물
    - 더 많은 코드
    - 더 많은 사전 지식

- LINQ to SQL은 데이터 접근 코드의 압박에서 벗어나 효율적으로 코드를 작성할 수 있게 해줌

### 1.6.3 더 가까이서 살펴본 LINQ to SQL
- LINQ to SQL은 원한다면 SQL 질의를 직접 손으로 작성하거나 저장된 프로시저를 이용하면서 LINQ to SQL의 나머지 장점을 그대로 흡수할 수 있는 방법도 제공함
- 개체 클래스를 생성하고 매핑 정보를 제공하는 부분: LINQ to SQL과 함께 제공되는 LINQ to SQL Designer에 의해 자동으로 생성 가능함

## 1.7 요약
- LINQ는 단순히 SQL이나 XML을 C#이나 VB.NET에서 코드 속에 내장시키는 것을 의미하지는 않음 
- 훨씬 광범위한 개념의 전환이 숨어 있음
- LINQ는 애플리케이션에서 데이터에 접근하는 완전히 새로운 방법을 제시해줌
