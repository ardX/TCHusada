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
   /// Interaction logic for RujukanUGD.xaml
   /// </summary>
   public partial class RujukanUGD : Window
   {
      private rujukanugd crystal;

      public RujukanUGD()
      {
         InitializeComponent();
         this.MyCrystalReportViewer.BackColor = System.Drawing.Color.AliceBlue;
         crystal = new rujukanugd();
         load_db(crystal);
         
      }

      private void load_db(rujukanugd crystal)
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

      private void load_pass(rujukanugd crystal, string val)
      {
         ParameterFieldDefinitions crParameterFieldDefinitions;
         ParameterFieldDefinition crParameterFieldDefinition;
         ParameterValues crParameterValues = new ParameterValues();
         ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

         crParameterDiscreteValue.Value = val;
         crParameterFieldDefinitions = crystal.DataDefinition.ParameterFields;
         crParameterFieldDefinition = crParameterFieldDefinitions["nopass"];
         crParameterValues = crParameterFieldDefinition.CurrentValues;

         crParameterValues.Clear();
         crParameterValues.Add(crParameterDiscreteValue);
         crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
      }

      private void load_view(rujukanugd crystal)
      {
         MyCrystalReportViewer.ReportSource = crystal;
         MyCrystalReportViewer.Refresh();
      }

      private void button1_Click(object sender, RoutedEventArgs e)
      {
         load_pass(crystal, textBox1.Text);//variabel yang di passing
         load_view(crystal);
      }

   }
}
