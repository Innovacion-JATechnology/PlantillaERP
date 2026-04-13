<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="homepage.aspx.cs" Inherits="HelpDesk.homepage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <img src="imgs/barra.png" class="img-fluid w-100 d-block" alt="barra">
    </section>
    <section>
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <center>
                        <br>
                            <h2>Sistema de Soporte 1.0
                            </h2>
                        </br>

                    </center>
                </div>
            </div>

            <div class="row">


                <div class="col-md-6">
                    <a href="userlogin.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="200px" src="imgs/usuario.png" />
                            <h4>Ingresar como usuario</h4>
                    </a>
                </div>


                <div class="col-md-6">
                    <a href="adminlogin.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="200px" src="imgs/admin.png" />
                            <h4>Ingresar como Administrador</h4>
                        </center>
                    </a>
                </div>



            </div>

        </div>
    </section>
</asp:Content>
