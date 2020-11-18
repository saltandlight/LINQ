# Cast와 OfType, Contain
## Cast
- 요소들을 내가 원하는 형식을 지정해서 캐스팅함
- 지연 실행 사용
- **원하는 형식으로 변환이 안 될 경우 Exception throw**
- 변환이라는 것은 본연의 타입 값을 살려주는 것임! string을 int로 바꿔줄 수는 없음(이 경우 에러가 발생!)
- 사실 이런 이유로 에러가 날 가능성이 있기 때문에 명확한 형이 아니라면 OfType이 적절함

## OfType
- 요소를 필터링함
- 지연 실행 사용
- 개체가 열거될 때까지 실행되지 않음
- 딱 그 타입인 아이만 필터링을 해서 뽑아냄
- Array에 int 형, string 형, double 형이 있다고 하자
```C#
var arr = new ArrayList();
arr.Add("4");
arr.Add(1);
arr.Add(1.4f);
arr.Add(3);

var numlist = arr.OfType(typeof<Integer>);
```
- 이 경우 numlist에는 1과 3만 담김
- 에러날 것 없이, 내가 원하는 타입의 아이들만 골라서 담는다!!

## Contain
- 말 그대로 포함된 애들만 걸러서 가져옴
- foreach로 같은지를 걸러줄 필요가 없이 Contain을 쓰면 해당 요소가 포함되어 있는지 알 수가 있다.
