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
      DataSet ds = new DataSet();

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

         dataview(load_data());
         status_box(false);
         tombol_e(true);
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

      private DataSet load_data()
      {
         DataSet sd = new DataSet();
         dr = new OracleDataAdapter("select * from KARYAWAN", cn);
         cmd = new OracleCommandBuilder(dr);
         dr.Fill(sd);
         return sd;
      }

      private DataSet update_data()
      {
         DataSet sd = new DataSet();
         dr = new OracleDataAdapter("select * from KARYAWAN", cn);
         cmd = new OracleCommandBuilder(dr);
         dr.Fill(sd);
         return sd;
      }

      private void dataview(DataSet sd)
      {
         dataGrid1.ItemsSource = sd.Tables[0].DefaultView;
         dataGrid1.ColumnWidth = DataGridLength.Auto;
      }

      private void status_box(bool status)
      {
         textBox1.IsEnabled = status;
         textBox2.IsEnabled = status;
         textBox3.IsEnabled = status;
         textBox4.IsEnabled = status;
         datePicker1.IsEnabled = status;
      }

      private void tombol_e(bool status)
      {
         tambahbtn.IsEnabled = status;
         ubahbtn.IsEnabled = status;
         hapusbtn.IsEnabled = status;
         simpanbtn.IsEnabled = !(status);
         bersihbtn.IsEnabled = !(status);
         batalbtn.IsEnabled = !(status);
      }

      private void clear_box()
      {
         textBox1.Text = "";
         textBox2.Text = "";
         textBox3.Text = "";
         textBox4.Text = "";
         datePicker1.Text = "";
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

      private void simpanbtn_Click(object sender, RoutedEventArgs e)
      {
         int ind = -1;
         bool error= false;
         if (pilih == 1)
         {
            error = false;
            string sql = "insert into KARYAWAN values ("
                          + "'" + textBox1.Text + "',"
                          + "'" + textBox2.Text + "',"
                          + "'" + textBox3.Text + "',"
                          + "'" + textBox4.Text + "',"
                          +"'" + datePicker1.Text + "')";
            if (Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGrid1.Items.Count;
            }
            else error = true;
            
         }

         else if (pilih == 2)
         {
            error = false;
            string sql = "update KARYAWAN set "
                         + " NAMA_KARYAWAN = '" + textBox2.Text + "',"
                         + " ALAMAT_KARYAWAN= '" + textBox3.Text + "',"
                         + " NO_TELP_KARYAWAN = '" + textBox4.Text + "',"
                         + " TGL_MASUK_KARYAWAN = '" + datePicker1.Text + "'"
                         + " where NIP_KARYAWAN = '" + textBox1.Text + "'";
            if (Execute(sql))
            {
               MessageBox.Show("Data telah diupdate");
               ind = dataGrid1.SelectedIndex;
            }
            else error = true;
         }
         if (!error)
         {
            status_box(false);
            tombol_e(true);
            boleh = true;
            pilih = 0;
            dataview(update_data());
            dataGrid1.SelectedIndex = ind;
         }
      }

      private void bersihbtn_Click(object sender, RoutedEventArgs e)
      {
         clear_box();
         boleh = true;
      }

      private void batalbtn_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGrid1.SelectedIndex;
         dataview(update_data());
         status_box(false);
         tombol_e(true);
         boleh = true;
         pilih = 0;
         dataGrid1.SelectedIndex = ind;
      }

      private void tambahbtn_Click(object sender, RoutedEventArgs e)
      {
         boleh = false;
         dataGrid1.UnselectAll();
         clear_box();
         status_box(true);
         tombol_e(false);
         pilih = 1;
      }

      private void ubahbtn_Click(object sender, RoutedEventArgs e)
      {
         if (textBox1.Text != "")
         {
            boleh = false;
            status_box(true);
            tombol_e(false);
            pilih = 2;
            textBox1.IsEnabled = false;
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void hapusbtn_Click(object sender, RoutedEventArgs e)
      {
         if (textBox1.Text != "")
         {
            if (MessageBox.Show("Hapus entry \"" + textBox1.Text + "\" dari tabel ?", "Delete " + textBox1.Text, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
               string sql = "delete from petugasadm where NIP_KARYAWAN = " + textBox1.Text;
               if (Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               dataview(update_data());
               dataGrid1.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtn_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGrid1.SelectedIndex;
         dataview(update_data());
         dataGrid1.SelectedIndex = ind;
      }

      private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if(!boleh)
            dataGrid1.UnselectAll();
      }

   }
}
