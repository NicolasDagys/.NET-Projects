<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 262px;
        }
        .auto-style2 {
            text-align: center;
            width: 475px;
            height: 26px;
        }
        .auto-style3 {
            width: 475px;
        }
        .auto-style4 {
            width: 262px;
            height: 27px;
        }
        .auto-style5 {
            width: 475px;
            height: 27px;
        }
        .auto-style6 {
            height: 27px;
            width: 78px;
        }
        .auto-style7 {
            width: 262px;
            height: 252px;
        }
        .auto-style8 {
            text-align: center;
            height: 252px;
        }
        .auto-style10 {
            width: 262px;
            height: 26px;
        }
        .auto-style11 {
            height: 26px;
            width: 78px;
        }
        .auto-style12 {
            width: 78px;
        }
        .auto-style13 {
            text-align: center;
            width: 312px;
            height: 26px;
        }
        .auto-style14 {
            width: 312px;
            height: 27px;
        }
        .auto-style15 {
            width: 312px;
        }
        .auto-style16 {
            width: 62%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style16">
                <tr>
                    <td class="auto-style1"><strong>ARTÍCULOS</strong></td>
                    <td class="auto-style3" colspan="2"></td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style7">
                        <asp:GridView ID="GVArticulos" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="5" OnPageIndexChanging="GVArticulos_PageIndexChanging" OnSelectedIndexChanged="GVArticulos_SelectedIndexChanged">
                            <Columns>
                                <asp:BoundField DataField="CodArt" HeaderText="Código" />
                                <asp:BoundField DataField="NomArt" HeaderText="Nombre" />
                                <asp:BoundField DataField="FechaVtoArt" HeaderText="Vencimiento" />
                                <asp:CommandField ShowSelectButton="True" />
                            </Columns>
                        </asp:GridView>
                        S</td>
                    <td class="auto-style8" colspan="3">Artículos por Nombre<br />
                        <asp:TextBox ID="TxtArtNom" runat="server"></asp:TextBox>
                        <br />
                        <br />
                        &nbsp; &nbsp;
&nbsp;<asp:DropDownList ID="DDLCategorias" runat="server" Width="250px">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <asp:Button ID="BtnFiltrar" runat="server" Text="Filtrar" Width="250px" OnClick="BtnFiltrar_Click" />
                        <br />
                        <br />
                        <asp:Button ID="BtnLimpiar" runat="server" Text="Reiniciar" Width="250px" OnClick="BtnLimpiar_Click" />
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10"></td>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style13">
                        <asp:Label ID="LblError" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                    <td class="auto-style11"></td>
                </tr>
                <tr>
                    <td class="auto-style4"><strong>Nombre</strong></td>
                    <td class="auto-style5">
                        <asp:Label ID="LblNombre" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style14">&nbsp;</td>
                    <td class="auto-style6"></td>
                </tr>
                <tr>
                    <td class="auto-style1"><strong>Código</strong></td>
                    <td class="auto-style3">
                        <asp:Label ID="LblCodigo" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style15">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"><strong>Categoría</strong></td>
                    <td class="auto-style3">
                        <asp:Label ID="LblCategoria" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style15">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"><strong>Fecha de Vencimiento</strong></td>
                    <td class="auto-style3">
                        <asp:Label ID="LblFVenc" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style15">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"><strong>Presentación</strong></td>
                    <td class="auto-style3">
                        <asp:Label ID="LblPresentacion" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style15">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"><strong>Tamaño</strong></td>
                    <td class="auto-style3">
                        <asp:Label ID="LblTamanio" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style15">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"><strong>Precio</strong></td>
                    <td class="auto-style3">
                        <asp:Label ID="LblPrecio" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style15">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"><strong>Cantidad de Ventas</strong></td>
                    <td class="auto-style3">
                        <asp:Label ID="LblCantVentas" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style15">&nbsp;</td>
                    <td class="auto-style12">&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
