using System;
using System.IO;
using System.Text;

// Creation date: 07.04.2006
// Checked: 05.05.2006
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
  /// <summary>Reader Class for OpenType Fonts</summary>
  internal class OpenTypeReader : IDisposable {
    /// <summary>Stream</summary>
    private Stream stream;

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Creates a reader for open type fonts.</summary>
    /// <param name="sFileName">Name of the font file</param>
    internal OpenTypeReader(String sFileName) {
      stream = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Skips the specified number of bytes of the stream.</summary>
    /// <param name="iBytes">Number of bytes that must be skipped.</param>
    internal void Skip(Int32 iBytes) {
      stream.Seek(iBytes, SeekOrigin.Current);
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>This method will set the read position to the specified offset.</summary>
    /// <param name="iOffset">New read position</param>
    internal void Seek(Int32 iOffset) {
      stream.Seek(iOffset, SeekOrigin.Begin);
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads the specified number of BYTE values (8-bit unsigned integer) from the stream.</summary>
    /// <param name="iLength">Number of BYTE values</param>
    /// <returns>array of BYTE values</returns>
    internal Byte[] aByte_ReadBYTE(Int32 iLength) {
      Byte[] aByte = new Byte[iLength];
      stream.Read(aByte, 0, iLength);
      return aByte;
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads the specified number of CHAR values (8-bit signed integer) from the stream.</summary>
    /// <param name="iLength">Number of CHAR values</param>
    /// <returns>string with the CHAR values</returns>
    internal String sReadCHAR(Int32 iLength) {
      Byte[] aByte = aByte_ReadBYTE(iLength);
      String s = System.Text.Encoding.GetEncoding("windows-1252").GetString(aByte);
      return s.Trim();
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads an USHORT value (16-bit unsigned integer) from the stream.</summary>
    /// <returns>USHORT value</returns>
    internal Int32 iReadUSHORT() {
      Int32 i1 = stream.ReadByte();
      Int32 i2 = stream.ReadByte();
      return (i1 << 8) + i2;
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads a SHORT value (16-bit signed integer) from the stream.</summary>
    /// <returns>SHORT value</returns>
    internal Int16 int16_ReadSHORT() {
      Int32 i1 = stream.ReadByte();
      Int32 i2 = stream.ReadByte();
      return (Int16)((i1 << 8) + i2);
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads an ULONG value (32-bit unsigned integer) from the stream.</summary>
    /// <returns>ULONG value</returns>
    internal UInt32 uReadULONG() {
      UInt32 u1 = (UInt32)stream.ReadByte();
      UInt32 u2 = (UInt32)stream.ReadByte();
      UInt32 u3 = (UInt32)stream.ReadByte();
      UInt32 u4 = (UInt32)stream.ReadByte();
      return (u1 << 24) + (u2 << 16) + (u3 << 8) + u4;
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads a LONG value (32-bit signed integer) from the stream.</summary>
    /// <returns>LONG value</returns>
    internal Int32 iReadLONG() {
      Int32 i1 = stream.ReadByte();
      Int32 i2 = stream.ReadByte();
      Int32 i3 = stream.ReadByte();
      Int32 i4 = stream.ReadByte();
      return (i1 << 24) + (i2 << 16) + (i3 << 8) + i4;
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads a FWORD value (16-bit signed integer, SHORT, quantity in FUnits) from the stream.</summary>
    /// <returns>FWORD value</returns>
    internal Int16 int16_ReadFWORD() {
      return int16_ReadSHORT();
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads an UFWORD value (16-bit unsigned integer, USHORT, quantity in FUnits) from the stream.</summary>
    /// <returns>USHORT value</returns>
    internal Int32 iReadUFWORD() {
      return iReadUSHORT();
    }

    //------------------------------------------------------------------------------------------07.04.2006
    /// <summary>Reads a tag (4 uint8s) from the stream.</summary>
    /// <returns>tag value</returns>
    internal String sReadTag() {
      return sReadCHAR(4);
    }

    //------------------------------------------------------------------------------------------05.05.2006
    /// <summary>Reads the specified number of CHAR values (8-bit signed integer) from the stream.</summary>
    /// <param name="iLength">Number of CHAR values</param>
    /// <returns>string with the CHAR values</returns>
    internal String sReadUnicodeString(Int32 iLength) {
      iLength /= 2;
      StringBuilder sb = new StringBuilder(iLength);
      for (Int32 i = 0;  i < iLength;  ++i) {
        sb.Append((Char)iReadUSHORT());
      }
      return sb.ToString();
    }

    //------------------------------------------------------------------------------------------07.04.2006
    #region IDisposable Members
    //----------------------------------------------------------------------------------------------------

    /// <summary>Releases all resources used by the OpenTypeReader object.</summary>
    public void Dispose() {
      stream.Close();
    }
    #endregion
  }
}
