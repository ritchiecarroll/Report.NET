using System;
using System.Diagnostics;
using System.Text;

// Creation date: 22.04.2002
// Checked: xx.05.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary></summary>
  internal sealed class PdfFontPropX {
    internal PdfIndirectObject_Font pdfIndirectObject_Font = null;

    internal readonly String sKey;

    /// <summary>This variable allows a quick test, whether the font properties are registered for the current page.
    /// If <c>pdfPageData_Registered</c> contains the current page, then it has been registered before.</summary>
    private PdfIndirectObject_Page _pdfPageData_Registered;  // !!! variable name

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    /// <param name="sKey"></param>
    /// <param name="afm"></param>
    public PdfFontPropX(String sKey, FontProp fontProp) : base() {
      this.sKey = sKey;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This variable allows a quick test, whether the font properties are registered for the current page.
    /// If <c>pdfPageData_Registered</c> contains the current page, then it has been registered before.</summary>
    internal PdfIndirectObject_Page pdfPageData_Registered {  // !!! property name
      get { return _pdfPageData_Registered; }
      set {
        _pdfPageData_Registered = value;
      }
    }
  }
}
