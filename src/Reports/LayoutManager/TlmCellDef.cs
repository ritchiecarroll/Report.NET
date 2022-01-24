using System;
using System.Diagnostics;

// Creation date: 01.05.2003
// Checked: 11.06.2003
// Author: Otto Mayer (mot@root.ch)
// Version: 1.02

// Report.NET copyright 2003-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Definition of the cell properties</summary>
  /// <remarks>
  /// When a new row is initialized, a new cell will be created for each column according to the cell create type <see cref="TlmBase.CellCreateType"/>.
  /// </remarks>
  public class TlmCellDef {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a cell data object.</summary>
    internal TlmCellDef() {
    }

    //----------------------------------------------------------------------------------------------------x
    // Layout
    //----------------------------------------------------------------------------------------------------x

    private Double _rAlignH = Double.NaN;
    /// <summary>Gets or sets the horizontal alignment of the cell contents (default: left)</summary>
    /// <value>Horizontal alignment: value between 0 and 1, 0:left <see cref="RepObj.rAlignLeft"/>, 0.5:centered <see cref="RepObj.rAlignCenter"/>, 1:right <see cref="RepObj.rAlignRight"/></value>
    public virtual Double rAlignH {
      get { return _rAlignH; }
      set { _rAlignH = value; }
    }
  
    /// <summary>Vertical alignment of the cell contents (default: top)</summary>
    public Double rAlignV = Double.NaN;

    private Double _rAngle = Double.NaN;
    /// <summary>Gets or sets the angle of the cell contents (default: 0°)</summary>
    /// <remarks>
    /// The preferred height <see cref="TlmRowDef.rPreferredHeight"/> must be set.
    /// <para>The text mode must be <see cref="TlmBase.TextMode">EllipsisCharacter</see>.</para>
    /// </remarks>
    /// <value>Angle of the cell contents, clockwise: 0° horizontal, -90° vertical upwards
    /// <note type="caution">Other values are not yet supported</note></value>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmVerticalSample"]/*'/>
    public Double rAngle {
      get { return _rAngle; }
      set {
        Debug.Assert(RT.bEquals(value, 0, 0.001) || RT.bEquals(value, -90, 0.001), "only '0°' or '-90°' allowed");
        _rAngle = value;
      }
    }
  
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    /// <summary>Text mode (default: EllipsisCharacter)</summary>
    public TlmTextMode tlmTextMode = TlmTextMode.FallBack;
  
    /// <summary>Height of the line feed (points, 1/72 inch, default: 1/6 inch)</summary>
    public Double rLineFeed = Double.NaN;
  
    /// <summary>Height of the line feed (mm, default: 1/6 inch)</summary>
    public Double rLineFeedMM {
      get { return RT.rMMFromPoint(rLineFeed); }
      set { rLineFeed = RT.rPointFromMM(value); }
    }

    //----------------------------------------------------------------------------------------------------x
    // Margins
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Left margin of the cell (points, 1/72 inch, default: 0)</summary>
    public Double rMarginLeft = Double.NaN;
  
    /// <summary>Left margin of the cell (mm, default: 0)</summary>
    public Double rMarginLeftMM {
      get { return RT.rMMFromPoint(rMarginLeft); }
      set { rMarginLeft = RT.rPointFromMM(value); }
    }

    /// <summary>Right margin of the cell (points, 1/72 inch, default: 0)</summary>
    public Double rMarginRight = Double.NaN;

    /// <summary>Right margin of the cell (mm, default: 0)</summary>
    public Double rMarginRightMM {
      get { return RT.rMMFromPoint(rMarginRight); }
      set { rMarginRight = RT.rPointFromMM(value); }
    }

    /// <summary>Top margin of the cell (points, 1/72 inch, default: 0)</summary>
    public Double rMarginTop = Double.NaN;

    /// <summary>Top margin of the cell (mm, default: 0)</summary>
    public Double rMarginTopMM {
      get { return RT.rMMFromPoint(rMarginTop); }
      set { rMarginTop = RT.rPointFromMM(value); }
    }

    /// <summary>Bottom margin of the cell (points, 1/72 inch, default: 0)</summary>
    public Double rMarginBottom = Double.NaN;
  
    /// <summary>Bottom margin of the cell (mm, default: 0)</summary>
    public Double rMarginBottomMM {
      get { return RT.rMMFromPoint(rMarginBottom); }
      set { rMarginBottom = RT.rPointFromMM(value); }
    }

    /// <summary>Gets or sets the horizontal margins of the cell (points, 1/72 inch).</summary>
    public Double rMarginH {
      get { return (rMarginLeft + rMarginRight) / 2; }
      set { rMarginLeft = rMarginRight = value; }
    }

    /// <summary>Gets or sets the horizontal margins of the cell (mm).</summary>
    public Double rMarginH_MM {
      get { return RT.rMMFromPoint(rMarginH); }
      set { rMarginH = RT.rPointFromMM(value); }
    }

    /// <summary>Gets or sets the vertical margins of the cell (points, 1/72 inch).</summary>
    public Double rMarginV {
      get { return (rMarginTop + rMarginBottom) / 2; }
      set { rMarginTop = rMarginBottom = value; }
    }

    /// <summary>Gets or sets the vertical margins of the cell (mm).</summary>
    public Double rMarginV_MM {
      get { return RT.rMMFromPoint(rMarginV); }
      set { rMarginV = RT.rPointFromMM(value); }
    }

    /// <summary>Sets all margins of the cell (points, 1/72 inch).</summary>
    public Double rMargin {
      set { rMarginH = rMarginV = value; }
    }

    /// <summary>Sets all margins of the cell (mm).</summary>
    public Double rMarginMM {
      set { rMargin = RT.rPointFromMM(value); }
    }

    //----------------------------------------------------------------------------------------------------x
    // Indents
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Left indent of the cell (points, 1/72 inch, default 1 mm)</summary>
    public Double rIndentLeft = Double.NaN;
  
    /// <summary>Left indent of the cell (mm, default 1 mm)</summary>
    public Double rIndentLeftMM {
      get { return RT.rMMFromPoint(rIndentLeft); }
      set { rIndentLeft = RT.rPointFromMM(value); }
    }

    /// <summary>Right indent of the cell (points, 1/72 inch, default 1 mm)</summary>
    public Double rIndentRight = Double.NaN;
  
    /// <summary>Right indent of the cell (mm, default 1 mm)</summary>
    public Double rIndentRightMM {
      get { return RT.rMMFromPoint(rIndentRight); }
      set { rIndentRight = RT.rPointFromMM(value); }
    }

    /// <summary>Top indent of the cell (points, 1/72 inch, default 1 mm)</summary>
    public Double rIndentTop = Double.NaN;
  
    /// <summary>Top indent of the cell (mm, default 1 mm)</summary>
    public Double rIndentTopMM {
      get { return RT.rMMFromPoint(rIndentTop); }
      set { rIndentTop = RT.rPointFromMM(value); }
    }

    /// <summary>Bottom indent of the cell (points, 1/72 inch, default 1 mm)</summary>
    public Double rIndentBottom = Double.NaN;
  
    /// <summary>Bottom indent of the cell (mm, default 1 mm)</summary>
    public Double rIndentBottomMM {
      get { return RT.rMMFromPoint(rIndentBottom); }
      set { rIndentBottom = RT.rPointFromMM(value); }
    }

    /// <summary>Gets or sets the horizontal indents of the cell (points, 1/72 inch).</summary>
    public Double rIndentH {
      get { return (rIndentLeft + rIndentRight) / 2; }
      set { rIndentLeft = rIndentRight = value; }
    }

    /// <summary>Gets or sets the horizontal indents of the cell (mm).</summary>
    public Double rIndentH_MM {
      get { return RT.rMMFromPoint(rIndentH); }
      set { rIndentH = RT.rPointFromMM(value); }
    }

    /// <summary>Gets or sets the vertical indents of the cell (points, 1/72 inch).</summary>
    public Double rIndentV {
      get { return (rIndentTop + rIndentBottom) / 2; }
      set { rIndentTop = rIndentBottom = value; }
    }

    /// <summary>Gets or sets the vertical indents of the cell (mm).</summary>
    public Double rIndentV_MM {
      get { return RT.rMMFromPoint(rIndentV); }
      set { rIndentV = RT.rPointFromMM(value); }
    }

    /// <summary>Sets all indents of the cell (points, 1/72 inch).</summary>
    public Double rIndent {
      set { rIndentH = rIndentV = value; }
    }

    /// <summary>Sets all indents of the cell (mm).</summary>
    public Double rIndentMM {
      set { rIndent = RT.rPointFromMM(value); }
    }

    //----------------------------------------------------------------------------------------------------x
    // Background
    //----------------------------------------------------------------------------------------------------x

    /// <summary>Brush properties of the cell background (default: null - no background)</summary>
    public BrushProp brushProp_Back = BrushProp.bp_Null;

    //----------------------------------------------------------------------------------------------------x
    // Lines
    //----------------------------------------------------------------------------------------------------x

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    /// <summary>Pen properties of the left line of the cell</summary>
    /// <remarks>Visibility:
    /// "left cell".order_LineRight == Back: this line (default)
    /// "left cell".order_LineRight == Front: right line of left cell</remarks>
    public PenProp penProp_LineLeft = PenProp.penProp_Null;
  
    /// <summary>Pen properties of the right line of the cell</summary>
    /// <remarks>Visibility:
    /// order_LineRight == Back: left line of the right cell (default)
    /// order_LineRight == Front: this line</remarks>
    public PenProp penProp_LineRight = PenProp.penProp_Null;

    /// <summary>Pen properties of the top line of the cell.</summary>
    /// <remarks>Visibility:
    /// "upper cell".order_LineBottom == Back: this line (default)
    /// "upper cell".order_LineBottom == Front: bottom line of upper cell</remarks>
    public PenProp penProp_LineTop = PenProp.penProp_Null;

    /// <summary>Pen properties of the bottom line of the cell.</summary>
    /// <remarks>Visibility:
    /// order_LineBottom == Back: top line of lower cell (default)
    /// order_LineBottom == Front: this line</remarks>
    public PenProp penProp_LineBottom = PenProp.penProp_Null;

    /// <summary>Sets the pen properties of the vertical lines of the cell.</summary>
    public PenProp penProp_LineV {
      set { penProp_LineLeft = penProp_LineRight = value; }
    }

    /// <summary>Sets the pen properties of the horizontal lines of the cell.</summary>
    public PenProp penProp_LineH {
      set { penProp_LineTop = penProp_LineBottom = value; }
    }

    /// <summary>Sets the pen properties of all lines of the cell.</summary>
    public PenProp penProp_Line {
      set { penProp_LineV = penProp_LineH = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    //----------------------------------------------------------------------------------------------------x
    #region Line Order/Visibility
    //----------------------------------------------------------------------------------------------------

    /// <summary>Gets or sets the order of the left line.</summary>
    /// <value>
    /// The preferred height of the row in points (1/72 inch).
    /// The default value of this property is <see cref="System.Double.NaN"/>, that is the text within the row will not be cut.
    /// </value>
    /// <remarks>
    /// This value sets the preferred height of the row.
    /// If the height of a cell of the row is less than this value and there is enough space left, the height of the cell will be enlarged.
    /// The preferred height can also be used to limit the length of <see cref="TlmCellDef.rAngle">vertical text</see>.
    /// <para>For the metric version see <see cref="TlmRowDef.rPreferredHeightMM"/>.</para>
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmVerticalSample"]/*'/>
    /// <seealso cref="TlmCellDef.rAngle"/>
    /// <summary>Order of the left line</summary>
    public Int32 iOrderLineLeft = Int32.MinValue;

    /// <summary>Order of the right line</summary>
    public Int32 iOrderLineRight = Int32.MinValue;

    /// <summary>Order of the top line</summary>
    public Int32 iOrderLineTop = Int32.MinValue;

    /// <summary>Order of the bottom line</summary>
    public Int32 iOrderLineBottom = Int32.MinValue;
    #endregion

    //------------------------------------------------------------------------------------------03.11.2006
    #if Compatible_0_8
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------03.11.2006
    public TlmBase.TextMode textMode {
      get {
        switch (tlmTextMode) {
          case TlmTextMode.EllipsisCharacter: { return TlmBase.TextMode.EllipsisCharacter; }
          case TlmTextMode.FallBack: { return TlmBase.TextMode.FallBack; }
          case TlmTextMode.MultiLine: { return TlmBase.TextMode.MultiLine; }
        }
        return TlmBase.TextMode.SingleMultiLine;
      }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'brushProp_Back'")]
    public BrushProp bp_Back {
      get { return brushProp_Back; }
      set { brushProp_Back = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'penProp_LineLeft'")]
    public PenProp pp_LineLeft {
      get { return penProp_LineLeft; }
      set { penProp_LineLeft = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'penProp_LineRight'")]
    public PenProp pp_LineRight {
      get { return penProp_LineRight; }
      set { penProp_LineRight = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'penProp_LineTop'")]
    public PenProp pp_LineTop {
      get { return penProp_LineTop; }
      set { penProp_LineTop = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'penProp_LineBottom'")]
    public PenProp pp_LineBottom {
      get { return penProp_LineBottom; }
      set { penProp_LineBottom = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'penProp_LineV'")]
    public PenProp pp_LineV {
      set { penProp_LineV = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'penProp_LineH'")]
    public PenProp pp_LineH {
      set { penProp_LineH = value; }
    }

    //------------------------------------------------------------------------------------------03.11.2006
    [Obsolete("use 'penProp_Line'")]
    public PenProp pp_Line {
      set { penProp_Line = value; }
    }
    #endif
  }
}
