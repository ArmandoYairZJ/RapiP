using RapiPirata.DB;

namespace RapiPirata.PantallaNegocios;

public partial class RegistrarProductos : ContentPage
{
    public string eUS;
    public string eConta;
    public RegistrarProductos( string eUs, string eConta)
	{
		InitializeComponent();
        this.eUS = eUs;
        this.eConta = eConta;
      

    }

    byte[] ImagenBytes;
    private async void btnImage_Clicked(object sender, EventArgs e)
    {

        try
        {

            var op = new MediaPickerOptions
            {
                Title = "Selecciona tu imagen"
            };

            var res = await MediaPicker.PickPhotoAsync(op);
            if (res != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var stream = await res.OpenReadAsync())
                    {
                        await stream.CopyToAsync(ms);
                    }
                    ImagenBytes = ms.ToArray();
                    imagenCargar.Source = ImageSource.FromStream(() => new MemoryStream(ImagenBytes));
                }
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error al cargar la imagen", $"Error al seleccionar su imagen:{ex.Message}", "Bueno");
        }
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string ProductoID = eProducoID.Text;
            string Descripcion = eDescripcion.Text;

            string Pventa = ePrecioVenta.Text;
            decimal Pv = decimal.Parse(Pventa);

            string Saldo = eSaldo.Text;
            float sal = float.Parse(Saldo);

            OpMySQL opMySQL = new OpMySQL();
            bool result = opMySQL.RegistrarProductos(eUS, eConta, ProductoID, Descripcion, Pv, sal, ImagenBytes);

            if (ImagenBytes != null)
            {

                if (result)
                {
                    await DisplayAlert("Éxito", "Producto registrado correctamente", "OK");
                    clear();
                 
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar el producto", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Por favor, seleccione una imagen", "OK");
            }


        }

        catch (Exception ex)
        {
            await DisplayAlert("ERROR", $"OCURRIO UN ERROR {ex.Message}", "OK");

        }
    }

    private void clear()
    {
        eProducoID.Text = string.Empty;
        eDescripcion.Text = string.Empty;
        ePrecioVenta.Text = string.Empty;
        eSaldo.Text = string.Empty;
        imagenCargar.Source = null; 
    }
}