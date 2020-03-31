# Chapter06. LINQ to SQL을 시작하기🌸
- 가격이 30달러 이하인 책들의 목록을 추출하고 주제에 따라 분류하도록 하겠다
- 질의 완성을 위해 Books와 Subjects라는 두 개의 데이터 집합에서 정보를 얻어 조합하는 과정을 거쳐야 함
[LINQ to Objects를 이용하여 Subjects와 Books에 대해 질의하기]
```C#
IEnumerable<Book> books = Book.GetBooks();
IEnumerable<Subject> subjects = Subject.GetSubjects();

var query = from subject in subjects
            join book in books
                 on subject.SubjectId equals book.SubjectId
            where book.Price < 30
            orderby subject.Description, book.Title
            select new
            {
                subject.Description,
                book.Title,
                book.Price
            };
```
- SQL 데이터베이스에서 추출한 책과 주제를 담는 메모리 객체를 만든 뒤 그 객체에 대한 질의를 수행하는 형태로 문제를 해결하고 있음
- 예제 코드를 수행하면 다음과 같은 SQL 질의문들이 동적으로 생성될 것
```sql
SELECT  ID, Isbn, Notes, PageCount, Price, PubDate
        Publisher, Subject, Summary, Title
FROM    Book

SELECT  ID, Name, Description
FROM    Subject;
```
- 필터링과 사영, 정렬 모두 클라이언트에서 수행됨 => 데이터를 다루기 전에 무조건 모든 레코드의 모든 필드의 정보를 클라이언트의 메모리에 올려야 함
- 결과적으로 필요한 데이터 이상을 읽어들어야 함 ... -> 비효율!
- 데이터베이스 시스테밍 제공하는 "인덱싱" 이라는 무기를 잃어버리게 됨
- 네트워크 및 시스템상의 불필요한 부하를 초래하게 된다는 뜻!
- 데이터에 접근하는 코드를 모두 수작업으로 작성하는 것은 너무 위험천만하고 반복적인 일...

- LINQ to SQL은 네트워크 및 시스테 부하를 줄여주고 데이터베이스 인덱스를 사용할 수 있게 함
- 지루하고 반복적인 코드 작성을 줄여주는 존재!!
- LIN로 코드를 작성한다고 가정하면 다음처럼 간소화된 SQL 질의문들이 동적으로 생성되는 것을 볼 수 있음
```SQL
SELECT      t0.Description, t1.Title, t1.Price
FROM        Subject AS t0 INNER JOIN 
                    Book AS t1 ON t0.ID = t1.Subject
WHERE       (t1.Price < @p0)
ORDER BY    t0.Description, t1.Title
```
- 단 한줄의 코드를 더하고 두 줄을 살짝 조정 -> 놀라운 변화
- 놀라운 변화: 
    - 많은 양의 ADO.NET 기반의 코드를 줄일 수 있게 함
    - 주기적으로 메모리와 관계형 데이터베이스 간의 데이터 동기화를 위해 별도로 코드 작성할 필요 없음

## 6.1 LINQ to SQL 속으로 뛰어들기
- 가격이 30달러 이하인 책들을 주제별로 모으는 것을 목표로 함
- 그 과정은 주제를 질의하는 과정, 가격 조건을 ㅗ 걸러진 책의 목록을 질의 하는 과정 주제와 책을 조합하는 과정 등 몇 개의 분리된 과정으로 세분화 가능

[가격이 30달러 이하인 책들의 주제와 가격을 받아오기]
```C#
IEnumerable<Book> books = Book.GetBooks();
var query = from book in books
            where book.Price < 30
            orderby book.Title
            select new 
            {
                book.Title,
                book.Price
            };
```
- DB에서 모든 책의 목록을 받아오고 있음
- 클라이언트가 메모리상에 그 목록을 올린 뒤 필요한 책만을 추려내고 있음
- DB에 실제로 행해진 SQL 질의는 다음과 같음
```sql
SELECT  ID, Isbn, Notes, PageCount, Price, PubDate,
        Publisher, Subject, Summary, Title
FROM    book;
```
- Title과 Price라는 두 가지 정보만 필요로 함에도 불구하고 모든 정보를 다 읽어들이고 있음
- 필요로 하는 책들이 아니라 모든 책들의 정보를 읽어들이는 비효율적인 작업을 수행하고 있음
- 클라이언트의 메모리상에서 정렬 작업을 수행하므로 데이터베이스 시스템이 제공하는 인덱싱 기능의 혜택을 찾아볼 수 없음

- 데이터베이스에 다음과 같은 이상적인 질의문을 보내주려고 함
```sql
SELECT      Title, Price
FROM        Book AS t0
WHERE       (Price < @p0)
ORDER BY    Title
```
- 이러한 이상적틴 코드를 DB 시스템에 보내주기 위해 얼마나 많은 수정을 해야 할까?
- 답: 하나도 없다
- 해야 할 일: Book 클래스를 수정하고 그것을 접근하는 방법을 바꿔주어야 함

- 먼저 DB 테이블과 메머리 객체의 정보항목들에 대해 일대일 매핑을 적용할 것임
- 이 장의 후반부에서 이 테이블을 주제 테이블과 연결지어 데이터베이스의 외래 키 기능과 연동시키는 방법에 대해 살펴볼 것
```C#
public class Book
{
    public Guid BookId { get; set; }
    public string Isbn { get; set; }
    public string Notes { get; set; }
    public Int32 PageCount { get; set; }
    public Decimal Price { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Summary { get; set; }
    public string Title { get; set; }
    public Guid SubjectId { get; set; }
    public Guid PublisherId { get; set; }
}
```
### 6.1.1 객체 매핑을 설정하기
- Book 클래스를 활성화하는 것부터 LINQ to SQL에 대해 배워보기
- 우선 .NET Framework 3.5의 일부분인 System.Data.Linq 어셈블리를 참조해야 함
- System.Data.Linq 어셈블리를 참조해야 함
- using 구문을 활용하여 클래스의 맨 위에서 정의해줘야 함
- Mapping 네임스페이스는 DB와 객체 간의 매핑을 선언적으로 확립할 수 있게 해줌
`using System.Data.Linq.Mappng`

- 속성(attribute)를 통해 데이터 매핑을 선언할 것
- 대부분의 경우 클래스 내애서 어떤 테이블과 관련된 매핑인지, 어떤 열과 관련된 매핑인지에 대해 정의해야 함
- Book 테이블을 메모리 내의 객체와 연관짓는 것이 가장 쉬운 매핑
- 이런 경우에는 데이터베이스는 Book이라는 테이블이 있고 메모리 객체에도 또한 Book이라는 이름의 클래스가 있어야 함
- 이미 이름이 같고 일대일 매핑이 가능한 데이터베이스 테이블과 메모리 객체를 갖추고 있지만 명시적으로 이 두 가지를 매핑하기 위해 클래스 정의에 Table이라는 속성을 추가해야 함
```C#
[Table]
public class Book {...}
```
- 좀 더 명시적으로 선언하고 싶다면 테이블의 이름에 Name이라는 매개변수를 이용함
```C#
[Table(Name="dbo.Book")]
public class Book {...}
```
- 클래스 속의 프로퍼티와 테이블 속의 열이 어떠한 일대일 대응관계를 갖는지 정의해야 함
- 매핑하고자 하는 프로퍼티들에 Column이라는 속성을 추가하는 방법을 이용
```C#
[Column]
public String Title {get; set;}
```
- 항상 직접적인 매핑만을 선택해야 하는 것은 아님
- 테이블의 열 이름과 메모리 내 객체의 프로퍼티 이름이 일치하지 않게 할 수도 있음
- 예를 들면... Book 테이블에는 PubDate라는 열이 있지만 실제 애플리케이션을 제작하는 사람들이 더 직관적으로 데이터를 이용할 수 있도록 장황한 이름을 이용하여 PublicationDate라는 이름의 프로퍼티와 매핑시킬 수 있음
- 이를 위해서 클래스 정의에 추가된 속성에 해당 열의 이름을 추가해야 함
```C#
[Column(Name="PubDate")]
public DateTime PublicatoinDate{ get; set; }
```
- 양쪽 모두에 확실히 정의해주어야 하는 것: 기본키(primary key)
- 이 예제에서는 BookId임
- 여기서 Name이라는 매개변수 외에 IsPrimaryKey라는 매개변수를 추가적으로 선언해야 함
- LINQ to SQL은 객체 간의 고유한 구별을 위해 각각의 객체에서 최소한 하나의 프로퍼티가 기본키로 지정될 것을 강제함
```C#
[Column(Name="ID", IsPrimaryKey=true)]
public Guid BookId { get; set; }
```
- 같은 방법으로 클래스 속에 있는 모든 프로퍼티들에 대해 매핑을 선언하면...
```C#
[Table]
using System.Data.Linq.Mapping;

public class Book
{

    [Column(Name = "ID", IsPrimaryKey = true)]
    public Guid BookId { get; set; }
    [Column]
    public string Isbn { get; set; }
    [Column(CanBeNull=true)]
    public string Notes { get; set; }
    [Column]
    public Int32 PageCount { get; set; }
    [Column]
    public Decimal Price { get; set; }
    [Column(CanBeNull = true)]
    public string Summary { get; set; }
    [Column(Name = "PubDate")]
    public DateTime PublicatoinDate { get; set; }

    [Column]
    public string Title { get; set; }
    [Column(Name = "Subject")]
    public Guid SubjectId { get; set; }
    [Column(Name = "Publisher")]
    public Guid PublisherId { get; set; }
}
```
- 언뜻 보면 코드 양이 두 배 늘어난 것처럼 보이지만 이 덕분에 CRUD와 관련도니 코드를 별도로 하지 않아도 됨
- -> 결국 앞으로 엄청난 양의 코드를 절약할 수 있음
- -> 그리고 특화된 질의의 수행을 위해 특호된 형태의 루틴 구현을 별도로 할 필요 없음! 나머지 작업은 프레임워크가 해준다!
- 고로 단순한 매핑을 한 번 정의해주기만 하면 됨

- 연결한 테이블이 어떤 데이터베이스에 속해 있는지 지정해주지 않았음
- 새로운 System.Data.Linq 네임스페이스에 속한 DataContext라는 새로운 객체를 생성하는 방법으로 DB와의 연결을 정의함!

### 6.1.2 DataContext를 설정하기
- DataContext는 LINQ to SQL의 핵심으로 대부분의 내부 작업을 담당함
  [DataContext에 의해 제공되는 서비스]
  ![](pic.PNG)

- 우선 DB와의 연결을 관장
- DataContext에  연결에 필요한 정보를 담은 연결 문자열(connection string)을 명시해줌
- DataContext가 그 정보를 근거로 DB에 대한 연결을 열고 닫는 역할을 전부 맡아 처리해줌
- 결과적으로 불필요하게 매우 성가시고 성능상의 문제를 야기시킬 수 있는 연결 관련 정보를 매번 처리하지 않아도 됨

## 6.2 LINQ to SQL을 이용하여 데이터 받아오기
## 6.3 질의 다듬기
## 6.4 객체 트리를 다루기
## 6.5 내 데이터는 어느 시점에 로딩되는가? 그리고 그것이 왜 중요할까?
## 6.6 데이터를 업데이트하기
## 6.7 요약