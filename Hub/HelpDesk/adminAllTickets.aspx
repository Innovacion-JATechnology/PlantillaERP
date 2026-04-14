<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="adminAllTickets.aspx.cs" Inherits="HelpDesk.adminAllTickets" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="container-fluid">
        <div class="row mb-3">

            <div class="col-10 mx-auto">
                <div class="card">
                    <div class="card-body"> 

                        <script type="text/javascript">
                            function toggleDesc(el) {
                                var parent = el.parentElement;
                                var desc = parent.querySelector('.ticket-desc');
                                if (!desc) return;
                                desc.style.display = (desc.style.display === 'none') ? 'block' : 'none';
                            }

                            function onAgentChange(sel) {
                                try {
                                    var row = sel.closest('tr');
                                    if (!row) return;
                                    var btn = row.querySelector('input[id$="btnAssign"]');
                                    if (!btn) return;
                                    // enable assign button only if selected value is not empty
                                    btn.disabled = !(sel.value && sel.value !== '');
                                } catch (e) { console.error(e); }
                            }

                            // when a row is clicked, open modal and populate values
                            function rowClick(row) {
                                var asunto = row.getAttribute('data-asunto') || '';
                                var descripcion = row.getAttribute('data-descripcion') || '';
                                var usuario = row.getAttribute('data-usuario') || '';
                                var creado = row.getAttribute('data-creado') || '';
                                var agente = row.getAttribute('data-agente') || '';
                                var estatus = row.getAttribute('data-estatus') || '';
                                var ticketId = row.getAttribute('data-ticketid') || '';

                                var modal = document.getElementById('ticketModal');
                                if (!modal) return;

                                modal.querySelector('.modal-title').innerText = asunto;
                                modal.querySelector('#modalUsuario').innerText = usuario;
                                modal.querySelector('#modalCreado').innerText = creado;
                              //  modal.querySelector('#modalAgente').innerText = agente;
                                modal.querySelector('#modalEstatus').innerText = estatus;
                                modal.querySelector('#modalDescripcion').innerText = descripcion;

                                if (ticketId) {
                                    var hf = document.getElementById(window.modalHiddenId);
                                    if (hf) hf.value = ticketId;
                                }

                                // set modal dropdown selection to current agent
                                var sel = document.getElementById(window.modalDdlId);
                                if (sel) {
                                    var current = row.getAttribute('data-agenteid') || '';
                                    var agentName = row.getAttribute('data-agente') || '';
                                    sel.setAttribute('data-current', current);

                                    try { sel.value = current; } catch (e) { }

                                    if (sel.value !== current) {
                                        // fallback: try to match by agent name (optional)
                                        var found = false;
                                        for (var i = 0; i < sel.options.length; i++) {
                                            var opt = sel.options[i];
                                            if (opt.value === current) { sel.selectedIndex = i; found = true; break; }
                                            if (agentName && opt.text.indexOf(agentName) !== -1) { sel.selectedIndex = i; found = true; break; }
                                        }
                                        if (!found) { sel.selectedIndex = 0; }
                                    }

                                    if (typeof onModalAgentChange === 'function') onModalAgentChange(sel);
                                }

                                // set ticket status dropdown
                                var statusDdl = document.getElementById(window.modalDdlStatusId);
                                if (statusDdl) {
                                    var ticketStatus = row.getAttribute('data-ticketstatus') || '1';
                                    statusDdl.setAttribute('data-current-status', ticketStatus);

                                    // Load valid states based on current status and user role
                                    loadValidStatusOptions(ticketStatus);

                                    if (typeof onModalStatusChange === 'function') onModalStatusChange(statusDdl);
                                }

                                // set prioridad label (map numeric to text)
                                try {
                                    var pr = row.getAttribute('data-prioridad') || '';
                                    var prLbl = document.getElementById('modalPrioridad');
                                    if (prLbl) {
                                        var txt = '';
                                        switch (pr) {
                                            case '1': txt = 'Crítico'; break;
                                            case '2': txt = 'Muy urgente'; break;
                                            case '3': txt = 'Urgente'; break;
                                            case '4': txt = 'Normal'; break;
                                            case '5': txt = 'Bajo'; break;
                                            default: txt = pr || 'NA'; break;
                                        }
                                        prLbl.innerText = txt;
                                    }
                                } catch (e) { }

                                $('#ticketModal').modal('show');
                            }

                            function onModalAgentChange(sel) {
                                var current = sel.getAttribute('data-current') || '';
                                var btn = document.getElementById(window.modalBtnId);
                                if (!btn) return;
                                btn.disabled = (!sel.value || sel.value === '' || sel.value === current);
                            }

                            function onModalStatusChange(sel) {
                                var current = sel.getAttribute('data-current-status') || '1';
                                var btn = document.getElementById(window.modalBtnStatusId);
                                if (!btn) return;
                                btn.disabled = (sel.value === current);
                            }

                            function loadValidStatusOptions(currentStatusValue) {
                                var statusDdl = document.getElementById(window.modalDdlStatusId);
                                if (!statusDdl) return;

                                // Call server-side WebMethod to get valid statuses based on role and current status
                                PageMethods.GetValidTicketStatuses(
                                    parseInt(currentStatusValue),
                                    function(result) {
                                        // Clear dropdown except for placeholder
                                        statusDdl.options.length = 0;

                                        // Add option for current status as placeholder
                                        var currentStatusText = getStatusName(parseInt(currentStatusValue));
                                        var opt = document.createElement('option');
                                        opt.value = currentStatusValue;
                                        opt.text = currentStatusText + ' (Actual)';
                                        opt.disabled = true;
                                        statusDdl.appendChild(opt);

                                        // Add valid next states
                                        if (result && result.length > 0) {
                                            for (var i = 0; i < result.length; i++) {
                                                opt = document.createElement('option');
                                                opt.value = result[i].Value;
                                                opt.text = result[i].Text;
                                                statusDdl.appendChild(opt);
                                            }
                                            statusDdl.value = currentStatusValue;
                                        }
                                    },
                                    function(error) {
                                        console.error('Error loading valid statuses:', error);
                                    }
                                );
                            }

                            function getStatusName(statusValue) {
                                switch (statusValue) {
                                    case 1: return 'Nuevo';
                                    case 2: return 'Abierto';
                                    case 3: return 'En Progreso';
                                    case 4: return 'En Espera';
                                    case 5: return 'Escalado';
                                    case 6: return 'Resuelto';
                                    case 7: return 'Cerrado';
                                    case 8: return 'Reabierto';
                                    case 9: return 'Cancelado';
                                    default: return 'Desconocido';
                                }
                            }

                            // expose client IDs to script for use when rows click
                            window.modalDdlId = '<%= modalDdlAgents.ClientID %>';
                            window.modalBtnId = '<%= modalBtnAssign.ClientID %>';
                            window.modalHiddenId = '<%= modalHiddenTicketId.ClientID %>';
                            window.modalDdlStatusId = '<%= modalDdlTicketStatus.ClientID %>';
                            window.modalBtnStatusId = '<%= modalBtnUpdateTicketStatus.ClientID %>';
                        </script>

                        <div class="row">
                            <div class="col">
                                <!-- Modal -->
                                <div class="modal fade" id="ticketModal" tabindex="-1" role="dialog" aria-labelledby="ticketModalLabel" aria-hidden="true">
                                  <div class="modal-dialog modal-lg" role="document">
                                    <div class="modal-content">
                                      <div class="modal-header">
                                        <h5 class="modal-title" id="ticketModalLabel">Detalles</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                          <span aria-hidden="true">&times;</span>
                                        </button>
                                      </div>
                                      <div class="modal-body">
                                        <dl class="row">
                                          <dt class="col-sm-3">Usuario</dt>
                                          <dd class="col-sm-9" id="modalUsuario"></dd>

                                          <dt class="col-sm-3">Creado</dt>
                                          <dd class="col-sm-9" id="modalCreado"></dd>

                                    <dt class="col-sm-3">Agente</dt>
<dd class="col-sm-9">
  <div class="input-group">
    <asp:DropDownList ID="modalDdlAgents" runat="server"
        CssClass="form-control"
        DataSourceID="SqlDataSourceAgentList"
        DataTextField="DisplayText"
        DataValueField="AgenteId"
        AppendDataBoundItems="true"
        onchange="onModalAgentChange(this)">
        <asp:ListItem Text="-- Selecciona agente --" Value="" />
    </asp:DropDownList>
    <div class="input-group-append">
      <asp:Button ID="modalBtnAssign" runat="server"
          CssClass="btn btn-primary"
          Text="Asignar"
          OnClick="modalBtnAssign_Click"
          Enabled="false" />
    </div>
  </div>
</dd>

                                          <dt class="col-sm-3">Estatus Ticket</dt>
                                          <dd class="col-sm-9">
                                            <div class="input-group">
                                              <asp:DropDownList ID="modalDdlTicketStatus" runat="server" CssClass="form-control" onchange="onModalStatusChange(this)">
                                                <asp:ListItem Text="Nuevo" Value="1" />
                                                <asp:ListItem Text="Abierto" Value="2" />
                                                <asp:ListItem Text="En Progreso" Value="3" />
                                                <asp:ListItem Text="En Espera" Value="4" />
                                                <asp:ListItem Text="Escalado" Value="5" />
                                                <asp:ListItem Text="Resuelto" Value="6" />
                                                <asp:ListItem Text="Cerrado" Value="7" />
                                                <asp:ListItem Text="Reabierto" Value="8" />
                                                <asp:ListItem Text="Cancelado" Value="9" />
                                              </asp:DropDownList>
                                              <div class="input-group-append">
                                                <asp:Button ID="modalBtnUpdateTicketStatus" runat="server"
                                                    CssClass="btn btn-warning"
                                                    Text="Actualizar"
                                                    OnClick="modalBtnUpdateTicketStatus_Click"
                                                    Enabled="false" />
                                              </div>
                                            </div>
                                          </dd>



                                          <dt class="col-sm-3">Prioridad</dt>
                                          <dd class="col-sm-9" id="modalPrioridad"></dd>

                                          <dt class="col-sm-3">Estatus</dt>
                                          <dd class="col-sm-9" id="modalEstatus"></dd>



                                          <dt class="col-sm-3">Descripción</dt>
                                          <dd class="col-sm-9" id="modalDescripcion"></dd>
                                        </dl>
 
                                      </div>
                                      <div class="modal-footer">
                                        <asp:HiddenField ID="modalHiddenTicketId" runat="server" />
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                                      </div>
                                    </div>
                                  </div>
                                </div>
                                <center>
                                    <h3 class="mb-4">Todos los Tickets</h3>
                                </center>
                            </div>
                        </div>

                        <!-- Search Section -->
                        <div class="row mb-2">
                            <div class="col">
                                <div class="card border-light">
                                    <div class="card-body p-3">
                                        <div class="row">
                                            <div class="col-md-8">
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-lg" placeholder="Buscar por asunto o descripción..." aria-label="Buscar"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary btn-block" Text="Buscar" OnClick="btnSearch_Click" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-secondary btn-block" Text="Limpiar Todo" OnClick="btnClear_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Advanced Filters Section -->
                        <div class="row mb-3">
                            <div class="col">
                                <div class="card border-light">
                                    <div class="card-header bg-light">
                                        <h6 class="mb-0">
                                            <i class="fas fa-filter"></i> Filtros Avanzados
                                        </h6>
                                    </div>
                                    <div class="card-body p-3">
                                        <div class="row">
                                            <!-- Date From -->
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label for="txtFechaFrom" class="form-label font-weight-bold">Desde:</label>
                                                    <asp:TextBox ID="txtFechaFrom" runat="server" CssClass="form-control" TextMode="Date" aria-label="Fecha desde"></asp:TextBox>
                                                    <small class="form-text text-muted">Fecha inicial</small>
                                                </div>
                                            </div>

                                            <!-- Date To -->
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label for="txtFechaTo" class="form-label font-weight-bold">Hasta:</label>
                                                    <asp:TextBox ID="txtFechaTo" runat="server" CssClass="form-control" TextMode="Date" aria-label="Fecha hasta"></asp:TextBox>
                                                    <small class="form-text text-muted">Fecha final</small>
                                                </div>
                                            </div>

                                            <!-- Status Filter -->
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label for="lbEstatus" class="form-label font-weight-bold">Estados:</label>
                                                    <asp:ListBox ID="lbEstatus" runat="server" CssClass="form-control" SelectionMode="Multiple" Rows="4">
                                                        <asp:ListItem Text="Nuevo" Value="1" />
                                                        <asp:ListItem Text="Abierto" Value="2" />
                                                        <asp:ListItem Text="En Progreso" Value="3" />
                                                        <asp:ListItem Text="En Espera" Value="4" />
                                                        <asp:ListItem Text="Escalado" Value="5" />
                                                        <asp:ListItem Text="Resuelto" Value="6" />
                                                        <asp:ListItem Text="Cerrado" Value="7" />
                                                        <asp:ListItem Text="Reabierto" Value="8" />
                                                        <asp:ListItem Text="Cancelado" Value="9" />
                                                    </asp:ListBox>
                                                    <small class="form-text text-muted d-block mt-2">
                                                        <kbd>Ctrl</kbd>+<kbd>Click</kbd> para seleccionar múltiples
                                                    </small>
                                                </div>
                                            </div>

                                            <!-- Apply Button -->
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label class="form-label">&nbsp;</label>
                                                    <asp:Button ID="btnApplyFilters" runat="server" CssClass="btn btn-info btn-block" Text="Aplicar" OnClick="btnApplyFilters_Click" />
                                                    <small class="form-text text-muted d-block mt-2 text-center">
                                                        Aplicar todos los filtros
                                                    </small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                                    SelectCommand="SELECT t.TicketId,(u.Nombre + ' ' + ISNULL(u.ApPaterno,'')) AS UsuarioNombre, t.Prioridad as Prioridad, t.CreadoUtc, t.AgenteId, ISNULL(a.nombre,'') AS AgenteNombre, ISNULL(a.estatus, 1) AS AgenteEstatus, t.Estatus, t.Asunto, t.Descripcion FROM [hd].[Ticket] t LEFT JOIN [hd].[Usuario] u ON t.UsuarioId = u.UsuarioId LEFT JOIN [hd].[Agente] a ON t.AgenteId = a.agenteId ORDER BY t.CreadoUtc DESC">
                                </asp:SqlDataSource>
                                <!-- DataSource for agent dropdown in tickets -->
                                <asp:SqlDataSource ID="SqlDataSourceAgentList" runat="server" ConnectionString="<%$ ConnectionStrings:ServerCon %>" SelectCommand="SELECT AgenteId, (nombre + ' | Nivel: ' + CAST(nivel AS nvarchar(10)) + ' | Asignados: ' + CAST(ISNULL(tAbiertos,0) AS nvarchar(10))) AS DisplayText FROM [hd].[agente]"></asp:SqlDataSource>
                                <asp:GridView class="table table-striped table-bordered table-sm"
                                    ID="GridView1" runat="server" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="TicketId" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Width="100%" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="TicketId" HeaderText="TicketId" InsertVisible="False" ReadOnly="True" SortExpression="TicketId" />
                                        <asp:TemplateField HeaderText="Prioridad" SortExpression="Prioridad">
                                            <ItemTemplate>
                                                <%# GetPriorityLabel(Eval("Prioridad")) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="UsuarioNombre" HeaderText="Nombre Usuario" SortExpression="UsuarioNombre" ReadOnly="True" />
                                        <asp:TemplateField HeaderText="Fecha Creación" SortExpression="CreadoUtc">
                                            <ItemTemplate>
                                                <%# FormatDateLocal(Eval("CreadoUtc")) %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AgenteNombre" HeaderText="Agente Asignado" ReadOnly="True" SortExpression="AgenteNombre" />
                                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" SortExpression="Estatus" />
                                        <asp:BoundField DataField="Asunto" HeaderText="Asunto" SortExpression="Asunto" />

                                         

                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                            </div>
                        </div>


                    </div>

                </div>
            </div>

        </div>
    </div>



</asp:Content>
