using RapiPirata.DB;
using System.Data;

namespace RapiPirata.PantallaNegocios;

public partial class ConsultarProductos : ContentPage
{
	public string eUs;
	public string eConta;
	public ConsultarProductos(string eUs, string eConta)
	{
		InitializeComponent();
		this.eUs = eUs;
		this.eConta = eConta;

		MostrarProducto(eUs, eConta);

        //Actuliza en automatico la pestaña
        this.InvalidateMeasure();
	}

	public async void MostrarProducto(string eUs, string eConta)
	{
        try
        {
            OpMySQL opMySQL = new OpMySQL();
            DataTable productos = opMySQL.MostrarProductos(eUs, eConta);

            if (productos != null && productos.Rows.Count > 0)
            {
                ScrollView scrollView = new ScrollView();
                StackLayout stackLayout = new StackLayout();
                stackLayout.Padding = new Thickness(10);
                stackLayout.Spacing = 10;
                stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;

                foreach (DataRow row in productos.Rows)
                {
                    byte[] imagenBytes = (byte[])row["Imagen"];
                    Image imgProducto = new Image();
                    imgProducto.Source = ImageSource.FromStream(() => new MemoryStream(imagenBytes));
                    imgProducto.Aspect = Aspect.AspectFit;
                    imgProducto.HeightRequest = 100; 
                    imgProducto.WidthRequest = 100;  

                    Label lblProductoID = new Label();
                    lblProductoID.Text = $"ProductoID: {row["ProductoID"]}";
                    Label lblDescripcion = new Label();
                    lblDescripcion.Text = $"Descripción: {row["Descripcion"]}";
                    Label lblPrecioVenta = new Label();
                    lblPrecioVenta.Text = $"Precio Venta: {row["PVenta"]}";
                    Label lblSaldo = new Label();
                    lblSaldo.Text = $"Saldo: {row["Saldo"]}";

                    Frame frameProducto = new Frame();
                    frameProducto.CornerRadius = 10;
                    frameProducto.Padding = 10;
                    frameProducto.BackgroundColor = Color.FromHex("#000000");

                    StackLayout frameContent = new StackLayout();
                    frameContent.Orientation = StackOrientation.Horizontal;
                    frameContent.Children.Add(imgProducto);
                    frameContent.Children.Add(new StackLayout { Children = { lblProductoID, lblDescripcion, lblPrecioVenta, lblSaldo } });
                    frameContent.BackgroundColor = Color.FromHex("#000000");

                    frameProducto.Content = frameContent;
                    stackLayout.Children.Add(frameProducto);
                }

                scrollView.Content = stackLayout;
                Content = scrollView;
                
            }
            else
            {
                await DisplayAlert("Advertencia", "No hay productos disponibles", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", $"OCURRIÓ UN ERROR: {ex.Message}", "OK");
        }
    }
}