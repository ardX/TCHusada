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
   /// Interaction logic for Pasien.xaml
   /// </summary>
   public partial class Pasien : Window
   {
      
      public Pasien()
      {
         InitializeComponent();
         labeljam.Content = jam.berapa();
         labellogin.Content = siapa.anda;
         load_kabeh();
      }

      public void load_kabeh()
      {
         load_saya();
         status_boxtt(true);
         tombol_gntp(true);
         status_gntp(false);
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

      private void status_boxtt(bool status)
      {
         textBoxniptt.IsReadOnly = status;
         textBoxnamtt.IsReadOnly = status;
         textBoxalatt.IsReadOnly = status;
         textBoxklmtt.IsReadOnly = status;
         textBoxtlptt.IsReadOnly = status;
         datemsktt.IsEnabled = !status;
      }

      private void load_saya()
      {
         F.cn.Open();
         string sql = "select * from pasien where NO_PASIEN = '" + siapa.anda + "'";
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
            datemsktt.SelectedDate = reader.GetDateTime(5);
         }
         reader.Close();
         F.cn.Close();
         labelpengguna.Content = "Selamat datang " + textBoxnamtt.Text;
      }

      private void tesreportbtn_Click(object sender, RoutedEventArgs e)
      {
         TransaksiUGD r = new TransaksiUGD();
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
      
   }
}
