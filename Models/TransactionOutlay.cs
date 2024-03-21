using System;

namespace My_budget.Models;

public class TransactionOutlay
{
    public int TransactionId { get; set; }
    public string UserId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public int Amount { get; set; }
    public string CategoryOutlayId { get; set; }
}