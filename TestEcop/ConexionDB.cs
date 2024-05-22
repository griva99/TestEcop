using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestEcop
{
    public class ConexionDB
    {
        public string ConexionString()
        {
            //Data Source=Nombre de la conexion;Initial Catalog= base de datos;Integrated Security=True
            string conexion = "Data Source=LAPTOP-OP2NKJ4L\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True";
            return conexion;
        }
    }
}