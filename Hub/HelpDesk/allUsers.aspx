<%@ Page Title="Usuarios" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="allUsers.aspx.cs" Inherits="HelpDesk.allUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .badge { display:inline-block; padding:.35em .6em; font-size:.75rem; border-radius:.25rem; }
        .badge-success { background:#28a745; color:#fff; }
        .badge-secondary { background:#6c757d; color:#fff; }
        /* cursor de mano en filas */
        .row-click { cursor:pointer; }
    </style>
    <script type="text/javascript">
        // IDs de controles del modal (renderizados por WebForms):
        window.modalHfUserId   = '<%= modalHiddenUsuarioId.ClientID %>';
        window.modalTxtNombre  = '<%= modalTxtNombre.ClientID %>';
        window.modalTxtApPat   = '<%= modalTxtApPaterno.ClientID %>';
        window.modalTxtApMat   = '<%= modalTxtApMaterno.ClientID %>';
        window.modalTxtCorreo  = '<%= modalTxtCorreo.ClientID %>';
        window.modalDdlEmpresa = '<%= modalDdlEmpresa.ClientID %>';
        window.modalDdlPuesto  = '<%= modalDdlPuesto.ClientID %>';
        window.modalDdlSla     = '<%= modalDdlSLA.ClientID %>';
        window.modalChkActivo = 'modalChkActivo';

        // Abre modal y rellena con los atributos data-* de la fila
        function rowClick(tr) {
            try {
                var get = function (name) { return tr.getAttribute('data-' + name) || ''; };

                var userId   = get('usuarioid');
                var nombre   = get('nombre');
                var apPat    = get('appaterno');
                var apMat    = get('apmaterno');
                var correo   = get('correo');
                var empresa  = get('empresaid');
                var puesto   = get('puestoid');
                var sla      = get('slaid'); 
                var activo = (get('activo') || 'false').toString().toLowerCase();// "True"/"False" desde Eval

                // Campos del modal
                var hf     = document.getElementById(window.modalHfUserId);
                var txtNo  = document.getElementById(window.modalTxtNombre);
                var txtPa  = document.getElementById(window.modalTxtApPat);
                var txtMa  = document.getElementById(window.modalTxtApMat);
                var txtCo  = document.getElementById(window.modalTxtCorreo);
                var ddlEm  = document.getElementById(window.modalDdlEmpresa);
                var ddlPu  = document.getElementById(window.modalDdlPuesto);
                var ddlSl  = document.getElementById(window.modalDdlSla);
                var chkAc  = document.getElementById(window.modalChkActivo);

                if (hf)    hf.value = userId;
                if (txtNo) txtNo.value = nombre;
                if (txtPa) txtPa.value = apPat;
                if (txtMa) txtMa.value = apMat;
                if (txtCo) txtCo.value = correo;

                // Seleccionar por value; si no existe, fallback al primer ítem
                function setSel(sel, val) {
                    if (!sel) return;
                    try { sel.value = val || ""; } catch (e) {}
                    if (val && sel.value !== val) sel.selectedIndex = 0;
                }
                setSel(ddlEm, empresa);
                setSel(ddlPu, puesto);
                setSel(ddlSl, sla);
                chkAc.checked = false;
                chkAc.checked = (activo === 'true');

                // Mostrar modal
                $('#userModal').modal('show');
            } catch (e) {
                console.error(e);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Modal de edición -->
    <div class="modal fade" id="userModal" tabindex="-1" role="dialog" aria-labelledby="userModalLabel" aria-hidden="true">
      <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="userModalLabel">Editar usuario</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Cerrar">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>

          <div class="modal-body">
            <asp:HiddenField ID="modalHiddenUsuarioId" runat="server" />
            <div class="form-row">
              <div class="form-group col-md-4">
                <label>Nombre</label>
                <asp:TextBox ID="modalTxtNombre" runat="server" CssClass="form-control" MaxLength="50" />
              </div>
              <div class="form-group col-md-4">
                <label>Apellido paterno</label>
                <asp:TextBox ID="modalTxtApPaterno" runat="server" CssClass="form-control" MaxLength="50" />
              </div>
              <div class="form-group col-md-4">
                <label>Apellido materno</label>
                <asp:TextBox ID="modalTxtApMaterno" runat="server" CssClass="form-control" MaxLength="50" />
              </div>
            </div>

            <div class="form-row">
              <div class="form-group col-md-6">
                <label>Correo</label>
                <asp:TextBox ID="modalTxtCorreo" runat="server" CssClass="form-control" MaxLength="256" ReadOnly="true" />
              </div>
              <div class="form-group col-md-6 d-flex align-items-center">
                <div class="custom-control custom-switch mt-4">
                

<input type="checkbox"
       id="modalChkActivo"    name="modalChkActivo"
       class="custom-control-input" />

<label class="custom-control-label" for="modalChkActivo">
    Activo
</label>



                </div>
              </div>
            </div>

            <div class="form-row">
              <div class="form-group col-md-4">
                <label>Empresa</label>
                <asp:DropDownList ID="modalDdlEmpresa" runat="server" CssClass="form-control"
                    DataSourceID="SqlDataSourceEmpresa" DataTextField="Nombre" DataValueField="EmpresaId" AppendDataBoundItems="true">
                    <asp:ListItem Text="-- seleccionar --" Value="" />
                </asp:DropDownList>
              </div>
              <div class="form-group col-md-4">
                <label>Puesto</label>
                <asp:DropDownList ID="modalDdlPuesto" runat="server" CssClass="form-control"
                    DataSourceID="SqlDataSourcePuesto" DataTextField="Nombre" DataValueField="PuestoId" AppendDataBoundItems="true">
                    <asp:ListItem Text="-- seleccionar --" Value="" />
                </asp:DropDownList>
              </div>
              <div class="form-group col-md-4">
                <label>SLA</label>
                <asp:DropDownList ID="modalDdlSLA" runat="server" CssClass="form-control"
                    DataSourceID="SqlDataSourceSLA" DataTextField="Nombre" DataValueField="SLAId" AppendDataBoundItems="true">
                    <asp:ListItem Text="-- seleccionar --" Value="" />
                </asp:DropDownList>
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

    <!-- Contenido principal (angosto y centrado) -->
    <div class="container-fluid">
        <!-- centramos a col-xl-8 / col-lg-9 -->
        <div class="row justify-content-center">
            <div class="col-xl-8 col-lg-9">

                <div class="row mb-3">
                    <div class="col">
                        <h3 class="mb-0">Usuarios</h3>
                        <small class="text-muted">Nombre + Puesto + SLA + Empresa + Activo</small>
                    </div>
                </div>

                <!-- Buscador -->
                <div class="row mb-3">
                    <div class="col-md-8">
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control"
                            placeholder="Buscar por nombre, puesto, SLA o empresa"></asp:TextBox>
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

                <!-- DataSource principal (Usuarios + joins) -->
                <asp:SqlDataSource ID="SqlDataSourceUsers" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    CancelSelectOnNullParameter="false"
                    SelectCommand="
                        SELECT 
                            u.UsuarioId,
                            u.Nombre,
                            u.ApPaterno,
                            u.ApMaterno,
                            u.Correo,
                            u.Empresa       AS EmpresaId,
                            u.Puesto        AS PuestoId,
                            u.SLA           AS SLAId,
                            (u.Nombre + ' ' + ISNULL(u.ApPaterno, '')) AS NombreCompleto,
                            ISNULL(p.Nombre, '(sin puesto)')  AS PuestoNombre,
                            ISNULL(s.Nombre, '(sin SLA)')     AS SLANombre,
                            ISNULL(e.Nombre, '(sin empresa)') AS EmpresaNombre,
                            u.Activo
                        FROM [hd].[Usuario] u
                        LEFT JOIN [hd].[Puesto]  p ON u.Puesto  = p.PuestoId
                        LEFT JOIN [hd].[SLA]     s ON u.SLA     = s.SLAId
                        LEFT JOIN [hd].[Empresa] e ON u.Empresa = e.EmpresaId
                        WHERE
                            (@q IS NULL OR @q = '')
                            OR (u.Nombre      LIKE '%' + @q + '%'
                                OR u.ApPaterno LIKE '%' + @q + '%'
                                OR u.ApMaterno LIKE '%' + @q + '%'
                                OR p.Nombre    LIKE '%' + @q + '%'
                                OR s.Nombre    LIKE '%' + @q + '%'
                                OR e.Nombre    LIKE '%' + @q + '%')
                        ORDER BY u.Activo DESC, u.Nombre, u.ApPaterno;">
                    <SelectParameters>
                        <asp:ControlParameter Name="q" ControlID="txtSearch" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    </SelectParameters>
                </asp:SqlDataSource>

                <!-- DataSources para modal -->
                <asp:SqlDataSource ID="SqlDataSourceEmpresa" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    SelectCommand="SELECT EmpresaId, Nombre FROM [hd].[Empresa] ORDER BY Nombre" />

                <asp:SqlDataSource ID="SqlDataSourcePuesto" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    SelectCommand="SELECT PuestoId, Nombre FROM [hd].[Puesto] ORDER BY Nombre" />

                <asp:SqlDataSource ID="SqlDataSourceSLA" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    SelectCommand="SELECT SLAId, Nombre FROM [hd].[SLA] ORDER BY Nombre" />

                <!-- Tabla de usuarios (angosta en esta columna) -->
                <asp:GridView ID="GridViewUsers" runat="server"
                    CssClass="table table-striped table-bordered table-sm"
                    DataSourceID="SqlDataSourceUsers"
                    AutoGenerateColumns="False"
                    DataKeyNames="UsuarioId"
                    AllowPaging="True" PageSize="10"
                    OnPageIndexChanging="GridViewUsers_PageIndexChanging"
                    OnRowDataBound="GridViewUsers_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="UsuarioId" HeaderText="Id" ReadOnly="true" Visible="false" />
                        <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" SortExpression="NombreCompleto" />
                        <asp:BoundField DataField="PuestoNombre" HeaderText="Puesto" SortExpression="PuestoNombre" />
                        <asp:BoundField DataField="SLANombre" HeaderText="SLA" SortExpression="SLANombre" />
                        <asp:BoundField DataField="EmpresaNombre" HeaderText="Empresa" SortExpression="EmpresaNombre" />
                        <asp:TemplateField HeaderText="Activo">
                            <ItemTemplate>
                                <asp:Label ID="lblActivo" runat="server"
                                    Text='<%# Convert.ToBoolean(Eval("Activo")) ? "Activo" : "Inactivo" %>'
                                    CssClass='<%# Convert.ToBoolean(Eval("Activo")) ? "badge badge-success" : "badge badge-secondary" %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
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