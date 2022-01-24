using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

// Creation date: 22.04.2002
// Checked: xx.05.2002
// Author: Otto Mayer, mot@root.ch
// Version 0.00.00

// copyright (C) 2002 root-software ag  -  Bürglen Switzerland  -  www.root.ch; Otto Mayer, Stefan Spirig, Roger Gartenmann
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>
  /// Summary description for StaticContainer.
  /// </summary>
  public class StaticContainer : Container {
    /// <summary>
    /// 
    /// </summary>
    public StaticContainer(Double rWidth, Double rHeight) {
      this.rWidth = rWidth;
      this.rHeight = rHeight;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public new void Add(Double rX, Double rY, RepObj repObj) {
        //Added By TechnoGuru - jjborie@yahoo.fr - http://www.borie.org/
        //Here we handle image comosed of severals images
        if(repObj is RepImage)
        {
#if !WindowsCE
            RepImage repImage = repObj as RepImage;
            using (Image image = Image.FromStream(repImage.stream))
            {
                if(image.RawFormat.Equals(ImageFormat.Tiff))
                {
                    Guid objGuid = (image.FrameDimensionsList[0]);
                    System.Drawing.Imaging.FrameDimension objDimension = new System.Drawing.Imaging.FrameDimension(objGuid);
                    // Numbre of image in the tiff file
                    int totFrame = image.GetFrameCount(objDimension);
                    if(totFrame > 1)
                    {
                        // Saves every frame in a seperate file.
                        for(int i=0;i<totFrame;i++)
                        {
                            image.SelectActiveFrame(objDimension, i);
                            string tempFile = Path.GetTempFileName() + ".tif";
                            if(image.PixelFormat.Equals(PixelFormat.Format1bppIndexed)) 
                            {
                                ImageCodecInfo myImageCodecInfo;
                                myImageCodecInfo = GetEncoderInfo("image/tiff");
                                EncoderParameters myEncoderParameters;
                                myEncoderParameters = new EncoderParameters(1);
                                myEncoderParameters.Param[0] = new
                                    EncoderParameter(Encoder.Compression,(long)EncoderValue.CompressionCCITT4);
                                image.Save(tempFile, myImageCodecInfo, myEncoderParameters);
                            }
                            else
                            {
                                image.Save(tempFile, ImageFormat.Tiff);
                            }
                            FileStream stream = new System.IO.FileStream(tempFile,FileMode.Open, FileAccess.Read);

                            if(i == 0)
                            {
                                repImage.stream = stream;
                                repObj = repImage as RepObj;
                            }
                            else
                            {
                                new Page(report);
                                RepImage di = new RepImageMM(stream, Double.NaN, Double.NaN);
                                report.page_Cur.Add(0, 0, di);
                            }
                        }
                    }
                }
            }
#endif
        }
        base.Add(rX, rY, repObj);
    }

    #if !WindowsCE
      //Added By TechnoGuru - jjborie@yahoo.fr - http://www.borie.org/
      /// <summary>
      /// Get encoding info for a mime type
      /// </summary>
      /// <param name="mimeType"></param>
      /// <returns></returns>
      private static ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for(j = 0; j < encoders.Length; ++j)
        {
            if(encoders[j].MimeType == mimeType)
                return encoders[j];
        }
        return null;
    }
    #endif

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container (metric version).</summary>
    /// <param name="rX">X-coordinate of the report object in millimeter</param>
    /// <param name="rY">Y-coordinate of the report objectt in millimeter</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddMM(Double rX, Double rY, RepObj repObj) {
      Add(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
    /// <param name="repObj">Report object to add to the container</param>
    public new void AddAligned(Double rX, Double rAlignH, Double rY, Double rAlignV, RepObj repObj) {
      repObj.matrixD.rDX = rX;
      repObj.rAlignH = rAlignH;
      repObj.matrixD.rDY = rY;
      repObj.rAlignV = rAlignV;
      Add(repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment (metric versîon).</summary>
    /// <param name="rX">X-coordinate of the report objectt in millimeter</param>
    /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
    /// <param name="rY">Y-coordinate of the report objectt in millimeter</param>
    /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddAlignedMM(Double rX, Double rAlignH, Double rY, Double rAlignV, RepObj repObj) {
      AddAligned(RT.rPointFromMM(rX), rAlignH, RT.rPointFromMM(rY), rAlignV, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container, horizontally centered.</summary>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddCB(Double rY, RepObj repObj) {
      base.AddCB(rWidth / 2.0, rY, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container, horizontally centered.</summary>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    [Obsolete("use 'AddCB(...)'")]
    public void AddCentered(Double rY, RepObj repObj) {
      repObj.matrixD.rDX = rWidth / 2;
      repObj.rAlignH = RepObj.rAlignCenter;
      repObj.matrixD.rDY = rY;
      Add(repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container, horizontally centered (metric version).</summary>
    /// <param name="rY">Y-coordinate of the report objectt in millimeter</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddCB_MM(Double rY, RepObj repObj) {
      AddCB(RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container, horizontally centered (metric version).</summary>
    /// <param name="rY">Y-coordinate of the report objectt in millimeter</param>
    /// <param name="repObj">Report object to add to the container</param>
    [Obsolete("use 'AddCB_MM(...)'")]
    public void AddCenteredMM(Double rY, RepObj repObj) {
      AddCentered(RT.rMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container, right justified.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddRight(Double rX, Double rY, RepObj repObj) {
      AddAligned(rX, RepObj.rAlignRight, rY, RepObj.rAlignBottom, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container, right justified (metric version).</summary>
    /// <param name="rX">X-coordinate of the report objectt in millimeter</param>
    /// <param name="rY">Y-coordinate of the report objectt in millimeter</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddRightMM(Double rX, Double rY, RepObj repObj) {
      AddRight(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }
  }

  //====================================================================================================x
  /// <summary>
  /// Summary description for StaticContainer.
  /// </summary>
  public class StaticContainerMM : StaticContainer {
    /// <summary>
    /// 
    /// </summary>
    public StaticContainerMM(Double rWidth, Double rHeight) : base(RT.rPointFromMM(rWidth), RT.rPointFromMM(rHeight)) {
    }

  }

}
