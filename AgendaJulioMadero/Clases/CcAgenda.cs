using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
                    string query = "SELECT c.Id, c.Nombre, c.Apellido, c.Mail, c.Telefono, c.IdCategoria, cat.Descripcion AS Categoria FROM Contactos c INNER JOIN Categoria cat ON c.IdCategoria = cat.Id";

                    using (OleDbCommand comando = new OleDbCommand(query, conexion))
                    {
                        using (OleDbDataReader reader = comando.ExecuteReader())
                        {
                            // Vaciar el TreeView y cargar datos
                            treeView.Nodes.Clear();
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["Id"]);
                                string categoria = reader["Categoria"].ToString();
                                string nombre = reader["Nombre"].ToString();
                                string apellido = reader["Apellido"].ToString();
                                string correo = reader["Mail"].ToString();
                                int telefono = int.TryParse(reader["Telefono"].ToString(), out int tel) ? tel : 0;
                                int idCategoria = Convert.ToInt32(reader["IdCategoria"]);

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

                                    TreeNodeInfo contactoInfo = new TreeNodeInfo
                                    {
                                        Id = id,
                                        Nombre = nombre,
                                        Apellido = apellido,
                                        Telefono = telefono,
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}");
            }
        }
        public void AgregarContacto(string Nombre, string Apellido, int Telefono, string Mail, int Categoria)
        {
            // String query para agregar contactos
            string query = "INSERT INTO Contactos ([Nombre], [Apellido], [Telefono], [Mail], [IdCategoria]) VALUES (?, ?, ?, ?, ?)";
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comandoInsertar = new OleDbCommand(query, conexion))
                    {
                        comandoInsertar.Parameters.AddWithValue("?", Nombre);
                        comandoInsertar.Parameters.AddWithValue("?", Apellido);
                        comandoInsertar.Parameters.AddWithValue("?", Telefono);
                        comandoInsertar.Parameters.AddWithValue("?", Mail);
                        comandoInsertar.Parameters.AddWithValue("?", Categoria);

                        // Ejecuta el comando
                        int resultado = comandoInsertar.ExecuteNonQuery();


                        // Si el registro fue exitoso, mostrar un mensaje y retornar true
                        if (resultado > 0)
                        {
                            MessageBox.Show("Registro con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EliminarContacto(int Id)
        {
            // String query para agregar contactos
            string query = "DELETE FROM CONTACTOS WHERE id = ?";

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comandoEliminar = new OleDbCommand(query, conexion))
                    {
                        comandoEliminar.Parameters.AddWithValue("?", Id);

                        // Ejecuta el comando
                        int resultado = comandoEliminar.ExecuteNonQuery();

                        // Si el registro fue exitoso, mostrar un mensaje y retornar true
                        if (resultado > 0)
                        {
                            MessageBox.Show("Contacto eliminado con exito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Eliminar Usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void EditarContacto(int Id, string Nombre, string Apellido, string Mail, int Telefono, int IdCategoria)
        {
            // String query para agregar contactos
            string query = "UPDATE CONTACTOS SET Nombre = ?, Apellido = ?, Mail = ?, Telefono = ?, IdCategoria = ? WHERE Id =" + Id;

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comandoEditar = new OleDbCommand(query, conexion))
                    {
                        comandoEditar.Parameters.AddWithValue("?", Nombre);
                        comandoEditar.Parameters.AddWithValue("?", Apellido);
                        comandoEditar.Parameters.AddWithValue("?", Telefono);
                        comandoEditar.Parameters.AddWithValue("?", Mail);
                        comandoEditar.Parameters.AddWithValue("?", IdCategoria);

                        // Ejecuta el comando
                        int resultado = comandoEditar.ExecuteNonQuery();

                        // Si el registro fue exitoso, mostrar un mensaje y retornar true
                        if (resultado > 0)
                        {
                            MessageBox.Show("Contacto Actualizado con exito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Actualizar Usuario: " + Nombre + " " + Apellido  + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
