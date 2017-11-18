# SQLiteDBAccess

SQLiteのデータベースアクセスユーティリティです｡  
利用には[Dapper](https://github.com/StackExchange/Dapper)､[SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core/)が必要です｡  

### App.config or Web.config
```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <connectionStrings>
        <add name="TestRecord" connectionString="./record.db" providerName="System.Data.SQLite" />
    </connectionStrings>
    ･･･
</configuration>
```  

### パラメーターの設定  
```C#
dba.SetParameters("Name", "Kudo");
dba.SetParameters("Age", 30);
```

## 使い方
以下のようなテーブル構成を想定
```SQL
CREATE TABLE IF NOT EXISTS employee (
    Id INTEGER PRIMARY KEY AUTOINCREMENT
   ,Name TEXT
   ,Age INTEGER
)
```
以下のEntityクラスを用意
```C#
class TestEntity {
  public int Id { get; set; }
  public string Name { get; set; }
  public int Age { get; set; }
}
```
### SELECT文
```C#
var sql = "SELECT * FROM employee";
return dba.Query<TestEntity>(sql)
```
### INSERT文
```C#
dba.SetParameters("Name", "Kudo");
dba.SetParameters("Age", 30);
var sql = @"
INSERT INTO employee
(Name, Age)
VALUES
(@Name, @Age)";

dba.BeginTransaction();
dba.Execute(sql);
dba.Commit();
```
### UPDATE文
```C#
dba.SetParameters("Name", "Kudo");
dba.SetParameters("Age", 31); // 30から31にアップロード
var sql = @"
UPDATE employee
SET Age = @Age
WHERE Name = @Name";

dba.BeginTransaction();
dba.Execute(sql);
dba.Commit();
```
### DELETE文
```C#
dba.SetParameters("Id", 1);
var sql = @"
DELETE FROM employee
WHERE Id = @Id";

dba.BeginTransaction();
dba.Execute(sql);
dba.Commit();
```
