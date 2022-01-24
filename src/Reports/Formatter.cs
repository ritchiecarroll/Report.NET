using System;
using System.IO;

// Creation date: 22.04.2002
// Checked: xx.05.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary></summary>
  public abstract class Formatter {
    /// <summary>Report to which this formatter belongs</summary>
    internal Report report;

    /// <summary>Stream to which the result must be written</summary>
    protected Stream _stream;

    /// <summary>Title of the document</summary>
    public String sTitle {
      get { return report.sTitle; }
      set { report.sTitle = value; }
    }

    /// <summary>The name of the person who created the document</summary>
    public String sAuthor {
      get { return report.sAuthor; }
      set { report.sAuthor = value; }
    }

    /// <summary>Determines the page layout in the PDF document</summary>
    public PageLayout pageLayout = PageLayout.SinglePage;

    /// <summary>Application that created the document</summary>
    public String sCreator;

    /// <summary>Creation date and time of  the document</summary>
    public DateTime dt_CreationDate = DateTime.Today;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a formatter object.</summary>
    protected Formatter() {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates the report.</summary>
    /// <param name="report">Report</param>
    /// <param name="stream">Output stream</param>
    public virtual void Create(Report report, Stream stream) {
      this.report = report;
      _stream = stream;
    }

    //----------------------------------------------------------------------------------------------------x
#if DEVELOPER  // bs
   /// <summary>Creates a graphics state data object for this formatter.</summary>
    /// <param name="graphicsState"></param>
    /// <returns></returns>
    internal protected abstract GraphicsStateData graphicsStateData_CreateInstance(GraphicsState graphicsState);
#endif
 
    //----------------------------------------------------------------------------------------------------
    /// <summary>Gets the output stream object.</summary>
    public Stream stream {
      set { _stream = value; }
      get { return _stream; }
    }

    //------------------------------------------------------------------------------------------xx.01.2006
    #region Create RepObjX Objects
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------03.02.2006
    internal virtual Object oCreate_Container() {
      return null;
    }

    //------------------------------------------------------------------------------------------03.02.2006
    internal virtual Object oCreate_RepArcBase() {
      return null;
    }

    //------------------------------------------------------------------------------------------03.02.2006
    internal virtual Object oCreate_RepImage() {
      return null;
    }

    //------------------------------------------------------------------------------------------03.02.2006
    internal virtual Object oCreate_RepLine() {
      return null;
    }

    //------------------------------------------------------------------------------------------03.02.2006
    internal virtual Object oCreate_RepRect() {
      return null;
    }

    //------------------------------------------------------------------------------------------03.02.2006
    internal virtual Object oCreate_RepString() {
      return null;
    }

    //------------------------------------------------------------------------------------------03.02.2006
    /// <summary>Creates the extended page data object.</summary>
    /// <param name="page">Page</param>
    /// <returns>Extended page data object</returns>
    internal abstract Object oCreate_PageX(Page page);
    #endregion
  }

  //------------------------------------------------------------------------------------------01.02.2006
  #region PageLayout
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Page Layout</summary>
  /// <remarks>This attribute specifies the page layout to be used when the document is opened.</remarks>
  public enum PageLayout {
    /// <summary>Display one page at a time.</summary>
    SinglePage,
    /// <summary>Display the pages in one column.</summary>
    OneColumn,
    /// <summary>Display the pages in two columns, with odd-numbered pages on the left.</summary>
    TwoColumnLeft,
    /// <summary>Display the pages in two columns, with odd-numbered pages on the right.</summary>
    TwoColumnRight,
    /// <summary>Display the pages two at a time, with odd-numbered pages on the left.</summary>
    TwoPageLeft,
    /// <summary>Display the pages two at a time, with odd-numbered pages on the right.</summary>
    TwoPageRight
  }
  #endregion
}
