# Chapter07. LINQ to SQL의 내부 매커니즘을 이해하기🙊
- **알아볼 내용들:**
    - LINQ가 어떻게 내부적으로 동작하는 지 살펴볼 것
    - 매핑 옵션들에 대해 다룰 것
    - 어떻게 Visual Studio에 포함되어 있는 도구들을 이용하여 쉽게 매핑할 수 있는지
    - LINQ to Objects와 LINQ to SQL의 차이를 알아볼 것
    - 객체의 생명주기
    - 어떻게 LINQ to SQL이 업데이트가 일어나기 전까지 객체드르이 변화를 관리해주는 지 
## 7.1 객체를 관계형 데이터와 매핑하기
- 프레임워크는 한 번 설정해준 선언적인 질의구조를 데이터베이스가 이해할 수 있는 형태의 SQL로 변환해줌
- 업무상의 요구를 수정하는 과정에서 개발자는 완전히 다른 데이터 계층을 구축할 필요 없음
- 프레임워크가 개발자가 설정하는 매핑에 맞추어 언어상의 변환들을 관리해줌

- 처음에 매핑을 다루면서 어떻게 수동으로 값들을 매핑할 수 있는지 살펴보았음
- 속성을 기반으로 하는 매핑은 매핑을 직접 명시하는 방법을 제시해줌
- 속성을 이용하여 명시적으로 매핑을 설정하는 방법 외에 Visual Studio는 세 가지 데이터 매핑 방법을 제공함
- 매핑 옵션들
    - 클래스내에서 내부적으로 선언된 속성
    - 외부 XML 파일
    - 커맨드라인에서 동작하는 SqlMetal 도구
    - GUI를 활용하는 LINQ to SQL 디자이너 도구
- 이 절에서는 위에 제시한 옵션을 하나씩 보면서 각각의 옵션이 가진 특징적인 점들에 대해 알아볼 것임
- 앞에서 사용했던 속성들이 제공하는 더 많은 기능을 구현하는 방법에 대해 알아보자

### 7.1.1 인라인 속성 사용하기
- Table 속성을 이용하여 어떻게 객체가 DB 테이블과 매핑이 되는지 설명 가능함

- 속성들은 Column 속성이나 Association 속성에 의해 치장 가능함

- Column 속성은 어떻게 각각의 속성이 테이블의 열과 매핑되는지 지정해 줌

- Association 속성은 어떻게 외래키 관계로 테이블 간의 상호관계가 엮어져 있는지 지정해줌

- [데이터베이스를 객체에 매핑하기 위해 사용되는 System.Data.Linq.Mapping이 제공하는 사용자 정의 속성]

  | 속성               | 설명                                                         |
  | ------------------ | ------------------------------------------------------------ |
  | Assocation         | 클래스 간의 기본키와 외래키 관계를 설정                      |
  | Column             | 지정한 프로퍼티나 항목과 DB 테이블 내의 열을 매핑해줌        |
  | Database           | 매핑 메타데이터에서 CreateDatabase 가 사용하는 데이터베이스 이름을 지정해줌 |
  | Function           | 사용자 정의 함수 또는 저장된 프로시저를 메소드에  매핑하기 위해 사용됨 |
  | InheritanceMapping | 다형성 객체를 매핑할 때 사용됨                               |
  | Parameter          | 저장된 프로시저나 함수의 매개변수를 지정해줌                 |
  | Provider           | 질의가 수행되는 대상형태를 지정해주는 데 사용함. LINQ to SQL은 현재 SQL Server만을 지원함 |
  | ResultType         | 저장된 프로시저나 함수에서 반환된 결과 객체의 형을 나타냄    |
  | Table              | 클래스에 매핑하고 싶은 테이블의 이름을 지정함                |

- 저장된 프로시저, 함수 및 상속에 대해 알아볼 것

####  Table 속성
- 테이블과 객체의 간극을 이어주기 위한 시발점이 됨
- 클래스가 특정 테이블을 대표하는 구조체라는 것을 명시하지 않으면 클래스가 어떤 테이블과 연관되었는지 알 수 있는 방법이 없음 -> 설정하는 속성값들이 무용지물이 되었을 것
- Author라는 이름의 클래스가 Author 테이블에 연결된다는 것을 밝히기 위해 해당 클래스를 Table이라는 속성으로 치장하게 됨
  ```C#
  [Table()]
  public class Author
  ```
- Table 속성은 가장 중요하고 가장 간단함
- 기본적으로 클래스를 Table 속성으로 치장하는 것만으로도 클래스 이름이 테이블의 이름과 동일하다는 것을 알릴 수 있음
- 개발자는 Name 매개변수를 이용해서 데이터베이스 내의 테이블들의 이름을 지정해야 함
    - 예) 만약 Author 클래스를 Authors라는 테이블에서 데이터를 가져오도록 수정하고 싶다면 Name 매개변수를 받아들일 수 있도록 속성을 수정해야 함
    `[Table(Name = "dbo.Authors")]`
#### Column 속성
- 가장 자주 이용되는 속성은 Column 속성임

- 이 속성은 DB의 열들을 클래스 프로퍼티에 연결해줌

- 열을 프로퍼티에 같은 이름으로 매핑할 때에는 어떤 매개변수값을 지정해주지 않고도 프로퍼티를 Column 속성으로 치장 가능(default)

- 대부분 다음의 표에 있는 매개변수를 설정해서 매핑을 추가하게 될 것임(추가적 속성)

- [Column 속성이 사용하는 매개변수들의 목록]

  | 매개변수 이름   | 설명                                                         |
  | --------------- | ------------------------------------------------------------ |
  | AutoSync        | Create 또는 Update 메소드의 결과로 수정되는 DB 내이 열들을 어떻게 LINQ to SQL이 다루어야 하는지 보여주는 값.이 값은 기본값이 있는 열에 있어 특히 유용함. 설정 가능한 옵션: Default, Always, Never, OnInsert, OnUpdate |
  | CanBeNull       | DB의 해당 열이 null이 될 수 있음을 나타냄 Null이 빈 문자열 객체와 다르다는 것을 반드시 기억해두어야 함 |
  | DbType          | 이 값은 DataContext.CreateDatabase  메소드를 이용하여 열을 생성 시 사용되는 DB형을 명시해주기 위해 사용됨. 적절한 용례는 NVarChar(50) Not NullDefault('')와 같음 |
  | Expression      | 이 값은 CreateDatabase 메소드를 이용해서 데이터베이스를 생성 시에만 사용됨 여기서 포함된 값은 DB에서 계산된 항목을 생성하는지의 여부를 지정해주는 SQL 문자열임 |
  | IsDbGenerated   | 이 프로퍼티의 값이 데이터베이스가 생성해주는 값인지를 지정해줌. 이 매개변수는 데이터베이스의 Identity 또는 AutoNumber열에 사용됨. 이 값은 레코드가 SubmitChanges 메소드를 이용하여 데이터베이스에서 업데이트되는 순간 설정될 것임 |
  | IsDiscriminator | 이 값을 이용하여 특정 열이 해당 행에 사용될 수 있는 특정한 인스턴스형을 지정하도록 함 |
  | IsPrimaryKey    | 사용자의 테이블에서 하나의 행을 구분시켜 줄 수 있는 고유정보를 담고 있는 열에 이 값을 설정해줘야 함 테이블의 기본키임. LINQ to SQL은 최소한 하나의 열이 기본키로 설정되어 있어서 객체 정체성 기능과 변화 추적기능에 사용되기를 원함. 여러 개의 열로 구성된 키의 경우, 이것을 각각의 Column 속성에서 지정해줘야 함 |
  | IsVersion       | 레코드의 타임스탬프 또는 버전정보에 해당하는 열들에 이 값을 설정해줘야 함 이 값은 행이 변할 때마다 업데이트되어야 함. 그리고 낙관적 동기화에 매우 유용하게 사용됨 |
  | Name            | 매핑하고 싶은 대상 테이블의 열 이름을 설정해줌               |
  | Storage         | 열을 클래스 내부 항목에 공용 프로퍼티 설정자를 사용하지 않고 바로 매핑하기 위해 Storage 매개변수로 항목의 이름을 지정해줌 |
  | UpdateCheck     | 낙관적 동기화 모델을 추종할 때 LINQ to SQL이 어떻게 이 열을 이용할지를 설정해줌. 기본적으로 모든 클래스 내의 매핑된 열은 동기화를 평가할 때 사용될 것임. 타임스탬프나 다른 방법을 이용하여 동기화를 관리한다면 이 매개변수를 이용해서 업데이트와 삭제 메서드를 최적화 가능. 이 매개변수는 다음과 같은 옵션의 열거된 값들을 받을 수 있음(바로 밑에 설명) |

  - Always(defualt): 항상 이 열을 체크할 것
  - Never: 절대 이 열을 체크하지 않을 것
  - WhenChanged:프로퍼티의 값이 변했을 때만 이 열을 체크할 것

  - 모르는 용어:
    - Identity: 테이블에 ID열을 만듦, CREATE TABLE 및 ALTER TABLE Transact-SQL에 사용됨
    - 낙관적 동기화: 정상적이지 않은 사건의 진행으로 생기는 영향을 제거하는 복구 매커니즘을 사용하는 매커니즘

- 이러한 속성들을 이용하면 Author 클래스를 DB 내의 상응하는 테이블과 매핑 가능
- 모든 EntitySet 컬렉션은 ID라는 이름의 고유한 Guid를 이용할 것임
  
  - Guid: 전역 고유 식별자, 응용 소프트웨어에서 사용되는 유사 난수
- Public 속성을 설정하는 메소드를 이용하는 대신 해당 값을 Storage 매개변수를 통해 직접 내부 _ID 항목에 저장하고 싶다고 명시함
- 명료함을 위해 열의 이름이 ID임을 Name 매개변수를 이용하여 명시함
- DB를 동적으로 생성하는 경우를 위해 해당 열의 DbType을 UniqueIdentifier NOT NULL로 설정함
- 이 열에서 가장 중요한 매개변수는 IsPrimaryKey임
- 이 매개변수는 각각의 클래스의 프로퍼티에 대해 최소 하나 이상 설정되어 있어야 함
- ID열의 마지막 매개변수는 CanBeNull이며 해당 속성에 값이 필요하다는 것을 의미함
- 값이 할당되지 않는다면 런타임에서 예외상황이 발생할 것임
```C#
private System.Guid _ID;
[Column(Storage = "_ID", Name="ID",
DbType="UniqueIdentifier NOT NULL",
IsPrimaryKey = true, CanBeNull = false)]
public System.Guid ID {get {return ID;} set{ _ID = value;}}
```
- 다음의 세 개의 열도 비슷함
- 각각의 경우에 속성에 대응되는 열의 이름은 지정해줘야 함. 각각의 열의 형은 VarChar임
- FirstName과 LastName은 선택사항(NOTNULL이 설정되어 있고 CanBeNull = false임)
- 다른 열은 DB에 null값을 넣을 수 있게 되어 있음
- 만약 둘 중의 어떤 열이라도 문자열이 아닌 값을 가지고 있다면, 이 속성들에 대해 .NET Frameowrk 2.0에서 도입된 null을 대입할 수 있는 형을 이용해야 할 것임
```C#
[Column(Name="LastName", DbType="VarChar(50) NOT NULL",
CanBeNull = false, UpdateCheck=UpdateCheck.Never)]
public string LastName{get;set;}
//얘는 null이 될 수 없는 Varchar임, 절대 체크하지 않는 열

[Column(Name="FirstName", DbType="VarChar(30) NOT NULL",
CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
public string FirstName{get; set;}
//얘도 null이 될 수 없는 Varchar임, 절대 체크하지 않는 열

[Column(Name="WebSite", DbType="VarChar(200)",
UpdateCheck = UpdateCheck.Never)]
public string WebSite {get; set;}
//얘는 null일 수도 있고 아닐 수도 있는 Varchar, 절대 체크하지 않는 열
```
- 이 세 개의 열은 모두 매개변수화된 명령을 포함하여 최종 타임스탬프 열이라는 특수 기능을 통해 업데이트를 확인함
- SQL Server를 이용하면 매번 레코드가 수정될 때마다 TimeStamp 열의 내용이 바뀌게 됨
- 이런 기능은 IsDbGenerated 매개변수를 true로 설정하여 동작 가능
- 이 열의 행이 바뀔 때마다 체크되도록 IsVersion 속성을 설정해주고, 해당 값이 필수라는 것을 CanBeNull = false를 사용하여 명시해야 함
```C#
[Column(Name="TimeStamp", DbType="rowversion NOT NULL", 
  IsDbGenerated = true, IsVersion =true, CanBeNull=false,
  UpdateCheck=UpdateCheck.Always)]
public byte[] TimeStamp {get; set;}
```
- DB는 각 행이 수정될 때마다 타임스탬프를 저장함 -> 다른 열에 대한 변화에 신경쓸 필요가 없음
- ID와 타임스탬프의 이전 값을 조합함 -> 작업하는 동안 충돌문제를 야기하지 않을지 확인함
- 나머지 속성은 동기화 체크에 필요하지 않음 -> 업데이트 시에 값을 확인할 필요가 절대 없다고 자신 있게 설정 가능(UpdateCheck.Never)

- 이런 매핑들이 다 갖춰졌으면, 수정된 Author 클래스에 대해 표준적인 질의를 날려볼 준비가 다 된 것임
- 만약 작가와 책 정보를 AuthorBooks 테이블을 이용하여 조인하고 싶다면 다른 한 가지 매핑 속성인 Association 이 설정되어 있어야 함

#### Association 속성
- 이 속성은 어떻게 두 클래스가 연관되어 있는지 확정해줄 때 사용함
- Tale이나 Column과는 달리 연관관계가 동작하기 위해 최소한 하나의 매개변수가 필요함
- [Association 속성이 사용하는 매개변수 목록]
  | 매개변수 이름 | 설명                                                         |
  | ------------- | ------------------------------------------------------------ |
  | DeleteRule    | 관계에서 연쇄 삭제 정책을 설정하는 매개변수                  |
  | DeleteOnNull  | 외래키 항목이 null 사용이 불가능할 때 1:1 관계에서 연쇄 삭제 정책을 설정하기 위해 사용함 |
  | IsForeignKey  | 부모-자식 관계에서 자식에 해당되는 클래스임을 나타내는 매개변수 |
  | IsUnique      | 외래키와 기본키가 양쪽 테이블에 모두 저장되어 있는 1:1 관계를 나타내기 위해 사용됨. 이 옵션은 대부분의 관계가 보통 1:0-1 또는 1:n의 관계에 가깝기 때문에 자주 사용되지 않음 |
  | Name          | 메타데이터에서 데이터베이스를 동적으로 생성할 때, 사용하는 외래키의 이름을 명시하는 매개변수 |
  | OtherKey      | 관련된 키값들을 포함하는 연관된 클래스의 열을 지정해주기 위해 사용됨. 만약 매개변수가 설정되지 않았다면 자동으로 다른 클래스의 ID열이 사용됨 |
  | Storage       | 연관된 자식 객체 EntitySets를 추적하기 위한 내부 항목을 지정해줌 |
  | ThisKey       | 로컬 ID 항목을 포함하는 프로퍼티를 지정해줌. 만약 설정되지 않았다면 Column 속성의 IsPrimary가 설정해준 열이 사용됨. 먄약 키가 여러 개의 열로 구성되어 있다면, 쉼표로 구분하여 각각 열거해줌 |

- 이런 정보가 주어졌을 때, 어떻게 새 Author 클래스와 BookAuthor 클래스 간의 연결을 추가해줄 수 있는지 알아보자
  ```C#
  private EntitySet<BookAuthor> _BookAuthors;
  [Assocation(Name="FK_BookAuthor_Author", Storage="_BookAuthors", OtherKey="Author", ThisKey="ID")]
  public EntitySet<BookAuthor> BookAuthors
  {
    get
    {
        return this._BookAuthors;
    }
    set
    {
        this._BookAuthors.Assign(value);
    }
  }
  ```
- Author 클래스의 기본키는 ID 프로퍼티(ThisKey), BookAuthor에서 연관된 키는 Author 프로퍼티(OtherKey).
- 우리는 해당 컬렉션(Storage) 을 _BookAuthors라고 불리는 EntitySet<BookAuthor> 항목에 저장할 것임  
- 우리의 클래스 속성 메타데이터에서 데이터베이스를 자동생성하고 싶다면, 해당 외래키의 이름을 FK_BookAuthor_ Author (Name)으로 정할 것임

- 이런 매핑구조를 이용하여 선언적으로 표준 질의 표현식을 이용하여 객체를 다루며, 프레임워크가 자질구레한 데이터 접근에 수반되는 귀찮은 코드들을 대신 처리해줄 것으로 기대가 됨
- 클래스에 직접적으로 매핑을 내장하는 것은 유지보수성의 측면에서는 양날의 검임
  - 비즈니스 클래스 생성 시에는 대부분의 개발자가 DB와 객체의 관계에 대해 신중하게 다룸
  - 개발자가 수정하는 사항들이 계속 다루는 과정에서 누락되거나 미아가 되는 경우가 발생하지 않음
  - 애플리케이션을 관리하는 과정에서 속성들이 코드의 여기저기에 산발적으로 정의되어 있다면 신속하게 찾아서 관리하는 데 어려움이 있을 수 있음
  - 추가적으로, 비즈니스 코드가 매핑 정보로 오염되면 가독성의 측면에서 핵심 비즈니스 요구사항에 집중하기가 어려워짐 -> 악영향

- 속성을 사용 시 발생되는 좀 더 큰 문제점: 속성들이 컴파일 시에 고정되어 버림
- 기존의 열이나 테이블의 이름을 바꾸거나 삭제하는 등과 같은 수정을 위해서는 애플리케이션을 재컴파일하여 새롭게 객체와 테이블내 데이터 간의 동기화를 재설정해야 함
- 만약 더 이상 데이터베이스에 존재하지 않는 매핑을 속성들이 명시하고 있다면 수행시에 예외상황이 발생할 가능성이 매우 높음
- 이런 두 가지 우려를 해결하기 위해 LINQ는 외부 XML 파일을 이용하는 두 번째 매핑방법을 사용함

### 7.1.2 외부 XML 파일과 매핑하기
- XML 파일을 이용하여 매핑을 명시하는 것은 클래스에서 속성을 사용하는 것과 같은 것
- XML 매핑을 이용하면 DataContext를 인스턴스화할 때 매핑 파일이 명시되어야 함 
- XMl 매핑 파일은 컴파일 과정을 생략하고 동적으로 변할 수 있음
- 속성들은 비즈니스 클래스 정의에서 제거될 수 있음 -> 업무상의 요구사항에 집중할 수 있도록 도와줌
- XML 매핑 파일들은 매핑을 중앙에 집중하여 관리할 수 있도록 해줌 -> 매핑을 유지보수하는 측면에서도 큰 장점이 있음

- 외부 파일을 이용하기 위해 완전히 다른 새로운 프로퍼티들을 배우는 
### 7.1.3 SqlMetal 도구를 사용하기

## 7.2 질의 표현식을 SQL로 변환하기
### 7.2.1 IQueryable
### 7.2.2 표현식 트리

## 7.3 객체의 생명주기
### 7.3.1 변화를 추적하기
### 7.3.2 변화를 저장하기
### 7.3.3 연결이 끊어진 데이터와 작업하기

참고: [https://www.wordrow.kr/%EC%9E%90%EC%84%B8%ED%95%9C-%EC%9D%98%EB%AF%B8/%EB%82%99%EA%B4%80%EC%A0%81%20%EB%8F%99%EA%B8%B0%ED%99%94/1/](https://www.wordrow.kr/자세한-의미/낙관적 동기화/1/)