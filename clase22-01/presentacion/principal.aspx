<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="principal.aspx.cs" Inherits="presentacion.principal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="CSS/estilos.css" rel="stylesheet" type="text/css" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <div class="navbar">
    <form id="form1" runat="server" class="login-form">
        <div >
            <h1>Bienvenidos</h1>
            <h3>Usuario</h3>
            <asp:Label ID="lblUsuario" runat="server" Text="Ingrese el usuario"></asp:Label>
            <br />
        </div>
        <asp:Button ID="btnHabitaciones" runat="server" Text="Habitaciones" OnClick="btnHabitaciones_Click" />
        <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesión" OnClick="btnCerrarSesion_Click" />
    </form>
    </div>
</body>
</html>
