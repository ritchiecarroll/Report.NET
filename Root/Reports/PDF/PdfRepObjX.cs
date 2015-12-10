using System;
using System.Diagnostics;

// Creation date: 19.01.2006
// Checked: 07.02.2006
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
  //------------------------------------------------------------------------------------------17.02.2006
  #region IPdfRepObjX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Interface for the extended data of a RepObj-object when a PDF formatter is used</summary>
  internal interface IPdfRepObjX {
    //------------------------------------------------------------------------------------------03.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    void Write(PdfIndirectObject_PageContents.Environment e);
  }
  #endregion

  //------------------------------------------------------------------------------------------17.02.2006
  #region PdfContainerX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Extended PDF Container Class</summary>
  internal sealed class PdfContainerX : IPdfRepObjX {
    /// <summary>Singleton instance of this class.</summary>
    internal static readonly PdfContainerX instance = new PdfContainerX();

    //------------------------------------------------------------------------------------------09.02.2006
    /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
    private PdfContainerX() {
    }

    //------------------------------------------------------------------------------------------17.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    public void Write(PdfIndirectObject_PageContents.Environment e) {
      PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
      Container container = (Container)e.repObj;

      PdfIndirectObject_PageContents.Environment e2 = new PdfIndirectObject_PageContents.Environment();
      e2.report = e.report;
      e2.pdfIndirectObject_PageContents = e.pdfIndirectObject_PageContents;
      foreach (RepObj repObj in container) {
        IPdfRepObjX pdfRepObjX = (IPdfRepObjX)repObj.oRepObjX;
        e2.repObj = repObj;
        e2.matrixD = e.matrixD.Clone();
        e2.matrixD.Multiply(repObj.matrixD);
        e2.bComplex = e2.matrixD.bComplex;
        pdfRepObjX.Write(e2);
      }
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------xx.02.2006
  #region PdfRepImageX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Extended PDF Image Class</summary>
  internal sealed class PdfRepImageX : IPdfRepObjX {
    /// <summary>Singleton instance of this class.</summary>
    internal static readonly PdfRepImageX instance = new PdfRepImageX();

    //------------------------------------------------------------------------------------------17.02.2006
    /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
    private PdfRepImageX() {
    }

    //------------------------------------------------------------------------------------------xx.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    public void Write(PdfIndirectObject_PageContents.Environment e) {
      PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
      RepImage repImage = (RepImage)e.repObj;
      Double rOfsX = repImage.rWidth * repImage.rAlignH;
      Double rOfsY = repImage.rHeight * (1 - repImage.rAlignV);
      e.matrixD.Multiply(new MatrixD(1, 0, 0, 1, -rOfsX, rOfsY));
      e.matrixD.Scale(repImage.rWidth, repImage.rHeight);
      p.Command("q");
      p.Write_Matrix(e.matrixD);  p.Command("cm");
      PdfIndirectObject_ImageJpeg pdfIndirectObject_ImageJpeg = (PdfIndirectObject_ImageJpeg)repImage.imageData.oImageResourceX;
      p.Name("I" + pdfIndirectObject_ImageJpeg.iObjectNumber);  p.Command("Do");
      p.Command("Q");
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------17.02.2006
  #region PdfRepLineX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Extended PDF Line Class</summary>
  internal sealed class PdfRepLineX : IPdfRepObjX {
    /// <summary>Singleton instance of this class.</summary>
    internal static readonly PdfRepLineX instance = new PdfRepLineX();

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
    private PdfRepLineX() {
    }

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    public void Write(PdfIndirectObject_PageContents.Environment e) {
      PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
      RepLine repLine = (RepLine)e.repObj;
      Double rOfsX = repLine.rWidth * repLine.rAlignH;
      Double rOfsY = repLine.rHeight * repLine.rAlignV;
      e.matrixD.Multiply(1, 0, 0, 1, -rOfsX, -rOfsY);
      if (repLine.penProp.rWidth > 0f) {
        p.Write_Pen(repLine.penProp);
        p.Write_Point(e.matrixD.rDX, e.matrixD.rDY);
        p.Command("m");
        p.Write_Point(e.matrixD, repLine.rWidth, repLine.rHeight);
        p.Command("l");
        p.Command("S");
      }
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------07.02.2006
  #region PdfRepArcBaseX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Extended PDF ArcBase Class</summary>
  internal sealed class PdfRepArcBaseX : IPdfRepObjX {
    /// <summary>Singleton instance of this class.</summary>
    internal static readonly PdfRepArcBaseX instance = new PdfRepArcBaseX();

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
    private PdfRepArcBaseX() {
    }

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    public void Write(PdfIndirectObject_PageContents.Environment e) {
      PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
      RepArcBase repArcBase = (RepArcBase)e.repObj;
      Double rOfsX = repArcBase.rWidth * (-repArcBase.rAlignH + 0.5);
      Double rOfsY = repArcBase.rHeight * (1 - repArcBase.rAlignV - 0.5);
      e.matrixD.Multiply(new MatrixD(1, 0, 0, 1, rOfsX, rOfsY));

      String sDrawCommand = null;
      if (repArcBase._penProp != null && repArcBase._penProp.rWidth != 0.0) {
        p.Write_Pen(repArcBase._penProp);
        if (repArcBase._brushProp != null) {
          p.Write_Brush(repArcBase._brushProp);
          sDrawCommand = "b";  // close, fill and stroke path
        }
        else {
          sDrawCommand = (repArcBase is RepArc ? "S" : "s");  // stroke : close and stroke path
        }
      }
      else if (repArcBase._brushProp != null) {
        p.Write_Brush(repArcBase._brushProp);
        sDrawCommand = "f";  // fill path
      }
      else {
        return;
      }

      Double rA = repArcBase.rWidth / 2;
      Double rA2 = rA * rA;
      Double rB = repArcBase.rHeight / 2;
      Double rB2 = rB * rB;

      // start point: P0
      Double rAngle0 = RT.rRadianFromDegree(repArcBase._rStartAngle);
      Double rP0X, rP0Y;
      repArcBase.GetEllipseXY(rAngle0, out rP0X, out rP0Y);
      p.Command("q");
      p.Write_Matrix(e.matrixD);
      p.Command("cm");
      if (repArcBase is RepArc || repArcBase is RepCircle || repArcBase is RepEllipse) {
        p.Number(rP0X);  p.Space();  p.Number(rP0Y);
        p.Command("m");
      }
      else {
        p.Number(0);  p.Space();  p.Number(0);
        p.Command("m");
        p.Number(rP0X);  p.Space();  p.Number(rP0Y);
        p.Command("l");
      }

      Double r = repArcBase._rSweepAngle / 180 * Math.PI;
      Int32 iNumberOfArcs = ((Int32)(r / (Math.PI / 3.0))) + 1;
      Double rSweepAngle = r / iNumberOfArcs;
      for (Int32 iArc = 0; iArc < iNumberOfArcs; iArc++) {
        // end point: P3
        Double rAngle3 = rAngle0 + rSweepAngle;
        Double rP3X, rP3Y;
        repArcBase.GetEllipseXY(rAngle3, out rP3X, out rP3Y);

        Double rAngle05 = rAngle0 + rSweepAngle / 2.0;
        Double rMX, rMY;
        repArcBase.GetEllipseXY(rAngle05, out rMX, out rMY);


        Double rP1X, rP2X, rP1Y, rP2Y;
        Double rDenominator = rP0X * rP3Y - rP3X * rP0Y;
        Debug.Assert(!RT.bEquals(rDenominator, 0, 0.0001), "parallel tangents never appears if the sweep angle is less than PI/2");
        if (RT.bEquals(rP0Y, 0, 0.0001)) {
          Debug.Assert(!RT.bEquals(rP3Y, 0, 0.0001), "P0 and P3 on x-axis: never appears if the sweep angle is less than PI/2");
          rP1X = rP0X;
          rP2X = 8.0 / 3.0 * rMX - 4.0 / 3.0 * rP0X - rP3X / 3.0;
          rP1Y = 8.0 / 3.0 * rMY - rB2 / rP3Y + rB2 * rP3X * (8.0 * rMX - 4 * rP0X - rP3X) / (3.0 * rA2 * rP3Y) - rP3Y / 3.0;
          rP2Y = rB2 / rP3Y * (1 - rP2X * rP3X / rA2);
        }
        else if (RT.bEquals(rP3Y, 0, 0.0001)) {
          rP1X = 8.0 / 3.0 * rMX - rP0X / 3.0 - 4.0 / 3.0 * rP3X;
          rP2X = rP3X;
          rP1Y = rB2 / rP0Y * (1 - rP0X * rP1X / rA2);
          rP2Y = 8.0 / 3.0 * rMY - rP0Y / 3.0 - rB2 / rP0Y + rB2 * rP0X * (8.0 * rMX - rP0X - 4 * rP3X) / (3.0 * rA2 * rP0Y);
        }
        else {
          rP1X = (3.0 * rA2 * rB2 * (rP0Y + rP3Y)
            + rA2 * rP0Y * rP3Y * (rP0Y + rP3Y - 8 * rMY)
            + rB2 * rP3X * rP0Y * (rP0X + rP3X - 8 * rMX))
            / (3 * rB2 * rDenominator);
          rP2X = 8.0 / 3.0 * rMX - (rP0X + rP3X) / 3.0 - rP1X;

          rP1Y = rB2 / rP0Y * (1 - rP0X * rP1X / rA2);
          rP2Y = rB2 / rP3Y * (1 - rP2X * rP3X / rA2);
        }
        Debug.Assert(RT.bEquals(rMX, rP0X / 8.0 + 3.0 / 8.0 * rP1X + 3.0 / 8.0 * rP2X + rP3X / 8.0, 0.0001));
        Debug.Assert(RT.bEquals(rMY, rP0Y / 8.0 + 3.0 / 8.0 * rP1Y + 3.0 / 8.0 * rP2Y + rP3Y / 8.0, 0.0001));

        p.Number(rP1X);  p.Space();  p.Number(rP1Y);  p.Space();  p.Number(rP2X);  p.Space();  p.Number(rP2Y);  p.Space();
        p.Number(rP3X);  p.Space();  p.Number(rP3Y);
        p.Command("c");

        rAngle0 += rSweepAngle;
        rP0X = rP3X;
        rP0Y = rP3Y;
      }
      p.Command(sDrawCommand);
      p.Command("Q");
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------07.02.2006
  #region PdfRepRectX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Extended PDF Rectangle Class</summary>
  internal sealed class PdfRepRectX : IPdfRepObjX {
    /// <summary>Singleton instance of this class.</summary>
    internal static readonly PdfRepRectX instance = new PdfRepRectX();

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
    private PdfRepRectX() {
    }

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    public void Write(PdfIndirectObject_PageContents.Environment e) {
      PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
      RepRect repRect = (RepRect)e.repObj;
      Double rOfsX = repRect.rWidth * repRect.rAlignH;
      Double rOfsY = repRect.rHeight * (1 - repRect.rAlignV);
      e.matrixD.Multiply(1, 0, 0, 1, -rOfsX, rOfsY);
      if (repRect.penProp != null) {
        if (repRect.penProp.rWidth > 0f) {
          p.Write_Pen(repRect.penProp);
        }
        else {
          repRect.penProp = null;
        }
      }
      if (repRect.brushProp != null) {
        p.Write_Brush(repRect.brushProp);
      }

      if (e.bComplex) {
        p.Command("q");
        p.Write_Matrix(e.matrixD);
        p.Command("cm");
        p.Write_Point(0, 0);  p.Space();  p.Number(repRect.rWidth);  p.Space();  p.Number(repRect.rHeight);  p.Space();
        p.Command("re");
        p.Command(repRect.penProp == null ? "f" : (repRect.brushProp == null ? "S" : "B"));
        p.Command("Q");
      }
      else {
        p.Write_Point(e.matrixD.rDX, e.matrixD.rDY);  p.Space();  p.Number(repRect.rWidth);  p.Space();  p.Number(repRect.rHeight);
        p.Command("re");
        p.Command(repRect.penProp == null ? "f" : (repRect.brushProp == null ? "S" : "B"));
      }
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------07.02.2006
  #region PdfRepStringX
  //----------------------------------------------------------------------------------------------------

  /// <summary>Extended PDF String Class</summary>
  internal sealed class PdfRepStringX : IPdfRepObjX {
    /// <summary>Singleton instance of this class.</summary>
    internal static readonly PdfRepStringX instance = new PdfRepStringX();

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Singleton instance <see cref="instance"/> must be used.</summary>
    private PdfRepStringX() {
    }

    //------------------------------------------------------------------------------------------07.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    public void Write(PdfIndirectObject_PageContents.Environment e) {
      PdfIndirectObject_PageContents p = e.pdfIndirectObject_PageContents;
      RepString repString = (RepString)e.repObj;
      Double rWidth = repString.fontProp.rGetTextWidth(repString.sText);
      Double rOfsX = rWidth * repString.rAlignH;
      Double rOfsY = repString.fontProp.rSize * (1 - repString.rAlignV);
      e.matrixD.Multiply(new MatrixD(1, 0, 0, 1, -rOfsX, rOfsY));

      p.Command("BT");
      p.Write_Font(repString.fontProp);
      if (e.bComplex) {
        p.Write_Matrix(e.matrixD);
        p.Command("Tm");
      }
      else {
        p.Write_Point(e.matrixD.rDX, e.matrixD.rDY);
        p.Command("Td");
      }
      p.String(repString.sText);
      p.Command("Tj");
      p.Command("ET");

      if (repString.fontProp.bUnderline) {
        Type1FontData type1FontData = (Type1FontData)repString.fontProp.fontData;
        Double rScaleFactor = repString.fontProp.rSizePoint;
        PenProp pp = new PenProp(e.report, rScaleFactor * type1FontData.fUnderlineThickness / 1000, repString.fontProp.color);
        p.Write_Pen(pp);
        Double rD = rScaleFactor * type1FontData.fUnderlinePosition / 1000;
        p.Write_Point(e.matrixD, 0, -rD);
        p.Command("m");
        p.Write_Point(e.matrixD, rWidth, -rD);
        p.Command("l");
        p.Command("S");
      }
    }
  }
  #endregion
}
