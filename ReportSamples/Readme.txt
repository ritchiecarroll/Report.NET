Report.NET

0.09.xx (will come soon)
- Documentation of samples
- support for TTF fonts

0.09.06 Beta

0.09.05 Beta
- new class FontPropPoint
- improved ASP-Sample
- new method FontProp.sGetTextLine for multiline text
- new method FontProp.fontProp_GetBestFit to get the best fitting font size

0.09.03 Beta
- ASP-Sample for VB

0.09.02 Beta
- FontProp.sTruncate: improved performance
- FontProp: bold and italic style corrected
- FontProp.rLineFeed can be used to change the line feed

0.09.01 Beta
- new ASP.NET samples for the .NET Framework 2.0

0.09.00 Beta
- VS 2005 / .NET Framework 2.0
- new kernel: improved indirect objects (used for TTF)
- Batch file to compile Reports.dll
- Use conditional compilation symbol "Compatible_0_8" for compatibility with Report.NET 0.08

0.08.01
- supports Visual Studio 2003
- improved ASP.NET samples with documentation
- Start of Acrobat Reader changed
- HelloWorld sample, StartTemplate sample documented
- Table Layout Manager: error in header row corrected
- Table Layout Manager: invalid page break in row corrected
- afm-files: conversion of numbers throws an exception with certain culture settings
- new Class "DebugTools"
- ReportException changed to "internal"

0.08.00
- Support for arcs, circles, ellipses and pie shapes
- improved samples
- StaticContainer: new method AddCB (AddCentered is now obsolete)
- TlmRow: new property "pp_LineH" that sets the horizontal lines of the row
- TlmColumn: new method "AddLT"

0.07.04
- Enumeration for pdf standard fonts
- Multipage tiff support
- Documents can be created many times (corrected)
- VB compatible properties
- VB samples

0.07.03
- Print directly without PDF-Viewer: RT.PrintPDF
- Table layout manager: preferred hight can be set for a row 
- tiff support CCIT 3 + 4
- optimized memory allocation for images

0.07.02
- TlmColumn: new property "rPosX"
- TlmBase: new method tlmRow_New()
- TlmBase.NewLineMM(...): conversion to metric value corrected
- Table Layout Manager: vertical text
- Column with more than one object in one cell: corrected for centered and right justified objects
- Documentation
- Italic fonts

0.07.01
- Method 'TlmClumn.Add': Current x-position has not been moved by the width of the object
- Support for 8-bit gray scale images
- Sample how to set the width and height of a page (Page.cs)
- improved TableLayoutManager

0.07.00 (30.05.2003)
- new TableLayoutManager (not fully compatible with the previous version)
  !!! Important !!!
  A handle of the table layout manager is now required for the column definition.
  instead of:  col = new TableLayoutManager.Column("City", 22);
               tlm.Add(col);
  now:         col = new TableLayoutManager.Column(tlm, "City", 22);
- removed: TableLayoutManager.rHeaderHeight
           TableLayoutManager.repLine_RowSeparator

0.06.00
- namespace changed: Reports
- Name of report objects changed (Doc ==> Rep)
- Table Layout Manager
- Constructor of RepImage with stream

0.05.00
- variable for current page: RsfDoc.page_Cur
- new pdf page and font management
- Flow Layout Manager

0.04.05
- ASP.NET-Sample

0.04.04
- PDF properties sample
- some PDF properties can be set:
  - Title, Author, Subject, Keywords
  - Creator, Creation Date
  - Page-Layout (Single Page, One Column, Two Column Left, Two Column Right)
  - Hide Tool Bar, Hide Menu Bar, Hide Window UI
  - Fit Window
  - Center Window
  - Display Document Title

0.04.03
- correction for regional settings other than "en-US"

0.04.02
- new sample of a standard frame for a report
- improved character mapping
- font metrics test
- new FontProp.sTruncateText: Truncates the text to the specified width and adds three dots if necessary.

0.04.01
- new ReadMe.html
- lines can be rotated correctly

0.04.00
- new Image Sample
- improved call framework (all samples have been updated)
- invalid error messages on console removed
- new function to get the width of a string (see TextSample.cs)

Tasks:
- Shapes (rectangles with rounded corners, circle)
- Table layout manager size-optimizer
- Support for PDF links
- Bar-Codes
- FontProp.rHeight() same as FontProp.rSize
- Polygone
- Win-Formatter (output directly to printer)

What Report.NET can not:
- Kerning
- Embedding of Type1 fonts
- CJK font
- Arabic text
