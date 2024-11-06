using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
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
                    string query = "SELECT c.Id, c.Nombre, c.Apellido, c.Mail, c.Telefono, c.IdCategoria, cat.Descripcion AS Categoria " +
                                    "FROM Contactos c INNER JOIN Categoria cat ON c.IdCategoria = cat.Id";

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
            string query = "UPDATE Contactos SET Nombre = ?, Apellido = ?, Mail = ?, Telefono = ?, IdCategoria = ? WHERE Id = ?";

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comandoEditar = new OleDbCommand(query, conexion))
                    {
                        comandoEditar.Parameters.AddWithValue("?", Nombre);
                        comandoEditar.Parameters.AddWithValue("?", Apellido);
                        comandoEditar.Parameters.AddWithValue("?", Mail);
                        comandoEditar.Parameters.AddWithValue("?", Telefono);
                        comandoEditar.Parameters.AddWithValue("?", IdCategoria);
                        comandoEditar.Parameters.AddWithValue("?", Id);

                        // Ejecutar el comando
                        int resultado = comandoEditar.ExecuteNonQuery();

                        // Si se actualizó el registro con éxito
                        if (resultado > 0)
                        {
                            MessageBox.Show("Contacto actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el contacto para actualizar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar usuario: " + Nombre + " " + Apellido + " " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void BuscarPorNombre(string Nombre,TreeView treeView)
        {
            string query = "SELECT c.Id, c.Nombre, c.Apellido, c.Mail, c.Telefono, c.IdCategoria, cat.Descripcion AS Categoria " +
                            "FROM Contactos c INNER JOIN Categoria cat ON c.IdCategoria = cat.Id WHERE c.Nombre = ? ";

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comandoBuscar = new OleDbCommand(query, conexion))
                    {
                        comandoBuscar.Parameters.AddWithValue("?", Nombre);
                        using (OleDbDataReader reader = comandoBuscar.ExecuteReader())
                        {
                            // Limpiar el TreeView antes de cargar los resultados de la búsqueda
                            treeView.Nodes.Clear();

                            bool hayResultados = false;

                            while (reader.Read())
                            {
                                hayResultados = true;

                                string categoria = reader["Categoria"].ToString();
                                string apellido = reader["Apellido"].ToString();
                                string mail = reader["Mail"].ToString();
                                int telefono = Convert.ToInt32(reader["Telefono"]);
                                int idCategoria = Convert.ToInt32(reader["IdCategoria"]);
                                int id = Convert.ToInt32(reader["Id"]);

                                // Crear o buscar el nodo de categoría
                                TreeNode categoriaNode = treeView.Nodes
                                    .Cast<TreeNode>()
                                    .FirstOrDefault(n => n.Text == categoria);

                                if (categoriaNode == null)
                                {
                                    categoriaNode = new TreeNode(categoria);
                                    treeView.Nodes.Add(categoriaNode);
                                }

                                // Crear el objeto de información del contacto
                                TreeNodeInfo contactoInfo = new TreeNodeInfo
                                {
                                    Nombre = Nombre,
                                    Apellido = apellido,
                                    Telefono = telefono,
                                    Mail = mail,
                                    IdCategoria = idCategoria
                                };

                                // Crear el nodo del contacto y establecer el Tag
                                TreeNode contactoNode = new TreeNode($"{Nombre} {apellido}");
                                contactoNode.Tag = contactoInfo;

                                // Añadir los detalles del contacto como subnodos
                                contactoNode.Nodes.Add($"Teléfono: {telefono}");
                                contactoNode.Nodes.Add($"Correo: {mail}");

                                // Agregar el nodo de contacto al TreeView
                                treeView.Nodes.Add(contactoNode);
                            }

                            if (!hayResultados)
                            {
                                MessageBox.Show("No se encontró el contacto buscado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Contacto(s) encontrado(s) con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar usuario: " + Nombre + " " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void BuscarPorMail(string mail, TreeView treeView)
        {
            string query = "SELECT c.Id, c.Nombre, c.Apellido, c.Mail, c.Telefono, c.IdCategoria, cat.Descripcion AS Categoria FROM Contactos c INNER JOIN Categoria cat ON c.IdCategoria = cat.Id WHERE c.Mail = ?";

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comandoBuscar = new OleDbCommand(query, conexion))
                    {
                        comandoBuscar.Parameters.AddWithValue("?", mail);

                        using (OleDbDataReader reader = comandoBuscar.ExecuteReader())
                        {
                            // Limpiar el TreeView antes de cargar los resultados de la búsqueda
                            treeView.Nodes.Clear();

                            bool hayResultados = false;

                            while (reader.Read())
                            {
                                hayResultados = true;

                                string categoria = reader["Categoria"].ToString();
                                string nombre = reader["Nombre"].ToString();
                                string apellido = reader["Apellido"].ToString();
                                int telefono = Convert.ToInt32(reader["Telefono"]);
                                int idCategoria = Convert.ToInt32(reader["IdCategoria"]);
                                int id = Convert.ToInt32(reader["Id"]);

                                // Crear o buscar el nodo de categoría
                                TreeNode categoriaNode = treeView.Nodes
                                    .Cast<TreeNode>()
                                    .FirstOrDefault(n => n.Text == categoria);

                                if (categoriaNode == null)
                                {
                                    categoriaNode = new TreeNode(categoria);
                                    treeView.Nodes.Add(categoriaNode);
                                }

                                // Crear el objeto de información del contacto
                                TreeNodeInfo contactoInfo = new TreeNodeInfo
                                {
                                    Nombre = nombre,
                                    Apellido = apellido,
                                    Telefono = telefono,
                                    Mail = mail,
                                    IdCategoria = idCategoria
                                };

                                // Crear el nodo del contacto y establecer el Tag
                                TreeNode contactoNode = new TreeNode($"{nombre} {apellido}");
                                contactoNode.Tag = contactoInfo;

                                // Añadir los detalles del contacto como subnodos
                                contactoNode.Nodes.Add($"Teléfono: {telefono}");
                                contactoNode.Nodes.Add($"Correo: {mail}");

                                // Agregar el nodo de contacto dentro del nodo de categoría
                                categoriaNode.Nodes.Add(contactoNode);
                            }

                            if (!hayResultados)
                            {
                                MessageBox.Show("No se encontró el contacto con el mail especificado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Contacto(s) encontrado(s) con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar por mail: " + mail + " " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void BuscarPorCategoria(int idCategoria, TreeView treeView)
        {
            string query = "SELECT c.Id, c.Nombre, c.Apellido, c.Mail, c.Telefono, c.IdCategoria, cat.Descripcion AS Categoria FROM Contactos c INNER JOIN Categoria cat ON c.IdCategoria = cat.Id WHERE c.IdCategoria = ?";

            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (OleDbCommand comandoBuscar = new OleDbCommand(query, conexion))
                    {
                        comandoBuscar.Parameters.AddWithValue("?", idCategoria);

                        using (OleDbDataReader reader = comandoBuscar.ExecuteReader())
                        {
                            // Limpiar el TreeView antes de cargar los resultados de la búsqueda
                            treeView.Nodes.Clear();

                            bool hayResultados = false;

                            TreeNode categoriaNode = null;

                            while (reader.Read())
                            {
                                hayResultados = true;

                                string categoria = reader["Categoria"].ToString();
                                string nombre = reader["Nombre"].ToString();
                                string apellido = reader["Apellido"].ToString();
                                string mail = reader["Mail"].ToString();
                                int telefono = Convert.ToInt32(reader["Telefono"]);
                                int id = Convert.ToInt32(reader["Id"]);

                                // Crear o buscar el nodo de categoría
                                if (categoriaNode == null)
                                {
                                    categoriaNode = new TreeNode(categoria);
                                    treeView.Nodes.Add(categoriaNode);
                                }

                                // Crear el objeto de información del contacto
                                TreeNodeInfo contactoInfo = new TreeNodeInfo
                                {
                                    Nombre = nombre,
                                    Apellido = apellido,
                                    Telefono = telefono,
                                    Mail = mail,
                                    IdCategoria = idCategoria
                                };

                                // Crear el nodo del contacto y establecer el Tag
                                TreeNode contactoNode = new TreeNode($"{nombre} {apellido}");
                                contactoNode.Tag = contactoInfo;

                                // Añadir los detalles del contacto como subnodos
                                contactoNode.Nodes.Add($"Teléfono: {telefono}");
                                contactoNode.Nodes.Add($"Correo: {mail}");

                                // Agregar el nodo de contacto dentro del nodo de categoría
                                categoriaNode.Nodes.Add(contactoNode);
                            }

                            if (!hayResultados)
                            {
                                MessageBox.Show("No se encontraron contactos en la categoría seleccionada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Contacto(s) encontrado(s) en la categoría con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar por categoría: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
