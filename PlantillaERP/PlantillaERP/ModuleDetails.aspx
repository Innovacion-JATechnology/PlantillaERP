<%@ Page Title="Detalle de Módulo" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ModuleDetails.aspx.cs" Inherits="PlantillaERP.ModuleDetails" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="~/Content/modulos.css" rel="stylesheet" type="text/css" />
    
    <div class="page-header mb-4">
        <div style="display: flex; align-items: center; gap: 15px;">
            <a href="/" style="color: rgb(87, 179, 252); font-size: 1.5rem; text-decoration: none;">
                <i class="fas fa-arrow-left"></i>
            </a>
            <h1 class="page-title" style="margin: 0;">
                <asp:Literal ID="litModuleTitle" runat="server"></asp:Literal>
            </h1>
        </div>
    </div>

    <div class="modules-grid" id="submodulesGrid" runat="server">
        <!-- Los submódulos se generarán dinámicamente aquí -->
    </div>

</asp:Content>
