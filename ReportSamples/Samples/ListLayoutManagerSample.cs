using Root.Reports;
using System;
using System.Drawing;

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
  /// <summary>List Layout Manager Sample</summary>
  /// <remarks>
  /// This sample creates a list with three rows.
  /// The container of the list has been created with <see cref="TlmBase.container_CreateMM"/>.
  /// If the list layout manager should handle page breaks, you must set the new-container event handler
  /// (see <see cref="ReportSamples.TableLayoutManagerSample"/>).
  /// <code>
  /// Static container:
  /// llm.container_CreateMM(page_Cur, rMarginLeft, rY);
  /// 
  /// 
  /// Automatic page breaks:
  /// llm.eNewContainer += new ListLayoutManager.NewContainerEventHandler(Llm_NewContainer);
  /// 
  /// public void Llm_NewContainer(Object oSender, ListLayoutManager.NewContainerEventArgs ea) {
  ///   new Page(this);
  ///   page_Cur.AddMM(20, 200, ea.container);
  /// }
  /// </code>
  /// <para>PDF file: <see href="http://web.root.ch/developer/report_net/samples/ListLayoutManagerSample.pdf">ListLayoutManagerSample.pdf</see></para>
  /// <para>Source: <see href="http://web.root.ch/developer/report_net/samples/ListLayoutManagerSample.cs">ListLayoutManagerSample.cs</see></para>
  /// </remarks>
  public class ListLayoutManagerSample : Report {
    private Double rMarginLeft = 20;  // millimeters
    private Double rWidth = 175;  // millimeters

    //------------------------------------------------------------------------------------------13.11.2004
    /// <summary>Creates this report.</summary>
    /// <remarks>
    /// This method overrides the method <see cref="Root.Reports.Report.Create"/> of the base class <see cref="Root.Reports.Report"/>.
    /// </remarks>
    protected override void Create() {
      FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica);
      FontProp fp = new FontPropMM(fd, 1.9);
      Double rY = 40;  // vertical position in millimeters

      new Page(this);
      FontProp fp_Title = new FontPropMM(fd, 8);
      page_Cur.AddCB_MM(rY, new RepString(fp_Title, "List Layout Manager Sample"));
      rY += 18;

      // create list
      ListLayoutManager llm = null;
      using (llm = new ListLayoutManager(this)) {
        PenProp pp_BorderLine = new PenPropMM(this, 0.4, Color.Blue);
        PenProp pp_GridLine = new PenPropMM(this, 0.1, Color.Blue);

        llm.tlmColumnDef_Default.penProp_BorderH = pp_BorderLine;
        llm.tlmCellDef_Default.penProp_Line = pp_GridLine;

        TlmColumn col_Number = new TlmColumnMM(llm, 10);
        col_Number.tlmCellDef_Default.rAlignH = RepObj.rAlignCenter;
        col_Number.tlmCellDef_Default.rAlignV = RepObj.rAlignCenter;
        col_Number.tlmCellDef_Default.penProp_LineLeft = pp_BorderLine;

        TlmColumn col_Text = new TlmColumnMM(llm, 100);
        col_Text.tlmCellDef_Default.tlmTextMode = TlmTextMode.MultiLine;

        TlmColumn col_Author = new TlmColumnMM(llm, rWidth - llm.rWidthMM);
        col_Author.tlmCellDef_Default.penProp_LineRight = pp_BorderLine;
        col_Author.tlmCellDef_Default.brushProp_Back = new BrushProp(this, Color.FromArgb(255, 210, 210));
      
        llm.container_CreateMM(page_Cur, rMarginLeft, rY);  // immediately creates a container

        // first row
        llm.NewRow();
        col_Number.Add(new RepString(fp, "1."));
        col_Text.Add(new RepString(fp, "Rapunzel "));
        col_Author.Add(new RepString(fp, "Gebrüder Grimm"));

        TlmRow row = llm.tlmRow_New();
        col_Text.Add(new RepString(fp, "There once was a woman and a man that lived next to a witch. They were very happy. The lady saw rapunzel in the witch’s garden and she really wanted to eat it. When she ate it, she liked it. "));

        // second row
        llm.tlmCellDef_Default.penProp_LineTop = pp_BorderLine;
        row = llm.tlmRow_New();
        row.aTlmCell[col_Text].penProp_Line = new PenPropMM(this, 0.5, Color.Red);
        row.aTlmCell[col_Text].iOrderLineRight = 1;
        row.aTlmCell[col_Text].iOrderLineBottom = 1;
        col_Number.Add(new RepString(fp, "2."));
        col_Text.Add(new RepString(fp, "The Princess and the Pea"));
        col_Author.Add(new RepString(fp, "Hans Christian Andersen"));

        llm.tlmCellDef_Default.penProp_LineTop = pp_GridLine;
        llm.NewRow();
        col_Text.Add(new RepString(fp, "In a far away land, there lived a prince who wanted a real princess to be his wife. He traveled around the world to find her. He returned alone and unhappy to his kingdom."));
        col_Text.NewLine();
        col_Text.Add(new RepString(fp, "One stormy night, a princess arrived at the door. The lightening was flashing. Torrents of rain were rushing down. The princess's clothes were soaked. Water was running into the heels of her shoes and out the toes. It was a dreadful storm."));

        // third row
        llm.tlmCellDef_Default.penProp_LineTop = pp_BorderLine;
        llm.NewRow();
        col_Number.Add(new RepString(fp, "3."));
        col_Text.Add(new RepString(fp, "Hansel and Gretel"));
        col_Author.Add(new RepString(fp, "Gebrüder Grimm"));

        llm.tlmCellDef_Default.penProp_LineTop = pp_GridLine;
        llm.NewRow();
        col_Text.Add(new RepString(fp, "Once upon a time in a vast forest lived a poor family. Since there was a famine, they had zero food."));
        col_Text.NewLineMM(fp.rLineFeedMM + 1);
        col_Text.Add(new RepString(fp, "There was a woodcutter who had a mean wife and a boy named Hansel and a girl named Gretel. The mean wife decided to get rid of Hansel and Gretel so she could have more food. The children overheard her plan to leave them in the forest. Hansel gathered pebbles from his yard."));
      }
      rY += llm.rCurY_MM + 1.5;
      fp.rSizeMM = 1.5;
      page_Cur.AddRT_MM(rMarginLeft + rWidth, rY, new RepString(fp, "End of list"));
    }
  }
}
