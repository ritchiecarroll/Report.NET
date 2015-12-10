using System;

// Creation date: 05.11.2002
// Checked: 08.07.2003
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
  /// <summary>Definition of the column properties</summary>
  /// <remarks>
  /// This class defines the properties of a column for the table layout manager.
  /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmBaseDefaults_Column"]/*'/>
  /// </remarks>
  public class TlmColumnDef {
    //----------------------------------------------------------------------------------------------------x
    #region Border Margins
    //----------------------------------------------------------------------------------------------------x

    private Double _rBorderTop = Double.NaN;
    /// <summary>Gets or sets the top border margin of the column (default: <see cref="System.Double.NaN"/>).</summary>
    /// <value>
    /// The top border margin of the column in points (1/72 inch) or <see cref="System.Double.NaN"/>
    /// if the cell margin should not be overwritten.
    /// </value>
    /// <remarks>
    /// This value sets the top border margin of the column.
    /// It overwrites the top margin (<see cref="TlmCellDefBase.rMarginTop"/>) of the topmost cell.
    /// Tables often have different properties for the topmost and bottommost lines than for the other cells.
    /// If this value is <see cref="System.Double.NaN"/> the top margin of the topmost cell will not be overwritten.
    /// <para>For the metric version see <see cref="TlmColumnDef.rBorderTopMM"/>.</para>
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmColumnSample"]/*'/>
    public Double rBorderTop {
      get { return _rBorderTop; }
      set { _rBorderTop = value; }
    }

    /// <summary>Gets or sets the top border margin of the column (default: <see cref="System.Double.NaN"/>).</summary>
    /// <value>
    /// The top border margin of the column in millimeters or <see cref="System.Double.NaN"/>
    /// if the cell margin should not be overwritten.
    /// </value>
    /// <remarks>
    /// This value sets the top border margin of the column.
    /// It overwrites the top margin (<see cref="TlmCellDefBase.rMarginTopMM"/>) of the topmost cell.
    /// Tables often have different properties for the topmost and bottommost lines than for the other cells.
    /// If this value is <see cref="System.Double.NaN"/> the top margin of the topmost cell will not be overwritten.
    /// <para>For the inch version see <see cref="TlmColumnDef.rBorderTop"/>.</para>
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmColumnSample"]/*'/>
    public Double rBorderTopMM {
      get { return RT.rMMFromPoint(rBorderTop); }
      set { rBorderTop = RT.rPointFromMM(value); }
    }

    private Double _rBorderBottom = Double.NaN;
    /// <summary>Gets or sets the bottom border margin of the column (default: <see cref="System.Double.NaN"/>).</summary>
    /// <value>
    /// The bottom border margin of the column in points (1/72 inch) or <see cref="System.Double.NaN"/>
    /// if the cell margin should not be overwritten.
    /// </value>
    /// <remarks>
    /// This value sets the bottom border margin of the column.
    /// It overwrites the bottom margin (<see cref="TlmCellDefBase.rMarginBottom"/>) of the bottommost cell.
    /// Tables often have different properties for the topmost and bottommost lines than for the other cells.
    /// If this value is <see cref="System.Double.NaN"/> the bottom margin of the bottommost cell will not be overwritten.
    /// <para>For the metric version see <see cref="TlmColumnDef.rBorderBottomMM"/>.</para>
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmColumnSample"]/*'/>
    public Double rBorderBottom {
      get { return _rBorderBottom; }
      set { _rBorderBottom = value; }
    }

    /// <summary>Gets or sets the bottom border margin of the column (default: <see cref="System.Double.NaN"/>).</summary>
    /// <value>
    /// The bottom border margin of the column in millimeters or <see cref="System.Double.NaN"/>
    /// if the cell margin should not be overwritten.
    /// </value>
    /// <remarks>
    /// This value sets the bottom border margin of the column.
    /// It overwrites the bottom margin (<see cref="TlmCellDefBase.rMarginBottomMM"/>) of the bottommost cell.
    /// Tables often have different properties for the topmost and bottommost lines than for the other cells.
    /// If this value is <see cref="System.Double.NaN"/> the bottom margin of the bottommost cell will not be overwritten.
    /// <para>For the inch version see <see cref="TlmColumnDef.rBorderBottom"/>.</para>
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmColumnSample"]/*'/>
    public Double rBorderBottomMM {
      get { return RT.rMMFromPoint(rBorderBottom); }
      set { rBorderBottom = RT.rPointFromMM(value); }
    }

    /// <summary>Gets or sets the vertical border margins of the column.</summary>
    /// <value>
    /// The vertical border margins of the column in points (1/72 inch) or <see cref="System.Double.NaN"/>
    /// if the cell margins should not be overwritten.
    /// </value>
    /// <remarks>
    /// This value sets the vertical border margins of the column,
    /// i.e. the top <see cref="TlmColumnDef.rBorderTop"/> and bottom margins <see cref="TlmColumnDef.rBorderBottom"/>.
    /// <para>For the metric version see <see cref="TlmColumnDef.rBorderV_MM"/>.</para>
    /// </remarks>
    public Double rBorderV {
      get { return (rBorderTop + rBorderBottom) / 2; }
      set { rBorderTop = rBorderBottom = value; }
    }

    /// <summary>Gets or sets the vertical border margins of the column.</summary>
    /// <value>
    /// The vertical border margins of the column in millimeters or <see cref="System.Double.NaN"/>
    /// if the cell margins should not be overwritten.
    /// </value>
    /// <remarks>
    /// This value sets the vertical border margins of the column,
    /// i.e. the top <see cref="TlmColumnDef.rBorderTopMM"/> and bottom margins <see cref="TlmColumnDef.rBorderBottomMM"/>.
    /// <para>For the inch version see <see cref="TlmColumnDef.rBorderV"/>.</para>
    /// </remarks>
    public Double rBorderV_MM {
      get { return RT.rMMFromPoint(rBorderV); }
      set { rBorderV = RT.rPointFromMM(value); }
    }
    #endregion

    //----------------------------------------------------------------------------------------------------x
    #region Border Lines
    //----------------------------------------------------------------------------------------------------x

    private PenProp _penProp_BorderTop = PenProp.penProp_Null;
    /// <summary>Gets or sets the pen properties of the top border line of the column (default: <see langword="null"/>).</summary>
    /// <value>
    ///		<list type="table">
    ///			<listheader>
    ///				<term>Value</term>
    ///				<description>Description</description>
    ///			</listheader>
    ///			<item>
    ///				<term><see cref="Root.Reports.PenProp"/> object</term>
    ///				<description>The top border line of the topmost cell of the table will be overwritten by this value.</description>
    ///			</item>
    ///			<item>
    ///				<term><see langword="null"/></term>
    ///				<description>The top border line of the topmost cell of the table will be removed.</description>
    ///			</item>
    ///			<item>
    ///				<term><see cref="PenProp.pp_Null"/></term>
    ///				<description>This value prevents that the top border line of the topmost cell of the table will be overwritten.</description>
    ///			</item>
    ///		</list>
    /// </value>
    /// <remarks>
    /// This value sets the top line of the column.
    /// It overwrites the top line (<see cref="TlmCellDefBase.pp_LineTop"/>) of the topmost cell.
    /// Tables often have different properties for the topmost and bottommost lines than for the other cells.
    /// If this value is <see cref="PenProp.pp_Null"/> the top line of the cell will not be overwritten.
    /// </remarks>
    public PenProp penProp_BorderTop {
      get { return _penProp_BorderTop; }
      set { _penProp_BorderTop = value; }
    }

    private PenProp _penProp_BorderBottom = PenProp.penProp_Null;
    /// <summary>Gets or sets the pen properties of the bottom border line of the column (default: <see langword="null"/>).</summary>
    /// <value>
    ///		<list type="table">
    ///			<listheader>
    ///				<term>Value</term>
    ///				<description>Description</description>
    ///			</listheader>
    ///			<item>
    ///				<term><see cref="Root.Reports.PenProp"/> object</term>
    ///				<description>The bottom border line of the bottommost cell of the table will be overwritten by this value.</description>
    ///			</item>
    ///			<item>
    ///				<term><see langword="null"/></term>
    ///				<description>The bottom border line of the bottommost cell of the table will be removed.</description>
    ///			</item>
    ///			<item>
    ///				<term><see cref="PenProp.pp_Null"/></term>
    ///				<description>This value prevents that the bottom border line of the bottommost cell of the table will be overwritten.</description>
    ///			</item>
    ///		</list>
    /// </value>
    /// <remarks>
    /// This value sets the bottom line of the column.
    /// It overwrites the bottom line (<see cref="TlmCellDefBase.pp_LineBottom"/>) of the bottommost cell.
    /// Tables often have different properties for the topmost and bottommost lines than for the other cells.
    /// If this value is <see cref="PenProp.pp_Null"/> the bottom line of the cell will not be overwritten.
    /// </remarks>
    public PenProp penProp_BorderBottom {
      get { return _penProp_BorderBottom; }
      set { _penProp_BorderBottom = value; }
    }

    /// <summary>Sets the pen properties of the horizontal border lines of the column.</summary>
    /// <value>
    ///		<list type="table">
    ///			<listheader>
    ///				<term>Value</term>
    ///				<description>Description</description>
    ///			</listheader>
    ///			<item>
    ///				<term><see cref="Root.Reports.PenProp"/> object</term>
    ///				<description>The top border line of the topmost cell and the bottom border line of the bottommost cell of the table will be overwritten by this value.</description>
    ///			</item>
    ///			<item>
    ///				<term><see langword="null"/></term>
    ///				<description>The top border line of the topmost cell and the bottom border line of the bottommost cell of the table will be removed.</description>
    ///			</item>
    ///			<item>
    ///				<term><see cref="PenProp.pp_Null"/></term>
    ///				<description>This value prevents that the top border line of the topmost cell and the bottom border line of the bottommost cell of the table will be overwritten.</description>
    ///			</item>
    ///		</list>
    /// </value>
    /// <remarks>
    /// This value sets the horizontal lines of the column.
    /// It overwrites the top border line of the topmost cell and the bottom line (<see cref="TlmCellDefBase.pp_LineBottom"/>) of the bottommost cell.
    /// Tables often have different properties for the topmost and bottommost lines than for the other cells.
    /// If this value is <see cref="PenProp.pp_Null"/> the horizontal lines of the cells will not be overwritten.
    /// </remarks>
    public PenProp penProp_BorderH {
      set { penProp_BorderTop = penProp_BorderBottom = value; }
    }
    #endregion

    /// <summary>Definition of the default values of a cell of this column</summary>
    public readonly TlmCellDef tlmCellDef_Header = new TlmCellDef();

    /// <summary>Header of the column</summary>
    public String sHeader;

    /// <summary>Header font properties</summary>
    public FontProp fontProp_Header;

    //----------------------------------------------------------------------------------------------------
    #if Compatible_0_8
    //----------------------------------------------------------------------------------------------------

    [Obsolete("use 'penProp_BorderTop'")]
    public PenProp pp_BorderTop {
      get { return _penProp_BorderTop; }
      set { _penProp_BorderTop = value; }
    }

    [Obsolete("use 'penProp_BorderBottom'")]
    public PenProp pp_BorderBottom {
      get { return _penProp_BorderBottom; }
      set { _penProp_BorderBottom = value; }
    }

    [Obsolete("use 'penProp_BorderH'")]
    public PenProp pp_BorderH {
      set { penProp_BorderTop = penProp_BorderBottom = value; }
    }

    [Obsolete("use 'tlmCellDef_Header'")]
    public TlmCellDef headerCellDef {
      get { return tlmCellDef_Header; }
    }

    [Obsolete("use 'fontProp_Header'")]
    public FontProp fp_Header {
      get { return fontProp_Header; }
      set { fontProp_Header = value; }
    }
    #endif
  }
}
