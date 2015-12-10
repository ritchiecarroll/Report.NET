using System;
using System.Text;

// Creation date: 19.01.2006
// Checked: 30.01.2006
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
  /// <summary>PDF File Element</summary>
  /// <remarks>This class is used to build the PDF file structure: body, cross-reference table, trailer.</remarks>
  internal abstract class PdfFileElement {
    //------------------------------------------------------------------------------------------30.01.2006
    #region PdfFileElement
    //----------------------------------------------------------------------------------------------------

    /// <summary>PDF formatter</summary>
    protected readonly PdfFormatter pdfFormatter;

    //------------------------------------------------------------------------------------------28.01.2006
    /// <summary>Creates a PDF file element.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfFileElement(PdfFormatter pdfFormatter) {
      this.pdfFormatter = pdfFormatter;
      this.sb = pdfFormatter.sb;
    }

    //------------------------------------------------------------------------------------------03.02.2006
    /// <summary>Gets the report object.</summary>
    protected Report report {
      get { return pdfFormatter.report; }
    }

    //------------------------------------------------------------------------------------------28.01.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal abstract void Write();

    //------------------------------------------------------------------------------------------30.01.2006
    /// <summary>Writes a string to the buffer.</summary>
    /// <param name="sString">String</param>
    protected void Write(String sString) {
      sb.Append(sString);
    }

    //------------------------------------------------------------------------------------------30.01.2006
    /// <summary>Writes a string and a new-line-sequence to the buffer.</summary>
    /// <param name="sString">String</param>
    [Obsolete()]
#warning remove
    protected void WriteLine(String sString) {
      sb.Append(sString);
      sb.Append('\n');
      bNeedSpace = false;
    }
    #endregion

    //------------------------------------------------------------------------------------------29.01.2006
    #region Buffer
    //----------------------------------------------------------------------------------------------------

    /// <summary>String builder</summary>
    protected readonly StringBuilder sb;

    /// <summary>true if a space is required before the next token</summary>
    protected Boolean bNeedSpace = false;

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a space to the buffer if a space is required.</summary>
    internal void Space() {
      if (bNeedSpace) {
        sb.Append(' ');
        bNeedSpace = false;
      }
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a new-line sequence to the buffer if a space is required.</summary>
    internal void NewLine() {
      if (bNeedSpace) {
        sb.Append('\n');
        bNeedSpace = false;
      }
    }
    #endregion

    //------------------------------------------------------------------------------------------29.01.2006
    #region PDF Standard Objects
    //----------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a boolean-object to the buffer.</summary>
    /// <param name="bValue">Boolean value</param>
    internal void Boolean(Boolean bValue) {
      NewLine();
      sb.Append(bValue ? "true" : "false");
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes an integer number-object to the buffer.</summary>
    /// <param name="iNumber">Integer number</param>
    internal void Number(Int32 iNumber) {
      NewLine();
      sb.Append(iNumber);
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------16.02.2006
    /// <summary>Writes an integer number-object to the buffer.</summary>
    /// <param name="iNumber">Integer number</param>
    internal void Number(Int64 lNumber) {
      NewLine();
      sb.Append(lNumber);
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a real 64-bit number-object to the buffer.</summary>
    /// <param name="rNumber">Real 64-bit number</param>
    internal void Number(Double rNumber) {
      NewLine();
      String s = RT.sPdfDim(rNumber);
      sb.Append(s);
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a string-object to the buffer.</summary>
    /// <param name="sValue">String value</param>
    internal void String(String sValue) {
      NewLine();
      String s = RT.sPdfString(sValue);
      sb.Append(s);
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a name-object to the buffer.</summary>
    /// <param name="sName">Name</param>
    internal void Name(String sName) {
      NewLine();
      sb.Append('/');
      sb.Append(sName);
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes the start sequence for an array-object to the buffer.</summary>
    internal void ArrayStart() {
      NewLine();
      sb.Append('[');
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes the end sequence for an array-object to the buffer.</summary>
    internal void ArrayEnd() {
      sb.Append("]");
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes the start sequence for a dictionary-object to the buffer.</summary>
    internal void Dictionary_Start() {
      NewLine();
      sb.Append("<<");
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes the end sequence for a dictionary-object to the buffer.</summary>
    internal void Dictionary_End() {
      NewLine();
      sb.Append(">>");
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes the key-value of a dictionary-entry to the buffer.</summary>
    /// <param name="sKey">Key value</param>
    internal void Dictionary_Key(String sKey) {
      Name(sKey);
      Space();
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes an indirect-reference-object to the buffer.</summary>
    /// <param name="pdfIndirectObject">Referenced indirect object</param>
    internal void IndirectReference(PdfIndirectObject pdfIndirectObject) {
      NewLine();
      sb.Append(pdfIndirectObject.iObjectNumber);
      sb.Append(" 0 R");
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a token to the buffer.</summary>
    /// <param name="sToken">Token</param>
    internal void Token(String sToken) {
      NewLine();
      sb.Append(sToken);
      bNeedSpace = true;
    }

    //------------------------------------------------------------------------------------------29.01.2006
    /// <summary>Writes a date-object to the buffer.</summary>
    /// <param name="dt">Date value</param>
    internal void Date(DateTime dt) {
      String("D:" + dt.ToString("yyyyMMdd"));
    }
    #endregion
  }

  //------------------------------------------------------------------------------------------30.01.2006
  #region PdfFileElement_XRef
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF File Element: XRef</summary>
  internal sealed class PdfFileElement_XRef : PdfFileElement {
    //------------------------------------------------------------------------------------------30.01.2006
    /// <summary>Creates the cross-reference table.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfFileElement_XRef(PdfFormatter pdfFormatter) : base(pdfFormatter) {
    }

    //------------------------------------------------------------------------------------------30.01.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      Token("xref");
      Number(0);  Space();  Number(pdfFormatter.list_PdfIndirectObject.Count + 1);
      NewLine();
      Write("0000000000 65535 f\r\n");
      foreach (PdfIndirectObject pdfIndirectObject in pdfFormatter.list_PdfIndirectObject) {
        Write(pdfIndirectObject.iObjectPosition.ToString("D10") + " 00000 n\r\n");
      }
    }
  }
  #endregion

  //------------------------------------------------------------------------------------------30.01.2006
  #region PdfFileElement_Trailer
  //----------------------------------------------------------------------------------------------------

  /// <summary>PDF File Element: Trailer</summary>
  internal sealed class PdfFileElement_Trailer : PdfFileElement {
    //------------------------------------------------------------------------------------------30.01.2006
    /// <summary>Creates the trailer.</summary>
    /// <param name="pdfFormatter">PDF formatter</param>
    internal PdfFileElement_Trailer(PdfFormatter pdfFormatter) : base(pdfFormatter) {
    }

    //------------------------------------------------------------------------------------------30.01.2006
    /// <summary>Writes the object to the buffer.</summary>
    internal override void Write() {
      Token("trailer");
      Dictionary_Start();
      Dictionary_Key("Size");  Number(pdfFormatter.list_PdfIndirectObject.Count + 1);
      Dictionary_Key("Root");  IndirectReference(pdfFormatter.pdfIndirectObject_Catalog);
      Dictionary_Key("Info");  IndirectReference(pdfFormatter.pdfIndirectObject_Info);
      if (pdfFormatter.sFileIdentifier != null) {
        Dictionary_Key("ID");
        ArrayStart();
        String(pdfFormatter.sFileIdentifier);  Space();  String(pdfFormatter.sFileIdentifier);
        ArrayEnd();
      }
      Dictionary_End();
      Token("startxref");
      Number(pdfFormatter.iXRefPos);
      Token("%%EOF");  // no need for EOL
    }
  }
  #endregion
}
