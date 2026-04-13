<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="userlogin.aspx.cs" Inherits="HelpDesk.userlogin" %>

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
                                    <h3>Ingreso de Usuarios</h3>
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
                            <div class="col">
                                <label>Usuario</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="usuario"
                                        runat="server" placeholder="Usuario"></asp:TextBox>
                                </div>

                            

                                <label>Contraseña</label>
<div class="form-group position-relative">
    <asp:TextBox CssClass="form-control pr-5" ID="contrasena"
                 runat="server" placeholder="Contraseña" TextMode="Password" />
    <!-- Toggle button -->
    <button type="button"
            id="togglePwd"
            class="btn btn-sm btn-light border position-absolute"
            style="right:8px; top:50%; transform:translateY(-50%);"
            aria-label="Mostrar u ocultar contraseña"
            aria-pressed="false">
        <span id="toggleIcon">👁️</span>
    </button>
</div>


                                <div class="form-group">
                                    <asp:Button ID="continuarIngreso"
                                        runat="server"
                                        Text="Continuar"
                                        CssClass="btn btn-primary btn-block btn-lg btn-2b399b" OnClick="Continuar_LogIn_Click" />
                                </div>
                            </div>
                        </div>


                    </div>
                </div>

                <a href="homepage.aspx"><< Regresar al Inicio</a><br>
                <br />
            </div>
        </div>
    </div>


    <script>
    (function () {
        var pwd = document.getElementById('<%= contrasena.ClientID %>');
        var btn = document.getElementById('togglePwd');
        var icon = document.getElementById('toggleIcon');

        if (!pwd || !btn) return;

        btn.addEventListener('click', function () {
            var showing = pwd.getAttribute('type') === 'text';
            pwd.setAttribute('type', showing ? 'password' : 'text');
            // Update icon/text & accessibility state
            icon.textContent = showing ? '👁️' : '🙈';
            btn.setAttribute('aria-pressed', (!showing).toString());
        });
    })();
    </script>
</asp:Content>
