<%@ Page Language="C#"  MasterPageFile="~/LayOut.Master" AutoEventWireup="true" CodeBehind="Trace.aspx.cs" Inherits="Trans.Web.Display.Trace" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<title>同步系统跟踪日志</title>
</head>
<body>
<form id="form1" runat="server">
<asp:ScriptManager ID="ScriptMaganger1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<table width="100%" border="1" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            站点<asp:DropDownList ID="SiteList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SiteNameList_SelectedIndexChanged"></asp:DropDownList>
            年<asp:DropDownList ID="YearList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="YearList_SelectedIndexChanged"></asp:DropDownList>
            月<asp:DropDownList ID="MonthList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="MonthList_SelectedIndexChanged"></asp:DropDownList>
            日<asp:DropDownList ID="DayList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DayList_SelectedIndexChanged"></asp:DropDownList>
            批次<asp:DropDownList ID="BlockList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="BlockList_SelectedIndexChanged"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="TGridView" Width="98%" Caption="跟踪日志" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="15" OnPageIndexChanging="TGridView_PageIndexChanging" OnRowDataBound="TGridView_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="编号" ItemStyle-Width="55px" />
                    <asp:BoundField DataField="Title" HeaderText="标题" ItemStyle-Width="280px" />
                    <asp:BoundField DataField="Site" HeaderText="站点" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="Description" HeaderText="描述" ItemStyle-Wrap="true" />
                    <asp:BoundField DataField="CreateTime" HeaderText="时间" ItemStyle-Width="170px" />
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
</ContentTemplate>
</asp:UpdatePanel>
</form>
</body>
</html>

    </asp:Content>