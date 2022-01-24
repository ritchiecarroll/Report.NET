using System;
using System.Drawing;

// Creation date: 02.05.2002
// Checked: 06.05.2006
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
  #region
  /// <summary>Defines the properties of a pen.</summary>
  /// <remarks>The pen property object defines the style of a pen.</remarks>
  /// <example>Pen property sample:
  /// <code>
  /// using Root.Reports;
  /// using System;
  /// using System.Drawing;
  ///
  /// public class PenPropSample : Report {
  ///   public static void Main() {
  ///     PdfReport&lt;PenPropSample&gt; pdfReport = new PdfReport&lt;PenPropSample&gt;();
  ///     pdfReport.View("PenPropSample.pdf");
  ///   }
  ///
  ///   protected override void Create() {
  ///     <b>PenProp penProp = new PenProp(this, 1.5, Color.Red)</b>;
  ///     new Page(this);
  ///     page_Cur.AddLT_MM(30, 30, new RepRectMM(<b>penProp</b>, 150, 60));
  ///   }
  /// }
  /// </code>
  /// </example>
  #endregion
  public class PenProp {
    /// <summary>Null value</summary>
    public static readonly PenProp penProp_Null = new PenProp(null, 0);

    /// <summary>Report to which this pen belongs</summary>
    internal readonly Report report;

    /// <summary>Width of the pen</summary>
    private Double _rWidth;

    /// <summary>Color of the pen</summary>
    private Color _color;

    /// <summary>Number of 1/72-units of the on-pattern</summary>
    private Double _rPatternOn;

    /// <summary>Number of 1/72-units of the off-pattern</summary>
    private Double _rPatternOff;
 
    /// <summary>Reference to the same but registered property object.
    /// If null, it has not yet been used and therefore it is not registered.</summary>
    private PenProp _penProp_Registered;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidth">Width of the pen</param>
    /// <param name="color">Color of the pen</param>
    /// <param name="rPatternOn">Number of 1/72-units of the on-pattern</param>
    /// <param name="rPatternOff">Number of 1/72-units of the off-pattern</param>
    public PenProp(Report report, Double rWidth, Color color, Double rPatternOn, Double rPatternOff) {
      this.report = report;
      _rWidth = rWidth;
      _color = color;
      _rPatternOn = rPatternOn;
      _rPatternOff = rPatternOff;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidth">Width of the pen</param>
    public PenProp(Report report, Double rWidth) : this(report, rWidth, Color.Black, 0, 0) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidth">Width of the pen</param>
    /// <param name="rPatternOn">Number of 1/72-units of the on-pattern</param>
    /// <param name="rPatternOff">Number of 1/72-units of the off-pattern</param>
    public PenProp(Report report, Double rWidth, Double rPatternOn, Double rPatternOff) : this(report, rWidth, Color.Black, rPatternOn, rPatternOff) {
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidth">Width of the pen</param>
    /// <param name="color">Color of the pen</param>
    public PenProp(Report report, Double rWidth, Color color) : this(report, rWidth, color, 0, 0) {
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the color of the pen</summary>
    public Color color {
      get { return _color; }
      set {
        _color = value;
        _penProp_Registered = null;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the number of 1/72-units of the on-pattern</summary>
    public Double rPatternOff { 
      get { return _rPatternOff; }
      set { 
        _rPatternOff = value;
        _penProp_Registered = null;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the number of 1/72-units of the on-pattern</summary>
    public Double rPatternOn{ 
      get { return _rPatternOn; }
      set { 
        _rPatternOn = value;
        _penProp_Registered = null;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Returns a reference to the same but registered property object.</summary>
    internal PenProp penProp_Registered {
      get {
#if DEVELOPER  // bs
        if (_report == null) {
          return PenProp.pp_Null;
        }
#endif
        if (_penProp_Registered == null) {
          String sKey = _rWidth.ToString("F3") + ";" + _color.R + "-" + _color.G + "-" + _color.B + ";" +  _rPatternOn + "-" + _rPatternOff;
          _penProp_Registered = (PenProp)report.ht_PenProp[sKey];
          if (_penProp_Registered == null) {
            _penProp_Registered = new PenProp(report, _rWidth, _color, _rPatternOn, _rPatternOff);
            _penProp_Registered._penProp_Registered = _penProp_Registered;
            report.ht_PenProp.Add(sKey, _penProp_Registered);
          }
        }
        return _penProp_Registered;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the width of the pen</summary>
    public Double rWidth { 
      get { return _rWidth; }
      set { 
        _rWidth = value;
        _penProp_Registered = null;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the width of the pen in millimeter.</summary>
    public Double rWidthMM {
      get { return RT.rMMFromPoint(_rWidth); }
      set { rWidth = RT.rPointFromMM(value); }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="o">The object to compare with the current object.</param>
    /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
    public override Boolean Equals(Object o) {
      if (o == null) {
        return false;
      }
      PenProp pp = (PenProp)o;
      return RT.bEquals(rWidth, pp.rWidth, 0.1) && Object.Equals(_color, pp._color) &&
        RT.bEquals(rPatternOn, pp.rPatternOn, 0.1) && RT.bEquals(rPatternOff, pp.rPatternOff, 0.1);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Hash function of this class.</summary>
    /// <returns>Hash code for the current Object.</returns>
    public override Int32 GetHashCode() {
      return _rWidth.GetHashCode() ^ _color.GetHashCode() ^ _rPatternOn.GetHashCode() ^ _rPatternOff.GetHashCode();
    }

    //----------------------------------------------------------------------------------------------------
    #if Compatible_0_8
    //----------------------------------------------------------------------------------------------------
    /// <summary>Null value</summary>
    public static readonly PenProp pp_Null = penProp_Null;
    #endif
  }


  //****************************************************************************************************
  /// <summary>Structure that defines the properties of a pen with metric values.</summary>
  public class PenPropMM : PenProp {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidthMM">Width of the pen in millimeter</param>
    /// <param name="color">Color of the pen</param>
    /// <param name="rPatternOn">Number of 1/72-units of the on-pattern</param>
    /// <param name="rPatternOff">Number of 1/72-units of the off-pattern</param>
    public PenPropMM(Report report, Double rWidthMM, Color color, Double rPatternOn, Double rPatternOff) : base(report, RT.rPointFromMM(rWidthMM), color, rPatternOn, rPatternOff) {
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidthMM">Width of the pen in millimeter</param>
    public PenPropMM(Report report, Double rWidthMM) : this(report, rWidthMM, Color.Black, 0, 0) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidthMM">Width of the pen in millimeter</param>
    /// <param name="rPatternOn">Number of 1/72-units of the on-pattern</param>
    /// <param name="rPatternOff">Number of 1/72-units of the off-pattern</param>
    public PenPropMM(Report report, Double rWidthMM, Double rPatternOn, Double rPatternOff) : this(report, rWidthMM, Color.Black, rPatternOn, rPatternOff) {
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new pen properties object</summary>
    /// <param name="report">Report to which this pen belongs</param>
    /// <param name="rWidthMM">Width of the pen in millimeter</param>
    /// <param name="color">Color of the pen</param>
    public PenPropMM(Report report, Double rWidthMM, Color color) : this(report, rWidthMM, color, 0, 0) {
    }

  }
}
