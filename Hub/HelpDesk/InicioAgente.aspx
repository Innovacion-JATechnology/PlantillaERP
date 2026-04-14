<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="InicioAgente.aspx.cs" Inherits="HelpDesk.InicioAgente" %>

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
                    <br></br>
                </div>
            </div>

            <div class="row">


                <div class="col-md-4">
                    <a href="allUsers.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="150px" src="imgs/catalogodeUsuarios.png" />
                            <h4>Ver Usuarios</h4>
                            <p class="text-center">
                                Click para ver todos los usuarios.
                            </p>
                        </center>
                    </a>
                </div>



                <div class="col-md-4">
                    <a href="usersignup.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="150px" src="imgs/agregarUsuario.png" />
                            <h4>Agregar Usuario</h4>
                            <p class="text-center">
                                Crear un nuevo usuario 
                            </p>
                        </center>
                    </a>
                </div>
                
                <div class="col-md-4"> 
                    <a href="adminAllTickets.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="150px" src="imgs/todosLosTickets.png" />
                            <h4>Ver Tickets</h4>
                            <p class="text-center">
                                Click para ver todos los Tickets.
                            </p>
                        </center>
                    </a>
                </div>
                
                <div class="col-md-4">
                    <a href="AdminAllAgents.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="150px" src="imgs/veragentes.png" />
                            <h4>Ver Agentes</h4>
                            <p class="text-center">
                                Ver y modificar todos los Agentes.
                            </p>
                        </center>
                    </a>
                </div>

                <div class="col-md-4">
                    <a href="agregarAgente.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="150px" src="imgs/agregarAgente.png" />
                            <h4>Agregar Agente</h4>
                            <p class="text-center">
                                Click para agregar Agente.
                            </p>
                        </center>
                    </a>
                </div>

                

                <div class="col-md-4">
                    <a href="ActualizaTablas.aspx" style="text-decoration: none; color: inherit;">
                        <center>
                            <img height="150px" src="imgs/mantenimiento.png" />
                            <h4>Mantenimiento</h4>
                            <p class="text-center">
                                Hacer Ajustes.
                            </p>
                        </center>
                    </a>
                </div>

            </div>

        </div>
    </section>


</asp:Content>
