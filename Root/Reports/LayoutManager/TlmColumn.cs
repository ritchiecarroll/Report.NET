using System;

// Creation date: 05.11.2002
// Checked: 11.06.2003
// Author: Otto Mayer (mot@root.ch)
// Version: 1.02

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Definition of a column for the table layout manager</summary>
  /// <remarks>
  /// <para>The columns of a table must be defined before the first report objects have been added to the table.</para>
  /// </remarks>
  public class TlmColumn : TlmColumnDef {
    /// <summary>Table layout manager of this column</summary>
    internal readonly TlmBase tlmBase;

    /// <summary>Definition of the default properties for a cell</summary>
    /// <remarks>
    /// This class defines the default properties for a cell of this column.
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmBaseDefaults_Cell"]/*'/>
    /// </remarks>
    public readonly TlmCellDef tlmCellDef_Default;

    /// <summary>Index of this column</summary>
    internal readonly Int32 iIndex;

    /// <summary>Number of committed report objects within the committed row</summary>
    internal Int32 iRepObjCommitted;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a column definition object.</summary>
    /// <param name="tlmBase">Table layout manager of this column</param>
    /// <param name="rWidth">Width of the column (points, 1/72 inch)</param>
    internal TlmColumn(TlmBase tlmBase, Double rWidth) {
      tlmBase.CheckStatus_Init("cannot add columns.");  
      this.tlmBase = tlmBase;
      tlmCellDef_Default = new TlmCellDef();
      iIndex = tlmBase.list_TlmColumn.Count;
      tlmBase.list_TlmColumn.Add(this);

      if (rWidth <= 0) {
        throw new ReportException("Invalid value for the column width");
      }
      this.rWidth = rWidth;

      rBorderTop = tlmBase.tlmColumnDef_Default.rBorderTop;
      rBorderBottom = tlmBase.tlmColumnDef_Default.rBorderBottom;
      penProp_BorderTop = tlmBase.tlmColumnDef_Default.penProp_BorderTop;
      penProp_BorderBottom = tlmBase.tlmColumnDef_Default.penProp_BorderBottom;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a column definition object.</summary>
    /// <param name="tlmBase">Table layout manager of this column</param>
    /// <param name="sHeader">Header of the column</param>
    /// <param name="rWidth">Width of the column</param>
    public TlmColumn(TlmBase tlmBase, String sHeader, Double rWidth) : this(tlmBase, rWidth) {
      this.sHeader = sHeader;
      fontProp_Header = tlmBase.fontProp_Header;
    }

    //----------------------------------------------------------------------------------------------------x
    #region Add/NewLine
    //----------------------------------------------------------------------------------------------------x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the current cell.</summary>
    /// <remarks>The current cell is the cell that is within the current row (<see cref="TlmBase.tlmRow_Cur">TlmBase.tlmRow_Cur</see>) and this column.</remarks>
    /// <param name="repObj">Report object that will be added to the current cell.</param>
    /// <exception cref="ReportException">No row is available or the current row is not open.</exception>
    public void Add(RepObj repObj) {
      tlmBase.Add(iIndex, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the current cell.</summary>
    /// <remarks>The current cell is the cell that is within the current row (<see cref="TlmBase.tlmRow_Cur">TlmBase.tlmRow_Cur</see>) and this column.</remarks>
    /// <param name="repObj">Report object that will be added to the current cell.</param>
    /// <exception cref="ReportException">No row is available or the current row is not open.</exception>
    public void AddLT(RepObj repObj) {
      repObj.rAlignH = RepObj.rAlignLeft;
      repObj.rAlignV = RepObj.rAlignTop;
      tlmBase.Add(iIndex, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <overloads>
    ///   <summary>Makes a new line in the current cell.</summary>
    /// </overloads>
    /// 
    /// <summary>Makes a new line in the current cell with the specified line feed height.</summary>
    /// <remarks>
    /// The current cell is the cell that is within the current row (<see cref="TlmBase.tlmRow_Cur">TlmBase.tlmRow_Cur</see>) and this column.
    /// The current vertical position <see cref="TlmCell.rCurY">TlmCell.rCurY</see> will be incremented by <paramref name="rLineFeed"/>, the current horizontal position <see cref="TlmCell.rCurX">TlmCell.rCurX</see> will be set to the left indent <see cref="TlmCellDef.rIndentLeft">TlmCellDef.rIndentLeft</see>.
    /// <para>For the metric version see <see cref="TlmColumn.NewLineMM"/>.</para>
    /// </remarks>
    /// <param name="rLineFeed">Height of the line feed in points (1/72 inch).</param>
    /// <exception cref="ReportException">The cell is not <see cref="TlmCell.Status">Open</see>.</exception>
    /// <seealso cref="TlmColumn.NewLineMM"/>
    public void NewLine(Double rLineFeed) {
      tlmBase.NewLine(iIndex, rLineFeed);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Makes a new line in the current cell with the standard line feed height.</summary>
    /// <remarks>
    /// The current cell is the cell that is within the current row (<see cref="TlmBase.tlmRow_Cur">TlmBase.tlmRow_Cur</see>) and this column.
    /// The current vertical position <see cref="TlmCell.rCurY">TlmCell.rCurY</see> will be incremented by the value of <see cref="TlmCellDef.rLineFeed">TlmCellDef.rLineFeed</see>, the current horizontal position <see cref="TlmCell.rCurX">TlmCell.rCurX</see> will be set to the left indent <see cref="TlmCellDef.rIndentLeft">TlmCellDef.rIndentLeft</see>.
    /// </remarks>
    /// <exception cref="ReportException">The cell is not <see cref="TlmCell.Status">Open</see>.</exception>
    /// <seealso cref="TlmColumn.NewLine"/>
    public void NewLine() {
      tlmBase.NewLine(iIndex);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Makes a new line in the current cell with the specified line feed height.</summary>
    /// <remarks>
    /// The current cell is the cell that is within the current row (<see cref="TlmBase.tlmRow_Cur">TlmBase.tlmRow_Cur</see>) and this column.
    /// The current vertical position <see cref="TlmCell.rCurY">TlmCell.rCurY</see> will be incremented by <paramref name="rLineFeedMM"/>, the current horizontal position <see cref="TlmCell.rCurX">TlmCell.rCurX</see> will be set to the left indent <see cref="TlmCellDef.rIndentLeft">TlmCellDef.rIndentLeft</see>.
    /// <para>For the inch version see <see cref="TlmColumn.NewLine(System.Double)"/>.</para>
    /// </remarks>
    /// <param name="rLineFeedMM">Height of the line feed in millimeters.</param>
    /// <exception cref="ReportException">The cell is not <see cref="TlmCell.Status">Open</see>.</exception>
    /// <seealso cref="TlmColumn.NewLine(System.Double)"/>
    public void NewLineMM(Double rLineFeedMM) {
      tlmBase.NewLineMM(iIndex, rLineFeedMM);
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Layout
    //----------------------------------------------------------------------------------------------------x

    internal Double _rPosX = 0;
    /// <summary>Gets the horizontal position of the left side of the column.</summary>
    /// <value>The position of the left side of the column in points (1/72 inch).</value>
    /// <remarks>For the metric version see <see cref="TlmColumn.rPosX_MM"/>.</remarks>
    public Double rPosX {
      get { return _rPosX; }
    }

    /// <summary>Gets the horizontal position of the left side of the column.</summary>
    /// <value>The position of the left side of the column in millimeters.</value>
    /// <remarks>For the inch version see <see cref="TlmColumn.rPosX"/>.</remarks>
    public Double rPosX_MM {
      get { return RT.rMMFromPoint(_rPosX); }
    }

    private Double _rWidth;
    /// <summary>Gets or sets the width of the column.</summary>
    /// <value>The width of the column in points (1/72 inch)</value>
    /// <remarks>
    /// The width of the column can be set as long as the table is in initialization mode.
    /// <para>For the metric version see <see cref="TlmColumn.rWidthMM"/>.</para>
    /// </remarks>
    public Double rWidth {
      get { return _rWidth; }
      set {
        tlmBase.CheckStatus_Init("cannot modify width of the column.");
        _rWidth = value;
      }
    }

    /// <summary>Gets or sets the width of the column.</summary>
    /// <value>The width of the column in millimeters.</value>
    /// <remarks>
    /// The width of the column can be set as long as the table is in initialization mode.
    /// <para>For the inch version see <see cref="TlmColumn.rWidth"/>.</para>
    /// </remarks>
    public Double rWidthMM {
      get { return RT.rMMFromPoint(rWidth); }
      set { rWidth = RT.rPointFromMM(value); }
    }
    #endregion

    //----------------------------------------------------------------------------------------------------
    #if Compatible_0_8
    //----------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------
    [Obsolete("use 'tlmCellDef_Default'")]
    public TlmCellDef cellDef {
      get { return tlmCellDef_Default; }
    }

    //----------------------------------------------------------------------------------------------------
    [Obsolete("use 'tlmCellDef_Default'")]
    public TlmCellDef _cellDef {
      get { return tlmCellDef_Default; }
    }
    #endif
  }

  /// <summary>Column definition with metric values.</summary>
  public class TlmColumnMM : TlmColumn {
    /// <summary>Creates a column definition object with metric values.</summary>
    /// <param name="tlm">Table layout manager of this column</param>
    /// <param name="sHeader">Header of the column</param>
    /// <param name="rWidthMM">Width of the column</param>
    public TlmColumnMM(TableLayoutManager tlm, String sHeader, Double rWidthMM)
      : base(tlm, sHeader, RT.rPointFromMM(rWidthMM)) {
    }

    /// <summary>Creates a column definition object with metric values.</summary>
    /// <param name="tlm">Table layout manager of this column</param>
    /// <param name="sHeader">Header of the column</param>
    /// <param name="rWidthMM">Width of the column</param>
    public TlmColumnMM(ListLayoutManager tlm, Double rWidthMM)
      : base(tlm, null, RT.rPointFromMM(rWidthMM)) {
    }
  }
}
