using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using My_budget.Models;

namespace My_budget;

public partial class UpdateOutlay : Window
{
    private TransactionOutlay _transactionOutlay;
    private db db = new db();
    private List<CategoryOutlay> _categoryOutlays;
    private TransactionOutlay newTransactionOutlay = new TransactionOutlay();
    
    public UpdateOutlay(TransactionOutlay transactionOutlay)
    {
        InitializeComponent();
        _transactionOutlay = transactionOutlay;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        db.GetAllCategoriesOutlay();
        CategoryComboBox1.ItemsSource = db.CategoryOutlayList;
        CategoryComboBox1.SelectedItem = db.CategoryOutlayList.FirstOrDefault(u => u.CategoryOutlayName == _transactionOutlay.CategoryOutlayId);
        DateDatePicker.SelectedDate = _transactionOutlay.Date;
        AmountTextBox.SelectedText = _transactionOutlay.Amount.ToString();
        DescriptionTextBox.SelectedText = _transactionOutlay.Description;
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        int category = ((CategoryComboBox1.SelectedItem) as CategoryOutlay).CategoryOutlayId;
        newTransactionOutlay.Date = DateDatePicker.SelectedDate.HasValue ? DateDatePicker.SelectedDate.Value.DateTime:DateTime.Now;
        string description = DescriptionTextBox.Text;
        newTransactionOutlay.Amount = Convert.ToInt32(AmountTextBox.Text);
        newTransactionOutlay.TransactionId = _transactionOutlay.TransactionId;
        db.UpdateTransactionsOutlay(category, description, newTransactionOutlay);
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}