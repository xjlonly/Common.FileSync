<%@ Page Language="C#"  MasterPageFile="~/LayOut.Master"  AutoEventWireup="true" CodeBehind="Monitor.aspx.cs" Inherits="Trans.Web.Display.Monitor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>U上传监控器</title>
<style type="text/css">
*{font-size:12px; font-family:arial;}
table{background-color:#333;}
table tr th{background-color:#eee;}
table tr td{background-color:#fff;}
table tr.out td{background-color:#fff;}
table tr.over td{background-color:#eee;}
.s1{color:#FF9900;}
.s2{color:green;}
.s3{color:black;}
.s-1{color:red;}
.s-2{color:red;}
.s-3{color:red;}
</style>
</head>
<body>
<form id="MyForm" runat="server">
    <div >
        批次：<%=block %><br/><br/>
		<asp:LinkButton ID="lkcancel" runat="server" OnClick="lkcancel_Click">取消上传</asp:LinkButton> 
		说明：用户取消将在上传完成或者备份完成时执行。在开始覆盖时取消无效.
    </div>
    <br/>
    <div id="Status">
    <%=Status%>
    </div>
    <div id="content">
    <%=mainContent %>
    </div>
    <iframe src="Saver.aspx?block=<%=block %>" style="display:none;"></iframe>
</form>
</body>
</html>
    
    </asp:Content>