using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaJulioMadero
{
    public class TreeNodeInfo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Telefono { get; set; } // Mantener como int
        public string Mail { get; set; }
        public int IdCategoria { get; set; } // Cambiar a int para el ID de categoría
    }

}
