using System;
using System.Globalization;

// Creation date: 24.04.2002
// Checked: 26.07.2002
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
  /// <summary>Report String Object.</summary>
  public class RepString : RepObj {
    /// <summary>Font properties of the string</summary>
    public readonly FontProp fontProp;

    /// <summary>Text of the string object.</summary>
    public String sText;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new string object.</summary>
    /// <param name="fontProp">Font properties of the string object</param>
    /// <param name="sText">Text of the string object</param>
    public RepString(FontProp fontProp, String sText) {
      this.fontProp = fontProp.fontProp_Registered;
      this.sText = (sText == null) ? "" : sText;
      oRepObjX = fontProp.fontDef.report.formatter.oCreate_RepString();
      if (fontProp.rAngle != 0) {
        RotateTransform(fontProp.rAngle);
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Sets or gets the height of this report object.</summary>
    public override Double rHeight {
      set { System.Diagnostics.Debug.Assert(false); }
      get {
        Double rX = fontProp.rGetTextWidth(sText);
        Double rY = fontProp.rSize;
        Double r = Math.Abs(rX * matrixD.rRY + rY * matrixD.rSY);
        return r;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Sets or gets the width of this report object.</summary>
    public override Double rWidth {
      set { System.Diagnostics.Debug.Assert(false); }
      get {
        Double rX = fontProp.rGetTextWidth(sText);
        Double rY = fontProp.rSize;
        Double r = Math.Abs(rX * matrixD.rSX + rY * matrixD.rRY);
        return r;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the top side of this report object (points, 1/72 inch).</summary>
    public override Double rPosTop {
      get {
        Double rW = fontProp.rGetTextWidth(sText);
        Double rH = fontProp.rSize;
        Double rX1 = -rW * rAlignH;
        Double rX2 = rW * (1 - rAlignH);
        Double rY1 = -rH * rAlignV;
        Double rY2 = rH * (1 - rAlignV);
        Double rMin = matrixD.rTransformY(rX1 , rY1);
        Double r = matrixD.rTransformY(rX1, rY2);
        rMin = Math.Min(rMin, r);
        r = matrixD.rTransformY(rX2, rY1);
        rMin = Math.Min(rMin, r);
        r = matrixD.rTransformY(rX2, rY2);
        rMin = Math.Min(rMin, r);
        return rMin;
      }
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the position of the bottom of this report object (points, 1/72 inch).</summary>
    public override Double rPosBottom {
      get {
        Double rW = fontProp.rGetTextWidth(sText);
        Double rH = fontProp.rSize;
        Double rX1 = -rW * rAlignH;
        Double rX2 = rW * (1 - rAlignH);
        Double rY1 = -rH * rAlignV;
        Double rY2 = rH * (1 - rAlignV);
        Double rMax = matrixD.rTransformY(rX1 , rY1);
        Double r = matrixD.rTransformY(rX1, rY2);
        rMax = Math.Max(rMax, r);
        r = matrixD.rTransformY(rX2, rY1);
        rMax = Math.Max(rMax, r);
        r = matrixD.rTransformY(rX2, rY2);
        rMax = Math.Max(rMax, r);
        return rMax;
      }
    }

  }


  //****************************************************************************************************
  /// <summary>Report Int32 Object.</summary>
  public class RepInt32 : RepString {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new Int32 object.</summary>
    /// <param name="fontProp">Font properties of the Int32 object</param>
    /// <param name="iVal">Int32 value</param>
    /// <param name="sFormat">Provides the format information for the Int32 value (ms-help://MS.VSCC/MS.MSDNVS/cpguide/html/cpconcustomnumericformatstrings.htm)</param>
    public RepInt32(FontProp fontProp, Int32 iVal, String sFormat) : base(fontProp, iVal.ToString(sFormat)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new Int32 object.</summary>
    /// <param name="fontProp">Font properties of the Int32 object</param>
    /// <param name="iVal">Int32 value</param>
    /// <param name="nfi">Provides the format information for the Int32 value (ms-help://MS.VSCC/MS.MSDNVS/cpref/html/frlrfSystemGlobalizationNumberFormatInfoClassTopic.htm)</param>
    //public RepInt32(FontProp fontProp, Int32 iVal, NumberFormatInfo nfi) : base(fontProp, iVal.ToString(nfi)) {
    public RepInt32(FontProp fontProp, Int32 iVal, NumberFormatInfo nfi) : base(fontProp, String.Format(nfi, "{0:N}", iVal)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new Int32 object.</summary>
    /// <param name="fontProp">Font properties of the Int32 object</param>
    /// <param name="iVal">Int32 value</param>
    public RepInt32(FontProp fontProp, Int32 iVal) : base(fontProp, iVal.ToString()) {
    }
    
  }


  //****************************************************************************************************
  /// <summary>Report Real32 Object.</summary>
  public class RepReal32 : RepString {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new RepReal32 object.</summary>
    /// <param name="fontProp">Font properties of the Real32 object</param>
    /// <param name="fVal">Real32 (Single) value</param>
    /// <param name="sFormat">Provides the format information for the Int32 value (ms-help://MS.VSCC/MS.MSDNVS/cpguide/html/cpconcustomnumericformatstrings.htm)</param>
    public RepReal32(FontProp fontProp, Single fVal, String sFormat) : base(fontProp, fVal.ToString(sFormat)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new RepReal32 object.</summary>
    /// <param name="fontProp">Font properties of the Real32 object</param>
    /// <param name="fVal">Real32 (Single) value</param>
    /// <param name="nfi">Provides the format information for the Int32 value (ms-help://MS.VSCC/MS.MSDNVS/cpref/html/frlrfSystemGlobalizationNumberFormatInfoClassTopic.htm)</param>
    public RepReal32(FontProp fontProp, Single fVal, NumberFormatInfo nfi) : base(fontProp, String.Format(nfi, "{0:N}", fVal)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new RepReal32 object.</summary>
    /// <param name="fontProp">Font properties of the Real32 object</param>
    /// <param name="fVal">Real32 (Single) value</param>
    public RepReal32(FontProp fontProp, Single fVal) : base(fontProp, fVal.ToString("0.00")) {
    }
   
  }


  //****************************************************************************************************
  /// <summary>Report Real64 Object.</summary>
  public class RepReal64 : RepString {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new RepReal64 object.</summary>
    /// <param name="fontProp">Font properties of the Real64 object</param>
    /// <param name="rVal">Real64 value</param>
    /// <param name="sFormat">Provides the format information for the Int32 value (ms-help://MS.VSCC/MS.MSDNVS/cpguide/html/cpconcustomnumericformatstrings.htm)</param>
    public RepReal64(FontProp fontProp, Double rVal, String sFormat) : base(fontProp, rVal.ToString(sFormat)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new RepReal64 object.</summary>
    /// <param name="fontProp">Font properties of the Real64 object</param>
    /// <param name="rVal">Real64 value</param>
    /// <param name="nfi">Provides the format information for the Int32 value (ms-help://MS.VSCC/MS.MSDNVS/cpref/html/frlrfSystemGlobalizationNumberFormatInfoClassTopic.htm)</param>
    //public RepReal64(FontProp fontProp, Double rVal, NumberFormatInfo nfi) : base(fontProp, rVal.ToString(nfi)) {
    public RepReal64(FontProp fontProp, Double rVal, NumberFormatInfo nfi) : base(fontProp, String.Format(nfi, "{0:N}", rVal)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new RepReal64 object.</summary>
    /// <param name="fontProp">Font properties of the Real64 object</param>
    /// <param name="rVal">Real64 value</param>
    public RepReal64(FontProp fontProp, Double rVal) : base(fontProp, rVal.ToString("0.00")) {
    }
   
  }


  //****************************************************************************************************
  /// <summary>Report DateTime Object.</summary>
  public class RepDateTime : RepString {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new DateTime object.</summary>
    /// <param name="fontProp">Font properties of the DateTime object</param>
    /// <param name="dt_Val">DateTime value</param>
    /// <param name="sFormat">Provides the format information for the DataTime value (ms-help://MS.VSCC/MS.MSDNVS/cpref/html/frlrfSystemGlobalizationDateTimeFormatInfoClassTopic.htm)</param>
    public RepDateTime(FontProp fontProp, DateTime dt_Val, String sFormat) : base(fontProp, dt_Val.ToString(sFormat)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new DateTime object.</summary>
    /// <param name="fontProp">Font properties of the DateTime object</param>
    /// <param name="dt_Val">DateTime value</param>
    public RepDateTime(FontProp fontProp, DateTime dt_Val) : base(fontProp, dt_Val.ToString()) {
    }
   
  }

}
