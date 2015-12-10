using Root.Reports;
using System;
using System.Drawing;

// Creation date: 19.08.2002
// Checked: 12.12.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace ReportSamples {
  /// <summary>Font Test</summary>
  public class Test : Report {
    //----------------------------------------------------------------------------------------------------
    #region Test
    //----------------------------------------------------------------------------------------------------

    private const Int32 rMarginL = 20;
    private const Int32 rMarginT = 20;

    private FontProp fp;
    private FontProp fp_Small;
    private FontProp fp_XSmall;
    private BrushProp bp;
    private Double rX;
    private Double rY;

    private readonly FontDef fontDef;
    private readonly FontProp fontProp_Title;
    private readonly FontProp fontProp_Label;
    private readonly PenProp penProp_Symbol;
    private readonly PenProp penProp_Line;

    private Color[] aColor = new Color[] {
      Color.FromArgb(255, 0, 0), Color.FromArgb(255, 127, 127), Color.FromArgb(255, 191, 191), Color.FromArgb(255, 223, 223),
      Color.FromArgb(0, 255, 0), Color.FromArgb(127, 255, 127), Color.FromArgb(191, 255, 191), Color.FromArgb(223, 255, 223),
      Color.FromArgb(0, 0, 255), Color.FromArgb(127, 127, 255), Color.FromArgb(191, 191, 255), Color.FromArgb(223, 223, 255)
    };

    public Test() {
      fontDef = FontDef.fontDef_FromName(this, FontDef.StandardFont.Helvetica);
      fontProp_Title = new FontPropMM(fontDef, 6);
      fontProp_Title.bBold = true;
      fontProp_Label = new FontPropMM(fontDef, 4);

      penProp_Symbol = new PenProp(this, 1);
      penProp_Symbol.color = Color.Red;
      penProp_Line = new PenProp(this, 1);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates this document</summary>
    protected override void Create() {
      FontTest();
      TestRepLine();
    }
    #endregion

    //----------------------------------------------------------------------------------------------------
    #region Test RepString
    //----------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Prints the specified character at the current position.</summary>
    /// <param name="iChar">Character code</param>
    protected void PrintCharacter(Int32 iChar) {
      if (rX > 185) {  // new line
        rY += fp.rLineFeedMM;
        rX = 22;
      }
      if (rY > 280) {  // new page
        new Page(this);
        rY = 40;
      }

      Char ch = (Char)iChar;
      String s = ch.ToString();
      Double rWidth = fp.rGetTextWidthMM(s);
      Double rHeight = fp.rLineFeedMM * 0.65;
      page_Cur.AddRightMM(rX, rY - 2.5, new RepInt32(fp_Small, (Int32)ch));
      page_Cur.AddMM(rX + 0.1, rY - 2, new RepInt32(fp_XSmall, 10));
      if (iChar < 256) {
        Int32 iOct = (iChar % 8) + 10 * ((iChar / 8) % 8) + 100 * ((iChar / 64) % 8);
        page_Cur.AddRightMM(rX, rY, new RepInt32(fp_Small, iOct, ""));
        page_Cur.AddMM(rX + 0.1, rY + 0.5, new RepInt32(fp_XSmall, 8));
      }
      page_Cur.AddMM(rX + 2, rY, new RepRectMM(bp, rWidth, rHeight));
      page_Cur.AddMM(rX + 2, rY, new RepString(fp, s));
      rX += 15;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates this document</summary>
    private void FontTest() {
      FontProp fp_Title = new FontPropMM(fontDef, 12);
      fp_Title.bBold = true;

      fp = new FontPropMM(fontDef, 6);
      fp_Small = new FontPropMM(fontDef, 1.4);
      fp_XSmall = new FontPropMM(fontDef, 0.8);
      bp = new BrushProp(this, Color.FromArgb(200, 200, 200));

      page_Cur = new Page(this);
      page_Cur.AddCB_MM(30, new RepString(fp_Title, "Font Test"));

      rX = 300;
      rY = 40;
      for (Int32 i = 32; i < 127; i++) {
        PrintCharacter(i);
      }
      for (Int32 i = 161; i < 256; i++) {
        PrintCharacter(i);
      }
      PrintCharacter('€');
    }
    #endregion

    //----------------------------------------------------------------------------------------------------
    #region Test RepLine
    //----------------------------------------------------------------------------------------------------

    /// <summary>Creates this document</summary>
    private void TestRepLine() {
      new Page(this);

      page_Cur.AddCT_MM(page_Cur.rWidthMM / 2, rMarginT, new RepString(fontProp_Title, "Line Test"));

      StaticContainer cont1 = new StaticContainer(170, 170);
      cont1.RotateTransform(10);
      page_Cur.AddMM(rMarginL + 20, rMarginT + 10, cont1);

      StaticContainer cont2 = new StaticContainer(170, 170);
      cont2.RotateTransform(-10);
      cont1.AddMM(0, 0, cont2);

      PrintLines(cont2);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Prints the specified number of lines.</summary>
    private void PrintLines(StaticContainer cont) {
      Double rY = 0;
      Double rSize = 15;
      Double rX = rSize;
      Double rDY = 10;
      Double rDX = 15;
      rY += rDY + rSize;
      PrintLines(cont, "Left / Top", rX, RepObj.rAlignLeft, rY, RepObj.rAlignTop, rSize, 12);
      rX += rDX + 2 * rSize;
      PrintLines(cont, "Center / Top", rX, RepObj.rAlignCenter, rY, RepObj.rAlignTop, rSize, 12);
      rX += rDX + 2 * rSize;
      PrintLines(cont, "Right / Top", rX, RepObj.rAlignRight, rY, RepObj.rAlignTop, rSize, 12);

      rX = rSize;
      rY += rDY + 2 * rSize;
      PrintLines(cont, "Left / Center", rX, RepObj.rAlignLeft, rY, RepObj.rAlignCenter, rSize, 12);
      rX += rDX + 2 * rSize;
      PrintLines(cont, "Center / Center", rX, RepObj.rAlignCenter, rY, RepObj.rAlignCenter, rSize, 6);
      rX += rDX + 2 * rSize;
      PrintLines(cont, "Right / Center", rX, RepObj.rAlignRight, rY, RepObj.rAlignCenter, rSize, 12);

      rX = rSize;
      rY += rDY + 2 * rSize;
      PrintLines(cont, "Left / Bottom", rX, RepObj.rAlignLeft, rY, RepObj.rAlignBottom, rSize, 12);
      rX += rDX + 2 * rSize;
      PrintLines(cont, "Center / Bottom", rX, RepObj.rAlignCenter, rY, RepObj.rAlignBottom, rSize, 12);
      rX += rDX + 2 * rSize;
      PrintLines(cont, "Right / Bottom", rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, rSize, 12);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Prints the specified number of lines.</summary>
    private void PrintLines(StaticContainer cont, String sLabel, Double rX, Double rAlignH, Double rY, Double rAlignV, Double rSize, Int32 iCount) {
      cont.AddCB_MM(rX, rY - rSize, new RepString(fontProp_Label, sLabel));
      cont.AddCC_MM(rX, rY, new RepCircleMM(penProp_Symbol, 3));
      for (Int32 i = 0; i < iCount; i++) {
        penProp_Line.color = aColor[i];
        RepLine repLine = new RepLineMM(penProp_Line, rSize / 1.414, rSize / 1.414);
        repLine.RotateTransform(i * 30);
        cont.AddAlignedMM(rX, rAlignH, rY, rAlignV, repLine);
      }
    }
    #endregion
  }
}
