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
        /************** Utils ********************/

        /// <summary>
        /// Converts object to decimal value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static decimal FromDBValToDecimal(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(decimal);
            }
            else
            {
                return Convert.ToDecimal(obj) / 100;
            }
        }

        /// <summary>
        /// Converts decimal value to int value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int FromDecimaltoDBInt(decimal value)
        {
            return decimal.ToInt32(value * 100);
        }
        // TODO Simplify
        private bool ExecuteNonQueryInsert(out int rowid, string sql, params SQLiteParameter[] parameters)
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
                rowid = Convert.ToInt32(connection.LastInsertRowId);
                return true;
            }
            catch (SQLiteException)
            {
                rowid = -1;
                return false;
            }
        }
        private bool ExecuteNonQuery(string sql, params SQLiteParameter[] parameters)
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (SQLiteException)
            {
                return false;
            }
        }
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

                sql = "CREATE TABLE IF NOT EXISTS Accounts(name TEXT, " +
                    "type TEXT, balance INTEGER, closed INTEGER, exbudget INTEGER, " +
                    "FOREIGN KEY(type) REFERENCES AccountTypes(name) ON DELETE RESTRICT)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                sql = "CREATE TABLE IF NOT EXISTS Categories(name TEXT NOT NULL UNIQUE)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                sql = "CREATE TABLE IF NOT EXISTS Subcategories(name TEXT, parent TEXT, UNIQUE(name, parent), " +
                    "FOREIGN KEY(parent) REFERENCES Categories(name) ON DELETE RESTRICT)";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                // TODO !!! !

                //sql = "CREATE TABLE IF NOT EXISTS Transactions(date DATE, " +
                //    "amount INTEGER, info TEXT, acc_id INTEGER, category_id INTEGER)";
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

        /************** Accounts *****************/
        /// <summary>
        /// Selects every account from DB.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(string name, string type, decimal balance, bool closed, bool excluded, int id)> SelectAccounts()
        {
            string sql = "SELECT *, rowid FROM Accounts";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(0), dr.GetString(1), FromDBValToDecimal(dr.GetDecimal(2)),
                        Convert.ToBoolean(dr.GetInt32(3)), Convert.ToBoolean(dr.GetInt32(4)), dr.GetInt32(5));
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Adds new account to DB. Method will not restrict non-unique account names.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool AddAccount(string name, string type, out int id)
        {
            // Can't add empty account name
            if (name == string.Empty)
            {
                id = -1;
                return false;
            }

            string sql = "INSERT INTO Accounts VALUES(@name, @type, 0, 0, 0)";
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
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@type",
                        DbType = System.Data.DbType.String,
                        Value = type
                    });
                    cmd.ExecuteNonQuery();
                }
                id = Convert.ToInt32(connection.LastInsertRowId);
                return true;
            }
            catch (SQLiteException)
            {
                id = -1;
                return false;
            }
        }
        /// <summary>
        /// Writes account changes to DB.
        /// </summary>
        public bool UpdateAccount(int id, string type, decimal balance, bool closed, bool excluded)
        {
            string sql = "UPDATE Accounts SET type=@type, balance=@balance, closed=@closed, " +
                "exbudget=@excluded WHERE rowid=@rowid";
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@type",
                        DbType = System.Data.DbType.String,
                        Value = type
                    });
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@balance",
                        DbType = System.Data.DbType.Int32,
                        Value = FromDecimaltoDBInt(balance)
                    });
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@closed",
                        DbType = System.Data.DbType.Int32,
                        Value = Convert.ToInt32(closed)
                    });
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@excluded",
                        DbType = System.Data.DbType.Int32,
                        Value = Convert.ToInt32(excluded)
                    });
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@rowid",
                        DbType = System.Data.DbType.Int32,
                        Value = id
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
        /// Deletes account from DB.
        /// </summary>
        /// <returns></returns>
        public bool DeleteAccount(int id)
        {
            // TODO Check foreign key transaction
            string sql = "DELETE FROM Accounts WHERE rowid=@rowid";
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.Add(new SQLiteParameter()
                    {
                        ParameterName = "@rowid",
                        DbType = System.Data.DbType.Int32,
                        Value = id
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
        /************** Categories *****************/
        public IEnumerable<(string name, int id)> SelectTopCategories()
        {
            string sql = "SELECT name, rowid FROM Categories ORDER BY name ASC";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(0), dr.GetInt32(1));
                }
                dr.Close();
            }
        }

        public IEnumerable<(string name, int id)> SelectSubCategories(string parent)
        {
            string sql = "SELECT name, rowid FROM Subcategories WHERE parent=@parent";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@parent",
                    DbType = System.Data.DbType.String,
                    Value = parent
                });
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(0), dr.GetInt32(1));
                }
                dr.Close();
            }
        }

        public bool AddTopCategory(string name, out int id)
        {
            // Can't add empty name 
            if (name == string.Empty)
            {
                id = -1;
                return false;
            }

            string sql = "INSERT INTO Categories VALUES(@name)";
            var param = new SQLiteParameter()
            {
                ParameterName = "@name",
                DbType = System.Data.DbType.String,
                Value = name
            };
            if (ExecuteNonQueryInsert(out int rowid, sql, param))
            {
                id = rowid;
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
        }

        public bool AddSubCategory(string name, string parent, out int id)
        {
            // Can't add empty name 
            if (name == string.Empty)
            {
                id = -1;
                return false;
            }

            string sql = "INSERT INTO Subcategories VALUES(@name, @parent)";
            var nameParam = new SQLiteParameter()
            {
                ParameterName = "@name",
                DbType = System.Data.DbType.String,
                Value = name
            };
            var parentParam = new SQLiteParameter()
            {
                ParameterName = "@parent",
                DbType = System.Data.DbType.String,
                Value = parent
            };
            if (ExecuteNonQueryInsert(out int rowid, sql, nameParam, parentParam))
            {
                id = rowid;
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
        }

        public bool DeleteTopCategory(string name)
        {
            string sql = "DELETE FROM Categories WHERE name=@name";
            var param = new SQLiteParameter()
            {
                ParameterName = "@name",
                DbType = System.Data.DbType.String,
                Value = name
            };
            return ExecuteNonQuery(sql, param);
        }

        public bool DeleteSubCategory(int id)
        {
            // TODO check foreign key transaction and budget record
            string sql = " DELETE FROM Subcategories WHERE rowid=@rowid";
            var param = new SQLiteParameter()
            {
                ParameterName = "@rowid",
                DbType = System.Data.DbType.Int32,
                Value = id
            };
            return ExecuteNonQuery(sql, param);
        }
    }
}
