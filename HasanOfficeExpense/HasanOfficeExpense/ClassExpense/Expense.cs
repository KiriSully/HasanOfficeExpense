using System;

namespace ClassExpense
{
    internal class Expense
    {
        public int Id { get; internal set; }
        public int Amount { get; internal set; }
        public object Category { get; internal set; }
        public DateTime Date { get; internal set; }
        public object Description { get; internal set; }
    }
}