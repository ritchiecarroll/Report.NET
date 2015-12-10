Imports Root.Reports
Imports System

' Creation date: 23.09.2003
' Checked: 14.02.2006
' Author: Otto Mayer (mot@root.ch), Thomas Gemperle (gth@root.ch)
' Version: 1.05

' Report.NET copyright © 2002-2006 root-software ag, Bürglen Switzerland - Otto Mayer, Stefan Spirig, all rights reserved
' This library is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License
' as published by the Free Software Foundation, version 2.1 of the License.
' This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details. You
' should have received a copy of the GNU Lesser General Public License along with this library; if not, write to the Free Software
' Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA www.opensource.org/licenses/lgpl-license.html

Namespace ReportSamples
  Module StartTemplateVB
    '------------------------------------------------------------------------------------------30.10.2004
    ' Starts the "Start Template" sample.
    Sub Main()
      RT.ViewPDF(New StartTemplate, "StartTemplateVB.pdf")
    End Sub
  End Module

  '------------------------------------------------------------------------------------------30.10.2004
  ' Start Template (VB Version)
  Class StartTemplate
    Inherits Report

    '------------------------------------------------------------------------------------------30.10.2004
    ' Creates this report.
    Protected Overrides Sub Create()
      Dim fd As FontDef = New FontDef(Me, FontDef.StandardFont.Helvetica)
      Dim fp As FontProp = New FontPropMM(fd, 20)
      Dim page As page = New page(Me)
      page.AddCB_MM(80, New RepString(fp, "Start Template"))
    End Sub

  End Class
End Namespace