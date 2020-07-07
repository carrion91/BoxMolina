<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdministrarClientes.aspx.cs" Inherits="BoxMolina.Admin.AdministrarClientes" MaintainScrollPositionOnPostback="true" %>

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
                    <asp:Label runat="server" Text="Clientes" Font-Bold="true"></asp:Label>
                </div>
                <%-- fin titulo pantalla --%>

                <div class="col-md-12 col-sm-12">
                    <hr />
                </div>

                <%-- tabla--%>
                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center; overflow-y: auto;">

                    <table class="table table-bordered">
                        <thead>
                            <tr style="text-align: center; background-color: black; color: white">
                                <th></th>
                                <th>Nombre completo</th>
                                <th>Clase</th>
                                <th>Cédula</th>
                                <th>Teléfono</th>
                                <th>Correo</th>
                                <th>Activo</th>
                                <th>Confirmado</th>
                            </tr>
                        </thead>
                        <tr>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txtBuscarNombre" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscarFiltro_TextChanged" placeholder="Filtro nombre"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuscarTipoClase" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscarFiltro_TextChanged" placeholder="Filtro clase"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuscarCedula" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscarFiltro_TextChanged" placeholder="Filtro cédula"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBuscarTelefono" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscarFiltro_TextChanged" placeholder="Filtro teléfono"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBucarCorreo" runat="server" CssClass="form-control chat-input" AutoPostBack="true" OnTextChanged="txtBuscarFiltro_TextChanged" placeholder="Filtro correo"></asp:TextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <asp:Repeater ID="rpClientes" runat="server">
                            <HeaderTemplate>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="btnEditar" runat="server" ToolTip="Editar" OnClick="btnEditar_Click" CommandArgument='<%# Eval("idCliente") %>' CssClass="btn glyphicon glyphicon-pencil" ForeColor="Black"></asp:LinkButton>
                                        <asp:LinkButton ID="btnEliminar" runat="server" ToolTip="Eliminar" OnClick="btnEliminar_Click" CommandArgument='<%# Eval("idCliente") %>' CssClass="btn glyphicon glyphicon-trash" ForeColor="Black"></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("nombreCompleto") %>
                                    </td>
                                    <td style="width: 10%">
                                        <%# Convert.ToBoolean(Eval("tipoClase")) ? "Taekwondo":"Funcional" %>
                                    </td>
                                    <td style="width: 10%">
                                        <%# Eval("cedula") %>
                                    </td>
                                    <td style="width: 10%">
                                        <%# Eval("telefono") %>
                                    </td>
                                    <td>
                                        <%# Eval("correo") %>
                                    </td>
                                    <td style="width: 5%">
                                        <asp:LinkButton ID="bntActivar" runat="server" Text="Activar" ToolTip="Activar" OnClick="bntActivar_Click" CommandArgument='<%# Eval("idCliente") %>' Visible='<%# !Convert.ToBoolean(Eval("activo")) %>' CssClass="btn btn-default"></asp:LinkButton>
                                        <asp:LinkButton ID="btnDesactivar" runat="server" Text="Desactivar" ToolTip="Desactivar" OnClick="btnDesactivar_Click" CommandArgument='<%# Eval("idCliente") %>' Visible='<%# Convert.ToBoolean(Eval("activo")) %>' CssClass="btn btn-default"></asp:LinkButton>
                                    </td>
                                    <td style="width: 5%">
                                        <asp:LinkButton ID="btnConfirmar" runat="server" Text="Confirmar" ToolTip="Confirmar" OnClick="btnConfirmar_Click" CommandArgument='<%# Eval("idCliente") %>' Visible='<%# !Convert.ToBoolean(Eval("confirmado")) %>' CssClass="btn btn-default"></asp:LinkButton>
                                        <asp:LinkButton ID="btnDesconfirmar" runat="server" Text="Desconfirmar" ToolTip="Desconfirmar" OnClick="btnDesconfirmar_Click" CommandArgument='<%# Eval("idCliente") %>' Visible='<%# Convert.ToBoolean(Eval("confirmado")) %>' CssClass="btn btn-default"></asp:LinkButton>
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
                                    <asp:Label runat="server" Text="Eliminar cliente" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <br />
                                </div>

                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <asp:Label ID="lblEliminar" runat="server" Text="Eliminar cliente"></asp:Label>
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
                                    <asp:Label runat="server" Text="Editar cliente" Font-Bold="true"></asp:Label>
                                </div>

                                <div class="col-md-12 col-sm-12" style="align-content: center; text-align: center">
                                    <br />
                                </div>

                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label1" runat="server" Text="Nombre completo"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-12">
                                        <asp:TextBox ID="txtNombreCompleto" runat="server" CssClass="form-control" placeholder="Nombre completo"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label2" runat="server" Text="Cédula (formato 123456789)"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-12">
                                        <asp:TextBox ID="txtCedula" runat="server" CssClass="form-control" placeholder="123456789" MaxLength="9" MinLength="9" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label3" runat="server" Text="Teléfono"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-12">
                                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Teléfono"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>
                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label4" runat="server" Text="Correo"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-12">
                                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Correo" TextMode="Email"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="col-md-12 col-sm-12">
                                    <br />
                                </div>

                                <div class="col-md-12 col-sm-12">
                                    <div class="col-md-3 col-sm-12">
                                        <asp:Label ID="Label7" runat="server" Text="Clase de"></asp:Label>
                                    </div>
                                    <div class="col-md-9 col-sm-12">
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                            <asp:ListItem Text="Box funcional" Value="F" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Taekwondo" Value="T"></asp:ListItem>
                                        </asp:RadioButtonList>
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
