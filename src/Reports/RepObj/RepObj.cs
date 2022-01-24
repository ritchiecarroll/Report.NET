using System;
using System.Diagnostics;

// Creation date: 22.04.2002
// Checked: 31.07.2002
// Author: Otto Mayer, mot@root.ch
// Version 1.00.01 

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Base class of all report objects.</summary>
  public abstract class RepObj {
    /// <summary>Container to which the report object belongs. Must be null for a page.</summary>
    internal Container container;

    /// <summary>Transformation matrix.</summary>
    internal MatrixD matrixD = new MatrixD(1, 0, 0, 1, 0, 0);

    /// <summary>Height of the report object.</summary>
    private Double _rHeight;

    /// <summary>Width of the report object.</summary>
    private Double _rWidth;

#if DEVELOPER  // bs
    /// <summary>Path used for clipping.</summary>
    private RepObj _clipPath;
#endif

    /// <summary>Horizontal alignment of the report object relative to [pointF_Pos].</summary>
    public Double rAlignH = 0;

    /// <summary>Vertical alignment of the report object relative to [pointF_Pos].</summary>
    public Double rAlignV = 1;

    /// <summary>Horizontal alignment: left</summary>
    public const Double rAlignLeft = 0;
    /// <summary>Vertical alignment: top</summary>
    public const Double rAlignTop = 0;
    /// <summary>Horizontal or vertical alignment: center</summary>
    public const Double rAlignCenter = 0.5;
    /// <summary>Horizontal alignment: right</summary>
    public const Double rAlignRight = 1;
    /// <summary>Vertical alignment: bottom</summary>
    public const Double rAlignBottom = 1;

#if DEVELOPER  // bs
    /// <summary>Pen and Brush states</summary>
    private GraphicsState _graphicsState;
#endif

    internal Object oRepObjX;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new instance of a report object class.</summary>
    /*protected !!!*/public RepObj() {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the page to which the report object belongs.</summary>
    internal Page page {
      get {
        if (this is Page) {
          return (Page)this;
        }
        Container c = container;
        while (c.container != null) {
          c = c.container;
        }
        return (Page)c;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Sets or gets the height of this report object.</summary>
    public virtual Double rHeight {
      set { _rHeight = value; }
      get { return _rHeight; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Sets or gets the height of this report object in millimeter.</summary>
    public Double rHeightMM {
      set { rHeight = RT.rPointFromMM(value); }
      get { return RT.rMMFromPoint(rHeight); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the left side of this report object (points, 1/72 inch).</summary>
    public Double rPosLeft {
      get { return matrixD.rDX - rWidth * rAlignH; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the left side of this report object (mm).</summary>
    public Double rPosLeftMM {
      get { return RT.rMMFromPoint(rPosLeft); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the right side of this report object (points, 1/72 inch).</summary>
    public Double rPosRight {
      get { return matrixD.rDX + rWidth * (1.0 - rAlignH); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the right side of this report object (mm).</summary>
    public Double rPosRightMM {
      get { return RT.rMMFromPoint(rPosRight); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the top side of this report object (points, 1/72 inch).</summary>
    public virtual Double rPosTop {
      get { return matrixD.rDY - rHeight * rAlignV; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the top side of this report object (mm).</summary>
    public Double rPosTopMM {
      get { return RT.rMMFromPoint(rPosTop); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the bottom of this report object (points, 1/72 inch).</summary>
    public virtual Double rPosBottom {
      get { return matrixD.rDY + rHeight * (1.0 - rAlignV); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the bottom side of this report object (mm).</summary>
    public Double rPosBottomMM {
      get { return RT.rMMFromPoint(rPosBottom); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the report to which the report object belongs.</summary>
    internal Report report {
      get { return page.report; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Sets or gets the width of this report object.</summary>
    public virtual Double rWidth {
      set { _rWidth = value; }
      get { return _rWidth; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Sets or gets the width of this report object in millimeter.</summary>
    public Double rWidthMM {
      set { rWidth = RT.rPointFromMM(value); }
      get { return RT.rMMFromPoint(rWidth); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the horizontal position of this report object relative to its container (points, 1/72 inch).</summary>
    public Double rX {
      get { return matrixD.rDX; }
      set { matrixD.rDX = value; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the horizontal position of this report object relative to its container (mm).</summary>
    public Double rX_MM {
      get { return RT.rMMFromPoint(matrixD.rDX); }
      set { matrixD.rDX = RT.rPointFromMM(value); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the vertical position of this report object relative to its container.</summary>
    public Double rY {
      get { return matrixD.rDY; }
#if DEVELOPER  // bs
      set { matrixD.rDY = value; }
#endif
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the vertical position (millimeter) of this report object relative to its container.</summary>
    public Double rY_MM {
      get { return RT.rMMFromPoint(matrixD.rDY); }
#if DEVELOPER  // bs
      set { matrixD.rDY = RT.rPointFromMM(value); }
#endif
    }

    //----------------------------------------------------------------------------------------------------x
#if DEVELOPER  // bs
    /// <summary>Gets the clipping rectangle.</summary>
    public RepObj ClipPath {
      get { return this._clipPath; }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the graphics state.</summary>
    internal GraphicsState graphicsState {
      get {
        if (_graphicsState == null) {
          _graphicsState = new GraphicsState(this, null, null);
        }
        return _graphicsState;
      }
    }
#endif

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called after the report object has been added to the container.</summary>
    internal protected virtual void OnAdded() {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Removes this report object from the container.</summary>
    public void Remove() {
      container.Remove(this);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Applies the specified rotation to the transformation matrix of this report object.</summary>
    /// <param name="rAngle">Angle of rotation in degrees</param>
    public void RotateTransform(Double rAngle) {
      matrixD.Rotate(rAngle);
    }

#if DEVELOPER  // bs
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Applies the specified clipping path to this report object.</summary>
    /// <param name="rX">X-coordinate of the clip path.</param>
    /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
    /// <param name="rY">Y-coordinate of the clip path.</param>
    /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
    /// <param name="repObj">The clipping path.</param>
    public void SetClip(Double rX, Double rAlignH, Double rY, Double rAlignV, RepObj repObj) {
      this._clipPath = repObj; 
      this._clipPath.matrixD.rDX = rX;
      this._clipPath.rAlignH = rAlignH;
      this._clipPath.matrixD.rDY = rY;
      this._clipPath.rAlignV = rAlignV;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Applies the specified clipping path to this report object.</summary>
    /// <param name="rX">X-coordinate of the clip path, in millimetres.</param>
    /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
    /// <param name="rY">Y-coordinate of the clip path, in millimetres.</param>
    /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
    /// <param name="repObj">The clipping path.</param>
    public void SetClip_MM(Double rX, Double rAlignH, Double rY, Double rAlignV, RepObj repObj) {
      SetClip(RT.rPointFromMM(rX), rAlignH, RT.rPointFromMM(rY), rAlignV, repObj);
    }
#endif
  }
}
