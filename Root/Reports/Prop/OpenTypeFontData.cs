using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
 
// Creation date: 11.10.2002
// Checked: xx.02.2005
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
  /// <summary>Type 1 Font Data</summary>
  /// <remarks>
  /// This class is based on the "Adobe Font Metrics File Format Specification"
  /// <see href="http://partners.adobe.com/asn/developer/pdfs/tn/5004.AFM_Spec.pdf"/>
  /// </remarks>
  internal class OpenTypeFontData : FontData {
    //------------------------------------------------------------------------------------------16.02.2005
    #region Constructor
    //----------------------------------------------------------------------------------------------------

    /// <summary>Font definition</summary>
    private readonly FontDef fontDef;

    //------------------------------------------------------------------------------------------16.02.2005
    /// <summary>Token delimiters</summary>
    private static readonly Char[] acDelimiterToken = {' ', '\t'};

    /// <summary>Creates the type 1 font data object.</summary>
    /// <param name="stream">AFM definition stream</param>
    /// <param name="style">Font style</param>
    /// <param name="bEmbedded"><see langword="true"/> if the font must be embedded in the PDF file</param>
    internal OpenTypeFontData(FontDef fontDef, FontStyle fontStyle)
      : base(fontDef, fontStyle, FontData.Encoding.Cp1252)
    {
      this.fontDef = fontDef;

      RegistryKey registryKey_CurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
      if (registryKey_CurrentVersion == null) {
        throw new ReportException("Cannot find the directory of the font files");
      }
      String sSystemRoot = (String)registryKey_CurrentVersion.GetValue("SystemRoot");

      RegistryKey registryKey_Font = registryKey_CurrentVersion.OpenSubKey(@"Fonts");
      if (registryKey_Font == null) {
        throw new ReportException("Cannot find the definition of the font");
      }
      String sFontFileName = (String)registryKey_Font.GetValue(fontDef.sFontName + " (TrueType)");
      String s = sFontFileName.ToLower();
      if (s.EndsWith(".ttc")) {
        bTrueTypeCollection = true;
      }
      else if (!(s.EndsWith(".ttf") || s.EndsWith(".otf"))) {
        throw new ReportException("'" + fontDef.sFontName + "' is not a TTF, OTF or TTC font.");
      }
      sFontFileName = Path.Combine(sSystemRoot, @"Fonts\" + sFontFileName);

      using (OpenTypeReader openTypeReader = new OpenTypeReader(sFontFileName)) {
        ReadFontDataFromFile(openTypeReader);
      }
    }
    #endregion

    //------------------------------------------------------------------------------------------16.02.2005
    #region Font Information
    //----------------------------------------------------------------------------------------------------

    /// <summary>Font metrics version</summary>
    /// <example><code>StartFontMetrics 4.1</code></example>
    internal readonly String sFontMetricsVersion;

    /// <summary>Metric sets</summary>
    /// <example><code>MetricsSets 0</code></example>
    internal readonly Int32 iMetricsSets = 0;

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Global Font Information

    /// <summary>Font name</summary>
    /// <example><code>FontName Times-Roman</code></example>
    internal readonly String sFontName;

    /// <summary>Full name</summary>
    /// <example><code>FullName Times Roman</code></example>
    internal String sFullFontName;

    /// <summary>Base font name</summary>
    /// <example><code>FamilyName PalatinoLinotype-Roman</code></example>
    internal String sBaseFontName;

    /// <summary>Base font flag</summary>
    /// <example><code>IsBaseFont true</code></example>
    internal readonly Boolean bIsBaseFont = true;

    // internal readonly Single fVVector_Origin0;  // only for MetricsSets 2

    // internal readonly Single fVVector_Origin1;  // only for MetricsSets 2

    // internal readonly Boolean bIsFixedV;  // only for MetricsSets 2

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    // Individual Character Metrics

    /// <summary>Number of character metrics entries</summary>
    internal readonly Int32 iCharMetricsCount;

    /// <summary>Character metrics definition array for unicode range 0 to 255</summary>
    private readonly CharMetrics[] aCharMetrics_0To255;

    /// <summary>Character metrics definition hashtable for unicode >= 256</summary>
    private readonly Hashtable ht_CharMetrics;
    #endregion

    //------------------------------------------------------------------------------------------16.02.2005
    #region Font Data
    //----------------------------------------------------------------------------------------------------

    /// <summary>TrueType Collection (file extension .ttc)</summary>
    private Boolean bTrueTypeCollection = false;

    /// <summary>TrueType Collection Index</summary>
    private Int32 iTrueTypeCollectionIndex = 0;

    //------------------------------------------------------------------------------------------04.04.2006
    private enum FontDataType {
      TrueTypeOutlines,
      CFFdata
    }

    private FontDataType fontDataType = FontDataType.TrueTypeOutlines;

    //------------------------------------------------------------------------------------------04.04.2006

    /// <summary>Number of tables</summary>
    private Int32 iNumTables;

    /// <summary>Maximum power of 2 <= numTables) x 16</summary>
    private Int32 iSearchRange;

    /// <summary>Log2(maximum power of 2 <= numTables)</summary>
    private Int32 iEntrySelector;

    /// <summary>NumTables x 16-searchRange</summary>
    private Int32 iRangeShift;
    
    //------------------------------------------------------------------------------------------08.04.2006
    /// <summary>Table directory</summary>
    private class TableDirectory {
      /// <summary>Identifier</summary>
      internal String sTag;

      /// <summary>Checksum for this table</summary>
      internal UInt32 uCheckSum;

      /// <summary>Offset from beginning of font file</summary>
      internal Int32 iOffset;

      /// <summary>Length of this table</summary>
      internal Int32 iLength;
    }

    private Dictionary<String, TableDirectory> dict_TableDirectory = new Dictionary<string, TableDirectory>(30);

    //------------------------------------------------------------------------------------------08.04.2006
    private class FontHeader {
      internal UInt32 uTableVersionNumber;
      internal Int32 iFlags;
      internal Int32 iUnitsPerEm;
      internal Int32 iXMin;
      internal Int32 iYMin;
      internal Int32 iXMax;
      internal Int32 iYMax;
      internal Int32 iMacStyle;
      internal Int32 iLowestRecPPEM;
      internal Int32 iFontDirectionHint;
      internal Int32 iIndexToLocFormat;
      internal Int32 iGlyphDataFormat;
    }

    private FontHeader fontHeader;

    //------------------------------------------------------------------------------------------08.04.2006
    private class HorizontalHeader {
      internal UInt32 uTableVersionNumber;
      internal Int32 iAscender;
      internal Int32 iDescender;
      internal Int32 iLineGap;
      internal Int32 iAdvanceWidthMax;
      internal Int32 iMinLeftSideBearing;
      internal Int32 iMinRightSideBearing;
      internal Int32 iXMaxExtent;
      internal Int32 iCaretSlopeRise;
      internal Int32 iCaretSlopeRun;
      internal Int32 iCaretOffset;
      internal Int32 iNumberOfHMetrics;
    }

    private HorizontalHeader horizontalHeader;

    //------------------------------------------------------------------------------------------06.04.2006
    private class WinMetrics {
      internal Int32 iVersion;
      internal Int32 iXAvgCharWidth;
      internal Int32 iUsWeightClass;
      internal Int32 iUsWidthClass;
      internal Int32 iFsType;
      internal Int32 iYSubscriptXSize;
      internal Int32 iYSubscriptYSize;
      internal Int32 iYSubscriptXOffset;
      internal Int32 iYSubscriptYOffset;
      internal Int32 iYSuperscriptXSize;
      internal Int32 iYSuperscriptYSize;
      internal Int32 iYSuperscriptXOffset;
      internal Int32 iYSuperscriptYOffset;
      internal Int32 iYStrikeoutSize;
      internal Int32 iYStrikeoutPosition;
      internal Int32 iSFamilyClass;
      internal Byte[] aByte_Panose;
      internal UInt32 uUlUnicodeRange1;
      internal UInt32 uUlUnicodeRange2;
      internal UInt32 uUlUnicodeRange3;
      internal UInt32 uUlUnicodeRange4;
      internal String sAchVendID;
      internal Int32 iFsSelection;
      internal Int32 iUsFirstCharIndex;
      internal Int32 iUsLastCharIndex;
      internal Int32 iSTypoAscender;
      internal Int32 iSTypoDescender;
      internal Int32 iSTypoLineGap;
      internal Int32 iUsWinAscent;
      internal Int32 iUsWinDescent;
      internal UInt32 iUlCodePageRange1 = 0;
      internal UInt32 iUlCodePageRange2 = 0;
      internal Int32 iSXHeight;
      internal Int32 iSCapHeight;
      internal UInt32 uUsDefaultChar;
      internal UInt32 uUsBreakChar;
      internal UInt32 uUsMaxContext;
    }

    private WinMetrics winMetrics;

    //------------------------------------------------------------------------------------------xx.02.2005
    //  post

    /// <summary>Italic angle</summary>
    internal Double rItalicAngle;
    
    /// <summary>Underline position</summary>
    private Int32 iUnderlinePosition;

    /// <summary>Underline thickness</summary>
    private Int32 iUnderlineThickness;

    /// <summary><see langword="true"/> if all the characters have the same width.</summary>
    internal Boolean bFixedPitch = false;

    //------------------------------------------------------------------------------------------xx.02.2005
    /// <summary>Reads the font data from the file.</summary>
    /// <param name="openTypeReader">Open Type Reader</param>
    private void ReadFontDataFromFile(OpenTypeReader openTypeReader) {
      #region TrueType Collection
      if (bTrueTypeCollection) {
        String sTTCTag = openTypeReader.sReadTag();
        if (sTTCTag != "ttcf") {
          throw new ReportException("'" + sFontName + " is not a valid TTC font file.");
        }
        UInt32 uVersion = openTypeReader.uReadULONG();
        Int32 iNumFonts = (Int32)openTypeReader.uReadULONG();
        if (iTrueTypeCollectionIndex >= iNumFonts) {
          throw new ReportException("'" + sFontName + " has invalid TrueType collection index.");
        }
        openTypeReader.Skip(iTrueTypeCollectionIndex * 4);
        Int32 iOffset = (Int32)openTypeReader.uReadULONG();
        openTypeReader.Seek(iOffset);
      }
      #endregion

      #region Offset Table
      UInt32 uSfntVersion = openTypeReader.uReadULONG();
      if (uSfntVersion == 0x4F54544F /* 'OTTO' */) {
        fontDataType = FontDataType.CFFdata;
      }
      #if DEBUG
      else if (uSfntVersion == 0x00010000 /* Version 1.0 */) {
        fontDataType = FontDataType.TrueTypeOutlines;
      }
      else {
        throw new ReportException("'" + sFontName + " is not a valid TTF, OTF or TTC font file.");
      }
      #endif

      iNumTables = (Int32)openTypeReader.iReadUSHORT();
      iSearchRange = (Int32)openTypeReader.iReadUSHORT();
      iEntrySelector = (Int32)openTypeReader.iReadUSHORT();
      iRangeShift = (Int32)openTypeReader.iReadUSHORT();
      #endregion

      #region Table Directory
      for (Int32 iTable = 0; iTable < iNumTables; iTable++) {
        TableDirectory tableDirectory_New = new TableDirectory();
        tableDirectory_New.sTag = openTypeReader.sReadTag();
        tableDirectory_New.uCheckSum = openTypeReader.uReadULONG();
        tableDirectory_New.iOffset = (Int32)openTypeReader.uReadULONG();
        tableDirectory_New.iLength = (Int32)openTypeReader.uReadULONG();
        dict_TableDirectory.Add(tableDirectory_New.sTag, tableDirectory_New);
      }
      Boolean bCFF = dict_TableDirectory.ContainsKey("CFF");
      if ((bCFF && fontDataType == FontDataType.TrueTypeOutlines) || (!bCFF && fontDataType == FontDataType.CFFdata)) {
        throw new ReportException("'" + sFontName + " is not a valid TTC font file.");
      }
      #endregion

      #region Font Header
      TableDirectory tableDirectory = dict_TableDirectory["head"];
      fontHeader = new FontHeader();
      openTypeReader.Seek(tableDirectory.iOffset);
      fontHeader.uTableVersionNumber = openTypeReader.uReadULONG();
      Debug.Assert(fontHeader.uTableVersionNumber == 0x00010000 /* Version 1.0 */);
      openTypeReader.Skip(12);
      fontHeader.iFlags = openTypeReader.iReadUSHORT();
      fontHeader.iUnitsPerEm = openTypeReader.iReadUSHORT();
      openTypeReader.Skip(16);
      fontHeader.iXMin = openTypeReader.int16_ReadSHORT();
      fontHeader.iYMin = openTypeReader.int16_ReadSHORT();
      fontHeader.iXMax = openTypeReader.int16_ReadSHORT();
      fontHeader.iYMax = openTypeReader.int16_ReadSHORT();
      fontHeader.iMacStyle = openTypeReader.iReadUSHORT();
      fontHeader.iLowestRecPPEM = openTypeReader.iReadUSHORT();
      fontHeader.iFontDirectionHint = openTypeReader.int16_ReadSHORT();
      fontHeader.iIndexToLocFormat = openTypeReader.int16_ReadSHORT();
      fontHeader.iGlyphDataFormat = openTypeReader.int16_ReadSHORT();
      #endregion

      #region Horizontal Header
      tableDirectory = dict_TableDirectory["hhea"];
      horizontalHeader = new HorizontalHeader();
      openTypeReader.Seek(tableDirectory.iOffset);
      horizontalHeader.uTableVersionNumber = openTypeReader.uReadULONG();
      Debug.Assert(horizontalHeader.uTableVersionNumber == 0x00010000 /* Version 1.0 */);
      horizontalHeader.iAscender = openTypeReader.int16_ReadFWORD();
      horizontalHeader.iDescender = openTypeReader.int16_ReadFWORD();
      horizontalHeader.iLineGap = openTypeReader.int16_ReadFWORD();
      horizontalHeader.iAdvanceWidthMax = openTypeReader.iReadUFWORD();
      horizontalHeader.iMinLeftSideBearing = openTypeReader.int16_ReadFWORD();
      horizontalHeader.iMinRightSideBearing = openTypeReader.int16_ReadFWORD();
      horizontalHeader.iXMaxExtent = openTypeReader.int16_ReadFWORD();
      horizontalHeader.iCaretSlopeRise = openTypeReader.int16_ReadSHORT();
      horizontalHeader.iCaretSlopeRun = openTypeReader.int16_ReadSHORT();
      horizontalHeader.iCaretOffset = openTypeReader.int16_ReadSHORT();
      openTypeReader.Skip(10);
      horizontalHeader.iNumberOfHMetrics = openTypeReader.iReadUSHORT();
      #endregion

      #region Windows Metrics (OS/2)
      tableDirectory = dict_TableDirectory["OS/2"];
      winMetrics = new WinMetrics();
      openTypeReader.Seek(tableDirectory.iOffset);
      winMetrics.iVersion = openTypeReader.iReadUSHORT();
      winMetrics.iXAvgCharWidth = openTypeReader.int16_ReadSHORT();
      winMetrics.iUsWeightClass = openTypeReader.iReadUSHORT();
      winMetrics.iUsWidthClass = openTypeReader.iReadUSHORT();
      winMetrics.iFsType = openTypeReader.iReadUSHORT();
      winMetrics.iYSubscriptXSize = openTypeReader.int16_ReadSHORT();
      winMetrics.iYSubscriptYSize = openTypeReader.int16_ReadSHORT();
      winMetrics.iYSubscriptXOffset = openTypeReader.int16_ReadSHORT();
      winMetrics.iYSubscriptYOffset = openTypeReader.int16_ReadSHORT();
      winMetrics.iYSuperscriptXSize = openTypeReader.int16_ReadSHORT();
      winMetrics.iYSuperscriptYSize = openTypeReader.int16_ReadSHORT();
      winMetrics.iYSuperscriptXOffset = openTypeReader.int16_ReadSHORT();
      winMetrics.iYSuperscriptYOffset = openTypeReader.int16_ReadSHORT();
      winMetrics.iYStrikeoutSize = openTypeReader.int16_ReadSHORT();
      winMetrics.iYStrikeoutPosition = openTypeReader.int16_ReadSHORT();
      winMetrics.iSFamilyClass = openTypeReader.int16_ReadSHORT();
      winMetrics.aByte_Panose = openTypeReader.aByte_ReadBYTE(10);
      if (winMetrics.iVersion == 0) {
        openTypeReader.Skip(4);  // skip ulCharRange
      }
      else {
        winMetrics.uUlUnicodeRange1 = openTypeReader.uReadULONG();
        winMetrics.uUlUnicodeRange2 = openTypeReader.uReadULONG();
        winMetrics.uUlUnicodeRange3 = openTypeReader.uReadULONG();
        winMetrics.uUlUnicodeRange4 = openTypeReader.uReadULONG();
      }
      winMetrics.sAchVendID = openTypeReader.sReadTag();
      winMetrics.iFsSelection = openTypeReader.iReadUSHORT();
      winMetrics.iUsFirstCharIndex = openTypeReader.iReadUSHORT();
      winMetrics.iUsLastCharIndex = openTypeReader.iReadUSHORT();
      winMetrics.iSTypoAscender = openTypeReader.int16_ReadSHORT();
      winMetrics.iSTypoDescender = openTypeReader.int16_ReadSHORT();
      winMetrics.iSTypoLineGap = openTypeReader.int16_ReadSHORT();
      winMetrics.iUsWinAscent = openTypeReader.iReadUSHORT();
      winMetrics.iUsWinDescent = openTypeReader.iReadUSHORT();
      if (winMetrics.iVersion > 0) {
        winMetrics.iUlCodePageRange1 = openTypeReader.uReadULONG();
        winMetrics.iUlCodePageRange2 = openTypeReader.uReadULONG();
        if (winMetrics.iVersion > 1) {
          winMetrics.iSXHeight = openTypeReader.int16_ReadSHORT();
          winMetrics.iSCapHeight = openTypeReader.int16_ReadSHORT();
          winMetrics.uUsDefaultChar = openTypeReader.uReadULONG();
          winMetrics.uUsBreakChar = openTypeReader.uReadULONG();
          winMetrics.uUsMaxContext = openTypeReader.uReadULONG();
        }
      }
      #endregion

      #region Post
      tableDirectory = dict_TableDirectory["post"];
      openTypeReader.Seek(tableDirectory.iOffset + 4);
      Double r1 = openTypeReader.int16_ReadSHORT();
      Double r0 = openTypeReader.iReadUSHORT();
      rItalicAngle = r1 + r0 / 16384.0;
      iUnderlinePosition = openTypeReader.int16_ReadFWORD();
      iUnderlineThickness = openTypeReader.int16_ReadFWORD();
      bFixedPitch = (openTypeReader.uReadULONG() != 0);
      #endregion

      ReadTable_name(openTypeReader);
      ReadTable_hmtx(openTypeReader);
    }
    #endregion

    //------------------------------------------------------------------------------------------16.02.2005
    #region Table "name"
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------xx.02.2005
    /// <summary>Reads the table "name".</summary>
    /// <param name="openTypeReader">Open Type Reader</param>
    private void ReadTable_name(OpenTypeReader openTypeReader) {
      TableDirectory tableDirectory = dict_TableDirectory["name"];
      openTypeReader.Seek(tableDirectory.iOffset + 2);
      Int32 iNumberOfRecords = openTypeReader.iReadUSHORT();
      Int32 iStringOffset = openTypeReader.iReadUSHORT();
      for (Int32 i = 0; i < iNumberOfRecords; i++) {
        openTypeReader.Seek(tableDirectory.iOffset + 6 + i * 12);
        Int32 iPlatformId = openTypeReader.iReadUSHORT();
        Int32 iEncodingId = openTypeReader.iReadUSHORT();
        Int32 iLanguageId = openTypeReader.iReadUSHORT();
        Int32 iNameId = openTypeReader.iReadUSHORT();
        Int32 iLength = openTypeReader.iReadUSHORT();
        Int32 iOffset = openTypeReader.iReadUSHORT();
        if (iNameId == 4) {
          openTypeReader.Seek(tableDirectory.iOffset + iStringOffset + iOffset);
          String sName;
          if (iPlatformId == 0 || (iPlatformId == 2 && iEncodingId == 1) || iPlatformId == 3) {
            sName = openTypeReader.sReadUnicodeString(iLength);
          }
          else {
            sName = openTypeReader.sReadCHAR(iLength);
          }
          sName = sName.Replace(Environment.NewLine, ";");
          //Console.WriteLine(iPlatformId + "\t" + iEncodingId + "\t" + iLanguageId + "\t" + iNameId + "\t" + sName);
          sFullFontName = sName.Replace(' ', '_');
        }
        else if (iNameId == 6) {
          openTypeReader.Seek(tableDirectory.iOffset + iStringOffset + iOffset);
          if (iPlatformId == 0 || iPlatformId == 3) {
            sBaseFontName = openTypeReader.sReadUnicodeString(iLength);
          }
          else {
            sBaseFontName = openTypeReader.sReadCHAR(iLength);
          }
        }
      }
    }
    #endregion

    //------------------------------------------------------------------------------------------16.02.2005
    #region Table "hmtx" (glyph widths)
    //----------------------------------------------------------------------------------------------------

    /// <summary>Glyph widths in font design units.</summary>
    private Int32[] aiGlyphWidth;

    //------------------------------------------------------------------------------------------xx.02.2005
    /// <summary>Reads the table "hmtx" (glyph widths).</summary>
    /// <remarks>The glyph widths are normalized to 1000 units.</remarks>
    /// <param name="openTypeReader">Open Type Reader</param>
    private void ReadTable_hmtx(OpenTypeReader openTypeReader) {
      aiGlyphWidth = new Int32[horizontalHeader.iNumberOfHMetrics];

      TableDirectory tableDirectory = dict_TableDirectory["hmtx"];
      openTypeReader.Seek(tableDirectory.iOffset);
      for (Int32 i = 0; i < aiGlyphWidth.Length; i++) {
        Int32 iWidth = openTypeReader.iReadUSHORT();
        aiGlyphWidth[i] = iWidth;
        openTypeReader.iReadUSHORT();  // left side bearing
      }
    }
    #endregion

    //------------------------------------------------------------------------------------------16.02.2005
    #region Methods
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------16.02.2005
    /// <summary>Returns the raw width of the specified text.</summary>
    /// <param name="sText">Text</param>
    /// <returns>Raw width of the text</returns>
    internal override Double rGetRawWidth(Char c) {
      return 500;
    }

    //------------------------------------------------------------------------------------------16.02.2005
    /// <summary>Returns the raw width of the specified text.</summary>
    /// <param name="sText">Text</param>
    /// <returns>Raw width of the text</returns>
    internal Int32 iGetRawWidth(Int32 iChar) {
      return aiGlyphWidth[iChar];
    }

    //------------------------------------------------------------------------------------------16.02.2005
    /// <summary>Returns the raw width of the specified text.</summary>
    /// <param name="sText">Text</param>
    /// <returns>Raw width of the text</returns>
    internal Double rRawWidth(String sText) {
      if (sText.Length == 0) {
        return 0;
      }

      Double rWidth = 0;
      //Double rCharSpacing = 0;
      //Double rWordSpacing = 0;

      foreach (Char c in sText) {
        Int16 iChar = (Int16)c;
        //CharMetrics acm = afmCharMetrics(iChar);
        //if (acm == null) {
        //  Console.WriteLine("Unknown character [" + c + "/" + iChar + "] in string [" + sText + "].");
        //}
        //else {
        //  Debug.Assert(!Single.IsNaN(acm.fWidthX));
        //  rWidth += acm.fWidthX;
        //}
        //if (c == ' ') {
        //  rWidth += rWordSpacing;
        //}
      }
      //rWidth += (sText.Length - 1) * rCharSpacing;
      return rWidth;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the height of the font in 1/72 inches.</summary>
    /// <param name="fontProp">Font properties</param>
    /// <returns>Height of the font in 1/72 inches</returns>
    internal  protected override Double rHeight(FontProp fontProp) {
//      Single fCapHeight = fontData.fCapHeight;
//      if (Single.IsNaN(fCapHeight)) {
//        fCapHeight = fontData.fFontBBox_ury - fontData.fFontBBox_lly;
//      }
//      Debug.Assert(!(Single.IsNaN(fCapHeight)));
//      return rSizeFactor(fontProp) * fCapHeight / 1000F;
      return 0;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the width of the specified text with this font in 1/72 inches.</summary>
    /// <param name="fontProp">Font properties</param>
    /// <param name="sText">Text</param>
    /// <returns>Width of the text in 1/72 inches</returns>
    internal  protected override Double rWidth(FontProp fontProp, String sText) {
//      return rSizeFactor(fontProp) * fontData.rRawWidth(sText) / 1000;
      return 0;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Gets the factor from the "EM"-size to the "H"-size.</summary>
    /// <returns>Factor from the "EM"-size to the "H"-size</returns>
    /// <remarks>"EM"-size * rGetFactor_EM_To_H() =  "H"-size</remarks>
    internal override Double rGetFactor_EM_To_H() {
      return /*fCapHeight*/1.00;  // !!!!!!!
    }
    #endregion

    //------------------------------------------------------------------------------------------16.02.2005
    #region CharMetrics
    //----------------------------------------------------------------------------------------------------

    /// <summary>AFM Character Metrics</summary>
    /// <example><code>C 102 ; WX 333 ; N f ; B 20 0 383 683 ; L i fi ; L l fl ;</code></example>
    internal class CharMetrics {
      /// <summary>Character code</summary>
      internal readonly Int32 iCharacterCode = -1;

      /// <summary>Horizontal character width for writing direction 0</summary>
      internal readonly Single fWidthX = 250;

      /// <summary>Character name</summary>
      internal readonly String sName;
      
      /// <summary>Font box</summary>
      internal readonly Single fBBox_llx;
      internal readonly Single fBBox_lly;
      internal readonly Single fBBox_urx;
      internal readonly Single fBBox_ury;
      
      /// <summary>Ligature sequence</summary>
      internal readonly ArrayList al_Ligature;

      //------------------------------------------------------------------------------------------13.02.2005
      /// <summary>Semicolon delimiter</summary>
      private static readonly Char[] acDelimiterSemicolon = {';'};

      /// <summary>Creates the character metrics object.</summary>
      /// <param name="type1FontData">Type1 font data object</param>
      /// <param name="sLine">Line with the character metrics information</param>
      internal CharMetrics(OpenTypeFontData openTypeFontData, String sLine) {
        #if WindowsCE
        String[] asLineToken = sLine.Split(acDelimiterSemicolon);
        #else
        String[] asLineToken = sLine.Split(acDelimiterSemicolon, 10);
        #endif
        if (asLineToken.Length <= 2) {
          throw new ReportException("Invalid character metrics definition in AFM file: " + openTypeFontData.sFontName);
        }
        for (Int32 iExpr = 0;  iExpr < asLineToken.Length;  iExpr++) {
          if (asLineToken[iExpr].Length == 0) {
            continue;
          }
          #if WindowsCE
          String[] asToken = asLineToken[iExpr].Trim().Split(acDelimiterToken);
          #else
          String[] asToken = asLineToken[iExpr].Trim().Split(acDelimiterToken, 5);
          #endif
          switch (asToken[0]) {
            case "C": { iCharacterCode = Int32.Parse(asToken[1], CultureInfo.InvariantCulture);  break; }
            case "CH": {
              iCharacterCode = Int32.Parse(asToken[1], System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
              break;
            }
            case "WX":  case "W0X": { fWidthX = Single.Parse(asToken[1], CultureInfo.InvariantCulture);  break; }
            case "N": { sName = asToken[1];  break; }
            case "B": {
              fBBox_llx = Single.Parse(asToken[1], CultureInfo.InvariantCulture);
              fBBox_lly = Single.Parse(asToken[2], CultureInfo.InvariantCulture);
              fBBox_urx = Single.Parse(asToken[3], CultureInfo.InvariantCulture);
              fBBox_ury = Single.Parse(asToken[4], CultureInfo.InvariantCulture);
              break;
            }
            case "L": {
              if (al_Ligature == null) {
                al_Ligature = new ArrayList(10);
              }
              al_Ligature.Add(new Ligature(asToken[1], asToken[2]));
              break;
            }
            default: {
              Debug.Fail("Unknown token [" + asToken[0] + "] in AFM file: " + openTypeFontData.sFontName);
              break;
            }
          }
        }
      }
    }
    #endregion

    //------------------------------------------------------------------------------------------13.02.2005
    #region Ligature
    //----------------------------------------------------------------------------------------------------

    /// <summary>AFM Ligature</summary>
    private struct Ligature {
      /// <summary>Successor</summary>
      internal readonly String sSuccessor;

      /// <summary>Ligature</summary>
      internal readonly String sLigature;

      //------------------------------------------------------------------------------------------13.02.2005
      /// <summary>Creates a ligature object.</summary>
      /// <param name="sSuccessor"></param>
      /// <param name="sLigature"></param>
      public Ligature(String sSuccessor, String sLigature) {
        this.sSuccessor = sSuccessor;
        this.sLigature = sLigature;
      }
    }
    #endregion
  }
}
