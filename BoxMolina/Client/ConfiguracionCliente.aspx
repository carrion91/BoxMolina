<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfiguracionCliente.aspx.cs" Inherits="BoxMolina.Client.ConfiguracionCliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row;contenido">
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
                        <td style="align-content:center;text-align:center">
                            <asp:Label ID="Label9" runat="server" Text="Cambiar contraseña"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="col-md-12 col-sm-12">
                                <div class="col-md-3 col-sm-12">
                                    <asp:Label ID="Label8" runat="server" Text="Contraseña actual"></asp:Label>
                                </div>
                                <div class="col-md-9 col-sm-12">
                                    <asp:TextBox ID="txtContrasenna0" runat="server" CssClass="form-control" placeholder="Contraseña actual" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12">
                                <br />
                            </div>
                            <div class="col-md-12 col-sm-12">
                                <div class="col-md-3 col-sm-12">
                                    <asp:Label ID="Label5" runat="server" Text="Nueva contraseña"></asp:Label>
                                </div>
                                <div class="col-md-9 col-sm-12">
                                    <asp:TextBox ID="txtContrasenna1" runat="server" CssClass="form-control" placeholder="Nueva contraseña" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12 col-sm-12">
                                <br />
                            </div>
                            <div class="col-md-12 col-sm-12">
                                <div class="col-md-3 col-sm-12">
                                    <asp:Label ID="Label6" runat="server" Text="Confirmación de nueva contraseña (vuelva a ingresarla)"></asp:Label>
                                </div>
                                <div class="col-md-9 col-sm-12">
                                    <asp:TextBox ID="txtContrasenna2" runat="server" CssClass="form-control" placeholder="Confirmación de nueva contraseña" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
      
                <div class="col-md-12 col-sm-12" style="align-content:center;text-align:center">
                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar información" OnClick="btnActualizar_Click" CssClass="btn btn-default" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
