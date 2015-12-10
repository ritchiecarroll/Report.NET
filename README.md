# Report.NET
The Report.NET class library is used to generate precise PDF documents. The code is written in C# for the .NET platform and
includes sample code for ASP.NET that can be used to create dynamic PDF-response pages.

This project is a fork of Otto Mayer's Report.NET class library that was originally posted on Source Forge
under http://sourceforge.net/projects/report/.

So far this fork has made no changes to the code and simply exists to create NuGet packages for various flavors of .NET.

Although you can usually use the original .NET 2.0 DLL in later versions of .NET without issues, under some circumstances
race conditions will occur when loading the .NET 2.0 framework side-by-side within a later version of .NET when executing
Report.NET code that causes unhandled exceptions. These exceptions do not occur when using a version of the Reports.NET
assembly that is compiled against the same .NET version as the deployed application.

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
| ADO Sample VB	| [AdoSampleVB.pdf](../blob/master/ReportSamples/bin/AdoSampleVB.pdf) | [AdoSample.vb](../blob/master/ReportSamples/AdoSampleVB/AdoSample.vb) |
| Flow Layout Manager Sample | [FlowLayoutManagerSample.pdf](../blob/master/ReportSamples/bin/FlowLayoutManagerSample.pdf) | [FlowLayoutManagerSample.cs](../blob/master/ReportSamples/SamplesDLL/FlowLayoutManagerSample.cs) |
| Font Test |	[FontTest.pdf](../blob/master/ReportSamples/bin/FontTest.pdf) | [FontTest.cs](../blob/master/ReportSamples/SamplesDLL/FontTest.cs) |
| Hello World C# | [HelloWorld.pdf](../blob/master/ReportSamples/bin/HelloWorld.pdf) | [HelloWorld.cs](../blob/master/ReportSamples/HelloWorld/HelloWorld.cs) |
| Hello World VB | [HelloWorldVB.pdf](../blob/master/ReportSamples/bin/HelloWorldVB.pdf) | [HelloWorldVB.vb](../blob/master/ReportSamples/HelloWorldVB/HelloWorldVB.vb) |
| Image Sample | [ImageSample.pdf](../blob/master/ReportSamples/bin/ImageSample.pdf) | [ImageSample.cs](../blob/master/ReportSamples/SamplesDLL/ImageSample.cs) |
| List Layout Manager Sample | [ListLayoutManagerSample.pdf](../blob/master/ReportSamples/bin/ListLayoutManagerSample.pdf) | [ListLayoutManagerSample.cs](../blob/master/ReportSamples/SamplesDLL/ListLayoutManagerSample.cs) |
| PDF-Properties Sample | [PdfPropertiesSample.pdf](../blob/master/ReportSamples/bin/PdfPropertiesSample.pdf) | [PdfPropertiesSample.cs](../blob/master/ReportSamples/SamplesDLL/PdfPropertiesSample.cs) |
| Table Layout Manager Sample | [TableLayoutManagerSample.pdf](../blob/master/ReportSamples/bin/TableLayoutManagerSample.pdf) | [TableLayoutManagerSample.cs](../blob/master/ReportSamples/SamplesDLL/TableLayoutManagerSample.cs) |
| Text Sample | [TextSample.pdf](../blob/master/ReportSamples/bin/TextSample.pdf") | [TextSample.cs](../blob/master/ReportSamples/SamplesDLL/TextSample.cs) |
| Start Template C# | [StartTemplate.pdf](../blob/master/ReportSamples/bin/StartTemplate.pdf) | [StartTemplate.cs](../blob/master/ReportSamples/StartTemplate/StartTemplate.cs) |
| Start Template VB | [StartTemplateVB.pdf](../blob/master/ReportSamples/bin/StartTemplateVB.pdf) | [StartTemplateVB.vb](../blob/master/ReportSamples/StartTemplateVB/StartTemplateVB.vb) |

## Requirements

* In order to be able to use the Report.NET library you should be acquainted with the .NET framework and C#.
* You will need the following software:
  * Microsoft .NET Framework SDK
  * Adobe Acrobat Reader from Adobe Systems Incorporated:
  * Microsoft Visual Studio .NET (recommended)
  * Microsoft IIS (for ASP.NET samples)
  
(c) 2002 by root-software ag, switzerland