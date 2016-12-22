<%@ Page Language="C#"  MasterPageFile="~/LayOut.Master"  AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="Trans.Web.Display.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>用户列表</title>
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

.tc{
position: fixed;
    left: 50%;
    top: 50%;
    background: #fff;
    border: solid 1px #ccc;
    -moz-box-shadow: 0 15px 50px #666;
    box-shadow: 0 15px 50px #666;
    -moz-border-radius: 3px;
    border-radius: 3px;
    overflow: hidden;
}
</style>
</head>
<body>
    <div>
    <%=ListStr %>
   </div>
   <input type="button" value="添加用户"   onclick ="Add();" />
   <br />
   <br />
 <form method="post" runat="server" >
    <div id="res" class="tc" style=" width: 300px; margin-top: -9%; margin-left: -13%; height: 250px; text-align: left;display:none;  " >
                 <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;姓名：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="text" name="UserName" /><br />
        <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;密码：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type="password" name="Password" /><br />
        <br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;确认密码：<input type="password" name="SruePwd" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
        <br />
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input style="align-content:center" type="submit" id="Sub" value="添加" />
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input style="align-content:center" type="button" onclick="$('#res').hide()"  value="取消" /><br /><br />
    </div></form>
        
    <br />
</body>
</html>
<script type="">
    function Add()
    {
        document.getElementById("res").style.display = "block";
      
    }
</script>
    
    </asp:Content>
