<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="misAgentes.aspx.cs" Inherits="HelpDesk.misAgentes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="row">

            <div class="col-md-10 mx-auto">
                <div class="card">
                    <div class="card-body">


                      <h3 class="text-center">Agentes</h3>


                        <div class="row">

                         
<asp:SqlDataSource
    ID="SqlDataSource1"
    runat="server"
    ConnectionString="<%$ ConnectionStrings:ServerCon %>"
    SelectCommand="
        SELECT email, nombre, nivel, tAbiertos, telefono, habilidades
        FROM [hd].[agente] ">
</asp:SqlDataSource>



                            <div class="col">
                                <!-- Responsive wrapper prevents overflow OnSelectedIndexChanged="GridView1_SelectedIndexChanged"-->
                                <div class="table-responsive">
                                    <asp:GridView
                                        ID="GridView1"
                                        runat="server"
                                        CssClass="table table-striped table-bordered"
                                        AutoGenerateColumns="False"
                                        DataSourceID="SqlDataSource1"
                                        ForeColor="#333333"
                                        GridLines="None"
                                        >

                                        <AlternatingRowStyle BackColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />

                                        <Columns>
                                         
                                            
<asp:BoundField DataField="email" HeaderText="email" SortExpression="email"
    ItemStyle-Width="70px">
</asp:BoundField>


<asp:BoundField DataField="nombre" HeaderText="nombre" SortExpression="nombre" />

                                            <asp:BoundField DataField="nivel" HeaderText="nivel" SortExpression="nivel" />
                                            <asp:BoundField DataField="tAbiertos" HeaderText="tAbiertos" SortExpression="tAbiertos" />
                                            <asp:BoundField DataField="telefono" HeaderText="telefono" SortExpression="telefono" />
                                            <asp:BoundField DataField="habilidades" HeaderText="habilidades" SortExpression="habilidades" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

                <a href="<%= ResolveUrl("~/InicioUsuario.aspx") %>">&laquo; Regresar al Inicio</a><br />
                <br />
            </div>

        </div>
    </div>

</asp:Content>
