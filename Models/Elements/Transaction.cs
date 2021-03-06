﻿using Models.Interfaces;
using System;

namespace Models.Elements
{
    internal class Transaction : ITransaction
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Info { get; set; }
        public ICategory Category { get; set; }
        public IAccount Account { get; set; }
        public int Id { get; internal set; }
    }
}
