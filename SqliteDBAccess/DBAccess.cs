using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using Dapper;


namespace SqliteDBAccess {
    public class DBAccess : IDisposable {

        private readonly SQLiteConnection ConectinObject;

        private SQLiteTransaction tran;

        private DynamicParameters Parameters = new DynamicParameters();


        /// <summary>
        /// パラメーター設定
        /// </summary>
        /// <param name="parameterName">プレースホルダ名</param>
        /// <param name="parameter">値</param>
        public void SetParameters(string parameterName, object parameter = null) {
            this.Parameters.Add(parameterName, parameter);
        }
        

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="connectionName">configファイルの接続文字列セクションのnameを設定</param>
        public DBAccess(string connectionName) {
            //参照元のconfigファイルから接続文字列を取得
            var conectionString = new SQLiteConnectionStringBuilder() {
                DataSource = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString
            };

            ConectinObject = new SQLiteConnection(conectionString.ToString());
            ConectinObject.Open();
        }


        /// <summary>
        /// SELECTを実行
        /// </summary>
        /// <typeparam name="T">取得したい型</typeparam>
        /// <param name="sql">SELECT文</param>
        /// <returns></returns>
        public IEnumerable<T> Query<T>(string sql) {
            return ConectinObject.Query<T>(sql, Parameters);
        }


        /// <summary>
        /// DML文を実行
        /// </summary>
        /// <param name="sql">DML文</param>
        /// <returns>実行時影響があったレコード数</returns>
        public int Execute(string sql) {
            return ConectinObject.Execute(sql, Parameters);
        }


        /// <summary>
        /// Transaction
        /// </summary>
        public void BeginTransaction() {
            this.tran = ConectinObject.BeginTransaction();
        }


        /// <summary>
        /// Commit
        /// </summary>
        public void Commit() {
            try {
                this.tran?.Commit();
            } catch {
                this.RollBack();
                throw;
            }
        }


        /// <summary>
        /// Rollback
        /// </summary>
        public void RollBack() {
            this.tran?.Rollback();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                }

                this.tran?.Dispose();
                this.ConectinObject?.Close();
                this.ConectinObject?.Dispose();

                disposedValue = true;
            }
        }

        ~DBAccess() {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(false);
        }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        void IDisposable.Dispose() {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
