using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;
using SAPBusinessObjects.WPF.Viewer;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TCHusada
{
   /// <summary>
   /// Interaction logic for Report1.xaml
   /// </summary>
   public partial class Report1 : Window
   {
      private CrystalReportCoba crystal;
      TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
      ConnectionInfo crConnectionInfo = new ConnectionInfo();
      Tables CrTables;
      public Report1()
      {
         InitializeComponent();
         this.MyCrystalReportViewer.BackColor = System.Drawing.Color.AliceBlue;
         crystal = new CrystalReportCoba();

         crConnectionInfo.ServerName = "127.0.0.1";
         crConnectionInfo.DatabaseName = "";
         crConnectionInfo.UserID = "basdat";
         crConnectionInfo.Password = "basdat";

         CrTables = crystal.Database.Tables;
         foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
         {
            crtableLogoninfo = CrTable.LogOnInfo;
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            CrTable.ApplyLogOnInfo(crtableLogoninfo);
         }

         MyCrystalReportViewer.ReportSource = crystal;
         MyCrystalReportViewer.Refresh();
      }
     
   }
}
