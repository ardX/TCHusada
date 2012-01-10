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
   /// Interaction logic for Perawat.xaml
   /// </summary>
   public partial class Perawat : Window
   {

      public Perawat()
      {
         InitializeComponent();
         labeljam.Content = jam.berapa();
         labellogin.Content = siapa.anda;
         load_kabeh();
      }

      public void load_kabeh()
      {
         load_saya();
         status_boxtt(false);
         tombol_ett(true);
         tombol_gntp(true);
         status_gntp(false);
         tombol_gntp(true);
         status_gntp(false);

         tombol_trlab(true);
         status_trlab(false);
         load_datalab();
         load_datapass();
         load_jadwalD();
         load_jadwalP();
         tombolcustlab(true);
         tombolcustpass(true);
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

      /// <summary>
      /// batas bawah fungsi global
      /// </summary>

      private void status_boxtt(bool status)//fungsi kopas global
      {
         textBoxniptt.IsEnabled = false;
         textBoxnamtt.IsEnabled = status;
         textBoxalatt.IsEnabled = status;
         textBoxtlptt.IsEnabled = status;
         textBoxklmtt.IsEnabled = status;
      }

      private void tombol_ett(bool status)
      {
         ubahbtntt.IsEnabled = status;
         simpanbtntt.IsEnabled = !(status);
         bersihbtntt.IsEnabled = !(status);
         batalbtntt.IsEnabled = !(status);
      }

      private void clear_boxtt()
      {
         //textBoxniptt.Text = "";
         textBoxnamtt.Text = "";
         textBoxalatt.Text = "";
         textBoxtlptt.Text = "";
         textBoxklmtt.Text = "";
      }

      private void load_saya()
      {
         F.cn.Open();
         string sql = "select * from perawat where NIP_PERAWAT = '" + siapa.anda + "'";
         OracleDataReader reader;
         OracleCommand cmd = new OracleCommand(sql, F.cn);
         reader = cmd.ExecuteReader();
         if (reader.Read())
         {
            textBoxniptt.Text = reader.GetString(0);
            textBoxnamtt.Text = reader.GetString(1);
            textBoxalatt.Text = reader.GetString(2);
            textBoxklmtt.Text = reader.GetString(3);
            textBoxtlptt.Text = reader.GetString(4);
         }
         reader.Close();
         F.cn.Close();
         labelpengguna.Content = "Selamat datang " + textBoxnamtt.Text;
      }

      private void ubahbtntt_Click(object sender, RoutedEventArgs e)
      {
         tombol_ett(false);
         status_boxtt(true);
      }

      private void simpanbtntt_Click(object sender, RoutedEventArgs e)
      {
         string sql = "update PERAWAT set "
                         + " NAMA_PERAWAT = '" + textBoxnamtt.Text + "',"
                         + " ALAMAT_PERAWAT= '" + textBoxalatt.Text + "',"
                         + " JENISKELAMIN_PERAWAT = '" + textBoxklmtt.Text + "',"
                         + " TELP_PERAWAT = '" + textBoxtlptt.Text + "'"
                         + " where NIP_PERAWAT = '" + textBoxniptt.Text + "'";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah diupdate");
         }
         //method
         tombol_ett(true);
         status_boxtt(false);
         load_saya();
      }

      private void bersihbtntt_Click(object sender, RoutedEventArgs e)
      {
         clear_boxtt();
      }

      private void batalbtntt_Click(object sender, RoutedEventArgs e)
      {
         tombol_ett(true);
         status_boxtt(false);
         load_saya();
      }

      private void tesreportbtn_Click(object sender, RoutedEventArgs e)
      {
         Report1 r = new Report1();
         r.Show();
      }

      private void tombol_gntp(bool status)
      {
         gantibtn.IsEnabled = status;
         simpnbtn.IsEnabled = !(status);
         batlbtn.IsEnabled = !(status);
      }

      private void status_gntp(bool status)
      {
         passwordLama.IsEnabled = status;
         passwordBaru.IsEnabled = status;
         passwordBaru2.IsEnabled = status;
      }

      private void clear_gntp()
      {
         passwordLama.Password = "";
         passwordBaru.Password = "";
         passwordBaru2.Password = "";
      }

      private void simpnbtn_Click(object sender, RoutedEventArgs e)
      {
         F.GantiPass(siapa.anda, passwordLama.Password, passwordBaru.Password, passwordBaru2.Password);
         tombol_gntp(true);
         status_gntp(false);
         clear_gntp();
      }

      private void gantibtn_Click(object sender, RoutedEventArgs e)
      {
         tombol_gntp(false);
         status_gntp(true);
      }

      private void batlbtn_Click(object sender, RoutedEventArgs e)
      {
         tombol_gntp(true);
         status_gntp(false);
         clear_gntp();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      private void tombol_trlab(bool status)
      {
         tambahtranslab.IsEnabled = status;
         simpantranslab.IsEnabled = !(status);
         bersihtranslab.IsEnabled = !(status);
         bataltranslab.IsEnabled = !(status);
      }

      private void status_trlab(bool status)
      {
         textBoxnolab.IsEnabled = status;
         textBoxnopass.IsEnabled = status;
         textBoxtidlab.IsEnabled = status;
         datetanglab.IsEnabled = status;
         textBoxbiayalab.IsEnabled = status;
      }

      private void clear_trlab()
      {
         textBoxnopass.Text = "";
         textBoxtidlab.Text = "";
         datetanglab.Text = "";
         textBoxbiayalab.Text = "";
      }

      private void tambahtranslab_Click(object sender, RoutedEventArgs e)
      {
         tombol_trlab(false);
         status_trlab(true);
         clear_trlab();
         textBoxnolab.Text = F.getmaxnomor("maxadmlab"); 
      }

      private void simpantranslab_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into ADMINISTRASI_LABORATORIUM values (seqadmlab.nextval,"
                                   + "'" + textBoxnopass.Text + "',"
                                   + "'" + siapa.anda + "',"
                                   + "'" + textBoxtidlab.Text + "',"
                                   + "'" + datetanglab.Text + "',"
                                   + "'" + textBoxbiayalab.Text + "')";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah ditambahkan");
            tombol_trlab(true);
            status_trlab(false);
            F.setmaxnomor("maxadmlab", "seqadmlab");
         }
         else F.fixmaxnomornextval("maxadmlab", "seqadmlab");
      }

      private void bataltranslab_Click(object sender, RoutedEventArgs e)
      {
         tombol_trlab(true);
         status_trlab(false);
         clear_trlab();
      }

      private void bersihtranslab_Click(object sender, RoutedEventArgs e)
      {
         clear_trlab();
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      private void load_datalab()
      {
         F.dataview(F.load_data("ADMINISTRASI_LABORATORIUM"), dataGridPer);
      }

      private void tombolcustlab(bool status)
      {
         textnoadmlab.IsEnabled = status;
         caributlab.IsEnabled = status;
         kembalibutlab.IsEnabled = !(status);
      }

      private void caributlab_Click(object sender, RoutedEventArgs e)
      {
         if (textnoadmlab.Text != "")
         {
            F.dataview(F.load_data_cust("ADMINISTRASI_LABORATORIUM where ID_ADM_LAB = " + textnoadmlab.Text, "*"), dataGridPer);
            tombolcustlab(false);
         }
      }

      private void kembalibutlab_Click(object sender, RoutedEventArgs e)
      {
         load_datalab();
         tombolcustlab(true);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>

      private void load_datapass()
      {
         F.dataview(F.load_data("PASIEN"), dataGridS);
      }

      private void tombolcustpass(bool status)
      {
         textnos.IsEnabled = status;
         caributpass.IsEnabled = status;
         kembalibutpass.IsEnabled = !(status);
      }

      private void caributpass_Click(object sender, RoutedEventArgs e)
      {
         if (textnos.Text != "")
         {
            F.dataview(F.load_data_cust("PASIEN where NO_PASIEN = " + textnos.Text, "*"), dataGridS);
            tombolcustpass(false);
         }
      }

      private void kembalibutpass_Click(object sender, RoutedEventArgs e)
      {
         load_datapass();
         tombolcustpass(true);
      }

      /// <summary>
      /// 
      /// </summary>

      private void load_jadwalD()
      {
         F.dataview(F.load_data("JADWALDOKTER"), dataGridjadwaldok);
      }

      private void load_jadwalP()
      {
         F.dataview(F.load_data("JADWALPERAWAT"), dataGridjadwalper);
      }

      private void jdwlpersya_Click(object sender, RoutedEventArgs e)
      {
         F.dataview(F.load_data_cust("JADWALPERAWAT where NIP_PERAWAT =" + siapa.anda, "*"), dataGridjadwalper);
      }

      private void jdwlpersmua_Click(object sender, RoutedEventArgs e)
      {
         load_jadwalP();
      }
   }
}
