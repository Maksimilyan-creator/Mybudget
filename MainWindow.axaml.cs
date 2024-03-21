using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using My_budget.Models;

namespace My_budget;

public partial class MainWindow : Window
{
    private AuthService authService;
    private string CaptchaText;
    private db db = new db();
    private bool captchaRequired = false;
    public MainWindow()
    {
        InitializeComponent();
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        db.GetAllUsers();
        this.authService = new AuthService(db.UsersList);
        GenerateAndDisplayCaptcha();
    }

    private void GenerateAndDisplayCaptcha()
    {
        // Генерация капчи
        CaptchaText = CaptchaGenerator.GenerateCaptcha();
        // отображение капчи с зачеркиванием
        captchaTextBlock.Text = CaptchaText;
    }

    private void LoginButton_OnClick(object? sender, RoutedEventArgs e)
    {
        string email = LoginTextBox.Text;
        string password = passwordBox.Text;
        string enteredCaptcha = captchaTextBox.Text;
        
        //Аутентификация пользователя
        Users authenticatedUsers = authService.Authenticate(email, password);

        if (authenticatedUsers != null)
        {
            if (captchaRequired && enteredCaptcha != CaptchaText)
            {
                errorTextBlock.Text = "";
                errorTextBlock.Text = "Неправильно введена капча";
                GenerateAndDisplayCaptcha();
                return;
            }

            captchaRequired = false;
            captchaTextBlock.Text = "";
            
            ShowMainWindow(authenticatedUsers);
            Close();
        }
        else
        {
            {
                if (!captchaRequired)
                {
                    captchaTextBlock.IsVisible = true;
                    captchaTextBox.IsVisible = true;
                    ImageRestart.IsVisible = true;
                    captchaRequired = true;
                }

                errorTextBlock.Text = "Неверное имя пользователя или пароль";
            }
        }
    }

    private void ImageRestart_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        GenerateAndDisplayCaptcha();
    }

    private void ShowMainWindow(Users user)
    {
        var Home = new WindowHome(user);
        Home.Show();
    }
}