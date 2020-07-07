<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BoxMolina.Default" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="background-color: black; text-align: right">
                <asp:LinkButton ID="btnIniciarSesion" OnClick="btnIniciarSesion_Click" runat="server" Style="color: white">Iniciar sesión</asp:LinkButton>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- Modal de sesion -->
    <div id="modalSesion" class="modal fade" role="alertdialog">
        <div class="modal-dialog modal-lg">

            <!-- Modal content -->
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <!-- Modal body -->
                        <div class="modal-body">

                            <!-- tabs -->
                            <ul class="nav nav-tabs">
                                <li id="liIngresar" runat="server" role="presentation" class="active">
                                    <asp:LinkButton ID="btnViewIngresar" runat="server" Text="Ingresar" OnClick="btnViewIngresar_Click"></asp:LinkButton>
                                </li>
                                <li id="liRegistrarse" runat="server" role="presentation">
                                    <asp:LinkButton ID="btnViewRegistrarse" runat="server" Text="Registrarse" OnClick="btnViewRegistrarse_Click"></asp:LinkButton>
                                </li>
                            </ul>
                            <!-- fin tabs -->

                            <div class="tab-content">
                                <!-- ------------------------ VISTA ingresar --------------------------- -->
                                <div id="ViewIngresar" runat="server" style="display: block">
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12" style="text-align: center;">
                                            <label for="exampleInputEmail1">Usuario (cédula)</label>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control chat-input" placeholder="123456789" MaxLength="9"></asp:TextBox>

                                            </div>
                                            <br />
                                            <label for="exampleInputPassword1">Contraseña </label>
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                                                <asp:TextBox ID="txtPassword" runat="server" CssClass=" form-control chat-input" TextMode="Password" placeholder="Contraseña" OnTextChanged="btnIngresar_Click"></asp:TextBox>
                                            </div>
                                            <br />
                                        </div>
                                        <div class="col-md-12 col-sm-12">
                                            <asp:CheckBox ID="ChckBxAdministrador" runat="server" Text="Administrador" />
                                            <div style="text-align: center; align-content: center">
                                                <asp:LinkButton ID="btnOlvidoContrasenna" runat="server">Olvidó su contraseña?</asp:LinkButton>
                                            </div>
                                        </div>

                                        <div class="col-md-12 col-sm-12">
                                            <asp:Button ID="btnIngresar" runat="server" CssClass="btn btn-default" Text="Ingresar" OnClick="btnIngresar_Click" />
                                            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <!-- ------------------------ Fin VISTA ingresar --------------------------- -->
                            <!-- ------------------------ VISTA registrarse --------------------------- -->
                            <div id="ViewRegistrarse" runat="server" style="display: none">
                                <div class="row">
                                    <br />
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

                                    <table class="table table-inverse">
                                        <tr>
                                            <td>
                                                <div class="col-md-12 col-sm-12">
                                                    <div class="col-md-3 col-sm-12">
                                                        <asp:Label ID="Label5" runat="server" Text="Contraseña"></asp:Label>
                                                    </div>
                                                    <div class="col-md-9 col-sm-12">
                                                        <asp:TextBox ID="txtContrasenna1" runat="server" CssClass="form-control" placeholder="Contraseña" TextMode="Password"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-12 col-sm-12">
                                                    <br />
                                                </div>
                                                <div class="col-md-12 col-sm-12">
                                                    <div class="col-md-3 col-sm-12">
                                                        <asp:Label ID="Label6" runat="server" Text="Confirmación de contraseña (vuelva a ingresarla)"></asp:Label>
                                                    </div>
                                                    <div class="col-md-9 col-sm-12">
                                                        <asp:TextBox ID="txtContrasenna2" runat="server" CssClass="form-control" placeholder="Confirmación de contraseña" TextMode="Password"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="col-md-12 col-sm-12">
                                        <br />
                                    </div>
                                    <div class="col-md-12 col-sm-12">
                                        <asp:Button ID="btnRegistrar" runat="server" CssClass="btn btn-default" Text="Registrar" OnClick="btnRegistrar_Click" />
                                        <button type="button" class="btn btn-danger" data-dismiss="modal">Cancelar</button>
                                    </div>
                                </div>
                            </div>
                            <!-- ------------------------ Fin VISTA registrarse --------------------------- -->
                        </div>
                        <!-- Fin Modal body -->

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Fin Modal content -->
    </div>

    <!-- Fin Modal de sesion -->

    <script type="text/javascript">

        function levantarModalSesion() {
            $('#modalSesion').modal('show');
        };

        function cerrarModalSesion() {
            $('#modalSesion').modal('hide');
        };

        function verViewIngresar() {
            document.getElementById('<%=liIngresar.ClientID%>').className = "active";
            document.getElementById('<%=liRegistrarse.ClientID%>').className = "";

            document.getElementById('<%=ViewRegistrarse.ClientID%>').style.display = 'none';
            document.getElementById('<%=ViewIngresar.ClientID%>').style.display = 'block';
        };

        function verViewRegistrarse() {
            document.getElementById('<%=liIngresar.ClientID%>').className = "";
            document.getElementById('<%=liRegistrarse.ClientID%>').className = "active";

            document.getElementById('<%=ViewRegistrarse.ClientID%>').style.display = 'block';
            document.getElementById('<%=ViewIngresar.ClientID%>').style.display = 'none';
        };
    </script>
</asp:Content>
