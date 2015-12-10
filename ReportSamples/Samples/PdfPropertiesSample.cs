using Root.Reports;
using System;

// Creation date: 08.08.2002
// Checked: 12.12.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace ReportSamples {
  /// <summary>PDF Properties Sample</summary>
  public class PdfPropertiesSample : Report {
    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    public PdfPropertiesSample() {
      PdfFormatter pf =  (PdfFormatter)formatter;
      pf.sTitle = "PDF Sample";
      pf.sAuthor = "Otto Mayer, mot@root.ch";
      pf.sSubject = "Sample of some PDF features";
      pf.sKeywords = "Sample PDF RSF";
      pf.sCreator = "RSF Sample Application";
      pf.dt_CreationDate = new DateTime(2002, 8, 15, 0,0,0,0);
      pf.pageLayout = PageLayout.TwoColumnLeft;
      pf.bHideToolBar = true;
      pf.bHideMenubar = false;
      pf.bHideWindowUI = true;
      pf.bFitWindow = true;
      pf.bCenterWindow = true;
      pf.bDisplayDocTitle = true;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates this report</summary>
    protected override void Create() {
      FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica);
      FontProp fp = new FontPropMM(fd, 4);
      FontProp fp_Title = new FontPropMM(fd, 11);
      fp_Title.bBold = true;

      Page page = new Page(this);
      page.AddCB_MM(40, new RepString(fp_Title, "PDF Properties Sample"));
      fp_Title.rSizeMM = 8;
      page.AddCB_MM(100, new RepString(fp_Title, "First Page"));
      page.AddCB_MM(120, new RepString(fp, "Choose <Document Properties, Summary> from the"));
      page.AddCB_MM(126, new RepString(fp, "File menu to display the document properties"));

      page = new Page(this);
      page.AddCB_MM(100, new RepString(fp_Title, "Second Page"));
    }

  }
}
