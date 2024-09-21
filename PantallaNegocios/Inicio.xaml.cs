using RapiPirata.PantallaNegocios;

namespace RapiPirata.Panta;

public partial class Inicio : FlyoutPage
{

    public string eUS;
    public string eConta;
    public Inicio(string eUS, string eConta)
	{
		InitializeComponent();
        IsPresented = false;

        this.eUS = eUS;
        this.eConta = eConta;

    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        IsPresented = !IsPresented; 

    }

    private void Registro_Clicked(object sender, EventArgs e)
    {
        Detail = new RegistrarProductos(eUS, eConta);
    }

    private void Consultar_Clicked(object sender, EventArgs e)
    {
        Detail = new ConsultarProductos(eUS, eConta);
    }

    private void ConsultarPedidos_Clicked(object sender, EventArgs e)
    {
        Detail = new ConsultarPedidos();
    }

    private void Todas_Clicked(object sender, EventArgs e)
    {
        Detail =  new Todas();
    }

    private void Reportes_Clicked(object sender, EventArgs e)
    {
        Detail = new Reportes();
    }

    private void Ventas_Clicked(object sender, EventArgs e)
    {
        Detail = new Ventas();
    }

  

 
}