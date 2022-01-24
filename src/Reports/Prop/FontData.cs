using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text; 

// Creation date: 11.10.2002
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
  /// <summary>Font Data</summary>
  /// <remarks>This is the base class of the font types that are supported by the Report.NET library.</remarks>
  internal abstract class FontData {
    internal FontDef fontDef;

    /// <summary>Font style</summary>
    internal readonly FontStyle fontStyle;

    /// <summary>Encoding to be applied to this font</summary>
    private readonly Encoding encoding;

    /// <summary><see langword="true"/> if the font is to be embedded in the document</summary>
    internal readonly Boolean bEmbed;

    /// <summary>Extended font property data (e.g. to add values that are used by the PDF formatter).</summary>
    internal Object oFontDataX;

    internal BitArray bitArray_UsedChar = new BitArray(10000);

    internal Int32 iFirstChar {
      get {
        for (Int32 i = 0; i < bitArray_UsedChar.Length; i++) {
          if (bitArray_UsedChar[i]) {
            return i;
          }
        }
        System.Diagnostics.Debug.Fail("no characters used");
        return 0;
      }
    }

    internal Int32 iLastChar {
      get {
        for (Int32 i = bitArray_UsedChar.Length - 1;  i >= 0;  i--) {
          if (bitArray_UsedChar[i]) {
            return i;
          }
        }
        System.Diagnostics.Debug.Fail("no characters used");
        return 0;
      }
    }

    //------------------------------------------------------------------------------------------06.03.2005
    /// <summary></summary>
    /// <param name="fontDef">Font definition</param>
    /// <param name="fontStyle">Font style</param>
    /// <param name="encoding">Encoding to be applied to this font</param>
    internal FontData(FontDef fontDef, FontStyle fontStyle, Encoding encoding) {
      this.fontDef = fontDef;
      this.fontStyle = fontStyle;
      this.encoding = encoding;
      this.bEmbed = false;
    }

    //------------------------------------------------------------------------------------------06.03.2005
    /// <summary>Gets the height of the font in points (1/72 inch).</summary>
    /// <param name="fontProp">Font properties</param>
    /// <returns>Height of the font in points (1/72 inch)</returns>
    internal protected abstract Double rHeight(FontProp fontProp);

    //------------------------------------------------------------------------------------------06.03.2005
    /// <summary>Gets the width of the specified text in points (1/72 inch).</summary>
    /// <param name="fontProp">Font properties</param>
    /// <param name="sText">Text</param>
    /// <returns>Width of the text in points (1/72 inch)</returns>
    internal protected abstract Double rWidth(FontProp fontProp, String sText);

    //------------------------------------------------------------------------------------------16.02.2005
    /// <summary>Returns the raw width of the specified text.</summary>
    /// <param name="sText">Text</param>
    /// <returns>Raw width of the text</returns>
    internal abstract Double rGetRawWidth(Char c);

    //------------------------------------------------------------------------------------------03.02.2005
    /// <summary>Gets the report to which this font property belongs.</summary>
    /// <remarks>A font property is only valid for one report.</remarks>
    public Report report {
      get { return fontDef.report; }
    }

    //------------------------------------------------------------------------------------------17.07.2006
    /// <summary>Gets the factor from the "EM"-size to the "H"-size.</summary>
    /// <returns>Factor from the "EM"-size to the "H"-size</returns>
    /// <remarks>"EM"-size * rGetFactor_EM_To_H() =  "H"-size</remarks>
    internal abstract Double rGetFactor_EM_To_H();

    //------------------------------------------------------------------------------------------16.02.2005
    /// <summary>Last Character</summary>
    private enum LastChar {
      /// <summary>Last character has been a space</summary>
      Space,
      /// <summary>Last character has been a separator that must be kept at the end of the line</summary>
      SeparatorAtEndOfLine,
      /// <summary>Last character has been a separator that can start on the new line</summary>
      SeparatorAtStartOfLine,
      /// <summary>Last character has been a character of a word</summary>
      Word
    }

    /// <summary>Gets a text line for the specified width.</summary>
    /// <param name="sText">Text</param>
    /// <param name="rWidthMax">Width</param>
    /// <param name="iStart">Start position in sText</param>
    /// <param name="textSplitMode">Text split mode</param>
    /// <returns>Line of text</returns>
    internal String sGetTextLine(String sText, Double rWidthMax, ref Int32 iStart, TextSplitMode textSplitMode) {
      if (iStart > sText.Length) {
        throw new ReportException("start position out of range");
      }
      if (iStart == sText.Length) {
        iStart++;
        return "";
      }
      Int32 iStartCopy = iStart;

      StringBuilder sb = new StringBuilder(120);
      Double rWidth = 0;
      Int32 iPos = iStart;
      Int32 iResultLength = 0;
      LastChar lastChar = LastChar.Space;
      while (true) {
        Char c = sText[iPos];
        iPos++;
        Double rWidthChar = rGetRawWidth(c);
        UnicodeCategory unicodeCategory = Char.GetUnicodeCategory(c);
        switch (unicodeCategory) {
          case UnicodeCategory.Control: {  // control character
            if (c == '\n') {
              goto case UnicodeCategory.LineSeparator;
            }
            if (lastChar != LastChar.Space) {
              iResultLength = sb.Length;
              lastChar = LastChar.Space;
            }
            iStart = iPos;
            break;
          }
          case UnicodeCategory.LineSeparator:
          case UnicodeCategory.ParagraphSeparator: {
            iResultLength = sb.Length;
            iStart = iPos;
            goto EndLoop;
          }
          case UnicodeCategory.ConnectorPunctuation:  // connects two characters
          case UnicodeCategory.EnclosingMark:  // nonspacing combining character that surrounds all previous characters up to and including a base character
          case UnicodeCategory.Format:  // not normally rendered but affects the layout of text or the operation of text processes
          case UnicodeCategory.ModifierLetter:  // free-standing spacing character that indicates modifications of a preceding letter
          case UnicodeCategory.ModifierSymbol:  // indicates modifications of surrounding characters
          case UnicodeCategory.NonSpacingMark:  // indicates modifications of a base character
          case UnicodeCategory.PrivateUse:  // private-use character with unicode value in the range U+E000 through U+F8FF
          case UnicodeCategory.SpacingCombiningMark:  // indicates modifications of a base character and affects the width of the glyph for that base character
          case UnicodeCategory.Surrogate: {  // high-surrogate or a low-surrogate with code values in the range U+D800 through U+DFFF
            break;
          }
          case UnicodeCategory.SpaceSeparator: {  // space
            if (lastChar != LastChar.Space) {
              iResultLength = sb.Length;
              lastChar = LastChar.Space;
            }
            rWidth += rWidthChar;
            sb.Append(c);
            iStart = iPos;
            break;
          }
          case UnicodeCategory.ClosePunctuation:  // separator on this line: )]}
          case UnicodeCategory.DashPunctuation:  // -
          case UnicodeCategory.FinalQuotePunctuation: {
            rWidth += rWidthChar;
            if (rWidth > rWidthMax) {
              iPos--;
              goto EndLoop;
            }
            sb.Append(c);
            lastChar = LastChar.SeparatorAtEndOfLine;
            break;
          }
          case UnicodeCategory.CurrencySymbol:  // separator for next line: €
          case UnicodeCategory.InitialQuotePunctuation:  // opening or initial quotation mark
          case UnicodeCategory.OpenPunctuation: {  // ([{
            if (lastChar == LastChar.SeparatorAtEndOfLine || lastChar == LastChar.Word) {
              iResultLength = sb.Length;
              iStart = iPos - 1;
            }
            rWidth += rWidthChar;
            if (rWidth > rWidthMax) {
              iPos--;
              goto EndLoop;
            }
            sb.Append(c);
            lastChar = LastChar.SeparatorAtStartOfLine;
            break;
          }
          case UnicodeCategory.OtherPunctuation: {  // .,
            rWidth += rWidthChar;
            if (rWidth > rWidthMax) {
              iPos--;
              goto EndLoop;
            }
            sb.Append(c);
            lastChar = LastChar.Word;
            break;
          }
          case UnicodeCategory.DecimalDigitNumber:  // character
          case UnicodeCategory.LetterNumber:
          case UnicodeCategory.LowercaseLetter:
          case UnicodeCategory.MathSymbol:  // + =
          case UnicodeCategory.OtherLetter:
          case UnicodeCategory.OtherNotAssigned:
          case UnicodeCategory.UppercaseLetter:
          case UnicodeCategory.TitlecaseLetter: {
            if (lastChar == LastChar.SeparatorAtEndOfLine) {
              iResultLength = sb.Length;
              iStart = iPos;
            }
            rWidth += rWidthChar;
            if (rWidth > rWidthMax) {
              iPos--;
              goto EndLoop;
            }
            sb.Append(c);
            lastChar = LastChar.Word;
            break;
          }
          default: {
            Debug.Fail("unknown unicode category");
            break;
          }
        }
        if (iPos >= sText.Length) {
          iResultLength = sb.Length;
          iStart = iPos + 1;
          goto EndLoop;
        }
      }
      EndLoop:
      if (textSplitMode == TextSplitMode.Truncate) {
        iStart = sb.Length;
        return sb.ToString();
      }
      if (iStart == iStartCopy) {
        if (sb.Length > 0) {
          iStart = iPos;
          return sb.ToString();
        }
        iStart++;
        return sText[iStart - 1].ToString();
      }
      return sb.ToString(0, iResultLength);
    }

    //------------------------------------------------------------------------------------------06.03.2005
    #region
    //----------------------------------------------------------------------------------------------------
    /// <summary>Font encoding</summary>
    internal enum Encoding {
      /// <summary>CP 1250 encoding</summary>
      Cp1250,
      /// <summary>CP 1252 (WIN ANSI) encoding</summary>
      Cp1252,
      /// <summary>CP 1257 encoding</summary>
      Cp1257,
      /// <summary>MAC ROMAN encoding</summary>
      MacRoman
    }
    #endregion
  }

  [Flags()]
  public enum TextSplitMode {
    Line,
    Truncate
  }
}
