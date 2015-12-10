using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;

// Creation date: 02.12.2002
// Checked: 08.03.2005
// Author: Otto Mayer (mot@root.ch)
// Version: 1.03

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Report Tools Class</summary>
  /// <remarks>This class provides general tools for the Report.NET library.</remarks>
  #if Framework2
  static
  #endif
  public class RT {
    //------------------------------------------------------------------------------------------07.03.2005
    #region Static
    //----------------------------------------------------------------------------------------------------

    /// <summary>Resource manager</summary>
    private static ResourceManager rm = new ResourceManager(typeof(RT));

    /// <summary>Paper size A0 width in millimeters</summary>
    /// <remarks>Width of a A0 page in millimeters: width = 2 ^ -0.25 m</remarks>
    public const Double rA0_WidthMM = 840.89641525371454303112547623321;

    /// <summary>Paper size A0 height in millimeters</summary>
    /// <remarks>Height of a A0 page in millimeters: height = 2 ^ 0.25 m</remarks>
    public const Double rA0_HeightMM = 1189.2071150027210667174999705605;

    /// <summary>Paper size A1 width in millimeters</summary>
    /// <remarks>Width of a A1 page in millimeters.</remarks>
    public const Double rA1_WidthMM = rA0_HeightMM / 2;

    /// <summary>Paper size A1 height in millimeters</summary>
    /// <remarks>Height of a A1 page in millimeters.</remarks>
    public const Double rA1_HeightMM = rA0_WidthMM;

    /// <summary>Paper size A2 width in millimeters</summary>
    /// <remarks>Width of a A2 page in millimeters.</remarks>
    public const Double rA2_WidthMM = rA1_HeightMM / 2;

    /// <summary>Paper size A2 height in millimeters</summary>
    /// <remarks>Height of a A2 page in millimeters.</remarks>
    public const Double rA2_HeightMM = rA1_WidthMM;

    /// <summary>Paper size A3 width in millimeters</summary>
    /// <remarks>Width of a A3 page in millimeters.</remarks>
    public const Double rA3_WidthMM = rA2_HeightMM / 2;

    /// <summary>Paper size A3 height in millimeters</summary>
    /// <remarks>Height of a A3 page in millimeters.</remarks>
    public const Double rA3_HeightMM = rA2_WidthMM;

    /// <summary>Paper size A4 width in millimeters</summary>
    /// <remarks>Width of a A4 page in millimeters.</remarks>
    public const Double rA4_WidthMM = rA3_HeightMM / 2;

    /// <summary>Paper size A4 height in millimeters</summary>
    /// <remarks>Height of a A4 page in millimeters.</remarks>
    public const Double rA4_HeightMM = rA3_WidthMM;

    /// <summary>Paper size A5 width in millimeters</summary>
    /// <remarks>Width of a A5 page in millimeters.</remarks>
    public const Double rA5_WidthMM = rA4_HeightMM / 2;

    /// <summary>Paper size A5 height in millimeters</summary>
    /// <remarks>Height of a A5 page in millimeters.</remarks>
    public const Double rA5_HeightMM = rA4_WidthMM;

    /// <summary>Paper size A6 width in millimeters</summary>
    /// <remarks>Width of a A6 page in millimeters.</remarks>
    public const Double rA6_WidthMM = rA5_HeightMM / 2;

    /// <summary>Paper size A6 height in millimeters</summary>
    /// <remarks>Height of a A6 page in millimeters.</remarks>
    public const Double rA6_HeightMM = rA5_WidthMM;

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Sets the number format for PDF values.</summary>
    static RT() {
      cultureInfo_PDF.NumberFormat.NumberDecimalSeparator = ".";
      #if (DEBUG)
      Double r = 1.2345;
      Debug.Assert(r.ToString(sPdfNumberFormat, cultureInfo_PDF) == "1.235");
      r = 0.5;
      Debug.Assert(r.ToString(sPdfNumberFormat, cultureInfo_PDF) == "0.5");
      #endif
    }

    //------------------------------------------------------------------------------------------07.03.2005
    #if Framework
    /// <summary>Instances of this class are not allowed.</summary>
    private RT() {
    }
    #endif

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Determines whether the specified numbers are considered equal.</summary>
    /// <param name="r1">First number to compare</param>
    /// <param name="r2">Second number to compare</param>
    /// <param name="rTolerance">Tolerance</param>
    /// <returns>
    /// <see langword="true"/> if r1 == r2 or if both numbers are <see cref="System.Double.NaN"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This method compares to double values.
    /// They are considered equal if the differenc between the to values is less than the tolerance value.
    /// </remarks>
    public static Boolean bEquals(Double r1, Double r2, Double rTolerance) {
      if (Double.IsNaN(r1)) {
        return Double.IsNaN(r2);
      }
      if (Double.IsNaN(r2)) {
        return false;
      }
      return (Math.Abs(r1 - r2) < rTolerance);
    }
    #endregion

    //------------------------------------------------------------------------------------------07.03.2005
    #region Conversion
    //----------------------------------------------------------------------------------------------------

    /// <summary>Conversion factor: millimeter to point</summary>
    private const Double rMMToPoint = 1.0 / 25.4 * 72.0;

    /// <summary>Conversion factor: point to millimeter</summary>
    private const Double rPointToMM = 1.0 / 72.0 * 25.4;

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Converts millimeters to points (1/72 inch).</summary>
    /// <param name="rMM">Value in millimeters</param>
    /// <returns>value in points (1/72 inch)</returns>
    /// <remarks>This method converts a millimeter value to points.</remarks>
    public static Double rPointFromMM(Double rMM) {
      return rMM * rMMToPoint;
    }

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Converts points (1/72 inch) to millimeters.</summary>
    /// <param name="rPoint">Value in points (1/72 inch)</param>
    /// <returns>value in millimeters</returns>
    /// <remarks>This method converts a point value to millimeters.</remarks>
    public static Double rMMFromPoint(Double rPoint) {
      return rPoint * rPointToMM;
    }

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Converts degrees to radians.</summary>
    /// <param name="rDegree">Value in degrees</param>
    /// <returns>value in radians</returns>
    internal static Double rRadianFromDegree(Double rDegree) {
      Double r = Math.Floor(rDegree / 360.0) * 360.0;  // normalize angle
      rDegree = rDegree - r;
      return rDegree / 180.0 * Math.PI;
    }
    #endregion

    //------------------------------------------------------------------------------------------07.03.2005
    #region PDF
    //----------------------------------------------------------------------------------------------------

    /// <summary>Culture info for formatting PDF values</summary>
    private static CultureInfo cultureInfo_PDF = new System.Globalization.CultureInfo("en-US");

    /// <summary>Number format string for PDF dimensions</summary>
    private const String sPdfNumberFormat = "0.###";

    /// <summary>Registry key of the acrobat reader version 5, 6 and 7.</summary>
    private const String sRegKey_AcrobatReader = @"Software\Adobe\Acrobat\Exe";

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Converts a dimension value to the PDF value format.</summary>
    /// <param name="rDim">Dimension value</param>
    /// <returns>Dimension value in the PDF value format</returns>
    internal static String sPdfDim(Double rDim) {
      return rDim.ToString(sPdfNumberFormat, cultureInfo_PDF);
    }

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>StringBuilder object for use in "sPdfString"</summary>
    private static StringBuilder sb = new StringBuilder(200);

    /// <summary>Converts a string to the PDF text format.</summary>
    /// <param name="sText">String to convert to the PDF text format</param>
    /// <returns>String in the PDF text format</returns>
    internal static String sPdfString(String sText) {
      lock (sb) {
        sb.Length = 0;
        sb.Append('(');
        for (Int32 i = 0;  i < sText.Length;  i++) {
          Char c = sText[i];
          if ((Int32)c == 8364) {
            sb.Append("\\200");
            continue;
          }
          if (c == '(' || c == ')' || c == '\\') {
            sb.Append('\\');
          }
          sb.Append(c);
        }
        sb.Append(')');
        return sb.ToString();
      }
    }

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Starts the acrobat reader.</summary>
    /// <param name="sFileName">File name of the PDF document</param>
    /// <param name="sArguments">Arguments</param>
    /// <param name="processWindowStyle">Windows style</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    #if !WindowsCE 
    private static void StartAcro(String sFileName, String sArguments, ProcessWindowStyle processWindowStyle) {
      RegistryKey registryKey_Acro = null; 
      try {
        registryKey_Acro = Registry.ClassesRoot.OpenSubKey(sRegKey_AcrobatReader, false);
      }
      catch (SystemException ex) {
        throw new ReportException(rm.GetString("e_AcroRegistryInvalid"), ex);
      }
      if (registryKey_Acro == null) {
        throw new ReportException(rm.GetString("e_AcroNotInstalled"));
      }
      String sAcroPath = (String)registryKey_Acro.GetValue("");
      if (sAcroPath == null || sAcroPath == "") {
        throw new ReportException(rm.GetString("e_AcroRegistryInvalid"));
      }
      sAcroPath = sAcroPath.Replace("\"", "");

      try {
        Process process = new Process();
        process.StartInfo.FileName = sAcroPath;
        if (sArguments == null) {
          process.StartInfo.Arguments = sFileName;
        }
        else {
          process.StartInfo.Arguments = sArguments + " " + sFileName;
        }
        process.StartInfo.WindowStyle = processWindowStyle;
        process.Start();
      }
      catch (Exception ex) {
        throw new ReportException(String.Format(rm.GetString("e_AcroStartProcess"), sFileName, sAcroPath), ex);
      }
    }
    #endif

    //------------------------------------------------------------------------------------------07.03.2005
    /// <overloads>
    /// <summary>Shows the PDF document in the acrobat reader.</summary>
    /// <remarks>These methods will show the specified PDF document.</remarks>
    /// </overloads>
    /// 
    /// <summary>Shows the specified PDF document in the acrobat reader in a maximized window.</summary>
    /// <param name="sFileName">File name of the PDF document</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    /// <remarks>This method will show the specified PDF document in the acrobat reader in a maximized window.</remarks>
    /// <example>
    /// <code>
    /// RT.ViewPDF("DetailReport.pdf");
    /// </code>
    /// </example>
    #if !WindowsCE
    public static void ViewPDF(String sFileName) {
      StartAcro(sFileName, null, ProcessWindowStyle.Maximized);
    }
    #endif

    //------------------------------------------------------------------------------------------07.03.2005
    #if !WindowsCE
    /// <summary>Shows the specified PDF document in a maximized window after that it has been created.</summary>
    /// <param name="report">Report object that creates the PDF document</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    /// <remarks>
    /// This method will create the specified PDF document.
    /// The resulting file will be stored in the current user's temporary folder.
    /// After that the document will be displayed in the acrobat reader in a maximized window.
    /// </remarks>
    /// <example>
    /// <code>
    /// RT.ViewPDF(new DetailReport());
    /// </code>
    /// </example>
    public static void ViewPDF(Report report) {
      System.Windows.Forms.Form form = System.Windows.Forms.Form.ActiveForm;
      System.Windows.Forms.Cursor cur_Old = System.Windows.Forms.Cursors.Default;
      try {
        if (form != null) {
          cur_Old = form.Cursor;
          form.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        }

        String sFileName = Path.GetTempFileName();
        report.Save(sFileName);
        ViewPDF(sFileName);
      }
      finally {
        if (form != null) {
          form.Cursor = cur_Old;
        }
      }
    }
    #endif

    //------------------------------------------------------------------------------------------07.03.2005
    #if !WindowsCE
    /// <summary>Shows the specified PDF document in a maximized window after that it has been created.</summary>
    /// <param name="report">Report object that creates the PDF document</param>
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
    /// RT.ViewPDF(new DetailReport(), "DetailReport.pdf");
    /// </code>
    /// </example>
    public static void ViewPDF(Report report, String sFileName) {
      if (!Path.IsPathRooted(sFileName)) {
        sFileName = Path.Combine(Path.GetTempPath(), sFileName);
      }
      if (File.Exists(sFileName)) {
        String sDateTime = DateTime.Now.ToString("yyyyMMdd\\_HHmmss");
        String s = Path.GetFileNameWithoutExtension(sFileName) + "_" + sDateTime + Path.GetExtension(sFileName);
        sFileName = Path.Combine(Path.GetDirectoryName(sFileName), s);
      }
      else {
        Directory.CreateDirectory(Path.GetDirectoryName(sFileName));
      }
      report.Save(sFileName);
      ViewPDF(sFileName);
    }
    #endif

    //------------------------------------------------------------------------------------------07.03.2005
    #if !WindowsCE
    /// <overloads>
    /// <summary>Prints the specified PDF document.</summary>
    /// <remarks>These methods will print the specified PDF document.</remarks>
    /// </overloads>
    /// 
    /// <summary>Prints the specified PDF document.</summary>
    /// <param name="sFileName">File name of the PDF document</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    /// <remarks>This method will print the specified PDF document with the acrobat reader.</remarks>
    /// <example>
    /// <code>
    /// RT.PrintPDF("DetailReport.pdf");
    /// </code>
    /// </example>
    public static void PrintPDF(String sFileName) {
      StartAcro(sFileName, "/p /h", ProcessWindowStyle.Hidden);
    }
    #endif

    //------------------------------------------------------------------------------------------07.03.2005
    #if !WindowsCE
    /// <summary>Prints the specified PDF document after that it has been created.</summary>
    /// <param name="report">Report object that creates the PDF document</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    /// <remarks>
    /// This method will create the specified PDF document.
    /// The resulting file will be stored in the current user's temporary folder.
    /// After that the document will be printed with the acrobat reader.
    /// </remarks>
    /// <example>
    /// <code>
    /// RT.PrintPDF(new DetailReport());
    /// </code>
    /// </example>
    public static void PrintPDF(Report report) {
      String sFileName = Path.GetTempFileName();
      report.Save(sFileName);
      PrintPDF(sFileName);
    }
    #endif

    //------------------------------------------------------------------------------------------07.03.2005
    /// <summary>Sends the specified report to the browser.</summary>
    /// <param name="report">Report object that creates the PDF document</param>
    /// <param name="page">Page that has requested this report</param>
    /// <exception cref="ReportException">
    /// The acrobat reader has not been installed, the registry entry is invalid or the reader cannot be started.
    /// </exception>
    /// <remarks>
    /// This method will create the specified PDF document.
    /// The resulting stream will be sent to the browser.
    /// </remarks>
    /// <example>
    /// <code>
    /// &lt;%@ Page language="c#" Debug="true" ContentType="application/pdf" %&gt;
    /// &lt;%@ Import namespace="Root.Reports" %&gt;
    /// &lt;%@ Import namespace="ReportSamples" %&gt;
    /// &lt;script runat="server" language="C#"&gt;
    ///   void Page_Load() {
    ///     RT.ResponsePDF(new TableLayoutManagerSample(), this);
    ///   }
    /// &lt;/script&gt;
    /// </code>
    /// </example>
    #if !WindowsCE
    public static void ResponsePDF(Report report, System.Web.UI.Page page) {
      page.Response.Clear();
      page.Response.ContentType = "application/pdf";

      if (!(report.formatter is PdfFormatter)) {
        throw new ReportException("PDF formatter required");
      }
      if (report.page_Cur == null) {
        report.Create();
      }

      report.formatter.Create(report, page.Response.OutputStream);
      page.Response.End();
    }
    #endif
    #endregion

    //------------------------------------------------------------------------------------------07.03.2005
    #region Obsolete
    //----------------------------------------------------------------------------------------------------

    /// <summary>Conversion factor: millimeter to point</summary>
    [Obsolete("use method: Double rPointFromMM(Double rMM)")]
    public const Double rMM_To_I72 = 1.0 / 25.4 * 72.0;

    /// <summary>Conversion factor: point to millimeter</summary>
    [Obsolete("use method: Double rMMFromPoint(Double rPoint)")]
    public const Double rI72_To_MM = 1.0 / 72.0 * 25.4;

    //------------------------------------------------------------------------------------------28.06.2004
    /// <summary>Converts millimeters to points (1/72 inch).</summary>
    /// <param name="rMM">value in millimeters</param>
    /// <returns>value in points (1/72 inch)</returns>
    [Obsolete("use method: Double rPointFromMM(Double rMM)")]
    public static Double rMM(Double rMM) {
      return rMM * rMM_To_I72;
    }

    //------------------------------------------------------------------------------------------02.08.2004
    #if !WindowsCE 
    /// <summary>Sends the specified report to the browser.</summary>
    /// <param name="report">Report object that creates the PDF document</param>
    /// <param name="response">Response stream</param>
    [Obsolete("use method: ResponsePDF(Report, Page)")]
    public static void ResponsePDF(Report report, System.Web.HttpResponse response) {
      if (report.formatter == null) {
        report.Init(new PdfFormatter());
      }
      if (report.page_Cur == null) {
        report.Create();
      }
      response.ClearContent();
      response.ContentType = "application/pdf";
      MemoryStream ms = new MemoryStream(20000);
      report.formatter.Create(report, ms);
      ms.Close();

      response.BinaryWrite(ms.GetBuffer());
      response.End();
    }
    #endif
    #endregion
  }
}
