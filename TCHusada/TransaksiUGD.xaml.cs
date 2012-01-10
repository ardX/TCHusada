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
   /// Interaction logic for TransaksiUGD.xaml
   /// </summary>
   public partial class TransaksiUGD : Window
   {
      private transaksiugd crystal;

      public TransaksiUGD()
      {
         InitializeComponent();
         this.MyCrystalReportViewer.BackColor = System.Drawing.Color.AliceBlue;
         crystal = new transaksiugd();
         load_db(crystal);

      }

      private void load_db(transaksiugd crystal)
      {
         TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
         ConnectionInfo crConnectionInfo = new ConnectionInfo();
         Tables CrTables;

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
      }

      private void load_pass(transaksiugd crystal, string val)
      {
         ParameterFieldDefinitions crParameterFieldDefinitions;
         ParameterFieldDefinition crParameterFieldDefinition;
         ParameterValues crParameterValues = new ParameterValues();
         ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

         crParameterDiscreteValue.Value = val;
         crParameterFieldDefinitions = crystal.DataDefinition.ParameterFields;
         crParameterFieldDefinition = crParameterFieldDefinitions["pasien"];
         crParameterValues = crParameterFieldDefinition.CurrentValues;

         crParameterValues.Clear();
         crParameterValues.Add(crParameterDiscreteValue);
         crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
      }

      private void load_view(transaksiugd crystal)
      {
         MyCrystalReportViewer.ReportSource = crystal;
         MyCrystalReportViewer.Refresh();
      }

      private void button1_Click(object sender, RoutedEventArgs e)
      {
         load_pass(crystal, textBox1.Text);//variabel yang di passing
         load_pass(crystal, siapa.anda);//variabel yang di passing
         load_view(crystal);
      }

   }
}
