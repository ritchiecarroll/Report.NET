using System;
using System.Collections;
using System.Drawing;

// Creation date: 22.04.2002
// Checked: 17.05.2002
// Author: Otto Mayer, mot@root.ch
// Version 0.02.00

// copyright (C) 2002 root-software ag  -  Bürglen Switzerland  -  www.root.ch; Otto Mayer, Stefan Spirig, Roger Gartenmann
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Page of a report</summary>
  /// <example>Page sample:
  /// <code>
  /// using Root.Reports; 
  /// using System; 
  /// 
  /// namespace ReportSamples { 
  ///   public class PageSample : Report { 
  ///     public static void Main() { 
  ///       RT.ViewPDF(new PageSample(), "PageSample.pdf"); 
  ///     } 
  /// 
  ///     protected override void Create() { 
  ///       FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica); 
  ///       FontProp fp = new FontPropMM(fd, 20); 
  ///       Page page = new Page(this); 
  ///       page.rWidthMM = 100; 
  ///       page.rHeightMM = 50; 
  ///       page.AddCenteredMM(80, new RepString(fp, "Page Sample")); 
  ///     } 
  ///   } 
  /// } 
  /// </code>
  /// </example>
  public class Page : StaticContainer {
    /// <summary>Report that this page belongs to</summary>
    internal readonly new Report report;

    /// <summary>Page Number</summary>
    public Int32 iPageNo;

    /// <summary>Page name</summary>
    public String sName;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a page for the report</summary>
    /// <param name="report">Report to which this page will be add</param>
    public Page(Report report) : base(RT.rPointFromMM(210.224), RT.rPointFromMM(297.302)) {
      this.report = report;
      report.RegisterPage(this);
      oRepObjX = report.formatter.oCreate_PageX(this);
      iPageNo = report.iPageCount;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Sets the landscape orientation for this page.</summary>
    public void SetLandscape() {
      Double r = rHeight;
      rHeight = rWidth;
      rWidth = r;
    }

  }
}
