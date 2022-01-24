using System;
using System.Drawing;

// Creation date: 15.02.2004
// Checked: xx.08.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.02

// copyright (C) 2004 root-software ag  -  Bürglen Switzerland  -  www.root.ch; Otto Mayer, Stefan Spirig, Roger Gartenmann
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  //------------------------------------------------------------------------------------------14.03.2004
  #region RepArcBase
  //----------------------------------------------------------------------------------------------------

  /// <summary>Base Class of the Arc, Circle, Ellipse and Pie Objects</summary>
  /// <remarks>
  /// This class is the base class of the <see cref="RepArc"/>, <see cref="RepCircle"/>, <see cref="RepEllipse"/> and <see cref="RepPie"/> classes.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepArcBase"]/*'/>
  public abstract class RepArcBase : RepObj {
#if !DEVELOPER  // bs
    /// <summary>Pen properties of the border line</summary>
    internal PenProp _penProp;

    /// <summary>Brush properties of the pie or circle</summary>
    internal BrushProp _brushProp;
#endif

    /// <summary>Angle in degrees measured clockwise from the x-axis to the first side of the pie section</summary>
    internal Double _rStartAngle;
    
    /// <summary>Angle in degrees measured clockwise from the startAngle parameter to the second side of the pie section</summary>
    internal Double _rSweepAngle;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes all parameters for an arc, circle, ellipse or pie.</summary>
    /// <remarks>
    /// This constructor must be called by all derived classes.
    /// The pen properties <paramref name="penProp"/> can be set to draw a border line.
    /// If the brush properties <paramref name="brushProp"/> are set, the interior of the shape will be filled.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
    /// <paramref name="rHeight"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> can be used to define a portion of an ellipse.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start point of the arc</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the arc</param>
    public RepArcBase(PenProp penProp, BrushProp brushProp, Double rWidth, Double rHeight, Double rStartAngle, Double rSweepAngle) {
      if (penProp != null) {
#if DEVELOPER  // bs
        this.graphicsState.penProp = penProp.penProp_Registered;
#else
        this._penProp = penProp.penProp_Registered;
#endif
      }
      if (brushProp != null) {
#if DEVELOPER  // bs
        this.graphicsState.brushProp = brushProp.brushProp_Registered;
#else
        this._brushProp = brushProp.brushProp_Registered;
#endif
      }
      this.rWidth = rWidth;
      this.rHeight = rHeight;
      this._rStartAngle = rStartAngle;
      this._rSweepAngle = rSweepAngle;

      oRepObjX = report.formatter.oCreate_RepArcBase();
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the report to which the report object belongs.</summary>
    private new Report report {
      get {
        if (_penProp != null) {
          return _penProp.report;
        }
        if (_brushProp != null) {
          return _brushProp.report;
        }
        return base.report;
      }
    }

    //----------------------------------------------------------------------------------------------------
    /// <summary>Calculates the x- and y-coordinates of the ellipse for the specified angle.</summary>
    /// <param name="rAngle">Angle in radians measured clockwise from the x-axis</param>
    /// <param name="rX">x-coordinate in points (1/72 inch)</param>
    /// <param name="rY">y-coordinate in points (1/72 inch)</param>
    internal void GetEllipseXY(Double rAngle, out Double rX, out Double rY) {
      const Double rPi1_2 = Math.PI / 2.0;
      const Double rPi3_2 = Math.PI / 2.0 * 3.0;
      rAngle = rAngle - Math.Floor(rAngle / 2.0 / Math.PI) * 2.0 * Math.PI;
      Double rA = rWidth / 2.0;
      Double rB = rHeight / 2.0;

      if (RT.bEquals(rAngle, rPi1_2, 0.0001)) {
        rX = 0;
        rY = -rB;
        return;
      }
      if (RT.bEquals(rAngle, rPi3_2, 0.0001)) {
        rX = 0;
        rY = rB;
        return;
      }

      // tan(@) = y/x  ==> y = x tan(@)                   @ != 0
      // x^2/a^2 + y^2/b^2 = 1
      // ==> 1. x^2/a^2 + x^2 tan(@)^2 / b^2 = 1
      //     2. x^2 (1/a^2 + tan(@)^2 / b^2) = 1
      //     3. x^2 = 1 / (1/a^2 + tan(@)^2 / b^2)
      //     4. x = SQRT(1 / (1/a^2 + tan(@)^2 / b^2))
      Double r = Math.Tan(-rAngle);
      r = 1.0 / rA / rA + r * r / rB / rB;
      rX = Math.Sqrt(1 / r);
      if (rAngle > rPi1_2 && rAngle < rPi3_2) {
        rX = -rX;
      }
      
      // y = x tan(@)
      rY = rX * Math.Tan(-rAngle);
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------14.03.2004
  #region RepArc
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Arc Object</summary>
  /// <remarks>
  /// This object draws an arc representing a portion of an ellipse or circle.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepArc"]/*'/>
  public class RepArc : RepArcBase {
    //------------------------------------------------------------------------------------------04.03.2004
    /// <overloads>
    /// <summary>Creates an arc representing a portion of an ellipse or circle.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The arc represents a portion of an ellipse or circle.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates an arc representing a portion of an ellipse specified by the bounding rectangle in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The arc represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
    /// </remarks>
    /// <param name="penProp">Pen properties of the arc</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the arc comes</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the arc comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the arc</param>
    public RepArc(PenProp penProp, Double rWidth, Double rHeight, Double rStartAngle, Double rSweepAngle)
      : base(penProp, null, rWidth, rHeight, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates an arc representing a portion of a circle specified by the radius in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The arc represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
    /// </remarks>
    /// <param name="penProp">Pen properties of the arc</param>
    /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the arc</param>
    public RepArc(PenProp penProp, Double rRadius, Double rStartAngle, Double rSweepAngle)
      : this(penProp, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------04.03.2004
  #region RepArcMM
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Arc Object (metric version)</summary>
  /// <remarks>
  /// This object draws an arc representing a portion of an ellipse or circle.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepArc"]/*'/>
  public class RepArcMM : RepArc {
    //------------------------------------------------------------------------------------------04.03.2004
    /// <overloads>
    /// <summary>Creates an arc representing a portion of an ellipse or circle.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The arc represents a portion of an ellipse or circle.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates an arc representing a portion of an ellipse specified by the bounding rectangle in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The arc represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
    /// </remarks>
    /// <param name="penProp">Pen properties of the arc</param>
    /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the arc comes</param>
    /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the arc comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the arc</param>
    public RepArcMM(PenProp penProp, Double rWidthMM, Double rHeightMM, Double rStartAngle, Double rSweepAngle)
      : base(penProp, RT.rPointFromMM(rWidthMM), RT.rPointFromMM(rHeightMM), rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates an arc representing a portion of a circle specified by the radius in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The arc represents a portion of a circle that is defined by the parameter <paramref name="rRadiusMM"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and end point of the arc.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the arc</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the arc</param>
    public RepArcMM(PenProp penProp, Double rRadiusMM, Double rStartAngle, Double rSweepAngle)
      : this(penProp, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------03.03.2004
  #region RepCircle
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Circle Object</summary>
  /// <remarks>
  /// This object draws a circle that may have a border line and/or may be filled.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepCircle"]/*'/>
  public class RepCircle : RepArcBase {
    //------------------------------------------------------------------------------------------03.03.2004
    /// <overloads>
    /// <summary>Creates a circle.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The circle is defined by the parameter <paramref name="rRadius"/>.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a filled circle with a border line defined by the radius in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The circle is defined by the parameter <paramref name="rRadius"/>.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
    public RepCircle(PenProp penProp, BrushProp brushProp, Double rRadius) : base(penProp, brushProp, rRadius * 2.0, rRadius * 2.0, 0.0, 360.0) {
    }

    //------------------------------------------------------------------------------------------03.03.2004
    /// <summary>Creates a circle with a border line defined by the radius in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The circle is defined by the parameter <paramref name="rRadius"/>.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
    public RepCircle(PenProp penProp, Double rRadius) : this(penProp, null, rRadius) {
    }

    //------------------------------------------------------------------------------------------03.03.2004
    /// <summary>Creates a filled circle defined by the radius in points (1/72 inch).</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The circle is defined by the parameter <paramref name="rRadius"/>.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
    public RepCircle(BrushProp brushProp, Double rRadius) : this(null, brushProp, rRadius) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------03.03.2004
  #region RepCircleMM
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Circle Object (metric version)</summary>
  /// <remarks>
  /// This object draws a circle that may have a border line and/or may be filled.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepCircle"]/*'/>
  public class RepCircleMM : RepCircle {
    //------------------------------------------------------------------------------------------03.03.2004
    /// <overloads>
    /// <summary>Creates a circle (metric version).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a filled circle with a border line defined by the radius in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
    public RepCircleMM(PenProp penProp, BrushProp brushProp, Double rRadiusMM) : base(penProp, brushProp, RT.rPointFromMM(rRadiusMM)) {
    }

    //------------------------------------------------------------------------------------------03.03.2004
    /// <summary>Creates a circle with a border line defined by the radius in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
    public RepCircleMM(PenProp penProp, Double rRadiusMM) : this(penProp, null, rRadiusMM) {
    }

    //------------------------------------------------------------------------------------------03.03.2004
    /// <summary>Creates a filled circle defined by the radius in millimeters.</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The circle is defined by the parameter <paramref name="rRadiusMM"/>.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
    public RepCircleMM(BrushProp brushProp, Double rRadiusMM) : this(null, brushProp, rRadiusMM) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------04.03.2004
  #region RepEllipse
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Ellipse Object</summary>
  /// <remarks>
  /// This object draws an ellipse that may have a border line and/or may be filled.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepEllipse"]/*'/>
  public class RepEllipse : RepArcBase {
    //------------------------------------------------------------------------------------------04.03.2004
    /// <overloads>
    /// <summary>Creates an ellipse.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
    /// <paramref name="rHeight"/> parameters.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a filled ellipse with a border line specified by the bounding rectangle in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
    /// <paramref name="rHeight"/> parameters.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    public RepEllipse(PenProp penProp, BrushProp brushProp, Double rWidth, Double rHeight)
      : base(penProp, brushProp, rWidth, rHeight, 0.0, 360.0) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled ellipse with a border line specified by the bounding rectangle in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
    /// <paramref name="rHeight"/> parameters.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    public RepEllipse(PenProp penProp, Double rWidth, Double rHeight)
      : this(penProp, null, rWidth, rHeight) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled ellipse specified by the bounding rectangle in points (1/72 inch).</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidth"/> and
    /// <paramref name="rHeight"/> parameters.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse</param>
    public RepEllipse(BrushProp brushProp, Double rWidth, Double rHeight)
      : this(null, brushProp, rWidth, rHeight) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------04.03.2004
  #region RepEllipseMM
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Ellipse Object (metric version)</summary>
  /// <remarks>
  /// This object draws an ellipse that may have a border line and/or may be filled.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepEllipse"]/*'/>
  public class RepEllipseMM : RepEllipse {
    //------------------------------------------------------------------------------------------04.03.2004
    /// <overloads>
    /// <summary>Creates an ellipse (metric version).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
    /// <paramref name="rHeightMM"/> parameters.
    /// </remarks>
    /// </overloads>
    ///
    /// <summary>Creates a filled ellipse with a border line specified by the bounding rectangle in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
    /// <paramref name="rHeightMM"/> parameters.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse</param>
    /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse</param>
    public RepEllipseMM(PenProp penProp, BrushProp brushProp, Double rWidthMM, Double rHeightMM)
      : base(penProp, brushProp, RT.rPointFromMM(rWidthMM), RT.rPointFromMM(rHeightMM)) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates an ellipse with a border line specified by the bounding rectangle in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
    /// <paramref name="rHeightMM"/> parameters.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse</param>
    /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse</param>
    public RepEllipseMM(PenProp penProp, Double rWidthMM, Double rHeightMM)
      : this(penProp, null, rWidthMM, rHeightMM) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled ellipse specified by the bounding rectangle in millimeters.</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The ellipse is defined by the bounding rectangle described by the <paramref name="rWidthMM"/> and
    /// <paramref name="rHeightMM"/> parameters.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse</param>
    /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse</param>
    public RepEllipseMM(BrushProp brushProp, Double rWidthMM, Double rHeightMM)
      : this(null, brushProp, rWidthMM, rHeightMM) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------04.03.2004
  #region RepPie
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Pie Object</summary>
  /// <remarks>
  /// This object draws a pie shape defined by an ellipse or a circle and two radial lines.
  /// </remarks>
  /// <include file='D:\Programs\DotNet03\Root\Reports\Docu\RepObj.xml' path='doc/sample[@name="RepPie"]/*'/>
  public class RepPie : RepArcBase {
    //------------------------------------------------------------------------------------------04.03.2004
    /// <overloads>
    /// <summary>Creates a pie shape defined by an ellipse or a circle and two radial lines.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse or circle.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a filled pie section with a border line defined by an ellipse specified by the bounding
    /// rectangle in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPie(PenProp penProp, BrushProp brushProp, Double rWidth, Double rHeight, Double rStartAngle, Double rSweepAngle)
      : base(penProp, brushProp, rWidth, rHeight, rStartAngle, rSweepAngle)
    {}

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a pie section with a border line defined by an ellipse specified by the bounding
    /// rectangle in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPie(PenProp penProp, Double rWidth, Double rHeight, Double rStartAngle, Double rSweepAngle)
      : this(penProp, null, rWidth, rHeight, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled pie section defined by an ellipse specified by the bounding rectangle in points (1/72 inch).</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidth"/> and <paramref name="rHeight"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidth">Width in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rHeight">Height in points (1/72 inch) of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPie(BrushProp brushProp, Double rWidth, Double rHeight, Double rStartAngle, Double rSweepAngle)
      : this(null, brushProp, rWidth, rHeight, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled pie section with a border line defined by a circle specified by the radius in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the rStartAngle parameter to the second side of the pie section</param>
    public RepPie(PenProp penProp, BrushProp brushProp, Double rRadius, Double rStartAngle, Double rSweepAngle)
      : this(penProp, brushProp, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle) {
     }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a pie section with a border line defined by a circle specified by the radius in points (1/72 inch).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The pie shape represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the rStartAngle parameter to the second side of the pie section</param>
    public RepPie(PenProp penProp, Double rRadius, Double rStartAngle, Double rSweepAngle)
      : this(penProp, null, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled pie section defined by a circle specified by the radius in points (1/72 inch).</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of a circle that is defined by the parameter <paramref name="rRadius"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadius">Radius of the circle in points (1/72 inch)</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the first side of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the rStartAngle parameter to the second side of the pie section</param>
    public RepPie(BrushProp brushProp, Double rRadius, Double rStartAngle, Double rSweepAngle)
      : this(null, brushProp, rRadius * 2.0, rRadius * 2.0, rStartAngle, rSweepAngle) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------04.03.2004
  #region RepPieMM
  //----------------------------------------------------------------------------------------------------
  
  /// <summary>Report Pie Object (metric version)</summary>
  public class RepPieMM : RepPie {
    //------------------------------------------------------------------------------------------04.03.2004
    /// <overloads>
    /// <summary>Creates a pie shape defined by an ellipse or a circle and two radial lines (metric version).</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse or circle.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a filled pie section with a border line defined by an ellipse specified by the bounding
    /// rectangle in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPieMM(PenProp penProp, BrushProp brushProp, Double rWidthMM, Double rHeightMM, Double rStartAngle, Double rSweepAngle)
      : base(penProp, brushProp, RT.rPointFromMM(rWidthMM), RT.rPointFromMM(rHeightMM), rStartAngle, rSweepAngle)
    {}

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a pie section with a border line defined by an ellipse specified by the bounding rectangle in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPieMM(PenProp penProp, Double rWidthMM, Double rHeightMM, Double rStartAngle, Double rSweepAngle)
      : this(penProp, null, rWidthMM, rHeightMM, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled pie section defined by an ellipse specified by the bounding rectangle in millimeters.</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse that is defined by the bounding rectangle described by the
    /// <paramref name="rWidthMM"/> and <paramref name="rHeightMM"/> parameters.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rWidthMM">Width in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rHeightMM">Height in millimeters of the bounding rectangle that defines the ellipse from which the pie section comes</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPieMM(BrushProp brushProp, Double rWidthMM, Double rHeightMM, Double rStartAngle, Double rSweepAngle)
      : this(null, brushProp, rWidthMM, rHeightMM, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled pie section with a border line defined by a circle specified by the radius in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse that is defined by the parameter <paramref name="rRadiusMM"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPieMM(PenProp penProp, BrushProp brushProp, Double rRadiusMM, Double rStartAngle, Double rSweepAngle)
      : this(penProp, brushProp, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a pie section with a border line defined by a circle specified by the radius in millimeters.</summary>
    /// <remarks>
    /// The pen properties object <paramref name="penProp"/> determines the characteristics of the border line.
    /// The pie shape represents a portion of an ellipse that is defined by the parameter <paramref name="rRadiusMM"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="penProp">Pen properties of the border line</param>
    /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPieMM(PenProp penProp, Double rRadiusMM, Double rStartAngle, Double rSweepAngle)
      : this(penProp, null, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle) {
    }

    //------------------------------------------------------------------------------------------04.03.2004
    /// <summary>Creates a filled pie section defined by a circle specified by the radius in millimeters.</summary>
    /// <remarks>
    /// The brush properties object <paramref name="brushProp"/> determines the characteristics of the fill.
    /// The pie shape represents a portion of an ellipse that is defined by the parameter <paramref name="rRadiusMM"/>.
    /// The parameters <paramref name="rStartAngle"/> and <paramref name="rSweepAngle"/> define the start and
    /// end point of the radial lines.
    /// </remarks>
    /// <param name="brushProp">Brush properties of the fill</param>
    /// <param name="rRadiusMM">Radius of the circle in millimeters</param>
    /// <param name="rStartAngle">Angle in degrees measured clockwise from the x-axis to the start position of the pie section</param>
    /// <param name="rSweepAngle">Angle in degrees measured clockwise from the <paramref name="rStartAngle"/> parameter to
    /// the end point of the pie section</param>
    public RepPieMM(BrushProp brushProp, Double rRadiusMM, Double rStartAngle, Double rSweepAngle)
      : this(null, brushProp, rRadiusMM * 2.0, rRadiusMM * 2.0, rStartAngle, rSweepAngle) {
    }
  }
  #endregion
}
