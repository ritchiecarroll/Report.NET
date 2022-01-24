using System;
using System.Drawing;

// Creation date: 22.04.2002
// Checked: 05.08.2002
// Author: Otto Mayer, mot@root.ch
// Version: 1.00

// copyright (C) 2002 root-software ag  -  Bürglen Switzerland  -  www.root.ch; Otto Mayer, Stefan Spirig, Roger Gartenmann
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Report Rectangle Object.</summary>
  public class RepRect : RepObj {
#if !DEVELOPER  // bs
    /// <summary>Pen properties of the border line</summary>
    public PenProp penProp;

    /// <summary>Brush properties of the rectangle</summary>
    public BrushProp brushProp;
#endif

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object with a border line.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rWidth">Width of the rectangle</param>
    /// <param name="rHeight">Height of the rectangle</param>
    public RepRect(PenProp penProp, BrushProp brushProp, Double rWidth, Double rHeight) {
#if DEVELOPER  // bs
      if (penProp != null) {
        this.graphicsState.penProp = penProp.penProp_Registered;
      }
      if (brushProp != null) {
        this.graphicsState.brushProp = brushProp.brushProp_Registered;
      }
#else
      if (penProp != null) {
        this.penProp = penProp.penProp_Registered;
      }
      if (brushProp != null) {
        this.brushProp = brushProp.brushProp_Registered;
      }
#endif
      this.rWidth = rWidth;
      this.rHeight = rHeight;
      if (penProp == null) {
        oRepObjX = brushProp.report.formatter.oCreate_RepRect();
      }
      else {
        oRepObjX = penProp.report.formatter.oCreate_RepRect();
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new rectangle object.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rWidth">Width of the rectangle</param>
    /// <param name="rHeight">Height of the rectangle</param>
    public RepRect(PenProp penProp, Double rWidth, Double rHeight) : this(penProp, null, rWidth, rHeight) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object without a border line.</summary>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rWidth">Width of the rectangle</param>
    /// <param name="rHeight">Height of the rectangle</param>
    public RepRect(BrushProp brushProp, Double rWidth, Double rHeight) : this(null, brushProp, rWidth, rHeight) {
    }

  }

  //****************************************************************************************************
  /// <summary>Report Rectangle Object with millimeter values.</summary>
  public class RepRectMM : RepRect {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object with a border line and millimeter values.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rWidth">Width of the rectangle</param>
    /// <param name="rHeight">Height of the rectangle</param>
    public RepRectMM(PenProp penProp, BrushProp brushProp, Double rWidth, Double rHeight) : base(penProp, brushProp, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new rectangle object with millimeter values.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rWidth">Width of the rectangle</param>
    /// <param name="rHeight">Height of the rectangle</param>
    public RepRectMM(PenProp penProp, Double rWidth, Double rHeight) : base(penProp, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object without a border line and with millimeter values.</summary>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rWidth">Width of the rectangle</param>
    /// <param name="rHeight">Height of the rectangle</param>
    public RepRectMM(BrushProp brushProp, Double rWidth, Double rHeight) : base(brushProp, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight)) {
    }
  }
}