# Chapter12. LINQ 확장하기😹
- LINQ의 확장성은 LINQ 프로바이더를 작성하여 사용자가 스스로의 취향에 맞는 LINQ의 변종을 만들어 사용가능하게 함!
- 대부분의 경우는 LINQ의 동작을 매우 제한적인 범위 내에서 수정하는 형태가 될 것임
- 다행히 LINQ는 필요에 맞게 질의 연산자를 새롭게 만들거나 재정의하는 것을 유연하게 허용하고 있음
- 이 장의 목적: LINQ가 제공하는 무한한 확장 가능성을 보여주고 업무상황에 맞는 최적화된 테크닉을 고르는 과정을 도와주는 데 있음
- 살펴볼 것: 
    - 새롭게 질의 연산자를 정의하고 그것들을 LINQ 질의를 간소화하는 유틸리티 메소드로 사용하는 방법
    - 특정 영역에 유효한 질의 연산자들을 작성할 것임
    - 어떻게 Where나 OrderBy 같은 매우 기초적인 연산자까지도 재정의할 수 있는지 살펴볼 것
    - LINQ to Amazon 이라는 새로운 LINQ 프로바이더를 만들어볼 것임
        - LINQ to Amazon을 통해 웹상의 API 에 대한 호출을 LINQ 프로바이더 형태로 캡슈로하하는 방법을 설명할 것
    - 표현식 트리와 System.Linq.IQueryable<T> 인터페이스를 포함하는 좀 더 고급의 확장기능을 알아볼 것임

## 12.1 LINQ의 확장 매커니즘을 찾아내기
- 실제로 LINQ는 설계될 떄부터 확장성을 염두에 두고 있었음
- 저장공간의 위치나 방법에 얽매이지 않음
- LINQ는 직면한 데이터 저장소에 맞게 변형하여 사용 가능함

- LINQ의 핵심: 질의 연산자와 질의 표현식
    - 질의 표현식: **LINQ 질의 표현식 패턴**이라는 규격을 적용 가능한 엄격한 문법기능
    - LINQ 질의 표현식 패턴: 질의 표현식을 완벽하게 지원하기 위해 필요한 연산자들과 이들의 구현에 대해 매우 엄격한 정의를 하고 있음
    - 이 패턴을 구현하는 과정: 정확한 이름과 시그너처로 메소드를 구현하는 작업
    - 평상시에 작업하면서 익숙해진 표준 질의 연산자들은 LINQ 질의 표현식 패턴이 구현된 예임
    - 이들은 IEnumerable<T>를 보충하는 확장 메소드들로 구성되어 있음

    - 표준 질의 연산자: .NET 프레임워크의 어떠한 배열이나 컬렉션 객체에 대한 질의도 수행할 수 있도록 LINQ 질의 표현식 패턴을 구현함
    - 개발자들은 LINQ 패턴에 맞게 확장 메소드들을 구현하는 이상 어떤 클래스에도 표준 질의문법을 사용 가능함
    - 서드파티들은 표준 질의 연산자들을 자신들이 다루는 다른 영역이나 기술에 맞게 변형시키는 것도 가능
    - 사용자 정의 구현물은 원격 평가나 질의 변환, 최적화 등의 부가적인 서비스를 제공 가능함
    - 표준적인 LINQ 패턴을 준수함으로써 이런 구현물 또한 언어 통합 과정에서 보이는 LINQ의 장점이나 도구 지원을 표준 연산자와의 차등 없이 그대로 받을 수 있음

### 12.1.1 LINQ 변종들이 LINQ 구현물인 이유 
- LINQ 질의구조의 확장성은 XML이나 SQL과 같은 다양한 데이터 저장소에 모두 적용 가능한 lINQ를 만들기 위해 도입된 개념
- XML을 대상으로 한 질의 연산자(LINQ to XML)은 XPath/XQuery가 하는 일들을 매우 효율적으로 프로그램 언어에 내장시켜 줌
- 관계형 데이터를 대상으로 한 질의 연산자(LINQ to SQL)는 SQL을 기반으로 한 관계형 데이터 구조를 CLR에 맞는 객체 시스템과 매끄럽게 연동시켜 주는 역할을 함
- 이런 통합은 관계형 데이터에 대해 매우 강한 형(type)을 부여해주는 동시에 관계형 모델이 갖는 장점인 높은 표현성과 성능을 그대로 유지해줌

- LINQ가 제공해줒는 기본 LINQ 구현물들은 모두 직접적으로 LINQ의 확장성 혜택을 받고 있음
- 이런 구현물은 LINQ to Objects, LINQ to XML, LINQ to DataSet, LINQ to SQL, LINQ to Entities 등으로 이루어짐
- 구현의 측면에서는 이들은 LINQ 프로바이더임
- 각각의 프로바이더는 LINQ가 지원하는 매우 구체적인 확장 테크닉을 기반으로 만들어짐

- 사용자는 필요로 하는 것이 무엇인지에 따라 기본 프로바이더에서 사용하는 테크닉을 바탕으로 스스로의 프로바이더를 작성할 것임
- 기본 프로바이더가 어떤 형태로 구현되어 잇는지를 살펴보는 것은 스스로의 LINQ 프로바이더를 작성하면서 어떤 기법을 선택해야 하는지 결정하는 지 아주 중요한 기반지식이 됨 

#### LINQ to Objects
- LINQ to Objects는 IEnumerable<T> 인터페이스를 구현하는 배열과 같은 컬렉션 객체들에 대해 질의를 수행할 수 있도록 해줌
- LINQ to Objects는 IEnumerable<T> 형의 확장 메소드인 표준 질의 연산자들에 의존하여 구현됨
- LINQ to Objects를 사용할 떄는 System.Linq.Enumerable에 구현된 질의 연산자들을 이용하는 것
- LINQ to Objects는 사실 간결함

#### LINQ to DataSet
- LINQ to DataSet은 LINQ를 통해 질의할 수 있도록 해줌
- LINQ to Objects에 비해 복잡할 것이 별로 없음 
- LINQ to DataSet 또한 동일한 표준 질의 연산자들을 기반으로 하고 있지만, DataSets와 관련 있는 System.Data.DataRow 등의 클래스와 연동하기 위한 내용들을 추가적으로 담고 있음

#### LINQ to XML
- LINQ to XML 또한 질의 연산자에 기반하고 있지만 XML 객체를 다루기 위한 몇몇 클래스들을 갖고 있음 
- LINQ to Object와 사용방법이 같지만 XNode, XElement, XAttribute, XText와 같은 객체를 다룬다는 점이 다름

#### LINQ to Entities
- LINQ to SQL과 동일한 테크닉을 이용하여 구현됨
- LINQ to Entities는 LINQ 표현식을 ADO.NET Entity Framework에서 자주 사용되는 정규 질의 트리(canonical query tree)로 변환시켜 줌
- 이런 트리는 Entity Framework의 질의 파이프라인에게 전달되어서 매핑과 SQL 자동생성이 이루어지게 됨

- LINQ의 확장성이 가져다주는 가능성:
    - delegate를 이용하여 LINQ 질의 표현식 패턴을 구현하는 사용자 정의 연산자들을 정의 가능
    - 표준 질의 여난자를 활용하면서 특정한 데이터 저장소나 데이터형과 동작하는 클래스를 작성 가능
    - 표현식 트리를 이용하여 LINQ 질의 표현식 패턴을 구현하는 사용자 정의 질의 연산자들을 정의 가능
    - IQueryable<T> 인터페이스를 구현 가능

### 12.1.2 사용자 정의 LINQ 확장을 이용하여 무엇을 할 수 있을까?
- LINQ의 확장성 기능이 제공하는 가능성의 범위는 무한함
- LINQ는 사용자가 원하는 수준의 사용자화(customization)을 할 수 있음

- 먼저 추가적인 질의 연산자를 구현하고 필요하면 표준 질의 연산자를 재정의하여 자신이 원하는 동작을 하도록 할 수도 있음
- System.Linq.Enumerable이 포함하는 구현물과 유사한 확장 메소드를 작성함으로써 표준 연산자들을 필요에 따라 동작하도록 수정 가능

#### LINQ의 확장성을 잘 활용하면 좋은 경우
- LINQ의 확장성을 이용하면 좋은 몇 가지 시나리오
    - 사용자 정의 데이터 저장소에 대해 질의하기
    - 웹 서비스에 대해 질의하기
    - 개발자들이 LINQ의 장점을 활용 가능하도록 구성된 프레임워크나 도구를 제공할 때

- LINQ를 객체-관계형 프레임워크와 연동하는 것은 IQueryable<T> 인터페이스 및 표현식 트리를 다루는 매우 난해한 작업임
- 이런 작업은 바로 LINQ to SQL이 LINQ 질의문에서 SQL 질의문을 자동생성 시 이루어지는 작업
- 이것은 NHibernate 같은 프레임워크가 LINQ 질의문에서 SQL 질의문을 만들어내기 위해 수행하는 작업이기도 함

## 12.2 사용자 정의 질의 연산자를 작성하기
- LINQ to Objects에 초점을 둘 것임
- LINQ는 51가지 기본적인 표준 질의 연산자를 가지고 있음, 그러나 이 51가지로 모든 질의를 만족스럽게 표현 가능한 것은 아니므로 새로운 것을 필요로 할 때가 있을 것임
- LINQ를 확장시키는 첫 번째 방법: 추가적인 질의 연산자를 작성하는 것
- 추가적인 질의 연산자를 정의하는 테크닉을 이용 시 위에서 설명한 대로 표준 질의 연산자만을 가지고 작업할 떄 발생 가능한 여러 제약 상황에서 벗어날 수 있을 것임

### 12.2.1 표준 질의 연산자를 개선하기
- 표준 질의 연산자의 한계를 뛰어넘는 것이 주요 관심사
- Sum 연산자라는 직접 작성한 질의 연산자를 보면서 이야기를 진행하겠다
- Sum 질의 연산자의 제약사항: 엄청나게 많은 정수의 합을 구하는 Sum 연산자를 수행 시 수가 너무 커지므로 오버플로우가 발생할 가능성이 매우 높다

`Enumerable.Sum(new int[] {int.MaxVAlue, 1});`
- "Arithmetic operation resulted in an overflow"라는 메세지와 함께 OverflowException을 발생시킴
- 문제점: 두 정수의 합이 int (System.Int32) 객체 안에 들어갈 수 없는 매우 큰 숫자라는 것
- 그래서 이것을 발견한 개발자 트로이 마제니스는 int 대신 long을 반환하는 LongSum을 작성하게 되었음

- LongSum 연산자를 새롭게 작성! 이 연산자는 IEnumerable<T>형의 확장 메소드임
- int에 대한 Sum 연산자가 System.Linq.Enumerable 클래스 속에 어떻게 정의되어 있는지 보여줌
- 키워드 `checked`: 명시적으로 적분형 산술 연산, 변환에 대한 오버플로우를 검사하도록 함

- [int에 대한 Sum 연산자의 표준 구현형태]
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    public static class Enumerable
    {
        public static int Sum(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int sum = 0;
            checked
            {
                foreach (int v in source)
                    sum += v;
            }
            return sum;
        }

        public static int? Sum(this IEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            int? sum = 0;
            checked
            {
                foreach (int? v in source)
                    if(v!=null)
                        sum += v;
            }
            return sum;
        }

        public static int Sum<T>(this IEnumerable<T> source,
            Func<T, int> selector)
        {
            return Enumerable.Sum(Enumerable.Select(source, selector));
        }

        private static int? Sum<T>(this IEnumerable<T> source, Func<T, int?> selector)
        {
            return Enumerable.Sum(Enumerable.Select(source, selector);
        }
    }
}

```
- 이 코드에서 확이할 수 있듯, Sum 연산자는 네 개의 오버로딩된 메소드로 구현되어 있음
- 이런 메소드는 LongSum 연산자를 작성하는 데 매우 유용하게 끌어쓸 수 있음
- 다음 코드는 동일한 네 개의 메소드를 구현, 출력을 long 형태로 내보내는 구현방법을 보여줌
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    public static class SumExtensions
    {
        public static long LongSum(this IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            long sum = 0;
            checked
            {
                foreach (int v in source)
                    sum += v;
            }
            return sum;
        }

        public static long? LongSum(this IEnumerable<int?> source)
        {
            if (source == null)
                throw new ArgumentException("source");
            long? sum = 0;
            checked
            {
                foreach (int? v in source)
                    if (v != null)
                        sum += v;
            }
            return sum;
        }

        public static long LongSum<T>(this IEnumerable<T> source, Func<T, int> selector)
        {
            return SumExtensions.LongSum(Enumerable.Select(source, selector));
        }

        public static long? LongSum<T>(this IEnumerable<T> source, Func<T, int?> selector)
        {
            return SumExtensions.LongSum(Enumerable.Select(source, selector));
        }
    }
}
```
- 새로운 LongSum 연산자는 int나 null이 될 수 있는 int의 산술적인 합을 long이나 null이 될 수 있는 long으로 보여줄 것임
- 이것은 기본 Sum 연산자와 비교하여 출력값의 오버플로우로 인한 의외의 상황을 방지하는 데 도움이됨
- 사용자가 직접 질의 연산자를 정의한다는 것은 다른 측면에서 매우 큰 장점이 될 수 있음

### 12.2.2 유틸리티 또는 특정 영역에 대한 질의 연산자를 생성하기 
- 표준 질의 연산자는 유용하고 다양한 상황에서 광범위하게 적용 가능
- 이들 연산자는 어떤 종류의 객체와도 사용 가능하므로 더욱 그럼
- 그러나 실무에서는 객체를 다룰 때 특정 연산자가 별도로 필요한 경우도 있을 수 있음

- 표준 질의 연산자는 일반화된 동작을 하므로 이것도 장점이지만 특화된 처리나 개념이 관여된 경우 큰 도움이 안 될 수도 있음
- 개발자는 스스로 정의한 사용자 정의 질의 연산자를 이용하고 싶을 것임

- 코드 작성 시, 개발자들은 종종 유틸리티 메소드나 헬퍼 메소드를 작성함
- 유틸리티 메소드는 코드를 간결하게 해주고 자주 사용되는 코드를 별도로 관리할 수 있도록 해줌
- LINQ 질의문에서 복잡하지 않게 하기 위해 유틸리티 메소드를 작성하는 일은 매우 유용한 기법임
- 개발자가 Book 객체의 컬렉션을 다루는 메소드를 작성하고자 한다고 가정하자
- 전통적인 형태의 메소드를 작성해서도 이 작업은 가능함
- 최선의 방법:
    - 새로운 질의 연산자를 작성하는 것
    - LINQ에서의 질의라는 것은 원래의 질의 연산자를 호출하는 형태이므로 유틸리티 메소드를 질의 연산자로 작성하면 LINQ에 매우 깔끔하게 접목 가능

- 유틸리티 질의 연산자가 어떤 것인지 느껴보기 위해 몇 개의 예제를 살펴볼 것임
- 소개할 각각의 연산자는 LinqBooks 예제 중 하나의 객체 또는 그것의 컬렉션에 대해 동작함
- 이런 연산자를 "특정 영역에 종속된 질의 연산자"라고 부름

- 일련의 책들에 대해 동작하는 연산자부터 살펴보자

#### IEnumerable<Book>.TotalPrice
- 다음 코드는 Book 객체에 대하여 전체 가격을 합산할 수 있는 연산자를 작성하는 방법을 보여줌
```C#
public static Decimal TotalPrice(this IEnumerable<Book> books)
{
    if (books == null)
        throw new ArgumentNullException("books");

    Decimal result = 0;
    foreach (Book book in books)
        if (book != null)
            result += book.Price;
}
```
- 새로운 TotalPrice 연산자는 질의 표현식에서 다음과 같이 아주 쉽게 사용 가능함
```C#
from publisher in SampleData.Publishers
join book in SampleData.Books
    on publisher equals book.Publisher into pubBooks
select new { Publisher = publisher.Name,
             TotalPrice = pubBooks.TotalPrice()};
```
- 이 질의에서 사용가능한 유틸리티 메소드를 작성한다는 측면에서 매우 유용함

#### IEnumerable<Book>.Min
- 표준 질의 연산자에서 제공되는 Min 연산자는 오직 수치값에만 사용 가능
- 다음 코드에서는 확장 메소드는 일련의 Book 객체에 대해 동작하며 페이지가 가장 적은 책을 반환하도록 구현되어 있음
- [Min 사용자 정의 질의 연산자]
```C#
public static Book Min(this IEnumerable<Book> source)
{
    if (source == null)
        throw new ArgumentNullException("source");

    Book result = null;
    foreach (Book book in source)
    {
        if ((result == null) || (book.PageCount < result.PageCount))
            result = book;
    }
    return result;
}
```
- 이런 사용자 질의 연산자로는 다음과 같은 코드를 작성 가능함
```C#
Book minBook = SampleData.Min();
Console.WriteLine(
    "Book with the lowest number of pages = {0} ({1} pages)",
    minBook.Title, minBook.PageCount);
```
- 이 예는 어떻게 특정 영역에 속한 객체들을 다룰 때, 표준 연산자가 다루고 있는 Min과 같은 개념을 적용할 수 있는지 보여줌
#### Publisher.Books
- 개발자는 코드를 간결하게 해주고 복잡함을 숨겨주는 임의의 확장 메소드를 구현할 수 잇음
- 다음 질의에서 join문을 이용하여 각각의 출판사의 책들을 구분해내는 경우를 생각해보자
```C#
from publisher in SampleData.Publishers
join book in SampleData.Books
    on publisher equals book.Publisher into books

select new {
    Publisher = publisher.Name,
    TotalPrice = books.TotalPrice()
};
```
- 사용자가 특정 출판사의 책을 확인하려고 할 때, 내부적으로는 비슷한 유형의 join 문을 매번 호출할 가능성이 높음
- 이런 반복적인 연산을 수행하기 위해 유틸리티 질의 연산자(utility query operator)를 매번 생성하는 것은 매우 유용하고 효과적일 것임
- 다음 코드의 연산자는 일련의 책 중에서 특정 출판사의 책들을 골라냄
- [Books 사용자 정의 질의 연산자]
```C#
static public IEnumerable<Book> Books(this Publisher publisher, IEnumerable<Book> books)
{
    return books.Where(book => book.Publisher == publihser);
}
```
- 이 새로운 Books 연산자는 기존 질의 표현식을 다음과 같이 간소하게 해줌
```C#
from publisher in SampleData.Publishers
select new{
    Publisher = publisher.Name,
    TotalPrice = publisher.Books(SampleData.Books).TotalPrice()
};
```

#### Book.IsExpensive
- 이 절의 마지막 연산자는 어떤 특정 개념을 단 한 번만 구현하여 뛰어난 재사용성을 바탕으로 간편하게 이후 코드에 적용 가능한 방법을 보여줄 것임 
- 다음 예제의 연산자는 어떤 책을 매개변수로 받아서 그 책이 비싼지 여부를 반환함
- [IsExpensive 사용자 질의 연산자]
```C#
public static Boolean IsExpensive(this Book book)
{
    if(book == null)
        throw new ArgumentNullException("book");
    
    return (book.Price > 50) ||
            ((book.Price / book.PageCount) > 0.10M);
            //책이 비싸다 = 절대적인 가격이 높거나 페이지 수에 비해 상대적으로 가격이 높은 경우임
}
```
- 예제에서 구현된 IsExpensive 연산자는 어떤 책이 비싼지 여부를 판단 시마다 LINQ 질의문 속에서 이용 가능(다음 예시에서 이 연산자를 이용해보겠음)
```C#
var books =
    from book in SampleData.Books
    group book.Title by book.IsExpensive() into bookGroup
    select new { Expensive = bookGroup.Key, Books = bookGroup };
ObjectDumper.Write(books, 1);
```
- IsExpensive 와 같은 연산자를 작성하여 사용하면 질의식에 표현해야 할 장황한 내용들을 추상화해서 간소화
- 앞에서 설명한 연산자들은 LINQ 질의 속에서 사용가능하지만 점을 이용해서 호출하는 방법밖에 없음
- 아주 적은 수의 질의 연산자만이 질의 표현식 구문에 따라 암시적으로 사용 가능

## 12.3 표준 질의 연산자의 사용자 정의 구현형태
- 사용자가 직접 만든 질의 연산자를 사용하는 방법을 설명할 때, 명시적 점 표현법을 이용함 
- [Books와 TotalPrice를 사용하는 질의식]
```C#
from publisher in SampleData.Publishers
where publisher.Name.StartsWith("A")
select new {
    Publisher = publisher.Name,
    TotalPrice = publisher.Books(SampleData.Books).TotalPrice()
};
```
- 기본적으로 질의 표현식의 이런 구문은 표준 질의 연산자로 변환됨
- 여기서는 Where와 Select가 질의 표현식 내부에서 호출되더라도 사용자가 구현한대로 동작할 수 있게 하는 방법을 살펴볼 것
- 컴파일러가 질의 표현식을 처리하면서 질의 연산자들을 다루기 떄문에 구현된 표준 질의 연산자가 어떻게 사용될 것인지 정의 가능

- 질의 표현식이 어떻게 메소드 호출로 변환되는지 알아보자
- 질의 표현식을 먼저 잘 이해하자!
- 질의 변환 매커니즘의 기초를 알고 나면 질의 표현식 패턴이 구현된 예를 살펴볼 것임!

### 12.3.1 질의 변환 메커니즘에 대한 생각
- 컴파일러가 어떻게 질의 표현식을 메소드에 대한 호출로 변환시키는가?
- 표준 질의 연산자를 사용자가 재구현하는 것의 기본이 되는 개념!
```C#
using System;
using LinqAction.LinqBooks.Common;

static class TestCustomImplementation
{
    static void Main()
    {
        var books =
            from book in SampleData.Books
            where book.Price < 30
            select book.Title;

        ObjectDumper.Write(books);
    }
}
```
- 질의에 의해 실제로 수행되는 코드는 사용자가 불러들이는 데 사용하는 인터페이스에 의존적임
- 질의 연산자들이 확장 메소드이므로 네임스페이스를 통해 참조되어 있음
- 컴파일러가 질의 표현식을 확장 메소드에 대한 호출로 변환시켜줌

- 컴파일러가 수행하는 작업: Where와 Select 메소드가 어디서 왔는지 규명해내는 것
- 만약 System.Linq를 참조한다면 컴파일러는 Where과 Select 확장 메소드를 System.Linq.Enumerable 클래스에서 찾으려고 함
- 만약 System.Linq를 참조하지 않고 스스로 작성한 Where과 Select를 구현하는 네임스페이스를 불러들인다면 코드는 다르게 변환될 것임
```C#
var query =
    System.Linq.Enumerable.Select(
        System.LinqEnumerable.Where(
            SampleData.Books,
            book => book.Price < 30),
    book => book.Title);
```
- 같은 질의식을 다음과 같이 변환할 수도 있음
```C#
var query = 
    MyNamespace.MyExtensions.Select(
        MyNamespace.MyExtensions.Where(
            SampleData.Books,
            book => book.Price < 30),
    book => book.Title)
```
### 12.3.2 질의 표현식 패턴의 기능상 정의 
- C# 3.0 기능 정의 문서는 질의 표현식을 완벽히 지원하기 위해 어떤 연산자를 어떻게 구현해야 하는지 설명하고 있음 
- 질의 표현식은 문법적 매핑(syntatic mapping)이라는 방법으로 메소드 호출로 변환됨 -> 질의 표현식을 구현할 떄 형은 매우 탄력적으로 사용 가능
- 몇 가지 알아야 할 사항들:
    - 제네릭형은 매개변수와 결과형 간의 관계를 잘 표현하기 위해 질의 표현식 패턴내에서 사용됨. 그러나 비제네릭형에 대해서도 해당 패턴을 사용하는 것이 불가능하지 않음
    - 3장과 4장에서 설명한 표준 질의 연산자들은 IEnumerable<T>를 구현하는 어떤 형에도 대응 가능한 연산자들임. 비록 LINQ to Objects와 LINQ to XML에 표준 질의 연산자를 주로 활용하지만 IEnumerable<T>는 패턴의 일부가 아님, 이것은 LINQ가 열거형이나 시퀀스를 제외한 나머지 형들에도 활용이 가능하다는 것임 
    - 표준 질의 연산자들은 확장 메소드의 형태로 구현되어 있찌만 패턴의 메소드들은 확장 메소드 또는 인스턴스 메소드로 구현될 수 있음(호출 방법이 일치하기 때문!)
    - 메소드들은 대리자나 표현식 트리를 매개변수로 받아들일 수 있음
- 어떻게 LINQ 질의 표현식 패턴을 구현하는 것이 가능한지 살펴보기 위해 다음 사항을 설명할 것임
    - 제네릭과 비제네릭 형태로 구현된 예제
    - IEnumerable<T>에 대해 동작하는 연산자와 다른 종류의 객체에 대해 동작하는 연산자를 만드는 방법
    - 대리자를 받아들이는 연산자와 표현식 트리를 받아오는 연산자
    - 확장 메소드로 구현된 연산자와 인스턴스 메소드로 구현된 연산자
    - 모든 것을 간단히 하기 위해 Where과 Select에 대해서만 구현

### 12.3.3 예제 1: 표준 질의 연산자의 동작을 추적하기
- Where와 Select 연산자를 직접 작성하여 사용자 정의 연산자를 구현해볼 것임
- 구현할 메소드들은 단순히 표준의 Enumerable.Where와 Enumerable.Select에 대리자(delegate)를 연결해주는 역할만을 담당할 겅ㅁ
- 다음 예제는 CustomImplementation이라는 클래스에 구현된 두 가지 연산자를 보여줌
```C#
using System;
using System.Collections.Generic;
using System.Linq;

public static class CustomImplementation
{
    public static IEnumerable<TSource> Where<TSource>(
            this IEnumerable<TSource> source, Func<TSource, Boolean> predicate)
    {
        Console.WriteLine("in CustomImplementation.Where<TSource>");
        return Enumerable.Where(source, predicate);
    }

    public static IEnumerable<TResult> Select<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector)
    {
        Console.WriteLine(
            "in CustomImplementation.Select<TSource, TResult>");
        return Enumerable.Select(source, selector);
    }
}
```
- 새로 구현된 두 연산자를 이용해보려면 이 클래스의 네임스페이스를 System.Linq 네임스페이스 대신 사용하면 됨
- 방금 설명한 매커니즘의 한계를 다음 절에서 설명하겠다

### 12.3.4 제한: 질의 표현식 간의 충돌
- 질의 표현식 패턴 구현시 염두에 두어야 하는 매우 중요한 제약: 
    - 구현물의 시그니처가 동일할 때는 직접 구현한 연산자 이외의 기본 연산자들과 혼용할 수 없음
    - 확장 메소드에 대한 호출이 이루어지는 방법으로 인한 제약임

- 질의 표현식을 수정해서 결과를 정렬하는 경우
```C#
var query =
    from Book book in SampleData.Books
    where book.Price < 30
    orderby book.Title
    select book.Title;
```
- OrderBy라는 새로운 질의 연산자가 사용됨
- 현재 Where과 Select만을 구현한 상태이므로 컴파일러는 OrderBy가 구현된 것을 찾을 수 없다고 에러 메세지를 반환함
- 일반적으로 이런 상황에서는 표준 구현형태를 재사용해서 기본적인 동작을 그대로 수행하게 해주는 선택을 할 것
- 이를 위해 System.Linq 네임스페이스를 예시 코드에 이미 정의한 네임스페이스와 함께 불러들여야 함
- 이런 시도를 할 경우, 컴파일러가 Wehre와 Select를 구현한 두 네임스페이스 중 어떤 네임스페이스의 구현형태를 따라야 할 지 잘 모르겠다고 하면서 에러 메세지를 반환할 수도 있음
- 이런 상황 = **네임스페이스 충돌**
    - 확장 메소드가 호출되는 순서는 하나의 시그너처를 가지고 하나의 범주 속에서 여러 가지 확장 메소드를 다루는 것을 매우 힘들게 함
- 그러나...이런 불명확함을 쉽게 해소할 수 있는 방법은 없음
    - 하나의 파일 내에서 스스로 작성한 연산자만을 사용하는 것
    - 또는 System.Linq에서 받아온 기본 연산자만을 사용하는 것
    - 위의 둘 중 하나를 강요받게 됨
    - 다른 옵션: 예시 코드의 연산자를 IEnumerable<T> 같이 좀더 보편적인 객체들이 아닌 IEnumerable<Book>과 같이 좀 더 제한적인 범위 내에서 사용될 수 있도록 하는 것(그러나 이 경우 각각의 형마다 매번 연산자를 작성해줘야 함 -> 부자연스럽고 귀찮다)
- 이 Func 대리자형에 접근하기 위해 System.Linq 네임스페이스를 불러들이게 되면 대신 System.Linq.Enumerable 클래스와의 충돌로 인해 직접 작성한 표준 질의 연산자를 사용할 수 없게 될 것임

### 12.3.5 예제 2: 비제네릭인 특정 영역에 대한 연산자
- 여기서는 표준 질의 연산자들의 사용자 정의 구현형태를 작성해서 특정 영역을 대상으로 하는 질의 연산자들을 바탕으로 질의 표현식 패턴을 활용할 수 있다는 것을 설명하고 있음
- IEnumerable<T>의 확장 메소드를 작성하는 대신 IEnumerable<Book>에 대한 확장 메소드를 작성가능함
- 다른 형에 대해서는 원래 구현된 대로 질의 연산자를 사용, Book 객체에 대해서만 사용자 정의 구현형태를 이용할 수 있도록 함

- Book 객체에 대해 동작하는 Where와 Select 연산자를 구현할 것임
- 앞에서 봤던 제네릭 구현형태를 첫 번째 예제로 삼아서 Book 객체에 해당하는 연산자가 각각의 책을 처리하면서 제목을 출력하게 할 것임
- [특정 영역에 대해 구현된 Where과 Select]
```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyLINQ_ch12
{
    static class DomainSpecificOperators
    {
        public static IEnumerable<Book> Where(
            this IEnumerable<Book> source,
            Func<Book, Boolean> predicate)
        {
            foreach (Book book in source)
            {
                Console.WriteLine(
                    "processing book \"{0}\" in "+
                    "DomainSpecificOperators.Where",
                    book.Title);
                if (predicate(book))
                    yield return book;
            }
        }

        public static IEnumerable<TResult> Select<TResult>(
            this IEnumerable<Book> source, Func<Book, TResult> selector)
        {
            foreach (Book book in source)
            {
                Console.WriteLine(
                    "processing book \"{0}\" in " +
                    "DomainSpecificOperators.Select<TResult>",
                    book.Title);
                yield return selector(book);
            }
        }
    }
}
```

- 이런 예제를 재사용해보자
```C#
static public void Main()
        {
            var books =
                from book in SampleData.Books
                where book.Price < 30
                select book.Title;

            foreach (string book in books)
            {
                Console.WriteLine(book);
            }
            Console.ReadKey();
        }
```
- ![](cap1.PNG)
- 이런 결과를 출력함
- 특정 영역에 대한 연산자는 어떤 형에 대해 자신들이 이 작업을 하는지 이미 알고 있음
- 에제처럼 Title과 같은 특정 멤버에게 명시적으로 접근 가능하게 함

- 이전에 언급했던 제약점은 이런 연산자에 대해서는 존재하지 않음
- 특정 영역에 대한 연산자는 물론 원래 구현된 다른 연산자와 함께 사용 가능함

- 이번 경우에는 orderby 절을 질의 내에서 사용 가능
- OrderBy 연산자의 사용자 정의 구현형태를 작성할 필요는 없고 기본적으로 제공되는 연산자를 혼용하여 사용 가능함
```C#
var query =
    from Book book in SampleData.Books
    where book.Price < 30
    orderby book.Title
    select book.Title;
```
- 이런 형태로 혼용하여 사용하려면 사용자 정의 연산자의 네임스페이스와 기본 네임스페이스인 System.Linq 네임스페이스를 함께 참조해야 함

### 12.3.6 예제 3: 비시퀀스 연산자
- 어떻게 개체의 객체들을 질의 속에 포함시킬 수 있는지 설명함
- 표준 질의 연산자들은 IEnumerable<T>에 해당하는 질의 연산자 패턴의 구현형태를 제공함
- 개발자가 SampleData.Books와 같은 Book 객체의 배열과 같은 컬렉션들에 대해 질의할 수 있게 해줌
- 만약 사용자가 객체의 시퀀스가 아닌 하나의 객체를 가지고 작업하고 싶다면 어떻게 해야 하나?

- 다음 질의에서는 특정 Publisher 객체에 작업한 후, Publisher 객체의 시퀀스에 대해 사용하는 것처럼 쓸 수 있음
```C#
from publisher in SampleData.Publishers[0]
join book in SampleData.Books
    on publisher equals book.Publisher into books
select new { Publisher = publisher.Name, Books = books};
```
- 그럴싸하지만 이 상태로는 표준 질의 연산자와 함께 동작 못함 
- 그것은 표준 질의 연산자들이 IEnumerable<T>와만 동작하도록 설계되었기 때문임
- 이 경우에는 컴파일러가 Publisher 형에 대해 GroupJoin을 할 수 없다는 점이 문제임

- Join 연산이 사용되기 때문에 호출되는 GroupJoin 연산자는 다음과 같이 정의되어 있음
```C#
public static IEnumerable<TResult>
    GroupJoin<TOuter, TInner, TKey, TResult>(
        this IEnumerable<TOuter> outer,
        IEnumerable<TInner> inner, 
        Func<TOuter, TKey> outerKeySelector,
        Func<TOuter, TKey> innerKeySelector,
        Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
```
- 매개변수 outer가 시퀀스 형태(IEnumerable<TOuter>)로 정의된 것을 알 수 있음
- 컴파일러가 불만을 갖지 않게 하기 위해서는 GroupJoin을 새롭게 구현해서 하나의 개체를 시퀀스 대신 매개변수로 받아들이는 형태로 변형시키면 됨
```C#
public static IEnumerable<TResult>
    GroupJoin<TOuter, TInner, TKey, TResult>(
    this TOuter outer,
    IEnumerable<TInner> inner,
    Func<TOuter, TKey> outerKeySelector,
    Func<TOuter, TKey> innerKeySelector,
    Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
{
    ILookup<TKey, TInner> lookup =
        inner.ToLookup(innerKeySelector);
    yield return resultSelector(outer,
                                lookup[outerKeySelector(outer)]);
}
```
- 단순히 첫 번째 매개변수의 형을 바꾸고 코드를 하나의 객체를 다루는 형태로 적절히 수정


- LINQ to Amazon은 그냥 읽어보자..!