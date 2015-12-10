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
  /// <summary>Image Sample</summary>
  public class ImageSample : Report {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates this report</summary>
    protected override void Create() {
      FontDef fd = new FontDef(this, FontDef.StandardFont.Helvetica);
      FontProp fp = new FontPropMM(fd, 2.1);
      FontProp fp_Title = new FontPropMM(fd, 15);
      fp_Title.bBold = true;
      FontProp fp_SubTitle = new FontPropMM(fd, 4);
      fp_SubTitle.bBold = true;
      BrushProp bp = new BrushProp(this, Color.LightGray);
      PenProp pp = new PenProp(this, 0.2, Color.FromArgb(235, 235, 235));
      PenProp pp_Black = new PenProp(this, 0.2, Color.Black);
      Double rY = 40;

      new Page(this);
      page_Cur.AddCB_MM(rY, new RepString(fp_Title, "Image Sample"));

      System.IO.Stream stream = GetType().Assembly.GetManifestResourceStream("ReportSamples.Image.jpg");

      page_Cur.AddMM(20, 90, new RepImageMM(stream, 40, Double.NaN));
      page_Cur.AddMM(20, 95, new RepString(fp, "W = 40mm, H = auto."));
      page_Cur.AddMM(67, 90, new RepImageMM(stream, 40, 20));
      page_Cur.AddMM(67, 95, new RepString(fp, "W = 40mm, H = 20mm"));
      page_Cur.AddMM(114, 90, new RepImageMM(stream, Double.NaN, 30));
      page_Cur.AddMM(114, 95, new RepString(fp, "W = auto., H = 30mm"));
      page_Cur.AddMM(161, 90, new RepImageMM(stream, 30, 30));
      page_Cur.AddMM(161, 95, new RepString(fp, "W = 30mm, H = 30mm"));
      rY +=  150;

      // adjust the size of a bounding rectangle
      RepRect dr = new RepRectMM(bp, 80, 60);
      page_Cur.AddMM(20, rY, dr);
      RepImage di  = new RepImageMM(stream, 70, Double.NaN);
      page_Cur.AddMM(25, rY - 5, di);
      dr.rHeightMM = di.rHeightMM + 10;

      // rotated image
      di = new RepImageMM(stream, 40, 30);
      di.RotateTransform(-15);
      page_Cur.AddMM(120, rY - 33, di);

      // rotated image with rectangle
      StaticContainer sc = new StaticContainer(RT.rPointFromMM(45), RT.rPointFromMM(35));
      page_Cur.AddMM(145, rY - 35, sc);
      sc.RotateTransform(15);
//      sc.AddMM(0, 35, new RepRectMM(bp, 45, 35));
      sc.AddMM(1.25, 33.75, new RepLineMM(pp, 42.5, 0));
      sc.AddMM(1.25, 1.25, new RepLineMM(pp, 42.5, 0));
      sc.AddAlignedMM(22.5, RepObj.rAlignCenter, 17.5, RepObj.rAlignCenter, new RepImageMM(stream, 40, 30));
      rY += 30;

      // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
      // alignment sample
      page_Cur.AddMM(20, rY, new RepString(fp_SubTitle, "Alignment"));
      rY += 18;
      Int32 rX = 40;
      Double rD = 20;
      bp.color = Color.DarkSalmon;
      page_Cur.AddMM(rX, rY + rD, new RepRectMM(bp, rD, rD));
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, new RepImageMM(stream, 20, Double.NaN));
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY + rD, RepObj.rAlignTop, new RepImageMM(stream, 20, Double.NaN));
      page_Cur.AddMM(rX + rD, rY, new RepImageMM(stream, 20, Double.NaN));  // default
      page_Cur.AddAlignedMM(rX + rD, RepObj.rAlignLeft, rY + rD, RepObj.rAlignTop, new RepImageMM(stream, 20, Double.NaN));
      page_Cur.AddAlignedMM(rX + rD / 2, RepObj.rAlignCenter, rY + rD / 2, RepObj.rAlignCenter, new RepImageMM(stream, 10, Double.NaN));

      // rotated
      rX = 140;
      page_Cur.AddMM(rX, rY + rD, new RepRectMM(bp, rD, rD));
      RepImage repImage = new RepImageMM(stream, 20, Double.NaN);
      repImage.RotateTransform(15);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, repImage);
      repImage = new RepImageMM(stream, 20, Double.NaN);
      repImage.RotateTransform(15);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY + rD, RepObj.rAlignTop, repImage);
      repImage = new RepImageMM(stream, 20, Double.NaN);
      repImage.RotateTransform(15);
      page_Cur.AddMM(rX + rD, rY, repImage);  // default
      repImage = new RepImageMM(stream, 20, Double.NaN);
      repImage.RotateTransform(15);
      page_Cur.AddAlignedMM(rX + rD, RepObj.rAlignLeft, rY + rD, RepObj.rAlignTop, repImage);
      repImage = new RepImageMM(stream, 10, Double.NaN);
      repImage.RotateTransform(15);
      page_Cur.AddAlignedMM(rX + rD / 2, RepObj.rAlignCenter, rY + rD / 2, RepObj.rAlignCenter, repImage);

      // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
      new Page(this);
      rY = 30;
      
      page_Cur.AddCB_MM(rY, new RepString(fp_Title, "Ellipse Sample"));
      rY += 15;

      // arc
      page_Cur.AddLT_MM(20, rY, new RepString(fp_SubTitle, "Arc"));
      page_Cur.AddLT_MM(50, rY, new RepRectMM(pp_Black, 40, 30));
      page_Cur.AddLT_MM(50, rY, new RepArcMM(pp_Black, 40, 30, 45, 270));
      page_Cur.AddLT_MM(100, rY, new RepArcMM(pp_Black, 15, 135, 270));
      page_Cur.AddLT_MM(150, rY, new RepArcMM(pp_Black, 40, 30, 150, 130));
      rY += 35;

      // circle
      page_Cur.AddLT_MM(20, rY, new RepString(fp_SubTitle, "Circle"));
      page_Cur.AddLT_MM(50, rY, new RepRectMM(pp_Black, 30, 30));
      page_Cur.AddLT_MM(50, rY, new RepCircleMM(pp_Black, bp, 15));
      page_Cur.AddLT_MM(100, rY, new RepCircleMM(pp_Black, 15));
      page_Cur.AddLT_MM(150, rY, new RepCircleMM(bp, 15));
      rY += 35;

      // ellipse
      page_Cur.AddLT_MM(20, rY, new RepString(fp_SubTitle, "Ellipse"));
      page_Cur.AddLT_MM(50, rY, new RepRectMM(pp_Black, 40, 30));
      page_Cur.AddLT_MM(50, rY, new RepEllipseMM(pp_Black, bp, 40, 30));
      page_Cur.AddLT_MM(100, rY, new RepEllipseMM(pp_Black, 40, 30));
      page_Cur.AddLT_MM(150, rY, new RepEllipseMM(bp, 40, 30));
      rY += 35;

      // pie ellipse
      page_Cur.AddLT_MM(20, rY, new RepString(fp_SubTitle, "Pie Ellipse"));
      page_Cur.AddLT_MM(50, rY, new RepRectMM(pp_Black, 40, 30));
      page_Cur.AddLT_MM(50, rY, new RepPieMM(pp_Black, bp, 40, 30, -135, 225));
      page_Cur.AddLT_MM(100, rY, new RepPieMM(pp_Black, 40, 30, 45, 270));
      page_Cur.AddLT_MM(150, rY, new RepPieMM(bp, 40, 30, 135, 225));
      rY += 35;

      // pie circle
      page_Cur.AddLT_MM(20, rY, new RepString(fp_SubTitle, "Pie Circle"));
      page_Cur.AddLT_MM(50, rY, new RepRectMM(pp_Black, 30, 30));
      page_Cur.AddLT_MM(50, rY, new RepPieMM(pp_Black, bp, 15, -135, 225));
      page_Cur.AddLT_MM(100, rY, new RepPieMM(pp_Black, 15, 45, 270));
      page_Cur.AddLT_MM(150, rY, new RepPieMM(bp, 15, 135, 225));
      rY += 45;

      // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
      // Pie alignment sample
      page_Cur.AddMM(20, rY, new RepString(fp_SubTitle, "Pie Alignment"));
      page_Cur.AddMM(105, rY, new RepString(fp_SubTitle, "rotated 30°"));
      rY += 25;
      rX = 50;
      rD = 20;
      bp.color = Color.DarkSalmon;
      page_Cur.AddAlignedMM(rX, RepObj.rAlignCenter, rY, RepObj.rAlignCenter, new RepRectMM(bp, rD, rD));
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, new RepPieMM(pp_Black, bp, rD, rD, 100, 250));
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignTop, new RepPieMM(pp_Black, bp, rD, rD, 10, 250));
      page_Cur.AddMM(rX, rY, new RepPieMM(pp_Black, bp, rD, rD, 190, 250));  // default
      page_Cur.AddAlignedMM(rX, RepObj.rAlignLeft, rY, RepObj.rAlignTop, new RepPieMM(pp_Black, bp, rD, rD, -80, 250));
      page_Cur.AddAlignedMM(rX, RepObj.rAlignCenter, rY, RepObj.rAlignCenter, new RepCircleMM(pp_Black, bp, rD / 2));
      
      rX = 150;
      page_Cur.AddAlignedMM(rX, RepObj.rAlignCenter, rY, RepObj.rAlignCenter, new RepRectMM(bp, rD, rD));

      RepPie pie = new RepPieMM(pp_Black, bp, rD, rD, 100, 250);
      pie.RotateTransform(30);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, pie);
      RepRect rect = new RepRectMM(pp_Black, rD, rD);
      rect.RotateTransform(30);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, rect);
      
      pie = new RepPieMM(pp_Black, bp, rD, rD, 10, 250);
      pie.RotateTransform(30);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignTop, pie);
      rect = new RepRectMM(pp_Black, rD, rD);
      rect.RotateTransform(30);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignRight, rY, RepObj.rAlignTop, rect);
      
      pie = new RepPieMM(pp_Black, bp, rD, rD, 190, 250);
      pie.RotateTransform(30);
      page_Cur.AddMM(rX, rY, pie);  // default
      rect = new RepRectMM(pp_Black, rD, rD);
      rect.RotateTransform(30);
      page_Cur.AddMM(rX, rY, rect);
      
      pie = new RepPieMM(pp_Black, bp, rD, rD, -80, 250);
      pie.RotateTransform(30);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignLeft, rY, RepObj.rAlignTop, pie);
      rect = new RepRectMM(pp_Black, rD, rD);
      rect.RotateTransform(30);
      page_Cur.AddAlignedMM(rX, RepObj.rAlignLeft, rY, RepObj.rAlignTop, rect);

      page_Cur.AddAlignedMM(rX, RepObj.rAlignCenter, rY, RepObj.rAlignCenter, new RepCircleMM(pp_Black, bp, rD / 2));
    }
  }
}
