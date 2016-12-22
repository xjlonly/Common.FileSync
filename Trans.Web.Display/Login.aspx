<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Trans.Web.Display.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
<title>家居文件同步系统</title>
<link rel="stylesheet" type="text/css" href="./Css/selectstyles.css">
    <style type="text/css">
        .incss {
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            outline: 0;
            background-color: white;
            border: 0;
            padding: 10px 15px;
            color: #53e3a6;
            border-radius: 3px;
            width: 250px;
            cursor: pointer;
            font-size: 18px;
            -webkit-transition-duration: 0.25s;
            transition-duration: 0.25s;
        }

        form button:hover {
            background-color: #f5f7f9;
        }
    </style>

</head>
    <body>
    <div class="htmleaf-container">
        <div class="wrapper">
            <div class="container">
                <h1>欢迎</h1>
                <form method="post">
                    <div id="namelogin" >
                        <input id="UserName" name="UserName" type="text"  placeholder="请输入用户名">
                        <input name="Password" id="userpwd" type="password"  placeholder="请输入密码">
                        <input name="loginbutton" type="submit" id="button" class="button" value="登陆">
                    </div>
                </form>
            </div>
            
            <ul class="bg-bubbles">
                <li></li>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
            </ul>
        </div>
    </div>

</body>
</html>