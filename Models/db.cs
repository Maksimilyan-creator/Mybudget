using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reactive;
using MsBox.Avalonia;
using MySqlConnector;


namespace My_budget.Models;


public class db
{

    private MySqlConnectionStringBuilder _connectionSb { get; set; }
    public List<Users> UsersList { get; set; } = new List<Users>();
    public List<TransactionIncome> TransactionIncomeList { get; set; } = new List<TransactionIncome>();
    public List<TransactionOutlay> TransactionOutlayList { get; set; } = new List<TransactionOutlay>();
    public List<CategoryIncome> CategoryIncomeList { get; set; } = new List<CategoryIncome>();
    public List<CategoryOutlay> CategoryOutlayList { get; set; } = new List<CategoryOutlay>();
    public List<Notification> NotificationList { get; set; } = new List<Notification>();

    public db()
    {
        _connectionSb = new MySqlConnectionStringBuilder
        {
            Server = "localhost",
            Database = "mybudget",
            UserID = "root",
            Password = "123456"
        };
        GetAllUsers();
    }

    public void GetAllUsers()
    {
        UsersList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM users";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UsersList.Add(new Users()
                        {
                            users_id = reader.GetInt32("users_id"),
                            username = reader.GetString("username"),
                            password = reader.GetString("password"),
                            email = reader.GetString("email")
                        });
                    }
                }

            }

            connection.Close();
        }
    }

    public void GetTransactionsIncome(int Iduser)
    {
        TransactionIncomeList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM transactionsincome JOIN users ON transactionsincome.users_id = users.users_id " +
                    "JOIN categoriesincome ON transactionsincome.categoryIncome_id = categoriesincome.categoryIncome_id " +
                    "WHERE transactionsincome.users_id = @Iduser";
                command.Parameters.AddWithValue("@Iduser", Iduser);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TransactionIncomeList.Add(new TransactionIncome()
                        {
                            TransactionId = reader.GetInt32("transaction_id"),
                            UserId = reader.GetString("username"),
                            Date = reader.GetDateTime("date"),
                            Amount = reader.GetInt32("amount"),
                            CategoryIncomeId = reader.GetString("categoryIncome_name")
                        });
                    }
                }

            }

            connection.Close();
        }
    }

    public void GetAllCategoriesIncome()
    {
        CategoryIncomeList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM categoriesincome";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryIncomeList.Add(new CategoryIncome()
                        {
                            CategoryIncomeId = reader.GetInt32("categoryIncome_id"),
                            CategoryIncomeName = reader.GetString("categoryIncome_name")
                        });
                    }
                }

            }

            connection.Close();
        }
    }

    public void GetTransactionsOutlay(int Iduser)
    {
        TransactionOutlayList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM transactionsoutlay JOIN users ON transactionsoutlay.users_id = users.users_id " +
                    "JOIN categoriesoutlay ON transactionsoutlay.categoryOutlay_id = categoriesoutlay.categoryOutlay_id " +
                    "WHERE transactionsoutlay.users_id = @Iduser";
                command.Parameters.AddWithValue("@Iduser", Iduser);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TransactionOutlayList.Add(new TransactionOutlay()
                        {
                            TransactionId = reader.GetInt32("transaction_id"),
                            UserId = reader.GetString("username"),
                            Date = reader.GetDateTime("date"),
                            Description = reader.GetString("description"),
                            Amount = reader.GetInt32("amount"),
                            CategoryOutlayId = reader.GetString("categoryOutlay_name")
                        });
                    }
                }

            }

            connection.Close();
        }
    }

    public void GetAllCategoriesOutlay()
    {
        CategoryOutlayList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM categoriesoutlay";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CategoryOutlayList.Add(new CategoryOutlay()
                        {
                            CategoryOutlayId = reader.GetInt32("categoryOutlay_id"),
                            CategoryOutlayName = reader.GetString("categoryOutlay_name")
                        });
                    }
                }

            }

            connection.Close();
        }
    }

    public void GetNotifications()
    {
        NotificationList.Clear();
        using (var connection = new MySqlConnection(_connectionSb.ConnectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM notifications JOIN users ON notifications.UserId = users.id WHERE users_id = <IdUser>";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NotificationList.Add(new Notification()
                        {
                            NotificationId = reader.GetInt32("notification_id"),
                            UserId = reader.GetString("username"),
                            Message = reader.GetString("message"),
                            DateTime = reader.GetDateTime("datetime")
                        });
                    }
                }

            }

            connection.Close();
        }
    }

    public void InsertTransactionsIncome(TransactionIncome transactionIncome, int category, int userss)
    {
        try
        {
            using (var conn = new MySqlConnection(_connectionSb.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into transactionsincome (users_id, date, amount, categoryIncome_id) " +
                                      "values (@users_id, @date, @amount, @categoryIncome_id)";
                    cmd.Parameters.AddWithValue("@users_id", userss);
                    cmd.Parameters.AddWithValue("@date", transactionIncome.Date);
                    cmd.Parameters.AddWithValue("@amount", transactionIncome.Amount);
                    cmd.Parameters.AddWithValue("@categoryIncome_id", category);
                    var reader = cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            MessageBoxManager.GetMessageBoxStandard(default, "Запись успешно добавлена в базу данных").ShowAsync();

        }
        catch (MySqlException ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка MySQL:", $"{ex.Message}").ShowAsync();
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Произошла ошибка", $"{ex.Message}").ShowAsync();
        }
        finally
        {
        }

    }

    public void UpdateTransactionsIncome(int category, TransactionIncome newTransactionIncome)
    {
        try
        {
            using (var conn = new MySqlConnection(_connectionSb.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE transactionsincome " +
                                      "SET date = @date, amount = @amount," +
                                      " categoryIncome_id = @categoryIncome_id WHERE transaction_id = @transaction_id";
                    cmd.Parameters.AddWithValue("@transaction_id", newTransactionIncome.TransactionId);
                    cmd.Parameters.AddWithValue("@categoryIncome_id", category);
                    cmd.Parameters.AddWithValue("@amount", newTransactionIncome.Amount);
                    cmd.Parameters.AddWithValue("@date", newTransactionIncome.Date);
                    var reader = cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            MessageBoxManager.GetMessageBoxStandard(default, "Запись успешно изменена в базе данных").ShowAsync();

        }
        catch (MySqlException ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка MySQL:", $"{ex.Message}").ShowAsync();
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Произошла ошибка", $"{ex.Message}").ShowAsync();
        }
    }

    public void DeleteTransactionsIncome(int id)
    {
        try
        {
            using (var conn = new MySqlConnection(_connectionSb.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Delete FROM transactionsincome WHERE transaction_id = @transaction_id";
                    cmd.Parameters.AddWithValue("@transaction_id", id);
                    var reader = cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            MessageBoxManager.GetMessageBoxStandard(default, "Запись успешно удвлена из базы данных").ShowAsync();
        }
        catch (MySqlException ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка MySQL:", $"{ex.Message}").ShowAsync();
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Произошла ошибка", $"{ex.Message}").ShowAsync();
        }

    }

    public void DeleteTransactionsOutlay(int id)
    {
        try
        {
            using (var conn = new MySqlConnection(_connectionSb.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Delete FROM transactionsoutlay WHERE transaction_id = @transaction_id";
                    cmd.Parameters.AddWithValue("@transaction_id", id);
                    var reader = cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            MessageBoxManager.GetMessageBoxStandard(default, "Запись успешно удвлена из базы данных").ShowAsync();
        }
        catch (MySqlException ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка MySQL:", $"{ex.Message}").ShowAsync();
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Произошла ошибка", $"{ex.Message}").ShowAsync();
        }
    }

    public void InsertTransactionsOutlay(TransactionOutlay transactionOutlay, int category, int userss,
        string description)
    {
        try
        {
            using (var conn = new MySqlConnection(_connectionSb.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "insert into transactionsoutlay (users_id, date, description, amount, categoryOutlay_id ) " +
                        "values (@users_id, @date, @description, @amount, @categoryOutlay_id)";
                    cmd.Parameters.AddWithValue("@users_id", userss);
                    cmd.Parameters.AddWithValue("@date", transactionOutlay.Date);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@amount", transactionOutlay.Amount);
                    cmd.Parameters.AddWithValue("@categoryOutlay_id", category);
                    var reader = cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            MessageBoxManager.GetMessageBoxStandard(default, "Запись успешно добавлена в базу данных").ShowAsync();

        }
        catch (MySqlException ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка MySQL:", $"{ex.Message}").ShowAsync();
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Произошла ошибка", $"{ex.Message}").ShowAsync();
        }
    }

    public void UpdateTransactionsOutlay(int category, string description,  TransactionOutlay newTransactionOutlay)
    {
        try
        {
            using (var conn = new MySqlConnection(_connectionSb.ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "UPDATE transactionsoutlay " +
                                      "SET date = @date, amount = @amount, description = @description, " +
                                      "categoryOutlay_id = @categoryOutlay_id WHERE transaction_id = @transaction_id";

                    cmd.Parameters.AddWithValue("@transaction_id", newTransactionOutlay.TransactionId);
                    cmd.Parameters.AddWithValue("@categoryOutlay_id", category);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@amount", newTransactionOutlay.Amount);
                    cmd.Parameters.AddWithValue("@date", newTransactionOutlay.Date);
                    var reader = cmd.ExecuteNonQuery();
                }

                conn.Close();
            }

            MessageBoxManager.GetMessageBoxStandard(default, "Запись успешно изменена в базе данных").ShowAsync();

        }
        catch (MySqlException ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Ошибка MySQL:", $"{ex.Message}").ShowAsync();
        }
        catch (Exception ex)
        {
            MessageBoxManager.GetMessageBoxStandard("Произошла ошибка", $"{ex.Message}").ShowAsync();
        }
    }

}