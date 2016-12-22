<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileServer.aspx.cs" Inherits="Trans.Web.Display.FileServer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
  <title>Index of /</title>
  <style>
    body {
        font-family: "Segoe UI", "Segoe WP", "Helvetica Neue", 'RobotoRegular', sans-serif;
        font-size: 14px;}
    header h1 {
        font-family: "Segoe UI Light", "Helvetica Neue", 'RobotoLight', "Segoe UI", "Segoe WP", sans-serif;
        font-size: 28px;
        font-weight: 100;
        margin-top: 5px;
        margin-bottom: 0px;}
    #index {
        border-collapse: separate; 
        border-spacing: 0; 
        margin: 0 0 20px; }
    #index th {
        vertical-align: bottom;
        padding: 10px 5px 5px 5px;
        font-weight: 400;
        color: #a0a0a0;
        text-align: center; }
    #index td { padding: 3px 10px; }
    #index th, #index td {
        border-right: 1px #ddd solid;
        border-bottom: 1px #ddd solid;
        border-left: 1px transparent solid;
        border-top: 1px transparent solid;
        box-sizing: border-box; }
    #index th:last-child, #index td:last-child {
        border-right: 1px transparent solid; }
    #index td.length, td.modified { text-align:right; }
    a { color:#1ba1e2;text-decoration:none; }
    a:hover { color:#13709e;text-decoration:underline; }
  </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <section id="main">
    <header><h1>Index of <a href="/">/</a></h1></header>
    <table id="index" summary="The list of files in the given directory.  Column headers are listed in the first row.">
    <thead>
      <tr><th abbr="Name">Name</th><th abbr="Size">Size</th><th abbr="Modified">Last Modified</th></tr>
    </thead>
    <tbody>
        <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <tr class="directory">
                    <td class="name"><%#  ((Trans.Web.Display.FileStruct)Container.DataItem).IsDirectory ?  "<a href=\"./BaiduYunDownload/\">" + ((Trans.Web.Display.FileStruct)Container.DataItem).Name + "</a>" : ((Trans.Web.Display.FileStruct)Container.DataItem).Name %> </td>
                    <td><%#((Trans.Web.Display.FileStruct)Container.DataItem).Size %></td>
                    <td class="modified"><%#((Trans.Web.Display.FileStruct)Container.DataItem).CreateTime%></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </tbody>
    </table>
  </section>
    </div>
    </form>
</body>
</html>
