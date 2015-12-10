using System;

// Creation date: 18.06.2003
// Checked: 30.06.2003
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
  /// <summary>Definition of the row properties</summary>
  /// <remarks>
  /// This class defines the properties of a row for a table layout manager.
  /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmBaseDefaults_Row"]/*'/>
  /// </remarks>
  public class TlmRowDef {
    private Double _rPreferredHeight = Double.NaN;
    /// <summary>Gets or sets the preferred height of the row (default: not set - <see cref="System.Double.NaN"/>)</summary>
    /// <value>
    /// The preferred height of the row in points (1/72 inch).
    /// The default value of this property is <see cref="System.Double.NaN"/>, that is the text within the row will not be cut.
    /// </value>
    /// <remarks>
    /// This value sets the preferred height of the row.
    /// If the height of a cell of the row is less than this value and there is enough space left, the height of the cell will be enlarged.
    /// The preferred height can also be used to limit the length of <see cref="TlmCellDef.rAngle">vertical text</see>.
    /// <para>For the metric version see <see cref="TlmRowDefBase.rPreferredHeightMM"/>.</para>
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmVerticalSample"]/*'/>
    /// <seealso cref="TlmCellDef.rAngle"/>
    public Double rPreferredHeight {
      get { return _rPreferredHeight; }
      set { _rPreferredHeight = value; }
    }

    /// <summary>Gets or sets the preferred height of the row (default: not set - <see cref="System.Double.NaN"/>)</summary>
    /// <value>
    /// The preferred height of the row in millimeters.
    /// The default value of this property is <see cref="System.Double.NaN"/>, that is the text within the row will not be cut.
    /// </value>
    /// <remarks>
    /// This value sets the preferred height of the row.
    /// If the height of a cell of the row is less than this value and there is enough space left, the height of the cell will be enlarged.
    /// The preferred height can also be used to limit the length of <see cref="TlmCellDef.rAngle">vertical text</see>.
    /// <para>For the inch version see <see cref="TlmRowDef.rPreferredHeight"/>.</para>
    /// </remarks>
    /// <include file='D:\Programs\DotNet03\Root\Reports\Include.xml' path='documentation/class[@name="TlmVerticalSample"]/*'/>
    /// <seealso cref="TlmCellDef.rAngle"/>
    public Double rPreferredHeightMM {
      get { return RT.rMMFromPoint(rPreferredHeight); }
      set { rPreferredHeight = RT.rPointFromMM(value); }
    }

  }
}
