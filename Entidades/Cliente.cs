using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Cliente
    {
        public int idCliente { get; set; }
        public String nombreCompleto { get; set; }
        public String telefono { get; set; }
        public String correo { get; set; }
        public String contrasenna { get; set; }
        public Boolean activo { get; set; }
        public Boolean confirmado { get; set; }
        public int cedula { get; set; }
        public Boolean tipoClase { get; set; }
    }
}
