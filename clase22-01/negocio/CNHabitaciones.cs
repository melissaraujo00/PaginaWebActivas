using datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    
    public class CNHabitaciones
    {
        CDHabitaciones habitaciones = new CDHabitaciones();
        public DataTable ObtenerhabitacionesN()
        {
            return habitaciones.ObtenerHabitaciones();
        }

        public bool AgregarHabitaciones(int numero, string descripcion, int cant_huespedes)
        {
            return habitaciones.agregarHabitacion(numero, descripcion, cant_huespedes);
        }

        public bool ModificarHabitacion(int id, int numero, string descripcion, int cant_huespedes)
        {
            return habitaciones.ModificarHabitacion(id, numero, descripcion, cant_huespedes);
        }

        public bool EliminarHabitacion(int id)
        {
            return habitaciones.EliminarHabitacion(id);
        }

    }

    
}
