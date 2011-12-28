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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace TCHusada
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      OracleConnection cn = new OracleConnection("Data Source=(DESCRIPTION="
             + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521)))"
             + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));"
             + "User Id=basdat; Password=basdat;");

      DispatcherTimer timer;
      public MainWindow()
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
      }

      public void masuk()
      {
         //login
         if (!(txtUserName.Text == "" || passwordBox1.Password == ""))
         {
            if (checkPass(txtUserName.Text, passwordBox1.Password))
            {
               siapa.anda = txtUserName.Text;//simpan siapa yang login
               //MessageBox.Show("Berhasil masuk");
               string pilih = txtUserName.Text;
               switch (pilih[0])
               {
                  case '5':
                     switch (pilih[1])
                     {
                        case '3':
                           //dokter
                           MessageBox.Show("Dokter");
                           Dokter d = new Dokter();
                           d.Show();
                           this.Hide();
                           break;
                        case '1':
                           //perawat
                           MessageBox.Show("Perawat");
                           Perawat p = new Perawat();
                           p.Show();
                           this.Hide();
                           break;
                        case '4':
                           //petugas
                           MessageBox.Show("Karyawan");
                           Karyawan k = new Karyawan();
                           k.Show();
                           this.Hide();
                           break;
                     }
                     break;
                  case '2':
                     //Pasien
                     MessageBox.Show("Pasien");
                     break;
                  default:
                     break;
               }
            }
            else MessageBox.Show("Username dan Password tidak cocok", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
         }
         else if (txtUserName.Text == "" && !(passwordBox1.Password == ""))
            MessageBox.Show("username kosong");
         else if (passwordBox1.Password == "" && !(txtUserName.Text == ""))
            MessageBox.Show("password kosong");
         else MessageBox.Show("username & password kosong");
      }

      //fungsi pass enkripsi
      public void addPass(string user, string pass)
      {
         string sql = "insert into pengguna values ("
                          + "'" + user + "',"
                          + "'" + HashCode(pass) + "')";
         if (Execute(sql))
            MessageBox.Show("Data telah ditambahkan");

         //file
         /*
         if (!checkPass(txtUserName.Text, passwordBox1.Password)) //cek udah ada belum
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"usrpss.db", true))
            {
               file.WriteLine(HashCode(user + pass));
            }
          */
      }

      public bool checkPass(string user, string pass)
      {
         //db
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

         //file
         /*
         string[] lines = System.IO.File.ReadAllLines(@"usrpss.db");
         foreach (string line in lines)
         {
            if (HashCode(user + pass) == line)
               return true;
         }
         return false;
         */
      }

      // disimpan dalam bentuk hashcode
      public static string HashCode(string str)
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
         if (cn != null)
         {
            affected = ExecuteNonQuery(sql);
            cn.Close();
            if (affected < 1) return false;
            return true;
         }
         return false;
      }

      protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
      {
         base.OnMouseLeftButtonDown(e);
         try
         {
            DragMove();
         }
         catch
         {}
      }

      private void FormFadeOut_Completed(object sender, EventArgs e)
      {
         Application.Current.Shutdown();
      }

      private void label2_MouseDown(object sender, MouseButtonEventArgs e)
      {
         //Application.Current.Shutdown();
         FormFadeOut.Begin();
      }

      private void image1_MouseDown(object sender, MouseButtonEventArgs e)
      {
         //Application.Current.Shutdown();
         FormFadeOut.Begin();
      }

      private void label5_MouseDown(object sender, MouseButtonEventArgs e)
      {
         //login gambar
            masuk();
      }

      private void txtUserName_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
            masuk();
      }

      private void passwordBox1_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
            masuk();
      }

      private void txtUserName_GotFocus(object sender, RoutedEventArgs e)
      {
         txtUserName.SelectAll();
      }

      private void txtUserName_GotMouseCapture(object sender, MouseEventArgs e)
      {
         txtUserName.SelectAll();
      }

      private void passwordBox1_GotFocus(object sender, RoutedEventArgs e)
      {
         passwordBox1.SelectAll();
      }

      private void passwordBox1_GotMouseCapture(object sender, MouseEventArgs e)
      {
         passwordBox1.SelectAll();
      }

      private void button1_Click(object sender, RoutedEventArgs e)
      {
         addPass(txtUserName.Text, passwordBox1.Password);
         MessageBox.Show("masuk");
      }

      private void label1_MouseDown(object sender, MouseButtonEventArgs e)
      {
         TimeSpan elapsedTime = new TimeSpan( 0, 0, 0, 5, 0);
         SplashScreen screen = new SplashScreen("Splash.png");
         screen.Show(autoClose: true, topMost: true);
         screen.Close(elapsedTime);
      }

      private void button2_Click(object sender, RoutedEventArgs e) //debugging
      {
         Karyawan k = new Karyawan();
         k.Show();
         this.Hide();
      }


   }


}
