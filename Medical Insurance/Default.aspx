<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="Medical_Insurance._Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>تسجيل دخول | التأمين الطبي</title>

    <link href="Style/CSS/MyStyle.css" rel="stylesheet" />
    <link href="Style/CSS/bootstrap.min.css" rel="stylesheet" />
    <link href="Style/CSS/bootstrap-rtl.css" rel="stylesheet" />

</head>
<body style="font-family: 'Cairo-Regular'">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <div class="row">
                <div class="col-sm-12">
                    <div class="top-navbar">
                        <nav class="navbar navbar-light" style="background-color: #f0f0f0; border-bottom: 1px solid #d8d8d8;">
                            <span class="navbar-brand mb-0 h1">التأمين الطبي</span>
                        </nav>
                    </div>
                </div>
            </div>
            <div class="row justify-content-center">
                <div class="col-sm-4">
                    <div class="card mt-5">
                        <div class="card-body bg-light text-center">
                            <h3>تسجيل دخول</h3>
                            <div class="login-img">
                                <asp:Image ID="img_login" CssClass="img-fluid img-thumbnail rounded-circle" runat="server" ImageUrl="~/Style/images/login.png" Width="100px" />
                            </div>
                            <div class="form-group row">
                                <label for="txt_user_name" class="col-sm-4 col-form-label">اسم المستخدم</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txt_user_name" CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="يجب إدخال اسم المستخدم" ControlToValidate="txt_user_name" Font-Size="X-Small" ValidationGroup="btn_login" ForeColor="Red"></asp:RequiredFieldValidator>                                                 
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="txt_user_name" class="col-sm-4 col-form-label">كلمة المرور</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txt_password" CssClass="form-control" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="يجب إدخال كلمة المرور" ControlToValidate="txt_password" Font-Size="X-Small" ValidationGroup="btn_login" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>                     
                            <div class="form-group row justify-content-center">
                                <div class="col-sm-8">
                                    <asp:Button ID="btn_login" CssClass="btn btn-outline-success btn-block" runat="server" ValidationGroup="btn_login" Text="تسجيل دخول" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <div class="footer text-center fixed-bottom">
                    <a href='http://www.injaztech-ly.com' style="color: #ffd800;">شركة إنجاز | Enjaz Co</a>
                </div>
            </div>
        </div>
    </form>

    <script src="Style/JS/jquery-3.4.1.min.js"></script>
    <script src="Style/JS/popper.min.js"></script>
    <script src="Style/JS/bootstrap.min.js"></script>

</body>
</html>
