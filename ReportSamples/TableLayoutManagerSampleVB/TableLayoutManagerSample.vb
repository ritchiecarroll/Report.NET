Imports Root.Reports
Imports System
Imports System.IO
Imports System.Data
Imports System.Drawing

' Creation date: 26.09.2003
' Checked: 08.03.2006
' Author: Otto Mayer (mot@root.ch), Thomas Gemperle (gth@root.ch)
' Version: 1.05

' Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
' This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
' as published by the Free Software Foundation, version 2.1 of the License.
' This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
' should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
' Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

' Table Layout Manager and Data-Set Sample (VB Version)
Module TableLayoutManagerSampleModule
  ' Starts this sample.
  Sub Main()
    RT.ViewPDF(New TableLayoutManagerSampleVB, "TableLayoutManagerSampleVB.pdf")
  End Sub
End Module

' Table Layout Manager and Data-Set Sample (VB Version)
' This sample creates a table with data from a data set.
' The table layout manager automatically builds the grid lines, the header of the table and the page breaks, etc.
' The event handler Tlm_NewContainer binds each table container to a new page.
' The first page has a caption. The following pages have no caption and therefore the table can be made higher.
Public Class TableLayoutManagerSampleVB
  Inherits Report

  Private fontDef_Helvetica As FontDef
  Private rPosLeft As Double = 20
  Private rPosRight As Double = 195
  Private rPosTop As Double = 24
  Private rPosBottom As Double = 278

  ' Creates this report.
  Protected Overrides Sub Create()
    fontDef_Helvetica = New FontDef(Me, FontDef.StandardFont.Helvetica)
    Dim fontProp_Text As FontProp = New FontPropMM(fontDef_Helvetica, 1.9)  ' standard font
    Dim fontProp_Header As FontProp = New FontPropMM(fontDef_Helvetica, 1.9)  ' font of the table header
    fontProp_Header.bBold = True

    Dim stream_Phone As Stream = Me.GetType().Assembly.GetManifestResourceStream("ReportSamples.Phone.jpg")
    Dim random As Random = New Random(6)

    ' create table
    Dim tlm As TableLayoutManager = New TableLayoutManager(fontProp_Header)
    Try
      tlm.rContainerHeightMM = rPosBottom - rPosTop  ' set height of table
      tlm.tlmCellDef_Header.rAlignV = RepObj.rAlignCenter  ' set vertical alignment of all header cells
      tlm.tlmCellDef_Default.penProp_LineBottom = New PenProp(Me, 0.05, Color.LightGray)
      tlm.tlmHeightMode = TlmHeightMode.AdjustLast
      AddHandler tlm.eNewContainer, AddressOf Tlm_NewContainer

      ' define columns
      Dim col As TlmColumn
      col = New TlmColumnMM(tlm, "ID", 13)

      col = New TlmColumnMM(tlm, "Company Name", 40)
      col.tlmCellDef_Default.tlmTextMode = TlmTextMode.MultiLine

      col = New TlmColumnMM(tlm, "Address", 36)

      col = New TlmColumnMM(tlm, "City", 22)

      col = New TlmColumnMM(tlm, "Postal Code", 16)

      col = New TlmColumnMM(tlm, "Country", 18)

      Dim col_Phone As TlmColumn = New TlmColumnMM(tlm, "Phone", rPosRight - rPosLeft - tlm.rWidthMM)
      col_Phone.fontProp_Header = New FontPropMM(fontDef_Helvetica, 1.9, Color.Brown)
      col_Phone.tlmCellDef_Header.rAlignH = RepObj.rAlignRight
      col_Phone.tlmCellDef_Default.rAlignH = RepObj.rAlignRight
      Dim brushProp_Phone As BrushProp = New BrushProp(Me, Color.FromArgb(255, 255, 200))
      col_Phone.tlmCellDef_Default.brushProp_Back = brushProp_Phone
      Dim brushProp_USA As BrushProp = New BrushProp(Me, Color.FromArgb(255, 180, 180))

      ' open data set
      Dim ds As DataSet = New DataSet
      Dim stream_Customers As Stream = Me.GetType().Assembly.GetManifestResourceStream("ReportSamples.Customers.xml")
      ds.ReadXml(stream_Customers)
      stream_Customers.Close()
      Dim dt As DataTable = ds.Tables(0)

      Dim dr As DataRow
      For Each dr In dt.Rows
        Dim sCountry As String = dr("Country").ToString()
        If sCountry = "USA" Then
          tlm.tlmCellDef_Default.brushProp_Back = brushProp_USA
          col_Phone.tlmCellDef_Default.brushProp_Back = New BrushProp(Me, Color.FromArgb(255, 227, 50))
        Else
          tlm.tlmCellDef_Default.brushProp_Back = Nothing
          col_Phone.tlmCellDef_Default.brushProp_Back = brushProp_Phone
        End If
        tlm.NewRow()
        tlm.Add(0, New RepString(fontProp_Text, dr("CustomerID").ToString()))
        tlm.Add(1, New RepString(fontProp_Text, dr("CompanyName").ToString()))
        tlm.Add(2, New RepString(fontProp_Text, dr("Address").ToString()))
        tlm.Add(3, New RepString(fontProp_Text, dr("City").ToString()))
        tlm.Add(4, New RepString(fontProp_Text, dr("PostalCode").ToString()))
        tlm.Add(5, New RepString(fontProp_Text, sCountry))
        tlm.Add(6, New RepString(fontProp_Text, dr("Phone").ToString()))
        If random.NextDouble() < 0.2 Then  ' mark randomly selected row with a phone icon
          tlm.tlmRow_Cur.aTlmCell(col_Phone).AddMM(1, 0.25, New RepImageMM(stream_Phone, 2.1, 2.3))
        End If
      Next dr

      ' print page number and current date/time
      ' VS 2003: For Each page As Page In enum_Page
      Dim rY As Double = rPosBottom + 1.5
      Dim page As Page
      For Each page In enum_Page
        page.AddLT_MM(rPosLeft, rY, New RepString(fontProp_Text, DateTime.Now.ToShortDateString() + "  " + DateTime.Now.ToShortTimeString()))
        page.AddRT_MM(rPosRight, rY, New RepString(fontProp_Text, page.iPageNo.ToString() + " / " + iPageCount.ToString()))
      Next
    Finally
      tlm.Close()
    End Try
  End Sub

  ' Creates a new page.
  Private Sub Tlm_NewContainer(ByVal oSender As Object, ByVal ea As TableLayoutManager.NewContainerEventArgs)
    Dim page As New Page(Me)

    ' first page with caption
    If page_Cur.iPageNo = 1 Then
      Dim fontProp_Title As FontProp = New FontPropMM(fontDef_Helvetica, 7)
      fontProp_Title.bBold = True
      page_Cur.AddCT_MM(rPosLeft + (rPosRight - rPosLeft) / 2, rPosTop, New RepString(fontProp_Title, "Customer List"))
      ea.container.rHeightMM -= fontProp_Title.rLineFeedMM  ' reduce height of table container for the first page
    End If

    ' the new container must be added to the current page
    page_Cur.AddMM(rPosLeft, rPosBottom - ea.container.rHeightMM, ea.container)
  End Sub
End Class
