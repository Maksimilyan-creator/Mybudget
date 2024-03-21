using System;

namespace My_budget.Models;

public class Notification
{
    public int NotificationId { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; }
    public DateTime DateTime { get; set; }
}