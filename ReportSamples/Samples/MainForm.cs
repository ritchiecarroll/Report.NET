using Root.Reports;
using System;
using System.Drawing;
using System.Windows.Forms;

// Creation date: 20.03.2004
// Checked: 08.11.2004
// Author: Otto Mayer (mot@root.ch)
// Version: 1.03

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
  ///     <term>Test</term>
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
  public class MainForm : Form {
    //------------------------------------------------------------------------------------------21.03.2004
    private readonly RadioButton radioButton_Image;
    private readonly RadioButton radioButton_Text;
    private readonly RadioButton radioButton_FlowLayoutManager;
    private readonly RadioButton radioButton_ListLayoutManager;
    private readonly RadioButton radioButton_TableLayoutManager;
    private readonly RadioButton radioButton_PdfProperties;
    private readonly RadioButton radioButton_Test;

    //------------------------------------------------------------------------------------------21.03.2004
    /// <summary>Creates the dialog.</summary>
    public MainForm() {
      SuspendLayout();
      
      Text = "Report.NET Samples";
      FormBorderStyle = FormBorderStyle.FixedDialog;
      MaximizeBox = false;
      MinimizeBox = false;
      
      // groupBox_Sample
      GroupBox groupBox_Sample = new GroupBox();
      groupBox_Sample.Text = "Chose a Sample";
      groupBox_Sample.SuspendLayout();
      groupBox_Sample.Parent = this;

      Int32 rX = 16;
      Int32 rY = 20;
      Int32 rWidth = 250;
      Int32 rHeight = 24;
      Int32 rIncY = 30;

      // radioButton_Image
      radioButton_Image = new RadioButton();
      radioButton_Image.Text = "Images";
      radioButton_Image.Bounds = new Rectangle(rX, rY, rWidth, rHeight);
      radioButton_Image.Parent = groupBox_Sample;
      rY += rIncY;

      // radioButton_Text
      radioButton_Text = new RadioButton();
      radioButton_Text.Text = "Text";
      radioButton_Text.Bounds = new Rectangle(rX, rY, rWidth, rHeight);
      radioButton_Text.Parent = groupBox_Sample;
      rY += rIncY;

      // radioButton_FlowLayoutManager
      radioButton_FlowLayoutManager = new RadioButton();
      radioButton_FlowLayoutManager.Text = "Flow Layout Manager";
      radioButton_FlowLayoutManager.Bounds = new Rectangle(rX, rY, rWidth, rHeight);
      radioButton_FlowLayoutManager.Parent = groupBox_Sample;
      rY += rIncY;

      // radioButton_ListLayoutManager
      radioButton_ListLayoutManager = new RadioButton();
      radioButton_ListLayoutManager.Text = "List Layout Manager";
      radioButton_ListLayoutManager.Bounds = new Rectangle(rX, rY, rWidth, rHeight);
      radioButton_ListLayoutManager.Parent = groupBox_Sample;
      rY += rIncY;

      // radioButton_TableLayoutManager
      radioButton_TableLayoutManager = new RadioButton();
      radioButton_TableLayoutManager.Text = "Table Layout Manager, ADO.NET";
      radioButton_TableLayoutManager.Bounds = new Rectangle(rX, rY, rWidth, rHeight);
      radioButton_TableLayoutManager.Parent = groupBox_Sample;
      rY += rIncY;

      // radioButton_PdfProperties
      radioButton_PdfProperties = new RadioButton();
      radioButton_PdfProperties.Text = "PDF Properties";
      radioButton_PdfProperties.Bounds = new Rectangle(rX, rY, rWidth, rHeight);
      radioButton_PdfProperties.Parent = groupBox_Sample;
      rY += rIncY;

      // radioButton_Test
      radioButton_Test = new RadioButton();
      radioButton_Test.Text = "Tests (Font, Line)";
      radioButton_Test.Bounds = new Rectangle(rX, rY, rWidth, rHeight);
      radioButton_Test.Parent = groupBox_Sample;
      rY += rIncY;

      groupBox_Sample.Bounds = new Rectangle(20, 25, rWidth + 20, rY);
      groupBox_Sample.ResumeLayout(false);
      rY = groupBox_Sample.Bottom + 15;
      rWidth = groupBox_Sample.Width + 2 * groupBox_Sample.Left;

      // button_Start
      Button button_Start = new Button();
      button_Start.Text = "Start";
      button_Start.Bounds = new Rectangle(rWidth / 3 - 40, rY, 80, 24);
      button_Start.Click += new System.EventHandler(Action_Start);
      button_Start.Parent = this;

      // button_Close
      Button button_Close = new Button();
      button_Close.Text = "Close";
      button_Close.Bounds = new Rectangle(rWidth * 2 / 3 - 40, rY, 80, 24);
      button_Close.Click += new System.EventHandler(Action_Close);
      button_Close.Parent = this;

      rY = button_Close.Bottom;

      // MainForm
      AcceptButton = button_Start;
      CancelButton = button_Close;
      ClientSize = new Size(rWidth, rY + 20);
      StartPosition = FormStartPosition.CenterScreen;
      
      ResumeLayout(false);
    }

    //------------------------------------------------------------------------------------------08.11.2004
    /// <summary>This method will start the selected sample.</summary>
    /// <param name="oSender">Sender</param>
    /// <param name="ea">Event argument</param>
    private void Action_Start(Object oSender, EventArgs ea) {
      if (radioButton_FlowLayoutManager.Checked) {
        RT.ViewPDF(new FlowLayoutManagerSample(), "FlowLayoutManagerSample.pdf");
      }
      else if (radioButton_Test.Checked) {
        RT.ViewPDF(new Test(), "Test.pdf");
      }
      else if (radioButton_Image.Checked) {
        RT.ViewPDF(new ImageSample(), "ImageSample.pdf");
      }
      else if (radioButton_ListLayoutManager.Checked) {
        RT.ViewPDF(new ListLayoutManagerSample(), "ListLayoutManagerSample.pdf");
      }
      else if (radioButton_PdfProperties.Checked) {
        RT.ViewPDF(new PdfPropertiesSample(), "PdfPropertiesSample.pdf");
      }
      else if (radioButton_TableLayoutManager.Checked) {
        RT.ViewPDF(new TableLayoutManagerSample(), "TableLayoutManagerSample.pdf");
      }
      else if (radioButton_Text.Checked) {
        RT.ViewPDF(new TextSample(), "TextSample.pdf");
      }
    }

    //------------------------------------------------------------------------------------------21.03.2004
    /// <summary>This method will close the dialog.</summary>
    /// <param name="oSender">Sender</param>
    /// <param name="ea">Event argument</param>
    private void Action_Close(Object oSender, EventArgs ea) {
      Close();
    }
  }
}
