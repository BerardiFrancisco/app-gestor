using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using negocio;

namespace ProyectoFinalLaMasi
{
    public partial class frmAltaArticulo : Form
    {

        
        private Articulo Articulo = null;
        public frmAltaArticulo()
        {
            InitializeComponent();
        }

        public frmAltaArticulo(Articulo articulo)
        {
            InitializeComponent();
            this.Articulo = articulo;
            Text = "Modificar Articulo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Articulo Articulo = new Articulo();
            ArticuloNegocio negocio = new ArticuloNegocio(); 
            try
            {
                Articulo.Codigo = txtCodigo.Text;
                Articulo.Nombre = txtNombre.Text; 
                Articulo.Descripcion = txtDescripcion.Text;
                Articulo.UrlImagen = txtImagen.Text;
                Articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                Articulo.Marca = (Marca)cboMarca.SelectedItem;

                if (decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    Articulo.Precio = precio;
                }
                negocio.agregar(Articulo);
                MessageBox.Show("Agregado exitosamente");
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
       
        private void frmAltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";    
            MarcaNegocio marcanegocio = new MarcaNegocio();
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";


            try
            {
                cboCategoria.DataSource = categoriaNegocio.listar();
                cboMarca.DataSource = marcanegocio.listar();

                if (Articulo != null)
                {
                    txtCodigo.Text = Articulo.Codigo.ToString();
                    txtNombre.Text = Articulo.Nombre;
                    txtDescripcion.Text = Articulo.Descripcion;
                    txtImagen.Text = Articulo.UrlImagen;
                    txtPrecio.Text = Articulo.Precio.ToString("N2");
                    cargarImagen(Articulo.UrlImagen);
                    cboCategoria.SelectedValue = Articulo.Categoria.Id;
                    cboMarca.SelectedValue = Articulo.Marca.Id;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        public void cargarImagen(string imagen)
        {
            try
            {
                pctAltaArticulo.Load(imagen);

            }
            catch (Exception)
            {
                pctAltaArticulo.Load("https://i1.wp.com/gelatologia.com/wp-content/uploads/2020/07/placeholder.png?ssl=1");
            }
        }


        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == '.')
            {
                if (e.KeyChar == '.' && txtPrecio.Text.Contains("."))
                {
                    e.Handled = true; 
                }
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
