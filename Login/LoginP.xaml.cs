using RapiPirata.DB;
using RapiPirata.Panta;
using RapiPirata.PantallasClientes;

namespace RapiPirata.Login;

public partial class LoginP : ContentPage
{
	public LoginP()
	{
		InitializeComponent();
	}

    private async void btnEntrar_Clicked(object sender, EventArgs e)
    {


        string uss = eUS.Text;
        string conn = eConta.Text;

        OpMySQL opMySQL = new OpMySQL();



        if(opMySQL.InSesion(uss, conn))
        {
            if (opMySQL.VeriUsuario(uss, conn))
            {
                await Navigation.PushModalAsync(new PantallaInicio());
            }

            else if (opMySQL.VeriCliente(uss,conn))
            {
                await Navigation.PushModalAsync(new Inicio(uss, conn));

            }
        }

        else
        {
            await DisplayAlert("Aviso", "Contraseña o usuario incorrecta digite una contraseña valida", "ok");
        }


    }
}