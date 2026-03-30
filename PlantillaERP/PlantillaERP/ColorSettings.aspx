<%@ Page Title="Configuracion de Colores" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ColorSettings.aspx.cs" Inherits="PlantillaERP.ColorSettings" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container-fluid">
        <h1 class="mt-4">Configuracion de Colores</h1>
        <ol class="breadcrumb mb-4">
            <li class="breadcrumb-item"><a href="Default.aspx">Dashboard</a></li>
            <li class="breadcrumb-item active">Configuracion de Colores</li>
        </ol>

        <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false" style="margin-bottom: 20px;"></asp:Label>

        <!-- Tabs Navigation -->
        <ul class="nav nav-tabs mb-4" id="colorTabs" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active" id="navbar-tab" data-bs-toggle="tab" data-bs-target="#navbar-content" type="button" role="tab" aria-controls="navbar-content" aria-selected="true">
                    <i class="fas fa-window-maximize me-2"></i>Barra de Navegacion (Navbar)
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="sidebar-tab" data-bs-toggle="tab" data-bs-target="#sidebar-content" type="button" role="tab" aria-controls="sidebar-content" aria-selected="false">
                    <i class="fas fa-bars me-2"></i>Barra Lateral (Sidebar)
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="modules-tab" data-bs-toggle="tab" data-bs-target="#modules-content" type="button" role="tab" aria-controls="modules-content" aria-selected="false">
                    <i class="fas fa-paint-brush me-2"></i>Colores de Modulos
                </button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="sidebar-hover-tab" data-bs-toggle="tab" data-bs-target="#sidebar-hover-content" type="button" role="tab" aria-controls="sidebar-hover-content" aria-selected="false">
                    <i class="fas fa-mouse me-2"></i>Hover del Sidebar
                </button>
            </li>
        </ul>

        <!-- Tabs Content -->
        <div class="tab-content" id="colorTabsContent">
            <!-- Navbar Tab -->
            <div class="tab-pane fade show active" id="navbar-content" role="tabpanel" aria-labelledby="navbar-tab">
                <div class="card mb-4">
                    <div class="card-header bg-primary text-white">
                        <i class="fas fa-window-maximize me-2"></i>Colores de la Barra de Navegacion
                    </div>
                    <div class="card-body">
                        <p class="text-muted mb-4">La barra de navegacion utiliza un degradado con 3 colores (angulo 45 grados).</p>
                        
                        <div class="row">
                            <div class="col-lg-6">
                                <!-- Color 1 -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color 1 - Inicio del Degradado</label>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtNavbar1R" runat="server" CssClass="form-control" placeholder="87" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtNavbar1G" runat="server" CssClass="form-control" placeholder="179" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtNavbar1B" runat="server" CssClass="form-control" placeholder="252" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerNavbar1" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewNavbar1" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>

                                <!-- Color 2 -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color 2 - Medio del Degradado</label>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtNavbar2R" runat="server" CssClass="form-control" placeholder="165" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtNavbar2G" runat="server" CssClass="form-control" placeholder="95" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtNavbar2B" runat="server" CssClass="form-control" placeholder="253" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerNavbar2" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewNavbar2" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>

                                <!-- Color 3 -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color 3 - Final del Degradado</label>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtNavbar3R" runat="server" CssClass="form-control" placeholder="16" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtNavbar3G" runat="server" CssClass="form-control" placeholder="55" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtNavbar3B" runat="server" CssClass="form-control" placeholder="161" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerNavbar3" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewNavbar3" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>
                            </div>

                            <!-- Preview -->
                            <div class="col-lg-6">
                                <div class="card h-100">
                                    <div class="card-header bg-light">
                                        <i class="fas fa-eye me-2"></i><strong>Vista Previa</strong>
                                    </div>
                                    <div class="card-body d-flex align-items-center justify-content-center">
                                        <div id="previewNavbarFull" style="width: 100%; height: 200px; border-radius: 8px; display: flex; align-items: center; justify-content: center; color: white; font-weight: bold; font-size: 18px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                                            Vista previa del degradado
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnSaveNavbar" runat="server" Text="Guardar Cambios" CssClass="btn btn-success btn-lg me-2" OnClick="btnSave_Click" />
                        <asp:Button ID="btnResetNavbar" runat="server" Text="Restablecer Valores Predeterminados" CssClass="btn btn-warning btn-lg" OnClick="btnReset_Click" />
                    </div>
                </div>
            </div>
            <div class="tab-pane fade" id="sidebar-content" role="tabpanel" aria-labelledby="sidebar-tab">
                <div class="card mb-4">
                    <div class="card-header bg-info text-white">
                        <i class="fas fa-bars me-2"></i>Colores de la Barra Lateral
                    </div>
                    <div class="card-body">
                        <p class="text-muted mb-4">La barra lateral utiliza un degradado con 2 colores (angulo 90 grados).</p>
                        
                        <div class="row">
                            <div class="col-lg-6">
                                <!-- Color 1 -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color 1 - Inicio del Degradado</label>
                                    <div class="row g-2">
                                        <div class="col-md-4">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtSidebar1R" runat="server" CssClass="form-control" placeholder="87" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtSidebar1G" runat="server" CssClass="form-control" placeholder="179" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtSidebar1B" runat="server" CssClass="form-control" placeholder="252" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row g-2 mt-2">
                                        <div class="col-12">
                                            <label class="form-label small">Selector de Color</label>
                                            <input type="color" id="colorPickerSidebar1" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewSidebar1" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>

                                <!-- Color 2 -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color 2 - Final del Degradado</label>
                                    <div class="row g-2">
                                        <div class="col-md-4">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtSidebar2R" runat="server" CssClass="form-control" placeholder="130" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtSidebar2G" runat="server" CssClass="form-control" placeholder="157" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtSidebar2B" runat="server" CssClass="form-control" placeholder="245" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row g-2 mt-2">
                                        <div class="col-12">
                                            <label class="form-label small">Selector de Color</label>
                                            <input type="color" id="colorPickerSidebar2" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewSidebar2" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>
                            </div>

                            <!-- Preview -->
                            <div class="col-lg-6">
                                <div class="card h-100">
                                    <div class="card-header bg-light">
                                        <i class="fas fa-eye me-2"></i><strong>Vista Previa</strong>
                                    </div>
                                    <div class="card-body d-flex align-items-center justify-content-center">
                                        <div id="previewSidebarFull" style="width: 100%; height: 200px; border-radius: 8px; display: flex; align-items: center; justify-content: center; color: white; font-weight: bold; font-size: 18px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);">
                                            Vista previa del degradado
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnSidebarSave" runat="server" Text="Guardar Cambios" CssClass="btn btn-success btn-lg me-2" OnClick="btnSave_Click" />
                        <asp:Button ID="btnSidebarReset" runat="server" Text="Restablecer Valores Predeterminados" CssClass="btn btn-warning btn-lg" OnClick="btnReset_Click" />
                    </div>
                </div>
            </div>

            <!-- Modules Tab -->
            <div class="tab-pane fade" id="modules-content" role="tabpanel" aria-labelledby="modules-tab">
                <div class="card mb-4">
                    <div class="card-header bg-warning text-dark">
                        <i class="fas fa-paint-brush me-2"></i>Colores de los Recuadros de Modulos
                    </div>
                    <div class="card-body">
                        <p class="text-muted mb-4">Personaliza los colores de los bordes de los recuadros de los modulos. Los colores por defecto coinciden con los del navbar.</p>

                        <div class="row">
                            <div class="col-lg-6">
                                <!-- Color 1 -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color de la Orilla (Reposo)</label>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtModule1R" runat="server" CssClass="form-control" placeholder="87" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtModule1G" runat="server" CssClass="form-control" placeholder="179" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtModule1B" runat="server" CssClass="form-control" placeholder="252" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerModule1" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewModule1" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>

                                <!-- Color 2 -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Hover: Color de la orilla </label>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtModule2R" runat="server" CssClass="form-control" placeholder="165" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtModule2G" runat="server" CssClass="form-control" placeholder="95" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtModule2B" runat="server" CssClass="form-control" placeholder="253" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerModule2" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewModule2" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>

                                <!-- Color 3 - Recuadro Hover Background -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color del Recuadro en Hover</label>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtModule3R" runat="server" CssClass="form-control" placeholder="26" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtModule3G" runat="server" CssClass="form-control" placeholder="26" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtModule3B" runat="server" CssClass="form-control" placeholder="42" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerModule3" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewModule3" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>
                            </div>

                            <!-- Preview -->
                            <div class="col-lg-6">
                                <div class="card h-100">
                                    <div class="card-header bg-light">
                                        <i class="fas fa-eye me-2"></i><strong>Vista Previa</strong>
                                    </div>
                                    <div class="card-body d-flex align-items-center justify-content-center">
                                        <div style="width: 100%; display: flex; flex-direction: column; gap: 20px;">
                                            <!-- Normal State -->
                                            <div style="text-align: center;">
                                                <p class="small text-muted mb-2">Estado Normal</p>
                                                <div id="previewModuleFull" style="width: 100%; height: 120px; border-radius: 15px; border: 3px solid; background-color: white; display: flex; align-items: center; justify-content: center; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                                    <div style="text-align: center;">
                                                        <i id="previewIcon" class="fas fa-shopping-cart" style="font-size: 2rem; margin-bottom: 5px; display: block; color: rgb(87, 179, 252) !important;"></i>
                                                        <strong style="color: rgb(16,55,161);">Modulo</strong>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- Hover State -->
                                            <div style="text-align: center;">
                                                <p class="small text-muted mb-2">Estado Hover (Tono Oscuro)</p>
                                                <div id="previewModuleHover" style="width: 100%; height: 120px; border-radius: 15px; border: 3px solid; background-color: #1a1a2a; display: flex; align-items: center; justify-content: center; box-shadow: 0 4px 12px rgba(0,0,0,0.2); transform: translateY(-4px);">
                                                    <div style="text-align: center;">
                                                        <i id="previewIconHover" class="fas fa-shopping-cart" style="font-size: 2rem; margin-bottom: 5px; display: block; color: rgb(165,95,253);"></i>
                                                        <strong style="color: rgb(87,179,252);">Modulo</strong>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnModulesSave" runat="server" Text="Guardar Cambios" CssClass="btn btn-success btn-lg me-2" OnClick="btnSave_Click" />
                        <asp:Button ID="btnModulesReset" runat="server" Text="Restablecer Valores Predeterminados" CssClass="btn btn-warning btn-lg" OnClick="btnReset_Click" />
                    </div>
                </div>
            </div>

            <!-- Sidebar Hover Tab -->
            <div class="tab-pane fade" id="sidebar-hover-content" role="tabpanel" aria-labelledby="sidebar-hover-tab">
                <div class="card mb-4">
                    <div class="card-header bg-success text-white">
                        <i class="fas fa-mouse me-2"></i>Hover del Sidebar
                    </div>
                    <div class="card-body">
                        <p class="text-muted mb-4">Personaliza los colores que aparecen cuando pasas el ratón sobre los elementos del menú lateral.</p>

                        <div class="row">
                            <div class="col-lg-6">
                                <!-- Sidebar Hover Border Color (Left Bar) -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color del Borde Izquierdo en Hover</label>
                                    <p class="small text-muted">La pequeña barra que aparece a la izquierda cuando pasas el mouse</p>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtSidebarHoverBorderR" runat="server" CssClass="form-control" placeholder="165" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtSidebarHoverBorderG" runat="server" CssClass="form-control" placeholder="95" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtSidebarHoverBorderB" runat="server" CssClass="form-control" placeholder="253" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerSidebarHoverBorder" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewSidebarHoverBorder" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>

                                <!-- Sidebar Hover Background Color -->
                                <div class="mb-4">
                                    <label class="form-label fw-bold">Color de Fondo en Hover</label>
                                    <p class="small text-muted">Transparencia del fondo cuando pasas el mouse (RGBA)</p>
                                    <div class="row g-2">
                                        <div class="col-md-3">
                                            <label class="form-label small">Rojo (0-255)</label>
                                            <asp:TextBox ID="txtSidebarHoverR" runat="server" CssClass="form-control" placeholder="255" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Verde (0-255)</label>
                                            <asp:TextBox ID="txtSidebarHoverG" runat="server" CssClass="form-control" placeholder="255" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Azul (0-255)</label>
                                            <asp:TextBox ID="txtSidebarHoverB" runat="server" CssClass="form-control" placeholder="255" type="number" min="0" max="255"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <label class="form-label small">Selector</label>
                                            <input type="color" id="colorPickerSidebarHover" class="form-control form-control-color" title="Seleccionar color" />
                                        </div>
                                    </div>
                                    <div id="previewSidebarHover" class="mt-2" style="height: 40px; border-radius: 5px; border: 1px solid #ddd;"></div>
                                </div>
                            </div>

                            <!-- Preview -->
                            <div class="col-lg-6">
                                <div class="card h-100">
                                    <div class="card-header bg-light">
                                        <i class="fas fa-eye me-2"></i><strong>Vista Previa</strong>
                                    </div>
                                    <div class="card-body d-flex align-items-center justify-content-center">
                                        <div style="width: 100%; display: flex; flex-direction: column; gap: 20px;">
                                            <!-- Normal State -->
                                            <div style="text-align: center;">
                                                <p class="small text-muted mb-2">Estado Normal</p>
                                                <div id="previewSidebarNormal" style="width: 100%; height: 60px; border-radius: 8px; background: linear-gradient(90deg, rgb(87, 179, 252), rgb(130, 157, 245)); display: flex; align-items: center; justify-content: center; color: white; font-weight: bold; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                                    <i class="fas fa-shopping-cart me-2"></i>Elemento del Sidebar
                                                </div>
                                            </div>
                                            <!-- Hover State -->
                                            <div style="text-align: center;">
                                                <p class="small text-muted mb-2">Estado Hover (con borde izquierdo)</p>
                                                <div id="previewSidebarHoverPreview" style="width: 100%; height: 60px; border-radius: 8px; display: flex; align-items: center; justify-content: center; color: white; font-weight: bold; box-shadow: 0 2px 4px rgba(0,0,0,0.1);">
                                                    <i class="fas fa-shopping-cart me-2"></i>Elemento del Sidebar (Hover)
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer bg-light">
                        <asp:Button ID="btnSidebarHoverSave" runat="server" Text="Guardar Cambios" CssClass="btn btn-success btn-lg me-2" OnClick="btnSave_Click" />
                        <asp:Button ID="btnSidebarHoverReset" runat="server" Text="Restablecer Valores Predeterminados" CssClass="btn btn-warning btn-lg" OnClick="btnReset_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            // Navbar Color Inputs
            const navbar1R = document.getElementById('<%= txtNavbar1R.ClientID %>');
            const navbar1G = document.getElementById('<%= txtNavbar1G.ClientID %>');
            const navbar1B = document.getElementById('<%= txtNavbar1B.ClientID %>');
            const navbar2R = document.getElementById('<%= txtNavbar2R.ClientID %>');
            const navbar2G = document.getElementById('<%= txtNavbar2G.ClientID %>');
            const navbar2B = document.getElementById('<%= txtNavbar2B.ClientID %>');
            const navbar3R = document.getElementById('<%= txtNavbar3R.ClientID %>');
            const navbar3G = document.getElementById('<%= txtNavbar3G.ClientID %>');
            const navbar3B = document.getElementById('<%= txtNavbar3B.ClientID %>');

            // Sidebar Color Inputs
            const sidebar1R = document.getElementById('<%= txtSidebar1R.ClientID %>');
            const sidebar1G = document.getElementById('<%= txtSidebar1G.ClientID %>');
            const sidebar1B = document.getElementById('<%= txtSidebar1B.ClientID %>');
            const sidebar2R = document.getElementById('<%= txtSidebar2R.ClientID %>');
            const sidebar2G = document.getElementById('<%= txtSidebar2G.ClientID %>');
            const sidebar2B = document.getElementById('<%= txtSidebar2B.ClientID %>');

            // Sidebar Hover Color Inputs
            const sidebarHoverR = document.getElementById('<%= txtSidebarHoverR.ClientID %>');
            const sidebarHoverG = document.getElementById('<%= txtSidebarHoverG.ClientID %>');
            const sidebarHoverB = document.getElementById('<%= txtSidebarHoverB.ClientID %>');

            // Sidebar Hover Border Color Inputs (NEW)
            const sidebarHoverBorderR = document.getElementById('<%= txtSidebarHoverBorderR.ClientID %>');
            const sidebarHoverBorderG = document.getElementById('<%= txtSidebarHoverBorderG.ClientID %>');
            const sidebarHoverBorderB = document.getElementById('<%= txtSidebarHoverBorderB.ClientID %>');

            // Module Color Inputs
            const module1R = document.getElementById('<%= txtModule1R.ClientID %>');
            const module1G = document.getElementById('<%= txtModule1G.ClientID %>');
            const module1B = document.getElementById('<%= txtModule1B.ClientID %>');
            const module2R = document.getElementById('<%= txtModule2R.ClientID %>');
            const module2G = document.getElementById('<%= txtModule2G.ClientID %>');
            const module2B = document.getElementById('<%= txtModule2B.ClientID %>');
            const module3R = document.getElementById('<%= txtModule3R.ClientID %>');
            const module3G = document.getElementById('<%= txtModule3G.ClientID %>');
            const module3B = document.getElementById('<%= txtModule3B.ClientID %>');

            // Color Pickers
            const colorPickerNavbar1 = document.getElementById('colorPickerNavbar1');
            const colorPickerNavbar2 = document.getElementById('colorPickerNavbar2');
            const colorPickerNavbar3 = document.getElementById('colorPickerNavbar3');
            const colorPickerSidebar1 = document.getElementById('colorPickerSidebar1');
            const colorPickerSidebar2 = document.getElementById('colorPickerSidebar2');
            const colorPickerSidebarHover = document.getElementById('colorPickerSidebarHover');
            const colorPickerSidebarHoverBorder = document.getElementById('colorPickerSidebarHoverBorder');
            const colorPickerModule1 = document.getElementById('colorPickerModule1');
            const colorPickerModule2 = document.getElementById('colorPickerModule2');
            const colorPickerModule3 = document.getElementById('colorPickerModule3');

            // Previews
            const previewNavbar1 = document.getElementById('previewNavbar1');
            const previewNavbar2 = document.getElementById('previewNavbar2');
            const previewNavbar3 = document.getElementById('previewNavbar3');
            const previewNavbarFull = document.getElementById('previewNavbarFull');
            const previewSidebar1 = document.getElementById('previewSidebar1');
            const previewSidebar2 = document.getElementById('previewSidebar2');
            const previewSidebarFull = document.getElementById('previewSidebarFull');
            const previewSidebarHover = document.getElementById('previewSidebarHover');
            const previewSidebarHoverBorder = document.getElementById('previewSidebarHoverBorder');
            const previewSidebarNormal = document.getElementById('previewSidebarNormal');
            const previewSidebarHoverPreview = document.getElementById('previewSidebarHoverPreview');
            const previewModule1 = document.getElementById('previewModule1');
            const previewModule2 = document.getElementById('previewModule2');
            const previewModule3 = document.getElementById('previewModule3');
            const previewModuleFull = document.getElementById('previewModuleFull');
            const previewModuleHover = document.getElementById('previewModuleHover');
            const previewIcon = document.getElementById('previewIcon');
            const previewIconHover = document.getElementById('previewIconHover');

            function rgbToHex(r, g, b) {
                return "#" + ((1 << 24) + (parseInt(r) << 16) + (parseInt(g) << 8) + parseInt(b)).toString(16).slice(1);
            }

            function hexToRgb(hex) {
                const result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
                return result ? [parseInt(result[1], 16), parseInt(result[2], 16), parseInt(result[3], 16)] : [0, 0, 0];
            }

            function updatePreview() {
                const navbar1RGB = `rgb(${navbar1R.value || 87}, ${navbar1G.value || 179}, ${navbar1B.value || 252})`;
                const navbar2RGB = `rgb(${navbar2R.value || 165}, ${navbar2G.value || 95}, ${navbar2B.value || 253})`;
                const navbar3RGB = `rgb(${navbar3R.value || 16}, ${navbar3G.value || 55}, ${navbar3B.value || 161})`;
                const sidebar1RGB = `rgb(${sidebar1R.value || 87}, ${sidebar1G.value || 179}, ${sidebar1B.value || 252})`;
                const sidebar2RGB = `rgb(${sidebar2R.value || 130}, ${sidebar2G.value || 157}, ${sidebar2B.value || 245})`;
                const sidebarHoverRGB = `rgb(${sidebarHoverR.value || 255}, ${sidebarHoverG.value || 255}, ${sidebarHoverB.value || 255})`;
                const sidebarHoverBorderRGB = `rgb(${sidebarHoverBorderR.value || 165}, ${sidebarHoverBorderG.value || 95}, ${sidebarHoverBorderB.value || 253})`;
                const module1RGB = `rgb(${module1R.value || 87}, ${module1G.value || 179}, ${module1B.value || 252})`;
                const module2RGB = `rgb(${module2R.value || 165}, ${module2G.value || 95}, ${module2B.value || 253})`;
                const module3RGB = `rgb(${module3R.value || 26}, ${module3G.value || 26}, ${module3B.value || 42})`;

                // Individual color previews
                if (previewNavbar1) previewNavbar1.style.background = navbar1RGB;
                if (previewNavbar2) previewNavbar2.style.background = navbar2RGB;
                if (previewNavbar3) previewNavbar3.style.background = navbar3RGB;
                if (previewSidebar1) previewSidebar1.style.background = sidebar1RGB;
                if (previewSidebar2) previewSidebar2.style.background = sidebar2RGB;
                if (previewSidebarHover) previewSidebarHover.style.background = sidebarHoverRGB;
                if (previewSidebarHoverBorder) previewSidebarHoverBorder.style.background = sidebarHoverBorderRGB;
                if (previewModule1) previewModule1.style.background = module1RGB;
                if (previewModule2) previewModule2.style.background = module2RGB;
                if (previewModule3) previewModule3.style.background = module3RGB;

                // Navbar and Sidebar full previews
                if (previewNavbarFull) previewNavbarFull.style.background = `linear-gradient(45deg, ${navbar1RGB}, ${navbar2RGB}, ${navbar3RGB})`;
                if (previewSidebarFull) previewSidebarFull.style.background = `linear-gradient(90deg, ${sidebar1RGB}, ${sidebar2RGB})`;

                // Sidebar Hover preview
                if (previewSidebarNormal) previewSidebarNormal.style.background = `linear-gradient(90deg, ${sidebar1RGB}, ${sidebar2RGB})`;
                if (previewSidebarHoverPreview) {
                    // Mostrar cómo se verá con rgba transparencia
                    const hoverR = sidebarHoverR.value || 255;
                    const hoverG = sidebarHoverG.value || 255;
                    const hoverB = sidebarHoverB.value || 255;

                    // Crear el CSS completo con múltiples backgrounds
                    const backgroundGradient = `linear-gradient(90deg, rgba(${hoverR}, ${hoverG}, ${hoverB}, 0.2), rgba(${hoverR}, ${hoverG}, ${hoverB}, 0.2)), linear-gradient(90deg, ${sidebar1RGB}, ${sidebar2RGB})`;

                    // Aplicar mediante cssText para limpiar estilos previos completamente
                    previewSidebarHoverPreview.style.cssText = `
                        width: 100%;
                        height: 60px;
                        border-radius: 8px;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        color: white;
                        font-weight: bold;
                        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
                        border-left: 4px solid ${sidebarHoverBorderRGB};
                        background: ${backgroundGradient} !important;
                        background-size: 100% 100%, 100% 100% !important;
                    `;
                }

                // Module card - Normal state
                if (previewModuleFull) {
                    previewModuleFull.style.borderColor = module1RGB;
                    previewModuleFull.style.backgroundColor = '#ffffff';
                }

                // Module card - Hover state  
                if (previewModuleHover) {
                    previewModuleHover.style.borderColor = module2RGB;
                    previewModuleHover.style.backgroundColor = module3RGB;
                }

                // Module icon - Hover state (Icono en Hover)
                if (previewIconHover) {
                    previewIconHover.style.color = module2RGB;
                    previewIconHover.style.setProperty('color', module2RGB, 'important');
                }
            }

            function syncRGBToHex(r, g, b, colorPicker) {
                colorPicker.value = rgbToHex(r.value || 0, g.value || 0, b.value || 0);
            }

            function syncHexToRGB(hex, r, g, b, skipUpdate = false) {
                const rgb = hexToRgb(hex);
                r.value = rgb[0];
                g.value = rgb[1];
                b.value = rgb[2];
                if (!skipUpdate) {
                    updatePreview();
                }
            }

            // Initialize color pickers
            syncRGBToHex(navbar1R, navbar1G, navbar1B, colorPickerNavbar1);
            syncRGBToHex(navbar2R, navbar2G, navbar2B, colorPickerNavbar2);
            syncRGBToHex(navbar3R, navbar3G, navbar3B, colorPickerNavbar3);
            syncRGBToHex(sidebar1R, sidebar1G, sidebar1B, colorPickerSidebar1);
            syncRGBToHex(sidebar2R, sidebar2G, sidebar2B, colorPickerSidebar2);
            syncRGBToHex(sidebarHoverR, sidebarHoverG, sidebarHoverB, colorPickerSidebarHover);
            syncRGBToHex(sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB, colorPickerSidebarHoverBorder);
            syncRGBToHex(module1R, module1G, module1B, colorPickerModule1);
            syncRGBToHex(module2R, module2G, module2B, colorPickerModule2);
            syncRGBToHex(module3R, module3G, module3B, colorPickerModule3);
            updatePreview();

            // Navbar color changes
            [navbar1R, navbar1G, navbar1B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(navbar1R, navbar1G, navbar1B, colorPickerNavbar1);
                    updatePreview();
                });
            });

            [navbar2R, navbar2G, navbar2B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(navbar2R, navbar2G, navbar2B, colorPickerNavbar2);
                    updatePreview();
                });
            });

            [navbar3R, navbar3G, navbar3B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(navbar3R, navbar3G, navbar3B, colorPickerNavbar3);
                    updatePreview();
                });
            });

            // Sidebar color changes
            [sidebar1R, sidebar1G, sidebar1B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(sidebar1R, sidebar1G, sidebar1B, colorPickerSidebar1);
                    updatePreview();
                });
            });

            [sidebar2R, sidebar2G, sidebar2B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(sidebar2R, sidebar2G, sidebar2B, colorPickerSidebar2);
                    updatePreview();
                });
            });

            // Sidebar Hover color changes
            [sidebarHoverR, sidebarHoverG, sidebarHoverB].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(sidebarHoverR, sidebarHoverG, sidebarHoverB, colorPickerSidebarHover);
                    updatePreview();
                });
            });

            // Sidebar Hover Border color changes (NEW)
            [sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB, colorPickerSidebarHoverBorder);
                    updatePreview();
                });
            });

            // Module color changes
            [module1R, module1G, module1B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(module1R, module1G, module1B, colorPickerModule1);
                    updatePreview();
                });
            });

            [module2R, module2G, module2B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(module2R, module2G, module2B, colorPickerModule2);
                    updatePreview();
                });
            });

            [module3R, module3G, module3B].forEach(input => {
                input.addEventListener('input', function () {
                    syncRGBToHex(module3R, module3G, module3B, colorPickerModule3);
                    updatePreview();
                });
            });

            // Color picker changes - Navbar
            colorPickerNavbar1.addEventListener('input', function () {
                syncHexToRGB(colorPickerNavbar1.value, navbar1R, navbar1G, navbar1B, true);
                updatePreview();
            });

            colorPickerNavbar2.addEventListener('input', function () {
                syncHexToRGB(colorPickerNavbar2.value, navbar2R, navbar2G, navbar2B, true);
                updatePreview();
            });

            colorPickerNavbar3.addEventListener('input', function () {
                syncHexToRGB(colorPickerNavbar3.value, navbar3R, navbar3G, navbar3B, true);
                updatePreview();
            });

            // Color picker changes - Sidebar
            colorPickerSidebar1.addEventListener('input', function () {
                syncHexToRGB(colorPickerSidebar1.value, sidebar1R, sidebar1G, sidebar1B, true);
                updatePreview();
            });

            colorPickerSidebar2.addEventListener('input', function () {
                syncHexToRGB(colorPickerSidebar2.value, sidebar2R, sidebar2G, sidebar2B, true);
                updatePreview();
            });

            // Color picker changes - Sidebar Hover
            colorPickerSidebarHover.addEventListener('input', function () {
                syncHexToRGB(colorPickerSidebarHover.value, sidebarHoverR, sidebarHoverG, sidebarHoverB, true);
                updatePreview();
            });

            // Color picker changes - Sidebar Hover Border (NEW)
            colorPickerSidebarHoverBorder.addEventListener('input', function () {
                syncHexToRGB(colorPickerSidebarHoverBorder.value, sidebarHoverBorderR, sidebarHoverBorderG, sidebarHoverBorderB, true);
                updatePreview();
            });

            // Color picker changes - Modules
            colorPickerModule1.addEventListener('input', function () {
                syncHexToRGB(colorPickerModule1.value, module1R, module1G, module1B, true);
                updatePreview();
            });

            colorPickerModule2.addEventListener('input', function () {
                syncHexToRGB(colorPickerModule2.value, module2R, module2G, module2B, true);
                updatePreview();
            });

            colorPickerModule3.addEventListener('input', function () {
                syncHexToRGB(colorPickerModule3.value, module3R, module3G, module3B, true);
                updatePreview();
            });

            updatePreview();
        });
    </script>
</asp:Content>
