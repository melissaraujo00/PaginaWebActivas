<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="presentacion.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Bienvenidos</h1>
            <asp:Label ID="usuario" runat="server" Text="Ingrese el usuario"></asp:Label>
            <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="clave" runat="server" Text="Ingrese la contraseña"></asp:Label>
            <asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox>

            <asp:Button ID="btnlogin" runat="server" Text="Ingresar" OnClick="btnlogin_Click" />
            <asp:Label ID="lblmensaje" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
