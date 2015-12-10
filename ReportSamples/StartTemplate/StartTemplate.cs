using Root.Reports;
using System;

// Creation date: 05.08.2002
// Checked: 11.03.2006
// Author: Otto Mayer (mot@root.ch)
// Version: 1.05

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace ReportSamples {
  #region Class Documentation
  /// <summary>Start Template (C# Version)</summary>
  /// <remarks>
  /// This sample code can be used as a start template.
  /// It is recommended that your report class (e.g. MyReport) has the following structure:<br></br>
  /// - class MyReport derived from class <see cref="Root.Reports.Report"/><br></br>
  /// - overridden method <see cref="StartTemplate.Create"/> that creates the contents of the report
  /// </remarks>
  /// <example>C#, Framework 2.0
  /// <code>
  /// public class StartTemplate : <b>Report</b> {
  ///   public static void Main() {
  ///     <b>PdfReport&lt;StartTemplate&gt; pdfReport = new PdfReport&lt;StartTemplate&gt;();</b>
  ///     pdfReport.pageLayout = PageLayout.SinglePage;
  ///     pdfReport.View("StartTemplate.pdf");
  ///   }
  /// 
  ///   protected override void <b>Create()</b> {
  ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
  ///     FontProp fontProp = new FontPropMM(fontDef, 20);
  ///     new Page(this);
  ///     page_Cur.AddCB_MM(120, new RepString(fontProp, "Start Template"));
  ///   }
  /// }
  /// </code>
  /// </example>
  /// <example>C#, Framework 1.0 / 1.1
  /// <code>
  /// public class StartTemplate : <b>Report</b> {
  ///   public static void Main() {
  ///     <b>RT.ViewPDF(new StartTemplate(), "StartTemplate.pdf");</b>
  ///   }
  /// 
  ///   protected override void <b>Create()</b> {
  ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
  ///     FontProp fontProp = new FontPropMM(fontDef, 20);
  ///     new Page(this);
  ///     page_Cur.AddCB_MM(120, new RepString(fontProp, "Start Template"));
  ///   }
  /// }
  /// </code>
  /// </example>
  /// <example>Visual Basic, Framework 2.0
  /// <code>
  /// Namespace ReportSamples
  ///   Module StartTemplateVB
  ///     Sub Main()
  ///       <b>Dim report As PdfReport(Of StartTemplate) = New PdfReport(Of StartTemplate)</b>
  ///       report.pageLayout = PageLayout.SinglePage
  ///       report.View("StartTemplateVB.pdf")
  ///     End Sub
  ///   End Module
  /// 
  ///   Class StartTemplate
  ///     Inherits <b>Report</b>
  /// 
  ///     Protected Overrides Sub <b>Create()</b>
  ///       Dim fd As FontDef = New FontDef(Me, FontDef.StandardFont.Helvetica)
  ///       Dim fp As FontProp = New FontPropMM(fd, 20)
  ///       Dim page As page = New page(Me)
  ///       page.AddCB_MM(80, New RepString(fp, "Start Template"))
  ///     End Sub
  ///   End Class
  /// End Namespace
  /// </code>
  /// </example>
  /// <example>Visual Basic, Framework 1.0 / 1.1
  /// <code>
  /// Namespace ReportSamples
  ///   Module StartTemplateVB
  ///     Sub Main()
  ///       <b>RT.ViewPDF(New StartTemplate, "StartTemplateVB.pdf")</b>
  ///     End Sub
  ///   End Module
  /// 
  ///   Class StartTemplate
  ///     Inherits <b>Report</b>
  /// 
  ///     Protected Overrides Sub <b>Create()</b>
  ///       Dim fd As FontDef = New FontDef(Me, FontDef.StandardFont.Helvetica)
  ///       Dim fp As FontProp = New FontPropMM(fd, 20)
  ///       Dim page As page = New page(Me)
  ///       page.AddCB_MM(80, New RepString(fp, "Start Template"))
  ///     End Sub
  ///   End Class
  /// End Namespace
  /// </code>
  /// </example>
  #endregion
  public class StartTemplate : Report {
    //------------------------------------------------------------------------------------------14.02.2006
    /// <summary>Starts the "Start Template" sample.</summary>
    /// <remarks>This method serves only to start the application.</remarks>
    public static void Main() {
      #if Framework2
      PdfReport<StartTemplate> pdfReport = new PdfReport<StartTemplate>();
      pdfReport.pageLayout = PageLayout.SinglePage;
      pdfReport.View("StartTemplate.pdf");
      #else
      RT.ViewPDF(new StartTemplate(), "StartTemplate.pdf");
      #endif
    }

    //------------------------------------------------------------------------------------------20.03.2004
    /// <summary>Creates this report.</summary>
    /// <remarks>
    /// The method <see cref="Root.Reports.Report.Create"/> of class <see cref="Root.Reports.Report"/> must
    /// be overridden to create the contents of the report.
    /// </remarks>
    protected override void Create() {
      FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
      FontProp fontProp = new FontPropMM(fontDef, 20);
      new Page(this);
      page_Cur.AddCB_MM(120, new RepString(fontProp, "Start Template"));
    }
  }
}
