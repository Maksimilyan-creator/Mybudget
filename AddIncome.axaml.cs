using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using My_budget.Models;
using MySqlConnector;

namespace My_budget;

public partial class AddIncome : Window
{
    private db db = new db();
    private Users user;
    private List<CategoryIncome> _categoryIncomes;
    private TransactionIncome newTransactionIncome = new TransactionIncome();
    public AddIncome(Users user)
    {
        InitializeComponent();
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.user = user;
        db.GetAllCategoriesIncome();
        CategoryComboBox.ItemsSource = db.CategoryIncomeList;
    }
    
    
    
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        int userss = user.users_id;
        int category = ((CategoryComboBox.SelectedItem) as CategoryIncome).CategoryIncomeId;
        newTransactionIncome.Date = DateDatePicker.SelectedDate.HasValue ? DateDatePicker.SelectedDate.Value.DateTime:DateTime.Now;
        newTransactionIncome.Amount = Convert.ToInt32(AmountTextBox.Text);
            db.InsertTransactionsIncome(newTransactionIncome, category, userss);
            
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}