<%@ Page Language="C#"  MasterPageFile="~/LayOut.Master"  AutoEventWireup="true" CodeBehind="Saver.aspx.cs" Inherits="Trans.Web.Display.Saver" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script>
this.parent.document.getElementById('content').innerHTML='<%=mainContent.Replace("\'","\\\'") %>';
this.parent.document.getElementById('Status').innerHTML='<%=Status %>';
</script>
<%=thiscontent %>
    </asp:Content>