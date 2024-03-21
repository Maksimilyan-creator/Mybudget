using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

using My_budget.Models;

namespace My_budget;
public partial class WindowHome : Window
{
    private Users user;
    private db db = new db();
    
    
    
    public WindowHome(Users user)
    {
        InitializeComponent();
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        this.user = user;
        UserNameProfile();
        db.GetTransactionsIncome(user.users_id);
        ListTransactionsIncome.ItemsSource = db.TransactionIncomeList;
        db.GetTransactionsOutlay(user.users_id);
        ListTransactionsOutlay.ItemsSource = db.TransactionOutlayList;
        Timer timer = new Timer();
        timer.Interval = 1000;
        timer.Elapsed += TimerElapsed;
        timer.Start();
    }
    
    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        
        
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            UpdateBalance();
            db.GetTransactionsIncome(user.users_id);
            db.GetTransactionsOutlay(user.users_id);
            ListTransactionsIncome.ItemsSource = db.TransactionIncomeList;
            ListTransactionsOutlay.ItemsSource = db.TransactionOutlayList;
            
        });
    }

    


    public void UserNameProfile()
    {
        string userName = user.username;
        TextBlockName.Text = userName;
    }
    
    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private double CalculateBalance(IEnumerable<TransactionIncome> incomes, IEnumerable<TransactionOutlay> outlays)
    {
        double totalIncome = incomes.Sum(t => t.Amount);
        double totalOutlay = outlays.Sum(t => t.Amount);
        return totalIncome - totalOutlay;
    }

    private bool isNegativeBalanceShown = false;
    public void UpdateBalance()
    {
        double balance = CalculateBalance(ListTransactionsIncome.ItemsSource as IEnumerable<TransactionIncome>,
            ListTransactionsOutlay.ItemsSource as IEnumerable<TransactionOutlay>);
        BalanceTextBlock.Text = $"{balance} ₽";

        if (balance < 0 && !isNegativeBalanceShown)
        {
            BalanceTextBlock.Foreground = Brushes.Red;
            MessageBoxManager.GetMessageBoxStandard("Упс", "Золото закончилось милорд").ShowAsync();
            isNegativeBalanceShown = true;
        }
        else if (balance >= 0)
        {
            isNegativeBalanceShown = false;
            BalanceTextBlock.Foreground = Brushes.Green;
        }
    }
    private void AddIncome_OnClick(object? sender, RoutedEventArgs e)
    {
        ShowAddIncome(user);
    }

    private void AddOutlay_OnClick(object? sender, RoutedEventArgs e)
    {
        ShowAddOutlay(user);
    }

    public void ShowAddIncome(Users user1)
    {
        var AddIncome = new AddIncome(user1);
        AddIncome.Show();
    }

    public void ShowAddOutlay(Users user1)
    {
        var AddOutlay = new AddOutlay(user1);
        AddOutlay.Show();
    }

    private void Navigator_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ListBoxItem selectedNavItem = (ListBoxItem)Navigator.SelectedItem;

        switch (selectedNavItem.Content.ToString())
        {
            case "Доходы":
                Income.IsVisible = true;
                Outlay.IsVisible = false;
                Diagram.IsVisible = false;
                break;
            case "Расходы":
                Income.IsVisible = false;
                Outlay.IsVisible = true;
                Diagram.IsVisible = false;
                break;
            case "Диаграмма":
                Income.IsVisible = false;
                Outlay.IsVisible = false;
                Diagram.IsVisible = true;
                break;
        }
    }
    
    private void Exit_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
    private async void DropButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Button btm = (Button)sender;
        var deleteIncome = btm.DataContext as TransactionIncome;
        int incomeId = deleteIncome.TransactionId;
        var box = await MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Вы уверены?", ButtonEnum.YesNo).ShowAsync();
        if (box == ButtonResult.Yes)
        {
            db.DeleteTransactionsIncome(incomeId);
        }
    }

    private void ListTransactionsIncome_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var editIncome = (TransactionIncome)ListTransactionsIncome.SelectedItem;
        UpdateIncome updateIncome = new UpdateIncome(editIncome);
        updateIncome.Show();
    }


    private async void DropButton2_OnClick(object? sender, RoutedEventArgs e)
    {
        Button btm = (Button)sender;
        var deleteOutlay = btm.DataContext as TransactionOutlay;
        int outlayId = deleteOutlay.TransactionId;
        var box = await MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Вы уверены?", ButtonEnum.YesNo).ShowAsync();
        if (box == ButtonResult.Yes)
        {
            db.DeleteTransactionsOutlay(outlayId);
        }
    }

    private void ListTransactionsOutlay_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var editOutlay = (TransactionOutlay)ListTransactionsOutlay.SelectedItem;
        UpdateOutlay updateOutlay = new UpdateOutlay(editOutlay);
        updateOutlay.Show();
    }
}