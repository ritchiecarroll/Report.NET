using System;
//using System.Collections;
 
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
  /// <summary>Grapics State Data Class</summary>
  public abstract class GraphicsStateData {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new graphics state data object.</summary>
    internal GraphicsStateData() {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary></summary>
    internal protected abstract void Init();

  }
}
