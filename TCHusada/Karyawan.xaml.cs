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
   /// <summary>
   /// Interaction logic for Karyawan.xaml
   /// </summary>
   public partial class Karyawan : Window
   {
      OracleConnection cn = new OracleConnection("Data Source=(DESCRIPTION="
             + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521)))"
             + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));"
             + "User Id=basdat; Password=basdat;");
      OracleDataAdapter dr = new OracleDataAdapter();
      OracleCommandBuilder cmd = new OracleCommandBuilder();

      DispatcherTimer timer;
      bool boleh = true;
      int pilih = 0;
      
      public Karyawan()
      {
         InitializeComponent();

         timer = new DispatcherTimer();
         timer.Interval = TimeSpan.FromSeconds(1.0);
         timer.Start();
         timer.Tick += new EventHandler(delegate(object s, EventArgs a)
         {
            label6.Content = "" + DateTime.Now.Hour.ToString("d2") + ":"
            + DateTime.Now.Minute.ToString("d2");
         });

         dataview(load_data("karyawan"), dataGridK);
         status_boxk(false);
         tombol_ek(true);
      }

      protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
      {
         base.OnMouseLeftButtonDown(e);
         try
         {
            DragMove();
         }
         catch
         { }
      }

      public int ExecuteNonQuery(string sql)
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

      private bool Execute(string sql)
      {
         int affected;
         cn.Open();
         if(cn != null)
         {
            affected = ExecuteNonQuery(sql);
            cn.Close();
            if (affected < 1) return false;
            return true;
         }
         return false;
      }

      private DataSet load_data(string tbl)//fungsi modif global
      {
         DataSet ds = new DataSet();
         dr = new OracleDataAdapter("select * from " + tbl, cn);
         cmd = new OracleCommandBuilder(dr);
         dr.Fill(ds);
         return ds;
      }

      private DataSet update_data(string tbl)//fungsi modif global
      {
         DataSet ds = new DataSet();
         dr = new OracleDataAdapter("select * from " + tbl, cn);
         cmd = new OracleCommandBuilder(dr);
         dr.Fill(ds);
         return ds;
      }

      private void dataview(DataSet sd, DataGrid dg)//fungsi modif global
      {
         dg.ItemsSource = sd.Tables[0].DefaultView;
         dg.ColumnWidth = DataGridLength.Auto;
      }

      /// <summary>
      /// batas bawah fungsi global
      /// </summary>

      private void status_boxk(bool status)//fungsi kopas global
      {
         textnipk.IsEnabled = status;
         textnamk.IsEnabled = status;
         textalmtk.IsEnabled = status;
         texttelpk.IsEnabled = status;
         datemskk.IsEnabled = status;
      }

      private void tombol_ek(bool status)//fungsi kopas global
      {
         tambahbtnk.IsEnabled = status;
         ubahbtnk.IsEnabled = status;
         hapusbtnk.IsEnabled = status;
         simpanbtnk.IsEnabled = !(status);
         bersihbtnk.IsEnabled = !(status);
         batalbtnk.IsEnabled = !(status);
      }

      private void clear_boxk()//fungsi kopas global
      {
         textnipk.Text = "";
         textnamk.Text = "";
         textalmtk.Text = "";
         texttelpk.Text = "";
         datemskk.Text = "";
      }

      private void image1_MouseDown(object sender, MouseButtonEventArgs e)
      {
         Application.Current.Shutdown();
      }

      private void tblexit_MouseDown(object sender, MouseButtonEventArgs e)
      {
         Application.Current.Shutdown();
      }

      private void tblogout_MouseDown(object sender, MouseButtonEventArgs e)
      {
         MainWindow m = new MainWindow();
         m.Show();
         this.Close();
      }

      private void simpanbtnk_Click(object sender, RoutedEventArgs e)
      {
         int ind = -1;
         bool error= false;
         if (pilih == 1)
         {
            error = false;
            string sql = "insert into KARYAWAN values ("
                          + "'" + textnipk.Text + "',"
                          + "'" + textnamk.Text + "',"
                          + "'" + textalmtk.Text + "',"
                          + "'" + texttelpk.Text + "',"
                          + "'" + datemskk.Text + "')";
            if (Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridK.Items.Count;
            }
            else error = true;
            
         }

         else if (pilih == 2)
         {
            error = false;
            string sql = "update KARYAWAN set "
                         + " NAMA_KARYAWAN = '" + textnamk.Text + "',"
                         + " ALAMAT_KARYAWAN= '" + textalmtk.Text + "',"
                         + " NO_TELP_KARYAWAN = '" + texttelpk.Text + "',"
                         + " TGL_MASUK_KARYAWAN = '" + datemskk.Text + "'"
                         + " where NIP_KARYAWAN = '" + textnipk.Text + "'";
            if (Execute(sql))
            {
               MessageBox.Show("Data telah diupdate");
               ind = dataGridK.SelectedIndex;
            }
            else error = true;
         }
         if (!error)
         {
            status_boxk(false);
            tombol_ek(true);
            boleh = true;
            pilih = 0;
            dataview(update_data("karyawan"), dataGridK);
            dataGridK.SelectedIndex = ind;
         }
      }

      private void bersihbtnk_Click(object sender, RoutedEventArgs e)
      {
         clear_boxk();
         boleh = true;
      }

      private void batalbtnk_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridK.SelectedIndex;
         dataview(update_data("karyawan"), dataGridK);
         status_boxk(false);
         tombol_ek(true);
         boleh = true;
         pilih = 0;
         dataGridK.SelectedIndex = ind;
      }

      private void tambahbtnk_Click(object sender, RoutedEventArgs e)
      {
         boleh = false;
         dataGridK.UnselectAll();
         clear_boxk();
         status_boxk(true);
         tombol_ek(false);
         pilih = 1;
      }

      private void ubahbtnk_Click(object sender, RoutedEventArgs e)
      {
         if (textnipk.Text != "")
         {
            boleh = false;
            status_boxk(true);
            tombol_ek(false);
            pilih = 2;
            textnipk.IsEnabled = false;
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void hapusbtnk_Click(object sender, RoutedEventArgs e)
      {
         if (textnipk.Text != "")
         {
            if (MessageBox.Show("Hapus entry \"" + textnipk.Text + "\" dari tabel ?", "Delete " + textnipk.Text, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
               string sql = "delete from petugasadm where NIP_KARYAWAN = " + textnipk.Text;
               if (Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               dataview(update_data("karyawan"), dataGridK);
               dataGridK.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtn_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridK.SelectedIndex;
         dataview(update_data("karyawan"), dataGridK);
         dataGridK.SelectedIndex = ind;
      }

      private void dataGridK_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if(!boleh)
            dataGridK.UnselectAll();
      }

      /// <summary>
      /// batas bawah tab karyawan
      /// </summary>
   }
}
