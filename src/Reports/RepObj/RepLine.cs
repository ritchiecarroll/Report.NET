using System;
using System.Drawing;

// Creation date: 22.04.2002
// Checked: 05.08.2002
// Author: Otto Mayer, mot@root.ch
// Version 1.00.00

// copyright (C) 2002 root-software ag  -  Bürglen Switzerland  -  www.root.ch; Otto Mayer, Stefan Spirig, Roger Gartenmann
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Report Line Object.</summary>
  public class RepLine : RepObj {
#if !DEVELOPER  // bs
    /// <summary>Pen properties of the line</summary>
    public PenProp penProp;
#endif

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new line object.</summary>
    /// <param name="penProp">Pen properties of the line</param>
    /// <param name="rX">X-coordinate of the end of the line, relative to the start point</param>
    /// <param name="rY">Y-coordinate of the end of the line, relative to the start point</param>
    public RepLine(PenProp penProp, Double rX, Double rY) {
#if DEVELOPER  // bs
      this.graphicsState.penProp = penProp.penProp_Registered;
#else
      this.penProp = penProp.penProp_Registered;
#endif
      this.rWidth = rX;
      this.rHeight = rY;
      oRepObjX = penProp.report.formatter.oCreate_RepLine();
    }

  }

  //****************************************************************************************************
  /// <summary>Report Line Object with millimeter values.</summary>
  public class RepLineMM : RepLine {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new line object with millimeter values</summary>
    /// <param name="penProp">Pen properties of the line</param>
    /// <param name="rX">X-coordinate of the end of the line, relative to the start point, in millimeter</param>
    /// <param name="rY">Y-coordinate of the end of the line, relative to the start point, in millimeter</param>
    public RepLineMM(PenProp penProp, Double rX, Double rY) : base(penProp, RT.rPointFromMM(rX), RT.rPointFromMM(rY)) {
    }

  }
}
