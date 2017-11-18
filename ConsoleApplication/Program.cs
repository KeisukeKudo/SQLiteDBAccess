using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqliteDBAccess;


namespace ConsoleApplication {
    class Program {
        static void Main(string[] args) {
            using (var dba = new DBAccess("TestRecord")) {
                CreateTable(dba);
                Console.WriteLine(InsertRecord(dba));

                foreach (var e in Query(dba)) {
                    Console.WriteLine($"ID: {e.Id} NAME: {e.Name} AGE: {e.Age}");
                }

                Console.WriteLine(UpdateRecod(dba));

                foreach (var e in Query(dba)) {
                    Console.WriteLine($"ID: {e.Id} NAME: {e.Name} AGE: {e.Age}");
                }
            }
        }

        /// <summary>
        /// TESTテーブルを作成
        /// </summary>
        /// <param name="dba"></param>
        private static void CreateTable(DBAccess dba) {
            var sql = @"
CREATE TABLE IF NOT EXISTS employee (
    Id INTEGER PRIMARY KEY AUTOINCREMENT
   ,Name TEXT
   ,Age INTEGER
)";
            dba.BeginTransaction();
            dba.Execute(sql);
            dba.Commit();
        }


        /// <summary>
        /// レコードを追加
        /// </summary>
        /// <param name="dba"></param>
        /// <returns></returns>
        private static int InsertRecord(DBAccess dba) {
            dba.SetParameters("Name", "Kudo");
            dba.SetParameters("Age", 30);
            var sql  = @"
INSERT INTO employee
(Name, Age)
VALUES
(@Name, @Age)";

            dba.BeginTransaction();
            var result = dba.Execute(sql);
            dba.Commit();
            return result;
        }


        /// <summary>
        /// レコードを更新
        /// </summary>
        /// <param name="dba"></param>
        /// <returns></returns>
        private static int UpdateRecod(DBAccess dba) {
            dba.SetParameters("Name", "Kudo");
            dba.SetParameters("Age", 31); // 30から31にアップロード
            var sql = @"
UPDATE employee
SET Age = @Age
WHERE Name = @Name";

            dba.BeginTransaction();
            var result = dba.Execute(sql);
            dba.Commit();
            return result;
        }


        /// <summary>
        /// 全データを取得
        /// </summary>
        /// <param name="dba"></param>
        /// <returns></returns>
        private static IEnumerable<TestEntity> Query(DBAccess dba) {
            var sql = @"
SELECT * FROM employee";
            return dba.Query<TestEntity>(sql);
        }



        /// <summary>
        /// TEST用Entityクラス
        /// </summary>
        class TestEntity {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}
