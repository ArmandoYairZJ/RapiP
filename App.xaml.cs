using RapiPirata.Login;

namespace RapiPirata
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginP();
        }
    }
}
