using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaJulioMadero
{
    public partial class FrmAgenda : Form
    {
        public FrmAgenda()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Carga el tree view 
            CcAgenda ccAgenda = new CcAgenda();
            ccAgenda.CargarTree(treeViewAgenda);

            //Cargo los dos combo box de categorias
            CcCategoria ccCategoria = new CcCategoria();
            ccCategoria.Llenarcmb(CmbCategoria);
            ccCategoria.Llenarcmb(CmbCategoriaBusquedad);
        }
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Verifica si el nodo seleccionado es un contacto
            if (e.Node.Tag is TreeNodeInfo contactoInfo)
            {
                // Cargar los datos en los TextBox
                txtNombre.Text = contactoInfo.Nombre;
                txtApellido.Text = contactoInfo.Apellido;
                TxtTelefono.Text = contactoInfo.Telefono.ToString(); // Convertir a string
                TxtEmail.Text = contactoInfo.Mail;

                // Cargar el ID de categoría en el ComboBox
                CmbCategoria.SelectedValue = contactoInfo.IdCategoria; // Ajustado para usar el ID
            }
        }
    }
}
