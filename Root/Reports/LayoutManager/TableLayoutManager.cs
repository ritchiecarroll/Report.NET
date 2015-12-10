using System;
using System.Diagnostics;
using System.Drawing;

// Creation date: 05.11.2002
// Checked: 30.05.2003
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
  /// <summary>Table Layout Manager</summary>
  public class TableLayoutManager : TlmBase {
    //====================================================================================================x
    /// <summary>Definition of the default properties of a cell of this table</summary>
    public readonly TlmCellDef tlmCellDef_Header;

    /// <summary>Definition of the default properties of a row of this table</summary>
    public readonly TlmRowDef tlmRowDef_Header;
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new table layout manager.</summary>
    /// <param name="report">Report of this table layout manager</param>
    public TableLayoutManager(Report report) : base(report) {
      tlmHeightMode = TlmHeightMode.Static;

      // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -x
      PenProp penProp_Solid = new PenProp(report, 0.5, Color.Black);
      tlmCellDef_Default.penProp_LineV = penProp_Solid;

      // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -x
      tlmColumnDef_Default.penProp_BorderH = penProp_Solid;

      // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -x
      tlmCellDef_Header = new TlmCellDef();
      tlmRowDef_Header = new TlmRowDef();

      tlmCellDef_Header.rAlignH = RepObj.rAlignLeft;
      tlmCellDef_Header.rAlignV = RepObj.rAlignTop;
      tlmCellDef_Header.rAngle = 0;
      tlmCellDef_Header.tlmTextMode = TlmTextMode.MultiLine;
      tlmCellDef_Header.rLineFeed = 72.0 / 6;

      tlmCellDef_Header.rMargin = 0;
      tlmCellDef_Header.rIndentH_MM = 1;
      tlmCellDef_Header.rIndentV_MM = 2;

      tlmCellDef_Header.brushProp_Back = new BrushProp(report, Color.FromArgb(220, 220, 220));
      tlmCellDef_Header.penProp_Line = penProp_Solid;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new table layout manager.</summary>
    /// <param name="fp_Header">Font property of the header</param>
    public TableLayoutManager(FontProp fontProp_Header) : this(fontProp_Header.fontDef.report) {
      this.fontProp_Header = fontProp_Header;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a column definition to the table layout manager</summary>
    [Obsolete("set 'tableHeight = TableHeight...' instead of calling 'AdjustHeight()'")]
    public void AdjustHeight() {
      tlmHeightMode = TlmHeightMode.AdjustLast;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    public CellCreateType[] aCellCreateType = null;
    
    /// <summary>Creates the table header.</summary>
    private void CreateHeader(Container cont) {
      TlmRow row = tlmRow_New((TlmRow)null, aCellCreateType);
      row.bAutoCommit = false;
      row.rPreferredHeight = tlmRowDef_Header.rPreferredHeight;

      foreach (TlmColumn col in list_TlmColumn) {
        TlmCell cell = row.aTlmCell[col.iIndex];

        TlmCellDef hd_Base = tlmCellDef_Header;
        TlmCellDef hd_Col = col.tlmCellDef_Header;
        
        cell.rAlignH = (Double.IsNaN(hd_Col.rAlignH) ? hd_Base.rAlignH : hd_Col.rAlignH);
        cell.rAlignV = (Double.IsNaN(hd_Col.rAlignV) ? hd_Base.rAlignV : hd_Col.rAlignV);
        cell.rAngle = (Double.IsNaN(hd_Col.rAngle) ? hd_Base.rAngle : hd_Col.rAngle);
        cell.tlmTextMode = (hd_Col.tlmTextMode == TlmTextMode.FallBack ? hd_Base.tlmTextMode : hd_Col.tlmTextMode);
        cell.rLineFeed = (Double.IsNaN(hd_Col.rLineFeed) ? hd_Base.rLineFeed : hd_Col.rLineFeed);

        cell.rMarginLeft = (Double.IsNaN(hd_Col.rMarginLeft) ? hd_Base.rMarginLeft : hd_Col.rMarginLeft);
        cell.rMarginRight = (Double.IsNaN(hd_Col.rMarginRight) ? hd_Base.rMarginRight : hd_Col.rMarginRight);
        cell.rMarginTop = (Double.IsNaN(hd_Col.rMarginTop) ? hd_Base.rMarginTop : hd_Col.rMarginTop);
        cell.rMarginBottom = (Double.IsNaN(hd_Col.rMarginBottom) ? hd_Base.rMarginBottom : hd_Col.rMarginBottom);

        cell.rIndentLeft = (Double.IsNaN(hd_Col.rIndentLeft) ? hd_Base.rIndentLeft : hd_Col.rIndentLeft);
        cell.rIndentRight = (Double.IsNaN(hd_Col.rIndentRight) ? hd_Base.rIndentRight : hd_Col.rIndentRight);
        cell.rIndentTop = (Double.IsNaN(hd_Col.rIndentTop) ? hd_Base.rIndentTop : hd_Col.rIndentTop);
        cell.rIndentBottom = (Double.IsNaN(hd_Col.rIndentBottom) ? hd_Base.rIndentBottom : hd_Col.rIndentBottom);

        cell.brushProp_Back = (Object.ReferenceEquals(hd_Col.brushProp_Back, BrushProp.bp_Null) ? hd_Base.brushProp_Back : hd_Col.brushProp_Back);

        cell.penProp_LineLeft = (Object.ReferenceEquals(hd_Col.penProp_LineLeft, PenProp.penProp_Null) ? hd_Base.penProp_LineLeft : hd_Col.penProp_LineLeft);
        cell.penProp_LineRight = (Object.ReferenceEquals(hd_Col.penProp_LineRight, PenProp.penProp_Null) ? hd_Base.penProp_LineRight : hd_Col.penProp_LineRight);
        cell.penProp_LineTop = (Object.ReferenceEquals(hd_Col.penProp_LineTop, PenProp.penProp_Null) ? hd_Base.penProp_LineTop : hd_Col.penProp_LineTop);
        cell.penProp_LineBottom = (Object.ReferenceEquals(hd_Col.penProp_LineBottom, PenProp.penProp_Null) ? hd_Base.penProp_LineBottom : hd_Col.penProp_LineBottom);

        cell.iOrderLineLeft = (hd_Col.iOrderLineLeft == Int32.MinValue ? hd_Base.iOrderLineLeft : hd_Col.iOrderLineLeft);
        cell.iOrderLineRight = (hd_Col.iOrderLineRight == Int32.MinValue ? hd_Base.iOrderLineRight : hd_Col.iOrderLineRight);
        cell.iOrderLineTop = (hd_Col.iOrderLineTop == Int32.MinValue ? hd_Base.iOrderLineTop : hd_Col.iOrderLineTop);
        cell.iOrderLineBottom = (hd_Col.iOrderLineBottom == Int32.MinValue ? hd_Base.iOrderLineBottom : hd_Col.iOrderLineBottom);

        if (col.sHeader != null) {
          cell.Add(new RepString(col.fontProp_Header, col.sHeader));
        }
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>The layout manager will be closed.</summary>
    [Obsolete("use 'Close()' or 'using (...)' instead of 'Done()'")]
    public void Done() {
      Close();
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Raises the NewContainer event.</summary>
    /// <param name="ea">Event argument</param>
    internal protected override void OnNewContainer(NewContainerEventArgs ea) {
      base.OnNewContainer(ea);
      CreateHeader(container_Cur);
    }

    //----------------------------------------------------------------------------------------------------x
    // Virtual Methods
    //----------------------------------------------------------------------------------------------------x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called before the row will be closed.</summary>
    /// <param name="row">Row that will be closed</param>
    internal protected override void OnClosingRow(TlmRow row) {
      if (row.iIndex != 1) {
        return;
      }
      for (Int32 iCol = 0;  iCol < list_TlmColumn.Count;  iCol++) {
        TlmCell cell = row.aTlmCell[iCol];
        if (cell.tlmColumn_Start.iIndex != iCol) {
          continue;
        }
        TlmColumn col = list_TlmColumn[iCol];
        if (!Double.IsNaN(col.rBorderTop)) {
          cell.rMarginTop = col.rBorderTop;
        }
        if (!Object.ReferenceEquals(col.penProp_BorderTop, PenProp.penProp_Null)) {
          cell.penProp_LineTop = col.penProp_BorderTop;
        }
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called before the report objects will be written to the container.</summary>
    internal override void OnBeforeWrite() {
      for (Int32 iCol = 0;  iCol < list_TlmColumn.Count;  iCol++) {
        TlmCell cell = tlmRow_Committed.aTlmCell[iCol];
        if (cell.tlmColumn_Start.iIndex != iCol) {
          continue;
        }
        TlmColumn col = list_TlmColumn[iCol];
        if (!Double.IsNaN(col.rBorderBottom)) {
          cell.rMarginBottom = col.rBorderBottom;
        }
        if (!Object.ReferenceEquals(col.penProp_BorderBottom, PenProp.penProp_Null)) {
          cell.penProp_LineBottom = col.penProp_BorderBottom;
        }
      }
    }

    #if Compatible_0_8x
    //====================================================================================================x
    /// <summary>Column definition.</summary>
    public class Column : TlmColumn {
      /// <summary>Horizontal alignment</summary>
      [Obsolete("use 'cellDef.rAlignH = ...'")]
      public Double rAlignH {
        set { tlmCellDef_Default.rAlignH = value; }
      }

      /// <summary>Vertical alignment</summary>
      [Obsolete("use 'cellDef.rAlignV = ...'")]
      public Double rAlignV {
        set { tlmCellDef_Default.rAlignV = value; }
      }

      /// <summary>Left margin within the column</summary>
      [Obsolete("use 'cellDef.rMarginLeft = ...'")]
      public Double rMarginLeft {
        set { tlmCellDef_Default.rMarginLeft = value; }
      }

      /// <summary>Right margin within the column</summary>
      [Obsolete("use 'cellDef.rMarginRight = ...'")]
      public Double rMarginRight {
        set { tlmCellDef_Default.rMarginRight = value; }
      }

      /// <summary>Multiline mode: true if the column supports automatic multiline text mode; otherwise, false.</summary>
      [Obsolete("use 'col.cellDef.textMode = TableLayoutManager.TextMode.MultiLine'")]
      public Boolean bMultiline {
        set { tlmCellDef_Default.tlmTextMode = (value ? TlmTextMode.MultiLine : TlmTextMode.EllipsisCharacter); }
      }

      //----------------------------------------------------------------------------------------------------x
      /// <summary>Creates a column definition object.</summary>
      /// <param name="tlm">Table layout manager of this column</param>
      /// <param name="sHeader">Header of the column</param>
      /// <param name="rWidth">Width of the column</param>
      public Column(TableLayoutManager tlm, String sHeader, Double rWidth) : base(tlm, sHeader, rWidth) {
      }
    }

    //====================================================================================================x
    /// <summary>Column definition with metric values.</summary>
    public class ColumnMM : Column {
      /// <summary>Creates a column definition object with metric values.</summary>
      /// <param name="tlm">Table layout manager of this column</param>
      /// <param name="sHeader">Header of the column</param>
      /// <param name="rWidthMM">Width of the column</param>
      public ColumnMM(TableLayoutManager tlm, String sHeader, Double rWidthMM)
        : base(tlm, sHeader, RT.rPointFromMM(rWidthMM)) {
      }
    }

    [Obsolete("use 'tlmCellDef_Header'")]
    public TlmCellDef headerCellDef {
      get { return tlmCellDef_Header; }
    }

    [Obsolete("use 'tlmCellDef_Header'")]
    public TlmCellDef _headerCellDef {
      get { return tlmCellDef_Header; }
    }

    [Obsolete("use 'tlmRowDef_Header'")]
    public TlmRowDef headerRowDef {
      get { return tlmRowDef_Header; }
    }

    [Obsolete("use 'tlmRowDef_Header'")]
    public TlmRowDef _headerRowDef {
      get { return tlmRowDef_Header; }
    }

    [Obsolete("use 'ScaleWidth(rWidth)'")]
    public Boolean bAutoColWidth {
      set {
        if (value) {
          ScaleWidth(rWidth);
        }
      }
    }

    [Obsolete("use 'tlmHeaderCellDef.pp_Line = ...'")]
    public PenProp pp_HeaderGrid {
      set { tlmCellDef_Header.penProp_Line = value; }
    }

    [Obsolete("use 'tlmHeaderCellDef.bp_Back = ...'")]
    public BrushProp bp_HeaderBack {
      set { tlmCellDef_Header.brushProp_Back = value; }
    }

    [Obsolete("use 'tlmHeaderCellDef.pp_LineV = ...' and 'cellDef.pp_LineV = ...'")]
    public PenProp pp_LineV {
      set { tlmCellDef_Header.penProp_LineV = value; tlmCellDef_Default.penProp_LineV = value; }
    }

    [Obsolete("use 'tlmHeaderCellDef.pp_LineH = ...' and 'columnDef.pp_BorderH = ...'")]
    public PenProp pp_LineH {
      set { tlmCellDef_Header.penProp_LineH = value; tlmColumnDef_Default.penProp_BorderH = value; }
    }

    [Obsolete("use 'cellDef.rMarginBottom = ...'")]
    public Double rRowBottomMargin {
      set { tlmCellDef_Default.rMarginBottom = value; }
    }
    #endif
  }
}

