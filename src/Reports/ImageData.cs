using System;
using System.Drawing;
using System.IO;

// Creation date: 22.04.2002
// Checked: 05.08.2002
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01

// Report.NET copyright 2002-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Image Data Object</summary>
  public class ImageData {
    /// <summary>Image stream</summary>
    internal Stream stream;

    /// <summary>Internal structure used by the formatter</summary>
    internal Object oImageResourceX;

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new image data object</summary>
    /// <param name="stream">Image stream</param>
    public ImageData(Stream stream) {
      this.stream = stream;
    }

  }
}
