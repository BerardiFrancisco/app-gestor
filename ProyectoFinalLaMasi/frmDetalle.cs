using negocio;
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
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace ProyectoFinalLaMasi
{
    public partial class frmDetalle : Form
    {
        private Articulo articulo;
        public frmDetalle()
        {
            InitializeComponent();
        }

        public frmDetalle(Articulo seleccionado)
        {
            InitializeComponent();
            this.articulo = seleccionado;
        }

        private List<Articulo> listaarticulo;
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmDetalle_Load(object sender, EventArgs e)
        {

            if (articulo != null)
            {

                dgvDetalle.DataSource = new[] { articulo };
                if (!string.IsNullOrEmpty(articulo.UrlImagen))
                {
                    try
                    {
                        pctDetalle.Load(articulo.UrlImagen);
                    }
                    catch (Exception)
                    {
                        pctDetalle.Load("https://i1.wp.com/gelatologia.com/wp-content/uploads/2020/07/placeholder.png?ssl=1");
                    }
                }
                else
                {
                    pctDetalle.Load("https://i1.wp.com/gelatologia.com/wp-content/uploads/2020/07/placeholder.png?ssl=1");
                }
            }
            else
            {
                MessageBox.Show("No se encontró el artículo.");
                Close();
            }
        }

    }

       
}
