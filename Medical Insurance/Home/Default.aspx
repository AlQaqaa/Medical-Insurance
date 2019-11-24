<%@ Page Title="الرئيسية" Language="vb" AutoEventWireup="false" MasterPageFile="~/Home/main.Master" CodeBehind="Default.aspx.vb" Inherits="Medical_Insurance._Default1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Style/JS/jquery-3.4.1.min.js"></script>
    <script src="../Style/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../Style/plugins/dataTables/dataTables.bootstrap4.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   <div class="row justify-content-center">
       <div class="col-sm-6">
           <asp:Image ID="Image1" runat="server" ImageUrl="~/Style/images/home.png" />
       </div>
   </div>

</asp:Content>
