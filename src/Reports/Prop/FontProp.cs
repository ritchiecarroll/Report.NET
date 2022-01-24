using System;
using System.Drawing;
using System.Globalization;

// Creation date: 24.04.2002
// Checked: 26.09.2006
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
  /// <summary>Defines the properties (i.e. format and style attributes) of a font.</summary>
  /// <remarks>
  /// Before a text object (e.g. <see cref="Root.Reports.RepString"/>) can be created,
  /// a <see cref="Root.Reports.FontDef"/> and a <see cref="Root.Reports.FontProp"/> object must be defined.
  /// </remarks>
  // <include file='Prop\FontProp.cs.xml' path='documentation/class[@name="FontProp"]/*'/>
  public class FontProp {
    //------------------------------------------------------------------------------------------28.07.2006
    #region Constructor
    //----------------------------------------------------------------------------------------------------

    /// <summary>Style of the font</summary>
    private FontStyle fontStyle = FontStyle.Regular;

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Creates a new font property object with the specified size and color.</summary>
    /// <param name="fontDef">Font definition</param>
    /// <param name="rSize">Size of the font in 1/72 inches, height of the letter "H".</param>
    /// <param name="color">Color of the font</param>
    /// <remarks>
    /// After a FontProp object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in 1/72 inches, height of the letter "H".
    /// </remarks>
    /// <include file='Prop\FontProp.cs.xml' path='documentation/class[@name="FontProp.FontProp"]/*'/>
    public FontProp(FontDef fontDef, Double rSize, Color color) {
      this.fontDef = fontDef;
      this._rSizeInternal = rSize;
      this.color = color;
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Creates a new font property object with the specified size.</summary>
    /// <param name="fontDef">Font definition</param>
    /// <param name="rSize">Size of the font in 1/72 inches, height of the letter "H".</param>
    /// <remarks>
    /// The default color of the font is black.
    /// After a FontProp object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in 1/72 inches, height of the letter "H".
    /// </remarks>
    /// <include file='Prop\FontProp.cs.xml' path='documentation/class[@name="FontProp.FontProp1"]/*'/>
    public FontProp(FontDef fontDef, Double rSize)
      : this(fontDef, rSize, Color.Black) {
    }
    #endregion

    //------------------------------------------------------------------------------------------28.07.2006
    #region Properties
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Gets <b>true</b> if the font property object is registered.</summary>
    private Boolean bRegistered {
      get { return Object.ReferenceEquals(_fontProp_Registered, this); } 
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Gets or sets the bold style of the font.</summary>
    /// <value><b>true</b> if the font is bold; otherwise <b>false</b></value>
    /// <remarks>This property can be used to change the bold style of the font.</remarks>
    /// <include file='Prop\FontProp.cs.xml' path='documentation/class[@name="FontProp.bBold"]/*'/>
    public Boolean bBold {
      get { return (fontStyle & FontStyle.Bold) > 0; } 
      set {
        ResetRegisteredFontAndFontData();
        if (value) {
          fontStyle |= FontStyle.Bold;
        }
        else {
          fontStyle &= ~FontStyle.Bold;
        }
      }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Gets or sets the italic style of the font.</summary>
    /// <value><b>true</b> if the font is italic; otherwise <b>false</b></value>
    /// <remarks>This property can be used to change the italic style of the font.</remarks>
    /// <include file='Prop\FontProp.cs.xml' path='documentation/class[@name="FontProp.bBold"]/*'/>
    public Boolean bItalic {
      get { return (fontStyle & FontStyle.Italic) > 0; } 
      set {
        ResetRegisteredFontAndFontData();
        if (value) {
          fontStyle |= FontStyle.Italic;
        }
        else {
          fontStyle &= ~FontStyle.Italic;
        }
      }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Gets or sets the underline attribute of the font.</summary>
    /// <value><b>true</b> if the font is underlined; otherwise <b>false</b></value>
    /// <remarks>This property can be used to change the underline attribute of the font.</remarks>
    /// <include file='Prop\FontProp.cs.xml' path='documentation/class[@name="FontProp.bUnderline"]/*'/>
    public Boolean bUnderline {
      get { return (fontStyle & FontStyle.Underline) > 0; }
      set {
        ResetRegisteredFont();
        if (value) {
          fontStyle |= FontStyle.Underline;
        }
        else {
          fontStyle &= ~FontStyle.Underline;
        }
      }
    }

    //------------------------------------------------------------------------------------------02.05.2006
    private Color _color;
    /// <summary>Gets or sets the color of the font.</summary>
    /// <value>Color of the font</value>
    /// <remarks>This property can be used to set the color of the font.</remarks>
    /// <include file='Prop\FontProp.cs.xml' path='documentation/class[@name="FontProp.color"]/*'/>
    public Color color {
      get { return _color; } 
      set {
        ResetRegisteredFont();
        _color = value;
      }
    }

    //------------------------------------------------------------------------------------------02.05.2006
    private FontDef _fontDef;
    /// <summary>Gets or sets the font definition object.</summary>
    /// <value>Font definition object of the font</value>
    /// <remarks>This property can be used to set the font definition object of this font property object.</remarks>
    public FontDef fontDef {
      get { return _fontDef; } 
      set {
        ResetRegisteredFontAndFontData();
        _fontDef = value;
      }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    private FontData _fontData;
    /// <summary>Gets the font data object of this font property.</summary>
    internal FontData fontData {
      get {
        if (_fontData == null) {
          _fontData = fontDef.aFontData[fontStyle & (FontStyle.Bold | FontStyle.Italic)];
        }
        return _fontData;
      }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    private FontProp _fontProp_Registered;
    /// <summary>Gets a reference to the font property object with the same attributes that is registered.</summary>
    /// <remarks>
    /// A text object (e.g. RepString) must reference a registered font property object.
    /// Registered font property objects cannot be changed, they can only be accessed by the internal system.
    /// If null, it has not yet been used and therefore it is not registered.
    /// </remarks>
    internal FontProp fontProp_Registered {
      get {
        if (_fontProp_Registered == null) {
          String sKey = _fontDef.sFontName + ";" + rSizePoint.ToString("0.###", CultureInfo.InvariantCulture) + ";"
            + _color.R + "-" + _color.G + "-" + _color.B + "-" + _color.A + ";"
            + fontStyle.ToString("d") + ";" + _rAngle.ToString("0.###", CultureInfo.InvariantCulture) + ";"
            + rLineFeed.ToString("0.###", CultureInfo.InvariantCulture);  // _rLineFeed could be NaN
          #if Framework2
          if (!report.dict_FontProp.TryGetValue(sKey, out _fontProp_Registered)) {
          #else
          _fontProp_Registered = (FontProp)report.dict_FontProp[sKey];
          if (_fontProp_Registered == null) {
          #endif
            _fontProp_Registered = new FontPropPoint(_fontDef, rSizePoint, _color);
            _fontProp_Registered.fontStyle = fontStyle;
            _fontProp_Registered._rAngle = _rAngle;
            _fontProp_Registered._rLineFeed = _rLineFeed;
            _fontProp_Registered._fontProp_Registered = _fontProp_Registered;
            report.dict_FontProp.Add(sKey, _fontProp_Registered);
          }
        }
        return _fontProp_Registered;
      }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    private Double _rAngle;
    /// <summary>Gets or sets the angle of the font.</summary>
    /// <value>Angle in degrees</value>
    /// <remarks>The text will be rotated clockwise and relative to the parent container.</remarks>
    public Double rAngle {
      get { return _rAngle; }
      set {
        ResetRegisteredFont();
        _rAngle = value;
      }
    }

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Gets the report object to which this font property belongs.</summary>
    /// <value>Report object</value>
    /// <remarks>A font property is only valid for one report.</remarks>
    public Report report {
      get { return fontData.report; }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    private Double _rLineFeed = Double.NaN;
    /// <summary>Gets the height of the line feed in points (1/72 inch).</summary>
    /// <value>Height of the line feed in points (1/72 inch)</value>
    /// <remarks>This property can be used to change the height for the line feed. The default height of the line feed is rSize * 2.</remarks>
    /// <example>Line feed sample in <see cref="FlowLayoutManager"/></example>
    public Double rLineFeed {
      get {
        if (Double.IsNaN(_rLineFeed)) {
          _rLineFeed = rSize * 2.0;
        }
        return _rLineFeed;
      }
      set {
        ResetRegisteredFont();
        _rLineFeed = value;
      }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Gets the height of the line feed in millimeters.</summary>
    /// <value>Height of the line feed in millimeters</value>
    /// <remarks>This property can be used to change the height for the line feed. The default height of the line feed is rSize * 2.</remarks>
    /// <example>Line feed sample in <see cref="FlowLayoutManager"/></example>
    public Double rLineFeedMM {
      get { return RT.rMMFromPoint(rLineFeed); }
      set { rLineFeed = RT.rPointFromMM(value); }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Size of the font</summary>
    /// <remarks>
    /// for a FontProp or FontPropMM object: size of the letter "H" in points (1/72 inch)
    /// for a FontPropPoint object: size of the font in points
    /// </remarks>
    internal Double _rSizeInternal;

    /// <summary>Gets or sets the size of the font in 1/72 inches, height of the letter "H".</summary>
    /// <value>Size of the font in 1/72 inches, height of the letter "H"</value>
    /// <remarks>This property can be used to set the size of the font.</remarks>
    /// <example>Font size sample:
    /// <code>
    /// using Root.Reports;
    /// using System;
    ///
    /// public class FontPropSample : Report {
    ///   public static void Main() {
    ///     PdfReport&lt;FontPropSample&gt; pdfReport = new PdfReport&lt;FontPropSample&gt;();
    ///     pdfReport.View("FontPropSample.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///     FontProp fontProp = new FontProp(fontDef, 45);
    ///     new Page(this);
    ///     page_Cur.AddCB_MM(80, new RepString(fontProp, "FontProp Sample"));
    ///     <b>fontProp.rSize = 30</b>;
    ///     page_Cur.AddCB_MM(110, new RepString(fontProp, "smaller font"));
    ///   }
    /// }
    /// </code>
    /// </example>
    public virtual Double rSize {
      get { return _rSizeInternal; } 
      set {
        ResetRegisteredFont();
        _rSizeInternal = value;
      }
    }

    //------------------------------------------------------------------------------------------02.02.2005
    /// <summary>Gets or sets the size of the font in millimeters.</summary>
    /// <value>Size of the font in millimeters: height of the letter "H"</value>
    /// <remarks>This property can be used to set the size of the font.</remarks>
    /// <example>Font size sample in <see cref="FontProp.rSize"/></example>
    public Double rSizeMM {
      get { return RT.rMMFromPoint(rSize); }
      set { rSize = RT.rPointFromMM(value); }
    }

    //------------------------------------------------------------------------------------------02.02.2005
    /// <summary>Gets or sets the size of the font in points.</summary>
    /// <value>Size of the font in points</value>
    /// <remarks>This property can be used to set the size of the font.</remarks>
    /// <example>Font size sample in <see cref="FontProp.rSize"/></example>
    public virtual Double rSizePoint {
      get { return _rSizeInternal / fontData.rGetFactor_EM_To_H(); }
      set { _rSizeInternal = value * fontData.rGetFactor_EM_To_H(); }
    }
    #endregion

    //------------------------------------------------------------------------------------------02.05.2006
    #region Methods
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>This method will disconnect this font from the registered font.</summary>
    /// <remarks>This operation is required if properties of the font change.</remarks>
    internal void ResetRegisteredFont() {
      if (bRegistered) {
        throw new ReportException("This font property cannot be changed.");
      }
      _fontProp_Registered = null;
    }

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>This method will disconnect this font from the registered font and its font data.</summary>
    /// <remarks>This operation is required if properties of the font change.</remarks>
    private void ResetRegisteredFontAndFontData() {
      ResetRegisteredFont();
      _fontData = null;
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Returns the width of the specified text in points (1/72 inch).</summary>
    /// <param name="sText">Text</param>
    /// <returns>Width of the text in points (1/72 inch)</returns>
    /// <remarks>This method calculates the width of the specified text.</remarks>
    /// <example>Width/Height sample:
    /// <code>
    /// using Root.Reports;
    /// using System;
    /// using System.Drawing;
    ///
    /// public class FontPropSample : Report {
    ///   public static void Main() {
    ///     PdfReport&lt;FontPropSample&gt; pdfReport = new PdfReport&lt;FontPropSample&gt;();
    ///     pdfReport.View("FontPropSample.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///     FontProp fontProp = new FontPropMM(fontDef, 15);
    ///     new Page(this);
    ///     String sText = "FontProp Sample";
    ///     BrushProp brushProp = new BrushProp(this, Color.Red);
    ///     page_Cur.AddMM(25, 80, new RepRect(brushProp, <b>fontProp.rGetTextWidth(sText), fontProp.rSize</b>));
    ///     page_Cur.AddMM(25, 80, new RepString(fontProp, sText));
    ///   }
    /// }
    /// </code>
    /// </example>
    public Double rGetTextWidth(String sText) {
      return fontData.rWidth(this, sText);
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Returns the width of the specified text in millimeters.</summary>
    /// <param name="sText">Text</param>
    /// <returns>Width of the text in millimeters</returns>
    /// <remarks>This method calculates the width of the specified text.</remarks>
    /// <example>Width/Height sample in <see cref="FontProp.rGetTextWidth"/></example>
    public Double rGetTextWidthMM(String sText) {
      return RT.rMMFromPoint(rGetTextWidth(sText));
    }

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Truncates the text to the specified width in points (1/72 inch) and adds three dots if necessary.</summary>
    /// <param name="sText">Text</param>
    /// <param name="rWidthMax">Maximal width of the text in points (1/72 inch)</param>
    /// <returns>Truncated text</returns>
    /// <remarks>This method truncates a string to the specified width.</remarks>
    /// <example>Truncate text sample:
    /// <code>
    /// using Root.Reports;
    /// using System;
    ///
    /// public class FontPropSample : Report {
    ///   public static void Main() {
    ///     PdfReport&lt;FontPropSample&gt; pdfReport = new PdfReport&lt;FontPropSample&gt;();
    ///     pdfReport.View("FontPropSample.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///     FontProp fontProp = new FontPropMM(fontDef, 15);
    ///     new Page(this);
    ///     String sText = fontProp.sTruncateTextMM("FontProp Sample", 150);
    ///     page_Cur.AddCB_MM(80, new RepString(fontProp, sText));
    ///   }
    /// }
    /// </code>
    /// </example>
    public String sTruncateText(String sText, Double rWidthMax) {
      if (rWidthMax <= 0) {
        return "";
      }
      Double rFullWidth = rGetTextWidth(sText);
      if (rFullWidth <= rWidthMax) {
        return sText;
      }
      rWidthMax -= rGetTextWidth("...");
      Int32 iStart = 0;
      return sGetTextLine(sText, rWidthMax, ref iStart, TextSplitMode.Truncate) + "...";
    }

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Truncates the text to the specified width in millimeters and adds three dots if necessary.</summary>
    /// <param name="sText">Text</param>
    /// <param name="rWidthMaxMM">Maximal width of the text in millimeters</param>
    /// <returns>Truncated text</returns>
    /// <remarks>This method truncates a string to the specified width.</remarks>
    /// <example>Truncate text sample in <see cref="FontProp.sTruncateText"/></example>
    public String sTruncateTextMM(String sText, Double rWidthMaxMM) {
      return sTruncateText(sText, RT.rPointFromMM(rWidthMaxMM));
    }

    //------------------------------------------------------------------------------------------22.08.2006
    /// <summary>Gets a line of text with a maximal width from the specified string.</summary>
    /// <param name="sText">Text</param>
    /// <param name="rWidthMax">Maximal width of the text in points (1/72 inch)</param>
    /// <param name="iStart">Start position in sText</param>
    /// <param name="textSplitMode">Text split mode</param>
    /// <returns>Text with the specified maximal width</returns>
    /// <remarks>
    /// The reference parameter <paramref name="iStart"/> must be initialized with the start position.
    /// After the call, it contains the new start position that can be used for a call of this method for the next line of text.
    /// </remarks>
    /// <example>Multiline text sample:
    /// <code>
    /// using Root.Reports;
    /// using System;
    ///
    /// public class FontPropSample : Report {
    ///   public static void Main() {
    ///     PdfReport&lt;FontPropSample&gt; pdfReport = new PdfReport&lt;FontPropSample&gt;();
    ///     pdfReport.View("FontPropSample.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///     FontProp fontProp = new FontPropMM(fontDef, 15);
    ///     new Page(this);
    ///     String sText = "Once upon a time there was a miller who was poor, but he had a beautiful daughter.";
    ///     Double rY = 20;
    ///     Int32 iStart = 0;
    ///     while (iStart &lt;= sText.Length) {
    ///       String sLine = fontProp.sGetTextLineMM(sText, 170, ref iStart, TextSplitMode.Line);
    ///       page_Cur.AddMM(20, rY, new RepString(fontProp, sLine));
    ///       rY += fontProp.rLineFeedMM;
    ///     }
    ///   }
    /// }
    /// </code>
    /// </example>
    public String sGetTextLine(String sText, Double rWidthMax, ref Int32 iStart, TextSplitMode textSplitMode) {
      return fontData.sGetTextLine(sText, rWidthMax * 1000.0 / rSizePoint, ref iStart, textSplitMode);
    }

    //------------------------------------------------------------------------------------------22.08.2006
    /// <summary>Gets a line of text with a maximal width from the specified string (metric version).</summary>
    /// <param name="sText">Text</param>
    /// <param name="rWidthMaxMM">Maximal width of the text in millimeters</param>
    /// <param name="iStart">Start position in sText</param>
    /// <param name="textSplitMode">Text split mode</param>
    /// <returns>Text with the specified maximal width</returns>
    /// <remarks>
    /// The reference parameter <paramref name="iStart"/> must be initialized with the start position.
    /// After the call, it contains the new start position that can be used for a call of this method for the next line of text.
    /// </remarks>
    /// <example>Multiline text sample in <see cref="FontProp.sGetTextLine"/></example>
    public String sGetTextLineMM(String sText, Double rWidthMaxMM, ref Int32 iStart, TextSplitMode textSplitMode) {
      return sGetTextLine(sText, RT.rPointFromMM(rWidthMaxMM), ref iStart, textSplitMode);
    }

    //------------------------------------------------------------------------------------------22.08.2006
    /// <summary>Gets the best fitting font to put the text into the specified rectangle.</summary>
    /// <param name="sText">Text</param>
    /// <param name="rWidthMax">Maximal width of the text in points (1/72 inch)</param>
    /// <param name="rHeightMax">Maximal height of the text in points (1/72 inch)</param>
    /// <param name="rFontSizeMin">Minimal size of the font in 1/72 inches, height of the letter "H"</param>
    /// <returns>Best fitting font</returns>
    /// <remarks>
    /// This method will try to fit the text into the rectangle with this font properties.
    /// If the text is too large to fit into the rectangle, the font size will be reduced down to <paramref name="rFontSizeMin"/>.
    /// </remarks>
    /// <example>Multiline text sample: text fits into the rectangle
    /// <code>
    /// using Root.Reports;
    /// using System;
    ///
    /// public class FontPropSample : Report {
    ///   public static void Main() {
    ///     PdfReport&lt;FontPropSample&gt; pdfReport = new PdfReport&lt;FontPropSample&gt;();
    ///     pdfReport.View("FontPropSample.pdf");
    ///   }
    ///
    ///   protected override void Create() {
    ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
    ///     FontProp fontProp = new FontPropMM(fontDef, 15);
    ///     PenProp penProp = new PenProp(this, 1);
    ///     new Page(this);
    ///     String sText = "Once upon a time there was a miller who was poor, but he had a beautiful daughter.";
    ///     Double rY = 20;
    ///     page_Cur.AddLT_MM(20, rY, new RepRectMM(penProp, 100, 70));
    ///     FontProp fontProp_BestFit = <b>fontProp.fontProp_GetBestFitMM(sText, 100, 70, 5)</b>;
    ///     rY += fontProp_BestFit.rSizeMM;
    ///     Int32 iStart = 0;
    ///     while (iStart &lt;= sText.Length) {
    ///       String sLine = fontProp_BestFit.sGetTextLineMM(sText, 100, ref iStart, TextSplitMode.Line);
    ///       page_Cur.AddMM(20, rY, new RepString(fontProp_BestFit, sLine));
    ///       rY += fontProp_BestFit.rLineFeedMM;
    ///     }
    ///   }
    /// }
    /// </code>
    /// </example>
    public FontProp fontProp_GetBestFit(String sText, Double rWidthMax, Double rHeightMax, Double rFontSizeMin) {
      if (sText == null) {
        throw new ArgumentNullException("sText", "text is required");
      }
      Double rFontSizeMax = rSize;
      FontProp fontProp_Test = new FontProp(fontDef, rSize);
      fontProp_Test.bBold = bBold;

      while (true) {
        Int32 iStart = 0;
        Double rHeight = 0;
        while (true) {
          fontProp_Test.sGetTextLine(sText, rWidthMax, ref iStart, TextSplitMode.Line);
          rHeight += fontProp_Test.rLineFeed;
          if (rHeight > rHeightMax) {
            rFontSizeMax = fontProp_Test.rSize;
            break;
          }
          if (iStart > sText.Length) {
            rFontSizeMin = fontProp_Test.rSize;
            break;
          }
        }
        if (rFontSizeMax - rFontSizeMin < 0.1) {
          break;
        }
        fontProp_Test.rSize = (rFontSizeMax + rFontSizeMin) / 2.0;
        fontProp_Test.rLineFeed = Double.NaN;
      }
      return fontProp_Test;
    }

    //------------------------------------------------------------------------------------------22.08.2006
    /// <summary>Gets the best fitting font to put the text into the specified rectangle (metric version).</summary>
    /// <param name="sText">Text</param>
    /// <param name="rWidthMaxMM">Maximal width of the text in millimeters</param>
    /// <param name="rHeightMaxMM">Maximal height of the text in millimeters</param>
    /// <param name="rFontSizeMinMM">Minimal size of the font in millimeters, height of the letter "H"</param>
    /// <returns>Best fitting font</returns>
    /// <remarks>
    /// This method will try to fit the text into the rectangle with this font properties.
    /// If the text is too large to fit into the rectangle, the font size will be reduced down to <paramref name="rFontSizeMin"/>.
    /// </remarks>
    /// <example>Multiline text sample in <see cref="FontProp.fontProp_GetBestFit"/></example>
    public FontProp fontProp_GetBestFitMM(String sText, Double rWidthMaxMM, Double rHeightMaxMM, Double rFontSizeMinMM) {
      return fontProp_GetBestFit(sText, RT.rPointFromMM(rWidthMaxMM), RT.rPointFromMM(rHeightMaxMM), RT.rPointFromMM(rFontSizeMinMM));
    }
    #endregion

    //------------------------------------------------------------------------------------------28.07.2006
    #if Compatible_0_8
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Returns the height of the font in points (1/72 inch).</summary>
    /// <returns>Height of the font in points (1/72 inch)</returns>
    /// <example><see cref="FontProp.rGetTextWidth">Width/Height sample</see></example>
    /// <remarks>This method returns the height of the letter "H".</remarks>
    [Obsolete("use property [rSize]")]
    public Double rHeight() {
      return fontData.rHeight(fontProp_Registered);
    }

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Returns the height of the font in millimeters.</summary>
    /// <returns>Height of the font in millimeters</returns>
    /// <example><see cref="FontProp.rGetTextWidth">Width/Height sample</see></example>
    /// <remarks>This method returns the height of the letter "H".</remarks>
    [Obsolete("use property [rSizeMM]")]
    public Double rHeightMM() {
      return RT.rMMFromPoint(rHeight());
    }

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Returns the width of the specified text in points (1/72 inch).</summary>
    /// <param name="sText">Text</param>
    /// <returns>Width of the text in points (1/72 inch)</returns>
    /// <remarks>This method calculates the width of the specified text.</remarks>
    /// <example><see cref="FontProp.rGetTextWidth">Width/Height sample</see></example>
    [Obsolete("use method [rGetTextWidth]")]
    public Double rWidth(String sText) {
      return fontData.rWidth(this, sText);
    }

    //------------------------------------------------------------------------------------------02.05.2006
    /// <summary>Returns the width of the specified text in millimeters.</summary>
    /// <param name="sText">Text</param>
    /// <returns>Width of the text in millimeters</returns>
    /// <remarks>This method calculates the width of the specified text.</remarks>
    /// <example><see cref="FontProp.rGetTextWidth">Width/Height sample</see></example>
    [Obsolete("use method [rGetTextWidthMM]")]
    public Double rWidthMM(String sText) {
      return RT.rMMFromPoint(rWidth(sText));
    }
    #endif
  }

  //------------------------------------------------------------------------------------------28.07.2006
  #region FontPropMM
  //----------------------------------------------------------------------------------------------------

  #region
  /// <summary>Defines the properties (i.e. format and style attributes) of a font with metric values.</summary>
  /// <remarks>
  /// Before a text object (e.g. <see cref="Root.Reports.RepString"/>) can be created,
  /// a <see cref="Root.Reports.FontDef"/> and a <see cref="Root.Reports.FontProp"/> object must be defined.
  /// </remarks>
  /// <example>Font property sample:
  /// <code>
  /// using Root.Reports;
  /// using System;
  /// using System.Drawing;
  /// 
  /// public class FontPropSample : Report {
  ///   public static void Main() {
  ///     PdfReport&lt;FontPropSample&gt; pdfReport = new PdfReport&lt;FontPropSample&gt;();
  ///     pdfReport.View("FontPropSample.pdf");
  ///   }
  ///
  ///   protected override void Create() {
  ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
  ///     <b>FontProp fontProp = new FontPropMM(fontDef, 15, Color.Red)</b>;
  ///     fontProp.bBold = true;
  ///     fontProp.bItalic = true;
  ///     fontProp.bUnderline = true;
  ///     new Page(this);
  ///     page_Cur.AddCB_MM(80, new RepString(<b>fontProp</b>, "FontProp Sample"));
  ///   }
  /// }
  /// </code>
  /// </example>
  #endregion
  public class FontPropMM : FontProp {
    //------------------------------------------------------------------------------------------28.07.2006
    /// <overloads>
    /// <summary>Creates a new font property object with metric values.</summary>
    /// <remarks>
    /// After a FontPropMM object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in millimeters, height of the letter "H".
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a new font property object with the specified size (in millimeters) and color.</summary>
    /// <param name="fontDef">Font definition</param>
    /// <param name="rSizeMM">Size of the font in millimeters, height of the letter "H".</param>
    /// <param name="color">Color of the font</param>
    /// <remarks>
    /// After a FontPropMM object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in millimeters, height of the letter "H".
    /// </remarks>
    /// <example><see cref="FontPropMM">FontPropMM constructor sample</see></example>
    public FontPropMM(FontDef fontDef, Double rSizeMM, Color color)
      : base(fontDef, RT.rPointFromMM(rSizeMM), color) {
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Creates a new font property object with the specified size (in millimeters).</summary>
    /// <param name="fontDef">Font definition</param>
    /// <param name="rSizeMM">Size of the font in millimeters, height of the letter "H".</param>
    /// <remarks>
    /// The default color of the font is black.
    /// After a FontProp object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in millimeters, height of the letter "H".
    /// </remarks>
    /// <example><see cref="FontPropMM">FontPropMM constructor sample</see></example>
    public FontPropMM(FontDef fontDef, Double rSizeMM)
      : this(fontDef, rSizeMM, Color.Black) {
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------28.07.2006
  #region FontPropPoint
  //----------------------------------------------------------------------------------------------------

  #region
  /// <summary>Defines the properties (i.e. format and style attributes) of a font that is defined in points.</summary>
  /// <remarks>
  /// Before a text object (e.g. <see cref="Root.Reports.RepString"/>) can be created,
  /// a <see cref="Root.Reports.FontDef"/> and a <see cref="Root.Reports.FontProp"/> object must be defined.
  /// </remarks>
  /// <example>Font property sample:
  /// <code>
  /// using Root.Reports;
  /// using System;
  /// using System.Drawing;
  /// 
  /// public class FontPropSample : Report {
  ///   public static void Main() {
  ///     PdfReport&lt;FontPropSample&gt; pdfReport = new PdfReport&lt;FontPropSample&gt;();
  ///     pdfReport.View("FontPropSample.pdf");
  ///   }
  ///
  ///   protected override void Create() {
  ///     FontDef fontDef = new FontDef(this, FontDef.StandardFont.Helvetica);
  ///     <b>FontProp fontProp = new FontPropPoint(fontDef, 50, Color.Red)</b>;
  ///     fontProp.bBold = true;
  ///     fontProp.bItalic = true;
  ///     fontProp.bUnderline = true;
  ///     new Page(this);
  ///     page_Cur.AddCB_MM(80, new RepString(<b>fontProp</b>, "FontProp Sample"));
  ///   }
  /// }
  /// </code>
  /// </example>
  #endregion
  public class FontPropPoint : FontProp {
    //------------------------------------------------------------------------------------------28.07.2006
    #region Constructor
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------28.07.2006
    /// <overloads>
    /// <summary>Creates a new font property object. The size is specified in points.</summary>
    /// <remarks>
    /// After a FontProp object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in points.
    /// </remarks>
    /// </overloads>
    /// 
    /// <summary>Creates a new font property object with the specified size (in points) and color.</summary>
    /// <param name="fontDef">Font definition</param>
    /// <param name="rSizePoint">Size of the font in points.</param>
    /// <param name="color">Color of the font</param>
    /// <remarks>
    /// After a FontProp object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in points.
    /// </remarks>
    /// <example><see cref="FontPropPoint">FontPropPoint constructor sample</see></example>
    public FontPropPoint(FontDef fontDef, Double rSizePoint, Color color)
      : base(fontDef, rSizePoint, color) {
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Creates a new font property object with the specified size (in points).</summary>
    /// <param name="fontDef">Font definition</param>
    /// <param name="rSizePoint">Size of the font in points.</param>
    /// <remarks>
    /// The default color of the font is black.
    /// After a FontProp object has been created, the format and style attributes of the object can be changed.
    /// The size of the font can be specified in points.
    /// </remarks>
    /// <example><see cref="FontPropPoint">FontPropPoint constructor sample</see></example>
    public FontPropPoint(FontDef fontDef, Double rSizePoint)
      : this(fontDef, rSizePoint, Color.Black) {
    }
    #endregion

    //------------------------------------------------------------------------------------------28.07.2006
    #region Properties
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Gets or sets the size of the font in 1/72 inches, height of the letter "H".</summary>
    /// <value>Size of the font in 1/72 inches, height of the letter "H"</value>
    /// <remarks>This property can be used to set the size of the font.</remarks>
    /// <example><see cref="FontProp.rSize">Font size sample</see></example>
    public override Double rSize {
      get { return _rSizeInternal * fontData.rGetFactor_EM_To_H(); }
      set {
        _rSizeInternal = value / fontData.rGetFactor_EM_To_H();
        ResetRegisteredFont();
      }
    }

    //------------------------------------------------------------------------------------------28.07.2006
    /// <summary>Gets or sets the size of the font in points.</summary>
    /// <value>Size of the font in points</value>
    /// <remarks>This property can be used to set the size of the font.</remarks>
    /// <example><see cref="FontProp.rSize">Font size sample</see></example>
    public override Double rSizePoint {
      get { return _rSizeInternal; }
      set { 
        _rSizeInternal = value;
        ResetRegisteredFont();
      }
    }
    #endregion
  }
  #endregion
}
