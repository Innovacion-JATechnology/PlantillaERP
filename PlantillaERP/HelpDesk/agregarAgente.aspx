<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="agregarAgente.aspx.cs" Inherits="HelpDesk.agregarAgente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-md-6 mx-auto">

                <div class="card">
                    <div class="card-body">

                        <div class="row">
                            <div class="col">
                                <center>
                                    <img src="imgs/agente.png" width="150px" />
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>
                                    <h3>Agregar Agente</h3>
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>
                                    <hr>
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <label>Nombre(s)</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="nombre"
                                        runat="server" placeholder="Nombre(s)"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                        </div>
                        <div class="row">

                            <div class="col-md-6">
                                <label>Correo-e</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="correo"
                                        runat="server" placeholder="Correo-e" TextMode="Email"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label>Teléfono de Contacto</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="contacto"
                                        runat="server" placeholder="Teléfono de Contacto" TextMode="Phone"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">


                            <div class="col-md-6">
                                <label>Nivel de Soporte</label>
                                <div class="form-group">
                                    <asp:DropDownList class="form-control" ID="listaSla" runat="server">

                                        <asp:ListItem Text="1" Value="1" />
                                        <asp:ListItem Text="2" Value="2" />
                                        <asp:ListItem Text="3" Value="3" />
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label>Habilidades</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="hbls"
                                        runat="server" placeholder="Habilidades"></asp:TextBox>
                                </div>
                            </div>
                        </div>



                        <div class="row">

                            <div class="col-md-12">
                                <label>Contraseña</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="contrasena"
                                        runat="server" placeholder="Contraseña" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-check">
                                    <asp:CheckBox ID="chkAdministrador" runat="server" CssClass="form-check-input" />
                                    <label class="form-check-label" for="<%= chkAdministrador.ClientID %>">
                                        ¿Es Administrador?
                                    </label>
                                    <small class="form-text text-muted d-block mt-1">
                                        Los administradores pueden gestionar agentes y acceder al panel administrativo.
                                    </small>
                                </div>
                            </div>
                        </div>

                        <div class="row mt-3">
                            <div class="col">
                                <div class="form-group">
                                    <asp:Button ID="continuar"
                                        runat="server"
                                        Text="Guardar"
                                        CssClass="btn btn-primary btn-block btn-lg btn-2b399b" OnClick="Button1_Click" />
                                </div>
                            </div>

                        </div>
                    </div>

                    <a href="InicioAgente.aspx"><< Regresar al Inicio</a><br>
                    <br />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
