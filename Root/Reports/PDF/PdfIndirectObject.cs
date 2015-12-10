using System;
using System.Collections;
#if Framework2
using System.Collections.Generic;
#endif
using System.Drawing;
using System.IO;

// Creation date: 19.01.2006
// Checked: 01.02.2006
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
  //------------------------------------------------------------------------------------------01.02.2006
  #region PdfIndirectObject
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Base Class</summary>
  internal abstract class PdfIndirectObject : PdfFileElement {
    /// <summary>PDF Object Number</summary>
    internal readonly Int32 iObjectNumber;

    /// <summary>Position of this object in the PDF file</summary>
    internal Int32 iObjectPosition = 0;

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Creates an indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfIndirectObject(PdfFormatter pdfFormatter) : base(pdfFormatter) {
      pdfFormatter.list_PdfIndirectObject.Add(this);
      iObjectNumber = pdfFormatter.list_PdfIndirectObject.Count;
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object-start sequence to the buffer.</summary>
    protected void StartObj() {
      pdfFormatter.FlushBuffer();
      iObjectPosition = pdfFormatter.iBytesWrittenToStream;

      Number(iObjectNumber);  Space();  Number(0);  Space();  Token("obj");
      NewLine();
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object-end sequence to the buffer.</summary>
    protected void EndObj() {
      Token("endobj");
      NewLine();
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Writes the font properties into the buffer.</summary>
    /// <param name="fontProp">New font properties</param>
    internal void Command(String sCommand) {
      Space();
      WriteLine(sCommand);
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------01.02.2006
  #region PdfIndirectObject_Catalog
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Catalog</summary>
  internal sealed class PdfIndirectObject_Catalog : PdfIndirectObject {
    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Creates a catalog indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfIndirectObject_Catalog(PdfFormatter pdfFormatter) : base(pdfFormatter) {
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      StartObj();
      Dictionary_Start();
      Dictionary_Key("Type");  Name("Catalog");
      Dictionary_Key("Version");  Name("1.4");
      Dictionary_Key("Pages");  IndirectReference(pdfFormatter.pdfIndirectObject_Pages);
      if (pdfFormatter.pageLayout != PageLayout.SinglePage) {
        Dictionary_Key("PageLayout");  Name(pdfFormatter.pageLayout.ToString("G"));
      }
      if (pdfFormatter.pageMode != PdfFormatter.PageMode.UseNone) {
        Dictionary_Key("PageMode");  Name(pdfFormatter.pageMode.ToString("G"));
      }
      if (pdfFormatter.pdfIndirectObject_ViewerPreferences != null) {
        Dictionary_Key("ViewerPreferences");  IndirectReference(pdfFormatter.pdfIndirectObject_ViewerPreferences);
      }
      if (pdfFormatter.sOpenActionURI != null) {
        Dictionary_Key("OpenAction");
          Dictionary_Start();
          Dictionary_Key("Type");  Name("Action");
          Dictionary_Key("S");  Name("URI");
          Dictionary_Key("URI");  String(pdfFormatter.sOpenActionURI);
          Dictionary_End();
      }
      else if (pdfFormatter.sOpenActionLaunch != null) {
        Dictionary_Key("OpenAction");
          Dictionary_Start();
          Dictionary_Key("Type");  Name("Action");
          Dictionary_Key("S");  Name("Launch");
          Dictionary_Key("F");  String(pdfFormatter.sOpenActionLaunch);
          Dictionary_End();
      }
      Dictionary_End();
      EndObj();
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------01.02.2006
  #region PdfIndirectObject_Info
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Info</summary>
  internal sealed class PdfIndirectObject_Info : PdfIndirectObject {
    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Creates an info indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfIndirectObject_Info(PdfFormatter pdfFormatter) : base(pdfFormatter) {
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      StartObj();
      Dictionary_Start();
      if (report.sTitle != null) {
        Dictionary_Key("Title");  String(report.sTitle);
      }
      if (report.sAuthor != null) {
        Dictionary_Key("Author");  String(report.sAuthor);
      }
      if (pdfFormatter.sSubject != null) {
        Dictionary_Key("Subject");  String(pdfFormatter.sSubject);
      }
      if (pdfFormatter.sKeywords != null) {
        Dictionary_Key("Keywords");  String(pdfFormatter.sKeywords);
      }
      if (pdfFormatter.sCreator != null) {
        Dictionary_Key("Creator");  String(pdfFormatter.sCreator);
      }
      if (pdfFormatter.sProducer != null) {
        Dictionary_Key("Producer");  String(pdfFormatter.sProducer);
      }
      if (pdfFormatter.dt_CreationDate.Year > 0) {
        Dictionary_Key("CreationDate");  Date(pdfFormatter.dt_CreationDate);
      }
      if (pdfFormatter.dt_ModDate.Year > 0) {
        Dictionary_Key("ModDate");  Date(pdfFormatter.dt_ModDate);
      }
      Dictionary_End();
      EndObj();
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------01.02.2006
  #region PdfIndirectObject_ViewerPreferences
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: viewer-preferences</summary>
  internal sealed class PdfIndirectObject_ViewerPreferences : PdfIndirectObject {
    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Creates a viewer-preferences indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfIndirectObject_ViewerPreferences(PdfFormatter pdfFormatter) : base(pdfFormatter) {
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      StartObj();
      Dictionary_Start();
      if (pdfFormatter.bHideToolBar) {
        Dictionary_Key("HideToolbar");  Boolean(true);
      }
      if (pdfFormatter.bHideMenubar) {
        Dictionary_Key("HideMenubar");  Boolean(true);
      }
      if (pdfFormatter.bHideWindowUI) {
        Dictionary_Key("HideWindowUI");  Boolean(true);
      }
      if (pdfFormatter.bFitWindow) {
        Dictionary_Key("FitWindow");  Boolean(true);
      }
      if (pdfFormatter.bCenterWindow) {
        Dictionary_Key("CenterWindow");  Boolean(true);
      }
      if (pdfFormatter.bDisplayDocTitle) {
        Dictionary_Key("DisplayDocTitle");  Boolean(true);
      }
      if (pdfFormatter.nonFullScreenPageMode != PdfFormatter.NonFullScreenPageMode.UseNone) {
        Dictionary_Key("NonFullScreenPageMode");  Name(pdfFormatter.nonFullScreenPageMode.ToString("G"));
      }
      Dictionary_End();
      EndObj();
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------01.02.2006
  #region PdfIndirectObject_ImageJpeg
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: JPEG Image</summary>
  internal sealed class PdfIndirectObject_ImageJpeg : PdfIndirectObject {
    /// <summary>JPEG Image</summary>
#warning ImageData ==> ImageResource
    private ImageData imageResource_Jpeg; 

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Creates a JPEG image indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    /// <param name="imageResource_Jpeg">JPEG Image Resource</param>
    internal PdfIndirectObject_ImageJpeg(PdfFormatter pdfFormatter, ImageData imageResource_Jpeg) : base(pdfFormatter) {
      this.imageResource_Jpeg = imageResource_Jpeg;
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      StartObj();
      imageResource_Jpeg.stream.Position = 0;
      using (Image image = Image.FromStream(imageResource_Jpeg.stream)) {
        Dictionary_Start();
        Dictionary_Key("Type");  Name("XObject");
        Dictionary_Key("Subtype");  Name("Image");
        Dictionary_Key("Name");  Name("I" + iObjectNumber);
        Dictionary_Key("Width");  Number(image.Width);
        Dictionary_Key("Height");  Number(image.Height);
        Dictionary_Key("Filter");  Name("DCTDecode");
        Dictionary_Key("BitsPerComponent");  Number(8);
        Dictionary_Key("ColorSpace");  Name("DeviceRGB");
        Int64 lLength = imageResource_Jpeg.stream.Length;
        Dictionary_Key("Length");  Number(lLength);
        Dictionary_End();
        NewLine();
        Command("stream");
        pdfFormatter.FlushBuffer();

        imageResource_Jpeg.stream.Position = 0;
        BinaryReader r = new BinaryReader(imageResource_Jpeg.stream);
        Byte[] aByte = r.ReadBytes((Int32)lLength);
        r.Close();

        //stream.Flush();
        pdfFormatter.bufferedStream.Write(aByte, 0, aByte.Length);
        pdfFormatter.iBytesWrittenToStream += aByte.Length;
        WriteLine("\nendstream");

        imageResource_Jpeg.stream.Close();
        imageResource_Jpeg.stream = null;
        EndObj();
      }
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------01.02.2006
  #region PdfIndirectObject_Pages
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Pages</summary>
  internal sealed class PdfIndirectObject_Pages : PdfIndirectObject {
    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Creates a pages indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfIndirectObject_Pages(PdfFormatter pdfFormatter) : base(pdfFormatter) {
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      StartObj();
      Dictionary_Start();
      Dictionary_Key("Type");  Name("Pages");
      Dictionary_Key("Count");  Number(report.iPageCount);
      Dictionary_Key("Kids");
        ArrayStart();
        foreach (Page page in report.enum_Page) {
          PdfIndirectObject_Page pdfIndirectObject_Page = (PdfIndirectObject_Page)page.oRepObjX;
          IndirectReference(pdfIndirectObject_Page);
        }
        ArrayEnd();
      Dictionary_End();
      EndObj();
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------xx.02.2006
  #region PdfIndirectObject_Page
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Page</summary>
  internal sealed class PdfIndirectObject_Page : PdfIndirectObject, IPdfRepObjX {
    /// <summary>Page</summary>
    private Page page;

    /// <summary>Page contents indirect object</summary>
    internal PdfIndirectObject_PageContents pdfIndirectObject_PageContents;

    /// <summary>List of all font properties that are used on this page</summary>
    /// <remarks>This list is used in the PDF page object to specify the font resources.</remarks>
    #if Framework2
    private readonly Dictionary<String, FontData> dict_FontData = new Dictionary<String, FontData>(20);
    #else
    private Hashtable ht_FontData = new Hashtable(20);
    #endif

    /// <summary>ProcSet PDF: painting and graphics state</summary>
    internal Boolean bProcSet_PDF = false;

    /// <summary>ProcSet Text: text</summary>
    internal Boolean bProcSet_Text = false;

    /// <summary>ProcSet ImageB: grayscale images or image masks</summary>
    internal Boolean bProcSet_ImageB = false;

    /// <summary>ProcSet ImageC: color images</summary>
    internal Boolean bProcSet_ImageC = false;

    /// <summary>ProcSet ImageI: indexed (color-table) images</summary>
    internal Boolean bProcSet_ImageI = false;

    //------------------------------------------------------------------------------------------03.02.2006
    /// <summary>Creates a page indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfIndirectObject_Page(PdfFormatter pdfFormatter, Page page) : base(pdfFormatter) {
      this.page = page;
    }

    //------------------------------------------------------------------------------------------xx.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      StartObj();
      Dictionary_Start();
      Dictionary_Key("Type");  Name("Page");
      Dictionary_Key("Parent");  IndirectReference(pdfFormatter.pdfIndirectObject_Pages);
      Dictionary_Key("Resources");  Dictionary_Start();
        Dictionary_Key("ProcSet");  ArrayStart();
          if (bProcSet_Text) {
            Name("Text");
          }
          if (bProcSet_PDF) {
            Space();
            Name("PDF");
          }
          if (bProcSet_ImageB) {
            Space();
            Name("ImageB");
          }
          if (bProcSet_ImageC) {
            Space();
            Name("ImageC");
          }
          if (bProcSet_ImageI) {
            Space();
            Name("ImageI");
          }
          ArrayEnd();
        Dictionary_Key("Font");  Dictionary_Start();
          foreach (FontData fontData in dict_FontData.Values) {
            PdfIndirectObject_Font pdfIndirectObject_Font = (PdfIndirectObject_Font)fontData.oFontDataX;
            Dictionary_Key("F" + pdfIndirectObject_Font.iObjectNumber);  IndirectReference(pdfIndirectObject_Font);
          }
          Dictionary_End();
        if (report.ht_ImageData.Count > 0) {
          Dictionary_Key("XObject");  Dictionary_Start();
          foreach (ImageData imageData in report.ht_ImageData.Values) {
            PdfIndirectObject_ImageJpeg pdfIndirectObject_ImageJpeg = (PdfIndirectObject_ImageJpeg)imageData.oImageResourceX;
            Dictionary_Key("I" + pdfIndirectObject_ImageJpeg.iObjectNumber);  IndirectReference(pdfIndirectObject_ImageJpeg);
          }
          Dictionary_End();
        }
        Dictionary_End();

      Dictionary_Key("MediaBox");  ArrayStart();
        Number(0);  Space();  Number(0);  Space();  Number(page.rWidth);  Space();  Number(page.rHeight);
        ArrayEnd();
      Dictionary_Key("CropBox");  ArrayStart();
        Number(0);  Space();  Number(0);  Space();  Number(page.rWidth);  Space();  Number(page.rHeight);
        ArrayEnd();
      //Dictionary_Key("Rotate");  Number(0);  // !!!
      Dictionary_Key("Contents");  IndirectReference(pdfIndirectObject_PageContents);
      Dictionary_End();
      EndObj();
    }

    //------------------------------------------------------------------------------------------xx.02.2006
    /// <summary>Registers the specified font properties for this page.</summary>
    /// <param name="fontProp">Font properties</param>
    /// <seealso cref="PdfPageData.ht_FontProp"/>
    /// <seealso cref="PdfFontPropData.pdfPageData_Registered"/>
    internal void RegisterFontData(FontData fontData) {
      PdfIndirectObject_Font pdfIndirectObject_Font = (PdfIndirectObject_Font)fontData.oFontDataX;
      if (pdfIndirectObject_Font.pdfIndirectObject_Page != this) {
        dict_FontData.Add(pdfIndirectObject_Font.sKey, fontData);
        pdfIndirectObject_Font.pdfIndirectObject_Page = this;
      }
    }

    //------------------------------------------------------------------------------------------01.02.2006
    #region IPdfRepObjX Members
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------xx.02.2006
    /// <summary>Writes the RepObj to the buffer.</summary>
    /// <param name="e">Environment data</param>
    public void Write(PdfIndirectObject_PageContents.Environment e) {
      Container container = (Container)e.repObj;

      PdfIndirectObject_PageContents.Environment e2 = new PdfIndirectObject_PageContents.Environment();
      e2.report = e.report;
      e2.pdfIndirectObject_PageContents = e.pdfIndirectObject_PageContents;

      foreach (RepObj repObj in container) {
        IPdfRepObjX pdfRepObjX = (IPdfRepObjX)repObj.oRepObjX;
        e2.repObj = repObj;
        e2.matrixD = e2.matrixD.Clone();
        e2.matrixD.Multiply(repObj.matrixD);
        e2.bComplex = e2.matrixD.bComplex;
        pdfRepObjX.Write(e2);
      }
    }
    #endregion
  }
  #endregion
}
