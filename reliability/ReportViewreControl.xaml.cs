using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WinForms;

namespace reliability
{
    /// <summary>
    /// Interaction logic for ReportViewreControl.xaml
    /// </summary>
    public partial class ReportViewreControl
    {
        public ReportViewreControl(List<PtResult> ptResults)
        {
            InitializeComponent();
            ReportViewer.LocalReport.ReportEmbeddedResource = "reliability.ResultReport.rdlc";
            ReportViewer.LocalReport.DataSources.Add(new ReportDataSource("PtResult_DS", ptResults));
            //ReportViewer.RefreshReport();

            System.IO.FileInfo fi = new System.IO.FileInfo("ResultExcel.xls");

            if (fi.Exists)
            {
                fi.Delete();
            }

            Warning[] warnings;
            string[] streamids;
            string mimeType, encoding, filenameExtension;
            byte[] bytes = ReportViewer.LocalReport.Render("Excel", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
            System.IO.FileStream fs = System.IO.File.Create("ResultExcel.xls");
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}
