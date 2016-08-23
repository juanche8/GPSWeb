using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GPS.Business;
using GPS.Data;

namespace BuscarDirecciones
{
    class Program
    {
        static void Main(string[] args)
        {
            Timer timer = new Timer(new TimerCallback(TimeCallBack), null, 1000, 12000);
            Console.Read();
            timer.Dispose();
        }

        public static void TimeCallBack(object o)
        {
            Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " - CKEQUEANDO DIRECCIONES ");
            try
            {
                List<vMonitoreo> registros = clsVehiculo.searchEmptyDirection();

                foreach(vMonitoreo registro in registros)
                {
                    String _error = "";
                    GPSDataOSM.Direccione _direccion = clsGoogle.getdireccion(registro.LATITUD, registro.LONGITUD, ref _error);

                    if (_error == "")
                    {
                        Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm;ss") + "-Actualizado direccion.");

                        clsVehiculo.UpdateDirection(_direccion.dir_nombre_via, _direccion.dir_localidad, _direccion.dir_provinica, registro.Codigo.ToString(), _direccion.dir_tipo_via.ToString());
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm;ss") + "-Error Consultando direccion:" + _error);
                         
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR EN EL PROCESO: " + ex.Message + " " + ex.StackTrace);
            }

        }
    }
}
