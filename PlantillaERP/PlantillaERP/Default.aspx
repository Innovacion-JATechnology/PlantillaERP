<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PlantillaERP._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-header mb-4">
        <h1 class="page-title">Inicio</h1>
    </div>

    <div class="modules-grid">
        <!-- Compras -->
        <a href="ModuleDetails.aspx?module=compras" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-shopping-cart"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Compras</h3>
                <p class="module-description">Compras Foraneas • OC • Proveedores</p>
            </div>
        </a>

        <!-- Inventario -->
        <a href="ModuleDetails.aspx?module=inventario" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-box"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Inventario</h3>
                <p class="module-description">Stock • Almacenes • Movimientos</p>
            </div>
        </a>

        <!-- Finanzas -->
        <a href="ModuleDetails.aspx?module=finanzas" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-money-bill-wave"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Finanzas</h3>
                <p class="module-description">Contabilidad • CxP • CxC</p>
            </div>
        </a>

        <!-- Mantenimiento -->
        <a href="ModuleDetails.aspx?module=mantenimiento" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-wrench"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Mantenimiento</h3>
                <p class="module-description">Equipos • Preventivo • Correctivo</p>
            </div>
        </a>

        <!-- Produccion -->
        <a href="ModuleDetails.aspx?module=produccion" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-cog"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Produccion</h3>
                <p class="module-description">LdMateriales • Ordenes • Materiales RP</p>
            </div>
        </a>

        <!-- RRHH -->
        <a href="ModuleDetails.aspx?module=rrhh" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-users"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">RRHH</h3>
                <p class="module-description">Empleados • Nomina • Asistencias</p>
            </div>
        </a>

        <!-- Proyectos -->
        <a href="ModuleDetails.aspx?module=proyectos" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-building"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Proyectos</h3>
                <p class="module-description">Tareas • Costos • Facturacion</p>
            </div>
        </a>

        <!-- Reporteador -->
        <a href="ModuleDetails.aspx?module=reporteador" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-chart-line"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Reporteador</h3>
                <p class="module-description">KPIs • Reportes • BI</p>
            </div>
        </a>

        <!-- Administracion -->
        <a href="ModuleDetails.aspx?module=administracion" runat="server" class="module-card">
            <div class="module-icon">
                <i class="fas fa-sliders-h"></i>
            </div>
            <div class="module-content">
                <h3 class="module-title">Administracion</h3>
                <p class="module-description">Usuarios • Roles • Configuracion</p>
            </div>
        </a>
    </div>

</asp:Content>
