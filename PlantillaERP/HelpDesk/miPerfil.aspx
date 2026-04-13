<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="miPerfil.aspx.cs" Inherits="HelpDesk.miPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .readonly-input {
            background-color: #f8f9fa;
            color: #6c757d;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-8 mx-auto">

                <div class="card">
                    <div class="card-body">

                        <!-- Validation Summary -->
                        <asp:ValidationSummary ID="ValidationSummary1"
                            runat="server"
                            HeaderText="Por favor corrige lo siguiente:"
                            CssClass="alert alert-danger"
                            DisplayMode="BulletList"
                            ValidationGroup="Perfil" />

                        <div class="row">
                            <div class="col text-center">
                                <img width="150px" src="imgs/usuario.png" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col text-center">
                                <h3>Perfil</h3>
                                <span>Estatus</span>
                                <asp:Label CssClass="badge badge-pill badge-success"
                                    ID="Label1" runat="server" Text="Activo"></asp:Label>
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col-md-8">
                                <label>Nombre(s)</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="TextBox3"
                                        runat="server" placeholder="Nombre(s)" ReadOnly="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                        ControlToValidate="TextBox3"
                                        CssClass="text-danger"
                                        ErrorMessage="El nombre es obligatorio."
                                        ValidationGroup="Perfil" />
                                </div>
                            </div>

                            <div class="col-md-4">
                                <label>ID</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control readonly-input" ID="TextBox1"
                                        runat="server" placeholder="ID" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-md-6">
                                <label>Correo-e</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="TextBox5"
                                        runat="server" placeholder="Correo-e" TextMode="Email" ReadOnly="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                        ControlToValidate="TextBox5"
                                        CssClass="text-danger"
                                        ErrorMessage="El correo es obligatorio."
                                        ValidationGroup="Perfil" />
                                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                        ControlToValidate="TextBox5"
                                        CssClass="text-danger"
                                        ErrorMessage="Correo no válido."
                                        ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                                        ValidationGroup="Perfil" />
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label>Número de Contacto</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="TextBox6"
                                        runat="server" placeholder="Número de Contacto" TextMode="Phone" ReadOnly="true"></asp:TextBox>

                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <label>Empresa</label>


                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="TextBox4"
                                        runat="server" placeholder="Empresa" ReadOnly="true"></asp:TextBox>
                                </div>


                            </div>

                            <div class="col-md-6">
                                <label>Puesto</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="TextBox8"
                                        runat="server" placeholder="Puesto" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Cambiar Contraseña -->
                        <div class="row">
                            <div class="col text-center">
                                <span class="badge badge-pill badge-primary align-middle">Cambiar Contraseña</span>
                                <br />
                                <br />
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-md-6">
                                <label>Nueva Contraseña</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="TextBox2"
                                        runat="server" placeholder="Contraseña" TextMode="Password"></asp:TextBox>
                                    <!-- Optional until user decides to change; enforce policy if filled -->
                                    <asp:RegularExpressionValidator ID="revPwdPolicy" runat="server"
                                        ControlToValidate="TextBox2"
                                        CssClass="text-danger"
                                        ErrorMessage="La contraseña debe tener 8+ caracteres, una mayúscula, una minúscula, un número y un carácter especial."
                                        ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$"
                                        ValidationGroup="Perfil" />
                                </div>
                            </div>

                            <div class="col-md-6">
                                <label>Repita Contraseña</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="TextBox7"
                                        runat="server" placeholder="Contraseña" TextMode="Password"></asp:TextBox>
                                    <asp:CompareValidator ID="cvPwdMatch" runat="server"
                                        ControlToValidate="TextBox7"
                                        ControlToCompare="TextBox2"
                                        CssClass="text-danger"
                                        ErrorMessage="Las contraseñas no coinciden."
                                        ValidationGroup="Perfil" />
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-8 mx-auto">
                                <div class="form-group text-center">
                                    <asp:Button
                                        CssClass="btn btn-primary btn-lg btn-2b399b"
                                        ID="Button1" runat="server" Text="Actualizar"
                                        OnClick="Button1_Click"
                                        CausesValidation="true"
                                        ValidationGroup="Perfil" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <a href="InicioUsuario.aspx">&laquo; Regresar al Inicio</a><br />
                <br />
            </div>
        </div>
    </div>

</asp:Content>
