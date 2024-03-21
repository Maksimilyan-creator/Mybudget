using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using My_budget.Models;

namespace My_budget;

public partial class AddOutlay : Window
{
    private db db = new db();
    private Users user;
    private List<CategoryOutlay> _categoryOutlays;
    private TransactionOutlay newTransactionOutlay = new TransactionOutlay();
    public AddOutlay(Users user)
    {
        InitializeComponent();
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.user = user;
        db.GetAllCategoriesOutlay();
        CategoryComboBox.ItemsSource = db.CategoryOutlayList;

        
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        int userss = user.users_id;
        int category = ((CategoryComboBox.SelectedItem) as CategoryOutlay).CategoryOutlayId;
        string description = DescriptionTextBox.Text.ToString();
        newTransactionOutlay.Date = DateDatePicker.SelectedDate.HasValue ? DateDatePicker.SelectedDate.Value.DateTime:DateTime.Now;
        newTransactionOutlay.Amount = Convert.ToInt32(AmountTextBox.Text);
        db.InsertTransactionsOutlay(newTransactionOutlay, category, userss, description);
    }

    private void Close_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}