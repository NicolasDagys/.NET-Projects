<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCli.master" AutoEventWireup="true" CodeFile="HistoricoCompras2.aspx.cs" Inherits="HistoricoCompras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">

        .style4
        {
            width: 51px;
            text-align: center;
        }
        .style5
        {
            width: 51px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table class="style1">
    <tr>
        <td class="style4">
                &nbsp;</td>
        <td class="style3">
            <strong>Historico de Compras</strong></td>
    </tr>
    <tr>
        <td class="style5">
                Pasaje</td>
        <td>
            <asp:GridView ID="GVPasaje" runat="server" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" Height="130px" 
                    onselectedindexchanged="GVVuelos_SelectedIndexChanged" Width="323px" 
                AutoGenerateSelectButton="True">
                <AlternatingRowStyle BackColor="White" />
                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                <SortedDescendingHeaderStyle BackColor="#820000" />
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td class="style5">
                &nbsp;</td>
        <td style="text-align: center">
            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="style5">
                Informacion del Vuelo</td>
        <td style="text-align: center">
            <asp:Label ID="lblAeropuertoP" runat="server"></asp:Label>
&nbsp;&nbsp;
                <asp:Label ID="lblAeropuertoL" runat="server"></asp:Label>
&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblViaje" runat="server"></asp:Label>
        </td>
    </tr>
</table>
</asp:Content>
