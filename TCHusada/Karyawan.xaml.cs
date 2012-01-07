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
      bool bolehK = true;
      int pilihK = 0;
      bool bolehD = true;
      int pilihD = 0;
      bool bolehP = true;
      int pilihP = 0;
      bool bolehS = true;
      int pilihS = 0;
      
      public Karyawan()
      {
         InitializeComponent();
         labeljam.Content = jam.berapa();
         labellogin.Content = siapa.anda;
         load_kabeh();
      }

      public void load_kabeh()
      {
         F.dataview(F.load_data("karyawan"), dataGridK);
         status_boxk(false);
         tombol_ek(true);

         F.dataview(F.load_data("dokter"), dataGridD);
         status_boxd(false);
         tombol_ed(true);

         F.dataview(F.load_data("perawat"), dataGridP);
         status_boxp(false);
         tombol_ep(true);

         F.dataview(F.load_data("pasien"), dataGridS);
         status_boxs(false);
         tombol_es(true);

         load_saya();
         status_boxtt(false);
         tombol_ett(true);

         tombol_gntp(true);
         status_gntp(false);

         tombol_trugd(true);
         status_trugd(false);

         tombol_trpoli(true);
         status_trpoli(false);
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
         //textnipk.Text = "auto"; autonumber from database
         textnamk.Text = "";
         textalmtk.Text = "";
         texttelpk.Text = "";
         datemskk.Text = "";
      }

      private void simpanbtnk_Click(object sender, RoutedEventArgs e)
      {
         int ind = -1;
         bool error= false;
         if (pilihK == 1)
         {
            error = false;
            string sql = "insert into KARYAWAN values (seqkar.nextval,"
                          + "'" + textnamk.Text + "',"
                          + "'" + textalmtk.Text + "',"
                          + "'" + texttelpk.Text + "',"
                          + "'" + datemskk.Text + "')";
            if (F.Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridK.Items.Count;
               F.setmaxnomor("maxkaryawan", "seqkar");
            }
            else error = true;
            
         }

         else if (pilihK == 2)
         {
            error = false;
            string sql = "update KARYAWAN set "
                         + " NAMA_KARYAWAN = '" + textnamk.Text + "',"
                         + " ALAMAT_KARYAWAN= '" + textalmtk.Text + "',"
                         + " NO_TELP_KARYAWAN = '" + texttelpk.Text + "',"
                         + " TGL_MASUK_KARYAWAN = '" + datemskk.Text + "'"
                         + " where NIP_KARYAWAN = '" + textnipk.Text + "'";
            if (F.Execute(sql))
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
            bolehK = true;
            pilihK = 0;
            F.dataview(F.load_data("karyawan"), dataGridK);
            dataGridK.SelectedIndex = ind;
         }
      }

      private void bersihbtnk_Click(object sender, RoutedEventArgs e)
      {
         clear_boxk();
      }

      private void batalbtnk_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridK.SelectedIndex;
         F.dataview(F.load_data("karyawan"), dataGridK);
         status_boxk(false);
         tombol_ek(true);
         bolehK = true;
         pilihK = 0;
         textnipk.Text = "";
         dataGridK.SelectedIndex = ind;
      }

      private void tambahbtnk_Click(object sender, RoutedEventArgs e)
      {
         bolehK = false;
         dataGridK.UnselectAll();
         clear_boxk();
         status_boxk(true);
         tombol_ek(false);
         pilihK = 1;
         //textnipk.Text = "AUTONUMBER"; textnipk.IsEnabled = false;
         textnipk.Text = F.getmaxnomor("maxkaryawan"); textnipk.IsEnabled = false;
      }

      private void ubahbtnk_Click(object sender, RoutedEventArgs e)
      {
         if (textnipk.Text != "")
         {
            bolehK = false;
            status_boxk(true);
            tombol_ek(false);
            pilihK = 2;
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
               string sql = "delete from KARYAWAN where NIP_KARYAWAN = " + textnipk.Text;
               if (F.Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               F.dataview(F.load_data("karyawan"), dataGridK);
               dataGridK.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtnk_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridK.SelectedIndex;
         F.dataview(F.load_data("karyawan"), dataGridK);
         dataGridK.SelectedIndex = ind;
      }

      private void dataGridK_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if(!bolehK)
            dataGridK.UnselectAll();
      }

      /// <summary>
      /// batas bawah tab karyawan
      /// </summary>
      /// 

      private void status_boxd(bool status)//fungsi kopas global
      {
         textnipd.IsEnabled = status;
         textnamd.IsEnabled = status;
         textalmtd.IsEnabled = status;
         combokeld.IsEnabled = status;
         texttelpd.IsEnabled = status;
      }

      private void tombol_ed(bool status)//fungsi kopas global
      {
         tambahbtnd.IsEnabled = status;
         ubahbtnd.IsEnabled = status;
         hapusbtnd.IsEnabled = status;
         simpanbtnd.IsEnabled = !(status);
         bersihbtnd.IsEnabled = !(status);
         batalbtnd.IsEnabled = !(status);
      }

      private void clear_boxd()//fungsi kopas global
      {
         textnamd.Text = "";
         textalmtd.Text = "";
         combokeld.Text = "";
         texttelpd.Text = "";
      }

      private void simpanbtnd_Click(object sender, RoutedEventArgs e)
      {
         int ind = -1;
         bool error = false;
         if (pilihD == 1)
         {
            error = false;
            string sql = "insert into DOKTER values (seqdok.nextval,"
                          + "'" + textnamd.Text + "',"
                          + "'" + textalmtd.Text + "',"
                          + "'" + combokeld.Text + "',"
                          + "'" + texttelpd.Text + "')";
            if (F.Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridD.Items.Count;
               F.setmaxnomor("maxdokter", "seqdok");
            }
            else error = true;

         }

         else if (pilihD == 2)
         {
            error = false;
            string sql = "update DOKTER set "
                         + " NAMA_DOKTER = '" + textnamd.Text + "',"
                         + " ALAMAT_DOKTER= '" + textalmtd.Text + "',"
                         + " JENISKELAMIN_DOKTER = '" + combokeld.Text + "',"
                         + " TELP_DOKTER = '" + texttelpd.Text + "'"
                         + " where NIP_DOKTER = '" + textnipd.Text + "'";
            if (F.Execute(sql))
            {
               MessageBox.Show("Data telah diupdate");
               ind = dataGridD.SelectedIndex;
            }
            else error = true;
         }
         if (!error)
         {
            status_boxd(false);
            tombol_ed(true);
            bolehD = true;
            pilihD = 0;
            F.dataview(F.load_data("dokter"), dataGridD);
            dataGridD.SelectedIndex = ind;
         }
      }

      private void bersihbtnd_Click(object sender, RoutedEventArgs e)
      {
         clear_boxd();
      }

      private void batalbtnd_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridD.SelectedIndex;
         combokeld.SelectedIndex = -1;
         F.dataview(F.load_data("dokter"), dataGridD);
         status_boxd(false);
         tombol_ed(true);
         bolehD = true;
         pilihD = 0;
         textnipd.Text = "";
         dataGridD.SelectedIndex = ind;
      }

      private void tambahbtnd_Click(object sender, RoutedEventArgs e)
      {
         bolehD = false;
         dataGridD.UnselectAll();
         clear_boxd();
         status_boxd(true);
         tombol_ed(false);
         pilihD = 1;
         //textnipd.Text = "AUTONUMBER"; textnipd.IsEnabled = false;
         textnipd.Text = F.getmaxnomor("maxdokter"); textnipd.IsEnabled = false;
      }

      private void ubahbtnd_Click(object sender, RoutedEventArgs e)
      {
         if (textnipd.Text != "")
         {
            bolehD = false;
            status_boxd(true);
            tombol_ed(false);
            pilihD = 2;
            textnipd.IsEnabled = false;
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void hapusbtnd_Click(object sender, RoutedEventArgs e)
      {
         if (textnipd.Text != "")
         {
            if (MessageBox.Show("Hapus entry \"" + textnipd.Text + "\" dari tabel ?", "Delete " + textnipd.Text, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
               string sql = "delete from dokter where NIP_DOKTER = " + textnipd.Text;
               if (F.Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               F.dataview(F.load_data("dokter"), dataGridD);
               dataGridD.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtnd_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridD.SelectedIndex;
         F.dataview(F.load_data("dokter"), dataGridD);
         dataGridD.SelectedIndex = ind;
      }

      private void dataGridD_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (!bolehD)
            dataGridD.UnselectAll();
      }

      /// <summary>
      /// batas bawah tab dokter
      /// </summary>
      /// 


      private void status_boxp(bool status)//fungsi kopas global
      {
         textnipp.IsEnabled = status;
         textnamp.IsEnabled = status;
         textalmtp.IsEnabled = status;
         combokelp.IsEnabled = status;
         texttelpp.IsEnabled = status;
      }

      private void tombol_ep(bool status)//fungsi kopas global
      {
         tambahbtnp.IsEnabled = status;
         ubahbtnp.IsEnabled = status;
         hapusbtnp.IsEnabled = status;
         simpanbtnp.IsEnabled = !(status);
         bersihbtnp.IsEnabled = !(status);
         batalbtnp.IsEnabled = !(status);
      }

      private void clear_boxp()//fungsi kopas global
      {
         textnamp.Text = "";
         textalmtp.Text = "";
         combokelp.Text = "";
         texttelpp.Text = "";
      }

      private void simpanbtnp_Click(object sender, RoutedEventArgs e)
      {
         int ind = -1;
         bool error = false;
         if (pilihP == 1)
         {
            error = false;
            string sql = "insert into PERAWAT values (seqper.nextval,"
                          + "'" + textnamp.Text + "',"
                          + "'" + textalmtp.Text + "',"
                          + "'" + combokelp.Text + "',"
                          + "'" + texttelpp.Text + "')";
            if (F.Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridP.Items.Count;
               F.setmaxnomor("maxperawat", "seqper");
            }
            else error = true;

         }

         else if (pilihP == 2)
         {
            error = false;
            string sql = "update PERAWAT set "
                         + " NAMA_PERAWAT = '" + textnamp.Text + "',"
                         + " ALAMAT_PERAWAT= '" + textalmtp.Text + "',"
                         + " JENISKELAMIN_PERAWAT = '" + combokelp.Text + "',"
                         + " TELP_PERAWAT = '" + texttelpp.Text + "'"
                         + " where NIP_PERAWAT = '" + textnipp.Text + "'";
            if (F.Execute(sql))
            {
               MessageBox.Show("Data telah diupdate");
               ind = dataGridP.SelectedIndex;
            }
            else error = true;
         }
         if (!error)
         {
            status_boxp(false);
            tombol_ep(true);
            bolehP = true;
            pilihP = 0;
            F.dataview(F.load_data("PERAWAT"), dataGridP);
            dataGridP.SelectedIndex = ind;
         }
      }

      private void bersihbtnp_Click(object sender, RoutedEventArgs e)
      {
         clear_boxp();
      }

      private void batalbtnp_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridP.SelectedIndex;
         combokelp.SelectedIndex = -1;
         F.dataview(F.load_data("PERAWAT"), dataGridP);
         status_boxp(false);
         tombol_ep(true);
         bolehP = true;
         pilihP = 0;
         textnipp.Text = "";
         dataGridP.SelectedIndex = ind;
      }

      private void tambahbtnp_Click(object sender, RoutedEventArgs e)
      {
         bolehP = false;
         dataGridP.UnselectAll();
         clear_boxp();
         status_boxp(true);
         tombol_ep(false);
         pilihP = 1;
         //textnipp.Text = "AUTONUMBER"; textnipp.IsEnabled = false;
         textnipp.Text = F.getmaxnomor("maxperawat"); textnipp.IsEnabled = false;
      }

      private void ubahbtnp_Click(object sender, RoutedEventArgs e)
      {
         if (textnipp.Text != "")
         {
            bolehP = false;
            status_boxp(true);
            tombol_ep(false);
            pilihP = 2;
            textnipp.IsEnabled = false;
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void hapusbtnp_Click(object sender, RoutedEventArgs e)
      {
         if (textnipp.Text != "")
         {
            if (MessageBox.Show("Hapus entry \"" + textnipp.Text + "\" dari tabel ?", "Delete " + textnipp.Text, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
               string sql = "delete from PERAWAT where NIP_PERAWAT = " + textnipp.Text;
               if (F.Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               F.dataview(F.load_data("PERAWAT"), dataGridP);
               dataGridP.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtnp_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridP.SelectedIndex;
         F.dataview(F.load_data("PERAWAT"), dataGridP);
         dataGridP.SelectedIndex = ind;
      }

      private void dataGridP_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (!bolehP)
            dataGridP.UnselectAll();
      }

      /// <summary>
      /// batas bawah tab PERAWAT
      /// </summary>
      /// 


      private void status_boxs(bool status)//fungsi kopas global
      {
         textnos.IsEnabled = status;
         textnams.IsEnabled = status;
         textalmts.IsEnabled = status;
         combokels.IsEnabled = status;
         texttelps.IsEnabled = status;
         datelhrs.IsEnabled = status;
      }

      private void tombol_es(bool status)//fungsi kopas global
      {
         tambahbtns.IsEnabled = status;
         ubahbtns.IsEnabled = status;
         hapusbtns.IsEnabled = status;
         simpanbtns.IsEnabled = !(status);
         bersihbtns.IsEnabled = !(status);
         batalbtns.IsEnabled = !(status);
      }

      private void clear_boxs()//fungsi kopas global
      {
         textnams.Text = "";
         textalmts.Text = "";
         combokels.Text = "";
         texttelps.Text = "";
         datelhrs.Text = "";
      }

      private void simpanbtns_Click(object sender, RoutedEventArgs e)
      {
         int ind = -1;
         bool error = false;
         if (pilihS == 1)
         {
            error = false;
            string sql = "insert into PASIEN values (seqpas.nextval,"
                          + "'" + textnams.Text + "',"
                          + "'" + textalmts.Text + "',"
                          + "'" + combokels.Text + "',"
                    + "'" + texttelps.Text + "',"
                          + "'" + datelhrs.Text + "')";
            if (F.Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridS.Items.Count;
               F.setmaxnomor("maxpasien","seqpas");
            }
            else error = true;

         }

         else if (pilihS == 2)
         {
            error = false;
            string sql = "update PASIEN set "
                         + " NAMA_PASIEN = '" + textnams.Text + "',"
                         + " ALAMAT_PASIEN= '" + textalmts.Text + "',"
                         + " KELAMIN_PASIEN = '" + combokels.Text + "',"
                   + " TELP_PASIEN = '" + texttelps.Text + "',"
                         + " LAHIR_PASIEN = '" + datelhrs.Text + "'"
                         + " where NO_PASIEN = '" + textnos.Text + "'";
            if (F.Execute(sql))
            {
               MessageBox.Show("Data telah diupdate");
               ind = dataGridS.SelectedIndex;
            }
            else error = true;
         }
         if (!error)
         {
            status_boxs(false);
            tombol_es(true);
            bolehS = true;
            pilihS = 0;
            F.dataview(F.load_data("PASIEN"), dataGridS);
            dataGridS.SelectedIndex = ind;
         }
      }

      private void bersihbtns_Click(object sender, RoutedEventArgs e)
      {
         clear_boxs();
      }

      private void batalbtns_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridS.SelectedIndex;
         combokels.SelectedIndex = -1;
         F.dataview(F.load_data("PASIEN"), dataGridS);
         status_boxs(false);
         tombol_es(true);
         bolehS = true;
         pilihS = 0;
         textnos.Text = "";
         dataGridS.SelectedIndex = ind;
      }

      private void tambahbtns_Click(object sender, RoutedEventArgs e)
      {
         bolehS = false;
         dataGridS.UnselectAll();
         clear_boxs();
         status_boxs(true);
         tombol_es(false);
         pilihS = 1;
         //textnos.Text = "AUTONUMBER"; textnos.IsEnabled = false;
         textnos.Text = F.getmaxnomor("maxpasien"); textnos.IsEnabled = false;
      }

      private void ubahbtns_Click(object sender, RoutedEventArgs e)
      {
         if (textnos.Text != "")
         {
            bolehS = false;
            status_boxs(true);
            tombol_es(false);
            pilihS = 2;
            textnos.IsEnabled = false;
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void hapusbtns_Click(object sender, RoutedEventArgs e)
      {
         if (textnos.Text != "")
         {
            if (MessageBox.Show("Hapus entry \"" + textnos.Text + "\" dari tabel ?", "Delete " + textnos.Text, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
               string sql = "delete from PASIEN where NO_PASIEN = " + textnos.Text;
               if (F.Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               F.dataview(F.load_data("PASIEN"), dataGridS);
               dataGridS.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtns_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridS.SelectedIndex;
         F.dataview(F.load_data("PASIEN"), dataGridS);
         dataGridS.SelectedIndex = ind;
      }

      private void dataGridS_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (!bolehS)
            dataGridS.UnselectAll();
      }

      /// <summary>
      /// batas bawah tab PASIEN
      /// </summary>
      /// 

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
         textBoxnougd.Text = F.getmaxnomor("maxadmugd"); textBoxnougd.IsEnabled = false;
      }

      private void simpantransugd_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into ADMINISTRASIUGD values (seqadugd.nextval,"
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
            F.setmaxnomor("maxadmugd", "seqadugd");
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
         textBoxnotranspoli.Text = F.getmaxnomor("maxadmpoli"); textBoxnotranspoli.IsEnabled = false;
      
      }

      private void simpantranspoli_Click(object sender, RoutedEventArgs e)
      {
         string sql = "insert into ADMINISTRASIPOLI values (seqadpoli.nextval,"
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
            F.setmaxnomor("maxadmpoli", "seqadpoli");
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

      

   }
}
