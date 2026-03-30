<%@ Page Title="Compras Internacionales" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComprasInternacionales.aspx.cs" Inherits="Maqueta.ComprasInternacionales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>

        /* ===== Layout y helpers locales ===== */
        .field-group {
            margin-bottom: 12px;
        }

        .section-title {
            margin-top: 8px;
            margin-bottom: 16px;
        }

        .input-sufijo {
            text-align: right;
        }

        /* ===== Sticky header (solo para este módulo) ===== */
        .sticky-filter {
            position: sticky;
            top: 0;
            z-index: 1020;
        }

        .sticky-filter .card {
            box-shadow: 0 4px 8px rgba(0,0,0,.06);
        }

        .sticky-spacer {
            height: 8px;
        }

        /* ===== Marco azul del Grid (local) ===== */
        .grid-wrapper {
            border: 3px solid #221D8A;
            border-radius: 6px;
            padding: 8px;
        }

        .grid-wrapper .table {
            margin-bottom: 0;
        }

    </style>

    <div class="container">
        <div class="row">
            <div class="col-lg-10 col-12 mx-auto">

                <!-- ===== Sticky: Título a la izquierda + Filtro a la derecha ===== -->
                <div class="sticky-filter">
                    <div class="card">
                        <div class="card-body">

                            <asp:HiddenField ID="hidId" runat="server" />

                            <div class="row align-items-center">

                                <!-- Columna izquierda: TÍTULO + ICONO -->
                                <div class="col-md-5 d-flex align-items-center gap-2"> 
                                     <img src="../imagenes/CapturadeCargas.png" height="55" alt="Cargas" />
                                    <h3 class="mb-0" style="color: #2B399B; font-weight: 700;">Compras Internacionales 
          </h3>
                                </div>

                                <!-- Columna derecha: FILTRO + BOTONES -->
                                <div class="col-md-7">
                                    <label class="text-muted mb-1" style="font-size: 14px;">
                                        Filtro rápido (Contrato / Contenedor / Proveedor)
         
                                    </label>

                                    <div class="input-group">
                                        <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" />
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar"
                                            CssClass="btn btn-outline-secondary"
                                            OnClick="btnBuscar_Click" />
                                        <asp:Button ID="btnLimpiarFiltro" runat="server" Text="Limpiar"
                                            CssClass="btn btn-outline-secondary"
                                            OnClick="btnLimpiarFiltro_Click" />
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>
                </div>

                <div class="sticky-spacer"></div>

                <!-- ===== Listado / selector con marco azul ===== -->
                <div class="card mb-3">
                    <div class="card-body"> 
                            <asp:GridView ID="gvCargas" runat="server"
                                CssClass="table table-sm table-hover"
                                AutoGenerateColumns="False" DataKeyNames="Id"
                                AllowPaging="True" PageSize="3"
                                OnPageIndexChanging="gvCargas_PageIndexChanging"
                                OnSelectedIndexChanged="gvCargas_SelectedIndexChanged">

                                <Columns>
                                    <asp:ButtonField Text="Seleccionar" CommandName="Select" HeaderText="Click/Selecciona" >
                                    <ItemStyle HorizontalAlign="Center" />
                                    </asp:ButtonField>
                                    <asp:BoundField DataField="FechaCompra" HeaderText="Compra" DataFormatString="{0:yyyy-MM-dd}" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Contract" HeaderText="Contract" />
                                    <asp:BoundField DataField="Container" HeaderText="Container" />
                                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                                    <asp:BoundField DataField="ETA" HeaderText="ETA" DataFormatString="{0:yyyy-MM-dd}" />
                                </Columns>
                            </asp:GridView>
                        </div> 
                </div>

                <!-- ===== Formulario de captura / edición ===== -->
                <div class="card">
                    <div class="card-body">

                        <!-- ==================== SECCIÓN: DATOS DE COMPRA ==================== -->
                        <div class="row">
                            <div class="col">
                                <h5 class="section-title">Datos de compra</h5>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3 field-group">
                                <label>Fecha de compra *</label>
                                <asp:TextBox ID="txtFechaCompra" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFechaCompra" runat="server"
                                    ControlToValidate="txtFechaCompra" Display="Dynamic" CssClass="text-danger"
                                    ErrorMessage="Requerido" />
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Cruce</label>
                                <asp:DropDownList ID="ddlCruce" runat="server" CssClass="form-control" ToolTip="Punto de cruce/frontera donde se realizará el despacho (A1, A4).">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="A1 - Cruce principal" Value="A1"></asp:ListItem>
                                    <asp:ListItem Text="A4 - Alterno/segundo cruce" Value="A4"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Incoterm</label>
                                <asp:DropDownList ID="ddlIncoterm" runat="server" CssClass="form-control" ToolTip="Término de compraventa internacional (Incoterms 2020).">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>

                                    <asp:ListItem Text="EXW – Ex Works (En fábrica)" Value="EXW"></asp:ListItem>
                                    <asp:ListItem Text="FCA – Free Carrier (Franco transportista)" Value="FCA"></asp:ListItem>
                                    <asp:ListItem Text="CPT – Carriage Paid To (Transporte pagado hasta)" Value="CPT"></asp:ListItem>
                                    <asp:ListItem Text="CIP – Carriage and Insurance Paid To (Transporte y seguro pagados hasta)" Value="CIP"></asp:ListItem>
                                    <asp:ListItem Text="DAP – Delivered At Place (Entregado en lugar)" Value="DAP"></asp:ListItem>
                                    <asp:ListItem Text="DPU – Delivered at Place Unloaded (Entregado en lugar descargado)" Value="DPU"></asp:ListItem>
                                    <asp:ListItem Text="DDP – Delivered Duty Paid (Entregado con derechos pagados)" Value="DDP"></asp:ListItem>

                                    <asp:ListItem Text="FAS – Free Alongside Ship (Franco al costado del buque)" Value="FAS"></asp:ListItem>
                                    <asp:ListItem Text="FOB – Free On Board (Libre a bordo)" Value="FOB"></asp:ListItem>
                                    <asp:ListItem Text="CFR – Cost and Freight (Costo y flete)" Value="CFR"></asp:ListItem>
                                    <asp:ListItem Text="CIF – Cost, Insurance & Freight (Costo, seguro y flete)" Value="CIF"></asp:ListItem>

                                </asp:DropDownList>
                            </div>



                            <div class="col-md-3 field-group">
                                <label>Contract *</label>
                                <asp:TextBox ID="txtContract" runat="server" CssClass="form-control" MaxLength="40" placeholder="Contrato / Ref. interna" autocomplete="off"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContract" runat="server"
                                    ControlToValidate="txtContract" Display="Dynamic" CssClass="text-danger"
                                    ErrorMessage="Requerido" />
                            </div>
                        </div>

                        <div class="row">



                            <div class="col-md-3 field-group">
                                <label>Container *</label>
                                <asp:TextBox ID="txtContainer" runat="server" CssClass="form-control" MaxLength="40" placeholder="Contenedor"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContainer" runat="server"
                                    ControlToValidate="txtContainer"
                                    Display="Dynamic"
                                    CssClass="text-danger"
                                    ErrorMessage="Requerido" />
                            </div>



                            <div class="col-md-3 field-group">
                                <label>Sello</label>
                                <asp:TextBox ID="txtSello" runat="server" CssClass="form-control" MaxLength="40" placeholder="Sello"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Invoice</label>
                                <asp:TextBox ID="txtInvoice" runat="server" CssClass="form-control" MaxLength="40" placeholder="Invoice"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Pedimento</label>
                                <asp:TextBox ID="txtPedimento" runat="server" CssClass="form-control" MaxLength="30" placeholder="Pedimento"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 field-group">
                                <label>Proveedor *</label>
                                <asp:TextBox ID="txtProveedor" runat="server" CssClass="form-control" MaxLength="200"
                                    placeholder="Proveedor"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvProveedor" runat="server"
                                    ControlToValidate="txtProveedor" Display="Dynamic" CssClass="text-danger"
                                    ErrorMessage="Requerido" />
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Origen</label>
                                <asp:TextBox ID="txtOrigen" runat="server" CssClass="form-control" MaxLength="60" placeholder="Origen"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Puerto</label>
                                <asp:TextBox ID="txtPuerto" runat="server" CssClass="form-control" MaxLength="60" placeholder="Puerto"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3 field-group">
                                <label>Naviera</label>
                                <asp:TextBox ID="txtNaviera" runat="server" CssClass="form-control" MaxLength="60" placeholder="Naviera"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Conexiones / Demoras</label>
                                <asp:TextBox ID="txtConexionesDemoras" runat="server" CssClass="form-control" MaxLength="40" placeholder="Conexiones"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>ETD</label>
                                <asp:TextBox ID="txtETD" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>ETA</label>
                                <asp:TextBox ID="txtETA" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-3 field-group">
                                <label>Fecha estimada destino</label>
                                <asp:TextBox ID="txtFechaEstimDestino" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Último día libre de demoras</label>
                                <asp:TextBox ID="txtUltimoDiaLibre" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Entrega de vacío</label>
                                <asp:TextBox ID="txtEntregaVacio" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Días generados de demoras</label>
                                <asp:TextBox ID="txtDiasDemoras" runat="server" CssClass="form-control input-sufijo" placeholder="0"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revDiasDemoras" runat="server"
                                    ControlToValidate="txtDiasDemoras" CssClass="text-danger" Display="Dynamic"
                                    ValidationExpression="^\d{1,4}$"
                                    ErrorMessage="Ingrese un entero válido (máx. 4 dígitos)" />
                            </div>
                        </div>

                        <!-- Demoras -->
                        <div class="row">
                            <div class="col">
                                <h5 class="section-title">Demoras</h5>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3 field-group">
                                <label>Factura de demoras</label>
                                <asp:TextBox ID="txtFacturaDemoras" runat="server" CssClass="form-control" MaxLength="40"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Monto USD de demoras</label>
                                <asp:TextBox ID="txtMontoUSDDemoras" runat="server" CssClass="form-control input-sufijo" placeholder="0.00"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revMontoUSDDemoras" runat="server"
                                    ControlToValidate="txtMontoUSDDemoras" CssClass="text-danger" Display="Dynamic"
                                    ValidationExpression="^\d{1,12}(\.\d{1,2})?$"
                                    ErrorMessage="Formato inválido (ej. 12345.67)" />
                            </div>
                            <div class="col-md-3 field-group">
                                <label>TC demoras</label>
                                <asp:TextBox ID="txtTCDemoras" runat="server" CssClass="form-control input-sufijo" placeholder="0.00"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revTCDemoras" runat="server"
                                    ControlToValidate="txtTCDemoras" CssClass="text-danger" Display="Dynamic"
                                    ValidationExpression="^\d{1,12}(\.\d{1,4})?$"
                                    ErrorMessage="Formato inválido (ej. 17.2500)" />
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Monto demoras en MXN</label>
                                <asp:TextBox ID="txtMontoDemorasMXN" runat="server" CssClass="form-control input-sufijo" placeholder="0.00"
                                    ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Documentos y fechas -->
                        <div class="row">
                            <div class="col">
                                <h5 class="section-title">Documentos y fechas</h5>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3 field-group">
                                <label>Factura comercializadora</label>
                                <asp:TextBox ID="txtFacturaComercializadora" runat="server" CssClass="form-control" MaxLength="40"
                                    placeholder="ANEXO / Folio"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Fecha de despacho</label>
                                <asp:TextBox ID="txtFechaDespacho" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Fecha pago pedimento</label>
                                <asp:TextBox ID="txtFechaPagoPedimento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label># Planta</label>
                                <asp:TextBox ID="txtPlanta" runat="server" CssClass="form-control input-sufijo" placeholder="0"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revPlanta" runat="server"
                                    ControlToValidate="txtPlanta" CssClass="text-danger" Display="Dynamic"
                                    ValidationExpression="^\d{1,6}$"
                                    ErrorMessage="Ingrese un entero válido (máx. 6 dígitos)" />
                            </div>


                        </div>

                        <!-- ==================== SECCIÓN: PRODUCCIÓN Y LOTE ==================== -->
                        <div class="row">
                            <div class="col">
                                <h5 class="section-title">Producción y lote</h5>
                            </div>
                        </div>

                        <!-- Fila 1: Lote / Fechas / Empresa (3|3|3|3) -->
                        <div class="row">
                            <div class="col-md-3 field-group">
                                <label>Lote</label>
                                <asp:TextBox ID="txtLote" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Fecha de producción</label>
                                <asp:TextBox ID="txtFechaProduccion" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Fecha de caducidad</label>
                                <asp:TextBox ID="txtFechaCaducidad" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-3 field-group">
                                <label>Empresa</label>
                                <asp:DropDownList ID="ddlEmpresa" runat="server" CssClass="form-control"
                                    ToolTip="Empresa de CEPESMAR responsable de la operación.">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="J&amp;J Seafood" Value="J&amp;J Seafood"></asp:ListItem>
                                    <asp:ListItem Text="Central" Value="Central"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <!-- Fila 2: Código / Descripción con UpdatePanel (3|6|3) -->
                        <asp:UpdatePanel ID="upCodigoDesc" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <!-- CÓDIGO -->
                                    <div class="col-md-2 field-group">
                                        <label>Código</label>
                                        <asp:TextBox ID="txtCodigo" runat="server"
                                            CssClass="form-control"
                                            AutoPostBack="true"
                                            OnTextChanged="txtCodigo_TextChanged"
                                            placeholder="Código" />
                                    </div>

                                    <!-- DESCRIPCIÓN (Producto + Talla) -->
                                    <div class="col-md-5 field-group">
                                        <label>Descripción</label>



                                        <asp:TextBox ID="txtDescripcionProducto" runat="server"
                                            CssClass="form-control"
                                            ReadOnly="true"
                                            placeholder="Descripción del producto (Producto + Talla)" />
                                    </div>

                                    
                            <div class="col-md-5 field-group">
                                <label>Producto</label>
                                <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-control"
                                    ToolTip="Seleccione el producto exacto.">
                                </asp:DropDownList>
                            </div>


                                    <!-- ESPACIADOR para cerrar 12 columnas -->
                                    <div class="col-md-3 field-group"></div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="txtCodigo" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                        <!-- Fila 3: Producto / Marca / Talla (5|4|3) - tu configuración actual -->
                        <div class="row">



                            <div class="col-md-3 field-group">
                                <label>Marca</label>
                                <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-control"
                                    ToolTip="Marca comercial impresa en la caja/bolsa.">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="AQUA CHILE" Value="AQUA CHILE"></asp:ListItem>
                                    <asp:ListItem Text="CEPESMAR" Value="CEPESMAR"></asp:ListItem>
                                    <asp:ListItem Text="REY DE MAR" Value="REY DE MAR"></asp:ListItem>
                                    <asp:ListItem Text="CONARTA" Value="CONARTA"></asp:ListItem>
                                    <asp:ListItem Text="SIN MARCA" Value="SIN MARCA"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-3 field-group">
                                <label>Talla</label>
                                <asp:DropDownList ID="ddlTalla" runat="server" CssClass="form-control"
                                    ToolTip="Rango de talla/gramaje.">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="2/3" Value="2/3"></asp:ListItem>
                                    <asp:ListItem Text="3/4" Value="3/4"></asp:ListItem>
                                    <asp:ListItem Text="3/5" Value="3/5"></asp:ListItem>
                                    <asp:ListItem Text="4-23 KG" Value="4-23 KG"></asp:ListItem>
                                    <asp:ListItem Text="4/5" Value="4/5"></asp:ListItem>
                                    <asp:ListItem Text="5/7" Value="5/7"></asp:ListItem>
                                    <asp:ListItem Text="7/9" Value="7/9"></asp:ListItem>
                                    <asp:ListItem Text="8/10" Value="8/10"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            
                             <!-- Cantidades -->

                         
                            <div class="col-md-3 field-group">
                                <label>Kgs</label>
                                <asp:TextBox ID="txtKgs" runat="server" CssClass="form-control input-sufijo" placeholder="0.00"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revKgs" runat="server" ControlToValidate="txtKgs"
                                    CssClass="text-danger" Display="Dynamic"
                                    ValidationExpression="^\d{1,12}(\.\d{1,2})?$"
                                    ErrorMessage="Formato inválido (ej. 12345.67)" />
                            </div>
                            <div class="col-md-3 field-group">
                                <label># Cajas</label>
                                <asp:TextBox ID="txtCajas" runat="server" CssClass="form-control input-sufijo" placeholder="0"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revCajas" runat="server" ControlToValidate="txtCajas"
                                    CssClass="text-danger" Display="Dynamic"
                                    ValidationExpression="^\d{1,9}$"
                                    ErrorMessage="Ingrese un entero válido" />
                            </div>
                        </div> 
                        <hr /> 
                        <!-- ESPACIADOR para cerrar 12 columnas y alinear siguiente fila -->
                                           
                        <!-- ====== SECCIÓN: ARCHIVOS / DOCUMENTOS ====== -->
<div class="row">
    <div class="col">
        <h5 class="section-title">Archivos</h5>
    </div>
</div>

<!-- FIX puntual: forzar el ancho completo del FileUpload -->
<style>
    /* Por si algún CSS global limita el input file */
    input[type="file"].form-control {
        width: 100% !important;
        max-width: none !important;
        display: block !important;
    }
</style>

<asp:UpdatePanel ID="upArchivos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <!-- Subida -->
        <div class="row align-items-end g-3">

            <!-- Seleccionar archivo  -->
            <div class="col-12 col-lg-6 field-group">
                <label class="form-label">Seleccionar  archivo</label>
                <asp:FileUpload ID="fuArchivo" runat="server" CssClass="form-control" />
            </div>

            <!-- Botón subir (solo el espacio necesario) -->
            <div class="col-auto field-group">
                <label class="form-label d-none d-lg-block">&nbsp;</label>
                <asp:Button ID="btnSubirArchivo" runat="server" Text="Subir Archivo"
                    CssClass="btn btn-blue" OnClick="btnSubirArchivo_Click" />
                <asp:Label ID="lblUploadMsg" runat="server"
                    CssClass="text-muted d-block mt-1"></asp:Label>
            </div>

            <!-- ID actual (ajustable) -->
            <div class="col-12 col-lg-2 field-group">
                <label class="form-label d-none d-lg-block">&nbsp;</label>
                <asp:Label ID="lblIdActual" runat="server" CssClass="text-muted"></asp:Label>
            </div>

        </div>

        <!-- Grid de archivos -->
        <div class="row mt-3">
            <div class="col">
                <asp:GridView ID="gvArchivos" runat="server"
                    CssClass="table table-sm table-hover"
                    AutoGenerateColumns="False"
                    DataKeyNames="Name"
                    OnRowDataBound="gvArchivos_RowDataBound">

                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Archivo" />
                        <asp:BoundField DataField="SizeKB" HeaderText="Tamaño (KB)" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        <asp:BoundField DataField="Url" HeaderText="Url" Visible="false" />
                    </Columns>

                </asp:GridView>

                <small class="text-muted">
                    Doble‑clic sobre un renglón para visualizar el archivo.
                </small>
            </div>
        </div>

        <hr />

    </ContentTemplate>

    <Triggers>
        <asp:PostBackTrigger ControlID="btnSubirArchivo" />
    </Triggers>
</asp:UpdatePanel>

<!-- JS: doble clic abre archivo -->
<script>
    function openFile(url) {
        if (!url) return;
        window.open(url, '_blank');
    }
</script>
 

                        <!-- Acciones -->
                        <div class="row">
                            <div class="col">
                                <asp:ValidationSummary ID="vs" runat="server" CssClass="text-danger" />
                                <div class="form-group" style="margin-top: 14px;">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar"
                                        CssClass="btn btn-success btn-lg"
                                        OnClick="btnGuardar_Click" />
                                    &nbsp;
               
                                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar"
                                        CssClass="btn btn-warning btn-lg" OnClick="btnActualizar_Click" />
                                    &nbsp;
               
                                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar"
                                        CssClass="btn btn-danger btn-lg" OnClientClick="return confirm('¿Eliminar este registro?');"
                                        OnClick="btnEliminar_Click" />
                                    &nbsp;
               
                                    <a class="btn btn-link" href="../Inicio.aspx">&laquo; Regresar</a>
                                </div>
                            </div>
                        </div>

                    </div>


                </div>
                <!-- card-body -->
            </div>
            <!-- card -->

        </div>
    </div>

    <!-- Cálculo automático de Monto Demoras en MXN (cliente) -->
    <script>
        (function () {
            function toNum(v) { return parseFloat((v || '').replace(/,/g, '.')) || 0; }
            function recalc() {
                var usd = toNum(document.getElementById('<%= txtMontoUSDDemoras.ClientID %>').value);
                var tc = toNum(document.getElementById('<%= txtTCDemoras.ClientID %>').value);
                var mxn = (usd * tc) || 0;
                document.getElementById('<%= txtMontoDemorasMXN.ClientID %>').value = mxn.toFixed(2);
            }
            document.addEventListener('DOMContentLoaded', function () {
                var usd = document.getElementById('<%= txtMontoUSDDemoras.ClientID %>');
            var tc = document.getElementById('<%= txtTCDemoras.ClientID %>');
            if (usd) usd.addEventListener('input', recalc);
            if (tc) tc.addEventListener('input', recalc);
        });
        })();
</script>
</asp:Content>
