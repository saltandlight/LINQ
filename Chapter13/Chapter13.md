# Chapter13. 다양한 계층에서 사용되는 LINQ🦌
- LINQ는 확장성에 힘입어 관계형 데이터베이스, XML, DataSets, 메모리 내 객체 컬렉션 등 여러 종류의 데이터를 다루는 데 사용 가능한 완전한 도구들의 집합임
- 이 장에서 살펴볼 것:
    - LINQ를 이용한 예제 어플리케이션
    - LinqBooks 예제를 보면 언제 어디서 어떻게 각각의 LINQ 변종이 사용되었는지 알 수 있을 것임
    - 목표: 각각의 애플리케이션 계층에서 LINQ를 사용해야 하는지 결정하는 것을 도와주고 애플리케이션 구조에 어떤 효과를 주는지 알아보는 것

## 13.1 LinqBooks 애플리케이션의 개요
- LinqBooks 애플리케이션: 사용자들이 개인적을 도서 카탈로그를 관리할 수 있도록 도와주는 애플리케이션
### 13.1.1 제공되는 여러 기능
- LinqBooks 애플리케이션의 주요 기능들
    - 사용자가 가진 책들을 추적하기
    - 사용자들의 서평을 저장하기 
    - 외부에서 책에 관련된 더 많은 정보를 받아와서 저장하기
    - 책의 목록과 서평 정보를 다른 사람들에게 제공 가능하도록 규격화해서 출력하기

## 13.3 LINQ to XML의 용도
- LINQ to XML은 XML을 읽어들이거나 생성하기 위해 사용 가능함
- LINQ to XML은 DB의 데이터와 함꼐 사용 가능, RSS 피드를 작성하는 등 여러가지로 활용이 가능함

### 13.3.1 Amazon에서 데이터를 불러들이기
- LinqBooks에서 LINQ to XML을 사용하는 가장 첫 번째 부분은 독립적으로 실행 가능한 유틸리티의 형태로 등장함
- [책의 목룍을 가져오기 위해 사용되는 LINQ to XML 질의]
```C#
var books=
    from result in amazonXml.Descendants(ns+"Item")
    let attributes = result.Element(ns+"ItemAttributes")
    select new Book{
        Isbn = (string)attributes.Element(ns+"ISBN");
        Title = (string)attributes.Element(ns+"Title");
    };

bookBindingSource.dataSource = books;
```
- Import 체크박스에 의해 선택된 책들의 목록은 LINQ to Objects를 이용해 완성됨
```C#
var selectedBooks =
    from row in bookDataGridView.Rows.OfType<DataGridViewRow>()
    where (bool)row.Cells[0].EditedFormattedValue
    select (Book)row.DataBoundItem;
```
- [LINQ to XML 질의를 이용하여 데이터베이스에 삽입하기 위한 데이터를 준비하기]
```C#
var booksToImport =
    from amazonItem in amazonXml.Descendants(ns+"Item")
    join selectedBook in selectedBooks
      on (string)amazonITem
                .Element(ns+"ItemAttributes")
                .Element(ns+"ISBN")
      equals selectedBook.Isbn
    join p in ctx.Publishers
      on(string)amazonItem
                .Element(ns+"ItemAttributes")
                .Element(ns+"Publisher")
      equals p.Name into publishers
    from existingPublishers.DefaultIfEmpty()
    let attributes = amazonItem.Element(ns+"ItemAttributes")
    select new Book {
        ID = Guid.NewGuid(),
        Isbn = (string)attributes.Element(ns+"ISBN"),
        Title = (string)attributes.Element(ns+"Title"),
        Publisher = (existingPublisher ??
             new Publisher{
                 ID = Guid.NewGuid(),
                 Name = (string)attributes.Element(ns+"Publisher")
             }
        },
        Subject = (Subject)categoryComboBox.SelectedItem,
        PubDate = (DateTime)attributes.Element(ns+"PublicationDate"),
        Price = ParsePRice(Attributes.Element(ns+"LastPrice")),
        BookAuthors = GetAuthors(attributes.Eelemnts(ns+"Author"))
    }; 
```
### 13.3.2 RSS 피드를 생성하기
- LinqBooks 웹 사이트에서 내보내는 RSS 피드는 DB 내에 있는 서평의 목록을 반환할 것임
- RSS 피드는 웹 메소드를 이용하여 XmlDocument의 형태로 반환될 것임
- [RSS 피드를 생성하여 XmlDocument로 반환해주는 웹 메소드]
```C#
[WebMethod]
public XmlDocument GetReviews()
{
    var dataContext = new LinqBooksDataContext();

    var xml = 
        new XElement("rss",
        new XAttribute("version", "2.0"),
            new XElement("channel", 
                new XElement("title","LinqBooks reviews"),
                from review in dataContext.Reviews
                select new XElement("item",
                    new XElement("title",
                        "Review of \""+review.BookObject.Title+"\" by "+
                        review.UserObject.Name),
                    new XElement("description", review.Comments),
                    new XElement("link",
                        "http://example.com/Book.aspx?ID="+
                        review.BookObject.ID.ToString())
                )
            )
        );

    XmlDocument result = new XmlDocument();
    result.Load(xml.CreateReader());
    return result;    
}
```
- 이 코드에서 LINQ to XML이 RSS 피드와 같은 간단한 XML 문서의 생성을 얼마나 간단하게 해주는지 알 수 있음

## 13.4 LINQ to DataSet의 용도
- DataSet의 활용을 통해 책 카탈로그내에 있는 전체 데이터를 XML로 변환시켜 추출이 가능함
- 불러오기, 내보내기 기능은 DataSet을 활용하여 구현됨
- 내보내기는 형을 가진 DataSEt 클래스와 생성되는 TableAdapters를 이용하여 구현되므로 LINQ가 여기서 사용되지는 않음
- 다음 코드를 보면 매우 직설적으로 구현되어 있음
- [데이터베이스에서 가져온 완전한 데이터를 형을 가진 DataSet에 집어넣기]
```C#
LinqBooksDataSet dataSet = new LinqBooksDataSet();
new SubjectTableAdapter().Fill(dataSet.Subject);
new PublisherTableAdapter().Fill(dataSet.Publisher);
new BookTableAdapter().Fill(dataSet.Book);
new AuthorTableAdapter().Fill(dataSet.Author);
new BookAuthorTableAdapter().Fill(dataSet.BookAuthor);
new UserTableAdapter().Fill(dataSet.User);
new ReviewTableAdpater().Fill(dataSet.Review);
```
- 불러들이는 연산은 조금 더 복잡함
- 목표: 친구나 다른 사람이 제공한 기존 XML 문서를 불러들여 예제의 카탈로그에 어떤 책들을 불러올지 선택하는 것
    - 이를 위해 LINQ to Objects를 이용할 것임
- 불러들이는 과정에서 첫 번째로 해야 하는 일: 선택된 XML을 다음과 같은 방법으로 DataSet으로 불러들이는 것
```C#
var dataSet = new LinqBooksDataSet();
dataSet.ReadXml(uploadXml.FileContent);
Session["DataSet"] = dataSet;
```
- DataSet을 ASP.NET의 세셔넹 저장해서 필요할 떄마다 쉽게 사용가능하도록 해야 함
- 데이터가 로딩된 후 DataSEt에 대해 질의하여 이미 카탈로그 안에 있는 책들의 목록을 보여줄 수 있음
- DB 안에 있는 책들의 목록을 준비해야 함
```C#
var dataContext = new LinqBooksDataContext();
IEnumerable<String> knownTitles =
    dataContext.Books.Select(book => book.Title);
```
- 그러고 나서 이 목록을 사용하여 DataSet에 있는 책들의 목록을 필터링 가능
- [DataSet의 데이터를 필터링하고 출력하기]
```C#
var dataSet = (LinqBooksDataSet)Session["DataSet"];
var queryExisting = 
    from book in dataSet.Book
    where knownTitles.Contains(book.Title)
    orderby book.Title
    select new {
        Title = book.Title,
        Publisher = book.PublisherRow.Name,
        ISBN = book.Field<String>("Isbn"),
        Subject = book.SubjectRow.Name
    };
GridViewDataSetExisting.DataSource = queryExisting;
GridViewDataSEtExsiting.DataBind();
```
- 아직 사용자의 카탈로그에 담기지 않은 책들의 정보도 출력 가능함
- 필터링에 사용된조건이 정반대라는 것을 제외하고는 거의 동일한 작업
- 다음에 해당 작업을 수행하는 코드가 나와있음
- [DataSet의 데이터를 필터링하고 출력하기]
```C#
var queryNew = 
    from book in dataSet.Book
    where !knownTitles.Contains(book.Title)
    orderby book.Title
    select new {
        Id = book.ID,
        Title = book.Title,
        Publisher = book.PublisherRow.Name,
        ISBN = book.Field<String>("Isbn"),
        Subject = book.SubjectRow.Name
    };
GridViewDataSetNew.DataSource = queryNew;
GridViewDataSetNew.DataBind();
```
- LINQ to DataSet은 DataSEt에 대한 질의를 매우 쉽게 수행할 수 있게 해줌
- 이것은 DataSet을 다룰 때 항상 도구로 사용해야 하는 필수도구에 가까움

## 13.7 미래에 대한 고찰
- LINQ의 가장 큰 장점: 확장 가능, 그로 인해 앞으로도 새로운 시나리오와 새로운 형태의 데이터를 지원할 수 있도록 꾸준히 진화가능하다는 점임

### 13.7.2 형을 가진 LINQ to XML인 LINQ to XSD
- XSD란: XML 스키마 정의
- LINQ to XML 프로그래머들은 일반화된 XML 트리에 대해 작업
- vs
- LINQ to XSD를 이용하는 개발자들은 형을 가진 XML 트리에 대해서 작업하게 됨
    - 형을 가진 XML 트리는 특정한 XML 스키마를 모델링하는 .NET 형의 인스턴스로 구성됨

- [어떤 주문서를 나타내는 XML 트리구조의 항목들의 합을 구하는 코드]
```C#
from item in purchaseOrder.Elements("Item")
select (double)item.Element("Price") * (int)item.Element("Quantity");
```
- LINQ to XSD를 사용하면 동일한 질의를 훨씬 명료하고 형 관련 오류가 적게 생기는 방법으로 작성 가능
```C#
from item i purchaseORder.ITem
select item.Price * item.Quantity;
```
- LINQ to XSD를 이용하면 위험하게 문자열과 형을 캐스팅하는 과정을 거칠 필요 없음
- 모든 것은 엄격하게 형이 정의되어 있고 구조화되어 있음

### 13.7.3 PLINQ : LINQ와 병렬 컴퓨팅의 집합
- PLINQ: Parallel LINQ를 뜼함
- 이것의 목표: LINQ 질의를 이용하여 연산을 여러 개의 CPU나 코어에 분산시키는 역할을 하는 것
- 작성하는 LINQ질의는 앞으로도 동일하게 작성하되, 내부적으로 알아서 그것을 여러 연산장치에 배분하고 병렬 수행이 가능하게 해주는 것

- PLNQ의 핵심요소는 AsPArallel이라는 질의 연산자임
- 이것은 LINQ 질의에 붙어 LINQ 질의들을 병렬로 수행시켜 줌
```C#
IEnumerable<T> leftData = ..., rightData = ...;
var query =
    from x in leftData.AsParallel()
    join y in rightData on x.A == y.B
    select f(x,y);
```