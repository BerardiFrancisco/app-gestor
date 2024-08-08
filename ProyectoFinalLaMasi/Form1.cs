using Dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoFinalLaMasi
{
    public partial class frmGestor : Form
    { 
        private List<Articulo> listaArticulo;
        public frmGestor()
        {
            InitializeComponent();
        }

        private void frmGestor_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Precio");
            cboMarca.Items.Add("Apple");
            cboMarca.Items.Add("Sony");
            cboMarca.Items.Add("Samsung");
            cboMarca.Items.Add("Motorola");
            cboMarca.Items.Add("Huawei");
            cboCategoria.Items.Add("Celulares");
            cboCategoria.Items.Add("Televisores");
            cboCategoria.Items.Add("Audio");
            cboCategoria.Items.Add("Media");
        }


        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            cargarImagen(seleccionado.UrlImagen);
           
        }
        
        public void cargarImagen(string imagen)
        {
            try
            {
                pcbImagen.Load(imagen);

            }
            catch (Exception)
            {
                pcbImagen.Load("https://i1.wp.com/gelatologia.com/wp-content/uploads/2020/07/placeholder.png?ssl=1");
            }
        }

        private void ocultarColumnas()
        {
            dgvArticulos.Columns["UrlImagen"].Visible = false;
            //dgvArticulos.Columns["Precio"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaArticulo alta = new frmAltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            frmAltaArticulo modificar = new frmAltaArticulo(seleccionado);
            modificar.ShowDialog();
        }

        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;
                ocultarColumnas();
                cargarImagen(listaArticulo[0].UrlImagen);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Deveritas?","Eliminando",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();

            if (opcion == "Nombre")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
            else if (opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if (cboCampo.SelectedItem == null || cboCriterio.SelectedItem == null || string.IsNullOrEmpty(txtFiltro.Text))
                {
                    cargar();
                    return;
                }

                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltro.Text;

                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {

            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                frmDetalle detalleForm = new frmDetalle(seleccionado);
                detalleForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un artículo para ver los detalles.");
            }
        }

        

        private void cboMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboMarca.Text;
            if (!string.IsNullOrEmpty(opcion))
            {
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                List<Articulo> listaFiltrada = articuloNegocio.filtrarPorMarca(opcion);
                dgvArticulos.DataSource = listaFiltrada;
            }
        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCategoria.Text;
            if (!string.IsNullOrEmpty(opcion))
            {
                ArticuloNegocio articuloNegocio = new ArticuloNegocio();
                List<Articulo> listaFiltrada = articuloNegocio.filtrarPorCategoria(opcion);
                dgvArticulos.DataSource = listaFiltrada;
            }
        }
    }
}
