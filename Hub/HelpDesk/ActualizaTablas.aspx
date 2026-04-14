<%@ Page Title="Catálogos: Empresas, Puestos y SLA" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ActulizaTablas.aspx.cs" Inherits="HelpDesk.ActulizaTablas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table td, .table th { vertical-align: middle; }
        .form-note { font-size:.875rem; color:#6c757d; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <div class="row justify-content-center">
            <!-- Narrower, centered -->
            <div class="col-xl-6 col-lg-7">

                <!-- =========================
                     EMPRESAS
                     ========================= -->
                <div class="row mb-3">
                    <div class="col">
                        <h3 class="mb-0">Empresas</h3>
                        <div class="form-note">Agregar y modificar empresas</div>
                    </div>
                </div>

                <!-- Alta rápida de empresa -->
                <div class="row mb-3">
                    <div class="col">
                        <div class="input-group">
                            <asp:TextBox ID="txtNuevaEmpresa" runat="server" CssClass="form-control" MaxLength="150"
                                placeholder="Nombre de la nueva empresa" />
                            <div class="input-group-append">
                                <asp:Button ID="btnAgregarEmpresa" runat="server" CssClass="btn btn-primary"
                                    Text="Agregar" OnClick="btnAgregarEmpresa_Click"
                                    ValidationGroup="vgEmpresas" CausesValidation="true" />
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNuevaEmpresa" runat="server"
                            ControlToValidate="txtNuevaEmpresa" CssClass="text-danger"
                            ErrorMessage="El nombre de la empresa es obligatorio."
                            Display="Dynamic" ValidationGroup="vgEmpresas" />
                    </div>
                </div>

                <!-- Mensajes Empresas -->
                <div class="row mb-2">
                    <div class="col">
                        <asp:Label ID="lblMsgEmpresas" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                </div>

                <!-- DataSource Empresas -->
                <asp:SqlDataSource ID="SqlDataSourceEmpresa" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    CancelSelectOnNullParameter="false"
                    SelectCommand="SELECT EmpresaId, Nombre FROM [hd].[Empresa] ORDER BY Nombre"
                    InsertCommand="INSERT INTO [hd].[Empresa] (Nombre) VALUES (@Nombre)"
                    UpdateCommand="UPDATE [hd].[Empresa] SET Nombre = @Nombre WHERE EmpresaId = @EmpresaId"
                    DeleteCommand="DELETE FROM [hd].[Empresa] WHERE EmpresaId = @EmpresaId"
                    OnInserted="SqlDataSourceEmpresa_Inserted"
                    OnUpdated="SqlDataSourceEmpresa_Updated"
                    OnDeleted="SqlDataSourceEmpresa_Deleted">
                    <InsertParameters>
                        <asp:Parameter Name="Nombre" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Nombre" Type="String" />
                        <asp:Parameter Name="EmpresaId" Type="Int32" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="EmpresaId" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>

                <!-- Grid Empresas -->
                <div class="table-responsive mb-5">
                    <asp:GridView ID="gvEmpresas" runat="server"
                        CssClass="table table-striped table-bordered table-sm"
                        DataSourceID="SqlDataSourceEmpresa"
                        AutoGenerateColumns="False"
                        DataKeyNames="EmpresaId"
                        AllowPaging="true" PageSize="8"
                        AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="EmpresaId" HeaderText="ID" ReadOnly="true" Visible="false" />

                            <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                                <ItemTemplate>
                                    <%# Eval("Nombre") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombreEmpresaEdit" runat="server" CssClass="form-control" MaxLength="150"
                                        Text='<%# Bind("Nombre") %>' />
                                    <asp:RequiredFieldValidator ID="rfvNombreEmpresaEdit" runat="server"
                                        ControlToValidate="txtNombreEmpresaEdit" CssClass="text-danger"
                                        ErrorMessage="El nombre es obligatorio." Display="Dynamic"
                                        ValidationGroup="vgEmpresas" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- Acciones --%>
                            <asp:CommandField ShowEditButton="true" EditText="Editar" UpdateText="Guardar" CancelText="Cancelar" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEliminarEmpresa" runat="server" CommandName="Delete"
                                        CssClass="btn btn-sm btn-outline-danger"
                                        OnClientClick="return confirm('¿Eliminar esta empresa?');"
                                        Text="Eliminar" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <AlternatingRowStyle BackColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>

                <!-- =========================
                     PUESTOS
                     ========================= -->
                <div class="row mb-3">
                    <div class="col">
                        <h3 class="mb-0">Puestos</h3>
                        <div class="form-note">Agregar y modificar puestos.</div>
                    </div>
                </div>

                <!-- Alta rápida de puesto -->
                <div class="row mb-3">
                    <div class="col">
                        <div class="input-group">
                            <asp:TextBox ID="txtNuevoPuesto" runat="server" CssClass="form-control" MaxLength="100"
                                placeholder="Nombre del puesto" />
                            <asp:TextBox ID="txtNuevoPuestoDesc" runat="server" CssClass="form-control" MaxLength="300"
                                placeholder="Descripción (opcional)" />
                            <div class="input-group-append">
                                <asp:Button ID="btnAgregarPuesto" runat="server" CssClass="btn btn-primary"
                                    Text="Agregar" OnClick="btnAgregarPuesto_Click"
                                    ValidationGroup="vgPuestos" CausesValidation="true" />
                            </div>
                        </div>
                        <asp:RequiredFieldValidator ID="rfvNuevoPuesto" runat="server"
                            ControlToValidate="txtNuevoPuesto" CssClass="text-danger"
                            ErrorMessage="El nombre del puesto es obligatorio."
                            Display="Dynamic" ValidationGroup="vgPuestos" />
                    </div>
                </div>

                <!-- Mensajes Puestos -->
                <div class="row mb-2">
                    <div class="col">
                        <asp:Label ID="lblMsgPuestos" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                </div>

                <!-- DataSource Puestos -->
                <asp:SqlDataSource ID="SqlDataSourcePuesto" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    CancelSelectOnNullParameter="false"
                    SelectCommand="SELECT PuestoId, Nombre, Descripcion FROM [hd].[Puesto] ORDER BY Nombre"
                    InsertCommand="INSERT INTO [hd].[Puesto] (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)"
                    UpdateCommand="UPDATE [hd].[Puesto] SET Nombre = @Nombre, Descripcion = @Descripcion WHERE PuestoId = @PuestoId"
                    DeleteCommand="DELETE FROM [hd].[Puesto] WHERE PuestoId = @PuestoId"
                    OnInserted="SqlDataSourcePuesto_Inserted"
                    OnUpdated="SqlDataSourcePuesto_Updated"
                    OnDeleted="SqlDataSourcePuesto_Deleted">
                    <InsertParameters>
                        <asp:Parameter Name="Nombre" Type="String" />
                        <asp:Parameter Name="Descripcion" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Nombre" Type="String" />
                        <asp:Parameter Name="Descripcion" Type="String" />
                        <asp:Parameter Name="PuestoId" Type="Int32" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="PuestoId" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>

                <!-- Grid Puestos -->
                <div class="table-responsive mb-5">
                    <asp:GridView ID="gvPuestos" runat="server"
                        CssClass="table table-striped table-bordered table-sm"
                        DataSourceID="SqlDataSourcePuesto"
                        AutoGenerateColumns="False"
                        DataKeyNames="PuestoId"
                        AllowPaging="true" PageSize="8"
                        AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="PuestoId" HeaderText="ID" ReadOnly="true" Visible="false" />

                            <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                                <ItemTemplate>
                                    <%# Eval("Nombre") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombrePuestoEdit" runat="server" CssClass="form-control" MaxLength="100"
                                        Text='<%# Bind("Nombre") %>' />
                                    <asp:RequiredFieldValidator ID="rfvNombrePuestoEdit" runat="server"
                                        ControlToValidate="txtNombrePuestoEdit" CssClass="text-danger"
                                        ErrorMessage="El nombre es obligatorio." Display="Dynamic"
                                        ValidationGroup="vgPuestos" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Descripcion") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescPuestoEdit" runat="server" CssClass="form-control" MaxLength="300"
                                        TextMode="MultiLine" Rows="2" Text='<%# Bind("Descripcion") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- Acciones --%>
                            <asp:CommandField ShowEditButton="true" EditText="Editar" UpdateText="Guardar" CancelText="Cancelar" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEliminarPuesto" runat="server" CommandName="Delete"
                                        CssClass="btn btn-sm btn-outline-danger"
                                        OnClientClick="return confirm('¿Eliminar este puesto?');"
                                        Text="Eliminar" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <AlternatingRowStyle BackColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    </asp:GridView>
                </div>

                <%-- =========================
                     SLA
                     ========================= --%>
                <div class="row mb-3">
                    <div class="col">
                        <h3 class="mb-0">SLA</h3>
                        <div class="form-note">Agregar/Modificar SLA (Urgencia 1 mas urgente – 10 menos urgente)</div>
                    </div>
                </div>

                <%-- Alta rápida de SLA --%>
                <div class="row mb-3">
                    <div class="col">
                        <div class="input-group">
                            <asp:TextBox ID="txtNuevoSla" runat="server" CssClass="form-control" MaxLength="100"
                                placeholder="Nombre del SLA" />
                            <asp:TextBox ID="txtNuevoSlaUrgencia" runat="server" CssClass="form-control"
                                placeholder="Urgencia (1-10)" />
                            <div class="input-group-append">
                                <asp:Button ID="btnAgregarSla" runat="server" CssClass="btn btn-primary"
                                    Text="Agregar" OnClick="btnAgregarSla_Click"
                                    ValidationGroup="vgSla" CausesValidation="true" />
                            </div>
                        </div>

                        <%-- Validaciones SLA (alta) --%>
                        <asp:RequiredFieldValidator ID="rfvNuevoSlaNombre" runat="server"
                            ControlToValidate="txtNuevoSla" CssClass="text-danger"
                            ErrorMessage="El nombre del SLA es obligatorio."
                            Display="Dynamic" ValidationGroup="vgSla" />
                        <br />
                        <asp:RequiredFieldValidator ID="rfvNuevoSlaUrgencia" runat="server"
                            ControlToValidate="txtNuevoSlaUrgencia" CssClass="text-danger"
                            ErrorMessage="La urgencia es obligatoria."
                            Display="Dynamic" ValidationGroup="vgSla" />
                        <asp:RangeValidator ID="rngNuevoSlaUrgencia" runat="server"
                            ControlToValidate="txtNuevoSlaUrgencia" CssClass="text-danger"
                            ErrorMessage="La urgencia debe estar entre 1 y 10."
                            Type="Integer" MinimumValue="1" MaximumValue="10"
                            Display="Dynamic" ValidationGroup="vgSla" />
                    </div>
                </div>

                <!-- Mensajes SLA -->
                <div class="row mb-2">
                    <div class="col">
                        <asp:Label ID="lblMsgSla" runat="server" EnableViewState="false"></asp:Label>
                    </div>
                </div>

                <%-- DataSource SLA --%>
                <asp:SqlDataSource ID="SqlDataSourceSla" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
                    CancelSelectOnNullParameter="false"
                    SelectCommand="SELECT SLAId, Nombre, Urgencia FROM [hd].[SLA] ORDER BY Nombre"
                    InsertCommand="INSERT INTO [hd].[SLA] (Nombre, Urgencia) VALUES (@Nombre, @Urgencia)"
                    UpdateCommand="UPDATE [hd].[SLA] SET Nombre = @Nombre, Urgencia = @Urgencia WHERE SLAId = @SLAId"
                    DeleteCommand="DELETE FROM [hd].[SLA] WHERE SLAId = @SLAId"
                    OnInserted="SqlDataSourceSla_Inserted"
                    OnUpdated="SqlDataSourceSla_Updated"
                    OnDeleted="SqlDataSourceSla_Deleted">
                    <InsertParameters>
                        <asp:Parameter Name="Nombre" Type="String" />
                        <asp:Parameter Name="Urgencia" Type="Int32" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Nombre" Type="String" />
                        <asp:Parameter Name="Urgencia" Type="Int32" />
                        <asp:Parameter Name="SLAId" Type="Int32" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="SLAId" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>

                <%-- Grid SLA --%>
                <div class="table-responsive mb-5">
                    <asp:GridView ID="gvSla" runat="server"
                        CssClass="table table-striped table-bordered table-sm"
                        DataSourceID="SqlDataSourceSla"
                        AutoGenerateColumns="False"
                        DataKeyNames="SLAId"
                        AllowPaging="true" PageSize="8"
                        AllowSorting="true">
                        <Columns>
                            <asp:BoundField DataField="SLAId" HeaderText="ID" ReadOnly="true" Visible="false" />

                            <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                                <ItemTemplate>
                                    <%# Eval("Nombre") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtNombreSlaEdit" runat="server" CssClass="form-control" MaxLength="100"
                                        Text='<%# Bind("Nombre") %>' />
                                    <asp:RequiredFieldValidator ID="rfvNombreSlaEdit" runat="server"
                                        ControlToValidate="txtNombreSlaEdit" CssClass="text-danger"
                                        ErrorMessage="El nombre es obligatorio." Display="Dynamic"
                                        ValidationGroup="vgSla" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Urgencia" SortExpression="Urgencia">
                                <ItemTemplate>
                                    <%# Eval("Urgencia") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUrgenciaSlaEdit" runat="server" CssClass="form-control"
                                        Text='<%# Bind("Urgencia") %>' />
                                    <asp:RequiredFieldValidator ID="rfvUrgenciaSlaEdit" runat="server"
                                        ControlToValidate="txtUrgenciaSlaEdit" CssClass="text-danger"
                                        ErrorMessage="La urgencia es obligatoria." Display="Dynamic"
                                        ValidationGroup="vgSla" />
                                    <asp:RangeValidator ID="rngUrgenciaSlaEdit" runat="server"
                                        ControlToValidate="txtUrgenciaSlaEdit" CssClass="text-danger"
                                        ErrorMessage="La urgencia debe estar entre 1 y 10."
                                        Type="Integer" MinimumValue="1" MaximumValue="10"
                                        Display="Dynamic" ValidationGroup="vgSla" />
                                </EditItemTemplate>
                            </asp:TemplateField>

                            <%-- Acciones --%>
                            <asp:CommandField ShowEditButton="true" EditText="Editar" UpdateText="Guardar" CancelText="Cancelar" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEliminarSla" runat="server" CommandName="Delete"
                                        CssClass="btn btn-sm btn-outline-danger"
                                        OnClientClick="return confirm('¿Eliminar este SLA?');"
                                        Text="Eliminar" CausesValidation="false" />
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
    </div>

</asp:Content>
