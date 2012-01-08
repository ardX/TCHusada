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
      public Karyawan()
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

         tombol_trugd(true);
         status_trugd(false);
         tombol_trpsk(true);
         status_trpsk(false);
         groupBoxUGD.IsEnabled = false;

         tombol_trpoli(true);
         status_trpoli(false);

         load_dataUGD();
         load_dataPoli();
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
         datemsktt.IsEnabled = status;
      }

      private void tombol_ett(bool status)//fungsi kopas global
      {
         ubahbtntt.IsEnabled = status;
         simpanbtntt.IsEnabled = !(status);
         bersihbtntt.IsEnabled = !(status);
         batalbtntt.IsEnabled = !(status);
      }

      private void clear_boxtt()//fungsi kopas global
      {
         //textBoxniptt.Text = "";
         textBoxnamtt.Text = "";
         textBoxalatt.Text = "";
         textBoxtlptt.Text = "";
         datemsktt.Text = "";
      }

      private void load_saya()
      {
         //MessageBox.Show(siapa.anda);
         F.cn.Open();
         string sql = "select * from karyawan where NIP_KARYAWAN = '" + siapa.anda + "'";
         OracleDataReader reader;
         OracleCommand cmd = new OracleCommand(sql, F.cn);
         reader = cmd.ExecuteReader();
         if (reader.Read())
         {
            textBoxniptt.Text = reader.GetString(0);
            textBoxnamtt.Text = reader.GetString(1);
            textBoxalatt.Text = reader.GetString(2);
            textBoxtlptt.Text = reader.GetString(3);
            datemsktt.SelectedDate = reader.GetDateTime(4);
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
         string sql = "update KARYAWAN set "
                         + " NAMA_KARYAWAN = '" + textBoxnamtt.Text + "',"
                         + " ALAMAT_KARYAWAN= '" + textBoxalatt.Text + "',"
                         + " NO_TELP_KARYAWAN = '" + textBoxtlptt.Text + "',"
                         + " TGL_MASUK_KARYAWAN = '" + datemsktt.Text + "'"
                         + " where NIP_KARYAWAN = '" + textBoxniptt.Text + "'";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah diupdate");
         }
         //method
         tombol_ett(true);
         status_boxtt(false);
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
      /// batas bawah profil
      /// </summary>
      /// <param name="status"></param>

      private void tombol_trpsk(bool status)
      {
         tambahpskugd.IsEnabled = status;
         simpanpskugd.IsEnabled = !(status);
         bersihpskugd.IsEnabled = !(status);
         batalpskugd.IsEnabled = !(status);
      }

      private void status_trpsk(bool status)
      {
         textBoxnoper.IsEnabled = status;
         textBoxdokugd.IsEnabled = status;
         textBoxkamarugd.IsEnabled = status;
         textBoxnopasugd.IsEnabled = status;
         textBoxjamugd.IsEnabled = status;
      }

      private void clear_trpsk()
      {
         textBoxdokugd.Text = "";
         textBoxkamarugd.Text = "";
         textBoxnopasugd.Text = "";
         textBoxjamugd.Text = "";
      }

      private void tambahpskugd_Click(object sender, RoutedEventArgs e)
      {
         tombol_trpsk(false);
         status_trpsk(true);
         clear_trpsk();
         textBoxnoper.Text = F.getmaxnomor("maxpskugd"); 
      }

      private void simpanpskugd_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into ID_PEMERIKSAANUGD values (seqpskugd.nextval,"
                          + "'" + textBoxdokugd.Text + "',"
                          + "'" + textBoxkamarugd.Text + "',"
                          + "'" + textBoxnopasugd.Text + "',"
                          + "'" + textBoxjamugd.Text + "')";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah ditambahkan");
            tombol_trpsk(true);
            status_trpsk(false);
            F.setmaxnomor("maxpskugd", "seqpskugd");
            groupBoxUGD.IsEnabled = true;
            groupBoxpsk.IsEnabled = true;
         }
      }

      private void bersihpskugd_Click(object sender, RoutedEventArgs e)
      {
         clear_trpsk();
      }

      private void batalpskugd_Click(object sender, RoutedEventArgs e)
      {
         tombol_trpsk(true);
         status_trpsk(false);
         clear_trpsk();
      }


      private void tombol_trugd(bool status)
      {
         tambahtransugd.IsEnabled = status;
         simpantransugd.IsEnabled = !(status);
         bersihtransugd.IsEnabled = !(status);
         bataltransugd.IsEnabled = !(status);
      } 

      private void status_trugd(bool status)
      {
         textBoxnougd.IsEnabled = status;
         textBoxnopass.IsEnabled = status;
         datetangugd.IsEnabled = status;
         textBoxtkeluhanugd.IsEnabled = status;
         textBoxbiayaugd.IsEnabled = status;
         textketugd.IsEnabled = status;
      }

      private void clear_trugd()
      {
         textBoxnopass.Text = "";
         datetangugd.Text = "";
         textBoxtkeluhanugd.Text = "";
         textBoxbiayaugd.Text = "";
         textketugd.Text = "";
      }

      private void tambahtransugd_Click(object sender, RoutedEventArgs e)
      {
         tombol_trugd(false);
         status_trugd(true);
         clear_trugd();
         textBoxnougd.Text = F.getmaxnomor("maxadmugd"); 
      }

      private void simpantransugd_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into ADMINISTRASIUGD values (seqadmugd.nextval,"
                          + "'" + siapa.anda + "',"
                          + "'" + textBoxnopass.Text + "',"
                          + "'" + datetangugd.Text + "',"
                          + "'" + textBoxtkeluhanugd.Text + "',"
                          + "'" + textBoxbiayaugd.Text + "',"
                          + "'" + textketugd.Text + "')";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah ditambahkan");
            tombol_trugd(true);
            status_trugd(false);
            F.setmaxnomor("maxadmugd", "seqadmugd");
            groupBoxUGD.IsEnabled = false;
            groupBoxpsk.IsEnabled = true;
         }
      }

      private void bataltransugd_Click(object sender, RoutedEventArgs e)
      {
         tombol_trugd(true);
         status_trugd(false);
         clear_trugd();
      }

      private void bersihtransugd_Click(object sender, RoutedEventArgs e)
      {
         clear_trugd();
      }

      /// <summary>
      /// batas bawah ugd
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>


      private void tombol_trpoli(bool status)
      {
         tambahtranspoli.IsEnabled = status;
         simpantranspoli.IsEnabled = !(status);
         bersihtranspoli.IsEnabled = !(status);
         bataltranspoli.IsEnabled = !(status);
      }

      private void status_trpoli(bool status)
      {
         textBoxnotranspoli.IsEnabled = status;
         textBoxnopasspoli.IsEnabled = status;
         datetangpoli.IsEnabled = status;
         textBoxtkeluhanpoli.IsEnabled = status;
         textBoxbiayapoli.IsEnabled = status;
         textketpoli.IsEnabled = status;
         textBoxidpoli.IsEnabled = status;
      }

      private void clear_trpoli()
      {
         textBoxnopasspoli.Text = "";
         datetangpoli.Text = "";
         textBoxtkeluhanpoli.Text = "";
         textBoxbiayapoli.Text = "";
         textketpoli.Text = "";
         textBoxidpoli.Text = "";
      }

      private void tambahtranspoli_Click(object sender, RoutedEventArgs e)
      {
         tombol_trpoli(false);
         status_trpoli(true);
         clear_trpoli();
         textBoxnotranspoli.Text = F.getmaxnomor("maxadmpoli");
      
      }

      private void simpantranspoli_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into ADMINISTRASIPOLI values (seqadmpoli.nextval,"
                          + "'" + textBoxnopasspoli.Text + "',"
                          + "'" + siapa.anda + "',"
                          + "'" + textBoxidpoli.Text + "',"
                          + "'" + datetangpoli.Text + "',"
                          + "'" + textBoxtkeluhanpoli.Text + "',"
                          + "'" + textBoxbiayapoli.Text + "',"
                          + "'" + textketpoli.Text + "')";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah ditambahkan");
            tombol_trpoli(true);
            status_trpoli(false);
            F.setmaxnomor("maxadmpoli", "seqadmpoli");
         }
      }

      private void bataltranspoli_Click(object sender, RoutedEventArgs e)
      {
         tombol_trpoli(true);
         status_trpoli(false);
         clear_trpoli();
      }

      private void bersihtranspoli_Click(object sender, RoutedEventArgs e)
      {
         clear_trpoli();
      }


      /// <summary>
      /// 
      /// </summary>

      private void tombolcustUGD(bool status)
      {
         textnoadmugd.IsEnabled = status;
         caributugd.IsEnabled = status;
         kembalibutugd.IsEnabled = !(status);
      }

      private void tombolcustPoli(bool status)
      {
         textnoadmpoli.IsEnabled = status;
         caributpoli.IsEnabled = status;
         kembalibutpoli.IsEnabled = !(status);
      }

      private void load_dataUGD()
      {
         F.dataview(F.load_data("ADMINISTRASIUGD"), dataGridUGD);
      }

      private void load_dataPoli()
      {
         F.dataview(F.load_data("ADMINISTRASIPOLI"), dataGridPoli);
      }

      private void caributugd_Click(object sender, RoutedEventArgs e)
      {
         if (textnoadmugd.Text != "")
         {
            F.dataview(F.load_data_cust("ADMINISTRASIUGD where ID_ADM = " + textnoadmugd.Text, "*"), dataGridUGD);
            tombolcustUGD(false);
         }
      }

      private void kembalibutugd_Click(object sender, RoutedEventArgs e)
      {
         load_dataUGD();
         tombolcustUGD(true);
      }

      private void caributpoli_Click(object sender, RoutedEventArgs e)
      {
         if (textnoadmpoli.Text != "")
         {
            F.dataview(F.load_data_cust("ADMINISTRASIPOLI where ID_ADM2 = " + textnoadmpoli.Text, "*"), dataGridPoli);
            tombolcustPoli(false);
         }
      }

      private void kembalibutpoli_Click(object sender, RoutedEventArgs e)
      {
         load_dataPoli();
         tombolcustPoli(true);
      }

      

   }
}
