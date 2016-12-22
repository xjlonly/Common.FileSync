<%@ Page Language="C#" MasterPageFile="~/LayOut.Master" AutoEventWireup="true" CodeBehind="Grant.aspx.cs" Inherits="Trans.Web.Display.Grant" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="/JS/jquery.1.9.1.min.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    
<% if(CanAcce){ 
        %><a href="javascript:history.go(-1);"> <<返回上一页</a>
    <br /> <br />
    <form id="form1" runat="server" style="align-content:center">
       
         <div style="align:center">  <p>当前授权人信息：</p>
             用户名：<%=NUser_Info.UserName %>
             邮箱：<%=NUser_Info.RefUserId %>
    </div>
        <br />
         <br />
        <p>权限列表：</p>
            <!--加载树-->
        <div id="JsonTree" class="demo" style=" margin-bottom:20px; margin-left:10px; margin-right:50px; float:left; width:50% ">
        <table   border="1px" cellpadding="0" cellspacing="0"  width="60%">
            <thead>
            <tr>
                <td style=" min-width:100px">站点名称</td>
                <td style=" min-width:100px">
                    查看列表权限
                </td>
                <td style=" min-width:100px">
                    同步权限
                </td>
                <td style=" min-width:100px">
                    回滚权限
                </td>
                <td style=" min-width:100px">操作</td>
            </tr>
            </thead>
           <tbody id="tbody">
               <%=WebListText %>
               </tbody>
        </table>
        </div>
   
    </form>
    <%} %>
   
</body>
</html> 
<script type="text/javascript">
    function Save(obj)
    {
        var UserID =<%=NUser_Info.UserId%> ;
        var WebName = $(obj).parent().siblings()[0].innerText;
        var AllowList = $(obj).parent().siblings()[1].childNodes[0].checked == true ? "1" : "0";
        var AllowSync = $(obj).parent().siblings()[2].childNodes[0].checked == true ? "1" : "0";
        var AllowRoll = $(obj).parent().siblings()[3].childNodes[0].checked == true ? "1" : "0";
       
        $.ajax({
            url: "AuthSave.ashx",
            data: { WebName: WebName, AllowList: AllowList, AllowSync: AllowSync, AllowRoll: AllowRoll, UserID: UserID },
            type: "post",
            success: function (_data) {
                alert(_data);
            }
        });
    }
</script>
    
    </asp:Content>
<%--<script type="text/javascript">
      //默认选中节点
        function CheckedNode(){            
        // $("#JsonTree").jstree("check_node", "<%=SelectedItem %>");
            var arry='<%=SelectedItem %>';
            var checkNode=arry.split(',');
            $('#JsonTree').find('li').each(function(){
               // alert($(this).attr('id'));
               var idAttr=$(this).attr('id').split(";;");
                for (var i = 0; i <checkNode.length; i++) {      
                    if(checkNode[i].indexOf( idAttr[0])!=-1 && checkNode[i].indexOf( ";;"+idAttr[1])!=-1){
                        $('#JsonTree').jstree('open_node',$(this));
                    }
                    if ($(this).attr('id') == checkNode[i]){
                        $('#JsonTree').jstree('check_node',$(this));
                    }
                }
            });
        }
        $('#JsonTree').jstree({
           'core' : {
               'data': [
               <%=SiteAuthor %>
            ]
		},
            plugins: ["themes", "json_data", "checkbox", "ui"]   //加载插件
        })
        .bind("loaded.jstree", function(e,data){  $('#JsonTree').jstree('open_node',$('li[id="-1"]'))})
        .bind("after_open.jstree", function(e,data){CheckedNode();});

        //全选
        $('#AllCheck').click(function(){
            var checked=$(this).prop('checked');
            $('#tbody [type="checkbox"]').prop("checked",checked);
        });
        //获得被选中项的站点文件，以‘,’分开
        function get_authchecked()
        {
            var authchecked = "";
            var nodes=$("#JsonTree").jstree("get_checked"); //使用get_checked方法 
            $.each(nodes, function(i, n) { 
            authchecked += n+",";
            }); 
//            $("li[aria-selected='true']").each(function(i, element){
//                    authchecked = authchecked + $(element).attr("id") + ",";  //jQuery代码获得以空格隔开的checkbox被选中的项的ID串
//            });
            //获得该站点下的
            return authchecked;
        }
        //获得选中的用户ID
        function get_userChecked(){
             var userChecked="";
            $('#tbody input:checked').each(function(i,element){
                  userChecked = userChecked + $(element).val() + ",";  //jQuery代码获得以空格隔开的checkbox被选中的项的ID串
           });
           return userChecked;
        }
        //给选中的用户添加站点文件权限
        $('#addAuthor').click(function(){
            var userChecked=get_userChecked();
            var authchecked=get_authchecked();
            $.ajax({
                url:"Auth.ashx",
                data:{uid: userChecked, aid: authchecked},
                type: "post",
                success:function(_data){
                    alert(_data);
                    window.parent.location.href="./";
                }
            });
        });
    </script>--%>