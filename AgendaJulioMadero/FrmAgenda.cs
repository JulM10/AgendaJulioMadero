using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
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
                TxtTelefono.Text = contactoInfo.Telefono.ToString();
                TxtEmail.Text = contactoInfo.Mail;

                // Cargar el ID de categoría en el ComboBox
                CmbCategoria.SelectedValue = contactoInfo.IdCategoria;
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            // Validar que todos los campos requeridos estén llenos
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtTelefono.Text) ||
                CmbCategoria.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, completa todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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
                MessageBox.Show("Por favor, ingresa un número de teléfono válido. Recurda solo usar numeros", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            // llama al método para agregar el contacto
            CcAgenda ccAgenda = new CcAgenda();
            ccAgenda.AgregarContacto(nombre, apellido, telefono, mail, idCategoria);
            ccAgenda.CargarTree(treeViewAgenda);
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            CcAgenda ccAgenda = new CcAgenda();

            // Verifica si hay un nodo seleccionado y si contiene la información del contacto
            if (treeViewAgenda.SelectedNode?.Tag is TreeNodeInfo contactoInfo)
            {
                int id = contactoInfo.Id;

                // Confirmar la eliminación
                var confirmResult = MessageBox.Show("¿Está seguro de que desea eliminar este contacto?",
                                                     "Confirmar eliminación",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        ccAgenda.EliminarContacto(id);
                        ccAgenda.CargarTree(treeViewAgenda);
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier error que ocurra durante la eliminación
                        MessageBox.Show($"Error al eliminar el contacto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un contacto para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            CcAgenda ccAgenda = new CcAgenda();

            // Verifica si hay un nodo seleccionado y si contiene la información del contacto
            if (treeViewAgenda.SelectedNode?.Tag is TreeNodeInfo contactoInfo)
            {

                // Validar que todos los campos requeridos estén llenos
                if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                    string.IsNullOrWhiteSpace(txtApellido.Text) ||
                    string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                    string.IsNullOrWhiteSpace(TxtTelefono.Text) ||
                    CmbCategoria.SelectedIndex == -1)
                {
                    MessageBox.Show("Por favor, completa todos los campos requeridos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                int id = contactoInfo.Id; // Asegúrate de que esta propiedad exista en TreeNodeInfo
                var confirmResult = MessageBox.Show("¿Está seguro de que desea editar este contacto?",
                                                    "Confirmar Edición",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Warning);
                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        // Obtener los valores de los campos
                        string nombre = txtNombre.Text;
                        string apellido = txtApellido.Text;
                        int telefono;
                        string mail = TxtEmail.Text;
                        int idCategoria = (int)CmbCategoria.SelectedValue;

                        // Validar que el teléfono sea un número entero
                        if (!int.TryParse(TxtTelefono.Text, out telefono))
                        {
                            MessageBox.Show("Por favor, ingresa un número de teléfono válido. Recuerda solo usar números.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Llama al método para editar el contacto
                        ccAgenda.EditarContacto(id, nombre, apellido, mail, telefono, idCategoria);

                        // Cargar el TreeView nuevamente
                        ccAgenda.CargarTree(treeViewAgenda);
                    }
                    catch (Exception ex)
                    {
                        // Manejar cualquier error que ocurra durante la edición
                        MessageBox.Show($"Error al editar el contacto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un contacto para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {

        }
    }
}
