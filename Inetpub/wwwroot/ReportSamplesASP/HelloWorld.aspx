<%@ Page Language="c#" Debug="true" ContentType="application/pdf" %>
<%@ Import namespace="Root.Reports" %>
<script runat="server" language="C#">
  void Page_Load() {
    PdfReport<HelloWorld> pdfReport = new PdfReport<HelloWorld>();
    pdfReport.Response(this);
  }

  private class HelloWorld : Report {
    protected override void Create() {
      FontDef fontDef = FontDef.fontDef_FromName(this, FontDef.StandardFont.Helvetica);
      FontProp fontProp = new FontPropMM(fontDef, 20);
      new Root.Reports.Page(this);
      page_Cur.AddCB_MM(80, new RepString(fontProp, "Hello World"));
    }
  }
</script>
