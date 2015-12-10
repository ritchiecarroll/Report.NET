using System;
using System.Diagnostics;
using System.Drawing;

// Creation date: 24.04.2002
// Checked: 06.03.2005
// Author: Otto Mayer (mot@root.ch)
// Version: 1.03

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  #region Class Documentation
  /// <summary>Font Definition</summary>
  /// <remarks>Each font family must be registered before it can be used. It can be registered only once.</remarks>
  /// <example>Font definition sample:
  /// <code>
  /// class HelloWorld {
  ///   public static void Main() {
  ///     Report report = new Report(new PdfFormatter());
  ///     <b>FontDef fd = new FontDef(report, FontDef.StandardFont.Helvetica)</b>;
  ///     FontProp fp = new FontPropMM(fd, 25);
  ///     Page page = new Page(report);
  ///     page.AddCenteredMM(80, new RepString(fp, "Hello World!"));
  ///     RT.ViewPDF(report, "HelloWorld.pdf");
  ///   }
  /// }
  /// </code>
  /// </example>
  #endregion
  public sealed class FontDef {
    //------------------------------------------------------------------------------------------xx.03.2006
    #region Static
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <overloads>
    /// <summary>Gets a font definition.</summary>
    /// <remarks>
    /// If the font is already registered, the handle of the registered font definition will be returned.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Gets a font definition for the specified font name.</summary>
    /// <param name="report">Report to which this font belongs</param>
    /// <param name="sFontName">Name of the font family</param>
    /// <returns>Font definition for the specified font family</returns>
    /// <remarks>
    /// If the font is already registered, the handle of the registered font definition will be returned.
    /// </remarks>
    public static FontDef fontDef_FromName(Report report, String sFontName) {
      FontDef fontDef = fontDef_Check(report, sFontName, FontType.InternalAFM);
      if (fontDef != null) {
        return fontDef;
      }
      return new FontDef(report, sFontName);
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Gets a font definition for the specified standard font.</summary>
    /// <param name="report">Report to which this font belongs</param>
    /// <param name="standardFont">Standard font enumeration value</param>
    /// <returns>Font definition for the specified standard font</returns>
    /// <remarks>
    /// If the font is already registered, the handle of the registered font definition will be returned.
    /// </remarks>
    public static FontDef fontDef_FromName(Report report, StandardFont standardFont) {
      String sFontName = sGetFontName(report, standardFont);
      FontDef fontDef = fontDef_Check(report, sFontName, FontType.InternalAFM);
      if (fontDef != null) {
        return fontDef;
      }
      return new FontDef(report, standardFont);
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Gets a font definition for the specified standard font.</summary>
    /// <param name="report">Report to which this font belongs</param>
    /// <param name="sFontName">TTF font name</param>
    /// <returns>Font definition for the specified standard font</returns>
    /// <remarks>
    /// If the font is already registered, the handle of the registered font definition will be returned.
    /// </remarks>
    public static FontDef fontDef_FromTTF(Report report, String sFontName) {
      FontDef fontDef = fontDef_Check(report, sFontName, FontType.TTF);
      if (fontDef != null) {
        return fontDef;
      }
      return new FontDef(report, sFontName, FontType.TTF);
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Gets a font definition for the specified standard font.</summary>
    /// <param name="report">Report to which this font belongs</param>
    /// <param name="sFontName">Standard font enumeration value</param>
    /// <param name="fontType"></param>
    /// <returns>Font definition for the specified standard font</returns>
    /// <remarks>
    /// If the font is already registered, the handle of the registered font definition will be returned.
    /// </remarks>
    private static FontDef fontDef_Check(Report report, String sFontName, FontType fontType) {
      if (sFontName == null) {
        throw new ArgumentNullException("sFontName", "font name must be specified");
      }
      #if Framework2
      FontDef fontDef;
      if (report.dict_FontDef.TryGetValue(sFontName, out fontDef)) {
        if (fontDef.fontType != fontType) {
          throw new ReportException(String.Format("Font '{0}' has been defined as '{1}' font.", sFontName, fontDef.fontType.ToString("G")));
        }
        return fontDef;
      }
      #else
      FontDef fontDef = (FontDef)report.dict_FontDef[sFontName];
      if (fontDef != null) {
        if (fontDef.fontType != fontType) {
          throw new ReportException(String.Format("Font '{0}' has been defined as '{1}' font.", sFontName, fontDef.fontType.ToString("G")));
        }
        return fontDef;
      }
      #endif
      return null;
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Gets the font name.</summary>
    /// <param name="report">Report to which this font definition belongs</param>
    /// <param name="standardFont">Standard font enumeration value</param>
    /// <returns>Name of the font family</returns>
    private static String sGetFontName(Report report, StandardFont standardFont) {
      if (report.formatter is PdfFormatter) {
        if (standardFont == FontDef.StandardFont.TimesRoman) {
          return "Times-Roman";
        }
        return standardFont.ToString("G");
      }
      switch (standardFont) {
        case FontDef.StandardFont.Courier: { return "Courier New"; }
        case FontDef.StandardFont.Helvetica: { return "Arial"; }
        case FontDef.StandardFont.Symbol: { return "Symbol"; }
        case FontDef.StandardFont.TimesRoman: { return "Times New Roman"; }
        case FontDef.StandardFont.ZapfDingbats: { return "Wingdings"; }
      }
      Debug.Fail("unknown standard font");
      return null;
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Gets the font family.</summary>
    /// <param name="report">Report to which this font definition belongs</param>
    /// <param name="standardFont">Standard font enumeration value</param>
    /// <returns>Font family object</returns>
    private static FontType fontType_Get(Report report, StandardFont standardFont) {
      if (report.formatter is PdfFormatter) {
        return FontType.InternalAFM;
      }
      return FontType.TTF;
    }
    #endregion

    //------------------------------------------------------------------------------------------xx.03.2006
    #region FontDef
    //----------------------------------------------------------------------------------------------------

    /// <summary>Report to which this font definition belongs.</summary>
    /// <remarks>A font definition must be assigned to a report.</remarks>
    public readonly Report report;

    /// <summary>Name of the font family</summary>
    /// <remarks>Unique name of the font family. A font can be registered only once.</remarks>
    public readonly String sFontName;

    /// <summary>Font family object for true type fonts</summary>
    private readonly FontType fontType;

    internal Object oFontDefX = null;

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Creates a new font definition.</summary>
    /// <param name="report">Report to which this font belongs</param>
    /// <param name="sFontName">Name of the font family</param>
    /// <param name="fontType">Font type</param>
    private FontDef(Report report, String sFontName, FontType fontType) {
      this.report = report;
      this.sFontName = sFontName;
      this.fontType = fontType;
      aFontData = new AFontData(this);

      if (report.dict_FontDef.ContainsKey(sFontName)) {
        throw new ReportException("Font '" + sFontName + "' is already registered");
      }
      report.dict_FontDef.Add(sFontName, this);
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <overloads>
    /// <summary>Creates a new font definition.</summary>
    /// <remarks>
    /// A font family can be defined only once.
    /// You should use <see cref="M:Root.Reports.FontDef.fontDef_FromName(Root.Reports.Report,String)"/>
    /// if you are not shure whether the font is already registered.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a new font definition for the specified font name.</summary>
    /// <param name="report">Report to which this font belongs</param>
    /// <param name="sFontName">Name of the font family</param>
    /// <remarks>
    /// A font family can be defined only once.
    /// You should use <see cref="M:Root.Reports.FontDef.fontDef_FromName(Root.Reports.Report,System.String)"/>
    /// if you are not shure whether the font is already registered.
    /// </remarks>
    /// <exception cref="ReportException">Font family is already registered.</exception>
    public FontDef(Report report, String sFontName) : this(report, sFontName, FontType.InternalAFM) {
      if (!(report.formatter is PdfFormatter)) {
        throw new ReportException("for 'PdfFormatter' only");
      }
      if (sFontName == "Arial") {
        sFontName = "Helvetica";
      }
      if (!"courier,helvetica,times-roman,symbol,zapfdingbats".Contains(sFontName.ToLower())) {
        throw new ReportException("this font is not a standard PDF font");
      }
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Creates a new font definition for the specified standard font.</summary>
    /// <param name="report">Report to which this font definition belongs</param>
    /// <param name="standardFont">Standard font enumeration value</param>
    /// <remarks>
    /// A font family can be defined only once.
    /// You should use <see cref="M:Root.Reports.FontDef.fontDef_FromName(Root.Reports.Report,Root.Reports.FontDef.StandardFont)"/>
    /// if you are not shure whether the font is already registered.
    /// </remarks>
    /// <exception cref="ReportException">Font family is already registered.</exception>
    public FontDef(Report report, StandardFont standardFont)
      : this(report, sGetFontName(report, standardFont), fontType_Get(report, standardFont))
    {
    }
    #endregion

    //------------------------------------------------------------------------------------------06.03.2005
    #region AFontData
    //----------------------------------------------------------------------------------------------------

    /// <summary>Array of Font Data Objects</summary>
    /// <remarks>
    /// This class provides an indexer for the font data objects
    /// (see <see cref="F:Root.Reports.FontDef.asFontData">FontDef.aFontData</see>).
    /// </remarks>
    internal sealed class AFontData {
      /// <summary>Font definition object</summary>
      private readonly FontDef fontDef;

      /// <summary>Array that contains the font data objects of the font definition.</summary>
      /// <remarks>This variable can be used to get the font data object of a font definition.</remarks>
      /// <example>
      /// <code>FontData fontData = fontDef.aFontData[FontDef.Style.Bold];</code>
      /// </example>
      private FontData[] aFontData = new FontData[4];

      //------------------------------------------------------------------------------------------06.03.2005
      /// <summary>Create the font data array.</summary>
      /// <param name="fontDef">Font definition object</param>
      internal AFontData(FontDef fontDef) {
        this.fontDef = fontDef;
      }

      //------------------------------------------------------------------------------------------06.03.2005
      /// <summary>Gets the font data object that is identified by the specified style.</summary>
      /// <param name="fontStyle">Style value that identifies the font data object</param>
      /// <value>Font data object that is identified by the specified style</value>
      /// <remarks>If there is no font data object with the specified style, <see langword="null"/> will be returned.</remarks>
      internal FontData this[FontStyle fontStyle] {
        get {
          Int32 i = (Int32)fontStyle;
          FontData fontData = aFontData[i];
          if (fontData == null) {
            if (fontDef.fontType == FontType.InternalAFM) {
              fontData = new Type1FontData(fontDef, fontDef.sFontName, fontStyle);
            }
            else {
              fontData = new OpenTypeFontData(fontDef, fontStyle);
            }
            aFontData[i] = fontData;
          }
          return fontData;
        }
      }
    }

    //------------------------------------------------------------------------------------------xx.03.2006
    /// <summary>Array that contains the font data objects for each style.</summary>
    internal readonly AFontData aFontData;
    #endregion

    //------------------------------------------------------------------------------------------xx.03.2006
    #region FontType
    //----------------------------------------------------------------------------------------------------

    /// <summary>Font type</summary>
    private enum FontType {
      /// <summary>Standard style</summary>
      InternalAFM,
      /// <summary>Bold style</summary>
      TTF
    }
    #endregion

    //------------------------------------------------------------------------------------------xx.03.2006
    #region StandardFont
    //----------------------------------------------------------------------------------------------------

    /// <summary>Predefined standard fonts</summary>
    /// <remarks>
    /// The standard fonts are supported by the viewer and must not be embedded in the PDF file.
    /// <seealso cref="Root.Reports.FontDef"/>
    /// </remarks>
    /// <example>Definition of a standard font:
    /// <code>
    ///   FontDef fd = new FontDef(report, <b>FontDef.StandardFont.Helvetica</b>);
    /// </code>
    /// </example>
    public enum StandardFont {
      /// <summary>Standard base 14 type 1 font "Courier"</summary>
      Courier,
      /// <summary>Standard base 14 type 1 font "Helvetica"</summary>
      Helvetica,
      /// <summary>Standard base 14 type 1 font "Symbol"</summary>
      Symbol,
      /// <summary>Standard base 14 type 1 font "Times-Roman"</summary>
      TimesRoman,
      /// <summary>Standard base 14 type 1 font "ZapfDingbats"</summary>
      ZapfDingbats
    }
    #endregion
  }
}
