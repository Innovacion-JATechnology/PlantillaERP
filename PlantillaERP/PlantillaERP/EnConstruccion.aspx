<%@ Page Title="En Construcción" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EnConstruccion.aspx.cs" Inherits="PlantillaERP.EnConstruccion" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .construccion-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            min-height: 60vh;
            text-align: center;
            padding: 40px 20px;
        }

        .construccion-icon {
            font-size: 5rem;
            color: rgb(87, 179, 252);
            margin-bottom: 30px;
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0%, 100% {
                transform: scale(1);
            }
            50% {
                transform: scale(1.05);
            }
        }

        .construccion-title {
            font-size: 2.5rem;
            font-weight: 700;
            color: #1a1a2e;
            margin-bottom: 15px;
        }

        .construccion-subtitle {
            font-size: 1.2rem;
            color: #7c8aa5;
            margin-bottom: 30px;
            max-width: 500px;
        }

        .construccion-description {
            font-size: 1rem;
            color: #999;
            margin-bottom: 40px;
            line-height: 1.6;
        }

        .btn-volver {
            background: linear-gradient(135deg, rgb(87, 179, 252), rgb(165, 95, 253), rgb(16, 55, 161));
            color: white;
            padding: 12px 40px;
            border: none;
            border-radius: 25px;
            font-size: 1rem;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.35s ease;
            text-decoration: none;
            display: inline-block;
        }

        .btn-volver:hover {
            transform: translateY(-3px);
            box-shadow: 0 10px 30px rgba(87, 179, 252, 0.3);
            color: white;
            text-decoration: none;
        }
    </style>

    <div class="construccion-container">
        <div class="construccion-icon">
            <i class="fas fa-tools"></i>
        </div>
        <h1 class="construccion-title">En Construcción</h1>
        <p class="construccion-subtitle">Esta funcionalidad está siendo desarrollada</p>
        <p class="construccion-description">
            Estamos trabajando en esta sección para ofrecerte la mejor experiencia.
            <br />Por favor, intenta nuevamente en la próxima actualización.
        </p>
        <a href="/" class="btn-volver">Volver al Inicio</a>
    </div>

</asp:Content>
