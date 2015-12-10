#if DEVELOPER  // bs
using System;
using System.Drawing;

// Creation date: 22.04.2002
// Checked: 05.08.2002
// Author: Brad Stoney - bstoney@gmail.com
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
  public class RepPoly : RepObj {
    /// <summary>X coodinate points.</summary>
    public Double[] rXPoints;
    /// <summary>Y coodinate points.</summary>
    public Double[] rYPoints;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object with a border line.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rXPoints">X-coodinate of points, relative to the start point.</param>
    /// <param name="rYPoints">Y-coodinate of points, relative to the start point.</param>
    public RepPoly(PenProp penProp, BrushProp brushProp, Double[] rXPoints, Double[] rYPoints) {
      graphicsState.penProp = penProp.penProp_Registered;
      graphicsState.brushProp = brushProp.brushProp_Registered;
      this.rXPoints = rXPoints;
      this.rYPoints = rYPoints;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new rectangle object.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rXPoints">X-coodinate of points, relative to the start point.</param>
    /// <param name="rYPoints">Y-coodinate of points, relative to the start point.</param>
    public RepPoly(PenProp penProp, Double[] rXPoints, Double[] rYPoints) {
      graphicsState.penProp = penProp.penProp_Registered;
      this.rWidth = rWidth;
      this.rHeight = rHeight;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object without a border line.</summary>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rXPoints">X-coodinate of points, relative to the start point.</param>
    /// <param name="rYPoints">Y-coodinate of points, relative to the start point.</param>
    public RepPoly(BrushProp brushProp, Double[] rXPoints, Double[] rYPoints) {
      graphicsState.brushProp = brushProp.brushProp_Registered;
      this.rWidth = rWidth;
      this.rHeight = rHeight;
    }
  }

  //****************************************************************************************************
  /// <summary>Report Rectangle Object with millimeter values.</summary>
  public class RepPolyMM : RepPoly {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object with a border line and millimeter values.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rXPoints">X-coodinate of points, relative to the start point, in millimeters.</param>
    /// <param name="rYPoints">Y-coodinate of points, relative to the start point, in millimeters.</param>
    public RepPolyMM(PenProp penProp, BrushProp brushProp, Double[] rXPoints, Double[] rYPoints) : base(penProp, brushProp, null, null) {
      this.rXPoints = (Double[])Array.CreateInstance(typeof(Double), rXPoints.Length);
      this.rYPoints = (Double[])Array.CreateInstance(typeof(Double), rYPoints.Length);
      for(int i = 0; i < rXPoints.Length; i++) {
        this.rXPoints[i] = RT.rPointFromMM(rXPoints[i]);
        this.rYPoints[i] = RT.rPointFromMM(rYPoints[i]);
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new rectangle object with millimeter values.</summary>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rXPoints">X-coodinate of points, relative to the start point, in millimeters.</param>
    /// <param name="rYPoints">Y-coodinate of points, relative to the start point, in millimeters.</param>
    public RepPolyMM(PenProp penProp, Double[] rXPoints, Double[] rYPoints) : base(penProp, null, null) {
      this.rXPoints = (Double[])Array.CreateInstance(typeof(Double), rXPoints.Length);
      this.rYPoints = (Double[])Array.CreateInstance(typeof(Double), rYPoints.Length);
      for(int i = 0; i < rXPoints.Length; i++) {
        this.rXPoints[i] = RT.rPointFromMM(rXPoints[i]);
        this.rYPoints[i] = RT.rPointFromMM(rYPoints[i]);
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new filled rectangle object without a border line and with millimeter values.</summary>
    /// <param name="brushProp">Brush properties of the rectangle</param>
    /// <param name="rXPoints">X-coodinate of points, relative to the start point, in millimeters.</param>
    /// <param name="rYPoints">Y-coodinate of points, relative to the start point, in millimeters.</param>
    public RepPolyMM(BrushProp brushProp, Double[] rXPoints, Double[] rYPoints) : base(brushProp, null, null) {
      this.rXPoints = (Double[])Array.CreateInstance(typeof(Double), rXPoints.Length);
      this.rYPoints = (Double[])Array.CreateInstance(typeof(Double), rYPoints.Length);
      for(int i = 0; i < rXPoints.Length; i++) {
        this.rXPoints[i] = RT.rPointFromMM(rXPoints[i]);
        this.rYPoints[i] = RT.rPointFromMM(rYPoints[i]);
      }
    }
  }
}
#endif
