using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaJulioMadero
{
    internal class CcAgenda
    {
        //Cadena de conexion 
        private string cadenaConexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../Db\Agenda.accdb";

        private string Tabla = "Contactos";


        //Funcion para cargar el Tree view 
        public void CargarTree(TreeView treeView)
        {
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    // Consulta para obtener los contactos junto con sus categorías
                    string query = "SELECT c.Nombre, c.Apellido, c.Mail, c.Telefono, c.IdCategoria, cat.Descripcion AS Categoria FROM Contactos c INNER JOIN Categoria cat ON c.IdCategoria = cat.Id";

                    using (OleDbCommand comando = new OleDbCommand(query, conexion))
                    {
                        using (OleDbDataReader reader = comando.ExecuteReader())
                        {
                            // Vaciar el TreeView y cargar datos
                            treeView.Nodes.Clear();
                            while (reader.Read())
                            {
                                string categoria = reader["Categoria"].ToString();
                                string nombre = reader["Nombre"].ToString();
                                string apellido = reader["Apellido"].ToString();
                                string correo = reader["Mail"].ToString();
                                string telefono = reader["Telefono"].ToString();

                                // Busca el nodo de la categoría
                                TreeNode categoriaNode = treeView.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == categoria);

                                if (categoriaNode == null)
                                {
                                    // Crea un nuevo nodo de categoría si no existe
                                    categoriaNode = new TreeNode(categoria);
                                    treeView.Nodes.Add(categoriaNode);
                                }

                                // Crea un nodo para el contacto
                                TreeNode contactoNode = new TreeNode($"{nombre} {apellido}");
                                categoriaNode.Nodes.Add(contactoNode);

                                // Maneja IdCategoria correctamente
                                if (reader["IdCategoria"] != DBNull.Value) // Verifica si el valor no es nulo
                                {
                                    int idCategoria = Convert.ToInt32(reader["IdCategoria"]); // Convierte directamente

                                    TreeNodeInfo contactoInfo = new TreeNodeInfo
                                    {
                                        Nombre = nombre,
                                        Apellido = apellido,
                                        Telefono = int.TryParse(telefono, out int tel) ? tel : 0, // Convierte a int
                                        Mail = correo,
                                        IdCategoria = idCategoria
                                    };

                                    // Establece el Tag del nodo de contacto
                                    contactoNode.Tag = contactoInfo;

                                    // Crea un nodo llamado detalles
                                    TreeNode infoNode = new TreeNode("Detalles");
                                    contactoNode.Nodes.Add(infoNode);

                                    // Agregar texto descriptivo al nodo de detalles
                                    infoNode.Nodes.Add($"Teléfono: {telefono}");
                                    infoNode.Nodes.Add($"Correo: {correo}");
                                }
                                else
                                {
                                    MessageBox.Show("IdCategoria es nulo para uno de los contactos.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }
    }
}
