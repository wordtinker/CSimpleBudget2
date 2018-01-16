using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Data
{
    public class Storage : StubStorage, IStorage
    {
        private SQLiteConnection connection;
        private string connString = "Data Source={0};Version=3;foreign keys=True;";

        public Storage(string fileName)
        {
            string cString = string.Format(connString, fileName);
            connection = new SQLiteConnection(cString);
            connection.Open();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    connection.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
        /************** File *****************/
        /// <summary>
        /// Initializes new empty file with proper DB structure.
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            try
            {
                string sql = "CREATE TABLE IF NOT EXISTS AccountTypes(name TEXT NOT NULL UNIQUE)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                // TODO !!! !

                //sql = "CREATE TABLE IF NOT EXISTS Accounts(name TEXT, " +
                //    "type TEXT, balance INTEGER, closed INTEGER, exbudget INTEGER)";
                //using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
                //{
                //    cmd.ExecuteNonQuery();
                //}

                //sql = "CREATE TABLE IF NOT EXISTS Transactions(date DATE, " +
                //    "amount INTEGER, info TEXT, acc_id INTEGER, category_id INTEGER)";
                //using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
                //{
                //    cmd.ExecuteNonQuery();
                //}

                //sql = "CREATE TABLE IF NOT EXISTS Categories(name TEXT UNIQUE)";
                //using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
                //{
                //    cmd.ExecuteNonQuery();
                //}

                //sql = "CREATE TABLE IF NOT EXISTS Subcategories(name TEXT, parent TEXT, UNIQUE(name, parent))";
                //using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
                //{
                //    cmd.ExecuteNonQuery();
                //}

                //sql = "CREATE TABLE IF NOT EXISTS Budget(amount INTEGER, " +
                //    "category_id INTEGER, type TEXT, day INTEGER, year INTEGER, month INTEGER)";
                //using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConn))
                //{
                //    cmd.ExecuteNonQuery();
                //}

                //dbConn.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /************** Acc Types ****************/

        /// <summary>
        /// Selects account types.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> SelectAccountTypes()
        {
            string sql = "SELECT * FROM AccountTypes";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return dr.GetString(0);
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Adds new account type to DB.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool AddAccountType(string name)
        {
            // Can't add empty account type name
            if (name == string.Empty)
            {
                return false;
            }

            string sql = "INSERT INTO AccountTypes VALUES(@name)";
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@name",
                        DbType = System.Data.DbType.String,
                        Value = name
                    });
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }
        /// <summary>
        /// Deletes account type from DB.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteAccountType(string name)
        {
            // TODO account foreign key check
            string sql = "DELETE FROM AccountTypes WHERE name=@name";
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@name",
                        DbType = System.Data.DbType.String,
                        Value = name
                    });
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }
    }
}
