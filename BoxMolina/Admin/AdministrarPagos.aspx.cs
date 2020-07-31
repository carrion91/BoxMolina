using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoxMolina.Admin
{
    public partial class AdministrarPagos : System.Web.UI.Page
    {
        #region variables globales
        PagoDatos pagoDatos = new PagoDatos();
        CostoDatos costoDatos = new CostoDatos();
        public static Pago pagoSeleccionado;
        ClienteDatos clienteDatos = new ClienteDatos();
        public static Cliente clienteSeleccionado;
        #endregion

        #region paginacion
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex, primerIndex2, ultimoIndex2;
        private int elmentosMostrar = 10;
        private int paginaActual
        {
            get
            {
                if (ViewState["paginaActual"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual"]);
            }
            set
            {
                ViewState["paginaActual"] = value;
            }
        }

        private int paginaActual2
        {
            get
            {
                if (ViewState["paginaActual2"] == null)
                {
                    return 0;
                }
                return ((int)ViewState["paginaActual2"]);
            }
            set
            {
                ViewState["paginaActual2"] = value;
            }
        }
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            if (!IsPostBack)
            {
                Session["listaPagos"] = pagoDatos.getPagos();
                Session["listaCostos"] = null;
                cargarPagos();
                llenarDdlCostos();
                Session["listaClientes"] = clienteDatos.getClientes();
                cargarClientes();
            }
        }
        #endregion

        #region logica

        private void llenarDdlCostos()
        {
            List<Costo> listaCostos = costoDatos.getCostos();
            Session["listaCostos"] = listaCostos;
            ddlCostos.DataSource = listaCostos;
            ddlCostos.DataTextField = "descripcion";
            ddlCostos.DataValueField = "monto";
            ddlCostos.DataBind();

            ddlCostosEditar.DataSource = listaCostos;
            ddlCostosEditar.DataTextField = "descripcion";
            ddlCostosEditar.DataValueField = "monto";
            ddlCostosEditar.DataBind();
        }

        public void cargarPagos()
        {
            List<Pago> listaPagos = (List<Pago>)Session["listaPagos"];

            List<Pago> listaPagosFiltrada = (List<Pago>)listaPagos.Where(pago => pago.cliente.nombreCompleto.ToUpper().Contains(txtBuscarCliente.Text.ToUpper()) &&
                pago.costo.descripcion.ToUpper().Contains(txtBuscarDescripcion.Text.ToUpper()) && pago.fechaDesde.ToShortDateString().ToUpper().Contains(txtBuscarDesde.Text.ToUpper()) &&
                pago.fechaHasta.ToShortDateString().ToUpper().Contains(txtBuscarHasta.Text.ToUpper()) && pago.monto.ToString().ToUpper().Contains(txtBuscarMonto.Text.ToUpper())).ToList();

            var dt = listaPagosFiltrada;
            pgsource.DataSource = dt;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina.Text = "Página " + (paginaActual + 1) + " de " + pgsource.PageCount + " (" + dt.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior.Enabled = !pgsource.IsFirstPage;
            lbSiguiente.Enabled = !pgsource.IsLastPage;
            lbPrimero.Enabled = !pgsource.IsFirstPage;
            lbUltimo.Enabled = !pgsource.IsLastPage;

            rpCostos.DataSource = pgsource;
            rpCostos.DataBind();

            //metodo que realiza la paginacion
            Paginacion();
        }

        public void cargarClientes()
        {
            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];
            List<Cliente> listaFiltrada = (List<Cliente>)listaClientes.Where(cliente =>
            cliente.nombreCompleto.ToUpper().Contains(txtBuscarNombre.Text.ToUpper()) &&
            cliente.cedula.ToString().ToUpper().Contains(txtBuscarCedula.Text.ToUpper()) &&
            cliente.correo.ToUpper().Contains(txtBucarCorreo.Text.ToUpper()) && cliente.telefono.ToUpper().Contains(txtBuscarTelefono.Text.ToUpper())
            && (cliente.tipoClase ? "Taekwondo" : "Funcional").ToUpper().Contains(txtBuscarTipoClase.Text.ToUpper())).ToList();

            var dt2 = listaFiltrada;
            pgsource.DataSource = dt2;
            pgsource.AllowPaging = true;
            //numero de items que se muestran en el Repeater
            pgsource.PageSize = elmentosMostrar;
            pgsource.CurrentPageIndex = paginaActual2;
            //mantiene el total de paginas en View State
            ViewState["TotalPaginas2"] = pgsource.PageCount;
            //Ejemplo: "Página 1 al 10"
            lblpagina2.Text = "Página " + (paginaActual2 + 1) + " de " + pgsource.PageCount + " (" + dt2.Count + " - elementos)";
            //Habilitar los botones primero, último, anterior y siguiente
            lbAnterior2.Enabled = !pgsource.IsFirstPage;
            lbSiguiente2.Enabled = !pgsource.IsLastPage;
            lbPrimero2.Enabled = !pgsource.IsFirstPage;
            lbUltimo2.Enabled = !pgsource.IsLastPage;

            rpClientes.DataSource = pgsource;
            rpClientes.DataBind();

            //metodo que realiza la paginacion
            Paginacion2();
        }
        #endregion

        #region paginacion
        private void Paginacion()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex = paginaActual - 2;
            if (paginaActual > 2)
                ultimoIndex = paginaActual + 2;
            else
                ultimoIndex = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex > Convert.ToInt32(ViewState["TotalPaginas"]))
            {
                ultimoIndex = Convert.ToInt32(ViewState["TotalPaginas"]);
                primerIndex = ultimoIndex - 4;
            }

            if (primerIndex < 0)
                primerIndex = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex; i < ultimoIndex; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion.DataSource = dt;
            rptPaginacion.DataBind();
        }

        protected void lbPrimero_Click(object sender, EventArgs e)
        {
            paginaActual = 0;
            cargarPagos();
        }

        protected void lbUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            cargarPagos();
        }

        protected void lbAnterior_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            cargarPagos();
        }

        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            cargarPagos();
        }

        protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
            cargarPagos();
        }

        protected void rptPaginacion_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion");
            if (lnkPagina.CommandArgument != paginaActual.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#000000");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        private void Paginacion2()
        {
            var dt = new DataTable();
            dt.Columns.Add("IndexPagina"); //Inicia en 0
            dt.Columns.Add("PaginaText"); //Inicia en 1

            primerIndex2 = paginaActual2 - 2;
            if (paginaActual2 > 2)
                ultimoIndex2 = paginaActual2 + 2;
            else
                ultimoIndex2 = 4;

            //se revisa que la ultima pagina sea menor que el total de paginas a mostrar, sino se resta para que muestre bien la paginacion
            if (ultimoIndex2 > Convert.ToInt32(ViewState["TotalPaginas2"]))
            {
                ultimoIndex2 = Convert.ToInt32(ViewState["TotalPaginas2"]);
                primerIndex2 = ultimoIndex2 - 4;
            }

            if (primerIndex2 < 0)
                primerIndex2 = 0;

            //se crea el numero de paginas basado en la primera y ultima pagina
            for (var i = primerIndex2; i < ultimoIndex2; i++)
            {
                var dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            rptPaginacion2.DataSource = dt;
            rptPaginacion2.DataBind();
        }

        protected void lbPrimero2_Click(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarClientes();
        }

        protected void lbUltimo2_Click(object sender, EventArgs e)
        {
            paginaActual2 = (Convert.ToInt32(ViewState["TotalPaginas2"]) - 1);
            cargarClientes();
        }

        protected void lbAnterior2_Click(object sender, EventArgs e)
        {
            paginaActual2 -= 1;
            cargarClientes();
        }

        protected void lbSiguiente2_Click(object sender, EventArgs e)
        {
            paginaActual2 += 1;
            cargarClientes();
        }

        protected void rptPaginacion2_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual2 = Convert.ToInt32(e.CommandArgument.ToString());
            cargarClientes();
        }

        protected void rptPaginacion2_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion2");
            if (lnkPagina.CommandArgument != paginaActual2.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#005da4");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        #endregion

        #region eventos
        protected void txtBuscarFiltro_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            cargarPagos();
        }

        protected void txtBuscarFiltroModal_TextChanged(object sender, EventArgs e)
        {
            paginaActual2 = 0;
            cargarClientes();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            txtMonto.Text = ddlCostos.SelectedValue;
            DateTime hoy = DateTime.Now;
            txtFechaDesde.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            if (ddlCostos.SelectedItem.Text.Equals("Día"))
            {
                txtFechaHasta.Text = DateTime.Now.ToString();
            }
            if (ddlCostos.SelectedItem.Text.Equals("Semana"))
            {
                hoy.AddDays(7);
                txtFechaHasta.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }
            if (ddlCostos.SelectedItem.Text.Equals("Mes"))
            {
                hoy.AddMonths(1);
                txtFechaHasta.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalNuevo();", true);
        }

        protected void btnSieliminar_Click(object sender, EventArgs e)
        {
            pagoDatos.eliminarPago(pagoSeleccionado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El pago ha sido eliminado correctamente" + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEliminar();", true);
            Session["listaPagos"] = pagoDatos.getPagos();
            cargarPagos();
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idPago = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Pago> listaCostos = (List<Pago>)Session["listaPagos"];

            pagoSeleccionado = (Pago)(listaCostos.Where(pago => pago.idPago == idPago).ToList().First());

            txtMontoEditar.Text = pagoSeleccionado.monto.ToString();
            txtFechaDesdeEditar.Text = pagoSeleccionado.fechaDesde.Year + "-" + (pagoSeleccionado.fechaDesde.Month < 10 ? "0" + pagoSeleccionado.fechaDesde.Month : pagoSeleccionado.fechaDesde.Month.ToString()) + "-" + (pagoSeleccionado.fechaDesde.Day < 10 ? "0" + pagoSeleccionado.fechaDesde.Day : pagoSeleccionado.fechaDesde.Day.ToString());
            txtFechaHastaEditar.Text = pagoSeleccionado.fechaHasta.Year + "-" + (pagoSeleccionado.fechaHasta.Month < 10 ? "0" + pagoSeleccionado.fechaHasta.Month : pagoSeleccionado.fechaHasta.Month.ToString()) + "-" + (pagoSeleccionado.fechaHasta.Day < 10 ? "0" + pagoSeleccionado.fechaHasta.Day : pagoSeleccionado.fechaHasta.Day.ToString());
            txtClienteSeleccionadoEditar.Text = pagoSeleccionado.cliente.nombreCompleto;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEditar();", true);
        }

        protected void btnSeleccionarClienteModalNuevo_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalSeleccionar();", true);
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idPago = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Pago> listaPagos = (List<Pago>)Session["listaPagos"];

            pagoSeleccionado = (Pago)(listaPagos.Where(pago => pago.idPago == idPago).ToList().First());

            lblEliminar.Text = "¿Esta seguro de eliminar el pago de " + pagoSeleccionado.cliente.nombreCompleto + " de forma permanente?";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEliminar();", true);
        }

        protected void ddlCostosEditar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMontoEditar.Text = ddlCostosEditar.SelectedValue;
            DateTime hoy = pagoSeleccionado.fechaDesde;
            txtFechaDesdeEditar.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            if (ddlCostosEditar.SelectedItem.Text.Equals("Día"))
            {
                txtFechaHastaEditar.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }
            if (ddlCostosEditar.SelectedItem.Text.Equals("Semana"))
            {
                hoy = hoy.AddDays(7);
                txtFechaHastaEditar.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }
            if (ddlCostosEditar.SelectedItem.Text.Equals("Mes"))
            {
                hoy = hoy.AddMonths(1);
                txtFechaHastaEditar.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }
        }

        protected void ddlCostos_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMonto.Text = ddlCostos.SelectedValue;
            DateTime hoy = DateTime.Now;
            txtFechaDesde.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            if (ddlCostos.SelectedItem.Text.Equals("Día"))
            {
                txtFechaHasta.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }
            if (ddlCostos.SelectedItem.Text.Equals("Semana"))
            {
                hoy = hoy.AddDays(7);
                txtFechaHasta.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }
            if (ddlCostos.SelectedItem.Text.Equals("Mes"))
            {
                hoy = hoy.AddMonths(1);
                txtFechaHasta.Text = hoy.Year + "-" + (hoy.Month < 10 ? "0" + hoy.Month : hoy.Month.ToString()) + "-" + (hoy.Day < 10 ? "0" + hoy.Day : hoy.Day.ToString());
            }

        }

        protected void btnSeleccionarClienteModal_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];

            clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

            txtClienteSeleccionado.Text = clienteSeleccionado.nombreCompleto;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalSeleccionar();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + clienteSeleccionado.nombreCompleto + " ha sido seleccionad@ correctamente." + "');", true);
        }

        protected void btnNuevoCosto_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtClienteSeleccionado.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe seleccionar un cliente." + "');", true);
            }
            else
            {
                if (String.IsNullOrEmpty(txtFechaDesde.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Debe seleccionar el campo de Fecha desde." + "');", true);
                }
                else
                {
                    if (String.IsNullOrEmpty(txtFechaHasta.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Debe seleccionar el campo de Fecha hasta." + "');", true);
                    }
                    else
                    {
                        DateTime fechaDesde = Convert.ToDateTime(txtFechaDesde.Text);
                        DateTime fechaHasta = Convert.ToDateTime(txtFechaHasta.Text);
                        if (DateTime.Compare(fechaHasta, fechaDesde) < 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "La fecha desde debe de ser menor que la fecha hasta." + "');", true);
                        }
                        else
                        {
                            Int32.TryParse(txtMonto.Text, out Int32 mont);

                            if (mont <= 0)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe ingresar un monto." + "');", true);
                            }
                            else
                            {
                                Pago pago = new Pago();
                                pago.cliente = clienteSeleccionado;
                                Costo costo = new Costo();
                                List<Costo> listaCostos = (List<Costo>)Session["listaCostos"];
                                costo = (Costo)(listaCostos.Where(costos => costos.descripcion == (ddlCostos.SelectedItem.Text)).ToList().First());
                                pago.costo = costo;
                                pago.fechaDesde = fechaDesde;
                                pago.fechaHasta = fechaHasta;
                                pago.monto = Convert.ToDouble(txtMonto.Text);
                                pagoDatos.insertarPago(pago);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El pago se ingreso correctamente." + "');", true);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalNuevo();", true);
                                Session["listaPagos"] = pagoDatos.getPagos();
                                cargarPagos();
                            }
                        }
                    }
                }
            }
        }

        protected void btnSiEditar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtFechaDesdeEditar.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Debe seleccionar el campo de Fecha desde." + "');", true);
            }
            else
            {
                if (String.IsNullOrEmpty(txtFechaHastaEditar.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Debe seleccionar el campo de Fecha hasta." + "');", true);
                }
                else
                {
                    DateTime fechaDesde = Convert.ToDateTime(txtFechaDesdeEditar.Text);
                    DateTime fechaHasta = Convert.ToDateTime(txtFechaHastaEditar.Text);
                    if (DateTime.Compare(fechaHasta, fechaDesde) < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "La fecha desde debe de ser menor que la fecha hasta." + "');", true);
                    }
                    else
                    {
                        Int32.TryParse(txtMontoEditar.Text, out Int32 mont);

                        if (mont <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe ingresar un monto." + "');", true);
                        }
                        else
                        {

                            Pago pago = pagoSeleccionado;
                            Costo costo = new Costo();
                            List<Costo> listaCostos = (List<Costo>)Session["listaCostos"];
                            costo = (Costo)(listaCostos.Where(costos => costos.descripcion == (ddlCostosEditar.SelectedItem.Text)).ToList().First());
                            pago.costo = costo;
                            pago.fechaDesde = fechaDesde;
                            pago.fechaHasta = fechaHasta;
                            pago.monto = Convert.ToDouble(txtMontoEditar.Text);
                            pagoDatos.actualizarPago(pago);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El pago se actualizo correctamente." + "');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEditar();", true);
                            Session["listaPagos"] = pagoDatos.getPagos();
                            cargarPagos();
                        }
                    }
                }
            }
        }
        #endregion
    }
}