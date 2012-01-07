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
   /// Interaction logic for Dokter.xaml
   /// </summary>
   public partial class Dokter : Window
   {

      public Dokter()
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

         F.dataview(F.load_data("RIWAYATPASIEN"), dataGridRM);
         status_boxrm(false);
         tombol_erm(true);
         tombolcust(true);

         load_obat();

         teraptbl(false);
         tambahtbl(false);
         tambahresep.IsEnabled = true;

         load_jadwalD();
         load_jadwalP();
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
         string sql = "select * from dokter where NIP_DOKTER = '" + siapa.anda + "'";
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
         string sql = "update DOKTER set "
                         + " NAMA_DOKTER = '" + textBoxnamtt.Text + "',"
                         + " ALAMAT_DOKTER= '" + textBoxalatt.Text + "',"
                         + " JENISKELAMIN_DOKTER = '" + textBoxklmtt.Text + "',"
                         + " TELP_DOKTER = '" + textBoxtlptt.Text + "'"
                         + " where NIP_DOKTER = '" + textBoxniptt.Text + "'";
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
      /// batas bawah profil
      /// </summary>
      /// <param name="status"></param>

      private void status_boxrm(bool status)//fungsi kopas global
      {
         textidrm.IsEnabled = status;
         textnopasrm.IsEnabled = status;
         textpenyrm.IsEnabled = status;
         datemasrm.IsEnabled = status;
         datekelrm.IsEnabled = status;
      }

      private void tombol_erm(bool status)//fungsi kopas global
      {
         tambahrm.IsEnabled = status;
         simpanrm.IsEnabled = !(status);
         bersihrm.IsEnabled = !(status);
         batalrma.IsEnabled = !(status);
      }

      private void clear_boxrm()//fungsi kopas global
      {
         textnopasrm.Text = "";
         textpenyrm.Text = "";
         datemasrm.Text = "";
         datekelrm.Text = "";
      }
      bool bolehRM = true;
      private void tambahrm_Click(object sender, RoutedEventArgs e)
      {
         bolehRM = false;
         dataGridRM.UnselectAll();
         clear_boxrm();
         status_boxrm(true);
         tombol_erm(false);
         textidrm.Text = F.getmaxnomor("maxpasien"); textidrm.IsEnabled = false;
      }

      private void simpanrm_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into RIWAYATPASIEN values (seqrm.nextval,"
                          + "'" + siapa.anda + "',"
                          + "'" + textnopasrm.Text + "',"
                          + "'" + textpenyrm.Text + "',"
                          + "'" + datemasrm.Text + "',"
                          + "'" + datekelrm.Text + "')";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah ditambahkan");
            int ind = dataGridRM.SelectedIndex;
            status_boxrm(false);
            tombol_erm(true);
            bolehRM = true;
            F.dataview(F.load_data("RIWAYATPASIEN"), dataGridRM);
            dataGridRM.SelectedIndex = ind;
            F.setmaxnomor("maxpasien", "seqrm");
         }
      }

      private void bersihrm_Click(object sender, RoutedEventArgs e)
      {
         clear_boxrm();
      }

      private void batalrma_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridRM.SelectedIndex;
         F.dataview(F.load_data("RIWAYATPASIEN"), dataGridRM);
         status_boxrm(false);
         tombol_erm(true);
         bolehRM = true;
         textidrm.Text = "";
         dataGridRM.SelectedIndex = ind;
      }

      private void refreshrm_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridRM.SelectedIndex;
         F.dataview(F.load_data("RIWAYATPASIEN"), dataGridRM);
         dataGridRM.SelectedIndex = ind;
      }

      private void dataGridRM_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (!bolehRM)
            dataGridRM.UnselectAll();
      }

      private void tombolcust(bool status)
      {
         tampilcust.IsEnabled = status;
         nopasscust.IsEnabled = status;
         kembalicust.IsEnabled = !(status);
      }

      private void tampilcust_Click(object sender, RoutedEventArgs e)
      {
         if (nopasscust.Text != "")
         {
            F.dataview(F.load_data_cust("RIWAYATPASIEN where no_pasien = " + nopasscust.Text, "*"), dataGridRM);
            tombolcust(false);
         }
      }

      private void kembalicust_Click(object sender, RoutedEventArgs e)
      {
         F.dataview(F.load_data("RIWAYATPASIEN"), dataGridRM);
         tombolcust(true);
      }

      /// <summary>
      /// batas bawah rekam
      /// </summary>
      /// <param name="status"></param>
      /// 

      private void load_detail_obat(string idrsp)
      {
         F.dataview(F.load_data_cust("detail_resep d, obat o where d.nomor_obat = o.nomor_obat and  id_resep = '" + idrsp + "'", "d.nomor_obat, o.nama_obat, o.bentuk_obat, o.merk_obat"), dataGridDetil);
      }

      private void load_obat()
      {
         F.dataview(F.load_data("OBAT"), dataGridObat);
      }

      private void tambahtbl(bool status)
      {
         groupBoxresep.IsEnabled = status;
         applyresep.IsEnabled = status;
         tambahresep.IsEnabled = !(status);
      }

      private void teraptbl(bool status)
      {
         groupBoxdetil.IsEnabled = status;
         simpanresep.IsEnabled = status;
      }

      private void tambahresep_Click(object sender, RoutedEventArgs e)
      {
         tambahtbl(true);
      }

      private void applyresep_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into RESEP values ("
                          + "'" + textidresep.Text + "',"
                          + "'" + textidriwayatres.Text + "',"
                          + "'" + siapa.anda + "',"
                          + "'" + texthargatotres.Text + "')";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah ditambahkan");
            tambahtbl(false);
            teraptbl(true);
            tambahresep.IsEnabled = false;

            load_detail_obat(textidresep.Text);
         }
      }

      private void simpanresep_Click(object sender, RoutedEventArgs e)
      {
         string sql = "update RESEP set "
                         + " HARGA_TOTAL = '" + texthargatotres.Text + "'"
                         + " where ID_RESEP = '" + textidresep.Text + "'";
         if (F.Execute(sql))
         {
            MessageBox.Show("Data telah diupdate");
         }

         teraptbl(false);
         tambahresep.IsEnabled = true;
      }

      private void tambahobat_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into DETAIL_RESEP values ("
                          + "'" + textnoobatres.Text + "',"
                          + "'" + textidresep.Text + "')";
         if (F.Execute(sql))
         {
            //MessageBox.Show("Data telah ditambahkan");
            load_detail_obat(textidresep.Text);
            texthargatotres.Text = F.getsumresep(textidresep.Text);
         }
      }

      private void hapusobat_Click(object sender, RoutedEventArgs e)
      {
         if (textnoobatres.Text != "")
         {
            if (MessageBox.Show("Hapus entry \"" + textnoobatres.Text + "\" dari tabel ?", "Delete " + textnoobatres.Text, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
               string sql = "delete from DETAIL_RESEP where NOMOR_OBAT = '" + textnoobatres.Text + "' and ID_RESEP = '" + textidresep.Text + "'";
               if (F.Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               load_detail_obat(textidresep.Text);
               texthargatotres.Text = F.getsumresep(textidresep.Text);
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);

      }

      /// <summary>
      /// batas bawah resep
      /// </summary>

      private void load_jadwalD()
      {
         F.dataview(F.load_data("JADWALDOKTER"), dataGridjadwaldok);
      }

      private void load_jadwalP()
      {
         F.dataview(F.load_data("JADWALPERAWAT"), dataGridjadwalper);
      }

      private void jdwldoksya_Click(object sender, RoutedEventArgs e)
      {
         F.dataview(F.load_data_cust("JADWALDOKTER where NIP_DOKTER =" + siapa.anda, "*"), dataGridjadwaldok);
      }

      private void jdwldoksmua_Click(object sender, RoutedEventArgs e)
      {
         load_jadwalD();
      }
   }
}
