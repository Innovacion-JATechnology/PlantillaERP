<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="misTickets.aspx.cs" Inherits="HelpDesk.misTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        /* ====== TEMA AZUL (sobre Bootstrap) ====== */
        .card-header.blue { background: #0d6efd; color: #fff; border-bottom: 1px solid #0b5ed7; }
        .accent-blue       { border-left: 4px solid #0d6efd; 

  border-right: 4px solid #0d6efd;
 }
        .group-title       { font-weight:600; color:#0d6efd; margin-bottom:.25rem; }

        .form-control[readonly] {
            background-color: #f1f5ff; /* suave azulado */
            color: #0b2e63;
            border-color: #cfe2ff;
            font-weight: 600;
        }
        .dates-box {
            border:1px solid #cfe2ff; border-radius:.5rem; padding: .75rem .9rem; background:#f8fbff;
        }

        /* Tabla */
        .table img { max-width: 100%; height: auto; display: block; }
        .table { word-break: break-word; }
        .card-body { padding: 1.25rem; }
    </style>

    <!-- Si usas DataTables de forma global, puedes conservar tu script original -->
    <script>
        $(function () {
            // Inicializa DataTables SOLO si ya lo usabas (opcional)
            $(".table").each(function () {
                var $t = $(this);
                if ($.fn.DataTable && !$t.hasClass("dataTable")) {
                    $t.DataTable({
                        responsive: true,
                        autoWidth: false,
                        pageLength: 5,
                        lengthMenu: [[5, 10, 20, 50, -1], [5, 10, 20, 50, "Todos"]],
                        language: {
                            lengthMenu: "_MENU_  -> Registros por página",
                            search: "Buscar:",
                            info: "Mostrando _START_ a _END_ de _TOTAL_ registros",
                            infoEmpty: "Mostrando 0 a 0 de 0 registros",
                            infoFiltered: "(filtrado de _MAX_ registros totales)",
                            loadingRecords: "Cargando...",
                            processing: "Procesando...",
                            zeroRecords: "No se encontraron resultados",
                            emptyTable: "No hay datos disponibles en la tabla",
                            paginate: { first: "Primero", previous: "Anterior", next: "Siguiente", last: "Último" },
                            aria: { sortAscending: ": activar para ordenar ascendente", sortDescending: ": activar para ordenar descendente" }
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="row">

            <div class="col-md-10 mx-auto">
                <div class="card">
                    <div class="card-body">

                        <h3 class="text-center">Todos mis Tickets</h3>

                        <div class="row">

                            <!-- ====== DataSource ====== -->
                            <asp:SqlDataSource
                                ID="SqlDataSource1"
                                runat="server"
                                ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                                SelectCommand="
                                    SELECT TicketId, Asunto, Estatus, AgenteId, CreadoUtc, Adjuntos, Descripcion, Prioridad
                                    FROM [hd].[Ticket]
                                    WHERE UsuarioId = @UserId">
                                <SelectParameters>
                                    <asp:SessionParameter Name="UserId" SessionField="userid" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                            <div class="col">
                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="GridView1"
                                        runat="server"
                                        CssClass="table table-striped table-bordered"
                                        AutoGenerateColumns="False"
                                        DataKeyNames="TicketId"
                                        DataSourceID="SqlDataSource1"
                                        ForeColor="#333333"
                                        GridLines="None"
                                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                         OnPreRender="GridView1_PreRender"
                                        OnRowDataBound="GridView1_RowDataBound">

                                        <AlternatingRowStyle BackColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                        <Columns>
                                            <asp:BoundField DataField="TicketId" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                                                            SortExpression="TicketId" ItemStyle-Width="70px">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            </asp:BoundField>

                                            <asp:TemplateField HeaderText="Detalle">
                                                <ItemTemplate>
                                                    <div class="row no-gutters align-items-start">
                                                        <div class="col-lg-11 pr-lg-2">

                                                            <div class="text-muted small">
                                                                Estatus:
                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Estatus") %>' />
                                                                &nbsp;|&nbsp; AgenteID:
                                                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("AgenteId") %>' />
                                                                &nbsp;|&nbsp; Creado:
                                                                <asp:Label ID="Label3" runat="server" Text='<%# ToDateTimeStr(Eval("CreadoUtc")) %>' />
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-1 text-lg-right text-left pl-lg-2">
                                                            <asp:Image ID="Image1" runat="server" CssClass="img-fluid"
                                                                       ImageUrl='<%# SafeImageUrl(Eval("Adjuntos")) %>' />
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="Asunto" HeaderText="Asunto" ReadOnly="True" SortExpression="Asunto" />

                                            <asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion">
                                                <ItemTemplate>
                                                    <span title='<%# Eval("Descripcion") %>'
                                                          class="d-inline-block text-truncate"
                                                          style="max-width: 220px;">
                                                        <%# Truncate(Eval("Descripcion"), 44) %>
                                                    </span>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <!-- ====== PANEL DE DETALLE (Tema azul, TextBox, Badges, Botón Cerrar) ====== -->
                        <asp:Panel ID="pnlDetalle" runat="server" CssClass="card mt-3 accent-blue" Visible="false">
                            <div class="card-header blue d-flex align-items-center justify-content-between">
                                <div>
                                    <span class="fw-semibold">Detalle del Ticket</span>
                                    <asp:Label ID="lblTicketId" runat="server" CssClass="ms-2 badge bg-light text-dark"></asp:Label>
                                </div>
                                <div class="d-flex align-items-center gap-2">
                                    <asp:Button ID="btnCerrar" runat="server" Text="Cerrar ticket"
                                                CssClass="btn btn-sm btn-light"
                                                OnClick="btnCerrar_Click"
                                                OnClientClick="return confirm('¿Cerrar este ticket?');" />
                                </div>
                                <asp:HiddenField ID="hidTicketId" runat="server" />
                            </div>

                            <div class="card-body">
                                <div class="row g-3">
                                    <!-- Asunto / Estatus / Prioridad (con badges) -->
                                    <div class="col-md-6">
                                        <label class="group-title">Asunto</label>
                                        <asp:TextBox ID="txtAsunto" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>

                                    <div class="col-md-3">
                                        <label class="group-title d-flex align-items-center justify-content-between">
                                            <span>Estatus</span>
                                            <span>
                                                <asp:Label ID="lblEstatusBadge" runat="server" CssClass="badge rounded-pill bg-primary"></asp:Label>
                                            </span>
                                        </label>
                                        <asp:TextBox ID="txtEstatus" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>

                                    <div class="col-md-3">
                                        <label class="group-title d-flex align-items-center justify-content-between">
                                            <span>Prioridad</span>
                                            <span>
                                                <asp:Label ID="lblPrioridadBadge" runat="server" CssClass="badge rounded-pill bg-secondary"></asp:Label>
                                            </span>
                                        </label>
                                        <asp:TextBox ID="txtPrioridad" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>

                                    <!-- Participantes -->
                                    <div class="col-md-3">
                                        <label class="group-title">Agente</label>
                                        <asp:TextBox ID="txtAgenteId" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>
                                    <div class="col-md-3">
                                        <label class="group-title">Usuario</label>
                                        <asp:TextBox ID="txtUsuarioId" runat="server" CssClass="form-control" ReadOnly="true" />
                                    </div>

                                    <!-- Caja de fechas -->
                                    <div class="col-12">
                                        <div class="d-flex align-items-center mb-1">
                                            <span class="group-title me-2">Fechas</span>
                                            <small class="text-muted">(UTC o zona según BD)</small>
                                        </div>
                                        <div class="dates-box">
                                            <div class="row g-3">
                                                <div class="col-sm-6 col-lg-3">
                                                    <label class="form-label mb-1">Para (UTC)</label>
                                                    <asp:TextBox ID="txtParaUtc" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                                <div class="col-sm-6 col-lg-3">
                                                    <label class="form-label mb-1">Creado</label>
                                                    <asp:TextBox ID="txtCreadoUtc" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                                <div class="col-sm-6 col-lg-3">
                                                    <label class="form-label mb-1">Actualizado</label>
                                                    <asp:TextBox ID="txtActualizadoUtc" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                                <div class="col-sm-6 col-lg-3">
                                                    <label class="form-label mb-1">Cerrado</label>
                                                    <asp:TextBox ID="txtCerradoUtc" runat="server" CssClass="form-control" ReadOnly="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!-- Descripción -->
                                    <div class="col-12">
                                        <label class="group-title">Descripción</label>
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" ReadOnly="true"
                                                     TextMode="MultiLine" Rows="4" />
                                    </div>

                                    <!-- Adjuntos -->
                                    <div class="col-12">
                                        <label class="group-title">Adjuntos</label>
                                        <div class="d-flex align-items-center flex-wrap gap-2">
                                            <asp:HyperLink ID="lnkAdjuntos" runat="server" Target="_blank"
                                                           CssClass="btn btn-sm btn-outline-primary" Text="Abrir adjunto"></asp:HyperLink>
                                            <asp:Label ID="lblAdjuntosRaw" runat="server" CssClass="text-muted small"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <a href="<%= ResolveUrl("~/InicioUsuario.aspx") %>">&laquo; Regresar al Inicio</a><br /><br />
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>