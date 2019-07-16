# Report.NET
The Report.NET class library is used to generate precise PDF documents. The code is written in C# for the .NET platform and
includes sample code for ASP.NET that can be used to create dynamic PDF-response pages.

This project is a fork of Otto Mayer's Report.NET class library that was originally posted on Source Forge
under http://sourceforge.net/projects/report/.

So far this fork has made no changes to the code and simply exists to create NuGet packages for various flavors of .NET: 
https://www.nuget.org/packages/Report.NET/.

If you are looking for other options for open source PDF generation, e.g., non-LGPL licensed, this C# library developed by Uzi Granot looks pretty good: https://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version-1-25 - it uses a [CPOL](https://www.codeproject.com/info/cpol10.aspx) license.

Although you can usually use the original .NET 2.0 DLL downloaded from SourceForge in later versions of .NET without issues, under some circumstances race conditions will occur when loading the .NET 2.0 framework side-by-side within a later version of .NET when executing Report.NET code that causes unhandled exceptions. This repository exposes all .NET versions from 2.0 to 4.8 and the stated exceptions do not occur when using a version of the Report.NET assembly that is compiled against the same .NET version as the deployed application.

## Project Description

Report.NET is a powerful library that will help you to generate PDF documents in a simple and flexible manner. The document
can be created with data that has been retrieved from any ADO.NET data set. The Report.NET library is available for free
under the LGPL license.

Start Samples with Visual Studio .NET

1. Create a folder for the Report.NET project e.g. C:\Report.NET
2. Copy folder ReportSamples to C:\Report.NET\ReportSamples
3. Copy folder Root to C:\Report.NET\Root
4. Copy Inetpub\wwwroot\ReportSamplesASP to C:\Inetpub\wwwroot\ReportSamplesASP
5. Start the administration panel of the IIS
5. Select "Internet Information Services Start" -> "YourPC (local computer)" -> "Default Web Site" -> ReportSamplesASP
6. Open the properties dialog of ReportSamplesASP
7. Click on the "Create" button and check the "Read" box
8. Open the solution C:\Report.NET\ReportSamples\ReportSamples.sln
 
## Samples

The executable program files (.exe) of all samples are in ReportSamples\bin. The samples can be executed directly if the
Microsoft .NET Framework has been installed.

| Example | Rendered PDF | Code |
| --- | --- | --- |
| Flow Layout Manager Sample | [FlowLayoutManagerSample.pdf](../master/ReportSamples/pdf/FlowLayoutManagerSample.pdf) | [FlowLayoutManagerSample.cs](../master/ReportSamples/Samples/FlowLayoutManagerSample.cs) |
| Font Test |	[FontTest.pdf](../master/ReportSamples/pdf/Test.pdf) | [FontTest.cs](../master/ReportSamples/Samples/Test.cs) |
| Image Sample | [ImageSample.pdf](../master/ReportSamples/pdf/ImageSample.pdf) | [ImageSample.cs](../master/ReportSamples/Samples/ImageSample.cs) |
| List Layout Manager Sample | [ListLayoutManagerSample.pdf](../master/ReportSamples/pdf/ListLayoutManagerSample.pdf) | [ListLayoutManagerSample.cs](../master/ReportSamples/Samples/ListLayoutManagerSample.cs) |
| PDF-Properties Sample | [PdfPropertiesSample.pdf](../master/ReportSamples/pdf/PdfPropertiesSample.pdf) | [PdfPropertiesSample.cs](../master/ReportSamples/Samples/PdfPropertiesSample.cs) |
| Table Layout Manager Sample C# | [TableLayoutManagerSample.pdf](../master/ReportSamples/pdf/TableLayoutManagerSample.pdf) | [TableLayoutManagerSample.cs](../master/ReportSamples/Samples/TableLayoutManagerSample.cs) |
| Table Layout Manager Sample VB | [TableLayoutManagerSampleVB.pdf](../master/ReportSamples/pdf/TableLayoutManagerSample.pdf) | [TableLayoutManagerSample.cs](../master/ReportSamples/TableLayoutManagerSampleVB/TableLayoutManagerSample.vb) |
| Text Sample | [TextSample.pdf](../master/ReportSamples/pdf/TextSample.pdf) | [TextSample.cs](../master/ReportSamples/Samples/TextSample.cs) |
| Start Template C# | [StartTemplate.pdf](../master/ReportSamples/pdf/StartTemplate.pdf) | [StartTemplate.cs](../master/ReportSamples/StartTemplate/StartTemplate.cs) |
| Start Template VB | [StartTemplateVB.pdf](../master/ReportSamples/pdf/StartTemplateVB.pdf) | [StartTemplateVB.vb](../master/ReportSamples/StartTemplateVB/StartTemplateVB.vb) |

## Requirements

* In order to be able to use the Report.NET library you should be acquainted with the .NET framework and C#.
* You will need the following software:
  * Microsoft .NET Framework SDK
  * Adobe Acrobat Reader from Adobe Systems Incorporated:
  * Microsoft Visual Studio .NET (recommended)
  * Microsoft IIS (for ASP.NET samples)
  
(c) 2002 by root-software ag, switzerland
