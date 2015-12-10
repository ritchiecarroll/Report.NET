using Root.Reports;
using System;
using System.Drawing;
using System.Windows.Forms;

// Creation date: 07.02.2006
// Checked: 08.11.2004
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
  /// <summary>Report.NET Sample Application</summary>
  /// <remarks>
  /// This application can be used to start the Report.NET samples.
  /// The samples can be started with the dialog or command line arguments.
  /// The program will show the dialog if no command line arguments are provided.
  /// <p/>-<p/><b>Syntax:</b> Samples.exe [Argument]
  /// <list type="table">
  ///   <listheader>
  ///     <term>Argument</term>
  ///     <description>Description</description>
  ///   </listheader>
  ///   <item>
  ///     <term>FlowLayoutManagerSample</term>
  ///     <description>Flow layout manager sample</description>
  ///   </item>
  ///   <item>
  ///     <term>FontTest</term>
  ///     <description>Font test</description>
  ///   </item>
  ///   <item>
  ///     <term>ImageSample</term>
  ///     <description>Image sample</description>
  ///   </item>
  ///   <item>
  ///     <term>ListLayoutManagerSample</term>
  ///     <description>List layout manager sample</description>
  ///   </item>
  ///   <item>
  ///     <term>PdfPropertiesSample</term>
  ///     <description>PDF properties sample</description>
  ///   </item>
  ///   <item>
  ///     <term>TableLayoutManagerSample</term>
  ///     <description>Table layout manager sample</description>
  ///   </item>
  ///   <item>
  ///     <term>TextSample</term>
  ///     <description>Text sample</description>
  ///   </item>
  /// </list>
  /// </remarks>
  static class Program {
    //------------------------------------------------------------------------------------------08.11.2004
    /// <summary>Starts the dialog window.</summary>
    /// <param name="asArg">Array of argument strings - for a description of the syntax see <see cref="ReportSamples.MainForm"/></param>
    /// <remarks>This method serves only to start the application.</remarks>
    [STAThread]
    static void Main(String[] asArg) {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);


      Boolean bStartMainForm = true;

      foreach (String s in asArg) {
        switch (s) {
          case "FlowLayoutManagerSample": {
              RT.ViewPDF(new FlowLayoutManagerSample(), "FlowLayoutManagerSample.pdf");
              bStartMainForm = false;
              break;
            }
          case "Test": {
              RT.ViewPDF(new Test(), "Test.pdf");
              bStartMainForm = false;
              break;
            }
          case "ImageSample": {
              RT.ViewPDF(new ImageSample(), "ImageSample.pdf");
              bStartMainForm = false;
              break;
            }
          case "ListLayoutManagerSample": {
              RT.ViewPDF(new ListLayoutManagerSample(), "ListLayoutManagerSample.pdf");
              bStartMainForm = false;
              break;
            }
          case "PdfPropertiesSample": {
              RT.ViewPDF(new PdfPropertiesSample(), "PdfPropertiesSample.pdf");
              bStartMainForm = false;
              break;
            }
          case "TableLayoutManagerSample": {
              RT.ViewPDF(new TableLayoutManagerSample(), "TableLayoutManagerSample.pdf");
              bStartMainForm = false;
              break;
            }
          case "TextSample": {
              RT.ViewPDF(new TextSample(), "TextSample.pdf");
              bStartMainForm = false;
              break;
            }
        }
      }

      if (bStartMainForm) {
        Application.Run(new MainForm());
      }
    }
  }
}
