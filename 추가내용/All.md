# All
- public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);
- 이런 방식인데, 시퀀스의 모든 요소가 조건을 만족하는지 여부를 결정함
- predicate를 만족하냐 안 하냐에 따라 true, false를 리턴함