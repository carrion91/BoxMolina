<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdministrarCostos.aspx.cs" Inherits="BoxMolina.Admin.AdministrarCostos" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>

            <div class="row;contenido">

                <div class="col-md-12 col-sm-12">
                    <br />
                </div>

                <%-- titulo pantalla --%>
                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                    <asp:Label runat="server" Text="Costos" Font-Bold="true"></asp:Label>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-sm-12">
                    <hr />
                </div>

                <div class="col-md-12 col-sm-12">
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo costo" OnClick="btnNuevo_Click" CssClass="btn btn-default" />
                </div>

                <div class="col-md-12 col-sm-12">
                    <hr />
                </div>

                <%-- tabla--%>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">

                    <table class="table table-bordered">
                        <thead>
                            <tr style="text-align: center; background-color: black; color: white">
                                <th></th>
                                <th>Descripción</th>
                                <th>Monto</th>
                            </tr>
                        </thead>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtBuscarDescripcion" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscarFiltro_TextChanged" placeholder="Filtro descripción"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuscarMonto" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscarFiltro_TextChanged" placeholder="Filtro monto"></asp:TextBox>
                            </td>
                        </tr>
                        <asp:Repeater ID="rpCostos" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="btnEditar" OnClick="btnEditar_Click" runat="server" ToolTip="Editar" CommandArgument='<%# Eval("idCosto") %>' CssClass="btn glyphicon glyphicon-pencil" ForeColor="Black"></asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" OnClick="btnEliminar_Click" runat="server" ToolTip="Eliminar" CommandArgument='<%# Eval("idCosto") %>' CssClass="btn glyphicon glyphicon-trash" ForeColor="Black"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("descripcion")%>
                                    </td>
                                    <td>
                                        ₡ <%# Eval("monto") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                            </FooterTemplate>
                        </asp:Repeater>
                    </table>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">
                    <center>
                    <table class="table" style="max-width:664px;">
                        <tr style="padding:1px !important">
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbPrimero" runat="server" CssClass="btn" Style="background-color:black" OnClick="lbPrimero_Click"><span class="glyphicon glyphicon-fast-backward" style="color:white"></span></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbAnterior" runat="server" CssClass="btn btn-default" OnClick="lbAnterior_Click"><span class="glyphicon glyphicon-backward"></asp:LinkButton>
                            </td>
                            <td style="padding:1px !important">
                                <asp:DataList ID="rptPaginacion" runat="server"
                                    OnItemCommand="rptPaginacion_ItemCommand"
                                    OnItemDataBound="rptPaginacion_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaginacion" runat="server" CssClass="btn btn-default"
                                            CommandArgument='<%# Eval("IndexPagina") %>' CommandName="nuevaPagina"
                                            Text='<%# Eval("PaginaText") %>' ></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbSiguiente" CssClass="btn btn-default" runat="server" OnClick="lbSiguiente_Click"><span class="glyphicon glyphicon-forward"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:LinkButton ID="lbUltimo" CssClass="btn" Style="background-color:black" runat="server" OnClick="lbUltimo_Click"><span class="glyphicon glyphicon-fast-forward" style="color:white"></asp:LinkButton>
                                </td>
                            <td style="padding:1px !important">
                                <asp:Label ID="lblpagina" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                        </center>
                </div>
                <%-- fin tabla--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal de nuevo -->
    <div id="modalNuevo" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content -->
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <asp:Label runat="server" Text="Nuevo costo" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <br />
                                </div>

                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label1" runat="server" Text="Descripción"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-12">
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Descripción"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label2" runat="server" Text="Monto"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtMonto" runat="server" CssClass="form-control" placeholder="Monto" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>
                            </div>
                        </div>
                        <!-- Fin Modal body -->
                        <!-- Modal footer -->
                        <div class="modal-footer" style="align-content: center; text-align: center;">
                            <asp:Button ID="btnNuevoCosto" runat="server" Text="Nuevo" CssClass="btn btn-default" OnClick="btnNuevoCosto_Click" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                        </div>
                        <!-- Fin Modal footer -->
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Fin Modal content -->
    </div>
    <!-- Fin Modal de nuevo -->

    <!-- Modal de eliminar -->
    <div id="modalEliminar" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content -->
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <asp:Label runat="server" Text="Eliminar costo" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <br />
                                </div>

                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <asp:Label ID="lblEliminar" runat="server" Text="Eliminar costo"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <!-- Fin Modal body -->

                        <!-- Modal footer -->
                        <div class="modal-footer" style="align-content: center; text-align: center;">
                            <asp:Button ID="btnSieliminar" runat="server" Text="Si" CssClass="btn btn-default" OnClick="btnSieliminar_Click" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">No</button>
                        </div>
                        <!-- Fin Modal footer -->

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Fin Modal content -->
    </div>
    <!-- Fin Modal de eliminar -->

    <!-- Modal de editar -->
    <div id="modalEditar" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">
            <!-- Modal content -->
            <div class="modal-content">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <asp:Label runat="server" Text="Editar costo" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <br />
                                </div>

                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label3" runat="server" Text="Descripción"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-12">
                                        <asp:TextBox ID="txtEditarDescripcion" runat="server" CssClass="form-control" placeholder="Descripción"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label4" runat="server" Text="Monto"></asp:Label>
                                    </div>
                                    <div class="col-md-3 col-sm-12">
                                        <div class="input-group">
                                            <span class="input-group-addon">₡</span>
                                            <asp:TextBox ID="txtEditarMonto" runat="server" CssClass="form-control" placeholder="Monto" TextMode="Number"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>
                            </div>
                        </div>
                        <!-- Fin Modal body -->
                        <!-- Modal footer -->
                        <div class="modal-footer" style="align-content: center; text-align: center;">
                            <asp:Button ID="btnSiEditar" runat="server" Text="Editar" CssClass="btn btn-default" OnClick="btnSiEditar_Click" />
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                        </div>
                        <!-- Fin Modal footer -->
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Fin Modal content -->
    </div>
    <!-- Fin Modal de editar -->

    <script type="text/javascript">

        function levantarModalNuevo() {
            $('#modalNuevo').modal('show');
        };

        function cerrarModalNuevo() {
            $('#modalNuevo').modal('hide');
        };

        function levantarModalEditar() {
            $('#modalEditar').modal('show');
        };

        function cerrarModalEditar() {
            $('#modalEditar').modal('hide');
        };

        function levantarModalEliminar() {
            $('#modalEliminar').modal('show');
        };

        function cerrarModalEliminar() {
            $('#modalEliminar').modal('hide');
        };

    </script>
</asp:Content>
