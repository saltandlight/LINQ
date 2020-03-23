# Chapter04. LINQ to Objects와 친해지기

## 4.1 앞으로 사용하게 될 예제 코드에 대한 설명
### 4.1.1 목표
- **예제를 활용하기 위해 기본적으로 요구되는 사항들:**
    - 다양한 LINQ 질의를 받아들이기 위해 객체 모델은 매우 잘 구성되어 있어야 함
    - 예제 코드는 메모리 내의 객체와 XML 문서, 관계형 데이터 등을 독립적으로나 조합하여 다룰 수 있어야 함
    - 예제 코드는 Windows Forms 애플리케이션뿐만 아니라 ASP.NET 웹 어플리케이션도 포함해야 함
    - 예제 코드는 로컬 환경의 데이터 저장소뿐만 아닌 공개 웹 서비스와 같은 외부의 원격지 데이터 저장소에 대해서도 질의할 수 있어야 함
    
### 4.1.2 구현할 기능
- LinqBooks의 주요 기능은 다음과 같은 것들을 포함해야 함
    - 어떤 책을 소장하고 있는지
    - 그런 책에 대해서 생각하는 바를 기록할 수 있어야 함
    - 책에 대해 더 많은 정보를 가져올 수 있어야 함
    - 책목록과 서평목록을 출력해줄 수 있어야 함
- 이 책에서 구현할 기술적인 기능에 포함되어야 하는 사항
    - 로컬 데이터베이스에 대해 질의하고, 삽입하고, 업데이트하는 기능
    - 로컬 저장소의 카탈로그 또는 서드파티의 데이터에도 검색기능을 제공하는 기능
    - 웹 사이트에서 책에 대한 정보를 불러 읽어들이는 기능
    - XML 문서에서 데이터를 불러오고 저장하는 기능
    - 추천하는 책으로 RSS 피드를 작성하는 기능
- 이런 기능들을 구현하기 위해 하나의 그룹의 비즈니스 개체들을 이용할 것임

### 4.1.3 비즈니스 개체
- 사용하게 될 객체 모델은 다음과 같은 클래스들로 이루어져 있음
Book, Author, Publisher, Subject, Review, User

### 4.1.4 데이터베이스 스키마
- 데이터베이스를 사용하여 애플리케이션들이 다루고 있는 정보를 저장하고 읽어올 것

### 4.1.5 예시 데이터
- LINQ to Objects를 시연하기 위해 메모리 내의 데이터를 이용할 것임
```C#

```
## 4.2 LINQ를 메모리 내의 컬렉션과 함께 사용하기
- LINQ to Objects는 메모리 내의 객체 컬렉션에 대해 동작하는 LINQ 변종
- 이 아이가 지원하는 컬렉션에는 어떤 것들이 있는가?
- 이런 컬렉션들에 대해 어떤 연산을 가할 수 있는가?

### 4.2.1 어떤 것들에 대해 질의할 수 있는가?
- LINQ to Objects가 모든 것을 대상으로 할 수 있는 것은 아님
- LINQ를 통한 질의가 가능하기 위한 첫 번째 전제조건: 대상 객체가 컬렉션이어야 함
- 컬렉션이 LINQ to Objects로 질의가 가능하기 위해서는 IEnumerable<T> 인터페이스를 구현하기만 함녀 됨
- **시퀀스**: 
    - LINQ의 세계에서 IEnumerable<T>를 구현하는 객체들
    
##### 배열

- 모든 종류의 배열을 지원하고 있음

- 형이 정해지지 않은 배열을 LINQ to Objects를 이용하여 질의하기

  ```C#
  using System;
  using System.Linq;
  
  static class TestArray
  {
      static void Main()
      {
          Object[] array = {"String", 12, true, 'a'};
          var types = 
              array
              	.Select(item => item.GetType().Name)
              	.OrderBy(type => type);
          ObjectDumper.Write(types);
      }
  }
  ```

  - 결과물

    ```C#
    Boolean
    Char
    Int32
    String
    ```

- 질의들은 사용자 정의 객체의 배열에 대해서도 수행 가능함

- Book 객체의 배열에 대한 질의를 수행

- 형을 가진 배열에 대해 LINQ to Objects 로 질의하기

  ```C#
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using LinqInAction.LinqBooks.Common;
  
  static class TestArray
  {
      static void Main()
      {
          Book[] books = 
          {
              new Book{Title = "LINQ in Action"},
              new Book{Title="LINQ for Fun"},
              new Book{Title = "Extreme LINQ"}
          };
          
          var titles = 
              books
              .Where(book => book.Title.Contains("Action"))
              .Select(book => book.Title);
          
          Object.Dumper.Write(titles);
      }
  }
  ```

- LINQ to Objects 질의는 어떤 데이터형의 질의에도 사용 가능함

##### 제너릭 리스트

- LINQ to Objects 는 List<T>를 비롯하여 다른 제네릭 리스트형에도 적용이 가능함

- 주요 제너릭 리스트형의 목록

  - System.Collections.Generic.List<T>
  - System.Collections.Generic.LinkedList<T>
  - System.Collections.Generic.Queue<T>
  - System.Collections.Generic.Stack<T>
  - System.Collections.Generic.HashSet<T>
  - System.Collections.ObjectModel.Collection<T>
  - System.ComponentModel.BindingList<T>

- 제너릭 리스트에 대해 LINQ to Objects로 질의하기 

  ```C#
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using LinqInAction.LinqBooks.Common;
  
  static class TestList
  {
      static void Main()
      {
          List<Book> books = new List<Book>(){
              new Book {Title = "LINQ in Action"},
              new Book {Title = "LINQ for Fun"},
              new Book {Title = "Extreme LINQ"}
          };
          
          var titles = 
              books
              .Where(book => book.Title.Contains("Action"))
              .Select(book => book.Title);
          
          ObjectDumper.Write(titles);
      }
  }
  ```

- 배열과 리스트 

##### 제너릭 딕셔너리

##### 문자열

##### 기타 컬렉션 객체

### 4.2.2 지원되는 여러 가지 연산

## 4.3 LINQ를 ASP.NET과 Windows Forms와 함께 바인딩하기
### 4.3.1 웹 어플리케이션에서의 데이터 바인딩
### 4.3.2 Windows Forms 애플리케이션을 위한 데이터 바인딩

## 4.4 주요 표준 질의 연산자에 대하여 알아보기
### 4.4.1 제한 연산자 "Where"
### 4.4.2 사영 연산자 이용하기
### 4.4.3 Distinct 이용하기
### 4.4.4 변환 연산자 이용하기
### 4.4.5 누적 연산자 이용하기

## 4.5 메모리 내의 객체 그래프에 대한 뷰 작성하기
### 4.5.1 정렬
### 4.5.2 중첩 질의
### 4.5.3 그룹화하기
### 4.5.4 조인 사용하기
### 4.5.5 분할
