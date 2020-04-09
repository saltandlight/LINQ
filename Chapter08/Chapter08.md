# Chapter08. LINQ to SQL의 고급기능🦄
- IQUeryable 인터페이스를 매핑 메타데이터와 표현식 트리와 함께 이용하는 방법으로 관계형 데이터에 LINQ to Objects의 질의 표현식을 그대로 사용하는 것에 대해 알아봄
- 알아볼 내용:
    - 고급화된 LINQ to SQL의 기능들
    - 객체의 생명주기에 대한 논의를 일정 수준 확장, 동기화나 트랜잭션
    - 어떻게 좀 더 직접적으로 DB에 접근하고 SQL Server가 제공하는 기능들의 장점을 그대로 활용할 수 있는지
    - 데이터 영역의 수준을 넘어서는 비즈니스 영역에서 미리 컴파일된 질의 표현식, 부분 클래스(partial class), 상속 다형성 등을 통해 LINQ to SQL이 가져다주는 새로운 옵션들에 대해 논의해볼 것임
    - 최종적으로 LINQ to SQL의 대안으로 떠오르고 있는 Entity Framework를 이용하여 관계형 데이터를 접근하는 방법에 대해 알아볼 것

## 8.1 동기화 다루기
- 시스템이 복수의 사용자들을 지원가능하도록 확장되기 시작하면 개발자는 두 명 이상의 사용자가 하나의 특정 레코드를 동시에 수정하려고 할 떄 발생할 수 있는 문제에 대해 조금씩 고려하기 시작해야 함
- 이런 상황에 대응하는 전략:
    - 1. 비관적 동기화: 첫 번째 사용자가 작업을 하는 동안 락을 걸어 두 번째 사용자의 작업을 제한함
    - 2. 낙관적 동기화: 두 사용자가 모두 우선 수정 가능함
        - 애플리케이션 설계자는 상황에 따라 첫 번째 사용자의 수정사항을 유지할지, 아니면 두 번째 사용자의 수정사항대로 유지할지, 아니면 두 사용자의 수정사항을 적절히 조합할지를 능동적으로 판단함
    - 이 두 가지 방식의 전략은 각각 명확하게 장단점을 가지고 있음
 
### 8.1.1 비관적 동기화
- .NET 을 사용하기 전에는 많은 애플리케이션들이 DB에 대한 여녁ㄹ상태를 유지하도록 코드를 작성했음
- 이런 설계하에서는 DB의 특정 레코드를 받아온 후 다른 사용자가 동시에 수정사항을 적용하는 것을 막기 위해서 락을 건 상태를 유지시킴

- 소규모의 윈도우 기반 애플리케이션은 비관적 동기화 시나리오하에서는 별 문제 없이 동작
- 그러나... 시스템이 더 많은 사용자를 대상으로 확대되는 경우, 비관적 동기화는 과도하게 많은 락 지속시간으로 인해 시스템 전체의 효율이 저하됨...

- And... 규모 확장성의 문제들이 나타나기 시작함
- 시스템들은 클라이언트-서버 구조에서 좀 더 배포상의 용이함을 얻기 위해 상태가 없는 웹 기반의 구조로 옮겨가게 되었음
- 상태가 없는 웹 애플리케이션들을 작성한다는 것 = 애플리케이션이 장기간의 독점상태를 유발하는 비관적 동기화에 더 이상 의존 불가능

- 비관적 동기화가 유발하는 비효율적 잠금(locking) 문제와 규모 확장성 문제를 해결하고자 하는 취지에서 .NET 프레임워크는 비연결성을 특징으로 하는 웹 기반 애플리케이션을 대상으로 설계됨
- ADO.NET 이라는 데이터를 다루는 .NET API는 테이블상에서 커서의 개념을 더 이상 사용하지 않게됨
- 결과적으로 자동으로 적용되던 비관적 동기화를 더 이상 사용하지 않게 됨
- 애플리케이션들은 아직도 "체크 아웃"이라는 플래그를 두어 동일한 레코드에 연달아 접근하려는 시도가 있다면 사용 가능하도록 설계 가능
- 그러나 사용자들이 언제 더 이상 그 레코드를 사용하지 않는지 판별하기 어려움 -> 종종 그런 플래그는 사용 후 해제가 안 되는 경우가 있었음
- 이런 문제의 예방을 위해 낙관적 동기화 모델이 비연결 기반의 환경에서 대세로 등장하기 시작함 

### 8.1.2 낙관적 동기화
- 비연결 기반의 환경에서 발생한 문제점들로 인해서 이에 대한 대체 전략으로 낙관적 동기화 모델이 사용되기 시작함
- 낙관적 동기화 모델은 아무 사용자나 그들의 사본 데이터에 수정을 가할 수 있도록 허용함
- 대신에 데이터가 저장되는 시점에 프로그램은 다른 사용자가 사용했던 값들이 수정되었는지 확인 
    - -> 만약 값들에 변화가 없었다면 그 레코드들은 잠궈지지 않은 것으로 판정 
    - -> 사용자가 저장하려던 값을 저장할 수 있게 해줌
- 만약 다른 사용자가 값에 변화를 준 상황이라면 이런 상황을 충돌이라고 함. 
    - 그리고 프로그램이 이전 사용자의 수정사항을 덮어씌우거나
    - 새로운 변화를 무시하거나
    - 다른 방법들로 변화를 병합할 지 선택해야 함

- 동기화 상태를 혹인하지 않는다면 데이터베이스에 대한 SQL 문은 다음과 같은 문법구조로 이루어져 있음
- `UPDATE TABLE SET [field = value] WHERE [Id = value].`
- 여기에 낙관적 동기화를 추가하기 위해 WHERE 절은 ID 열의 값 뿐만 아니라 각각의 열의 원래 값을 비교하도록 확장되어야 함
- [Book에 대해 낙관적 동기화를 수행하기 위한 SQL Update문]
 ```SQL
 UPDATE dbo.Book
 SET Title = @NewTitle,
   Subject = @NewSubject,
   Publisher = @NewPublisher,
   PubDate = @NewPubDate,
   Price = @NewPrice,
   PageCount = @NewPageCount,
   Isbn = @NewIsbn,
   Summary = @NewSummary,
   Notes = @NewNotes,
WHERE ID = @ID AND Title = @OldTitle AND 
-- 원래 있던 값들과 비교
   Subject = @OldSubject AND
   PUblisher = @OldPublisher AND
   PubDate = @PubDate AND
   Price = @Price AND
   PageCount = @PageCount AND
   Isbn = @OldIsdn AND
   Summary = @OldSummary AND
   Notes = @OldNotes
   RETURN @@RowCount
-- 업데이트에 성공했는가?
 ```
- 업데이트에 성공했는 지 확인하려면 업데이트 수행 이후의 데이터베이스 내의 RowCount를 세어보면 됨.
- 만약 1이 반환된다면 원래 있던 값이 변하지 않았고 업데이트가 동작했다고 할 수 있음 
- 만약 0이 반환된다면 처음 로딩된 값과 동일한 값을 가진 레코드를 찾을 수 없음 -> 데이터를 받아온 이후 업데이트하는 사이에 다른 사람이 그 데이터를 바꾸어 논 것임
- 그 시점에는 사용자에게 충돌이 있었다는 점을 통지 -> 동기화 위배상황을 적절히 처리하면 됨

- 클래스가 낙관적 동기화 모델을 다르도록 구현하는 것은 매우 쉬운 일임.
- 사실 테이블과 열 매핑 구축만으로도 낙관적 동기화 모델이 기본적으로 적용되었다고 볼 수 있음
- SubmitChanges 를 호출 시 DataContext는 자동적으로 낙관적 동기화를 사용할 것임
- 간단한 업데이트 수행을 위해 어떤 SQL이 작성되었는지 살펴보려면 다음과 같이 테이블에서 가장 비싼 책을 골라내고 그 가격을 10% 할인하려고 시도하는 경우를 고려해보자
- [LINQ to SQL을 이용한 기본적인 동기화 구현]
```C#
Ch8DataContext context = new Ch8DataContext();
Book mostExpensiveBook = (from book in context.Books
                          orderby book.Price descending
                          select book).First();
decimal discount = .1M;
mostExpensiveBook.Price -= mostExpensiveBook.Price*discount;
context.SubmitChanges();
```
- 위 코드는 책을 선택하기 위한 SQL과 업데이트하는 SQL을 나타냄
```sql
UPDATE  [dbo].[Book]
SET     [Price] = @p
FROM    [dbo].Book
WHERE   ([Title] = @p0) AND ([Subject] = @p1) AND ([Publisher] = @p2)
    AND   ([PubDate] = @p3) AND ([Price] = @p4) AND ([PageCount] = @p5)
    AND   ([Isbn] = @p6) AND ([Summary] IS NULL) AND ([Notes] IS NULL)
    AND   ([ID] = @p7)
```
- DataContext에 Submitchanges가 호출되면 Update문이 자동으로 생성되어 서버에서 수행됨
- WHERE절에 전달된 이전 값을 기준으로 이들 값과 일치하는 레코드가 없다면 DataContext는 이 문장이 어떤 레코드도 수정하지 않는다는 것을 알게 될 것임
- 어떠한 레코드도 영향을 받지 않는 경우에는 ChangeConflictException 예외가 발생함

- 상황에 따라 낙관적 동기화를 구현하기 위해 필요한 매개변수의 개수가 성능상의 문제를 야기할 수도 있음(개수가 너무 많아서...)
- 그런 경우에는, 매핑을 보다 세밀하게 조정하여 값이 변하지 않았음을 확인하는 데 필요한 최소한의 필드들을 구분 가능
- UpdateCheck 속성을 설정하면 됨
    - UpdateCheck는 LINQ to SQL이 항상 열을 낙관적 동기화를 위해 체크하고 있다는 의미에서 Always로 설정되어 있음
    - 원한다면 값이 변했을 때에만(WhenChanged), 또는 절대 체크 안 되도록(Never)로 설정 가능함

- 만약 UpdateCheck 속성의 강력함을 활용하여 테이블 스키마를 변경하고 싶다면, RowVersion 또는 TimeStamp 열을 각 테이블에 추가하면 됨
- DB는 자동적으로 행에 변화가 생길 떄마다 RowVersion을 자동으로 업데이트할 것임
- 동기화 체크는 버전과 ID 열의 조합에 대해서만 이루어지면 될 것임
- 모든 다른 열은 UpdateCheck=Never로 설정되고 DB는 동기화 체크에 의해 보조될 것임
- 이런 경우는 Author 클래스를 매핑하는 데 사용한 적이 있음
- Update문을 살펴보면 TimeStamp열을 이용하여 효율화된 WHERE절을 볼 수 있음
- [타임스탬프 열을 이용하여 Authors 테이블에 낙관적 동기화를 적용하기]
```C#
Ch8DataContext context = new Ch8DataContext();
Author authorToChange = (context.Authors).First();

authorToChange.FirstName = "Jim";
authorToChange.LastName = "Wooley";

context.SubmitChanges();
```
- 이 문장은 다음과 같은 SQL로 변환됨
```SQL
UPDATE  [dbo].[Author]
SET     [LastName] = @p2, [FirstName] = @p3
FROM    [dbo].[Author]
WHERE   ([ID] = @p0) AND ([TimeStamp] = @p1)

SELECT  [t1].[TimeStamp]
FROM    [dbo].[Author] AS [t1]
WHERE   ((@ROWCOUNT) > 0) AND ([t1].[ID] = @p4)
```
- 표준적인 낙관적 동기화를 모든 필드가 항상 체크하도록 또는 타임스탬프를 이용하는 형태로 수정하는 방법 외에도 다른 동기화 모델이 존재함
    - 1. 동시에 일어나는 모든 변화를 무시하고 항상 마지막 변경사항만 남기도록 하는 것
        - 그런 경우에는 UpdateCheck는 모든 속성에 대해 Never로 설정되어 있어야 함
        - 그러나 동기화가 전혀 필요 없다고 단언할 수 있는 상황이 아닌 이상 이 방법은 그다지 추천할 만한 좋은 선택이 아님
    - 2. 최고의 선택: 사용자에게 충돌이 있었다는 점을 통지, 그 상황을 잘 봉합하기 위한 대안을 제시해주는 것

- 경우에 따라서 두 사용자가 하나의 테이블 내의 다른 두 개의 열을 동시에 수정할 수 있게 하는 것은 별 문제가 없을 수도 있음
- 예) 열이 아주 많은 테이블에서 각각 다른 열들의 집합을 다른 객체나 문맥, 상황에서 다루려고 하는 경우에는 UpdateCheck를 Always 보다는 WhenChanged로 설정함 
- 그러나 이것이 모든 경우에 사용하도록 추천할 수 있는 방법은 아님
- 여러 개의 필드가 연산된 결과물의 기반자료가 되는 경우에는 더욱 그러함
- 예를 들어 일반적인 OrderDetail테이블에서 단가나 개수에 수정을 한다면 총계 또한 변해야 함
- 하나의 사용자가 개수를 수정하는 동안 다른 사용자가 단가를 수정한다면 총계의 계산이 이상해질 것..!
    - 이런 이유로 자동병합 동기화 관리는 필요하며 실무상황에서 그런 관리가 허용 가능한지는 반드시 확인해야 함

- LINQ to SQL에서 동기화 확인은 필드 수준에서 이루어질 수 있음
- LINQ to SQL은 여러 사용자 정의형태의 구현물이 존재할 수 있는 유연함을 충분히 제공하고 있음
- 아무런 설정을 해주지 않았을 때는 기본적으로 완전히 낙관적인 동기화를 지원함

### 8.1.3 동기화 예외사항을 처리하기
- UpdateCheck에 Always나 WhenChanged 옵션을 사용할 때, 두 명의 사용자가 동일한 값을 수정하려고 하면 필연적으로 문제가 발생함
- 그런 경우 DataContext는 두 번째 사용자가 SubmitChanges 요청을 하는 순간 ChangeConflictException을 발생시킬 것
- 예외상황을 발생시킬 가능성이 있을 때에는 업데이트를 잘 구조화된 예외처리 블록 내에서 수행할 필요가 있음

- 예외가 발생하면 이 예외상황을 잘 해결하기 위한 몇몇 옵션들이 있음
- DataContext는 단순히 충돌의 당사자가 되는 객체들 뿐만 아니라 원래의 값, 변한한 값, DB내부의 값 사이의 불일치도 검출해냄
- 이런 수준의 정보를 제공하기 위해 RefreshMode를 설정하여 충돌을 일으키는 레코드들이 데이터베이스에서 새로 수정될 수 있게 함
- [값이 변하면서 생기는 충돌을 KeepChanges로 해결하기]
```C#
try
{
    context.SubmitChanges(ConflictMode.ContinueOnConflict);
}
catch(ChangeConflictException)
{
    context.ChangeConflicts.ResolveAll(RefreshMode.KeepChanges);

    context.SubmitChanges();
}
```
- 만약 KeepChanges 옵션을 사용하면 변화된 값을 다시 확인할 필요 없이 단순히 값들이 맞다고 단언, 적절한 행에 그 데이터를 집어넣음
- 이러한 "마지막 사람이 승리하는" 형태의 메소드는 수정하지 않은 열들을 데이터베이스의 현재 값으로 채워질 것이라는 잠재적인 위험을 내포함

- 만약 업무상 필요하다면, 변화들을 DB에서 가져온 새 값들과 병합 가능
- 단순히 RefreshMode를 KeepCurrentValues로 바꿔주기만 하면 됨

- 이런 방법으로 다른 사용자가 만든 변화들을 레코드로 가져와 변화를 더할 수 있음
- 그러나 만약 두 사용자 모두 같은 열에 대해 수정했다면 최종적으로 수정한 값이 첫번째 업데이트된 값을 덮어쓰게 될 것

- 안전하게 두 번째 사용자가 바꾼 값을 무시하고 DB에서 불러온 값을 설정할 수도 있음
- 그런 경우에는 RefreshMode.OverwriteCurrentValues 옵션을 이용함
- 이 시점에서 변화를 다시 데이터베이스에 저장하는 것은 그다지 바람직한 선택은 아닐 것.
- 왜냐하면 현재의 객체와 DB의 값 사이에 차이가 없을 것이기 때문임.
- 다시 가져온 값을 사용자에게 제공하여 적절히 수정가능하도록 해줌

- 사용자가 가해준 변화의 수에 따라서 사용자는 다시 값을 전부 입력하는 것을 매우 싫어할 수도 있음
- SubmitChanges는 하나의 명령으로 여러 개의 레코드를 한꺼번에 업데이트할 수 있게 해주기 떄문에 변화의 수는 중요하지 않음
- 이를 도와주기 위해, SubmitChanges 메소드는 오버로딩된 값 하나를 매개변수로 받아들여 충돌을 어떻게 해결하는지 알고 있음
- 즉, 더 이상의 해석을 중지하거나 충돌이 있는 객체들을 목록으로 모음
- ConflictMode는 FailOnFirstConflict와 ContinueOnConflict라는 두 가지 옵션을 담고 있음

- ContinueOnConflict 옵션이 설정되면 충돌이 있는 옵션들 속을 확인하면서 적절한 RefreshMode를 통해 충돌을 해결해줘야 함
- 다음 코드는 충돌하지 않는 레코드를 저장하고 성공적이지 못했던 값들을 덮어쓰는 과정을 보여줌
- [사용자의 값들을 DB에서 가져온 값들로 대체하기]
```C#
try
{
  context.SubmitChanges(ConflictMode.ContinueOnConflict);
}
catch
{
  context.ChangeConflicts.ResolveAll(RefreshMode.OverwriteCurrentValues);
}
```
- 이 메소드를 사용하면 최소한 몇 개의 값을 우선 저장한 후 사용자에게 충돌이 발생한 항목들을 재입력할 것을 요구 가능
- 모든 변화를 직접 추적하여 어떤 레코드를 변경해야 하는지 확인해야 하는 사용자 입장에서는 귀찮을 수도...
- 더 나은 방법은 사용자들에게 변경된 레코드와 항목들을 보여주는 것일 수도 있음
- LINQ to SQL은 이런 정보에 대한 접근을 허용, 충돌하는 항목의 현재 값, 원래 값, 데이터베이스에 저장된 값을 대조할 수 있게 해줌
- 다음의 코드는 DataContext의 ChangeConflicts 컬렉션을 이용해서 각 충돌의 세부사항들을 정리할 수 있게 해줌
- [충돌에 대한 상세한 정보를 보여주기]
```C#
try
{
    context.SubmitChanges(ConflictMode.ContinueOnConflict);
}
catch(ChangeConflictException)
{
    var exceptionDetail = 
        from conflict in context.ChangeConflicts
        from member in conflict.MemberConflicts
        //ChangeConflicts 컬렉션 각각의 항목은 충돌이 발생한 객체와 MembersConflicts 컬렉션을 포함함
        select new
        {
            TableName = context.GetTableName(conflict.Object),
            MemberName = member.Member.Name,
            CurrentValue = member.CurrentValue.ToString(),
            DatabaseValue = member.DatabaseValue.ToString(),
            OriginalVaue = member.OriginalValue.ToString()
        };
        //이 컬렉션은 Memebr, CurrentValue, DatabaseValue, OriginalVaue에 관한 정보를 포함함
        //이런 정보를 갖고 있으면 사용자에게 원하는 형태로 보여줄 수 있음
    ObjectDumper.Write(exceptionDetail);
}
```
- 이 코드를 이용하면 사용자가 발생시키는 동기화 관련 에러들에 대한 세부정보를 show 가능 
- 두 사용자가 동시에 책의 가격을 수정하려고 하는 경우, 첫 번째 사용자가 가격을 2달러 올리고, 두 번째 사용자가 가격을 1달러 낮추려고 하면 어떤 일이 발생할까?
    - 첫 번째 수정하는 사용자는 문제 없지만 두 번째 사용자가 수정사항을 제출하려고 하는 순간 ChangeConflictException이 발생하게 될 것임
    - 발생한 exceptionDetail에 관한 정보들을 손쉽게 확인 가능
- 충돌에 관한 세부내용을 알고 나면 두 번째 사용자는 어떻게 그 상황에서 각각의 레코드를 처리해야 하는지에 대한 올바른 판단 내릴 수 있음

- DataContext는 단순히 DB에 대한 연결만을 위한 객체가 아니라는 것은 매우 중요함
- 이것은 기본적으로 모든 변화를 추적, 동기화 관리를 하는 역할을 수행
- 개발자는 낙관적 동기화 옵션을 중지시키기 위해 추가적인 일을 해야 함

- 복수의 동시 사용자를 허용하는 시스템을 설계하는 과정에서 동기화 관련 문제들을 어떻게 처리할지 고려해야 함
- 대부분의 경우 ChangeConflictException이 발생할 지 안 할지의 여부는 별로 안 중요함
- 중요한 것은 **언제** 발생하느냐임
- 이것은 예외를 캐치함으로써 발생한 모든 트랜잭션을 롤백하거나 갈등상황을 처리하는 옵션들 중 하나에 의존하게 됨

### 8.1.4 트랜잭션을 이용하여 충돌을 해결하기
- 동기화 옵션을 거론할 때, 데이터베이스를 SubmitChanges를 통해 업데이트하면 하나 또는 여러 개의 레코드를 업데이트할 수 있다고 했음
- 만약 충돌상황을 접하게 되면 어떻게 해결할 것인지 선택이 가능함
- 그러나... 만약 변화를 롤백하기 위한 노력이 가해지지 않는다면 예외상황 이전에 저장된 변화들은 데이터베이스에 제출될 것이라는 점을 알아야 함
- 이것은 데이터베이스가 무결성을 갖게 되는 중대한 원인이 될 수 있음

- 왜 어떤 레코드는 저장하고 어떤 레코드는 저장하지 않는 것이 나쁜 건가?
    - 컴퓨터 상점에 가서 새 컴퓨터를 만들기 위해 몇 가지 부픔을 따로 사는 상황을 가정하자
    - 보통 마더보드, 케이스, 파워, 하드드라이브, 비디오 카드 등을 고른 후 카운터로 가서 계산을 하게 됨
    - 스마트한 판매원은 메모리와 프로ㅔ서가 빠졌다는 사실을 알아챔 -> 잠시 둘러보고 자신의 가게에는 호환되는 프로세서가 없다는 것을 알려줄 것임
    - 그런 경우 선택한 부품들을 사고 다른 가게에 호환되는 프로세서가 있을 것을 기대하면서 살 것인지 아니면 모두 구매 안 할 건지 판단해야 함

- 방금의 컴퓨터를 데이터베이스에 비유하면...
    - 부품들: 업데이트되어야 할 비즈니스 객체들
    - 판매원: DataContext
    - 판매원이 문제를 발견하는 것: DataContext가 ChangeConflictException을 발생시키는 것에 비교 가능
    - 첫 번쨰 방안: 우선 살 수 있는 것을 사는 선택
        - ConflictMode.ContinueOnConflict 사용해서 그냥 충돌 무시하고 지나가는 것
        - DataContext는 충돌 발생 전에 어떻게 그런 상황을 처리하는지 확실히 알고 있어야 함
    - 두 번째 방안: DB에서 원래의 상태로 모두 환원시킨 후 다시 어떤 변화를 수행할지 결정하는 과정
    - 세 번째 방안: 그냥 집에 가기로 함
        - 시도한 모든 변화는 무시되고 원래 상태로 환원됨

- LINQ to SQL은 트랜잭션을 관리하기 위한 세 가지 주요 방법을 제공함
    - 첫 번째 옵션: 기본 옵션
        - SubmitChanges가 호출될 떄 트랜잭션을 생성하는 방법
        - ConflictMode 옵션이 어떻게 설정되어 있느냐에 따라 자동적으로 변화를 환원시켜 줄 것
    - 두 번쨰 옵션: 수동으로 트랜잭션을 관리하고 싶다면 DataContext가 이미 관리하고 있는 연결을 사용하는 옵션
        - 이런 경우, Begin Transaction을 DataContext.Connection에 대해 변화를 적용시키기 전에 수행 
        - 변화가 모두 적용된 후에는 그 결과를 제출하거나 로랩ㄱ하는 선택을 할 수 있음
        - [DataContext를 통해 트랜잭션을 관리하기]
        ```C#
        try
        {
            context.Connection.Open();
            context.Transaction = context.Connection.BeginTransaction();
            context.SubmitChanges(ConflictMode.ContinueOncConflict);
            context.Transaction.Commit();
        }
        catch(ChangedConflictException)
        {
            context.Transaction.Rollback();
        }
        ```
        - DataContext를 이용하여 트랜잭션을 관리하게 되면 여러 개의 DB 연결을 유지하거나 여러 개의 DataCOntext 객체를 다룰 수 없다는 단점이 있음
    - 세 번째 옵션: .NET framework 2.0에서 소개된 System.Transactions.Transactioncope는 여러 연결에 대해 매끄럽게 대응 가능하도록 설계됨 <- 이것 사용 위해서 System.Transaction 라이브러리를 추가 참조해줘야 함
        - 이 객체는 데이터 소통을 그것이 다루는 객체에 맞춰서 자동적으로 확장되며 만약 범주가 하나의 DB에 대한 호출로 국한된다면 매우 간단한 DB 트랜잭션을 이용할 것임
        - 그러나 이것이 여러 개의 클래스와 여러 개의 연결을 필요로 한다면 자동적으로 좀 더 강력한 기업형 트랜잭션으로 전환될 것
        - TransactionScope는 명시적으로 트랜잭션을 시작하거나 롤백할 것을 요구하지 않음
        - 개발자는 그걸 완벽하게 종결짓기만 하면 됨
        - [TransactionScope 객체로 트랜잭션을 관리하기]
        ```C#
        using(System.Transactions.TransactionScope scope =
              new System.Transactions.TransactionScope())
        {
            context.SubmitChanges(ConflictMode.ContinueOnConflict);
            scope.Complete();
        }
        ```
        - 이렇게 하면 다른 동기화 처리 매커니즘과 달리 수행된 트랜잭션을 롤백하기 위해 try-catch문을 이용할 필요가 없음
        - TransactionScope를 이용하면 Complete 메소드를 호출하지 않은 트랜잭션에 대해 자동을 롤백이 수행됨
        - 만약 SubmitChanges에서 예외상황이 발생하면 Complete 메소드를 건너뛰게 될 것
        - 결론: 트랜잭션을 롤백하기 위해 노력할 필요 없음, 그래도 예외처리 부분을 명시해줘야 하긴 함

        - TransactionScope 객체의 진정한 장점: 주어진 문맥에 맞게 범위를 자동으로 설정해줌
        - 로컬 트랜잭션과도 함께 잘 동작, 여러 가지 데이터가 혼재된 경우에도 잘 동작함
        - LINQ to SQL을 사용 시 TransactionScope는 유연함을 통해 자연스러운 규모 선택을 가능하게 해주는 매우 뛰어난 방식
        
## 8.2 고급 데이터베이스 기능
- 많은 경우에 간단한 CRUD 작업은 테이블과 객체 간의 기본적인 매핑만으로 충분함
- 그러나 가끔 기본적인 매핑에 의한 직접적인 관계만으로는 충분하지 않은 경우가 있음

### 8.2.1 SQL 전달 : SQL 질의에서 객체를 반환하기

### 8.2.2 저장된 프로시저와 작업하기

### 8.2.3 사용자 정의 함수


## 8.3 비즈니스 계층을 개선하기

### 8.3.1 컴파일된 질의

### 8.3.2 사용자 정의 비즈니스 로직을 위한 부분 클래스

### 8.3.3 부분 메소드의 장점들을 활용하기

### 8.3.4 객체 상속을 이용하기

## 8.4 LINQ to Entities로 잠시 눈길을 돌리기
