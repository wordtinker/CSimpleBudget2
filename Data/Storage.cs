using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Data
{
    public class Storage : IStorage
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
            string query =
                @"CREATE TABLE IF NOT EXISTS AccountTypes(name TEXT NOT NULL UNIQUE);
                  CREATE TABLE IF NOT EXISTS Accounts(id INTEGER PRIMARY KEY, name TEXT,
                  type TEXT, balance INTEGER, closed INTEGER, exbudget INTEGER,
                  FOREIGN KEY(type) REFERENCES AccountTypes(name) ON DELETE RESTRICT);
                  CREATE TABLE IF NOT EXISTS Categories(name TEXT NOT NULL UNIQUE);
                  CREATE TABLE IF NOT EXISTS Subcategories(id INTEGER PRIMARY KEY, name TEXT,
                  parent TEXT, UNIQUE(name, parent), FOREIGN KEY(parent)
                  REFERENCES Categories(name) ON DELETE RESTRICT);
                  CREATE TABLE IF NOT EXISTS Transactions(date DATE, amount INTEGER,
                  info TEXT, accId INTEGER, categoryId INTEGER,
                  FOREIGN KEY(accId) REFERENCES Accounts(id) ON DELETE RESTRICT,
                  FOREIGN KEY(categoryId) REFERENCES Subcategories(id) ON DELETE RESTRICT);
                  CREATE TABLE IF NOT EXISTS Budget(amount INTEGER, categoryId INTEGER,
                  type TEXT, day INTEGER, year INTEGER, month INTEGER,
                  FOREIGN KEY(categoryId) REFERENCES Subcategories(id) ON DELETE RESTRICT)";
            return ExecuteNonQuery(query);
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
            var param = new SQLiteParameter()
            {
                ParameterName = "@name",
                DbType = System.Data.DbType.String,
                Value = name
            };
            return ExecuteNonQuery(sql, param);
        }
        /// <summary>
        /// Deletes account type from DB.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool DeleteAccountType(string name)
        {
            string sql = "DELETE FROM AccountTypes WHERE name=@name";
            var param = new SQLiteParameter()
            {
                ParameterName = "@name",
                DbType = System.Data.DbType.String,
                Value = name
            };
            return ExecuteNonQuery(sql, param);
        }
        /************** Accounts *****************/
        /// <summary>
        /// Selects every account from DB.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(string name, string type, decimal balance, bool closed, bool excluded, int id)> SelectAccounts()
        {
            string sql = "SELECT * FROM Accounts";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetString(1), dr.GetString(2), FromDBValToDecimal(dr.GetDecimal(3)),
                        Convert.ToBoolean(dr.GetInt32(4)), Convert.ToBoolean(dr.GetInt32(5)), dr.GetInt32(0));
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

            string sql = "INSERT INTO Accounts(name, type, balance, closed, exbudget) VALUES(@name, @type, 0, 0, 0)";
            var nameParam = new SQLiteParameter()
            {
                ParameterName = "@name",
                DbType = System.Data.DbType.String,
                Value = name
            };
            var typeParam = new SQLiteParameter()
            {
                ParameterName = "@type",
                DbType = System.Data.DbType.String,
                Value = type
            };
            return ExecuteNonQueryInsert(out id, sql, nameParam, typeParam);
        }
        /// <summary>
        /// Writes account changes to DB.
        /// </summary>
        public bool UpdateAccount(int id, string type, decimal balance, bool closed, bool excluded)
        {
            string sql = "UPDATE Accounts SET type=@type, balance=@balance, closed=@closed, " +
                "exbudget=@excluded WHERE id=@id";
            var parameters = new[]
            {
                new SQLiteParameter()
                    {
                        ParameterName = "@type",
                        DbType = System.Data.DbType.String,
                        Value = type
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@balance",
                        DbType = System.Data.DbType.Int32,
                        Value = FromDecimaltoDBInt(balance)
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@closed",
                        DbType = System.Data.DbType.Int32,
                        Value = Convert.ToInt32(closed)
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@excluded",
                        DbType = System.Data.DbType.Int32,
                        Value = Convert.ToInt32(excluded)
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@id",
                        DbType = System.Data.DbType.Int32,
                        Value = id
                    }
            };
            return ExecuteNonQuery(sql, parameters);
        }

        /// <summary>
        /// Deletes account from DB.
        /// </summary>
        /// <returns></returns>
        public bool DeleteAccount(int id)
        {
            string sql = "DELETE FROM Accounts WHERE id=@id";
            var param = new SQLiteParameter()
            {
                ParameterName = "@id",
                DbType = System.Data.DbType.Int32,
                Value = id
            };
            return ExecuteNonQuery(sql, param);
        }

        private int GetAccountIdForTransaction(int transactionId)
        {
            string sql = "SELECT accId FROM Transactions WHERE rowid=@id";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@id",
                    DbType = System.Data.DbType.Int32,
                    Value = transactionId
                });
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        /// <summary>
        /// Recalculates total for a given account. 
        /// </summary>
        private void UpdateTotal(int accountId)
        {
            string sql =
                @"UPDATE Accounts SET balance=(SELECT SUM(amount)
                  FROM Transactions WHERE accId=@id) WHERE id=@id";
            ExecuteNonQuery(sql, new SQLiteParameter()
            {
                ParameterName = "@id",
                DbType = System.Data.DbType.Int32,
                Value = accountId
            });
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
            string sql = "SELECT name, id FROM Subcategories WHERE parent=@parent";
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
            return ExecuteNonQueryInsert(out id, sql, param);
        }
        public bool AddSubCategory(string name, string parent, out int id)
        {
            // Can't add empty name 
            if (name == string.Empty)
            {
                id = -1;
                return false;
            }

            string sql = "INSERT INTO Subcategories(name, parent) VALUES(@name, @parent)";
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
            return ExecuteNonQueryInsert(out id, sql, nameParam, parentParam);
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
            // TODO check foreign key budget record
            string sql = " DELETE FROM Subcategories WHERE id=@id";
            var param = new SQLiteParameter()
            {
                ParameterName = "@id",
                DbType = System.Data.DbType.Int32,
                Value = id
            };
            return ExecuteNonQuery(sql, param);
        }
        /************** Transactions *****************/
        /// <summary>
        /// Selects all transactions for a given account.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int id)>
            SelectTransactions(int accountId)
        {
            string sql = "SELECT date, amount, info, categoryId, rowid FROM Transactions " +
                         "WHERE accId=@id ORDER BY date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@id",
                    DbType = System.Data.DbType.Int32,
                    Value = accountId
                });
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetDateTime(0), FromDBValToDecimal(dr.GetDecimal(1)),
                        dr.GetString(2), dr.GetInt32(3), dr.GetInt32(4));
                }
                dr.Close();
            }
        }
        public IEnumerable<(DateTime date, decimal amount, string info, int categoryId, int accountId, int id)>
            SelectTransactions(int year, int month, int categoryId)
        {
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
            DateTime lastDayofPrevMonth = firstDayOfMonth.AddSeconds(-1);

            string sql = @"SELECT date, amount, info, categoryId, t.rowid, t.accId FROM Transactions as t
                           INNER JOIN Accounts as a
                           on t.accId = a.rowid
                           WHERE date>@startDate and date<=@endDate
                           AND categoryId=@catId
                           AND exbudget = 0
                           ORDER BY date DESC";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@catId",
                    DbType = System.Data.DbType.Int32,
                    Value = categoryId
                });
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@startDate",
                    DbType = System.Data.DbType.Date,
                    Value = lastDayofPrevMonth
                });
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@endDate",
                    DbType = System.Data.DbType.Date,
                    Value = lastDayOfMonth
                });
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (dr.GetDateTime(0), FromDBValToDecimal(dr.GetDecimal(1)), dr.GetString(2),
                        dr.GetInt32(3), dr.GetInt32(5), dr.GetInt32(4));
                }
                dr.Close();
            }
        }
        /// <summary>
        /// Deletes specified transaction from DB.
        /// </summary>
        /// <returns></returns>
        public bool DeleteTransaction(int id)
        {
            string sql = "DELETE FROM Transactions WHERE rowid=@id";
            int accountId = GetAccountIdForTransaction(id);
            var param = new SQLiteParameter()
            {
                ParameterName = "@id",
                DbType = System.Data.DbType.Int32,
                Value = id
            };
            if (ExecuteNonQuery(sql, param))
            {
                UpdateTotal(accountId);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddTransaction(int accountId, DateTime date, decimal amount, string info, int categoryId, out int id)
        {
            string sql = "INSERT INTO Transactions VALUES(@date, @amount, @info, @accId, @catId)";
            var parameters = new[]
            {
                new SQLiteParameter()
                    {
                        ParameterName = "@date",
                        DbType = System.Data.DbType.Date,
                        Value = date.Date
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@amount",
                        DbType = System.Data.DbType.Int32,
                        Value = FromDecimaltoDBInt(amount)
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@info",
                        DbType = System.Data.DbType.String,
                        Value = info ?? string.Empty
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@accId",
                        DbType = System.Data.DbType.Int32,
                        Value = accountId
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@catId",
                        DbType = System.Data.DbType.Int32,
                        Value = categoryId
                    }
            };
            if (ExecuteNonQueryInsert(out id, sql, parameters))
            {
                UpdateTotal(accountId);
                return true;
            }
            else
            {
                id = -1;
                return false;
            }
        }
        public bool UpdateTransaction(int id, DateTime date, decimal amount, string info, int categoryId)
        {
            string sql = "UPDATE Transactions SET date=@date, amount=@amount, info=@info, categoryId=@catId WHERE rowid=@rowid";
            var parameters = new[]
            {
                new SQLiteParameter()
                    {
                        ParameterName = "@date",
                        DbType = System.Data.DbType.Date,
                        Value = date.Date
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@amount",
                        DbType = System.Data.DbType.Int32,
                        Value = FromDecimaltoDBInt(amount)
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@info",
                        DbType = System.Data.DbType.String,
                        Value = info ?? string.Empty
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@catId",
                        DbType = System.Data.DbType.Int32,
                        Value = categoryId
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@rowid",
                        DbType = System.Data.DbType.Int32,
                        Value = id
                    }
            };
            if (ExecuteNonQuery(sql, parameters))
            {
                int accountId = GetAccountIdForTransaction(id);
                UpdateTotal(accountId);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Returns total decimal value of all transactions for specified
        /// year, month and category.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public decimal SelectTransactionsCombined(int year, int month, int categoryId)
        {
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddSeconds(-1);
            DateTime lastDayofPrevMonth = firstDayOfMonth.AddSeconds(-1);

            // BETWEEN firstDay and lastDay is glitchy
            // have to use > and <=
            string sql = @"SELECT sum(t.amount) FROM Transactions as t
                           INNER JOIN Accounts as a
                           on t.accId = a.id
                           WHERE date>@startDate and date<=@endDate
                           AND categoryId=@catId
                           AND exbudget = 0";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@catId",
                    DbType = System.Data.DbType.Int32,
                    Value = categoryId
                });
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@startDate",
                    DbType = System.Data.DbType.Date,
                    Value = lastDayofPrevMonth
                });
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@endDate",
                    DbType = System.Data.DbType.Date,
                    Value = lastDayOfMonth
                });
                return FromDBValToDecimal(cmd.ExecuteScalar());
            }
        }
        /************** Records *****************/
        /// <summary>
        /// Selects budget records for a given year and month.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public IEnumerable<(decimal amount, int categoryId, string type, int onDay, int id)> SelectRecords(int year, int month)
        {
            string sql = "SELECT *, rowid FROM Budget WHERE month=@month AND year=@year";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@month",
                    DbType = System.Data.DbType.Int32,
                    Value = month
                });
                cmd.Parameters.Add(new SQLiteParameter()
                {
                    ParameterName = "@year",
                    DbType = System.Data.DbType.Int32,
                    Value = year
                });
                SQLiteDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    yield return (FromDBValToDecimal(dr.GetDecimal(0)), dr.GetInt32(1),
                        dr.GetString(2), dr.GetInt32(3), dr.GetInt32(6));
                }
                dr.Close();
            }
        }

        /// <summary>
        /// Adds new budget record to DB.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="categoryId"></param>
        /// <param name="type"></param>
        /// <param name="onDay"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool AddRecord(decimal amount, int categoryId, string type, int onDay, int year, int month, out int id)
        {
            string sql = "INSERT INTO Budget VALUES(@amount, @catId, @btype, @onDay, @year, @month)";
            var parameters = new[]
            {
                new SQLiteParameter()
                    {
                        ParameterName = "@amount",
                        DbType = System.Data.DbType.Int32,
                        Value = FromDecimaltoDBInt(amount)
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@catId",
                        DbType = System.Data.DbType.Int32,
                        Value = categoryId
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@btype",
                        DbType = System.Data.DbType.String,
                        Value = type
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@onDay",
                        DbType = System.Data.DbType.Int32,
                        Value = onDay
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@month",
                        DbType = System.Data.DbType.Int32,
                        Value = month
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@year",
                        DbType = System.Data.DbType.Int32,
                        Value = year
                    }
            };
            return ExecuteNonQueryInsert(out id, sql, parameters);
        }
        /// <summary>
        /// Deletes budget record from DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteRecord(int id)
        {
            string sql = "DELETE FROM Budget WHERE rowid=@rowid";
            var param = new SQLiteParameter()
            {
                ParameterName = "@rowid",
                DbType = System.Data.DbType.Int32,
                Value = id
            };
            return ExecuteNonQuery(sql, param);
        }
        /// <summary>
        /// Updates parameters of provided budget record in DB.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <param name="categoryId"></param>
        /// <param name="type"></param>
        /// <param name="onDay"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public bool UpdateRecord(int id, decimal amount, int categoryId, string type, int onDay, int year, int month)
        {
            string sql = "UPDATE Budget SET amount=@amount, categoryId=@catId, type=@btype, day=@onDay, year=@year, month=@month WHERE rowid=@rowid";
            var parameters = new[]
            {
                new SQLiteParameter()
                    {
                        ParameterName = "@amount",
                        DbType = System.Data.DbType.Int32,
                        Value = FromDecimaltoDBInt(amount)
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@catId",
                        DbType = System.Data.DbType.Int32,
                        Value = categoryId
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@btype",
                        DbType = System.Data.DbType.String,
                        Value = type
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@onDay",
                        DbType = System.Data.DbType.Int32,
                        Value = onDay
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@month",
                        DbType = System.Data.DbType.Int32,
                        Value = month
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@year",
                        DbType = System.Data.DbType.Int32,
                        Value = year
                    },
                new SQLiteParameter()
                    {
                        ParameterName = "@rowid",
                        DbType = System.Data.DbType.Int32,
                        Value = id
                    }
            };
            return ExecuteNonQuery(sql, parameters);
        }
        /// <summary>
        /// Calculates decimal value of all budget records for a given year, month and category.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public decimal SelectRecordsCombined(int year, int month, int categoryId)
        {
            string sql = "SELECT sum(amount) FROM Budget WHERE month=@month AND year=@year AND categoryId=@catId";
            var parameters = new[]
            {
                new SQLiteParameter()
                {
                    ParameterName = "@catId",
                    DbType = System.Data.DbType.Int32,
                    Value = categoryId
                },
                new SQLiteParameter()
                {
                    ParameterName = "@month",
                    DbType = System.Data.DbType.Int32,
                    Value = month
                },
                new SQLiteParameter()
                {
                    ParameterName = "@year",
                    DbType = System.Data.DbType.Int32,
                    Value = year
                }
            };
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                cmd.Parameters.AddRange(parameters);
                return FromDBValToDecimal(cmd.ExecuteScalar());
            }
        }
        /************** Misc *****************/
        /// <summary>
        /// Returns the last year of the available budget records.
        /// </summary>
        /// <returns></returns>
        public int? GetMaximumYear()
        {
            int? maxYear;
            string sql =
                @"SELECT MAX(MaxYear) FROM 
                    (SELECT MAX(year) as MaxYear FROM Budget
                     UNION ALL
                     SELECT MAX(strftime('%Y', date)) as MaxYear FROM Transactions
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                object toConvert = cmd.ExecuteScalar();
                if (Convert.IsDBNull(toConvert))
                {
                    maxYear = null;
                }
                else
                {
                    maxYear = Convert.ToInt32(toConvert);
                }
            }
            return maxYear;
        }
        /// <summary>
        /// Returns the earliest year of the available budget records
        /// or transactions.
        /// </summary>
        /// <returns></returns>
        public int? GetMinimumYear()
        {
            int? minYear;
            string sql =
                @"SELECT MIN(MinYear) FROM
                    (SELECT MIN(year) as MinYear FROM Budget
                     UNION ALL
                     SELECT MIN(strftime('%Y', date)) as MinYear FROM Transactions
                    )";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
            {
                object toConvert = cmd.ExecuteScalar();
                if (Convert.IsDBNull(toConvert))
                {
                    minYear = null;
                }
                else
                {
                    minYear = Convert.ToInt32(toConvert);
                }
            }
            return minYear;
        }
    }
}
