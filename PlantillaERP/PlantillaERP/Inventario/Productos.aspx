<%@ Page Title="Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Maqueta.Inventario.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Contenedor angosto y centrado para la tabla y el formulario */
        .narrow-wrap {
            max-width: 720px;   /* ajusta: 520–820px  */
            margin-left: auto;
            margin-right: auto;
        }
        .search-bar .form-control { max-width: 360px; }
    </style>
    <div class="narrow-wrap">

        <!-- Título + Buscador -->
        <div class="d-flex flex-wrap align-items-center justify-content-between mb-3">
            <h4 class="section-title m-0">  Catálogo</h4>

            <div class="search-bar">
                <div class="input-group">
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar producto o talla..." />
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-outline-secondary" OnClick="btnLimpiar_Click" />
                </div>
            </div>
        </div>

        <!-- Mensajes -->
        <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-2 text-muted"></asp:Label>

       

        <!-- Grid angosta -->
        <asp:GridView ID="gvProductos" runat="server"
            CssClass="table table-sm table-hove"
            AutoGenerateColumns="False"
            DataKeyNames="Id,Producto,Talla"
            AllowPaging="true" PageSize="10"
            AllowSorting="true"
            OnPageIndexChanging="gvProductos_PageIndexChanging"
            OnSorting="gvProductos_Sorting"
            OnRowCommand="gvProductos_RowCommand">

            <Columns> 
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                 
                <asp:BoundField DataField="ProductoCompleto" HeaderText="Producto" SortExpression="ProductoCompleto" />
                 
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditar" runat="server"
                            CssClass="btn btn-sm btn-outline-primary me-1"
                            CommandName="Cargar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>">
                            Editar
                        </asp:LinkButton>

                        <asp:LinkButton ID="lnkEliminar" runat="server"
                            CssClass="btn btn-sm btn-outline-danger"
                            CommandName="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>"
                            OnClientClick="return confirm('¿Eliminar este producto?');">
                            Eliminar
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

            <EmptyDataTemplate>
                <div class="text-muted">No hay productos para mostrar.</div>
            </EmptyDataTemplate>
        </asp:GridView>

              <!-- Formulario compacto: Agregar / Editar --> 
      
<div class="card mb-3">
          <div class="card-body">
    <h5>Modificar/Agregar Productos</h5>
              <asp:HiddenField ID="hfId" runat="server" />

              <div class="row g-3 align-items-end">
                  <!-- NUEVO: Id -->
                  <div class="col-12 col-md-3">
                      <label class="form-label">Id</label>
                      <asp:TextBox ID="txtId" runat="server"
                                   CssClass="form-control"
                                   MaxLength="50"
                                   placeholder="Ej. SKU-001" />
                  </div>

                  <div class="col-12 col-md-5">
                      <label class="form-label">Producto</label>
                      <asp:TextBox ID="txtProducto" runat="server" CssClass="form-control" MaxLength="150" />
                  </div>
                  <div class="col-6 col-md-2">
                      <label class="form-label">Talla</label>
                      <asp:TextBox ID="txtTalla" runat="server" CssClass="form-control" MaxLength="50" />
                  </div>
                  <div class="col-6 col-md-2 d-grid">
                      <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success"
                          OnClick="btnGuardar_Click" />
                      <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-secondary mt-2"
                          OnClick="btnCancelar_Click" />
                  </div>
              </div>
          </div>
      </div>


    </div>
</asp:Content>