using Root.Reports;
using System;
using System.Drawing;

// Creation date: 25.07.2002
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
  /// <summary>Text Sample</summary>
  public class TextSample : Report {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates this report</summary>
    protected override void Create() {
      FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica);
      FontProp fp = new FontPropMM(fd, 3);
      FontProp fp_Title = new FontPropMM(fd, 20);
      fp_Title.bBold = true;
      FontProp fp_Small = new FontPropMM(fd, 1.3);
      PenProp pp = new PenPropMM(this, 0.2, Color.Blue);
      BrushProp bp = new BrushProp(this, Color.FromArgb(240, 240, 240));

      #region Page 1
      Page page = new Page(this);
      page.AddCB_MM(40, new RepString(fp_Title, "Text Sample"));
      fp_Title.rSizeMM = 4;
      fp_Title.rLineFeedMM = 8;

      // font sample
      Double rX = 20;
      Double rY = 60;
      page.AddMM(rX, rY, new RepString(fp_Title, "Fonts"));
      rY += fp_Title.rLineFeedMM;
      FontDef[] aFontDef = {new FontDef(this, FontDef.StandardFont.Courier), fd, new FontDef(this, FontDef.StandardFont.TimesRoman)};
      foreach (FontDef fontDef in aFontDef) {
        FontProp fp_Test = new FontPropMM(fontDef, 2.8);
        page.AddMM(rX, rY, new RepString(fp_Test, fontDef.sFontName));
        fp_Test.bBold = true;
        page.AddMM(rX + 30, rY, new RepString(fp_Test, fontDef.sFontName + " Bold"));
        fp_Test.bBold = false;
        fp_Test.bItalic = true;
        page.AddMM(rX + 72, rY, new RepString(fp_Test, fontDef.sFontName + " Italic"));
        fp_Test.bItalic = false;
        fp_Test.bUnderline = true;
        page.AddMM(rX + 120, rY, new RepString(fp_Test, fontDef.sFontName + " Underline"));
        rY += fp.rLineFeedMM;
      }

      rY += 3;
      aFontDef = new FontDef[] {new FontDef(this, "Symbol"), new FontDef(this, "ZapfDingbats")};
      foreach (FontDef fontDef in aFontDef) {
        FontProp fp_Test = new FontPropMM(fontDef, 3);
        page.AddMM(rX, rY, new RepString(fp, fontDef.sFontName));
        page.AddMM(rX + 30, rY, new RepString(fp_Test, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"));
        rY += fp.rLineFeedMM;
      }
      rY += 10;

      // Int32 sample
      rX = 20;
      Double rYcopy = rY;
      page.AddMM(rX, rY, new RepString(fp_Title, "Int32 Values"));
      rY += fp_Title.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "no format string"));
      page.AddRightMM(rX + 80, rY, new RepInt32(fp, 12345));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"0000000\""));
      page.AddRightMM(rX + 80, rY, new RepInt32(fp, 12345, "0000000"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#,#\""));
      page.AddRightMM(rX + 80, rY, new RepInt32(fp, 12345, "$#,#"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#;($#);Zero\""));
      page.AddRightMM(rX + 80, rY, new RepInt32(fp, 12345, "$#;($#);Zero"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#;($#);Zero\""));
      page.AddRightMM(rX + 80, rY, new RepInt32(fp, -12345, "$#;($#);Zero"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#;($#);Zero\""));
      page.AddRightMM(rX + 80, rY, new RepInt32(fp, 0, "$#;($#);Zero"));

      // Single / Double sample
      rX = 115;
      rY = rYcopy;
      page.AddMM(rX, rY, new RepString(fp_Title, "Single / Double Values"));
      rY += fp_Title.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "no format string"));
      page.AddRightMM(rX + 80, rY, new RepReal64(fp, 123.456));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"0.0000\""));
      page.AddRightMM(rX + 80, rY, new RepReal64(fp, 123.456, "0.0000"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#,#0.00\""));
      page.AddRightMM(rX + 80, rY, new RepReal64(fp, 123.456, "$#,#0.00"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#.0;($#.0);Zero\""));
      page.AddRightMM(rX + 80, rY, new RepReal64(fp, 123.456, "$#.0;($#.0);Zero"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#.0;($#.0);Zero\""));
      page.AddRightMM(rX + 80, rY, new RepReal64(fp, -123.456, "$#.0;($#.0);Zero"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"$#.0;($#.0);Zero\""));
      page.AddRightMM(rX + 80, rY, new RepReal64(fp, 0, "$#.0;($#.0);Zero"));
      rY += fp.rLineFeedMM + 10;

      // DateTime sample
      rX = 20;
      rYcopy = rY;
      page.AddMM(rX, rY, new RepString(fp_Title, "DateTime Values"));
      rY += fp_Title.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "no format string"));
      page.AddRightMM(rX + 80, rY, new RepDateTime(fp, DateTime.Now));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"D\""));
      page.AddRightMM(rX + 80, rY, new RepDateTime(fp, DateTime.Now, "D"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"t\""));
      page.AddRightMM(rX + 80, rY, new RepDateTime(fp, DateTime.Now, "t"));
      rY += fp.rLineFeedMM;
      page.AddMM(rX, rY, new RepString(fp, "format \"dd.MM.yyyy\""));
      page.AddRightMM(rX + 80, rY, new RepDateTime(fp, DateTime.Now, "dd.MM.yyyy"));

      // color sample
      StaticContainer sc = new StaticContainer(RT.rPointFromMM(100), RT.rPointFromMM(100));
      page.AddMM(115, rYcopy + 5, sc);
      sc.RotateTransform(-8);
      sc.AddAlignedMM(-2, RepObj.rAlignLeft, -6, RepObj.rAlignTop, new RepRectMM(bp, 85, 33));
      rY = 0;
      sc.AddMM(0, rY, new RepString(fp_Title, "Colors"));
      rY += fp_Title.rLineFeedMM;
      sc.AddMM(0, rY, new RepString(fp, "Red"));
      fp.color = Color.Red;  fp.bUnderline = true;
      sc.AddRightMM(80, rY, new RepString(fp, "ABCDEFGHIJKLM"));
      fp.color = Color.Black;  fp.bUnderline = false;
      rY += fp.rLineFeedMM;
      sc.AddMM(0, rY, new RepString(fp, "Green"));
      fp.color = Color.Green;  fp.bUnderline = true;
      sc.AddRightMM(80, rY, new RepString(fp, "ABCDEFGHIJKLM"));
      fp.color = Color.Black;  fp.bUnderline = false;
      rY += fp.rLineFeedMM;
      sc.AddMM(0, rY, new RepString(fp, "Blue"));
      fp.color = Color.Blue;  fp.bUnderline = true;
      sc.AddRightMM(80, rY, new RepString(fp, "ABCDEFGHIJKLM"));
      fp.color = Color.Black;  fp.bUnderline = false;
      rY += fp.rLineFeedMM;
      sc.AddMM(0, rY, new RepString(fp, "RGB(255,180,255)"));
      fp.color = Color.FromArgb(200, 200, 255);  fp.bUnderline = true;
      sc.AddRightMM(80, rY, new RepString(fp, "ABCDEFGHIJKLM"));
      fp.color = Color.Black;  fp.bUnderline = false;
      rY += rYcopy + fp.rLineFeedMM + 10;

      // alignment sample
      rX = 20;
      String s = "Alignment";
      page.AddMM(rX, rY, new RepString(fp_Title, s));
      Double rLengthMM = fp_Title.rGetTextWidthMM(s);
      page.AddMM(rX, rY + 3, new RepLineMM(pp, rLengthMM, 0));
      page.AddMM(rX, rY + 2, new RepLineMM(pp, 0, 2));
      page.AddMM(rX + rLengthMM, rY + 2, new RepLineMM(pp, 0, 2));
      page.AddAlignedMM(rX + rLengthMM / 2, RepObj.rAlignCenter, rY + 4, RepObj.rAlignTop, new RepReal64(fp_Small, rLengthMM, "0.0 mm"));
      rX = 100;
      rY += fp_Title.rLineFeedMM;
      Double rD = 15;
      bp.color = Color.LightSkyBlue;
      page.AddMM(rX, rY + rD, new RepRectMM(bp, rD, rD));
      page.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, new RepString(fp, "right - bottom"));
      page.AddAlignedMM(rX, RepObj.rAlignRight, rY + rD, RepObj.rAlignTop, new RepString(fp, "right - top"));
      page.AddMM(rX + rD, rY, new RepString(fp, "left - bottom"));  // default
      page.AddAlignedMM(rX + rD, RepObj.rAlignLeft, rY + rD, RepObj.rAlignTop, new RepString(fp, "left - top"));
      page.AddAlignedMM(rX + rD / 2, RepObj.rAlignCenter, rY + rD / 2, RepObj.rAlignCenter, new RepString(fp, "center"));
      rY += 30;

      // rotated string
      rX = 60;
      page.AddMM(20, rY, new RepString(fp_Title, "Rotated Strings"));
      rY += fp_Title.rLineFeedMM + 10;
      rYcopy = rY;
      rD = 10;
      page.AddMM(rX, rY + rD, new RepRectMM(bp, rD, rD));
      fp.rAngle = 45;
      page.AddAlignedMM(rX + rD, RepObj.rAlignLeft, rY + rD, RepObj.rAlignTop, new RepString(fp, "[45°]"));
      fp.rAngle = 135;
      page.AddMM(rX, rY + rD, new RepString(fp, "[135°]"));
      fp.rAngle = 225;
      page.AddAlignedMM(rX, RepObj.rAlignLeft, rY, RepObj.rAlignTop, new RepString(fp, "[225°]"));
      fp.rAngle = 315;
      page.AddMM(rX + rD, rY, new RepString(fp, "[315°]"));
      fp.rAngle = 0;

      rX = 155;
      rY = rYcopy;
      fp.bUnderline = true;
      page.AddMM(rX, rY + rD, new RepRectMM(bp, rD, rD));
      fp.rAngle = 45;
      page.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, new RepString(fp, "1/4 * PI"));
      fp.rAngle = 135;
      page.AddAlignedMM(rX + rD, RepObj.rAlignRight, rY, RepObj.rAlignTop, new RepString(fp, "3/4 * PI"));
      fp.rAngle = 225;
      page.AddAlignedMM(rX + rD, RepObj.rAlignRight, rY + rD, RepObj.rAlignBottom, new RepString(fp, "5/4 * PI"));
      fp.rAngle = 315;
      page.AddAlignedMM(rX, RepObj.rAlignRight, rY + rD, RepObj.rAlignTop, new RepString(fp, "7/4 * PI"));
      fp.rAngle = 0.0;
      fp.bUnderline = false;
      rY += 35;
      #endregion

      #region Page 2
      rX = 20;
      rY = 60;
      String sText = "Once upon a time there was a miller who was poor, but he had a beautiful daughter." + Environment.NewLine +
        "Now it happened that he was talking with the king one time, and in order to make himself seem important, " +
        "he said to the king, \"I have a daughter who can spin straw into gold.\"";
      page = new Page(this);
      page.AddMM(rX, rY, new RepString(fp_Title, "Best Fitting Font"));
      rY += fp_Title.rLineFeedMM;

      page.AddLT_MM(rX, rY, new RepRectMM(bp, 80, 40));
      FontProp fp_BestFit = fp_Title.fontProp_GetBestFitMM(sText, 80, 40, 1);
      rY += fp_BestFit.rLineFeedMM;
      Int32 iStart = 0;
      while (iStart <= sText.Length) {
        String sLine = fp_BestFit.sGetTextLine(sText, RT.rPointFromMM(80), ref iStart, TextSplitMode.Line);
        page.AddMM(rX, rY, new RepString(fp_BestFit, sLine));
        rY += fp_BestFit.rLineFeedMM;
      }
      rY += 10;

      page.AddLT_MM(rX, rY, new RepRectMM(bp, 40, 40));
      fp_BestFit = fp.fontProp_GetBestFitMM(sText, 40, 40, 1);
      rY += fp_BestFit.rLineFeedMM;
      iStart = 0;
      while (iStart <= sText.Length) {
        String sLine = fp_BestFit.sGetTextLine(sText, RT.rPointFromMM(40), ref iStart, TextSplitMode.Line);
        page.AddMM(rX, rY, new RepString(fp_BestFit, sLine));
        rY += fp_BestFit.rLineFeedMM;
      }
      rY += 10;

      page.AddLT_MM(rX, rY, new RepRectMM(bp, 30, 30));
      fp_BestFit = fp.fontProp_GetBestFitMM(sText, 30, 30, 1);
      rY += fp_BestFit.rLineFeedMM;
      iStart = 0;
      while (iStart <= sText.Length) {
        String sLine = fp_BestFit.sGetTextLine(sText, RT.rPointFromMM(30), ref iStart, TextSplitMode.Line);
        page.AddMM(rX, rY, new RepString(fp_BestFit, sLine));
        rY += fp_BestFit.rLineFeedMM;
      }
      #endregion
    }

  }
}
