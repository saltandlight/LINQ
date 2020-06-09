# XmlParsing✨
## 함수들
### XContainer가 가지고 있는 메서드
- Element
    - XName을 갖는 문서 순으로 첫 번째 자식을 반환함
- Elements
    - 문서 순서에 따라 필터링된 자식 컬렉션을(자식들 중에 해당되는 요소들을) 모두 반환함
### XElement가 가지고 있는 메서드
- Attribute
    - 지정된 XAttribute가 있는 이 XElement의 XName을 반환함!
## LINQ 사용 시 생각해야 할 것
- 지연 실행을 방지하고 싶다면 ToList()함수를 사용!

## 팁
- 언제나 함수의 명세를 잘 확인하자!(특히 막힐 때는 이렇게 하기!)
- 뭔가 어렵거나 복잡해보여도 좋은 아이디어가 있어보이면 시도해보기!
- 참고사이트: https://docs.microsoft.com/ko-kr/dotnet/api/system.xml.linq.xelement.attribute?view=netcore-3.1