using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using My_budget.Models;

namespace My_budget;

public partial class UpdateIncome : Window
{
    private TransactionIncome _transactionIncome;
    private db db = new db();
    private List<CategoryIncome> _categoryIncomes;
    private TransactionIncome newTransactionIncome = new TransactionIncome();
    
    public UpdateIncome(TransactionIncome transactionIncome)
    {
        InitializeComponent();
        _transactionIncome = transactionIncome;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        db.GetAllCategoriesIncome();
        CategoryComboBox.ItemsSource = db.CategoryIncomeList;
        CategoryComboBox.SelectedItem = db.CategoryIncomeList.FirstOrDefault(u => u.CategoryIncomeName == _transactionIncome.CategoryIncomeId);
        DateDatePicker.SelectedDate = _transactionIncome.Date;
        AmountTextBox.SelectedText = _transactionIncome.Amount.ToString();
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        int category = ((CategoryComboBox.SelectedItem) as CategoryIncome).CategoryIncomeId;
        newTransactionIncome.Date = DateDatePicker.SelectedDate.HasValue ? DateDatePicker.SelectedDate.Value.DateTime:DateTime.Now;
        newTransactionIncome.Amount = Convert.ToInt32(AmountTextBox.Text);
        newTransactionIncome.TransactionId = _transactionIncome.TransactionId;
        db.UpdateTransactionsIncome(category, newTransactionIncome);
    }
}