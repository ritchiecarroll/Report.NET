using System;
using System.Drawing;

// Creation date: 31.03.2003
// Checked: 08.04.2003
// Author: Otto Mayer (mot@root.ch)
// Version: 1.01

// Report.NET copyright 2003-2004 root-software ag, Bürglen Switzerland - O. Mayer, S. Spirig, R. Gartenmann, all rights reserved
// This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
// as published by the Free Software Foundation, version 2.1 of the License.
// This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
// should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

namespace Root.Reports {
  /// <summary>List Layout Manager</summary>
  public class ListLayoutManager : TlmBase {
    //====================================================================================================x

    //----------------------------------------------------------------------------------------------------x
    /// <summary>Creates a new list layout manager.</summary>
    /// <param name="report">Report object of this list layout manager</param>
    public ListLayoutManager(Report report) : base(report) {
      //pp_Border = new PenPropMM(report, 0.1, Color.Black);
    }

    //----------------------------------------------------------------------------------------------------x###
    /// <summary>This method will be called after a new row has been created.</summary>
    /// <param name="row"></param>
    internal protected override void OnNewRow(TlmRow row) {
      if (row.iIndex != 0) {
        return;
      }
      foreach (TlmColumn col in list_TlmColumn) {
        TlmCell cell = row.aTlmCell[col.iIndex];
        if (!Double.IsNaN(col.rBorderTop)) {
          cell.rMarginTop = col.rBorderTop;
        }
        if (!Object.ReferenceEquals(col.penProp_BorderTop, PenProp.penProp_Null)) {
          cell.penProp_LineTop = col.penProp_BorderTop;
        }
      }
    }

    //----------------------------------------------------------------------------------------------------x###
    /// <summary>This method will be called before the report objects will be written to the container.</summary>
    internal override void OnBeforeWrite() {
      foreach (TlmColumn col in list_TlmColumn) {
        TlmCell cell = tlmRow_Committed.aTlmCell[col.iIndex];
        if (!Double.IsNaN(col.rBorderBottom)) {
          cell.rMarginBottom = col.rBorderBottom;
        }
        if (!Object.ReferenceEquals(col.penProp_BorderBottom, PenProp.penProp_Null)) {
          cell.penProp_LineBottom = col.penProp_BorderBottom;
        }
      }
    }

    //------------------------------------------------------------------------------------------16.02.2006
    #if Compatible_0_8
    //----------------------------------------------------------------------------------------------------

    //----------------------------------------------------------------------------------------------------
    public class Column : TlmColumn {
      public Column(ListLayoutManager llm, Double rWidth) : base(llm, rWidth) {
      }
    }

    //----------------------------------------------------------------------------------------------------
    public class ColumnMM : Column {
      public ColumnMM(ListLayoutManager llm, Double rWidthMM) : base(llm, RT.rPointFromMM(rWidthMM)) {
      }
    }
    #endif
  }
}
