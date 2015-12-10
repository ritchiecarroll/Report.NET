<%@ Page Language="c#" Debug="true" ContentType="application/pdf" %>
<%@ Import namespace="Root.Reports" %>
<%@ Import namespace="ReportSamples" %>
<script runat="server" language="C#">
  void Page_Load() {
    PdfReport<TableLayoutManagerSampleVB> pdfReport = new PdfReport<TableLayoutManagerSampleVB>();
    pdfReport.Response(this);
  }
</script>
