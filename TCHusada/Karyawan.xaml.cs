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

         timer = new DispatcherTimer();
         timer.Interval = TimeSpan.FromSeconds(1.0);
         timer.Start();
         timer.Tick += new EventHandler(delegate(object s, EventArgs a)
         {
            label6.Content = "" + DateTime.Now.Hour.ToString("d2") + ":"
            + DateTime.Now.Minute.ToString("d2");
         });
         load_kabeh();
        
      }

      public void load_kabeh()
      {
         dataview(load_data("karyawan"), dataGridK);
         status_boxk(false);
         tombol_ek(true);

         dataview(load_data("dokter"), dataGridD);
         status_boxd(false);
         tombol_ed(true);

         dataview(load_data("perawat"), dataGridP);
         status_boxp(false);
         tombol_ep(true);

         dataview(load_data("pasien"), dataGridS);
         status_boxs(false);
         tombol_es(true);

         load_saya();
         status_boxtt(false);
         tombol_ett(true);
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

      private void dataview(DataSet sd, DataGrid dg)//fungsi modif global
      {
         dg.ItemsSource = sd.Tables[0].DefaultView;
         dg.ColumnWidth = DataGridLength.Auto;
      }

      private string getmaxnomor(string kolom, string tabel)
      {
         string metu = "";
         cn.Open();
         string sql = "select max(" + kolom + ") from " + tabel;
         OracleDataReader reader;
         OracleCommand cmd = new OracleCommand(sql, cn);
         reader = cmd.ExecuteReader();
         if (reader.Read())
         {
            metu = reader.GetString(0);
         }
         ulong metuu = UInt64.Parse(metu) + 1;
         metu = metuu+"";
         reader.Close();
         cn.Close();
         return metu;
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
            if (Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridK.Items.Count;
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
            bolehK = true;
            pilihK = 0;
            dataview(load_data("karyawan"), dataGridK);
            dataGridK.SelectedIndex = ind;
         }
      }

      private void bersihbtnk_Click(object sender, RoutedEventArgs e)
      {
         clear_boxk();
         bolehK = true;
      }

      private void batalbtnk_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridK.SelectedIndex;
         dataview(load_data("karyawan"), dataGridK);
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
         textnipk.Text = getmaxnomor("NIP_KARYAWAN", "KARYAWAN"); textnipk.IsEnabled = false;
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
               if (Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               dataview(load_data("karyawan"), dataGridK);
               dataGridK.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtnk_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridK.SelectedIndex;
         dataview(load_data("karyawan"), dataGridK);
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
         textkeld.IsEnabled = status;
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
         textkeld.Text = "";
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
                          + "'" + textkeld.Text + "',"
                          + "'" + texttelpd.Text + "')";
            if (Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridD.Items.Count;
            }
            else error = true;

         }

         else if (pilihD == 2)
         {
            error = false;
            string sql = "update DOKTER set "
                         + " NAMA_DOKTER = '" + textnamd.Text + "',"
                         + " ALAMAT_DOKTER= '" + textalmtd.Text + "',"
                         + " JENISKELAMIN_DOKTER = '" + textkeld.Text + "',"
                         + " TELP_DOKTER = '" + texttelpd.Text + "'"
                         + " where NIP_DOKTER = '" + textnipd.Text + "'";
            if (Execute(sql))
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
            dataview(load_data("dokter"), dataGridD);
            dataGridD.SelectedIndex = ind;
         }
      }

      private void bersihbtnd_Click(object sender, RoutedEventArgs e)
      {
         clear_boxd();
         bolehD = true;
      }

      private void batalbtnd_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridD.SelectedIndex;
         dataview(load_data("dokter"), dataGridD);
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
         textnipd.Text = getmaxnomor("NIP_DOKTER", "DOKTER"); textnipd.IsEnabled = false;
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
               if (Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               dataview(load_data("dokter"), dataGridD);
               dataGridD.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtnd_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridD.SelectedIndex;
         dataview(load_data("dokter"), dataGridD);
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
         textkelp.IsEnabled = status;
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
         textkelp.Text = "";
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
                          + "'" + textkelp.Text + "',"
                          + "'" + texttelpp.Text + "')";
            if (Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridP.Items.Count;
            }
            else error = true;

         }

         else if (pilihP == 2)
         {
            error = false;
            string sql = "update PERAWAT set "
                         + " NAMA_PERAWAT = '" + textnamp.Text + "',"
                         + " ALAMAT_PERAWAT= '" + textalmtp.Text + "',"
                         + " JENISKELAMIN_PERAWAT = '" + textkelp.Text + "',"
                         + " TELP_PERAWAT = '" + texttelpp.Text + "'"
                         + " where NIP_PERAWAT = '" + textnipp.Text + "'";
            if (Execute(sql))
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
            dataview(load_data("PERAWAT"), dataGridP);
            dataGridP.SelectedIndex = ind;
         }
      }

      private void bersihbtnp_Click(object sender, RoutedEventArgs e)
      {
         clear_boxp();
         bolehP = true;
      }

      private void batalbtnp_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridP.SelectedIndex;
         dataview(load_data("PERAWAT"), dataGridP);
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
         textnipp.Text = getmaxnomor("NIP_PERAWAT", "PERAWAT"); textnipp.IsEnabled = false;
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
               if (Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               dataview(load_data("PERAWAT"), dataGridP);
               dataGridP.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtnp_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridP.SelectedIndex;
         dataview(load_data("PERAWAT"), dataGridP);
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
         textkels.IsEnabled = status;
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
         textkels.Text = "";
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
                          + "'" + textkels.Text + "',"
                    + "'" + texttelps.Text + "',"
                          + "'" + datelhrs.Text + "')";
            if (Execute(sql))
            {
               MessageBox.Show("Data telah ditambahkan");
               ind = dataGridS.Items.Count;
            }
            else error = true;

         }

         else if (pilihS == 2)
         {
            error = false;
            string sql = "update PASIEN set "
                         + " NAMA_PASIEN = '" + textnams.Text + "',"
                         + " ALAMAT_PASIEN= '" + textalmts.Text + "',"
                         + " KELAMIN_PASIEN = '" + textkels.Text + "',"
                   + " TELP_PASIEN = '" + texttelps.Text + "',"
                         + " LAHIR_PASIEN = '" + datelhrs.Text + "'"
                         + " where NO_PASIEN = '" + textnos.Text + "'";
            if (Execute(sql))
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
            dataview(load_data("PASIEN"), dataGridS);
            dataGridS.SelectedIndex = ind;
         }
      }

      private void bersihbtns_Click(object sender, RoutedEventArgs e)
      {
         clear_boxs();
         bolehS = true;
      }

      private void batalbtns_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridS.SelectedIndex;
         dataview(load_data("PASIEN"), dataGridS);
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
         textnos.Text = getmaxnomor("NO_PASIEN", "PASIEN"); textnos.IsEnabled = false;
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
               if (Execute(sql))
                  MessageBox.Show("Data telah dihapus");
               dataview(load_data("PASIEN"), dataGridS);
               dataGridS.UnselectAll();
            }
         }
         else MessageBox.Show("Tidak ada data yang dipilih", "Perhatian", MessageBoxButton.OK, MessageBoxImage.Warning);
      }

      private void refreshbtns_Click(object sender, RoutedEventArgs e)
      {
         int ind = dataGridS.SelectedIndex;
         dataview(load_data("PASIEN"), dataGridS);
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
         cn.Open();
         string sql = "select * from karyawan where NIP_KARYAWAN = '" + siapa.anda + "'";
         OracleDataReader reader;
         OracleCommand cmd = new OracleCommand(sql, cn);
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
         cn.Close();
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
         if (Execute(sql))
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

      
   }
}
