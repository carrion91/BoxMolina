<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BoxMolina.Default" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Contenido" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12 col-xs-12 col-sm-12">
                    <div id="demo" class="carousel slide" data-ride="carousel">

                        <!-- Indicators -->
                        <ul class="carousel-indicators">
                            <li data-target="#demo" data-slide-to="0" class="active"></li>
                            <li data-target="#demo" data-slide-to="1"></li>
                            <li data-target="#demo" data-slide-to="2"></li>
                        </ul>

                        <!-- The slideshow -->
                        <div class="carousel-inner">
                            <div class="carousel-item active">

                                <img class="d-block w-100" style="height: 300px" src="Imagenes/gym1.jpg" />
                                <div class="carousel-caption d-none d-md-block">
                                    <h5>El mejor material</h5>
                                    <p>Lo mejor para nuestros clientes</p>
                                </div>
                            </div>
                            <div class="carousel-item">
                                <img class="d-block w-100" style="height: 300px" src="Imagenes/gym2.jpg" />
                                <div class="carousel-caption d-none d-md-block">
                                    <h5>Nuestros clientes son nuestra prioridad</h5>
                                    <p>Todos son igualmente importante</p>
                                </div>
                            </div>
                            <div class="carousel-item">
                                <img class="d-block w-100" style="height: 300px" src="Imagenes/gym3.jpg" />
                                <div class="carousel-caption d-none d-md-block">
                                    <h5>Siempre tratando de mejorar</h5>
                                    <p>Nuestros clientes lo merecen</p>
                                </div>
                            </div>
                        </div>

                        <!-- Left and right controls -->
                        <a class="carousel-control-prev" href="#demo" data-slide="prev">
                            <span class="carousel-control-prev-icon"></span>
                        </a>
                        <a class="carousel-control-next" href="#demo" data-slide="next">
                            <span class="carousel-control-next-icon"></span>
                        </a>

                    </div>
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12">
                    <br />
                </div>

                <div class="col-md-12 col-xs-12 col-sm-12" style="text-align: center">
                    <asp:Panel ID="pnlImagenes" runat="server">
                    </asp:Panel>
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
