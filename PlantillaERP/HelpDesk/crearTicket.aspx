<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="crearTicket.aspx.cs" Inherits="HelpDesk.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imgview').attr('src', e.target.result);
            };
            reader.readAsDataURL(input.files[0]);
        }
    }
</script>
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
                                    <h3>Solicitud de Servicio</h3>
                                </center>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <center>
                                    <img id="imgview" src="inventory/bitacora.png" Height="150px" width="150px"/>
                                    <hr>
                                </center>
                            </div>
                        </div>



                        <div class="row">
                            <div class="col">
                                <label>Asunto</label>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="asunto"
                                        runat="server" placeholder="Asunto"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox CssClass="form-control" ID="descripcion"
                                        runat="server" placeholder="Descripción"
                                        TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <label>Severidad</label>
                                        <div class="form-group">
                                            <asp:DropDownList CssClass="form-control" ID="DropDownListSeveridad" runat="server">
                                                <asp:ListItem Text="Crítico" Value="Critico" />
                                                <asp:ListItem Text="Muy urgente" Value="Muy urgente" />
                                                <asp:ListItem Text="Urgente" Value="Urgente" />
                                                <asp:ListItem Text="Normal" Value="Normal" />
                                                <asp:ListItem Text="Bajo" Value="Bajo" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="col-md-8 d-flex align-items-end">
                                        <div class="form-group w-100">
                                            
                                <asp:FileUpload onchange="readURL(this);"
                                    class="form-control"
                                    ID="FileUpload1"
                                    runat="server" />
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col">
                                        <center>
                                            <hr>
                                        </center>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <asp:Button ID="CrearTicket" runat="server" Text="Crear Ticket"
                                        CssClass="btn btn-primary btn-block btn-lg btn-2b399b" OnClick="CrearTicket_Click" />




                                </div>

                            </div>
                        </div>


                    </div>

                </div>
                <a href="InicioUsuario.aspx"><< Regresar al Inicio</a><br>
                <br />
            </div>
        </div>


    </div>
</asp:Content>


