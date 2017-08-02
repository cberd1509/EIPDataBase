using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using System.Data;

namespace BDMaestriaEIP
{
    class SQLHandler
    {
        
        SqlCeConnection con;

        public SQLHandler()
        {
            string baseDir = System.Environment.CurrentDirectory;

            string conString = @"Data Source={0}\Data\database.sdf";

            
            conString = string.Format(conString, baseDir);

            con = new SqlCeConnection(conString);
        }

         ~SQLHandler()
        {
            con.Close();
        }

        public bool NonQuery(string q)
        {
            con.Open();
            SqlCeCommand query = new SqlCeCommand(q, con);

            try
            {
                query.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch(SqlCeException ex)
            {
                MessageBox.Show("Hubo un error haciendo la inserción/eliminación", "Error en la inserción/eliminación. CODE "+ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Clipboard.SetText(q);
                con.Close();
                return false;
            }

        }

        public DataTable SelectQuery(string q)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCeDataAdapter da = new SqlCeDataAdapter(q, con);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                MessageBox.Show("Hubo un error haciendo la búsqueda","Error en la busqueda",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return dt;
            }
        }

    }
}
