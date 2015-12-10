<%@ Page Language="VB" Debug="true" ContentType="application/pdf" %>
<%@ Import namespace="Root.Reports" %>
<%@ Import namespace="ReportSamples" %>
<script runat="server" language="VB">
  Sub Page_Load(Sender As Object, E As EventArgs)
    RT.ResponsePDF(New ReportSamples.TableLayoutManagerSampleVB, Me)
  End sub
</script>
