<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="presentacion.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="CSS/estilos.css" rel="stylesheet" type="text/css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="navbar">
            <h1>Sistema</h1>
            <div class="login-form">
                <asp:TextBox ID="txtUsuario" runat="server" Placeholder="Usuario" />
                <asp:TextBox ID="password" runat="server" TextMode="Password" Placeholder="Contraseña" />
                <asp:Button ID="btnLogin" runat="server" Text="Ingresar" OnClick="btnlogin_Click" />
            </div>
        </div>

        <asp:Label ID="lblMensaje" runat="server" CssClass="mensaje" />
    </form>
</body></html>
