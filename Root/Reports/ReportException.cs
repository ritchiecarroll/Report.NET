using System;

// Creation date: 02.12.2002
// Checked: 05.07.2004
// Author: Otto Mayer (mot@root.ch)
// Version: 1.03

// Report.NET copyright 2002-2004 root-service ag, Bürglen Switzerland - O. Mayer, S. Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Report Exception</summary>
  public class ReportException : SystemException {
    //------------------------------------------------------------------------------------------05.07.2004
    /// <summary>Creates a report exception.</summary>
    /// <param name="sMsg">Error message</param>
    internal ReportException(String sMsg) : base(sMsg) {
    }
    
    //------------------------------------------------------------------------------------------05.07.2004
    /// <summary>Creates a report exception.</summary>
    /// <param name="sMsg">Error message</param>
    /// <param name="exception_Inner">Inner exception</param>
    internal ReportException(String sMsg, Exception exception_Inner) : base(sMsg, exception_Inner) {
    }
  }
}
