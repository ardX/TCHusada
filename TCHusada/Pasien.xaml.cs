using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
      DispatcherTimer timer;
      public Pasien()
      {
         InitializeComponent();
         timer = new DispatcherTimer();
         timer.Interval = TimeSpan.FromSeconds(1.0);
         timer.Start();
         timer.Tick += new EventHandler(delegate(object s, EventArgs a)
         {
            label6.Content = "" + DateTime.Now.Hour.ToString("d2") + ":"
            + DateTime.Now.Minute.ToString("d2");// +":"
            // + DateTime.Now.Second;
         });
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
         //Close();
         Application.Current.Shutdown();
      }
   }
}
