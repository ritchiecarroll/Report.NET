//#define Test

using System;
#if Framework2
using System.Collections.Generic;
#else
using System.Collections;
#endif
using System.Diagnostics;
#if Test
using System.Drawing;
#endif

// Creation date: 24.04.2002
// Checked: 27.10.2006
// Author: Otto Mayer (mot@root.ch)
// Version: 2.01

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Table Layout Manager Base Class</summary>
  // <remarks>
  // <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmBaseDefaults"]/*'/>
  // </remarks>
  public abstract class TlmBase : LayoutManager, IDisposable {
    //----------------------------------------------------------------------------------------------------x
    #region Constructor
    //----------------------------------------------------------------------------------------------------x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new table layout manager.</summary>
    /// <param name="report">Report of this table layout manager</param>
    internal TlmBase(Report report) : base(report) {
      tlmCellDef_Default = new TlmCellDef();
      tlmCellDef_Default.rAlignH = RepObj.rAlignLeft;
      tlmCellDef_Default.rAlignV = RepObj.rAlignTop;
      tlmCellDef_Default.rAngle = 0;
      tlmCellDef_Default.tlmTextMode = TlmTextMode.EllipsisCharacter;
      tlmCellDef_Default.rLineFeed = Double.NaN;

      tlmCellDef_Default.rMarginLeft = 0;
      tlmCellDef_Default.rMarginRight = 0;
      tlmCellDef_Default.rMarginTop = 0;
      tlmCellDef_Default.rMarginBottom = 0;

      tlmCellDef_Default.rIndentLeftMM = 1;
      tlmCellDef_Default.rIndentRightMM = 1;
      tlmCellDef_Default.rIndentTopMM = 1;
      tlmCellDef_Default.rIndentBottomMM = 1;

      tlmCellDef_Default.brushProp_Back = null;

      tlmCellDef_Default.penProp_LineLeft = null;
      tlmCellDef_Default.penProp_LineRight = null;
      tlmCellDef_Default.penProp_LineTop = null;
      tlmCellDef_Default.penProp_LineBottom = null;

      tlmCellDef_Default.iOrderLineLeft = 0;
      tlmCellDef_Default.iOrderLineRight = 0;
      tlmCellDef_Default.iOrderLineTop = 0;
      tlmCellDef_Default.iOrderLineBottom = 0;

      tlmColumnDef_Default = new TlmColumnDef();
      tlmRowDef_Default = new TlmRowDef();
      #if (Test)
      pp_Test = new PenPropMM(report, 0.1, Color.Orange);
      #endif
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Checks whether the layout manager status is 'Init'.</summary>
    /// <exception cref="ReportException">The layout manager status is not 'Init'</exception>
    internal void CheckStatus_Init(String sMsg) {
      if (status != Status.Init) {
        throw new ReportException("The layout manager must be in initialization mode; " + sMsg);
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Checks whether the layout manager is opened.</summary>
    /// <exception cref="ReportException">The layout manager status is not 'ContainerOpen'</exception>
    internal void CheckStatus_Open(String sMsg) {
      if (status != Status.Open) {
        throw new ReportException("The layout manager must be opened; " + sMsg);
      }
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Properties
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Definition of the default properties of a cell of this table</summary>
    public readonly TlmCellDef tlmCellDef_Default;

    /// <summary>Definition of the default properties of a column of this table</summary>
    public readonly TlmColumnDef tlmColumnDef_Default;

    /// <summary>Definition of the default properties of a row of this table</summary>
    public readonly TlmRowDef tlmRowDef_Default;

    /// <summary>Lines will be shortened by this value.</summary>
    /// <remarks>Lines in PDF are sometimes too long.</remarks>
    private const Double rLineDelta = 0.0;

    /// <summary>Tolerance for comparing coordinates.</summary>
    internal const Double rTol = 0.01;

    /// <summary>Header font properties</summary>
    public FontProp fontProp_Header;

    #if (Test)
    private const Double rTest = 3;
    private PenProp pp_Test;
    #else
    private const Double rTest = 0;
    #endif

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    /// <summary>Status of the layout manager</summary>
    private enum Status {
      /// <summary>Initialization mode</summary>
      Init,
      /// <summary>Table open</summary>
      Open,
      /// <summary>Container closed</summary>
      Closed
    }

    /// <summary>Status of the layout manager</summary>
    private Status status = Status.Init;

    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Initialization / Definition
    //----------------------------------------------------------------------------------------------------x

#if !Framework2
    /// <summary>Array of Column Definition Objects</summary>
    internal class List_TlmColumn : ArrayList {
      /// <summary>Creates the array of column definition objects.</summary>
      internal List_TlmColumn(Int32 iCapacity) : base(iCapacity) {
      }

      /// <summary>Gets the column definition with the specified index.</summary>
      internal new TlmColumn this[Int32 iIndex] {
        get { return (TlmColumn)base[iIndex]; }
      }
    }
#endif

    /// <summary>Column definition</summary>
    #warning must be changed for framework 2
    #if Framework2
    internal List<TlmColumn> list_TlmColumn = new List<TlmColumn>(20);
    #else
    internal List_TlmColumn list_TlmColumn = new List_TlmColumn();
    #endif

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Scales the current width of the columns to the specified width.</summary>
    /// <param name="rWidthNew">New width (points, 1/72 inch)</param>
    /// <exception cref="ReportException">The layout manager status is not 'Init'</exception>
    public void ScaleWidth(Double rWidthNew) {
      CheckStatus_Init("the width of the columns cannot be scaled.");

      Double rWidthCur = 0;
      foreach (TlmColumn col in list_TlmColumn) {
        rWidthCur += col.rWidth;
      }
      Double rScale = rWidthNew / rWidthCur;
      foreach (TlmColumn col in list_TlmColumn) {
        col.rWidth *=  rScale;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Scales the current width of the columns to the specified width (metric version).</summary>
    /// <param name="rWidthNewMM">New width (mm)</param>
    /// <exception cref="ReportException">The layout manager status is not 'Init'</exception>
    public void ScaleWidthMM(Double rWidthNewMM) {
      ScaleWidth(RT.rPointFromMM(rWidthNewMM));
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Table
    //----------------------------------------------------------------------------------------------------x

    private Double _rWidth = Double.NaN;
    /// <summary>Width of the table (points, 1/72 inch)</summary>
    public Double rWidth {
      get {
        if (Double.IsNaN(_rWidth)) {
          Double r = 0;
          foreach (TlmColumn col in list_TlmColumn) {
            r += col.rWidth;
          }
          return r;
        }
        return _rWidth;
      }
    }

    /// <summary>Width of the table (mm)</summary>
    public Double rWidthMM {
      get { return RT.rMMFromPoint(rWidth); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>The layout manager will be opened.</summary>
    public void Open() {
      if (status == Status.Open) {
        throw new ReportException("The layout manager has been opened already; " +
          "it must be in initialization mode or it must have been closed.");
      }

      if (status == Status.Init) {
        // Set position of the columns
        aCellCreateType_New = new TlmBase.CellCreateType[list_TlmColumn.Count];
        _rWidth = 0;
        foreach (TlmColumn col in list_TlmColumn) {
          col._rPosX = rWidth;
          _rWidth += col.rWidth;
          aCellCreateType_New[col.iIndex] = TlmBase.CellCreateType.New;
        }
      }

      Debug.Assert(list_TlmRow.Count == 0);
      Debug.Assert(tlmRow_Committed == null);
      foreach (TlmColumn col in list_TlmColumn) {
        Debug.Assert(col.iRepObjCommitted == 0);
      }

      status = Status.Open;

      CreateNewContainer();
      report.al_PendingTasks.Add(this);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>The layout manager will be closed.</summary>
    public void Close() {
      if (status == Status.Init ||  status == Status.Closed) {
        return;
      }

      if (list_TlmRow.Count > 0) {
        WriteAll();
      }

      Debug.Assert(list_TlmRow.Count == 0);
      Debug.Assert(tlmRow_Committed == null);

      status = Status.Closed;
      report.al_PendingTasks.Remove(this);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>The layout manager will be closed.</summary>
    public void Dispose() {
      Close();
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region List_TlmRow
    //----------------------------------------------------------------------------------------------------x

    #if !Framework2
    /// <summary>Array of TlmRows</summary>
    internal class List_TlmRow : ArrayList {
      internal List_TlmRow(Int32 iCapacity) : base(iCapacity) {
      }

      internal new TlmRow this[Int32 iIndex] {
        get { return (TlmRow)this[iIndex]; }
      }
    }
    #endif

    /// <summary>Lines will be shortened by this value.</summary>
    /// <remarks>Lines in PDF are sometimes too long.</remarks>
    public Double rMarginTop = RT.rPointFromMM(1);

    internal void InsertRow(TlmRow tlmRow_Prev, TlmRow row_New) {
      Int32 iIndex = (tlmRow_Prev == null ? 0 : tlmRow_Prev.iIndex + 1);
      list_TlmRow.Insert(iIndex, row_New);

      for (;  iIndex < list_TlmRow.Count;  iIndex++) {
        TlmRow row = list_TlmRow[iIndex];
        row.iIndex = iIndex;
      }
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Rows and Report Objects
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Array of all rows of the table</summary>
    #if Framework2
    internal List<TlmRow> list_TlmRow = new List<TlmRow>(100);
    #else
    internal List_TlmRow list_TlmRow = new List_TlmRow(100);
    #endif

    /// <summary>Default cell creation definition</summary>
    internal CellCreateType[] aCellCreateType_New;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the current row.</summary>
    public TlmRow tlmRow_Cur {
      get {
        if (list_TlmRow.Count == 0) {
          return null;
        }
        return list_TlmRow[list_TlmRow.Count - 1];
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will create a new row (without commit).</summary>
    public TlmRow tlmRow_New() {
      return new TlmRow(this, tlmRow_Cur, aCellCreateType_New);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will commit all rows and after that it will create a new row.</summary>
    /// <remarks>The layout manager will be opened if required.</remarks>
    public void NewRow() {
      if (status != Status.Open) {
        Open();
      }
      TlmRow tlmRow = tlmRow_Cur;
      if (tlmRow != null && tlmRow.bAutoCommit) {
        tlmRow.Close();
        Commit();
      }
      new TlmRow(this, tlmRow_Cur, aCellCreateType_New);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new row after the specified row.</summary>
    /// <param name="tlmRow_Prev">The new row will be inserted after this row or at the beginning of list.</param>
    public TlmRow tlmRow_New(TlmRow tlmRow_Prev) {
      CheckStatus_Open("cannot create a new row");
      return new TlmRow(this, tlmRow_Prev, aCellCreateType_New);
    }

    //-----------------------------------------------------------------------------------------------------
    /// <summary>This method will create a new row.</summary>
    /// <param name="tlmRow_Prev">The new row will be inserted after this row or at the beginning of list.</param>
    /// <param name="aCellCreateType"></param>
    public TlmRow tlmRow_New(TlmRow tlmRow_Prev, CellCreateType[] aCellCreateType) {
      if (aCellCreateType == null) {
        aCellCreateType = aCellCreateType_New;
      }

      return new TlmRow(this, tlmRow_Prev, aCellCreateType);
    }

    //----------------------------------------------------------------------------------------------------x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called after a new row has been created.</summary>
    /// <param name="row">New row</param>
    internal protected virtual void OnNewRow(TlmRow row) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called before the row will be closed.</summary>
    /// <param name="row">Row that will be closed</param>
    internal protected virtual void OnClosingRow(TlmRow row) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the cell of the current row according to the column index.</summary>
    /// <param name="sMsg">Error message</param>
    /// <param name="iCol">Index of the column</param>
    /// <exception cref="ReportException">No row available, row is not open or the column index is out of range.</exception>
    private TlmCell tlmCell_FromColumnIndex(String sMsg, Int32 iCol) {
      CheckStatus_Open(sMsg);
      TlmRow tlmRow = tlmRow_Cur;
      if (tlmRow == null) {
        throw new ReportException("No row has been opened; " + sMsg);
      }
      if (tlmRow.status != TlmRow.Status.Open) {
        throw new ReportException("Row is not open; " + sMsg);
      }
      if (iCol < 0 || iCol >= list_TlmColumn.Count) {
        throw new ReportException("Column index out of range; " + sMsg);
      }
      return tlmRow.aTlmCell[iCol];
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the specified cell of the current row.</summary>
    /// <param name="iCol">Index of the column</param>
    /// <param name="repObj">Report object that will be added</param>
    /// <exception cref="ReportException">No row available, row is not open or the column index is out of range.</exception>
    public void Add(Int32 iCol, RepObj repObj) {
      TlmCell tlmCell = tlmCell_FromColumnIndex("cannot add a report object.", iCol);
      tlmCell.Add(repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Makes a new line within the specified cell of the current row.</summary>
    /// <param name="iCol">Index of the column</param>
    /// <param name="rLineFeed">Height of the line feed (points, 1/72 inch)</param>
    /// <exception cref="ReportException">No row available, row is not open or the column index is out of range.</exception>
    public void NewLine(Int32 iCol, Double rLineFeed) {
      TlmCell tlmCell = tlmCell_FromColumnIndex("cannot make a new line.", iCol);
      tlmCell.NewLine(rLineFeed);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Makes a new line within the specified cell of the current row (metric version).</summary>
    /// <param name="iCol">Index of the column</param>
    /// <param name="rLineFeedMM">Height of the line feed (mm)</param>
    /// <exception cref="ReportException">No row available, row is not open or the column index is out of range.</exception>
    public void NewLineMM(Int32 iCol, Double rLineFeedMM) {
      NewLine(iCol, RT.rPointFromMM(rLineFeedMM));
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Makes a new line within the specified cell of the current row.</summary>
    /// <param name="iCol">Index of the column</param>
    /// <exception cref="ReportException">No row available, row is not open or the column index is out of range.</exception>
    public void NewLine(Int32 iCol) {
      TlmCell tlmCell = tlmCell_FromColumnIndex("cannot make a new line.", iCol);
      tlmCell.NewLine();
    }

    //------------------------------------------------------------------------------------------02.11.2006
    /// <summary>Creates a new container or page.</summary>
    /// <remarks>
    /// After the current contents has been committed and written to the report, a new container or page will be created.
    /// </remarks>
    /// <include file='LayoutManager\TlmBase.cs.xml' path='documentation/class[@name="TlmBase.NewContainer"]/*'/>
    public void NewContainer() {
      Commit();  // commit all objects, only the last container will be kept open
      WriteCommittedReportObjects(false);  // write the last container to the report
      CreateNewContainer();
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Write Objects to Report
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Break Mode</summary>
    public enum BreakMode {
      /// <summary>Break on commited position</summary>
      Commit,
      /// <summary>Break on commited position or whole rows</summary>
      Row,
      /// <summary>Break on commited position or whole lines</summary>
      Line
    }

    /// <summary>Break mode</summary>
    public BreakMode breakMode = BreakMode.Commit;

    /// <summary>Break mode overflow handling</summary>
    /// <remarks>Commit: prints the whole committed block on a new page, overwriting the bottom margin if the committed block is lager than the container</remarks>
    /// <remarks>Row: prints whole rows on a new page, overwriting the bottom margin if a row is larger than the container</remarks>
    /// <remarks>Line: breaks after any line</remarks>
    public BreakMode breakMode_Overflow = BreakMode.Line;

    //----------------------------------------------------------------------------------------------------x
    private TlmRow _tlmRow_Committed;
    /// <summary>All rows up to this one have been committed</summary>
    internal TlmRow tlmRow_Committed {
      get { return _tlmRow_Committed; }
    }

    private Boolean bFullRowCommitted;

    private Double _rCurY = 0;
    /// <summary>Current vertical position (points, 1/72 inch)</summary>
    public Double rCurY {
      get { return _rCurY; }
    }

    /// <summary>Current vertical position (mm)</summary>
    public Double rCurY_MM {
      get { return RT.rMMFromPoint(rCurY); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Writes the committed horizontal lines.</summary>
    /// <param name="bOrderTop">Order</param>
    private void WriteCommittedHorLines(Boolean bOrderTop) {
      for (Int32 iRow = 0;  iRow <= tlmRow_Committed.iIndex;  iRow++) {
        TlmRow row = list_TlmRow[iRow];
        // top line
        for (Int32 iCol = 0;  iCol < list_TlmColumn.Count;  iCol++) {
          TlmCell tlmCell = row.aTlmCell[iCol];
          if (tlmCell == null || tlmCell.bVisibleLineTop(iCol) != bOrderTop) {
            continue;
          }
          if (tlmCell.tlmRow_Start.iIndex != iRow || tlmCell.penProp_LineTop == null) {
            iCol = tlmCell.tlmColumn_End.iIndex;
            continue;
          }
          Int32 iColEnd = iCol;
          OptimizeTopLine(bOrderTop, tlmCell, ref iColEnd);
          Double rPosStart = (iCol == tlmCell.tlmColumn_Start.iIndex ? tlmCell.rPosMarginLeft : list_TlmColumn[iCol].rPosX);
          TlmColumn col_End = list_TlmColumn[iColEnd];
          TlmCell cell_End = row.aTlmCell[iColEnd];
          Double rPosEnd = (iCol == col_End.iIndex ? cell_End.rPosMarginRight : col_End.rPosX + col_End.rWidth);
          RepLine repLine = new RepLine(tlmCell.penProp_LineTop, rPosEnd - rPosStart, rTest);
          container_Cur.AddLT(rPosStart, tlmCell.rPosMarginTop, repLine);
          iCol = iColEnd;
        }

        // bottom line
        for (Int32 iCol = 0;  iCol < list_TlmColumn.Count;  iCol++) {
          TlmCell tlmCell = row.aTlmCell[iCol];
          if (tlmCell == null || tlmCell.bVisibleLineBottom(iCol) != bOrderTop) {
            continue;
          }
          if (tlmCell.tlmRow_End.iIndex != iRow || tlmCell.penProp_LineBottom == null) {
            iCol = tlmCell.tlmColumn_End.iIndex;
            continue;
          }
          Int32 iColEnd = iCol;
          OptimizeBottomLine(bOrderTop, tlmCell, ref iColEnd);
          Double rPosStart = (iCol == tlmCell.tlmColumn_Start.iIndex ? tlmCell.rPosMarginLeft : list_TlmColumn[iCol].rPosX);
          TlmColumn col_End = list_TlmColumn[iColEnd];
          TlmCell cell_End = row.aTlmCell[iColEnd];
          Double rPosEnd = (iCol == col_End.iIndex ? cell_End.rPosMarginRight : col_End.rPosX + col_End.rWidth);
          RepLine repLine = new RepLine(tlmCell.penProp_LineBottom, rPosEnd - rPosStart, -rTest);
          container_Cur.AddLT(rPosStart, tlmCell.rPosMarginBottom, repLine);
          iCol = iColEnd;
        }
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method can be used to optimize the top line.</summary>
    /// <param name="bOrderTop">Order</param>
    /// <param name="cell">Start cell</param>
    /// <param name="iColEnd">End column</param>
    private void OptimizeTopLine(Boolean bOrderTop, TlmCell tlmCell, ref Int32 iColEnd) {
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method can be used to optimize the bottom line.</summary>
    /// <param name="bOrderTop">Order</param>
    /// <param name="tlmCell">Start cell</param>
    /// <param name="iColEnd">End column</param>
    private void OptimizeBottomLine(Boolean bOrderTop, TlmCell tlmCell, ref Int32 iColEnd) {
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Writes the committed vertical lines.</summary>
    /// <param name="bOrderTop">Order</param>
    private void WriteCommittedVertLines(Boolean bOrderTop) {
      for (Int32 iCol = 0;  iCol < list_TlmColumn.Count;  iCol++) {
        TlmColumn col = list_TlmColumn[iCol];
        // left line
        for (Int32 iRow = 0;  iRow <= tlmRow_Committed.iIndex;  iRow++) {
          TlmRow row = list_TlmRow[iRow];
          TlmCell tlmCell = row.aTlmCell[iCol];
          if (tlmCell == null || tlmCell.bVisibleLineLeft(iRow) != bOrderTop) {
            continue;
          }
          if (tlmCell.tlmColumn_Start.iIndex != iCol || tlmCell.penProp_LineLeft == null) {
            iRow = tlmCell.tlmRow_End.iIndex;
            continue;
          }
          Int32 iRowEnd = iRow;
          OptimizeLeftLine(bOrderTop, tlmCell, ref iRowEnd);
          Double rPosStart = (iRow == tlmCell.tlmRow_Start.iIndex ? tlmCell.rPosMarginTop : row.rPosTop);
          TlmRow row_End = list_TlmRow[iRowEnd];
          TlmCell cell_End = row_End.aTlmCell[iCol];
          Double rPosEnd = (iRow == row_End.iIndex ? cell_End.rPosMarginBottom : row_End.rPosBottom);
          RepLine repLine = new RepLine(tlmCell.penProp_LineLeft, rTest, rPosEnd - rPosStart);
          container_Cur.AddLT(tlmCell.rPosMarginLeft, rPosStart, repLine);
          iRow = iRowEnd;
        }

        // right line
        for (Int32 iRow = 0;  iRow <= tlmRow_Committed.iIndex;  iRow++) {
          TlmRow row = list_TlmRow[iRow];
          TlmCell tlmCell = row.aTlmCell[iCol];
          if (tlmCell == null || tlmCell.bVisibleLineRight(iRow) != bOrderTop) {
            continue;
          }
          if (tlmCell.tlmColumn_End.iIndex != iCol || tlmCell.penProp_LineRight == null) {
            iRow = tlmCell.tlmRow_End.iIndex;
            continue;
          }
          Int32 iRowEnd = iRow;
          OptimizeRightLine(bOrderTop, tlmCell, ref iRowEnd);
          Double rPosStart = (iRow == tlmCell.tlmRow_Start.iIndex ? tlmCell.rPosMarginTop : row.rPosTop);
          TlmRow row_End = list_TlmRow[iRowEnd];
          TlmCell cell_End = row_End.aTlmCell[iCol];
          Double rPosEnd = (iRow == row_End.iIndex ? cell_End.rPosMarginBottom : row_End.rPosBottom);
          RepLine repLine = new RepLine(tlmCell.penProp_LineRight, -rTest, rPosEnd - rPosStart);
          container_Cur.AddLT(tlmCell.rPosMarginRight, rPosStart, repLine);
          iRow = iRowEnd;
        }
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method can be used to optimize the left line.</summary>
    /// <param name="bOrderTop">Order</param>
    /// <param name="tlmCell">Start cell</param>
    /// <param name="iRowEnd">End row</param>
    private void OptimizeLeftLine(Boolean bOrderTop, TlmCell tlmCell, ref Int32 iRowEnd) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method can be used to optimize the right line.</summary>
    /// <param name="bOrderTop">Order</param>
    /// <param name="tlmCell">Start cell</param>
    /// <param name="iRowEnd">End row</param>
    private void OptimizeRightLine(Boolean bOrderTop, TlmCell tlmCell, ref Int32 iRowEnd) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called before the report objects will be written to the container.</summary>
    internal virtual void OnBeforeWrite() {
    }

    //------------------------------------------------------------------------------------------30.10.2006
    /// <summary>Writes all committed objects to the report.</summary>
    /// <param name="bLastContainer"><b>true</b> if the last container must be processed</param>
    private void WriteCommittedReportObjects(Boolean bLastContainer) {
      if (tlmRow_Committed == null) {
        return;
      }

      // check whether the last row must be splitted
      Boolean bSplitRequired = false;
      foreach (TlmCell tlmCell in _tlmRow_Committed.tlmCellEnumerator) {
        if (!bFullRowCommitted) {
          if (tlmCell.tlmColumn_Start.iRepObjCommitted < tlmCell.iRepObjCount) {  // not all objects are committed
            bSplitRequired = true;
            break;
          }
        }
        if (_tlmRow_Committed.iIndex < tlmCell.tlmRow_End.iIndex) {  // split vertically merged cell
          bSplitRequired = true;
          break;
        }
      }

      if (bSplitRequired) {  // split row
        TlmRow tlmRow_2 = new TlmRow(this);

        foreach (TlmColumn tlmColumn in list_TlmColumn) {
          TlmCell tlmCell = _tlmRow_Committed.aTlmCell[tlmColumn];
          if (tlmCell.tlmColumn_Start.iIndex == tlmColumn.iIndex) {  // create new cell
            tlmCell.SplitCell(tlmRow_2);
          }
          else {  // merge cell
            Debug.Assert(tlmCell.tlmColumn_Start.iIndex < tlmColumn.iIndex);
            tlmRow_2.aTlmCell.SetCell(tlmColumn.iIndex, tlmRow_2.aTlmCell[tlmCell.tlmColumn_Start]);
          }
        }
        InsertRow(tlmRow_Committed, tlmRow_2);
        tlmRow_Committed.Close();
      }

      // set position
      tlmRow_Committed.rPosBottom = tlmRow_Committed.rCalculateBottomPos();
      if (tlmHeightMode == TlmHeightMode.Static || (tlmHeightMode == TlmHeightMode.AdjustLast && !bLastContainer)) {
        double rDelta = container_Cur.rHeight - tlmRow_Committed.rPosBottom;  // vertically aligned contents must have the bottom position - indent
        foreach (TlmCell tlmCell in tlmRow_Committed.tlmCellEnumerator) {
          tlmCell.rIndentBottom += rDelta;
        }
        tlmRow_Committed.rPosBottom = container_Cur.rHeight;
      }

      OnBeforeWrite();

      // background
      Byte[,] aaiDone = new Byte[tlmRow_Committed.iIndex + 1, list_TlmColumn.Count];  // 0:init;  1:temp;  2:done
      for (Int32 iRow = 0;  iRow <= tlmRow_Committed.iIndex;  iRow++) {
        TlmRow tlmRow = list_TlmRow[iRow];
        for (Int32 iCol = 0;  iCol < list_TlmColumn.Count;  iCol++) {
          if (aaiDone[iRow, iCol] == 2) {  // background of this cell has been created before
            continue;
          }
          TlmCell tlmCell = tlmRow.aTlmCell[iCol];
          if (tlmCell == null || tlmCell.brushProp_Back == null) {
            continue;
          }
          Int32 iColEnd = iCol;
          Int32 iRowEnd = iRow;
          OptimizeBackground(aaiDone, ref iRowEnd, ref iColEnd);
          Double rPosX1 = (iCol == tlmCell.tlmColumn_Start.iIndex ? tlmCell.rPosMarginLeft : list_TlmColumn[iCol].rPosX);
          Double rPosY1 = (iRow == tlmCell.tlmRow_Start.iIndex ? tlmCell.rPosMarginTop : tlmRow.rPosTop);
          TlmRow row_End = list_TlmRow[iRowEnd];
          TlmColumn col_End = list_TlmColumn[iColEnd];
          TlmCell cell_End = row_End.aTlmCell[iColEnd];
          Double rPosX2 = (iColEnd == col_End.iIndex ? cell_End.rPosMarginRight : col_End.rPosX + col_End.rWidth);
          Double rPosY2 = (iRowEnd == row_End.iIndex ? cell_End.rPosMarginBottom : row_End.rPosBottom);
          #if (Test)
            container_Cur.AddLT(rPosX1, rPosY1, new RepLine(pp_Test, rPosX2 - rPosX1, rPosY2 - rPosY1));
          #else
          RepRect repRect = new RepRect(tlmCell.brushProp_Back, rPosX2 - rPosX1, rPosY2 - rPosY1);
          container_Cur.AddLT(rPosX1, rPosY1, repRect);
          #endif
        }
      }

      // contents
      for (Int32 iRow = 0;  iRow <= tlmRow_Committed.iIndex;  iRow++) {
        TlmRow tlmRow = list_TlmRow[iRow];
        foreach (TlmCell tlmCell in tlmRow.tlmCellEnumerator) {
          Double rOfsY = 0;
#warning must be tested for vertical text
          if (!RT.bEquals(tlmCell.rAngle, -90, 0.001)) {
            Debug.Assert(!Double.IsNaN(tlmCell.tlmRow_End.rPosBottom));
            Double rMaxY = tlmCell.rCalculateMaxY(false);
            rOfsY = (tlmCell.tlmRow_End.rPosBottom - tlmRow.rPosTop - rMaxY) * tlmCell.rAlignV;
          }
          Int32 iRepObjCount = tlmCell.iRepObjCount;
          for (Int32 iRepObj = 0;  iRepObj < iRepObjCount;  iRepObj++) {
            RepObj repObj = tlmCell.repObj_Get(iRepObj);
            repObj.matrixD.rDX += tlmCell.rPosMarginLeft;
            repObj.matrixD.rDY += tlmCell.rPosMarginTop + rOfsY;
            container_Cur.Add(repObj);
          }

          #if (Test)
          Double rX1 = tlmCell.rPosMarginLeft + tlmCell.rIndentLeft;
          Double rY1 = tlmCell.rPosMarginTop + tlmCell.rIndentTop;
          Double rX2 = tlmCell.rPosMarginRight - tlmCell.rIndentRight;
          Double rY2 = tlmCell.rPosMarginBottom - tlmCell.rIndentBottom;
          container_Cur.AddLT(rX1, rY1, new RepRect(pp_Test, rX2 - rX1, rY2 - rY1));
          #endif
        }
      }

      // horizontal lines
      WriteCommittedHorLines(false);
      WriteCommittedHorLines(true);

      WriteCommittedVertLines(false);
      WriteCommittedVertLines(true);

      // Remove committed report objects
      list_TlmRow.RemoveRange(0, tlmRow_Committed.iIndex + 1);
      _tlmRow_Committed = null;

      // Reset index of rows
      Int32 iIndex = 0;
      foreach (TlmRow row in list_TlmRow) {
        row.iIndex = iIndex++;
        //row.rPosTop = 0;
        //row.rPosBottom = Double.NaN;
      }

      _container_Cur = null;
    }

    //----------------------------------------------------------------------------------------------------x
    #region Optimiation !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //----------------------------------------------------------------------------------------------------x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will optimize the background rectangles.</summary>
    /// <param name="aaiDone">Status of the cells</param>
    /// <param name="iRowEnd">End row</param>
    /// <param name="iColEnd">End column</param>
    internal protected virtual void OptimizeBackground(Byte[,] aaiDone, ref Int32 iRowEnd, ref Int32 iColEnd) {
      TlmRow row = list_TlmRow[iRowEnd];
      TlmCell tlmCell = row.aTlmCell[iColEnd];
      for (Int32 iR = iRowEnd;  iR <= tlmCell.tlmRow_End.iIndex;  iR++) {
        for (Int32 iC = iColEnd;  iC <= tlmCell.tlmColumn_End.iIndex;  iC++) {
          aaiDone[iR, iC] = 2;
        }
      }
      iRowEnd = tlmCell.tlmRow_End.iIndex;
      iColEnd = tlmCell.tlmColumn_End.iIndex;

    }
    #endregion

    //------------------------------------------------------------------------------------------03.10.2006
    /// <summary>Commits as many objects as there can be placed into the current container.</summary>
    /// <remfarks>If not all objects fit into the current container the method stops and returns true.</remarks>
    /// <returns>True if a new container is required</returns>
    private Boolean bCommit() {
      // Get the index and top position of the first uncommitted row that is not closed
      Int32 iRow = 0;
      Double rY = 0;
      if (_tlmRow_Committed != null) {
        iRow = _tlmRow_Committed.iIndex;
        if (bFullRowCommitted) {
          Debug.Assert(_tlmRow_Committed.status == TlmRow.Status.Closed);
          iRow++;
        }
        if (iRow > 0) {
          rY = list_TlmRow[iRow - 1].rPosBottom;
        }
      }
      Debug.Assert(iRow <= list_TlmRow.Count);
      if (iRow >= list_TlmRow.Count) {  // no more rows
        return false;
      }

      // Set vertical position of committed rows and close all rows except the last one
      TlmRow tlmRow;
      while (true) {
        tlmRow = list_TlmRow[iRow];
        tlmRow.rPosTop = rY;
        rY = tlmRow.rCalculateBottomPos();
        if (rY > container_Cur.rHeight) {  // new container required
          break;
        }

        if (!Double.IsNaN(tlmRow.rPreferredHeight)) {  // check preferred row height
          Double rRowHeight = rY - tlmRow.rPosTop;
          if (tlmRow.rPreferredHeight > rRowHeight) {  // row can be made higher
            if (tlmRow.rPosTop + tlmRow.rPreferredHeight > container_Cur.rHeight) {  // limit height to container
              rY = container_Cur.rHeight;
            }
            else {  // set preferred row height
              rY = tlmRow.rPosTop + tlmRow.rPreferredHeight;
            }
          }
        }
        tlmRow.rPosBottom = rY;

        iRow++;
        if (iRow >= list_TlmRow.Count) {  // last row
          _rCurY = rY;
          _tlmRow_Committed = list_TlmRow[iRow - 1];
          if (_tlmRow_Committed.status == TlmRow.Status.Closed) {  // full row committed
            bFullRowCommitted = true;
          }
          else {  // current set of objects committed
            bFullRowCommitted = false;
            foreach (TlmCell tlmCell in _tlmRow_Committed.tlmCellEnumerator) {
              tlmCell.tlmColumn_Start.iRepObjCommitted = tlmCell.iRepObjCount;
            }
          }
          return false;  // more objects can be placed into this container
        }
        tlmRow.Close();
      }

      // new container required
      BreakMode breakMode_Cur = breakMode;
      if (breakMode_Cur == BreakMode.Commit) {
        if (tlmRow_Committed != null) {
          return true;  // print committed objects and create new container
        }
        if (breakMode_Overflow == BreakMode.Commit) {
          throw new ReportException("Overflow break mode 'Commit' is not allowed if the standard break mode is 'Commit'");
        }
        breakMode_Cur = breakMode_Overflow;
      }

      if (breakMode_Cur == BreakMode.Row) {
        if (iRow > 0) {
          _tlmRow_Committed = list_TlmRow[iRow - 1];
          bFullRowCommitted = true;
          return true;  // print committed objects and create new container
        }
        if (breakMode == BreakMode.Row && breakMode_Overflow != BreakMode.Line) {
          throw new ReportException("Overflow break mode 'Commit' or 'Row' are not allowed if the standard break mode is 'Row'");
        }
      }

      // BreakMode.Line
      bFullRowCommitted = true;  // will be adjusted in the following loop
      Boolean bObjectsAvailable = false;
      foreach (TlmCell tlmCell in tlmRow.tlmCellEnumerator) {
        Int32 iRepObj = 0;
        while (true) {
          if (iRepObj >= tlmCell.iRepObjCount) {  // no more objects
            tlmCell.tlmColumn_Start.iRepObjCommitted = iRepObj;
            break;
          }
          RepObj repObj = tlmCell.repObj_Get(iRepObj);
          if (tlmCell.tlmRow_Start.rPosTop + repObj.rY + repObj.rHeight > container_Cur.rHeight) {  // object too height
            if (iRow > 0 || iRepObj > 0) {  // at least one object must be in the container
              tlmCell.tlmColumn_Start.iRepObjCommitted = iRepObj ;
              bFullRowCommitted = false;
              break;
            }
          }
          bObjectsAvailable = true;
          iRepObj++;
        }
      }
      if (!bObjectsAvailable) {
        _tlmRow_Committed = list_TlmRow[iRow - 1];
        bFullRowCommitted = true;
        return true;  // print committed objects and create new container
      }
      _tlmRow_Committed = tlmRow;
      if (bFullRowCommitted && iRow <= list_TlmRow.Count) {  // can happen if a RepObj is higher than the container; this object cannot be splitted
        return false;
      }
      return true;
    }

    //------------------------------------------------------------------------------------------xx.10.2006
    /// <summary>Commits the current contents of the table layout manager.</summary>
    /// <remarks>
    /// All rows will be closed except the last one.
    /// This method can be used to allow a page break at the current position.
    /// </remarks>
    /// <include file='LayoutManager\TlmBase.cs.xml' path='documentation/class[@name="TlmBase.Commit"]/*'/>
    public void Commit() {
      while (bCommit()) {  // true if a new container is required
        WriteCommittedReportObjects(false);
        CreateNewContainer();
      }
    }

    //------------------------------------------------------------------------------------------02.11.2006
    /// <summary>Writes all objects to the report.</summary>
    private void WriteAll() {
      Commit();  // commit all objects, only the last container will be kept open
      WriteCommittedReportObjects(true);  // write the last container to the report
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Container
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Default height of the table (points, 1/72 inch)</summary>
    public Double rContainerHeight = 72 * 1000;

    /// <summary>Default height of the table (mm)</summary>
    public Double rContainerHeightMM {
      get { return RT.rMMFromPoint(rContainerHeight); }
      set { rContainerHeight = RT.rPointFromMM(value); }
    }

    private Container _container_Cur;
    /// <summary>Current container</summary>
    public Container container_Cur {
      get { return _container_Cur; }
    }

    //------------------------------------------------------------------------------------------06.01.2004
    /// <summary>Table height mode</summary>
    public TlmHeightMode tlmHeightMode = TlmHeightMode.Adjust;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Provides data for the NewContainer event</summary>
    public class NewContainerEventArgs : EventArgs {
      /// <summary>Table layout manager</summary>
      public readonly TlmBase tlm;

      /// <summary>New container</summary>
      public readonly Container container;

      /// <summary>Creates a data object for the NewContainer event.</summary>
      /// <param name="tlmBase">Table layout manager</param>
      /// <param name="container">New container: this container must be added to a page or a container.</param>
      internal NewContainerEventArgs(TlmBase tlmBase, Container container) {
        this.tlm = tlmBase;
        this.container = container;
      }
    }

    /// <summary>Represents the method that will handle the NewContainer event.</summary>
    public delegate void NewContainerEventHandler(Object oSender, NewContainerEventArgs ea);

    /// <summary>Occurs when a new container must be created.</summary>
    public event NewContainerEventHandler eNewContainer;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Raises the NewContainer event.</summary>
    /// <param name="ea">Event argument</param>
    internal protected virtual void OnNewContainer(NewContainerEventArgs ea) {
      if (eNewContainer != null) {
        eNewContainer(this, ea);
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new container.</summary>
    private void CreateNewContainer() {
      if (_container_Cur == null) {
        _container_Cur = new StaticContainer(rWidth, rContainerHeight);
        NewContainerEventArgs ea = new NewContainerEventArgs(this, _container_Cur);
        OnNewContainer(ea);
        //if (_container_Cur.container == null) {
        //  throw new ReportException("The current container has not been added to the page.");
        //}
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will create a new container that will be added to the parent container at the specified position.</summary>
    /// <param name="container_Parent">Parent container</param>
    /// <param name="rX">X-coordinate of the new container (points, 1/72 inch)</param>
    /// <param name="rY">Y-coordinate of the new container (points, 1/72 inch)</param>
    /// <exception cref="ReportException">The layout manager status is not 'Init'</exception>
    public Container container_Create(Container container_Parent, Double rX, Double rY) {
      if (status != Status.Init && status != Status.Closed) {
        throw new ReportException("The layout manager must be in initialization mode or it must be closed; cannot create a new container.");
      }
      if (_container_Cur != null) {
        throw new ReportException("The container has been defined already.");
      }
      CreateNewContainer();
      if (container_Parent != null) {
        container_Parent.Add(rX, rY, _container_Cur);
      }
      return _container_Cur;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will creates a new container that will be added to the parent container at the specified position (metric version).</summary>
    /// <param name="container_Parent">Parent container</param>
    /// <param name="rX_MM">X coordinate of the new container (mm)</param>
    /// <param name="rY_MM">Y coordinate of the new container (mm)</param>
    public Container container_CreateMM(Container container_Parent, Double rX_MM, Double rY_MM) {
      return container_Create(container_Parent, RT.rPointFromMM(rX_MM), RT.rPointFromMM(rY_MM));
    }
    #endregion

    //------------------------------------------------------------------------------------------06.01.2004
    #region CellCreateType
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Cell creation type</summary>
    public enum CellCreateType {
      /// <summary>A new cell will be created for this row</summary>
      New,
      /// <summary>The cell will be merged with the previous cell of the column</summary>
      MergedV,
      /// <summary>The cell will be merged with the left cell of the row</summary>
      MergedH,
      /// <summary>The column will have no cell in this row</summary>
      Empty
    }
    #endregion

    //------------------------------------------------------------------------------------------16.02.2006
    #if Compatible_0_8
    //----------------------------------------------------------------------------------------------------

    public enum TextMode {
      /// <summary>Single line text mode, text is trimmed to the nearest character and an ellipsis is inserted at the end of the line.</summary>
      EllipsisCharacter,
      /// <summary>Multiline text mode</summary>
      MultiLine,
      /// <summary>Multiline text mode, each line committed</summary>
      SingleMultiLine,
      /// <summary>Fallback: text mode of the fallback cell definition</summary>
      FallBack
    }

    [Obsolete("use 'TlmHeightMode'")]
    public enum TableHeight {
      /// <summary>Adjust height of last container</summary>
      AdjustLast,
      /// <summary>Adjust height of each container</summary>
      Adjust,
      /// <summary>No adjustment</summary>
      Static
    }

    /// <summary>Table height mode</summary>
    [Obsolete("use 'tlmHeightMode'")]
    public TableHeight tableHeight {
      get {
        switch (tlmHeightMode) {
          case TlmHeightMode.Adjust: { return TableHeight.Adjust; }
          case TlmHeightMode.AdjustLast: { return TableHeight.AdjustLast; }
        }
        return TableHeight.Static;
      }
      set {
        switch (value) {
          case TableHeight.Adjust: { tlmHeightMode = TlmHeightMode.Adjust; break; }
          case TableHeight.AdjustLast: { tlmHeightMode = TlmHeightMode.AdjustLast; break; }
          default: { tlmHeightMode = TlmHeightMode.Static; break; }
        }
      }
    }
    /// <summary>Table height mode (VB version)</summary>
    [Obsolete("use 'tlmHeightMode'")]
    public TableHeight _tableHeight {
      get { return tableHeight; }
      set { tableHeight = value; }
    }

    //------------------------------------------------------------------------------------------06.01.2004
    /// <summary>Definition of the default properties of a column of this table.</summary>
    public class ColumnDef : TlmColumnDef {
      //------------------------------------------------------------------------------------------06.01.2004
      /// <summary>Creates a definition structure for the default values of a column of this table.</summary>
      internal ColumnDef() {
      }
    }

    //------------------------------------------------------------------------------------------06.01.2004
    /// <summary>Definition of the default properties of a row of this table.</summary>
    public class RowDef : TlmRowDef {
      //------------------------------------------------------------------------------------------06.01.2004
      /// <summary>Creates a definition structure for the default values of a row of this table.</summary>
      internal RowDef() {
      }
    }

    /// <summary>Definition of the default properties of a cell of this table.</summary>
    [Obsolete("use 'tlmCellDef_Default'")]
    public TlmCellDef cellDef {
      get { return tlmCellDef_Default; }
    }
    /// <summary>Definition of the default properties of a cell of this table (VB version)</summary>
    [Obsolete("use 'tlmCellDef_Default'")]
    public TlmCellDef _cellDef {
      get { return tlmCellDef_Default; }
    }

    /// <summary>Definition of the default properties of a column of this table (VB version)</summary>
    [Obsolete("use 'tlmColumnDef_Default'")]
    public TlmColumnDef columnDef {
      get { return tlmColumnDef_Default; }
    }

    /// <summary>Definition of the default properties of a column of this table (VB version)</summary>
    [Obsolete("use 'tlmColumnDef_Default'")]
    public TlmColumnDef _columnDef {
      get { return tlmColumnDef_Default; }
    }

    /// <summary>Definition of the default properties of a row of this table (VB version)</summary>
    [Obsolete("use 'tlmRowDef_Default'")]
    public TlmRowDef rowDef {
      get { return tlmRowDef_Default; }
    }

    /// <summary>Definition of the default properties of a row of this table (VB version)</summary>
    [Obsolete("use 'tlmRowDef_Default'")]
    public TlmRowDef _rowDef {
      get { return tlmRowDef_Default; }
    }

    [Obsolete("use 'fontProp_Header'")]
    public FontProp fp_Header {
      get { return fontProp_Header; }
      set { fontProp_Header = value; }
    }
    #endif
  }

  //------------------------------------------------------------------------------------------06.01.2004
  #region TlmTextMode
  //----------------------------------------------------------------------------------------------------x

  /// <summary>Text mode</summary>
  public enum TlmTextMode {
    /// <summary>Single line text mode, text is trimmed to the nearest character and an ellipsis is inserted at the end of the line.</summary>
    EllipsisCharacter,
    /// <summary>Multiline text mode</summary>
    MultiLine,
    /// <summary>Multiline text mode, each line committed</summary>
    SingleMultiLine,
    /// <summary>Fallback: text mode of the fallback cell definition</summary>
    FallBack
  }
  #endregion

  //------------------------------------------------------------------------------------------06.01.2004
  #region TlmHeightMode
  //----------------------------------------------------------------------------------------------------x

  /// <summary>Table height mode</summary>
  public enum TlmHeightMode {
    /// <summary>Adjust height of last container</summary>
    AdjustLast,
    /// <summary>Adjust height of each container</summary>
    Adjust,
    /// <summary>No adjustment</summary>
    Static
  }
  #endregion
}
