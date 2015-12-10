# Report.NET
The Report.NET class library used to generate precise PDF documents. The code is written in C# for the .NET platform and include sample code for ASP.NET that can be used to create dynamic PDF-response pages.

This project is a fork of Otto Mayer's Report.NET class library that was originally posted on Source Forge under http://sourceforge.net/projects/report/.

So far this fork has made no changes to the code and simply exists to create NuGet packages for various flavors of .NET.

Although you can usually use the original .NET 2.0 DLL in later versions of .NET without issues, under some confitions there can be race conditions when loading the .NET 2.0 framework within a later version of .NET and starting to use the Report.NET code that can cause unhandled exceptions. These exceptions go away when using a version of Reports.NET compiled against the same .NET version as the deployed application.

## Project Description

Report.NET is a powerful library that will help you to generate PDF documents in a simple and flexible manner. The document can be created with data that have been retrieved from any ADO.NET data set. The Report.NET library is available for free under the LGPL license.
Start Samples with Visual Studio .NET

1. Create a folder for the Report.NET project e.g. C:\Report.NET
2. Copy folder ReportSamples to C:\Report.NET\ReportSamples
3. Copy folder Root to C:\Report.NET\Root
4. Copy Inetpub\wwwroot\ReportSamplesASP to C:\Inetpub\wwwroot\ReportSamplesASP
5. Start the administration panel of the IIS
5. Select "Internet Information Services Start" -> "YourPC (local computer)" -> "Default Web Site" -> ReportSamplesASP
6. Open the properties dialog of ReportSamplesASP
7. Klick on the "Create" button and check the "Read" box
8. Open the solution C:\Report.NET\ReportSamples\ReportSamples.sln
 
## Samples

The executable program files (.exe) of all samples are in ReportSamples\bin. The samples can be executed directly if the Microsoft .NET Framework has been installed.

| Example | Rendered PDF | Code |
| --- | --- | --- |
| ADO Sample VB	| AdoSampleVB.pdf | AdoSample.vb |
| Flow Layout Manager Sample | FlowLayoutManagerSample.pdf | FlowLayoutManagerSample.cs |
| Font Test |	FontTest.pdf | FontTest.cs |
| Hello World C# | HelloWorld.pdf |	HelloWorld.cs |
| Hello World VB | HelloWorldVB.pdf | HelloWorldVB.vb |
| Image Sample | ImageSample.pdf | ImageSample.cs |
| List Layout Manager Sample | ListLayoutManagerSample.pdf | ListLayoutManagerSample.cs |
| PDF-Properties Sample | PdfPropertiesSample.pdf | PdfPropertiesSample.cs |
| Table Layout Manager Sample | TableLayoutManagerSample.pdf | TableLayoutManagerSample.cs |
| Text Sample | TextSample.pdf | TextSample.cs |
| Start Template C# | StartTemplate.pdf | StartTemplate.cs |
| Start Template VB | StartTemplateVB.pdf | StartTemplateVB.vb |

## Requirements

* In order to be able to use the Report.NET library you should be acquainted with the .NET framework and C#.
* You need the following software:
  * Microsoft .NET Framework SDK
  * Adobe Acrobat Reader from Adobe Systems Incorporated:
  * Microsoft Visual Studio .NET (recommended)
  * Microsoft IIS (for ASP.NET samples)
  
(c) 2002 by root-software ag, switzerland