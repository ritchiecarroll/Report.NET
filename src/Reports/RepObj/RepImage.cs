using System;
using System.Drawing;
using System.IO;

// Creation date: 24.04.2002
// Checked: 05.08.2002
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
  /// <summary>Report Image Object.</summary>
  public class RepImage : RepObj {
    /// <summary>Image stream</summary>
    internal Stream stream;

    /// <summary>Image data</summary>
    internal ImageData imageData;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new image object.</summary>
    /// <param name="sFileName">Filename of the Image</param>
    /// <param name="rWidth">Width of the image</param>
    /// <param name="rHeight">Height of the image</param>
    public RepImage(String sFileName, Double rWidth, Double rHeight) {
      this.stream = new FileStream(sFileName, FileMode.Open);
      this.rWidth = rWidth;
      this.rHeight = rHeight;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new image object.</summary>
    /// <param name="stream">Image stream</param>
    /// <param name="rWidth">Width of the image</param>
    /// <param name="rHeight">Height of the image</param>
    public RepImage(Stream stream, Double rWidth, Double rHeight) {
      this.stream = stream;
      this.rWidth = rWidth;
      this.rHeight = rHeight;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called after the report object has been added to the container.</summary>
    internal protected override void OnAdded() {
      oRepObjX = report.formatter.oCreate_RepImage();

      imageData = (ImageData)report.ht_ImageData[stream];
      if (imageData == null) {
        imageData = new ImageData(stream);
        report.ht_ImageData.Add(stream, imageData);
      }

      //Changed By TechnoGuru - jjborie@yahoo.fr - http://www.borie.org/
      imageData.stream.Position = 0;
#if !WindowsCE
      using (Image image = Image.FromStream(imageData.stream)) {
        if (Double.IsNaN(rWidth)) {
          if (Double.IsNaN(rHeight)) {
            rWidth = image.Width / image.HorizontalResolution * 72;
            rHeight = image.Height / image.VerticalResolution * 72;;
          }
          else {
            rWidth = image.Width * rHeight / image.Height;
          }
        }
        else if (Double.IsNaN(rHeight)) {
          rHeight = image.Height * rWidth / image.Width;
        }
      }
#endif
    }
  }


  //****************************************************************************************************
  /// <summary>Creates a new image object.</summary>
  public class RepImageMM : RepImage {
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new Image object with millimeter values</summary>
    /// <param name="sFileName">Filename of the Image</param>
    /// <param name="rWidth">Width of the image in millimeter</param>
    /// <param name="rHeight">Height of the image in millimeter</param>
    public RepImageMM(String sFileName, Double rWidth, Double rHeight) : base(sFileName, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight)) {
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new Image object with millimeter values</summary>
    /// <param name="stream">Image stream</param>
    /// <param name="rWidth">Width of the image in millimeter</param>
    /// <param name="rHeight">Height of the image in millimeter</param>
    public RepImageMM(Stream stream, Double rWidth, Double rHeight) : base(stream, RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight)) {
    }

  }

}
