using Root.Reports;
using System;
using System.Text;

// Creation date: 08.11.2002
// Checked: 13.11.2004
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
  /// <summary>Flow Layout Manager Sample</summary>
  /// <remarks>
  /// This sample creates a table with data from an ADO.NET data source (SQL-Server, sample database "Northwind").
  /// The table layout manager <see cref="Root.Reports.TableLayoutManager"/> automatically builds the grid lines,
  /// header of the table, page breaks, etc.
  /// The event handler <see cref="ReportSamples.TableLayoutManagerSample.Tlm_NewContainer"/> binds each table container to a new page.
  /// The first page has a caption. The following pages have no caption and therefore the table can be made higher.
  /// <note type="caution">
  ///   The connection string must contain the correct login data in order that the program can open a connection to the database.
  ///   <p/>Sample with <i>user id</i> "sa" and <i>password</i> "empty":
  ///   <code>private String sConnectionString = @"Provider=SQLOLEDB;initial catalog=northwind;user id=sa;password=";</code>
  /// </note>
  /// <para>PDF file: <see href="http://web.root.ch/developer/report_net/samples/FlowLayoutManagerSample.pdf">FlowLayoutManagerSample.pdf</see></para>
  /// <para>Source: <see href="http://web.root.ch/developer/report_net/samples/FlowLayoutManagerSample.cs">FlowLayoutManagerSample.cs</see></para>
  /// </remarks>
  public class FlowLayoutManagerSample : Report {
    private FontDef fd;
    private Double rPosLeft = 20;  // millimeters
    private Double rPosRight = 195;  // millimeters
    private Double rPosTop = 24;  // millimeters
    private Double rPosBottom = 278;  // millimeters

    //------------------------------------------------------------------------------------------31.10.2004
    /// <summary>Creates this report.</summary>
    /// <remarks>
    /// This method overrides the method <see cref="Root.Reports.Report.Create"/> of the base class <see cref="Root.Reports.Report"/>.
    /// </remarks>
    protected override void Create() {
      fd = new FontDef(this, FontDef.StandardFont.Helvetica);
      FontProp fp_Title = new FontPropMM(fd, 7.8);
      fp_Title.bBold = true;
      FontProp fp = new FontPropMM(fd, 2.1);
      fp.rLineFeedMM *= 0.85;  // reduce line height by 15%
      FontProp fp_Bold = new FontPropMM(fd, 2.1);
      fp_Bold.rLineFeedMM *= 0.85;
      fp_Bold.bBold = true;

      FlowLayoutManager flm = new FlowLayoutManager();
      flm.rContainerWidthMM = rPosRight - rPosLeft;
      flm.rContainerHeightMM = rPosBottom - rPosTop;
      flm.eNewContainer += new FlowLayoutManager.NewContainerEventHandler(Flm_NewContainer);

      // generate random text
      StringBuilder sb = new StringBuilder(20000);
      Random random = new Random(unchecked((Int32)DateTime.Now.Ticks));
      for (Int32 iItem = 0;  iItem < 2000;  iItem++) {
        Int32 iLength = (Int32)Math.Sqrt(random.NextDouble() * 200) + 2;
        for (Int32 iWord = 0;  iWord < iLength;  iWord++) {
          sb.Append((Char)(random.Next((Int32)'z' - (Int32)'a' + 1) + (Int32)'a'));
        }
        sb.Append(" ");
        Int32 iOp = random.Next(40);
        if (iOp == 0) {
          flm.Add(new RepString(fp_Bold, sb.ToString()));  // append random text with bold font
          sb.Length = 0;
        }
        else if (iOp < 5) {
          flm.Add(new RepString(fp, sb.ToString()));  // append random text with normal font
          sb.Length = 0;
        }
        if (iOp == 1) {
          flm.NewLine(fp.rLineFeed * 1.5);  // new line
        }
      }

      // print page number and current date/time
      Double rY = rPosBottom + 1.5;
      foreach (Page page in enum_Page) {
        page.AddLT_MM(rPosLeft, rY, new RepString(fp, DateTime.Now.ToShortDateString()  + "  " + DateTime.Now.ToShortTimeString()));
        page.AddRT_MM(rPosRight, rY, new RepString(fp, page.iPageNo + " / " + iPageCount));
      }
    }

    //------------------------------------------------------------------------------------------31.10.2004
    /// <summary>Creates a new page.</summary>
    /// <param name="oSender">Sender</param>
    /// <param name="ea">Event argument</param>
    /// <remarks>
    /// The first page has a caption . The following pages have no caption and therefore the text area can be made higher.
    /// </remarks>
    private void Flm_NewContainer(Object oSender, FlowLayoutManager.NewContainerEventArgs ea) {
      new Page(this);

      // first page with caption
      if (page_Cur.iPageNo == 1) {
        FontProp fp_Title = new FontPropMM(fd, 7);
        fp_Title.bBold = true;
        page_Cur.AddCT_MM(rPosLeft + (rPosRight - rPosLeft) / 2, rPosTop, new RepString(fp_Title, "Flow Layout Manager Sample"));
        ea.container.rHeightMM -= fp_Title.rLineFeedMM;  // reduce height of table container for the first page
      }

      // the new container must be added to the current page
      page_Cur.AddMM(rPosLeft, rPosBottom - ea.container.rHeightMM, ea.container);
    }
  }
}
