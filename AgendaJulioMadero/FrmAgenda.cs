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

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            // Validar que todos los campos requeridos estén llenos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtTelefono.Text) ||
                CmbCategoria.SelectedIndex == -1) // Verificar que se haya seleccionado una categoría
            {
                MessageBox.Show("Por favor, completa todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Salir del método si hay campos vacíos
            }

            // Obtener los valores de los campos
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            int telefono;
            string mail = TxtEmail.Text;
            int idCategoria = (int)CmbCategoria.SelectedValue; // Asegúrate de que el ComboBox tenga el valor correcto configurado

            // Validar que el teléfono sea un número entero
            if (!int.TryParse(TxtTelefono.Text, out telefono))
            {
                MessageBox.Show("Por favor, ingresa un número de teléfono válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Salir del método si el teléfono no es válido
            }

            // Crear una instancia de CcAgenda y llamar al método para agregar el contacto
            CcAgenda ccAgenda = new CcAgenda();
            ccAgenda.AgregarContacto(nombre, apellido, telefono, mail, idCategoria);
            ccAgenda.CargarTree(treeViewAgenda);
        }
    }
}
