using System;
using System.Drawing;

// Creation date: 02.05.2002
// Checked: 20.08.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Structure that defines the properties of a brush.</summary>
  public class BrushProp {
    /// <summary>Background fallback value</summary>
    public static readonly BrushProp bp_Null = new BrushProp(null, System.Drawing.Color.White);

    /// <summary>Report to which this brush belongs</summary>
    internal readonly Report report;
    
    /// <summary>Color of the brush</summary>
    private Color _color;

    /// <summary>Reference to the same but registered property object. 
    /// If null, it has not yet been used and therefore it is not registered.</summary>
    private BrushProp _brushProp_Registered;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Initializes a new brush properties object.</summary>
    /// <param name="report">Report to which this brush belongs</param>
    /// <param name="color">Color of the brush</param>
    public BrushProp(Report report, Color color) {
      this.report = report;
      _color = color;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets a reference to the same but registered brush property object.</summary>
    internal BrushProp brushProp_Registered {
      get {
#if DEVELOPER  // bs
        if (report == null) {
          return BrushProp.bp_Null;
        }
#endif
        if (_brushProp_Registered == null) {
          String sKey = _color.R + "-" + _color.G + "-" + _color.B + "-" + _color.A;
          _brushProp_Registered = (BrushProp)report.ht_BrushProp[sKey];
          if (_brushProp_Registered == null) {
            _brushProp_Registered = new BrushProp(report, _color);
            _brushProp_Registered._brushProp_Registered = _brushProp_Registered;
            report.ht_BrushProp.Add(sKey, _brushProp_Registered);
          }
        }
        return _brushProp_Registered;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the color of the brush</summary>
    public Color color{ 
      get { return _color; }
      set {
        System.Diagnostics.Debug.Assert(_brushProp_Registered != this);
        _color = value;
        _brushProp_Registered = null;
      }
    }
    
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="o">The object to compare with the current object.</param>
    /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
    public override Boolean Equals(Object o) {
      if (o == null) {
        return false;
      }
      BrushProp bp = (BrushProp)o;
      return Equals(_color, bp._color);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Hash function of this class.</summary>
    /// <returns>Hash code for the current Object.</returns>
    public override Int32 GetHashCode() {
      return _color.GetHashCode();
    }

  }
}
