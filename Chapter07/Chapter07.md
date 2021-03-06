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

- 외부 파일을 이용하기 위해 완전히 다른 새로운 프로퍼티들을 배우는 것에 대해 걱정할 필요 없음
- XML 매핑 개체들은 이미 논의한 속성들과 매우 비슷하게 생김, 오히려 관리 코드의 양이 줄어듬
- 외부 매핑 파일을 이용하면 클래스 내에서 정적으로 정의된 속성들을 제거 가능
- 대신 다음의 방법으로 Author 객체를 DB에 매핑이 가능함

```XML
<?xml version="1.0" encoding="utf-16"?>
<Database Name="lia"
 xmlns="http://schemas.microsoft.com/linqtosql/mapping/2007">
  <Table Name="Author">
    <Type Name="LinqInAction.LinqBooks.Common.Author">
      <Column Name="ID" Member="ID" Storage="_Id"
      DbType="UniqueIdentifier NOT NULL" IsPrimaryKey="true"/>
      <Column Name="LastName" Member="LastName" 
      DbType="VarChar(50) NOT NULL" CanBeNull="False"/>
      <Column Name="FirstName" Member="FirstName" 
      DbType="VarChar(30)"/>
      <Column Name="WebSite" Member="WebSite"
      DbType="VarChar(200)"/>
      <Column Name="TimeStamp" Member="TimeStamp"
      DbType="rowversion NOT NULL" CanBeNull="False"
      IsDbGenerated="True" IsVersion="True" AutoSync="Always"/>
    </Type>
  </Table>
</Database>
```
- 앞서 했던 모든 추가적인 매핑은 XML 파일로 옮겨짐
- XML 매핑 파일에 담긴 정보는 이미 클래스의 속성으로 사용한 매개변수들과 거의 완벽하게 일치함
- 어떤 클래스와 어떤 속성에 매핑하는지를 설정해주기 위해서 Type과 Member 항목을 반드시 설정해야 함

- XML의 최상위 노드는 Database 객체임
- 여기에 매핑하고자 하는 Database의 이름을 명시해주면 됨
- Database는 복수의 테이블 개체를 가지고 있을 수도 있고 각각의 Table 개체들은 주어진 테이블을 매핑할 댸 사용하는 Type이라는 개체를 포함함
- Type은 원하는 개수의 Column과 Association 개체를 가질 수 있음
- Column을 위한 속성과 Association 개체는 Member라는 속성을 기바으로 한 매핑에서 보기 힘들었던 추가적인 값을 하나 더 포함함

- 직접적으로 각각의 개체를 프로퍼티로 묘사하는 것이 아니기 떄문에 어떤 프로퍼티 또는 Member에 열이 매핑되어 있는지 명시해줘야 함
- 클래스 정의의 속성들을 직접 Column과 Association 개체로 일대일 대응시켜 이전 가능
- XML로 옮기면서 Member 속성을 잊지 않도록 항상 주의!!

- 새로운 XML 매핑 파일방식을 이용하기 위해, DataContext가 선언에 의한 기본 속성 대신 매핑 파일을 이용하도록 지시해야 함
- 다음 예제에서는 어떻게 외부 매핑 파일(lia.xml)을 DataContext에 붙이고 XML 매핑 파일을 이용해서 잘 설명되지 않는 상태의 비즈니스 객체들에 대해 질의할 수 있는지 살펴볼 것
- [DataContext에 외부 XML 매핑 파일을 붙이기]
```C#
XmlMappingSource map =
  XmlMappingSource.FromXml(File.ReadAllText(@"lia.xml"));
// System.Data.Linq.XmlMAppingSource 인스턴스를 애플리케이션 디렉토리 내의 lia.xml 파일 내에 생성함

DataContext dataContext = 
  new DataContext(liaConnectionString, map);
// 이 문서는 다음과 같은 메소드 중 하나로 읽어들일 수 있음
// FromXml, FromUrl, FromStream, FromReader, XMLMappingSource 객체를 DataContext에 붙이기 위해서는 오버로딩된
// 생성자의 두 번쨰 매개변수로 전달함

Table<Author> authors = dc.GetTable<Author>();
// 외부 매핑 파일을 DataContext에 연결하고 나면 이전 장에서 배운 질의 테크닉들을 모두 사용할 수 있음
```
- XML 매핑은 정의들을 중앙에서 별도로 정리해서 관리하면서 DB의 스키마 변화에 따라 동적으로 변경 가능함
- XML  파일을 읽어들이고 해석하는 과정은 물론 속성을 이용하는 것에 비하여ㅑ DataContext를 생성하는 데 더 많은 성능상의 부하를 유발 가능함

- XML이나 속성 기반의 매핑은 손으로 클래스와 매핑을 생성하고 관리해야 한다는 단점이 있음
- Visual Studio는 SqlMetal과 GUI 환경에서 동작하는 LINQ to SQL 디자이너라는 두 가지 자동화 옵션을 제공함

### 7.1.3 SqlMetal 도구를 사용하기
- DB에 SqlMetal 도구를 사용하여 해당하는 비즈니스 클래스들을 생성해냄
- SqlMetal을 이용하는 기본적인 문법은 SqlMetal [switches] [input file]임.

- ... 생략(직접 해봐야 함)
### 7.1.4 LINQ to SQL 디자이너
- 개발자들이 데이터 매핑을 눈으로 보면서 관리하고 확인할 수 있도록 Visual Studio에는 디자이너라는 도구가 내장되어 있음
- 디자이너는 개발자들이 DB에서 객체를 드래그 앤 드롭으로 가져오거나 개념적 모델을 직접 더하면서 눈으로 확인하면서 매핑을 관리할 수 있도록 해줌
- 특정한 데이터 관계를 어떻게 맺어줘야 할 지 확실하지 않을 때, 디자이너 도구를 한 번 사용해보는 것도 괜찮다.
- 디자이너가 만들어준 코드 수정 가능

- ... 생략(직접 해봐야 함)
## 7.2 질의 표현식을 SQL로 변환하기
- LINQ to SQL 을 잘 이해할 수 있는 부분은 질의 표현식에 대한 부분임
- LINQ의 질의기능은 IEnumerable<T>를 확장하여 구현하는 형들을 바탕으로 하고 있음 -> IEnumerable<T>를 구현하기 위한 EntitySet과 Tables를 필요로 함

- 자연적으로 EntitySet<T>와 Table<T>는 IEnumerable<T>를 구현함
- But... 그게 전부였다면 필터링이나 정렬과 같ㅇㄴ 모든 질의기능들은 클라이언트에서 별개의 코드로 구현되어 있었어야 함
- 그런 작업이 서버에서 이루어질 수 있도록 네트워크 소통량 등의 이득을 볼 수 있게 하려고 더 특화된 형태로 구현할 필요 있었음
- LINQ는 그런 의미에서 IQueryable<T>라는 IEnumerable<T>의 더 확장된 형태를 이용함

### 7.2.1 IQueryable
- LINQ to SQL이 LINQ to Objects에 비해 가진 가장 큰 장점 중 하나: 질의 표현식을 받아들여 다른 형식으로 변환하는 기능을 가짐
- 이런 기능을 구현하기 위해 객체들은 질의의 구조와 관련된 추가적 정보를 노출시킬 필요 있음
- LINQ to Objects의 모든 질의 표현식은 IEnumerable<T>를 확장하도록 구축되어 있음 
- 그러나 IEnumerable<T>는 데이터에 대하여 반복적인 작업을 수행할 수 있는 기능만 제공
- 필요한 변환만을 가능하게 할 수 있게 질의의 정의를 쉽게 분석하는 데 필요한 정보를 미포함
- .NET Framework 3.5는 IQueryable이라는 IEnumerable을 확장하면서 필요로 하는 정보들까지 포함하는 새로운 인터페이스를 포함하고 있음

- IQueryable은 IEnumerbale을 상속하는 구현 클래스를 필요로 함
- 이것이 포함하는 ElementType, 수행되어야 하는 동작을 표현하는 Expression, IQueryProvider 제네릭 인터페이스를 구현하는 세 가지 정보들을 담아야 한다는 조건이 있음

- 인터페이스 구현을 포함함 -> IQueryable은 SQL Server가 아닌 다른 SQL flavor를 비롯한 다른 데이터 형태에 대해 추가적인 프로바이더 모델을 지원 가능하게 됨
- 프로바이더는 IQueryable 표현식에 포함된 정보를 가져다가 DB가 인식할 수 있는 형태의 표현으로 변환하는 작업을 함(매우 어려움)
- 변환은 CreateQuery라는 메소드에서 받아서 처리함, Execute 메소드는 변환의 결과물을 수행하는 역할을 함 

- Expression 프로퍼티는 메소드의 정의를 포함
- 다음 질의의 경우를 생각해보자
`var query = books.Where(book => book.Price > 30);`
- 만약 책 객체가 IEnumerable<T>를 구현했다면 컴파일러는 다음고 같으 표준의 정적인 메소드로 변환했을 것
```C#
IEnumerable<Book> query =
  System.Linq.Enumerable.Where<Book>(
    delegate(Book book) {return book.Price > 30M;});
```
- 그러나 Books 객체가 IQueryable<T>를 구현했따면 컴파일러는 다음 예제와 같이 결과를 표현식 트리로 만들어내는 기능을 유지하여 구현할 것임
- [표현식으로 표현된 질의]
```C#
LinqBooksDataContext context = new LinqBooksDataContext();

var bookParam = Expression.Parameter(Typeof(Book), "book");

var query = 
  context.Books.Where<Book>(Expression.Lambda<Func<Book, bool>>
    (Expression.GreaterThan(
  Expression.Property(
    bookParam,
    typeof(Book).GetProperty("Price")),
  Expression.Constant(30M, typeof(decimal?))),
new ParameterExpression[] {bookParam}));
```
- 질의를 사용하는 데 사용되는 과정들을 그대로 보존함 -> IQueryable의 Provider 구현물은 언어구조를 DB가 이해할 수 있는 형태로 변환할 수 있는 기능을 그대로 갖추게 됨
- 질의구조에 대해 추가적으로 정렬, 그룹화, 누적연산, 페이지별 구분 등의 기능을 추가하여 한 번에 수행시킬 수 있음

### 7.2.2 표현식 트리
- 표현식 트리는  LINQ to SQL에게 동작하는 데 필요한 정보들을 전달해주는 역할을 함
- LINQ to SQL에서 기존의 표현식 트리를 가져와서 DB가 알아듣는 문법으로 질의 표현식을 변환하기 위해 가지별로 분석함
- 데이터베이스에 대한 접근을 좀 더 일반화시켜서 여러 DB 엔진을 지원하는 공통된 질의 문법을 만들려는 노력이 있어왔음
- 종종 이런 해법들은 질의를 문자열화시켜서 문자열 조작을 통해 한 가지 표현법을 다른 표현법으로 변환해내는 방법에 의존하기도 함

- 다른 질의 변환 시스템과는 달리 LINQ to SQL은 질의 표현식을 표현식 트리로 만들어내는 방법으로 질의를 구조화하여 비교함
- 표현식을 보존함으써 우리는 추가적인 절을 질의에 조합하는 방법으로 질의에 기능을 추가할 수 있음
  - 질의 내에서 엄격한 형을 유지하고, IDE와의 더 나은 통합 및 메타데이터의 보존을 도모할 수 있음
- **가장 중요한 점:** 목적을 담은 문자열을 해석하기 위해 수고할 필요가 없음
- 표현식 트리는 컴파일러가 구현된 대로 좀 더 최적화된 방법을 사용할 수 있도록 해줌
- 이 개념을 IQueryable 예에 적용해보며 확인해보자
```C#
LinqBooksDataContext context = new LinqBooksDataContext();

var bookParam = Expression.Parameter(typeof(Book), "book");

var query = 
  context.Books.Where<Book>(Expression.Lambda<Func<Book, bool>>
  (Expression.GreatherThan(
    Expression.Property(
      bookParam,
      typeof(Book).GetProperty("Price")),
    Expression.Constant(30M, typeof(decimal?))),
  new ParameterExpression[] { bookParam }));
```
- 표현식 형에 대해 살펴보자
- Lambda, GreatherThan, Property, Parameter, Constnat 각각의 이런 표현식 형은 좀 더 작은 부분의 단위들로 나누저여 있음 -> 더 많은 정보를 담을 수 있음
- Datacontext가 속성이나 XML로 정의된 매핑 등의 메타데이터를 포함하고 있으므로 데이터베이스가 알아듣는 형태로 객체의 표현형태를 손쉽게 바꿀 수 있음

- 표현식 트리를 좀 더 깊게 살펴보면, GreatherThan BinaryExpression을 적용할 때, 추가적인 노드들이 어떻게 삽입되는 지 확인 가능
- GreatherThan을 CLR 형에 대해 수행 시에는 유사한 형을 데이터끼리 비교할 수 있도록 해줘야 함
- 이런 의미에서 ConstantExpression을 null이 될 수 있는 Decimal형으로 바꿔서 책의 Price 프로퍼티에 있는 데이터 형과 비교할 수 있어야 함
- 하지만 굳이 DB에 SQL문 보낼 떄 사용자가 이걸 신경 쓸 필요 없음

- LINQ to SQL은 어떻게 이 많은 정보를 모두 조합하여 DB가 알아볼 수 있도록 반환하는 것일까
  - IQueryable<T> 형의 결과들에 대해 반복문을 수행할 떄 전체 Expression값이 지정된 Provider에게 넘겨짐
  - 프로바이더는 Visitor패턴을 이용해서 Where와 GreatherThan과 같이 자기가 다룰 줄 아는 표현식 형들을 골라냄
  - 트리를 아래부터 천천히 확인해서 상수를 null을 대입할 수 있는 형으로 바꿔줌... 
  - 자신이 필요없는 노드들을 모두 확인함
  - 그런 후에 SQL과 더욱 유사한 형태인 평행 표현식 트리 형태로 만들어줌

- 표현식이 해석된 후에는, 프로바이더가 XMl 매핑에서 가져온 테이블과 열의 이름들을 기반으로 객체를 적절히 변환 -> 최종 SQL 문을 만들어냄
- SQL 문이 완성되면 그것을 DB에 전송하며, 반환되는 결과값들은 적절한 매핑 정보를 이용하여 필요한 객체 컬렉션을 채우는 데 이용됨

- 표현식 트리를 특정 프로바이더에 대한 구현물로 변환해내는 것은 트리에 함수를 추가하는 과정을 거치면서 매우 복잡해짐
- 만약 모든 가능한 질의의 조합을 다루는 것에 대해 더 알고 싶다면 LINQ to SQL을 설계한 사람 중 한 분인 매트 워렌의 블로그에서 IQueryable 프로바이더를 어떻게 구현했는지 설명한 글을 읽기
  - http://blogs.msdn.com/mattwar/archive/2007/07/30/linq-building-an-inqueryable-provider-patt-i.aspx.

## 7.3 개체의 생명주기
- LINQ 는 언어 내에서 엄격하게 형을 가지는 질의를 직접 구성할 수 있는 기능 제공
- 게다가 프레임워크는 객체 변화를 관리하고 그 값에 따라 데이터베이스 소통을 함

- DataContext 객체는 개체의 생명주기상에서 매우 중요한 역할을 함
- DataContext는 데이터베이스에 대한 연결을 관리, 매핑을 평가 -> 표현식 트리를 사용 가능한 구조로 변환시켜 줌
- 만약 데이터를 단순히 보여주는 것만을 목표로 했다면 매핑과 변환 서비스는 요구사항을 충족시키기에 충분할 것
- 생명주기는 객체를 전달하는 순간 끝나게 될 것임

- 애플리케이션들은 질의의 결과를 표출하기도 하고 변화를 주기도 함
- 객체 생명주기를 완전하게 다루기위해 DataContext는 받아온 객체들에 대한 참조도 관리함
- DataContext는 받아온 객체의 정체성과 변한 값을 추적하면서 변화를 관찰함

- ![](cap1.PNG)
- 이 그림은 DataContext가 제공하는 서비스들을 보여줌

- 개체의 생명주기는 처음 값을 DB에서 읽어왔을 때부터 시작됨
- 결과 객체를 애프리케이션 코드에 전달하기 전에, DataContext는 객체에 대한 참조를 저장하고 정체성 관리 서비스는 매피에서 지정된 정체성을 바탕으로 객체를 목록에 관리함
- 이 값이 저장되어 있기 떄문에 정체성을 기반으로 하여 이전 객체에 접근 가능하게 됨

- 매번 DB의 값들에 접근할 떄마다 DataContext는 정체성 관리 서비스를 통해 이전에 반환된 객체들 중 동일한 정체성을 가지고 있는 것이 있는지 확인
- 만약 있다면, DataContext는 테이블에 새로 매핑하는 대신 내부 캐시에 저장된 값을 가져올 것임
- 원래 값을 저장해두는 방법으로 클라이언트들이 다른 사용자들이 만든 수정사항들에 대해 신경 안 쓰고 값을 수정할 수 있도록 해줌
- 그래서 개발자는 수정사항들이 저장되는 시점까지 동기화 문제에 대해 신경 안 써도 됨

- 컨텍스트가 반환된 값들을 임시로 저장하고 있다면 정보를 요구할 떄마다 데이터베이스에 대한 질의가 수행되지 않아도 됨
- But... 사용자가 만약 ToList나 유사한 확장 메소드로 미리 값을 받아놓기만 하고 메모리내에서 사용 안한다면 매번 정보를 요구할 때마다 질의가 수행될 것
- 만약... 컨텍스트가 이미 객체에 대해 알고 있다면 ID에 해당하는 열만 사용되고 나머지 열들은 무시됨

- 자동화된 객체 정체성 구현이 개발자를 곤란하게 만들 수 있는 경우:
  - Single 확장 메소드를 이용하는 것 = 원래의 캐시 젗액에 어긋나는 예외
      - Single을 이용하면 내부 캐시가 먼저 확인됨, 만약 요청된 객체가 캐시에 존재하지 않으면 DB가 질의될 것임

- 테이블에 저장되고 테이블에서 삭제되는 항목들이 질의 가능할 것으로 생각하겠지만 DB는 실제로 자신이 알고 있는 값에 대해서만 질의 가능 
  - InsertOnSubmot or DeleteOnSubmit이 수행되었다고 해도 실제로 SUbmitchanges 메소드로 그런 변화가 저장소에 저장되기 전까지는 DB에서 변화 모름
  - XXXOnSubmit 메소드들이 일반적인 IList 메소드 이름 대신에 사용된 주된 이유임

### 7.3.1 변화를 추적하기
- 객체에 변화를 줄 때마다, DataContext는 그 속성의 원본 값과 새로 변한 뒤의 값을 대조 -> 변화 추적기능을 이용하여 관리함
- 원본 값과 수정 값을 둘 다 보관하고 있으므로 DB에 대한 수정을 이것이 필요한 레코드만 업데이트해서 최대한 효율적으로 처리 가능
- 밑의 예제는 두 개의 DataContext가 있는데 각각 별도로 객체 정체성과 변화 추적을 관리함
- [정체성 관리와 변화 추적]
```C#
LinqBooksDataContext context1 = new LinqBooksDataContext();
LinqBooksDataContext context2 = new LinqBooksDataContext();

context1.Log = Console.Out;
context2.Log = Console.Out;

Guid Id = new Guid("92f10ca6-....");

Subject editingSubject = 
  context1.Subjects.Where(s => s.Id == Id).SingleOrDefault();

ObjectDumper.Write(editingSubject);
ObjectDumper.Write(context2.Subjects.Where(s => s.ID == Id));

editingSubject.Description = @"Testing update";

ObjectDumper.Write(Context1.Subjects.Where(s => s.ID == Id));
ObjectDumper.Write(Context2.Subjects.Where(s => s.ID == Id));
```

- 첫 번째 컨텍스트에서 editingSubject를 가져옴, 두 번째 컨텍스트에서 editingSUbject와 데이터베이스의 값을 모두 가져오면 다음과 같음

- [변화를 주기 전과 후에 질의가 반환해주는 값의 상태]
- | 동작                    | Context1 | Context2 | 데이터베이스 |
  | ----------------------- | -------- | -------- | ------------ |
  | 원래 질의에서 반환된 값 | 원본     | 원본     | 원본         |
  | 변화를 준 후의 재질의   | 변함     | 원본     | 원본         |

- 원래 질의를 다시 수행시켜 결과를 내보낼 때, context1에 대한 질의가 반환한 설명은 새로운 설명값임을 확인 가능함
- context2는 원래 값을 반환해줄 것임
- 각각의 컨텍스트는 개념상 다른 사용자라고 생각할 수 있다는 점을 고려하면 두 사용자가 각각의 다른 값을 가진 데이터를 보게 되는 것임
- 데이터베이스의 값을 확인해보면 아직 저장소 수준에서는 원래의 값을 유지하고 있음을 확인 가능함

- 첫 번째 컨텍스트 객체에 대한 재질의가 DB에서 받아온 내용을 바탕으로 한 새로운 객체가 아니라 정체성 추적기능에 의해 반환된 결과라는 것이 놀라움
- 두 번쨰 컨텍스트도 자신이 갖고 있던 값을 반환한 것임
- 두 번째 컨텍스트가 추적하는 값에 대해 아무런 변화도 주지 않았음 -> DB에 저장된 값과 동일한 값인 것처럼 보일 뿐임
- 열과 매핑된 프로퍼티값들의 변화를 추적하는 것과 함께 변화 추적기능은 객체 간의 관계변화도 추적함
- 그래서 만약 하나의 책에 대한 설명을 다른 책으로 옮기려고 하면 DB에 실제로 저장하라고 명령하기 전까지느 변화 추적기능은 변화 내용을 메모리에 갖고 있을 것임.

### 7.3.2 변화를 저장하기
- 지금까지 가했던 수정사항들은 메모리상에서만 저장되어 있을 뿐 DB에 영원히 반영되지 않고 DataContext의 정체성 추적기능에 의해 차후에 적용되는 질의드렝 대해서만 적용되었을 것임
- 변화를 DB에 영구적으로 저장하기 위해서는 SubmitChanges를 한 번 호출해줘야 함
- SubmitChanges가 호출되었을 때, 컨텍스트는 그것이 추적하고 있는 객체의 원래 값을 현재의 값과 비교함
- 만약 두 값 사이에 차이가 있다면 컨텍스트는 변화를 잘 저장해둔 후, DB가 수행해야 할 질의를 동적으로 작성해 줌

- 어떠한 충돌도 일어나지 않고 적절한 레코드들이 업데이트되었다는 가정하에, 컨텍스트는 그것이 저장하고 있는 변화의 목록들을 실제로 데이터베이스에 적용해줌
- 만약 문제가 있다면 DB의 동기화 관리기능을 기반으로 하여 변화들은 취소되고 원상태로 복귀됨

- [정체성과 변화 추적 관리를 이용하면서 변화를 저장하기]
```C#
LinqBooksDataContext context1 = new LinqBooksDataContext();
LinqBooksDataContext context2 = new LinqBooksDataContext();

Guid Id = new Guid("92f10ca6-...");

context1.Log = Console.Out;
context2.Log = Console.Out;

Guid Id = new Guid("92f10ca6-....");

Subject editingSubject = 
  context1.Subjects.Where(s => s.Id == Id).SingleOrDefault();

Console.WriteLine("Before Changes:");
ObjectDumper.Write(editingSubject);
ObjectDumper.Write(context2.Subjects.Where(s => s.ID == Id));

editingSubject.Description = @"Testing update";

Console.WriteLine("After Change:");
ObjectDumper.Write(context1.Subjects.Where(s => s.ID == Id));
ObjectDumper.Write(context2.Subjects.Where(s => s.ID == Id));
// SubmitChanges 호출 전까지는 메모리 상에서만 변경이 되어 있음

context1.SubmitChanges();
//DB에 실제로 변경사항이 저장됨, 변화 추적 기능이 context1에서 초기화됨

Console.WriteLine("After Submit Changes:");
ObjectDumper.Write(context1.Subjects.Where(s => s.ID == Id));
ObjectDumper.Write(context2.Subjects.Where(s => s.ID == Id));

//동일한 LINQ 질의를 context1과 context2에서 수행시키면 목적에 맞게 다른 컨텍스트의 정체성이나 변화 추적상태에 대해 전혀 모르는 새로운 
//세 번째 컨텍스트를 생성해줌
LinqBooksDataContext context3 = new LinqBooksDataContext();
ObjectDumper.Write(context3.SUbjects.Where(s => s.ID == Id));
```
- 어떻게 객체들이 데이터 컨텍스트 속에서 동작하는지 이에 대해서 아는 것은 매우 중요함
- DataContext는 애초에 아주 단기간 살아 있어야 하는 객체로 구성됨
- 개발자는 사용하고 있는 Context에 대해 잘 알고 있어야 하며, 어떻게 정체성고 ㅏ변화 추적 서비스가 예상하지 못한 결말을 피하기 위해 동작하고 있는지 알아야 함
- 데이터를 전달하기만 할 떄는 전달할 떄마다 컨텍스트 생성한 후 값의 전송 끝나면 버리는 형태를 취함
- 그런 경우 ObjectTrakingEnabled 프로퍼티를 false로 바꿈으로써 컨텍스트의 최적화가 가능함
- 이것은 정체성과 변화를 추적하는 서비스 제거 -> 전체적인 성능 향상에 도움이 되지만 변화를 업데이트하는 기능을 끄게 됨

- 만약 데이터를 업데이트할 필요가 있다면, 컨텍스트의 관리 범주에 주의하고 적절하게 조정해야 함
- 모든 객체와 변화한 값들을 메모리내에 가지고 있는 것의 성능, 메모리 점유율상으 ㅣ비용을 고려해야 함
- LINQ to SQL을 사용하면서 권장되는 패턴: 질의-보고-편집-제출-폐기
- 더 이상 객체에 대한 변화를 관리할 필요 없을 때, 컨텍스트를 정리하고 새로운 것을 만들어야 함

### 7.3.3 연결이 끊어진 데이터와 작업하기
- DB 연결이 때로 끊길 수도 있다.
- 이런 상황은 웹 서비스나 WF, WCF 등의 비연결 기반 모델을 사용할 때 발생하는 경우가 많음
- 비연결 기반 모델에서 데이터를 전달할 때는 결과를 잘 포장해서 캡슐화할 필요가 있음
- 데이터 컨텍스트를 캐싱하거나 연결되지 않은 사용자에게 전송해줄 수 없기 때문임.

- 데이터 컨텍스트와 객체가 완전 분리되어 동작해야 하므로, 더 이상 데이터 컨텍스트를 제공하는 변화 추적기능이나 정체성 관리 서비스를 이용할 수 없게 됨
- 클라이언트에게 전달해줄 수 있는 객체의 종류는 아주 간단한 것, or 그런 객체의 XML 표현형태로 제한됨
- 비연결 기반 모델에서 변화를 관리한ㄴ 것은 엄청난 부담임...

- LINQ to SQL은 비연결 기반 모델을 지원하기 위해 변화를 적용하는 두 가지 방법을 제공
  - 1. 테이블에 단순히 행을 하나 추가하는 일
    - 해당 DataContext의 테이블 객체에 대해 InsertOnSubmit  메소드 호출 가능
    - 새로운 레코드 추가 시에는 변화 추적이 반드시 필요한 것은 아님 -> 기존 레코드와의 충돌 걱정할 필요 없이 InsertOnSubmit에 대한 호출은 제대로 작동할 것
  - 2. 기존의 레코드를 수정하는 일
    ```C#
    public void UpdateSubject(Subject cachedSubject)
    {
      LinqBooksDataContext context = new LinqBooksDataContext();
      context.Subjects.Attach(cachedSubject);
      cachedSubject.Name = @"Testing Update";
      context.SubmitOnChanges();
    }
    ```
    - 바뀌는 부분을 잘 연결해줘야 함
    - DataContext에 변화된 객체를 연결해주는 방법에는 여러 가지가 있지만 가장 선호되는 방법: Attach 메소드 이용 -> DataContext에 레코드를 일반적인 질의처럼 접목시키는 것
    - 일단 붙여지면 데이터 컨텍스트의 변화 및 정체성 추적기능이 적용하려는 변화들을 관리 가능
    - 붙여진 객체의 Name을 바꾸는 부분은 변화 추적 서비스에 의 해 추적되고 그에 맞게 업데이트됨
    - 안 그럴 경우 변화 추적기능이 변화를 관리 못하게 될 수 있음
    - 만약 업데이트된 어떤 값을 붙이려고 한다면, 객체가 특이한 몇 가지 특성을 갖고 있지 안핟면 이미 수정이 가해진 객체를 간단하게 데이터 컨텍스트에 붙일 수 없음
    - 먄약 Author 객체에서 한 것처럼 TimeStamp 열을 객체에 구현한다면 오버로딩된 Attach 메소드를 이용하여 붙일 수도 있음
    - `context.Authors.Attach(cachedAuthor, True);`
      - 두 번째 매개변수는 객체가 오염된 것으로 간주하게 함
      - 데이터컨텍스트가 변화된 객체의 목록에 해당 객체를 추가하도록 강제함
    - 만약 데이터베이스 스키마에 타임스탬프 열을 추가할 만한 여유나 유연성이 없지만 Atach를 이런 식으로 사용해야 한다면 매핑의 UpdateCheck 프로퍼티를 설정해서 값들이 체크되지 않도록 할 수 있음  
    - 이 두 가지 경우 모두 모든 프로퍼티는 변화가 일어났는지의 여부와 상관없이 무조건 업데이트될 것임
    
    - 만약 원본 객체를 캐싱이나 개체내에 직접 보관하는 방식으로 복사본을 하나 갖고 있다면, 새 객체에 Attach 메소드를 이용하여 변화된 버전과 원본 버전을 함께 붙여줄 수 있음
    - `context.Submects.Attach(changedSubject, originalSubject);`
    - 이런 경우 모든 열들에 대해 업데이트가 강제되지 않고 변화된 열들만 Update 절에 포함될 것
    - 원래의 값들은 Where절에서 동기화 확인을 위해 사용될 것
    - 만약 이런 Attach 시나리오의 혜택을 볼 수 없다면 OriginalSubject를 다음 예제처럼 업데이트 과정 중에 데이터베이스에서 새롭게 전달된 것으로 대체 가능함
    - [이미 바뀐 연결이 끊어진 객체를 업데이트하기]
    ```C#
    public static void UpdateSubject(Subject changingSubject)
    {
      LinqBooksDataContext context = new LinqBooksDataContext();
      Subject existingSubject = context.Subjects
                                        .Where(s => s.ID == changingSubject.ID)
                                        .FirstOrDefault<Subject>();
      existingSubject.Name = changingSubject.Name;
      //만약 프로퍼티 내의 값들이 동일하다면 변화 추적 서비스는 지속적으로 이런 프로퍼티들의 변화가 필요없다고 판단할 것임
      //그리고 업데이트 수행을 안 함
      existingSubject.Description = changingSubject.Description;
      context.SubmitChanges();
      //변화된 프로퍼티와 객체들만 변화가 강제될 것임
    }
    ```
    - 업데이트하는 객체들이 꾸준히 데이터베이스에서 수정했던 값드릉 기반으로 하고 있을지 모른다는 사실을 항상 주의!
    - 동기화 추적 관리를 위해 개발자의 최선의 선택: 원래 레코드 전달 시 데이터베이스 버전을 타임스탬프에 담아 제공하는 것

    - 만약 타임스탬프를 추가하는 방법을 선택하기 어렵다면 원본 값의 복사본을 하나 가지고 있거나 원본 값의 해시값을 저장하고 있어야 햠
    - 이런 방법으로 원래 값들과 변화된 값의 비교를 통해 직접 동기화 상태를 확인하고 관리 가능함.

    - DataContext의 객체 정체성과 변화 추적기능은 객체의 생명주기에서 매우 중요한 역할을 담당
    - 만약 단순히 값들을 읽는 것이 목적이라면 ObjectTrackingEnabled 옵션을 false로 설정하는 방법을 통해 DataContext를 읽기 전용 모드로 동작시킬 수 있음
    - 그러나 값들을 변화시키고 변화를 지속하고자 한다면 객체와 그에 대한 변화를 추적하는 것은 매우 중요한 기능

참고: [https://www.wordrow.kr/%EC%9E%90%EC%84%B8%ED%95%9C-%EC%9D%98%EB%AF%B8/%EB%82%99%EA%B4%80%EC%A0%81%20%EB%8F%99%EA%B8%B0%ED%99%94/1/](https://www.wordrow.kr/자세한-의미/낙관적 동기화/1/)