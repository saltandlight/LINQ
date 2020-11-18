# LINQ의 Count()와 List 의 Count 차이🕵🏻‍♀️
- LINQ의 Count():
    - Count를 가져오는 게 아니라, LINQ에서 다시 한 번 해당 자료구조의 구성요소 개수를 셈
    - O(N)의 시간복잡도를 가짐
- List의 Count:
    - 세지 않고 개수를 가져옴
    - O(1)의 시간복잡도를 가짐
    - 왠만하면 이 방식을 채택하기!