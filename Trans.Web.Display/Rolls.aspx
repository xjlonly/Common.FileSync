<%@ Page Language="C#" MasterPageFile="~/LayOut.Master"  AutoEventWireup="true" CodeBehind="Rolls.aspx.cs" Inherits="Trans.Web.Display.Roll" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>同步系统回滚</title>
</head>
<body>



<form id="MyForm" method="post" action="Rolls.aspx">
    <table width="100%" border="1" cellspacing="0" cellpadding="0">
        <caption style="color:Red;">备份回滚</caption>
        <tr>
            <td>
                <%=BakDateLab %>
                

            </td>
        </tr>
        <tr>
            <td>
                <%=BakMarkLab %>
            </td>
        </tr>
    </table>
    <input type="hidden" id="RollBackMark" value="" name="RollBackMark" />
</form>
<script type="text/javascript" language="javascript">
    function SelBakSite() {
            document.getElementById("RollBackMark").value = '';
            document.getElementById("MyForm").submit();
        
    }

function SelBakDate()
{
        document.getElementById("RollBackMark").value='';
        document.getElementById("MyForm").submit();
}
function RollBack(rid)
{
    document.getElementById("RollBackMark").value=rid;
    document.getElementById("MyForm").submit();
}
function ViewRBOp(rid)
{
    window.open("Rollback.aspx?RID="+encodeURIComponent(rid));
}
</script>
</body>
</html>

</asp:Content>