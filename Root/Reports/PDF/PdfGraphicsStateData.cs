#if DEVELOPER // Brad Stoney
using System;
using System.Diagnostics;
using System.Text;

// Creation date: 30.11.2004
// Checked: 
// Author: Brad Stoney
// Version: 

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary></summary>
  public sealed class PdfGraphicsStateData : GraphicsStateData {
    /// <summary>ID of the PDF object</summary>
    internal Int32 iObjId;

    internal readonly String sKey;

    /// <summary>This variable allows a quick test, whether the font properties are registered for the current page.
    /// If <c>pdfPageData_Registered</c> contains the current page, then it has been registered before.</summary>
    private PdfPageData _pdfPageData_Registered;
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    /// <param name="sKey"></param>
    public PdfGraphicsStateData(String sKey) {
      this.sKey = sKey;
      Init();
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    internal protected override void Init() {
      iObjId = 0;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This variable allows a quick test, whether the font properties are registered for the current page.
    /// If <c>pdfPageData_Registered</c> contains the current page, then it has been registered before.</summary>
    internal PdfPageData pdfPageData_Registered {
      get { return _pdfPageData_Registered; }
      set {
#if (DEBUG)
        StackFrame sf = new StackFrame(1);
        Debug.Assert(sf.GetMethod().Name == "RegisterGraphicsState");
#endif
        _pdfPageData_Registered = value;
      }
    }
  }
}
#endif
