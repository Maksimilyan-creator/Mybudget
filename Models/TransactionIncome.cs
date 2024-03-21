using System;

namespace My_budget.Models;

public class TransactionIncome
{
    public int TransactionId { get; set; }
    public string UserId { get; set; }
    public DateTime Date { get; set; }
    public int Amount { get; set; }
    public string CategoryIncomeId { get; set; }
}