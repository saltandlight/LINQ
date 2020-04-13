# Chapter11. 통상적인 LINQ to XML 시나리오🐩
**알아볼 내용(발생 가능한 시나리오들):**
    - XML에서 객체를 만드는 경우
    - 객체에서 XML을 만드는 경우
    - 데이터베이스의 데이터를 바탕으로 XML을 만드는 경우
    - 데이터베이스에서 불러온 데이터와 XML의 데이터를 조합하여 사용하는 경우
    - 데이터베이스를 XML에서 읽어온 데이터로 수정하고 업데이트하는 경우
    - 텍스트 파일을 LINQ to XML을 이용하여 XML 파일로 만드는 경우

## 11.1 XML에서 객체를 만들어내기
- 왜 먼저 LINQ to XML을 이용하여 XML에서 객체를 만들어내는 과정을 거쳐야 하는지 생각해보기
- 요즘 애플리케이션을 작성할 때 객체를 이용하여 만드는 경우가 많음
- 객체는 애플리케이션 내에서 사용하는 로직과 데이터를 캡술화할 수 있도록 해줌
- XML은 데이터의 규격이고 프로그래밍 언어가 아님 -> 애플리케이션에서 이용하고자 할 떄 데이터를 XML에서 불러내야 함
- 애플리케이션이 XML의 구조상의 제약에 너무 속박되지 않도록 하기 위해 XML 파일의 데이터를 LINQ to XML이 제공하는 강력한 기능들을 바탕으로 객체들의 집합으로 변환할 것임.

### 11.1.1 목표
- 목표: LINQ to XML이 제공하는 기능을 이용해서 다음의 코드에 나타난 XML 문서 내의 데이터를 포함하는 객체의 컬렉션을 만들어내는 것
    - XML 파일 내의 데이터를 가져와서 Book 객체의 컬렉션을 만드는 것임
    - Book 객체에 내용을 채워넣는 것 외에도, Book 클래스의 Subject, Publisher, Authors, Reviews 속성도 XML 파일에서 해당하는 데이터를 찾아와 채워넣을 것
### 11.1.2 구현
- XML에서 객체들을 만들기 위해서 LINQ to XML이 제공하는 축 메소드를 잘 활용하는 몇 가지 질의를 작성해볼 것임
- [객체 초기화 함수를 이용하여 XML에서 XML에서 Book객체를 만들기]
```C#
XElement booksXml = XElement.Load("books.xml");
IEnumerable<XElement> bookElements = booksXml.Elements("book");

var books =
        from bookElement in booksXml.Elements("book")
        select new Book
        {
            Title = (string)bookElement.Element("title"),
            PublicationDate = (DateTime)bookElement.Element("publicationDate"),
            Price = (decimal)bookElement.Element("price"),
            Isbn = (string)bookElement.Element("isbn"),
            Notes = (string)bookElement.Element("notes"),
            Summary = (string)bookElement.Element("summary")
        };
```
- 자식 개체에 포함된 책에 관한 정보들을 읽기 위해 필요한 개체들을 Element 축 메소드와 XElement에 정의된 명시적인 캐스팅 연산자를 이용하여 값들을 제대로 된 데이터형으로 변환함
- 이 질의가 책에 대한 기초적 정보는 반환하지만 저자목록이나 서평과 같이 자식 노드에 담긴 정보는 미포함함
- 저자들을 포함하기 위해서는 지릐를 변경해서 XML에 정의된 저자 설명과 일치하는 Author 객체의 목록을 반환하는 중첩 질의를 추가해야 함
- <author> 객체가 <book> 객체의 바로 하위 개체가 아니므로 Descendants 질의 축 메소드와 객체 초기화 함수 문법을 다시한번 이용해서 XML에 포함된 데이터로 저자(Author) 데이터 객체를 생성하면 됨
```C#
Authors =
    from authorElement in bookElement.Descendants("author")
    select new Author{
        FirstName = (string)authorElement.Element("firstName"),
        LastName = (string)authorElement.Element("lastName")
    }
```
- 이 코드의 질의 표현식은 IEnumerable<Author> 형식의 데이터를 반환하므로 질의 표현식의 결과를 바로 책 인스턴승듸 Authors 프로퍼티에 할당 가능함
- 서평을 포함시키는 과정에서도 동일한 접근방식 선택 가능
- XML에서 서평을 추출하는 질의 표현식 작성한 후, Review 객체 속에 정보를 집어넣는 질의 표현식을 작성하면 됨
```C#
Reviews =
    from reviewElement in bookElement.Descendants("review")
    select new Review{
        User = new User{Name = (string)reviewElement.Element("user")},
        Rating = (int)reviewElement.Element("rating"),
        Comments = (string)reviewElement.Element("comments")
    }
```
- 모든 코드를 다 조합해보면 다음과 같이 해서 책의 목록을 만들 수 있음
- [XML에서 객체를 생성하기]
```C#
 var books =
                from bookElement in booksXml.Elements("book")
                select new Book
                {
                    Title = (string)bookElement.Element("title"),
                    Publisher = new Publisher
                    {
                        Name = (string)bookElement.Element("publisher")
                    },
                    PublicationDate = (DateTime)bookElement.Element("publicationDate"),
                    Price = (decimal)bookElement.Element("price"),
                    Isbn = (string)bookElement.Element("isbn"),
                    Notes = (string)bookElement.Element("notes"),
                    Summary = (string)bookElement.Element("summary"),
                    Authors =
                                from authorElement in bookElement.Descendants("author")
                                select new Author
                                {
                                    FirstName = (string)authorElement.Element("firstName"),
                                    LastName = (string)authorElement.Element("lastName")
                                },
                    Reviews =
                                from reviewElement in bookElement.Descendants("review")
                                select new Review
                                {
                                    User = new User { Name = (string)reviewElement.Element("user") },
                                    Rating = (int)reviewElement.Element("rating"),
                                    Comments = (string)reviewElement.Element("comments")
                                }
                };

```

## 11.6 텍스트 파일을 XML로 변환시키기
- 아직도 많은 파일들이 텍스트 파일로 존재하고 있음
- 시스템을 모두 XML을 통해 바꾸고 싶음...
- LINQ to XML이 텍스트 파일을 XML로 변환 시 어떤 도움을 줄 수 있는지 살펴보자

### 11.6.1 목표
- 텍스트 파일을 계층구조를 가진 XML 문서로 변환하는 것을 목표로 함
- 텍스트파일은 ISBN, 제목, 저자, 출판사, 출판일자,가격 등의 정보 포함해야 함
- 텍스트파일의 데이터를 해석하여 다음과 같은 xml로 만들어야 함
```xml
<?xml version="1.0" encoding="utf-8" ?>
<books>
  <book>
    <title>LINQ in Action</title>
    <authors>
      <author>
        <firstname>Fabrice</firstname>
        <lastname>Marguerie</lastname>
        <website>http://linqinaction.net/</website>
      </author>
      <author>
        <firstname>Steve</firstname>
        <lastname>Eichert</lastname>
        <website>http://iqueryable.com/</website>
      </author>
      <author>
        <firstname>Jim</firstname>
        <lastname>Wooley</lastname>
        <website>http://devauthority.com/blogs/jwooley/</website>
      </author>
    </authors>
    <subject>
      <name>LINQ</name>
      <description>LINQ shall rule the world</description>
    </subject>
    <publisher>Manning</publisher>
    <publicationDate>January 15, 2008</publicationDate>
    <price>44.99</price>
    <isbn>1933988169</isbn>
    <notes>Great book!</notes>
    <summary>LINQ in Action is great!</summary>
    <reviews>
      <review>
        <user>Steve Eichert</user>
        <rating>5</rating>
        <comments>What can I say, I'm biased!</comments>
      </review>
    </reviews>
  </book>
  <book>
    <title>Patterns of Enterprise Application Architecture</title>
    <authors>
      <author>
        <firstname>Martin</firstname>
        <lastname>Fowler</lastname>
        <website>http://linqinaction.net/</website>
      </author>
    </authors>
    <subject>
      <name>LINQ</name>
      <description>LINQ shall rule the world</description>
    </subject>
    <publisher>Addison-Wesley</publisher>
    <publicationDate>March 9, 2012</publicationDate>
    <price>41.57</price>
    <isbn>0321127420</isbn>
    <notes>Great book!</notes>
    <summary>LINQ in Action!!!!!</summary>
    <reviews>
      <review>
        <user>Joseph</user>
        <rating>1</rating>
        <comments>Stream-of-conscouisness presenatation promoting worst-practices</comments>
      </review>
    </reviews>
  </book>
</books>
```
### 11.6.2 구현
- 원하는 XML을 작성하기 위해 텍스트파일을 열어서 각각의 행을 분리하여 배열에 넣은 다음, 배열에 들어있는 각각의 항목들을 적절한 XML 개체로 만들어내야 함
- 파일을 열고 부분들로 나누자
```C#
from line in File.ReadAllLines("books.txt")
let items = line.Split(',');
```
- File 클래스의 정적 메소드 ReadAllLines를 이용하여 텍스트 파일 내의 각각의 행을 읽어들임
- 각각의 행을 분리하기 위해 string에 사용 가능한 Split 메소드와 C#의 let 절을 이용함
- [텍스트 파일에서 읽어와서 XElement 객체를 채우기]
```C#
var booksXml = new XElement("books",
    from line in File.ReadAllLines("books.txt")
    let items = line.Split(',')
    select new XElement("book",
        new XElement("title", items[1]),
        new XElement("publisher", items[3]),
        new XElement("publicationDate", items[4]),
        new XElement("price", items[5]),
        new XElement("isbn", items[0])
    );
```
- 처리하고자 하는 파일이 큰 경우에는 이와 같은 코드가 매우 비효율적임
- 큰 파일을 가지고 작업하거나 성능이 중요할 떄는 스트림을 통해 파일을 읽어들여야 함
