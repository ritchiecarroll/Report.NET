using System;
using System.Drawing;

// Creation date: 19.01.2006
// Checked: 29.07.2006
// Author: Otto Mayer (mot@root.ch)
// Version: 2.01

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  //------------------------------------------------------------------------------------------29.07.2006
  #region PdfIndirectObject_Font
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Font</summary>
  /// <remarks>Each font data object that is used in the PDF document must point to an object of this type (FontData.oFontDataX).</remarks>
  internal abstract class PdfIndirectObject_Font : PdfIndirectObject {
    /// <summary>Font data</summary>
    protected readonly FontData fontData;

    internal readonly String sKey;

    /// <summary>This variable allows a quick test, whether the font properties are registered for the current page.
    /// If <c>pdfPageData_Registered</c> contains the current page, then it has been registered before.</summary>
    internal PdfIndirectObject_Page pdfIndirectObject_Page;

    //------------------------------------------------------------------------------------------29.07.2006
    /// <summary>Creates a font indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    /// <param name="fontProp">Font property</param>
    internal PdfIndirectObject_Font(PdfFormatter pdfFormatter, FontData fontData) : base(pdfFormatter) {
      this.fontData = fontData;

      sKey = fontData.fontDef.sFontName;
      if ((fontData.fontStyle & FontStyle.Bold) > 0) {
        sKey += ";B";
      }
      if ((fontData.fontStyle & FontStyle.Italic) > 0) {
        sKey += ";I";
      }
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------29.07.2006
  #region PdfIndirectObject_Font_Type1
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Font Type1</summary>
  internal sealed class PdfIndirectObject_Font_Type1 : PdfIndirectObject_Font {
    //------------------------------------------------------------------------------------------29.07.2006
    /// <summary>Creates a font indirect object for a Type1 font.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    /// <param name="type1FontData">Type1 font data</param>
    internal PdfIndirectObject_Font_Type1(PdfFormatter pdfFormatter, Type1FontData type1FontData)
      : base(pdfFormatter, type1FontData)
    {
    }

    //------------------------------------------------------------------------------------------29.07.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      Type1FontData type1FontData = (Type1FontData)fontData;
      StartObj();
      Dictionary_Start();
      Dictionary_Key("Type");  Name("Font");
      Dictionary_Key("Subtype");  Name("Type1");
      Dictionary_Key("BaseFont");  Name(type1FontData.sFontName);
      if (type1FontData.sFamilyName != "ZapfDingbats" && type1FontData.sFamilyName != "Symbol") {
        Dictionary_Key("Encoding");  Name("WinAnsiEncoding");
      }
      Dictionary_End();
      EndObj();
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------04.08.2006
  #region PdfIndirectObject_Font_OpenType
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Font Open Type</summary>
  internal sealed class PdfIndirectObject_Font_OpenType : PdfIndirectObject_Font {
    /// <summary>Font descriptor that belongs to this font type</summary>
    private readonly PdfIndirectObject_FontDescriptor pdfIndirectObject_FontDescriptor;

    //------------------------------------------------------------------------------------------04.08.2006
    /// <summary>Creates a font indirect object for an open type font.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    /// <param name="openTypeFontData">Open type font data</param>
    internal PdfIndirectObject_Font_OpenType(PdfFormatter pdfFormatter, OpenTypeFontData openTypeFontData)
      : base(pdfFormatter, openTypeFontData)
    {
      pdfIndirectObject_FontDescriptor = new PdfIndirectObject_FontDescriptor(pdfFormatter, openTypeFontData);
    }

    //------------------------------------------------------------------------------------------01.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      OpenTypeFontData openTypeFontData = (OpenTypeFontData)fontData;

      StartObj();
      Dictionary_Start();
      Dictionary_Key("Type");  Name("Font");
      Dictionary_Key("Subtype");  Name("TrueType");
      System.Diagnostics.Debug.Assert(openTypeFontData.sBaseFontName != null);
      Dictionary_Key("BaseFont");  Name(openTypeFontData.sBaseFontName);
      Dictionary_Key("Encoding");  Name("WinAnsiEncoding");
      Dictionary_Key("FontDescriptor");  IndirectReference(pdfIndirectObject_FontDescriptor);
      Int32 iFirstChar = openTypeFontData.iFirstChar;
      Dictionary_Key("FirstChar");  Number(iFirstChar);
      Int32 iLastChar = openTypeFontData.iLastChar;
      Dictionary_Key("LastChar");
      Number(iLastChar);
      Dictionary_Key("Widths");
      ArrayStart();
      for (int i = iFirstChar; i <= iLastChar; i++) {
        Space();
        Int32 iWidth = openTypeFontData.iGetRawWidth(iFirstChar + i);
        Number(iWidth);
      }
      ArrayEnd();
      Dictionary_End();
      EndObj();
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------xx.02.2006
  #region PdfIndirectObject_FontDescriptor
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF Indirect Object: Font Descriptor</summary>
  internal sealed class PdfIndirectObject_FontDescriptor : PdfIndirectObject {
    /// <summary>Font property</summary>
    private readonly OpenTypeFontData openTypeFontData;

    //------------------------------------------------------------------------------------------04.05.2006
    /// <summary>Creates a font descriptor indirect object.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    /// <param name="fontProp">Font property</param>
    internal PdfIndirectObject_FontDescriptor(PdfFormatter pdfFormatter, OpenTypeFontData openTypeFontData)
      : base(pdfFormatter)
    {
      this.openTypeFontData = openTypeFontData;
    }

//2 0 obj
//<</FontDescriptor 5 0 R
///BaseFont /PalatinoLinotype-Roman
///FirstChar 32
///Encoding /WinAnsiEncoding
///Subtype /TrueType
///Widths [250 0 0 0 0 0 0 208 0 0 0 0 0 0 250 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 708 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 612 0 0 0 0 0 0 0 0 0 0 0 0 500 0 443 0 479 333 0 582 291 0 0 291 882 582 545 601 560 395 423 326 603 0 0 0 556 0 0 0 0 0 0 500 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 277 0 0 0 0 0 0 0 0 0 0 0 0 0 333 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 500 0 0 0 500 0 0 443 0 479 0 0 0 0 0 0 0 0 0 0 0 0 545 0 0 0 0 0 603]
///LastChar 252
///Type /Font
//>>
//endobj

// 5 0 obj
// <</FontName /PalatinoLinotype-Roman
///StemV 80
// /Descent -284
// /Ascent 731
// /Flags 32
///ItalicAngle 0
// /CapHeight 699
// /FontBBox [-169 -291 1419 1049]
// /Type /FontDescriptor
// >>
// endobj

    //------------------------------------------------------------------------------------------xx.02.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      //PdfFontPropX pdfFontPropX = (PdfFontPropX)fontProp.oFontPropX;
      //Type1FontData type1FontData = (Type1FontData)pdfFontPropData.fontData;

      StartObj();
      Dictionary_Start();
      Dictionary_Key("Type");  Name("FontDescriptor");
      //Dictionary_Key("FontName");  Name(openTypeFontData.sFullFontName);
      Dictionary_Key("FontName");  Name("PalatinoLinotype-Roman");
      Int32 iFlags = 0;
      if (openTypeFontData.bFixedPitch) {
        iFlags |= 1;
      }
      //if (openTypeFontData.bFontSpecific) {
      //  iFlags |= 4;
      //}
      //else {
      //  iFlags |= 32;
      //}
      //if (openTypeFontData.rItalicAngle < 0) {
      //  iFlags |= 64;
      //}
      //  iFlags |= 131072;
      //if (sWeight.Equals("Bold")) {
      //  iFlags |= 262144;
      //}
      iFlags = 32;  // !!!
      Dictionary_Key("Flags");  Number(iFlags);
      Dictionary_Key("Ascent");  Number(/*openTypeFontData.fAscender*/731);
      Dictionary_Key("CapHeight");  Number(/*openTypeFontData.fCapHeight*/699);
      Dictionary_Key("Descent");  Number(/*openTypeFontData.fDescender*/-284);
      Dictionary_Key("FontBBox");
      ArrayStart();
      Number(/*openTypeFontData.fFontBBox_llx*/-169);
      Space();
        Number(/*openTypeFontData.fFontBBox_lly*/-291);  Space();
        Number(/*openTypeFontData.fFontBBox_urx*/1419);  Space();
        Number(/*openTypeFontData.fFontBBox_ury*/1049);
      ArrayEnd();
      Dictionary_Key("ItalicAngle");  Number(openTypeFontData.rItalicAngle);
      Dictionary_Key("StemV");  Number(/*openTypeFontData.fStdVW*/80);
      // FontFile
      Dictionary_End();
      EndObj();
    }
  }
  #endregion
}
