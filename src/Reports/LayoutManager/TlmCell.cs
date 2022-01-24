using System;
using System.Collections;
using System.Diagnostics;

// Creation date: 05.11.2002
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
  /// <summary>Represents the contents and properties of a cell of the table layout manager</summary>
  /// <remarks>
  /// When a new row is initialized, a new cell will be created for each column according to the cell create type <see cref="TlmBase.CellCreateType"/>.
  /// The properties of the row will be set on the basis of the row default values of the table layout manager <see cref="TlmBase.rowDef">TlmRow.rowDef</see>.
  /// The properties of the cell will be set on the basis of the cell defaults that are defined in the corresponding column <see cref="TlmColumn.cellDef">TlmColumn.cellDef</see>.
  /// The column definition may contain fallback values.
  /// In that case the properties of the cell will be set on the basis of the cell default values of the table layout manager <see cref="TlmBase.cellDef">TlmBase.cellDef</see>.
  /// Whenever a new row has been initialized a new cell will be created for each column.
  /// The properties of the cell will be set on the basis of the cell defaults that are defined in the corresponding column (col.cellDef).
  /// The column definition may contain fallback values.
  /// In that case the properties of the cell will be set on the basis of the default values of the table layout manager (tlm.cellDef).
  /// </remarks>
  public sealed class TlmCell : TlmCellDef {
    //------------------------------------------------------------------------------------------03.11.2006
    //----------------------------------------------------------------------------------------------------x
    #region Constructor
    //----------------------------------------------------------------------------------------------------

    /// <summary>Start column</summary>
    internal readonly TlmColumn tlmColumn_Start;

    /// <summary>End column</summary>
    internal TlmColumn tlmColumn_End;

    /// <summary>Start row</summary>
    internal TlmRow tlmRow_Start;

    /// <summary>End row</summary>
    internal TlmRow tlmRow_End;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a cell definition object.</summary>
    /// <param name="tlmColumn_Start">Start column</param>
    /// <param name="tlmColumn_End">End column</param>
    /// <param name="tlmRow">Start row</param>
    internal TlmCell(TlmColumn tlmColumn_Start, TlmColumn tlmColumn_End, TlmRow tlmRow) {
      this.tlmColumn_Start = tlmColumn_Start;
      this.tlmColumn_End = tlmColumn_End;
      tlmRow_Start = tlmRow;
      tlmRow_End = tlmRow;

      TlmCellDef cd_Col = tlmColumn_Start.tlmCellDef_Default;
      TlmCellDef cd_Base = tlmBase.tlmCellDef_Default;

      rAlignH = (Double.IsNaN(cd_Col.rAlignH) ? cd_Base.rAlignH : cd_Col.rAlignH);
      rAlignV = (Double.IsNaN(cd_Col.rAlignV) ? cd_Base.rAlignV : cd_Col.rAlignV);
      rAngle = (Double.IsNaN(cd_Col.rAngle) ? cd_Base.rAngle : cd_Col.rAngle);
      tlmTextMode = (cd_Col.tlmTextMode == TlmTextMode.FallBack ? cd_Base.tlmTextMode : cd_Col.tlmTextMode);
      rLineFeed = (Double.IsNaN(cd_Col.rLineFeed) ? cd_Base.rLineFeed : cd_Col.rLineFeed);

      rMarginLeft = (Double.IsNaN(cd_Col.rMarginLeft) ? cd_Base.rMarginLeft : cd_Col.rMarginLeft);
      rMarginRight = (Double.IsNaN(cd_Col.rMarginRight) ? cd_Base.rMarginRight : cd_Col.rMarginRight);
      rMarginTop = (Double.IsNaN(cd_Col.rMarginTop) ? cd_Base.rMarginTop : cd_Col.rMarginTop);
      rMarginBottom = (Double.IsNaN(cd_Col.rMarginBottom) ? cd_Base.rMarginBottom : cd_Col.rMarginBottom);

      rIndentLeft = (Double.IsNaN(cd_Col.rIndentLeft) ? cd_Base.rIndentLeft : cd_Col.rIndentLeft);
      rIndentRight = (Double.IsNaN(cd_Col.rIndentRight) ? cd_Base.rIndentRight : cd_Col.rIndentRight);
      rIndentTop = (Double.IsNaN(cd_Col.rIndentTop) ? cd_Base.rIndentTop : cd_Col.rIndentTop);
      rIndentBottom = (Double.IsNaN(cd_Col.rIndentBottom) ? cd_Base.rIndentBottom : cd_Col.rIndentBottom);

      brushProp_Back = (Object.ReferenceEquals(cd_Col.brushProp_Back, BrushProp.bp_Null) ? cd_Base.brushProp_Back : cd_Col.brushProp_Back);

      penProp_LineLeft = (Object.ReferenceEquals(cd_Col.penProp_LineLeft, PenProp.penProp_Null) ? cd_Base.penProp_LineLeft : cd_Col.penProp_LineLeft);
      penProp_LineRight = (Object.ReferenceEquals(cd_Col.penProp_LineRight, PenProp.penProp_Null) ? cd_Base.penProp_LineRight : cd_Col.penProp_LineRight);
      penProp_LineTop = (Object.ReferenceEquals(cd_Col.penProp_LineTop, PenProp.penProp_Null) ? cd_Base.penProp_LineTop : cd_Col.penProp_LineTop);
      penProp_LineBottom = (Object.ReferenceEquals(cd_Col.penProp_LineBottom, PenProp.penProp_Null) ? cd_Base.penProp_LineBottom : cd_Col.penProp_LineBottom);

      iOrderLineLeft = (cd_Col.iOrderLineLeft == Int32.MinValue ? cd_Base.iOrderLineLeft : cd_Col.iOrderLineLeft);
      iOrderLineRight = (cd_Col.iOrderLineRight == Int32.MinValue ? cd_Base.iOrderLineRight : cd_Col.iOrderLineRight);
      iOrderLineTop = (cd_Col.iOrderLineTop == Int32.MinValue ? cd_Base.iOrderLineTop : cd_Col.iOrderLineTop);
      iOrderLineBottom = (cd_Col.iOrderLineBottom == Int32.MinValue ? cd_Base.iOrderLineBottom : cd_Col.iOrderLineBottom);
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    /// <summary>The cell will be closed.</summary>
    internal void Close() {
      status = Status.Closed;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the handle of the table layout manager.</summary>
    private TlmBase tlmBase {
      get { return tlmColumn_Start.tlmBase; }
    }

    //----------------------------------------------------------------------------------------------------x
    #region Status
    //----------------------------------------------------------------------------------------------------x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Status of the cell</summary>
    internal enum Status {
      /// <summary>Initialization mode</summary>
      Init,
      /// <summary>Cell prepared for use</summary>
      Open,
      /// <summary>Cell prepared for text contents</summary>
      OpenText,
      /// <summary>Cell closed</summary>
      Closed
    }

    /// <summary>Status of the cell</summary>
    private Status status = Status.Init;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Checks whether the cell status is <see cref="TlmCell.Status.Init"/>.</summary>
    /// <param name="sMsg">Message text</param>
    /// <exception cref="ReportException">The column status is not 'Init'.</exception>
    private void CheckStatus_Init(String sMsg) {
      tlmBase.CheckStatus_Open(sMsg);
      if (status != Status.Init) {
        throw new ReportException("The cell must be in initialization mode; " + sMsg);
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Checks whether the cell status is 'Open'.</summary>
    /// <param name="sMsg">Message text</param>
    /// <exception cref="ReportException">The column status is not 'Open'.</exception>
    private void CheckStatus_Open(String sMsg) {
      tlmBase.CheckStatus_Open(sMsg);
      if (status != Status.Open && status != Status.OpenText) {
        throw new ReportException("The cell is not open; " + sMsg);
      }
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Position
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Horizontal alignment of the cell contents (default: left)</summary>
    public override Double rAlignH {
      get { return base.rAlignH; }
      set { base.rAlignH = value;  iFirstRepObjOfCurLine = iRepObjCount;  bCut = false; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Current horizontal position (points, 1/72 inch)</summary>
    public Double rCurX = 0;

    /// <summary>Current horizontal position (mm)</summary>
    public Double rCurX_MM {
      get { return RT.rMMFromPoint(rCurX); }
      set { rCurX = RT.rPointFromMM(value); }
    }

    private Double _rCurY = 0;
    /// <summary>Current vertical position (points, 1/72 inch)</summary>
    public Double rCurY {
      get { return _rCurY; }
      set { _rCurY = value;  iFirstRepObjOfCurLine = iRepObjCount;  bCut = false; }
    }

    /// <summary>Current vertical position (mm)</summary>
    public Double rCurY_MM {
      get { return RT.rMMFromPoint(rCurY); }
      set { rCurY = RT.rPointFromMM(value); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the left margin of the cell.</summary>
    /// <value>Position of the left margin of the cell (points, 1/72 inch)</value>
    internal Double rPosMarginLeft {
      get { return tlmColumn_Start.rPosX + rMarginLeft; }
    }

    /// <summary>Gets the position of the right margin of the cell.</summary>
    /// <value>Position of the right margin of the cell (points, 1/72 inch)</value>
    internal Double rPosMarginRight {
      get { return tlmColumn_End.rPosX + tlmColumn_End.rWidth - rMarginRight; }
    }

    /// <summary>Gets the position of the top margin of the cell.</summary>
    /// <value>Position of the top margin of the cell (points, 1/72 inch)</value>
    internal Double rPosMarginTop {
      get { return tlmRow_Start.rPosTop + rMarginTop; }
    }

    /// <summary>Gets the position of the bottom margin of the cell.</summary>
    /// <value>Position of the bottom margin of the cell (points, 1/72 inch)</value>
    internal Double rPosMarginBottom {
      get { return tlmRow_End.rPosBottom - rMarginBottom; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the width of the inner (indented) area (points, 1/72 inch).</summary>
    private Double rInnerWidth {
      get { return rPosMarginRight - rPosMarginLeft - rIndentLeft - rIndentRight; }
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <overloads>
    ///   <summary>Makes a new line in the cell.</summary>
    /// </overloads>
    /// 
    /// <summary>Makes a new line in the cell with the specified line feed height (inch version).</summary>
    /// <remarks>
    /// The current vertical position <see cref="TlmCell.rCurY"/> will be incremented by <paramref name="rLineFeed"/>, the current horizontal position <see cref="TlmCell.rCurX"/> will be set to the left indent <see cref="TlmCellDef.rIndentLeft"/>.
    /// <para>For the metric version see <see cref="TlmCell.NewLineMM"/>.</para>
    /// </remarks>
    /// <param name="rLineFeed">Height of the line feed (points, 1/72 inch)</param>
    /// <exception cref="ReportException">The cell is not <see cref="TlmCell.Status">Open</see>.</exception>
    /// <seealso cref="TlmCell.NewLineMM"/>
    public void NewLine(Double rLineFeed) {
      CheckStatus_Open("cannot make a new line.");
      rCurX = rIndentLeft + rInnerWidth * rAlignH;
      rCurY += rLineFeed;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Makes a new line in the cell with the standard line feed height.</summary>
    /// <remarks>The current vertical position <see cref="TlmCell.rCurY"/> will be incremented by the value of <see cref="TlmCellDef.rLineFeed"/>, the current horizontal position <see cref="TlmCell.rCurX"/> will be set to the left indent <see cref="TlmCellDef.rIndentLeft"/>.</remarks>
    /// <exception cref="ReportException">The cell is not <see cref="TlmCell.Status">Open</see>.</exception>
    /// <seealso cref="TlmCellDef.rLineFeed"/>
    public void NewLine() {
      CheckStatus_Open("cannot make a new line.");
      NewLine(rLineFeed);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Makes a new line in the cell with the specified line feed height (metric version).</summary>
    /// <remarks>
    /// The current vertical position <see cref="TlmCell.rCurY"/> will be incremented by <paramref name="rLineFeedMM"/>, the current horizontal position <see cref="TlmCell.rCurX"/> will be set to the left indent <see cref="TlmCellDef.rIndentLeft"/>.
    /// <para>For the inch version see <see cref="TlmCell.NewLine(System.Double)"/>.</para>
    /// </remarks>
    /// <param name="rLineFeedMM">Height of the line feed (mm)</param>
    /// <exception cref="ReportException">The cell is not <see cref="TlmCell.Status">Open</see>.</exception>
    /// <seealso cref="TlmCell.NewLine(System.Double)"/>
    public void NewLineMM(Double rLineFeedMM) {
      NewLine(RT.rPointFromMM(rLineFeedMM));
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Calculates the bottom position of the row.</summary>
    /// <param name="bCommitted">If <see langword="true"/>, the bottom position of the committed rows will be calculated, otherwise the bottom position of all rows will be calculated. </param>
    /// <returns>Bottom position (points, 1/72 inch)</returns>
    internal Double rCalculateMaxY(Boolean bCommitted) {
      Double rMaxY = 0;
      Debug.Assert(!bCommitted || (tlmBase.tlmRow_Committed.iIndex >= tlmRow_Start.iIndex && tlmBase.tlmRow_Committed.iIndex <= tlmRow_End.iIndex));
      Int32 iRepObjCount = (bCommitted ? tlmColumn_Start.iRepObjCommitted : this.iRepObjCount);
      for (Int32 iRepObj = 0;  iRepObj < iRepObjCount;  iRepObj++) {
        RepObj repObj = repObj_Get(iRepObj);
        Double rPosBottom = repObj.rPosBottom;
        if (rPosBottom > rMaxY) {
          rMaxY = rPosBottom;
        }
      }
      return rMaxY + rIndentBottom;
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Cell Data
    //----------------------------------------------------------------------------------------------------x

    /// <summary>This variable holds the report objects.</summary>
    /// <remarks>null: no report objects; RepObj: one report object; ArrayList: contains many report objects.</remarks>
    private Object oData;

    /// <summary>Array list of report objects: temporary used in method 'Add'.</summary>
    private static ArrayList al_RepObj = new ArrayList(50);

    //----------------------------------------------------------------------------------------------------x
    /// <summary>First report object of current line</summary>
    /// <remarks>This index will be used to adjust the position of the report objects in one line.</remarks>
    private Int32 iFirstRepObjOfCurLine = 0;

    private Boolean bCut = false;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the current cell.</summary>
    /// <param name="repObj">Report object that will be added to the current cell</param>
    private void AddRepObj(RepObj repObj) {
      if (oData == null) {
        oData = repObj;
      }
      else {
        ArrayList al_RepObj = oData as ArrayList;
        if (al_RepObj == null) {
          al_RepObj = new ArrayList(10);
          al_RepObj.Add(oData);
          oData = al_RepObj;
        }
        al_RepObj.Add(repObj);
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Removes a range of report objects.</summary>
    /// <param name="iIndex">Start index</param>
    /// <param name="iCount">Number of report objects to remove</param>
    internal void RemoveRange(Int32 iIndex, Int32 iCount) {
      if (iIndex >= iRepObjCount) {
        return;
      }
      ArrayList al_RepObj = oData as ArrayList;
      if (al_RepObj == null) {
        Debug.Assert(iCount <= 1);
        oData = null;
        return;
      }
      al_RepObj.RemoveRange(iIndex, iCount);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the specified report object.</summary>
    /// <param name="iIndex">Index of the report object</param>
    /// <returns>Report object with the specified index</returns>
    /// <exception cref="IndexOutOfRangeException">The specified index is out of range.</exception>
    internal RepObj repObj_Get(Int32 iIndex) {
      if (oData == null) {
        throw new IndexOutOfRangeException("Index out of range");
      }
      else {
        ArrayList al_RepObj = oData as ArrayList;
        if (al_RepObj == null) {
          if (iIndex != 0) {
            throw new IndexOutOfRangeException("Index out of range");
          }
          return (RepObj)oData;
        }
        return (RepObj)al_RepObj[iIndex];
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the current number of report objects.</summary>
    /// <value>Number of report objects</value>
    internal Int32 iRepObjCount {
      get {
        if (oData == null) {
          return 0;
        }
        else {
          ArrayList al_RepObj = oData as ArrayList;
          if (al_RepObj == null) {
            return 1;
          }
          return al_RepObj.Count;
        }
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <overloads>
    ///   <summary>Adds a report object to the cell.</summary>
    /// </overloads>
    /// 
    /// <summary>Adds a report object to the cell at the current position with the specified offset (inch version).</summary>
    /// <remarks>
    /// The current horizontal position <see cref="TlmCell.rCurX"/> will be incremented by the width of the report object.
    /// <para>The horizontal offset <paramref name="rOfsH"/> can be used for example to make a space in front of the report object.
    /// The vertical offset <paramref name="rOfsV"/> can be used for example to adjust the vertical position of an image or to set super-/subscript fonts.</para>
    /// <para>For the metric version see <see cref="TlmCell.AddMM"/>.</para>
    /// </remarks>
    /// <param name="rOfsH">Horizontal offset (points, 1/72 inch)</param>
    /// <param name="rOfsV">Vertical offset (points, 1/72 inch)</param>
    /// <param name="repObj">Report object that will be added to the cell</param>
    /// <seealso cref="TlmCell.AddMM"/>
    public void Add(Double rOfsH, Double rOfsV, RepObj repObj) {
      RepString repString = repObj as RepString;
      if (RT.bEquals(rAngle, -90, 0.001)) {  // vertical
        Debug.Assert(tlmRow_Start.iIndex == tlmRow_End.iIndex, "vertically merged cell are not supported");
        Double rPreferredHeight = tlmRow_Start.rPreferredHeight;
        Double rInnerHeight = rPreferredHeight - rIndentTop - rIndentBottom;
        if (status == Status.Init) {
          if (Double.IsNaN(rPreferredHeight)) {
            throw new ReportException("The preferred height of the row must be set");
          }
          rCurX = rIndentLeft + rInnerWidth * rAlignV;
          rCurY = rPreferredHeight - rIndentBottom - rInnerHeight * rAlignH;
          status = Status.Open;
        }
        CheckStatus_Open("cannot add a report object.");

        Double rUsedWidth = 0;
        if (iFirstRepObjOfCurLine < iRepObjCount) {
          RepObj ro = repObj_Get(iRepObjCount - 1);
          rUsedWidth = ro.rPosBottom;
          ro = repObj_Get(iFirstRepObjOfCurLine);
          rUsedWidth -= ro.rPosTop;
        }
        rUsedWidth += rOfsH;

        Double rRemainingWidth = rInnerHeight - rUsedWidth;
        if (repString != null) {
          if (status == Status.Open) {
            if (Double.IsNaN(rLineFeed)) {
              rLineFeed = repString.fontProp.rLineFeed;
            }
            //rCurX -= repString.fontProp.rHeight() * rAlignV;
            status = Status.OpenText;
          }
          if (tlmTextMode == TlmTextMode.EllipsisCharacter) {
            repString.sText = repString.fontProp.sTruncateText(repString.sText, rRemainingWidth);
            // ... !!!
          }
        }

        if (repString != null && (tlmTextMode == TlmTextMode.MultiLine || tlmTextMode == TlmTextMode.SingleMultiLine)) {
          Debug.Fail("not implemented");
        }
        else {
          Double rOfs = (repObj.rWidth + rOfsH) * rAlignH;
          for (Int32 i = iFirstRepObjOfCurLine;  i < iRepObjCount;  i++) {
            RepObj ro = repObj_Get(i);
            ro.matrixD.rDY += rOfs;
          }
          repObj.RotateTransform(rAngle);
          repObj.matrixD.rDX = rCurX - rOfsV;
          repObj.rAlignH = rAlignH;
          repObj.matrixD.rDY = rCurY - rOfsH * (1- rAlignH);
          repObj.rAlignV = rAlignV;
          AddRepObj(repObj);
          rCurY = repObj.rPosTop;
        }
      }
      else {  // horizontal
        if (status == Status.Init) {
          rCurX = rIndentLeft + rInnerWidth * rAlignH;
          rCurY = rIndentTop;
          status = Status.Open;
        }
        CheckStatus_Open("cannot add a report object.");

        Double rUsedWidth = 0;
        if (iFirstRepObjOfCurLine < iRepObjCount) {
          RepObj ro = repObj_Get(iRepObjCount - 1);
          rUsedWidth = ro.rPosRight;
          ro = repObj_Get(iFirstRepObjOfCurLine);
          rUsedWidth -= ro.rPosLeft;
        }
        rUsedWidth += rOfsH;

        Double rRemainingWidth = rInnerWidth - rUsedWidth;
        if (repString != null) {
          if (status == Status.Open) {
            if (Double.IsNaN(rLineFeed)) {
              rLineFeed = repString.fontProp.rLineFeed;
            }
            rCurY += repString.fontProp.rSize;
            status = Status.OpenText;
          }
          if (tlmTextMode == TlmTextMode.EllipsisCharacter) {
            Double rWidth = repString.fontProp.rGetTextWidth(repString.sText);
            if (rWidth > rRemainingWidth) {
              if (bCut) {
                return;
              }
              repString.sText = repString.fontProp.sTruncateText(repString.sText, rRemainingWidth);
              bCut = true;
              rWidth = repString.fontProp.rGetTextWidth(repString.sText);
              if (rWidth >= rRemainingWidth) {
                if (iFirstRepObjOfCurLine < iRepObjCount) {
                  RepObj ro = repObj_Get(iRepObjCount - 1);
                  RepString rs = ro as RepString;
                  if (rs != null) {
                    rs.sText = rs.sText.Substring(0, rs.sText.Length - 1) + "...";
                  }
                }
                return;
              }
            }
          }
        }

        if (repString != null && (tlmTextMode == TlmTextMode.MultiLine || tlmTextMode == TlmTextMode.SingleMultiLine)) {
          Double rWidth = rInnerWidth + rIndentLeft;
          lock (al_RepObj) {
            Debug.Assert(al_RepObj.Count == 0);
            Double rCopy = rCurY;
            Double rOfs = rWidth * rAlignH;
            rCurX -= rOfs;
            tlmBase.FormatString(al_RepObj, repString, ref rCurX, rIndentLeft, rAlignH, ref _rCurY, rWidth);
            rCurX += rOfs;
            foreach (RepObj ro in al_RepObj) {
              AddRepObj(ro);
              if (tlmTextMode == TlmTextMode.SingleMultiLine) {
                tlmBase.Commit();
              }
            }
            al_RepObj.Clear();
            if (!RT.bEquals(rCopy, _rCurY, TlmBase.rTol)) {
              rCurY = _rCurY;  // trigger iFirstRepObjOfCurLine
              iFirstRepObjOfCurLine = iRepObjCount - 1;
            }
          }
        }
        else {
          Double rOfs = (repObj.rWidth + rOfsH) * rAlignH;
          for (Int32 i = iFirstRepObjOfCurLine;  i < iRepObjCount;  i++) {
            RepObj ro = repObj_Get(i);
            ro.matrixD.rDX -= rOfs;
          }
          repObj.matrixD.rDX = rCurX + rOfsH * (1 - rAlignH);
          repObj.rAlignH = rAlignH;
          repObj.matrixD.rDY = rCurY + rOfsV;
          AddRepObj(repObj);
          rCurX = repObj.rPosRight;
        }
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the cell at the current position.</summary>
    /// <remarks>The current horizontal position <see cref="TlmCell.rCurX"/> will be incremented by the width of the report object.</remarks>
    /// <param name="repObj">Report object that will be added to the cell</param>
    public void Add(RepObj repObj) {
      Add(0, 0, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the cell at the current position with the specified offset (metric version).</summary>
    /// <remarks>
    /// The current horizontal position <see cref="TlmCell.rCurX"/> will be incremented by the width of the report object.
    /// <para>The horizontal offset <paramref name="rOfsH_MM"/> can be used for example to make a space in front of the report object.
    /// The vertical offset <paramref name="rOfsV_MM"/> can be used for example to adjust the vertical position of an image or to set super-/subscript fonts.</para>
    /// <para>For the inch version see <see cref="TlmCell.Add(System.Double,System.Double,RepObj)"/>.</para>
    /// </remarks>
    /// <param name="rOfsH_MM">Horizontal offset (mm)</param>
    /// <param name="rOfsV_MM">Vertical offset (mm)</param>
    /// <param name="repObj">Report object that will be added to the cell</param>
    /// <seealso cref="TlmCell.Add(System.Double,System.Double,RepObj)"/>
    public void AddMM(Double rOfsH_MM, Double rOfsV_MM, RepObj repObj) {
      Add(RT.rPointFromMM(rOfsH_MM), RT.rPointFromMM(rOfsV_MM), repObj);
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the cell at the current position.</summary>
    /// <remarks>The current horizontal position <see cref="TlmCell.rCurX"/> will be incremented by the width of the report object.</remarks>
    /// <param name="repObj">Report object that will be added to the cell</param>
    internal void SplitCell(TlmRow tlmRow_2) {
      TlmRow tlmRow_Committed = tlmBase.tlmRow_Committed;

      TlmCell tlmCell_2 = new TlmCell(tlmColumn_Start, tlmColumn_End, tlmRow_2);
      if (tlmRow_End.iIndex != tlmRow_Committed.iIndex) {
        tlmCell_2.tlmRow_End = tlmRow_End;
      }
      tlmRow_2.aTlmCell.SetCell(tlmColumn_Start.iIndex, tlmCell_2);
      tlmRow_End = tlmRow_Committed;

      tlmCell_2.rAlignH = rAlignH;
      tlmCell_2.rAlignV = rAlignV;
      tlmCell_2.rAngle = rAngle;
      tlmCell_2.tlmTextMode = tlmTextMode;
      tlmCell_2.rLineFeed = rLineFeed;
      tlmCell_2.rMarginLeft = rMarginLeft;
      tlmCell_2.rMarginRight = rMarginRight;
      tlmCell_2.rMarginTop = rMarginTop;
      tlmCell_2.rMarginBottom = rMarginBottom;
      tlmCell_2.rIndentLeft = rIndentLeft;
      tlmCell_2.rIndentRight = rIndentRight;
      tlmCell_2.rIndentTop = rIndentTop;
      tlmCell_2.rIndentBottom = rIndentBottom;
      tlmCell_2.brushProp_Back = brushProp_Back;
      tlmCell_2.penProp_LineLeft = penProp_LineLeft;
      tlmCell_2.penProp_LineRight = penProp_LineRight;
      tlmCell_2.penProp_LineTop = penProp_LineTop;
      tlmCell_2.penProp_LineBottom = penProp_LineBottom;
      tlmCell_2.iOrderLineLeft = iOrderLineLeft;
      tlmCell_2.iOrderLineRight = iOrderLineRight;
      tlmCell_2.iOrderLineTop = iOrderLineTop;
      tlmCell_2.iOrderLineBottom = iOrderLineBottom;

      // get vertical offset
      Int32 iStartIndex = tlmColumn_Start.iRepObjCommitted;
      Double rDelta = 0.0;
      if (iStartIndex < iRepObjCount) {
        RepObj repObj = repObj_Get(iStartIndex);
        rDelta = repObj.rPosTop - rIndentTop;
      }

      tlmCell_2.status = status;
      tlmCell_2.rCurX = rCurX;
      tlmCell_2.rCurY = rCurY - rDelta;
      tlmCell_2.iFirstRepObjOfCurLine = Math.Max(0, iFirstRepObjOfCurLine - iStartIndex);
      tlmCell_2.bCut = bCut;

      // copy RepObjects
      for (Int32 iRepObj = iStartIndex; iRepObj < iRepObjCount; iRepObj++) {
        RepObj repObj = repObj_Get(iRepObj);
        repObj.matrixD.rDY -= rDelta;
        tlmCell_2.AddRepObj(repObj);
      }
      RemoveRange(iStartIndex, iRepObjCount - iStartIndex);
    }

    //----------------------------------------------------------------------------------------------------x
    #region Line Visibility
    //----------------------------------------------------------------------------------------------------x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Determines the visibility of the left line in reference to the specified row.</summary>
    /// <param name="iRow">Row index</param>
    /// <returns>If the left line is visible, the method returns <see langword="true"/>, otherwise it returns <see langword="false"/>.</returns>
    internal Boolean bVisibleLineLeft(Int32 iRow) {
      Debug.Assert(iRow >= tlmRow_Start.iIndex && iRow <= tlmRow_End.iIndex);
      Int32 i = tlmColumn_Start.iIndex;
      if (i == 0) {
        return true;
      }
      TlmRow row = tlmBase.list_TlmRow[iRow];
      TlmCell cell_Left = row.aTlmCell[i - 1];
      return (iOrderLineLeft >= cell_Left.iOrderLineRight);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Determines the visibility of the right line in reference to the specified row.</summary>
    /// <param name="iRow">Row index</param>
    /// <returns>If the right line is visible, the method returns <see langword="true"/>, otherwise it returns <see langword="false"/>.</returns>
    internal Boolean bVisibleLineRight(Int32 iRow) {
      Debug.Assert(iRow >= tlmRow_Start.iIndex && iRow <= tlmRow_End.iIndex);
      Int32 i = tlmColumn_End.iIndex;
      if (i == tlmBase.list_TlmColumn.Count - 1) {
        return true;
      }
      TlmRow row = tlmBase.list_TlmRow[iRow];
      TlmCell cell_Right = row.aTlmCell[i + 1];
      return (iOrderLineRight > cell_Right.iOrderLineLeft);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Determines the visibility of the top line in reference to the specified column.</summary>
    /// <param name="iCol">Column index</param>
    /// <returns>If the top line is visible, the method returns <see langword="true"/>, otherwise it returns <see langword="false"/>.</returns>
    internal Boolean bVisibleLineTop(Int32 iCol) {
      Debug.Assert(iCol >= tlmColumn_Start.iIndex && iCol <= tlmColumn_End.iIndex);
      Int32 i = tlmRow_Start.iIndex;
      if (i == 0) {
        return true;
      }
      TlmRow row = tlmBase.list_TlmRow[i - 1];
      TlmCell cell_Top = row.aTlmCell[iCol];
      return (iOrderLineTop >= cell_Top.iOrderLineBottom);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Determines the visibility of the bottom line in reference to the specified column.</summary>
    /// <param name="iCol">Column index</param>
    /// <returns>If the bottom line is visible, the method returns <see langword="true"/>, otherwise it returns <see langword="false"/>.</returns>
    internal Boolean bVisibleLineBottom(Int32 iCol) {
      Debug.Assert(iCol >= tlmColumn_Start.iIndex && iCol <= tlmColumn_End.iIndex);
      Int32 i = tlmRow_End.iIndex;
      if (i >= tlmBase.tlmRow_Committed.iIndex) {
        return true;
      }
      TlmRow row = tlmBase.list_TlmRow[i + 1];
      TlmCell cell_Bottom = row.aTlmCell[iCol];
      return (iOrderLineBottom > cell_Bottom.iOrderLineTop);
    }
    #endregion
  }
}
