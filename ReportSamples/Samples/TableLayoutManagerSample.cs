using Root.Reports;
using System;
using System.IO;
using System.Data;
using System.Drawing;

// Creation date: 08.11.2002
// Checked: 31.10.2004
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
  /// <summary>Table Layout Manager and Data-Set Sample</summary>
  /// <remarks>
  /// This sample creates a table with data from a data set.
  /// The table layout manager <see cref="Root.Reports.TableLayoutManager"/> automatically builds the grid lines,
  /// header of the table, page breaks, etc.
  /// The event handler <see cref="ReportSamples.TableLayoutManagerSample.Tlm_NewContainer"/> binds each table container to a new page.
  /// The first page has a caption. The following pages have no caption and therefore the table can be made higher.
  /// <para>PDF file: <see href="http://web.root.ch/developer/report_net/samples/TableLayoutManagerSample.pdf">TableLayoutManagerSample.pdf</see></para>
  /// <para>Source: <see href="http://web.root.ch/developer/report_net/samples/TableLayoutManagerSample.cs">TableLayoutManagerSample.cs</see></para>
  /// </remarks>
  public class TableLayoutManagerSample : Report {
    private FontDef fontDef_Helvetica;
    private Double rPosLeft = 20;  // millimeters
    private Double rPosRight = 195;  // millimeters
    private Double rPosTop = 24;  // millimeters
    private Double rPosBottom = 278;  // millimeters

    //------------------------------------------------------------------------------------------30.10.2004
    /// <summary>Creates this report.</summary>
    /// <remarks>
    /// This method overrides the method <see cref="Root.Reports.Report.Create"/> of the base class <see cref="Root.Reports.Report"/>.
    /// </remarks>
    protected override void Create() {  
      fontDef_Helvetica = new FontDef(this, FontDef.StandardFont.Helvetica);
      FontProp fontProp_Text = new FontPropMM(fontDef_Helvetica, 1.9);  // standard font
      FontProp fontProp_Header = new FontPropMM(fontDef_Helvetica, 1.9);  // font of the table header
      fontProp_Header.bBold = true;

      Stream stream_Phone = GetType().Assembly.GetManifestResourceStream("ReportSamples.Phone.jpg");
      Random random = new Random(6);

      // create table
      TableLayoutManager tlm;
      using (tlm = new TableLayoutManager(fontProp_Header)) { 
        tlm.rContainerHeightMM = rPosBottom - rPosTop;  // set height of table
        tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter;  // set vertical alignment of all header cells
        tlm.tlmCellDef_Default.penProp_LineBottom = new PenProp(this, 0.05, Color.LightGray);  // set bottom line for all cells
        tlm.tlmHeightMode = TlmHeightMode.AdjustLast;
        tlm.eNewContainer += new TableLayoutManager.NewContainerEventHandler(Tlm_NewContainer);

        // define columns
        TlmColumn col;
        col = new TlmColumnMM(tlm, "ID", 13);

        col = new TlmColumnMM(tlm, "Company Name", 40);
        col.tlmCellDef_Default.tlmTextMode = TlmTextMode.MultiLine;

        col = new TlmColumnMM(tlm, "Address", 36);

        col = new TlmColumnMM(tlm, "City", 22);

        col = new TlmColumnMM(tlm, "Postal Code", 16);

        col = new TlmColumnMM(tlm, "Country", 18);

        TlmColumn col_Phone = new TlmColumnMM(tlm, "Phone", rPosRight - rPosLeft - tlm.rWidthMM);
        col_Phone.fontProp_Header = new FontPropMM(fontDef_Helvetica, 1.9, Color.Brown);
        col_Phone.tlmCellDef_Header.rAlignH = RepObj.rAlignRight;
        col_Phone.tlmCellDef_Default.rAlignH = RepObj.rAlignRight;
        BrushProp brushProp_Phone = new BrushProp(this, Color.FromArgb(255, 255, 200));
        col_Phone.tlmCellDef_Default.brushProp_Back = brushProp_Phone;
        BrushProp brushProp_USA = new BrushProp(this, Color.FromArgb(255, 180, 180));

        // open data set
        DataSet dataSet = new DataSet();
        using (Stream stream_Customers = GetType().Assembly.GetManifestResourceStream("ReportSamples.Customers.xml")) {
          dataSet.ReadXml(stream_Customers);
        }
        DataTable dataTable_Customers = dataSet.Tables[0];

        foreach (DataRow dr in dataTable_Customers.Rows) {
          String sCountry = (String)dr["Country"];
          tlm.tlmCellDef_Default.brushProp_Back = (sCountry == "USA" ? brushProp_USA : null);
          col_Phone.tlmCellDef_Default.brushProp_Back = (sCountry == "USA" ? new BrushProp(this, Color.FromArgb(255, 227, 50)) : brushProp_Phone);
          tlm.NewRow();
          tlm.Add(0, new RepString(fontProp_Text, (String)dr["CustomerID"]));
          tlm.Add(1, new RepString(fontProp_Text, (String)dr["CompanyName"]));
          tlm.Add(2, new RepString(fontProp_Text, (String)dr["Address"]));
          tlm.Add(3, new RepString(fontProp_Text, (String)dr["City"]));
          tlm.Add(4, new RepString(fontProp_Text, (String)dr["PostalCode"]));
          tlm.Add(5, new RepString(fontProp_Text, sCountry));
          tlm.Add(6, new RepString(fontProp_Text, (String)dr["Phone"]));
          if (random.NextDouble() < 0.2) {  // mark randomly selected row with a phone icon
            tlm.tlmRow_Cur.aTlmCell[col_Phone].AddMM(1, 0.25, new RepImageMM(stream_Phone, 2.1, 2.3));
          }
        }
      }
      page_Cur.AddCT_MM(rPosLeft + tlm.rWidthMM / 2, rPosTop + tlm.rCurY_MM + 2, new RepString(fontProp_Text, "- end of table -"));

      // print page number and current date/time
      Double rY = rPosBottom + 1.5;
      foreach (Page page in enum_Page) {
        page.AddLT_MM(rPosLeft, rY, new RepString(fontProp_Text, DateTime.Now.ToShortDateString()  + "  " + DateTime.Now.ToShortTimeString()));
        page.AddRT_MM(rPosRight, rY, new RepString(fontProp_Text, page.iPageNo + " / " + iPageCount));
      }
    }

    //------------------------------------------------------------------------------------------30.10.2004
    /// <summary>Creates a new page.</summary>
    /// <param name="oSender">Sender</param>
    /// <param name="ea">Event argument</param>
    /// <remarks>
    /// The first page has a caption. The following pages have no caption and therefore the table can be made higher.
    /// </remarks>
    public void Tlm_NewContainer(Object oSender, TableLayoutManager.NewContainerEventArgs ea) {  // only "public" for NDoc, should be "private"
      new Page(this);

      // first page with caption
      if (page_Cur.iPageNo == 1) {
        FontProp fontProp_Title = new FontPropMM(fontDef_Helvetica, 7);
        fontProp_Title.bBold = true;
        page_Cur.AddCT_MM(rPosLeft + (rPosRight - rPosLeft) / 2, rPosTop, new RepString(fontProp_Title, "Customer List"));
        ea.container.rHeightMM -= fontProp_Title.rLineFeedMM;  // reduce height of table container for the first page
      }

      // the new container must be added to the current page
      page_Cur.AddMM(rPosLeft, rPosBottom - ea.container.rHeightMM, ea.container);
    }
  }
}
