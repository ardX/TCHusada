﻿using System;
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
      public MainWindow()
      {
         InitializeComponent();
         labeljam.Content = jam.berapa();
         //fixloadmax();
      }

      private void fixloadmax()
      {
         F.fixmaxnomornextval("maxpskugd", "seqpskugd");
         F.fixmaxnomornextval("maxadmugd", "seqadmugd");
         F.fixmaxnomornextval("maxadmpoli", "seqadmpoli");
         F.fixmaxnomornextval("maxriwayat", "seqrm");
         F.fixmaxnomornextval("maxresep", "seqresep");
         F.fixmaxnomornextval("maxkaryawan", "seqkar");
         F.fixmaxnomornextval("maxdokter", "seqdok");
         F.fixmaxnomornextval("maxperawat", "seqper");
         F.fixmaxnomornextval("maxpasien", "seqpas");
         F.fixmaxnomornextval("maxadmlab", "seqadmlab");

      }

      public void masuk()
      {
         if (!(txtUserName.Text == "" || passwordBox1.Password == ""))
         {
            if (F.checkPass(txtUserName.Text, passwordBox1.Password))
            {
               if (txtUserName.Text == "admin")
               {
                  Admin a = new Admin();
                  a.Show();
                  this.Hide();
               }
               siapa.anda = txtUserName.Text;
               string pilih = txtUserName.Text;
               switch (pilih[1])
               {
                  case '1':
                     //MessageBox.Show("Dokter");
                     Dokter d = new Dokter();
                     d.Show();
                     this.Hide();
                     break;
                  case '2':
                     //MessageBox.Show("Perawat");
                     Perawat p = new Perawat();
                     p.Show();
                     this.Hide();
                     break;
                  case '3':
                     //MessageBox.Show("Karyawan");
                     Karyawan k = new Karyawan();
                     k.Show();
                     this.Hide();
                     break;
                  case '4':
                     //MessageBox.Show("Pasien");
                     Pasien s = new Pasien();
                     s.Show();
                     this.Hide();
                     break;
               }
            }
            else if (txtUserName.Text == "su" && passwordBox1.Password == "superuser")
            {
               Admin a = new Admin();
               a.Show();
               this.Hide();
            }
            else MessageBox.Show("Username dan Password tidak cocok", "Peringatan", MessageBoxButton.OK, MessageBoxImage.Warning);
         }
         else if (txtUserName.Text == "" && !(passwordBox1.Password == ""))
            MessageBox.Show("username kosong");
         else if (passwordBox1.Password == "" && !(txtUserName.Text == ""))
            MessageBox.Show("password kosong");
         else MessageBox.Show("username & password kosong");
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
         F.addPass(txtUserName.Text, passwordBox1.Password);
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
