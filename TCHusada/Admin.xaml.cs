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
   /// Interaction logic for Admin.xaml
   /// </summary>
   public partial class Admin : Window
   {
      bool bolehK = true;
      int pilihK = 0;
      bool bolehD = true;
      int pilihD = 0;
      bool bolehP = true;
      int pilihP = 0;
      bool bolehS = true;
      int pilihS = 0;
      bool bolehU = true;
      int pilihU = 0;

      public Admin()
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

         F.dataview(F.load_data("pengguna"), dataGridU);
         status_boxusr(false);
         tombol_eusr(true);
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
         bool error = false;
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
         textnipk.Text = F.getmaxnomor("maxkaryawan");
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
         if (!bolehK)
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
         textnipd.Text = F.getmaxnomor("maxdokter");
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
         textnipp.Text = F.getmaxnomor("maxperawat");
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
               F.setmaxnomor("maxpasien", "seqpas");
            }
            else error = true;

         }

         else if (pilihS == 2)
         {
            error = false;
            string sql = "update PASIEN set "
                         + " NAMA_PASIEN = '" + textnams.Text + "',"
                         + " ALAMAT_PASIEN= '" + textalmts.Text + "',"
                         + " JENISKELAMIN_PASIEN = '" + combokels.Text + "',"
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
         textnos.Text = F.getmaxnomor("maxpasien");
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

      private void status_boxusr(bool status)
      {
         textusername.IsEnabled = status;
         passwordBox1.IsEnabled = status;
         passwordBox2.IsEnabled = status;
         passwordBox3.IsEnabled = status;
      }

      private void tombol_eusr(bool status)
      {
         tambahuser.IsEnabled = status;
         ubahuser.IsEnabled = status;
         hapususer.IsEnabled = status;
         simpanbtnuser.IsEnabled = !(status);
         bersihuser.IsEnabled = !(status);
         bataluser.IsEnabled = !(status);
      }

      private void clear_boxusr()
      {
         textusername.Text = "";
         passwordBox1.Password = "";
         passwordBox2.Password = "";
         passwordBox3.Password = "";
      }

      private void simpanbtnuser_Click(object sender, RoutedEventArgs e)
      {
         int ind = -1;
         if (pilihU == 1)
         {
            F.MasukPass(textusername.Text,passwordBox3.Password,passwordBox1.Password,passwordBox2.Password);
            ind = dataGridU.Items.Count;
         }

         else if (pilihU == 2)
         {
            F.GantiPass(textusername.Text, passwordBox3.Password, passwordBox1.Password, passwordBox2.Password);
            ind = dataGridU.SelectedIndex;
         }

         {
            status_boxusr(false);
            tombol_eusr(true);
            bolehU = true;
            pilihU = 0;
            F.dataview(F.load_data("PENGGUNA"), dataGridU);
            dataGridU.SelectedIndex = ind;
         }
      }

      private void bersihuser_Click(object sender, RoutedEventArgs e)
      {
         clear_boxusr();
      }

      private void bataluser_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridU.SelectedIndex;
         F.dataview(F.load_data("PENGGUNA"), dataGridU);
         status_boxusr(false);
         tombol_eusr(true);
         bolehU = true;
         pilihU = 0;
         textusername.Text = "";
         dataGridU.SelectedIndex = ind;
      }

      private void tambahuser_Click(object sender, RoutedEventArgs e)
      {
         bolehU = false;
         dataGridU.UnselectAll();
         clear_boxusr();
         status_boxusr(true);
         tombol_eusr(false);
         pilihU = 1;
         passwordBox3.IsEnabled = false;
      }

      private void ubahuser_Click(object sender, RoutedEventArgs e)
      {
         if (textusername.Text != "")
         {
            bolehU = false;
            status_boxusr(true);
            tombol_eusr(false);
            pilihU = 2;
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);

      }

      private void hapususer_Click(object sender, RoutedEventArgs e)
      {
         if (textusername.Text != "")
         {
            if (MessageBox.Show("Hapus entry \"" + textusername.Text + "\" dari tabel ?", "Delete " + textusername.Text, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
               string sql = "delete from PENGGUNA where USERNAME = '" + textusername.Text + "'";
               if (F.Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               F.dataview(F.load_data("PENGGUNA"), dataGridU);
               dataGridU.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);

      }

      private void dataGridU_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (!bolehU)
            dataGridU.UnselectAll();
      }

      private void refreshbtnusr_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridU.SelectedIndex;
         F.dataview(F.load_data("PENGGUNA"), dataGridU);
         dataGridU.SelectedIndex = ind;
      }


   }
}
