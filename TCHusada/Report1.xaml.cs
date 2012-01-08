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
      private CrystalReportPass crystal;
      
      public Report1()
      {
         InitializeComponent();
         this.MyCrystalReportViewer.BackColor = System.Drawing.Color.AliceBlue;
         crystal = new CrystalReportPass();
         load_db(crystal);
         load_pass(crystal,"66003");//variabel yang di passing
         load_view(crystal);
      }

      private void load_db(CrystalReportPass crystal)
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

      private void load_pass(CrystalReportPass crystal, string val)
      {
         ParameterFieldDefinitions crParameterFieldDefinitions;
         ParameterFieldDefinition crParameterFieldDefinition;
         ParameterValues crParameterValues = new ParameterValues();
         ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();

         crParameterDiscreteValue.Value = val;
         crParameterFieldDefinitions = crystal.DataDefinition.ParameterFields;
         crParameterFieldDefinition = crParameterFieldDefinitions["idresep"];
         crParameterValues = crParameterFieldDefinition.CurrentValues;

         crParameterValues.Clear();
         crParameterValues.Add(crParameterDiscreteValue);
         crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
      }

      private void load_view(CrystalReportPass crystal)
      {
         MyCrystalReportViewer.ReportSource = crystal;
         MyCrystalReportViewer.Refresh();
      }
     
   }
}
