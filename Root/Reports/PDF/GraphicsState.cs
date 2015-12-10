#if DEVELOPER  // bs
using System;
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
  /// <summary>Grapics State Data Class</summary>
  public class GraphicsState {
    /// <summary>Report object</summary>
    private RepObj _repObj;
    /// <summary>Pen definition</summary>
    private PenProp _penProp;
    /// <summary>Brush definition</summary>
    private BrushProp _brushProp;

    /// <summary>Reference to the same but registered state object.
    /// If null, it has not yet been used and therefore it is not registered.</summary>
    private GraphicsState _graphicsState_Registered;

    /// <summary>Internal structure used by the formatter</summary>
    private GraphicsStateData _graphicsStateData;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new graphics state object.</summary>
    internal GraphicsState(RepObj repObj, PenProp penProp, BrushProp brushProp) {
      _repObj = repObj;
      _penProp = penProp;
      _brushProp = brushProp;
    }
  
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the pen properties.</summary>
    public PenProp penProp {
      get { return _penProp; } 
      set {
        System.Diagnostics.Debug.Assert(_graphicsState_Registered != this);
        _graphicsState_Registered = null;
        _graphicsStateData = null;
        _penProp = value;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets or sets the brush properties.</summary>
    public BrushProp brushProp {
      get { return _brushProp; } 
      set {
        System.Diagnostics.Debug.Assert(_graphicsState_Registered != this);
        _graphicsState_Registered = null;
        _graphicsStateData = null;
        _brushProp = value;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the graphics state data.</summary>
    internal GraphicsStateData graphicsStateData {
      get {
        if (_graphicsStateData == null) {
          if (graphicsState_Registered == this) {
            // FontPropData may be created only for registered GrapicsState objects
            _graphicsStateData = _repObj.report.formatter.graphicsStateData_CreateInstance(this);
          }
          else {
            _graphicsStateData = graphicsState_Registered.graphicsStateData;
          }
        }
        return _graphicsStateData;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets a  reference to the same but registered graphics state object.</summary>
    internal GraphicsState graphicsState_Registered {
      get {
        if (_graphicsState_Registered == null) {
          StringBuilder sb = new StringBuilder(50);
          if(_penProp != null) {
            sb.Append(_penProp.color.R);
            sb.Append("-");
            sb.Append(_penProp.color.G);
            sb.Append("-");
            sb.Append(_penProp.color.B);
            sb.Append("-");
            sb.Append(_penProp.color.A);
          }
          sb.Append(":");
          if(_brushProp != null) {
            sb.Append(_brushProp.color.R);
            sb.Append("-");
            sb.Append(_brushProp.color.G);
            sb.Append("-");
            sb.Append(_brushProp.color.B);
            sb.Append("-");
            sb.Append(_brushProp.color.A);
          }
          String sKey = sb.ToString();
          _graphicsState_Registered = (GraphicsState)_repObj.report.ht_GraphicState[sKey];
          if (_graphicsState_Registered == null) {
            _graphicsState_Registered = new GraphicsState(_repObj, _penProp, _brushProp);
            _graphicsState_Registered._graphicsState_Registered = _graphicsState_Registered;
            _repObj.report.ht_GraphicState.Add(sKey, _graphicsState_Registered);
          }
        }
        return _graphicsState_Registered;
      }
    }
  }
}
#endif