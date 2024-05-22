using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.SqlServer;

namespace Modelo
{
    public class ConexionDB
    {
        public string ObtenerConexion()
        {
            string strConexion = ConfigurationSettings.GetConfig.GetString("cadena");
            return strConexion;
        }
    }
}
