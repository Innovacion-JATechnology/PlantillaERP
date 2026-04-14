<%@ Page Title="Agentes" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AdminAllAgents.aspx.cs" Inherits="HelpDesk.AdminAllAgents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .badge { display:inline-block; padding:.35em .6em; font-size:.75rem; border-radius:.25rem; }
        .badge-info { background:#17a2b8; color:#fff; }
        /* cursor de mano en filas */
        .row-click { cursor:pointer; }
    </style>
    <script type="text/javascript">
        // IDs de controles del modal (renderizados por WebForms):
        window.modalHfAgenteId = '<%= modalHiddenAgenteId.ClientID %>';
        window.modalTxtNombre  = '<%= modalTxtNombre.ClientID %>';
        window.modalTxtNivel   = '<%= modalTxtNivel.ClientID %>';
        window.modalTxtAbiertos = '<%= modalTxtAbiertos.ClientID %>';
        window.modalTxtTelefono = '<%= modalTxtTelefono.ClientID %>';
        window.modalTxtHabilidades = '<%= modalTxtHabilidades.ClientID %>';
        window.modalDdlEstatus = '<%= modalDdlEstatus.ClientID %>';

        // Abre modal y rellena con los atributos data-* de la fila
        function rowClick(tr) {
            try {
                var get = function (name) { return tr.getAttribute('data-' + name) || ''; };

                var agenteId = get('agenteid');
                var nombre   = get('nombre');
                var nivel    = get('nivel');
                var abiertos = get('tickets');
                var telefono = get('telefono');
                var habilidades = get('habilidades');
                var estatus = get('estatus');

                // Campos del modal
                var hf     = document.getElementById(window.modalHfAgenteId);
                var txtNo  = document.getElementById(window.modalTxtNombre);
                var txtNi  = document.getElementById(window.modalTxtNivel);
                var txtAb  = document.getElementById(window.modalTxtAbiertos);
                var txtTe  = document.getElementById(window.modalTxtTelefono);
                var txtHa  = document.getElementById(window.modalTxtHabilidades);
                var ddlEs  = document.getElementById(window.modalDdlEstatus);

                if (hf)    hf.value = agenteId;
                if (txtNo) txtNo.value = nombre;
                if (txtNi) txtNi.value = nivel;
                if (txtAb) txtAb.value = abiertos;
                if (txtTe) txtTe.value = telefono;
                if (txtHa) txtHa.value = habilidades;
                if (ddlEs) ddlEs.value = estatus;

                // Mostrar modal
                $('#agenteModal').modal('show');
            } catch (e) {
                console.error(e);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Modal de edición -->
    <div class="modal fade" id="agenteModal" tabindex="-1" role="dialog" aria-labelledby="agenteModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="agenteModalLabel">Editar agente</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Cerrar">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>

          <div class="modal-body">
            <asp:HiddenField ID="modalHiddenAgenteId" runat="server" />
            <div class="form-row">
              <div class="form-group col-md-6">
                <label>Nombre</label>
                <asp:TextBox ID="modalTxtNombre" runat="server" CssClass="form-control" MaxLength="200" />
              </div>
              <div class="form-group col-md-3">
                <label>Nivel</label>
                <asp:TextBox ID="modalTxtNivel" runat="server" CssClass="form-control" TextMode="Number" />
              </div>
              <div class="form-group col-md-3">
                <label>Tickets Abiertos</label>
                <asp:TextBox ID="modalTxtAbiertos" runat="server" CssClass="form-control" TextMode="Number" ReadOnly="true" />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group col-md-6">
                <label>Teléfono</label>
                <asp:TextBox ID="modalTxtTelefono" runat="server" CssClass="form-control" MaxLength="25" />
              </div>
              <div class="form-group col-md-6">
                <label>Estatus</label>
                <asp:DropDownList ID="modalDdlEstatus" runat="server" CssClass="form-control">
                  <asp:ListItem Text="Activo" Value="1" />
                  <asp:ListItem Text="Inactivo" Value="0" />
                </asp:DropDownList>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group col-md-12">
                <label>Habilidades</label>
                <asp:TextBox ID="modalTxtHabilidades" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="4000" />
              </div>
            </div>
          </div>

          <div class="modal-footer">
            <asp:Button ID="modalBtnSave" runat="server" CssClass="btn btn-primary" Text="Guardar cambios" OnClick="modalBtnSave_Click" />
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Contenido principal (centrado) -->
    <div class="container-fluid">
        <!-- centramos a col-xl-8 / col-lg-9 -->
        <div class="row justify-content-center">
            <div class="col-xl-8 col-lg-9">

                <div class="row mb-3">
                    <div class="col">
                        <h3 class="mb-0">Agentes</h3>
                        <small class="text-muted">Click en un renglon para modificar Agente</small>
                    </div>
                </div>

                <!-- Buscador -->
                <div class="row mb-3">
                    <div class="col-md-8">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                            placeholder="Buscar por nombre de agente"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary btn-block"
                            Text="Buscar" OnClick="btnSearch_Click" />
                    </div>
                    <div class="col-md-2 text-right">
                        <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary"
                            Text="Limpiar" OnClick="btnClear_Click" />
                    </div>
                </div>

                <!-- Botón para recalcular -->
                <div class="row mb-3">
                    <div class="col">
                        <asp:Button ID="btnRecalculate" runat="server" CssClass="btn btn-warning"
                            Text="🔄 Recalcular Tickets Asignados" OnClick="btnRecalculate_Click" />
                    </div>
                </div>

                <!-- DataSource principal (Agentes) -->
                <asp:SqlDataSource ID="SqlDataSourceAgents" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    CancelSelectOnNullParameter="false"
                    SelectCommand="
                        SELECT 
                            a.AgenteId,
                            a.nombre,
                            a.nivel,
                            ISNULL(a.tAbiertos, 0) AS TicketsAbiertos,
                            ISNULL(a.telefono, '') AS telefono,
                            ISNULL(a.habilidades, '') AS habilidades,
                            ISNULL(a.estatus, 1) AS Estatus
                        FROM [hd].[Agente] a
                        WHERE
                            (@q IS NULL OR @q = '')
                            OR (a.nombre LIKE '%' + @q + '%')
                        ORDER BY a.nombre;">
                    <SelectParameters>
                        <asp:ControlParameter Name="q" ControlID="txtSearch" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <!-- Tabla de agentes -->
                <asp:GridView ID="GridViewAgents" runat="server"
                    CssClass="table table-striped table-bordered table-sm"
                    DataSourceID="SqlDataSourceAgents"
                    AutoGenerateColumns="False"
                    DataKeyNames="AgenteId"
                    AllowPaging="True" PageSize="10"
                    OnPageIndexChanging="GridViewAgents_PageIndexChanging"
                    OnRowDataBound="GridViewAgents_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="AgenteId" HeaderText="Id" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" SortExpression="nombre" />
                        <asp:BoundField DataField="nivel" HeaderText="Nivel" SortExpression="nivel" />
                        <asp:BoundField DataField="TicketsAbiertos" HeaderText="Tickets Asignados" SortExpression="TicketsAbiertos" />
                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                    </Columns>

                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <AlternatingRowStyle BackColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                </asp:GridView>

            </div>
        </div>
    </div>

</asp:Content>
