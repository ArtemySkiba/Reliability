using System.Collections.Generic;
using System.IO;
using Microsoft.Reporting.WinForms;

namespace reliability
{
    /// <summary>
    /// Interaction logic for ReportViewreControl.xaml
    /// </summary>
    public partial class ReportViewreControl
    {
        public ReportViewreControl(List<PtResult> ptResults, string fileName)
        {
            InitializeComponent();
            ReportViewer.LocalReport.ReportEmbeddedResource = "reliability.ResultReport.rdlc";
            ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("PtResult_DS", ptResults));
            ReportViewer.RefreshReport();
            if (File.Exists(fileName + ".xls"))
            {
                File.Delete(fileName + ".xls");
            }

            byte[] bytes = ReportViewer.LocalReport.Render("Excel", null);
            FileStream fs = File.Create(fileName + ".xls");
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}
