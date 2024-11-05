using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaJulioMadero
{
    internal class CcCategoria
    {
        private string cadenaConexion = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=../../Db\Agenda.accdb";

        private string Tabla = "Categoria";


        public void Llenarcmb(ComboBox cmb)
        {
            try
            {
                using (OleDbConnection conexion = new OleDbConnection(cadenaConexion))
                {
                    conexion.Open();
                    string query = "SELECT Id, Descripcion FROM Categoria"; // Asegúrate de que traes el Id
                    using (OleDbCommand comando = new OleDbCommand(query, conexion))
                    {
                        using (OleDbDataReader reader = comando.ExecuteReader())
                        {
                            cmb.Items.Clear();
                            // Crea un DataTable para usar como DataSource
                            DataTable dt = new DataTable();
                            dt.Load(reader);
                            cmb.DataSource = dt;
                            cmb.DisplayMember = "Descripcion"; // El nombre que se mostrará en el ComboBox
                            cmb.ValueMember = "Id"; // El valor que se almacenará
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar Categorías: {ex.Message}");
            }
        }
    }
}
