<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Habitaciones.aspx.cs" Inherits="presentacion.Habotaciones" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Gestión de Habitaciones</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Agregar Habitaciones</h2>
            <asp:Label ID="lblnumero" runat="server" Text="Número de habitación" /><br />
            <asp:TextBox ID="txtnumero" runat="server" /><br />
            
            <asp:Label ID="lbldescripcion" runat="server" Text="Descripción general"/><br />
            <asp:TextBox ID="txtdescripcion" runat="server" /><br />
            
            <asp:Label ID="lablcant" runat="server" Text="Cantidad de huéspedes permitidos" /><br />
            <asp:TextBox ID="txtcantidad" runat="server" /><br />

            <asp:Button ID="btnguardar" runat="server" Text="Guardar" OnClick="btnguardar_Click" />
            <hr />
        </div>

        <asp:GridView ID="dgvHabitaciones" runat="server" AutoGenerateColumns="false"
            DataKeyNames="id_habitaciones"
            OnRowDeleting="dgvHabitaciones_RowDeleting" 
            OnRowEditing="dgvHabitaciones_RowEditing" 
            OnRowCancelingEdit="dgvHabitaciones_RowCancelingEdit" 
            OnRowUpdating="dgvHabitaciones_RowUpdating"> 
            
            <Columns>
                <asp:BoundField DataField="id_habitaciones" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="numero" HeaderText="# " />
                <asp:BoundField DataField="descripcion" HeaderText="Descripción" />
                <asp:BoundField DataField="cant_huespedes" HeaderText="Max Personas" />
                
                <asp:CommandField ShowEditButton="true" EditText="Editar" UpdateText="Actualizar" CancelText="Cancelar"/>
                <asp:CommandField ShowDeleteButton="true" DeleteText="Eliminar"/>
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>