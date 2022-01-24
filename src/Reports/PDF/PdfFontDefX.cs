using System;
using System.Diagnostics;

// Creation date: 19.01.2006
// Checked: 07.02.2006
// Author: Otto Mayer (mot@root.ch)
// Version: 1.05

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  //------------------------------------------------------------------------------------------17.02.2006
  #region PdfTrueTypeFontDataX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Extended PDF Container Class</summary>
  internal sealed class PdfTrueTypeFontDataX {
    Boolean bEmbedded = true;

    //------------------------------------------------------------------------------------------09.02.2006
    /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
    private PdfTrueTypeFontDataX() {
    }
  #endregion
  }
}
