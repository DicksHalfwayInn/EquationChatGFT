using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;


namespace EquationChatGFT
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _solver = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _solver;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
//using Avalonia.Controls;

//namespace EquationChatGFT.Views
//{
//    public partial class MainWindow : Window
//    {
//        public MainWindow()
//        {
//            InitializeComponent();
//        }
//    }
//}
