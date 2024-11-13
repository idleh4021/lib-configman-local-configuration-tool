
# ConfigMan
<!--![배지 또는 로고 이미지 (선택사항)](링크)-->
<!--프로젝트에 대한 간단한 설명을 여기에 작성합니다.-->
로컬에 저장되는 설정정보(Configuration)을 쉽게 관리할 수 있는 라이브러리 입니다.

## 목차
- [소개](#소개)<!--- [설치](#설치)-->
- [설치](#설치)
- [사용법](#사용법)
- [예제](#예제)
<!--- [기여](#기여)
- [라이선스](#라이선스)
- [문의](#문의)
-->
## 소개
<!--프로젝트에 대한 자세한 설명을 여기에 작성합니다.  -->
- **개발환경** : C#, .Net FrameWork 4.7.2 
- **주요기능** : 저장,읽기,삭제
```
  ConfigManager config = new ConfigManager("TesterSystem","config1");
  TestConfig test = new TestConfig();
  test.name = "23";
  test.maxValue= 100;
  test.minValue = 0;
  test.enabled= true;
  
  //저장
  config.Save(test);
  //읽기
  object result = config.Open(typeof(TestConfig));
  //삭제
  config.Remove();
```

## 설치

NewtonSoft.Json 패키지를 필요로 합니다.

```
install-package newtonsoft.Json
```

## 사용법
1. ConfigMan 참조추가
2. ConfigManager 인스턴스 생성
   ```
    ConfigManager config = new ConfigManager("ProgramName","configfileName");
   ```
   * 기본 저장경로 = 내문서
   * "ProgramName" : 폴더명
   * "configfileName" : Config 파일명

3. Config 데이터 입력
   * 데이터타입은 DataTable,HashTable,Class 등

4. 저장 / 읽기 / 삭제 
   ```
    //저장
    config.Save(test);
    //읽기
    object result = config.Open(typeof(TestConfig));
    //삭제
    config.Remove();
   ```

 ## 예제
 
 ```
  static void Main(string[] args)
 {
     ConfigManager config = new ConfigManager("ProgramName","configfileName");
     TestConfig test = new TestConfig();
     test.name = "23";
     test.maxValue= 100;
     test.minValue = 0;
     test.enabled= true;
     config.Save(test);
    
     object result = config.Open(typeof(TestConfig));
     if (result is TestConfig) Console.WriteLine(((TestConfig)result).name);
     else Console.WriteLine("Fail");
   
 }

 class TestConfig
 {
     public string name;
     public int minValue;
     public int maxValue;
     public bool enabled;
     public string address;
 }
 ```

