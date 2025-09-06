<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LogeoCliente.aspx.cs" Inherits="LogeoCliente" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 41%;
        }
        .style2
        {
            width: 150px;
        }
        .style3
        {
            text-align: right;
            width: 163px;
        }
        .style4
        {
            width: 150px;
            text-align: right;
        }
        .style5
        {
            width: 163px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table class="style1">
        <tr>
            <td class="style2">
                Pasaporte</td>
            <td class="style5">
                <asp:TextBox ID="TxtPasaporte" runat="server" Width="151px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Contraseña</td>
            <td class="style5">
                <asp:TextBox ID="TxtContra" runat="server" TextMode="Password" Width="148px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style4">
                &nbsp;</td>
            <td class="style3">
                <asp:Button ID="BtnLogeo" runat="server" onclick="BtnLogeo_Click" 
                    Text="Login" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td class="style5">
                <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
