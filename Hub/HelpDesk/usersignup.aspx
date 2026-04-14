<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="usersignup.aspx.cs" Inherits="HelpDesk.usersignup" %>

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
                                    <img width="150px" src="imgs/usuario.png" />
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>
                                    <h3>Registro de Usuarios</h3>
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
                            

                            <div class="col-md-6">
                                <label>Apellido Paterno</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="paterno"
                                        runat="server" placeholder="Apellido Paterno" ></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label>Apellido Materno</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="materno"
                                        runat="server" placeholder="Apellido Materno"></asp:TextBox>
                                </div>
                            </div>
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
                                <label>Empresa</label>
                                <div class="form-group">
                                    <asp:DropDownList class="form-control" ID="listaempresa" runat="server" >
                                        <asp:ListItem Text="Cepesmar" Value="Cepesmar" />                                    
                                    </asp:DropDownList>
                                </div>
                            </div>



                            <div class="col-md-6">
                                <label>SLA</label>
                                <div class="form-group">
                                    <asp:DropDownList class="form-control" ID="listaSla" runat="server">

                                        <asp:ListItem Text="1" Value="1" />
                                        <asp:ListItem Text="2" Value="2" />
                                        <asp:ListItem Text="3" Value="3" />
                                        <asp:ListItem Text="4" Value="4" />
                                        <asp:ListItem Text="5" Value="5" />
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Puesto</label>
                                <div class="form-group">
                                    <asp:DropDownList class="form-control" ID="listaPuesto" runat="server">
                                        <asp:ListItem Text="Cepesmar" Value="Cepesmar" />
                                    </asp:DropDownList>
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
