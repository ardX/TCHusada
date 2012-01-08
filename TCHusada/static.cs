using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace TCHusada
{
   class siapa
   {
      private static string isi = "Someone";

      public static string anda
      {
         get { return isi; }
         set { isi = value; }
      }
   }

   class jam
   {
      public static ContentControl berapa()
      {
         ContentControl lalala = new ContentControl();
         DispatcherTimer timer;
         timer = new DispatcherTimer();
         timer.Interval = TimeSpan.FromSeconds(1.0);
         timer.Start();
         timer.Tick += new EventHandler(delegate(object s, EventArgs a)
         {
            lalala.Content = "" + DateTime.Now.Hour.ToString("d2") + ":"
            + DateTime.Now.Minute.ToString("d2");
         });
         return lalala;
      }
   }

   public class KelaminList : List<string>
   {
      public KelaminList()
      {
         this.Add("L");
         this.Add("P");
      }
   }

   public static class F
   {
      public static OracleConnection cn = new OracleConnection("Data Source=(DESCRIPTION="
             + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521)))"
             + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));"
             + "User Id=basdat; Password=basdat;");

      public static void addPass(string user, string pass)
      {
         string sql = "insert into pengguna values ("
                          + "'" + user + "',"
                          + "'" + HashCode(pass) + "')";
         if (Execute(sql))
            MessageBox.Show("Data telah ditambahkan");
      }

      public static void updPass(string user, string pass)
      {
         string sql = "update pengguna set pass = '" + HashCode(pass) + "' where username = '" + user +"'";
         if (Execute(sql))
            MessageBox.Show("Password telah diganti");
      }

      public static bool checkPass(string user, string pass)
      {
         cn.Open();
         string sql = "select pass from pengguna where username = '" + user + "'";
         OracleDataReader reader;
         OracleCommand cmd = new OracleCommand(sql, cn);
         reader = cmd.ExecuteReader();
         string cek = "";
         if(reader.Read())
            cek = reader.GetString(0);
         reader.Close();
         cn.Close();
         if (HashCode(pass) == cek)
            return true;
         return false;
      }

      private static string HashCode(string str)
      {
         string rethash = "";
         try
         {
            System.Security.Cryptography.SHA1 hash = System.Security.Cryptography.SHA1.Create();
            System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
            byte[] combined = encoder.GetBytes(str);
            hash.ComputeHash(combined);
            rethash = Convert.ToBase64String(hash.Hash);
         }
         catch (Exception ex)
         {
            string strerr = "Error in HashCode : " + ex.Message;
         }
         return rethash;
      }

      private static int ExecuteNonQuery(string sql)
      {
         try
         {
            int affected;
            OracleTransaction mytransaction = cn.BeginTransaction();
            OracleCommand cmd = cn.CreateCommand();
            cmd.CommandText = sql;
            affected = cmd.ExecuteNonQuery();
            mytransaction.Commit();
            return affected;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         return -1;
      }

      public static bool Execute(string sql)
      {
         int affected;
         cn.Open();
         if (cn != null)
         {
            affected = ExecuteNonQuery(sql);
            cn.Close();
            if (affected < 1) return false;
            return true;
         }
         return false;
      }

      public static DataSet load_data(string tbl)
      {
         OracleDataAdapter dr = new OracleDataAdapter();
         OracleCommandBuilder cmd = new OracleCommandBuilder();
         DataSet ds = new DataSet();
         dr = new OracleDataAdapter("select * from " + tbl, cn);
         cmd = new OracleCommandBuilder(dr);
         dr.Fill(ds);
         return ds;
      }

      public static DataSet load_data_cust(string tbl, string col)
      {
         OracleDataAdapter dr = new OracleDataAdapter();
         OracleCommandBuilder cmd = new OracleCommandBuilder();
         DataSet ds = new DataSet();
         dr = new OracleDataAdapter("select " + col + " from " + tbl, cn);
         cmd = new OracleCommandBuilder(dr);
         dr.Fill(ds);
         return ds;
      }

      public static void dataview(DataSet sd, DataGrid dg)
      {
         dg.ItemsSource = sd.Tables[0].DefaultView;
         dg.ColumnWidth = DataGridLength.Auto;
      }

      public static string getmaxnomor(string var)
      {
         string metu = "0";
         cn.Open();
         string sql = "select val from maxval where var = '" + var+"'";
         OracleDataReader reader;
         OracleCommand cmd = new OracleCommand(sql, cn);
         reader = cmd.ExecuteReader();
         if (reader.Read())
         {
            if (!reader.IsDBNull(0))
               metu = reader.GetString(0);
         }
         ulong metuu = UInt64.Parse(metu) + 1;
         metu = metuu + "";
         reader.Close();
         cn.Close();
         if (metu == "1")
            return "AUTONUMBER";
         return metu;
      }

      public static void setmaxnomor(string var, string val)
      {
         string sql = "update maxval set VAL = " + val + ".CURRVAL where VAR = '" + var + "'";
         Execute(sql);
      }

      public static string getsumresep(string idresep)
      {
         string metu = "0";
         decimal metuu = 0;
         cn.Open();
         string sql = "select sum(o.harga_obat) from detail_resep d, obat o where d.nomor_obat = o.nomor_obat and id_resep = '" + idresep + "'";
         OracleDataReader reader;
         OracleCommand cmd = new OracleCommand(sql, cn);
         reader = cmd.ExecuteReader();
         if (reader.Read())
         {
            if (!reader.IsDBNull(0))
            {
               metuu = reader.GetDecimal(0);
            }
         }
         metu = metuu + "";
         reader.Close();
         cn.Close();
         return metu;
      }

      public static void GantiPass(string user, string passL, string passB, string passB2)
      {
         if (!(passL == "" || passB == "" || passB2 == ""))
         {
            if (passB == passB2)
            {
               if (checkPass(user, passL))
               {
                  updPass(user, passB);
               }
               else MessageBox.Show("Password Lama tidak cocok", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else MessageBox.Show("Password Baru tidak sama", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
         }
         else if (passL == "")
            MessageBox.Show("Password lama kosong");
         else if (passB == "" && !(passL == ""))
            MessageBox.Show("Password Baru kosong");
         else if (passB2 == "" && !(passB == "") && !(passL == ""))
            MessageBox.Show("konfirmasi password kosong");
         else MessageBox.Show("password kosong");
      }

      public static void MasukPass(string user, string passL, string passB, string passB2)
      {
         if (!(passB == "" || passB2 == ""))
         {
            if (passB == passB2)
            {
               addPass(user, passB);
            }
            else MessageBox.Show("Password Baru tidak sama", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
         }
         else if (passB == "" )
            MessageBox.Show("Password Baru kosong");
         else if (passB2 == "" && !(passB == ""))
            MessageBox.Show("konfirmasi password kosong");
         else MessageBox.Show("password kosong");
      }

   }
}
