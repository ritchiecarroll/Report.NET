using System;
#if Framework2
using System.Collections.Generic;
#else
using System.Collections;
#endif
using System.Diagnostics;

// Creation date: 22.04.2002
// Checked: 17.05.2002
// Author: Otto Mayer, mot@root.ch
// Version: 1.05

// Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>Container for report objects</summary>
  #if Framework2
  public abstract class Container : RepObj, System.Collections.Generic.IEnumerable<RepObj> {
  #else
  public abstract class Container	: RepObj, IEnumerable {
  #endif
    /// <summary>Array that contains all report objects of this container</summary>
    #if Framework2
    private readonly List<RepObj> list_RepObj;
    #else
    private readonly ArrayList list_RepObj;
    #endif

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new container.</summary>
    protected Container() {
      #if Framework2
      list_RepObj = new List<RepObj>(20);
      #else
      list_RepObj = new ArrayList(20);
      #endif
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new container.</summary>
    public Container(Double rWidth, Double rHeight) {
      this.rWidth = rWidth;
      this.rHeight = rHeight;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called after the report object has been added to the container.</summary>
    public void Remove(RepObj repObj) {
      list_RepObj.Remove(repObj);
      repObj.container = null;
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>This method will be called after the report object has been added to the container.</summary>
    internal protected override void OnAdded() {
      oRepObjX = report.formatter.oCreate_Container();
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Appends a report object to the container.</summary>
    /// <param name="repObj">Report object that must be added to the container</param>
    internal void Add(RepObj repObj) {
      Add(repObj, null);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void Add(Double rX, Double rY, RepObj repObj) {
      repObj.matrixD.rDX = rX;
      repObj.matrixD.rDY = rY;
      Add(repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container.</summary>
    /// <param name="repObj">Report object that must be added to the container</param>
    /// <param name="repObj_Pos">The new report object will be inserted before the specified position.
    /// If the position is null it will be appended to the end of the list.</param>
    internal void Add(RepObj repObj, RepObj repObj_Pos) {
      if (repObj.container != null) {
        throw new ReportException("Report objects cannot be added to more than one container");
      }
      if (Object.ReferenceEquals(this, repObj)) {
        throw new ReportException("Report objects cannot be added to itself");
      }

      if (repObj_Pos == null) {
        list_RepObj.Add(repObj);
      }
      else {
        if (!Object.ReferenceEquals(repObj_Pos.container, this)) {
          throw new ReportException("The report object indicating the position is not a member of the container");
        }
        Int32 iIndex = list_RepObj.IndexOf(repObj_Pos);
        Debug.Assert(iIndex >= 0);
        list_RepObj.Insert(iIndex, repObj);
      }
      repObj.container = this;
      repObj.OnAdded();
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
    /// <param name="repObj">Report object to add to the container</param>
    /// <param name="repObj_Pos">The new report object will be inserted before the specified position.
    /// If the position is null it will be appended to the end of the list.</param>
    internal void AddAligned(Double rX, Double rAlignH, Double rY, Double rAlignV, RepObj repObj, RepObj repObj_Pos) {
      repObj.matrixD.rDX = rX;
      repObj.rAlignH = rAlignH;
      repObj.matrixD.rDY = rY;
      repObj.rAlignV = rAlignV;
      Add(repObj, repObj_Pos);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rAlignH">Horizontal alignment of the report object relative to [X].</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="rAlignV">Vertical alignment of the report object relative to [Y].</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void AddAligned(Double rX, Double rAlignH, Double rY, Double rAlignV, RepObj repObj) {
      AddAligned(rX, rAlignH, rY, rAlignV, repObj, null);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    /// <param name="repObj_Pos">The new report object will be inserted before the specified position.
    /// If the position is null it will be appended to the end of the list.</param>
    public void AddLT(Double rX, Double rY, RepObj repObj, RepObj repObj_Pos) {
      AddAligned(rX, RepObj.rAlignLeft, rY, RepObj.rAlignTop, repObj, repObj_Pos);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void AddLT(Double rX, Double rY, RepObj repObj) {
      AddLT(rX, rY, repObj, null);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddLT_MM(Double rX, Double rY, RepObj repObj) {
      AddLT(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void AddCT(Double rX, Double rY, RepObj repObj) {
      AddAligned(rX, RepObj.rAlignCenter, rY, RepObj.rAlignTop, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddCT_MM(Double rX, Double rY, RepObj repObj) {
      AddCT(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void AddCC(Double rX, Double rY, RepObj repObj) {
      AddAligned(rX, RepObj.rAlignCenter, rY, RepObj.rAlignCenter, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddCC_MM(Double rX, Double rY, RepObj repObj) {
      AddCC(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void AddCB(Double rX, Double rY, RepObj repObj) {
      AddAligned(rX, RepObj.rAlignCenter, rY, RepObj.rAlignBottom, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddCB_MM(Double rX, Double rY, RepObj repObj) {
      AddCB(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void AddRT(Double rX, Double rY, RepObj repObj) {
      AddAligned(rX, RepObj.rAlignRight, rY, RepObj.rAlignTop, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddRT_MM(Double rX, Double rY, RepObj repObj) {
      AddRT(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    internal void AddRC(Double rX, Double rY, RepObj repObj) {
      AddAligned(rX, RepObj.rAlignRight, rY, RepObj.rAlignCenter, repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Adds a report object to the container and sets the alignment.</summary>
    /// <param name="rX">X-coordinate of the report object</param>
    /// <param name="rY">Y-coordinate of the report object</param>
    /// <param name="repObj">Report object to add to the container</param>
    public void AddRC_MM(Double rX, Double rY, RepObj repObj) {
      AddRC(RT.rPointFromMM(rX), RT.rPointFromMM(rY), repObj);
    }

    //----------------------------------------------------------------------------------------------------x
    #region IEnumerable Members
    //----------------------------------------------------------------------------------------------------

    #if Framework2
    //----------------------------------------------------------------------------------------------------x
    /// <summary>Returns an enumerator that can iterate through the report objects.</summary>
    /// <returns>An enumerator that can be used to iterate through the report objects</returns>
    public System.Collections.Generic.IEnumerator<RepObj> GetEnumerator() {
      return list_RepObj.GetEnumerator();
    }
    #endif

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Returns an enumerator that can iterate through the report objects.</summary>
    /// <returns>An enumerator that can be used to iterate through the report objects</returns>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return list_RepObj.GetEnumerator();
    }
    #endregion
  }
}
