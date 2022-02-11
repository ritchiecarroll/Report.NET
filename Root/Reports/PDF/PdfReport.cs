#if Framework2
using System;

// Creation date: 19.01.2006
// Checked: 07.02.2006
// Author: Otto Mayer (mot@root.ch)
// Version: 1.05

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>PDF-Report</summary>
  /// <typeparam name="ReportClass">Report-Class that will be used to create the PDF report.</typeparam>
  /// <remarks>This class can be used to create, view and print the PDF report.</remarks>
  public sealed class PdfReport<ReportClass> : PdfFormatter where ReportClass : Report, new() {
    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Shows the specified PDF document in a maximized window after that it has been created.</summary>
    /// <param name="sFileName">File name of the new PDF document</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    /// <remarks>
    /// This method will create the specified PDF document.
    /// If the file name is relative, the file will be created in the current user's temporary folder.
    /// If it exists, the name of the file will be made unique with a time stamp.
    /// If the specified file name is absolute, it will be overwritten if it exists.
    /// After that the document will be displayed in the acrobat reader in a maximized window.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyReport : Report {
    ///   public static void Main() {
    ///     PdfReport<MyReport> pdfReport = new PdfReport<MyReport>();
    ///     pdfReport.pageLayout = PageLayout.TwoColumnLeft;
    ///     pdfReport.View("MyReport.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///     FontProp fp = new FontPropMM(fd, 20);
    ///     new Page(this);
    ///     page_Cur.AddCB_MM(80, new RepString(fp, "My Report"));
    ///   }
    /// }
    /// </code>
    /// </example>
    public void View(String sFileName) {
      ReportClass report = new ReportClass();
      report.formatter = this;
      RT.ViewPDF(report, sFileName);
    }

    //------------------------------------------------------------------------------------------07.02.2006
    #if !NETCOREAPP
    /// <summary>Shows the specified PDF document in a maximized window after that it has been created.</summary>
    /// <param name="sFileName">File name of the new PDF document</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    /// <remarks>
    /// This method will create the specified PDF document.
    /// If the file name is relative, the file will be created in the current user's temporary folder.
    /// If it exists, the name of the file will be made unique with a time stamp.
    /// If the specified file name is absolute, it will be overwritten if it exists.
    /// After that the document will be displayed in the acrobat reader in a maximized window.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyReport : Report {
    ///   public static void Main() {
    ///     PdfReport<MyReport> pdfReport = new PdfReport<MyReport>();
    ///     pdfReport.pageLayout = PageLayout.TwoColumnLeft;
    ///     pdfReport.View("MyReport.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///     FontProp fp = new FontPropMM(fd, 20);
    ///     new Page(this);
    ///     page_Cur.AddCB_MM(80, new RepString(fp, "My Report"));
    ///   }
    /// }
    /// </code>
    /// </example>
    public void Response(System.Web.UI.Page page) {
      ReportClass report = new ReportClass();
      report.formatter = this;
      RT.ResponsePDF(report, page);
    }
    #endif
    }
}
#endif
