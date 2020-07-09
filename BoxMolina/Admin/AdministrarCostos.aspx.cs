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
    public partial class AdministrarCostos : System.Web.UI.Page
    {
        #region variables globales
        CostoDatos costoDatos = new CostoDatos();
        public static Costo costoSeleccionado;
        #endregion

        #region paginacion
        readonly PagedDataSource pgsource = new PagedDataSource();
        int primerIndex, ultimoIndex;
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
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            int[] rolesPermitidos = { 2 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            if (!IsPostBack)
            {
                Session["listaCostos"] = costoDatos.getCostos();
                cargarCostos();
            }
        }
        #endregion

        #region logica
        public void cargarCostos()
        {
            List<Costo> listaCostos = (List<Costo>)Session["listaCostos"];

            List<Costo> listaCostosFiltrada = (List<Costo>)listaCostos.Where(costo => costo.descripcion.ToUpper().Contains(txtBuscarDescripcion.Text.ToUpper())
            && costo.monto.ToString().ToUpper().Contains(txtBuscarMonto.Text.ToUpper())).ToList();

            var dt = listaCostosFiltrada;
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
            cargarCostos();
        }

        protected void lbUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            cargarCostos();
        }

        protected void lbAnterior_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            cargarCostos();
        }

        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            cargarCostos();
        }

        protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
            cargarCostos();
        }

        protected void rptPaginacion_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var lnkPagina = (LinkButton)e.Item.FindControl("lbPaginacion");
            if (lnkPagina.CommandArgument != paginaActual.ToString()) return;
            lnkPagina.Enabled = false;
            lnkPagina.BackColor = Color.FromName("#000000");
            lnkPagina.ForeColor = Color.FromName("#FFFFFF");
        }

        #endregion

        #region eventos
        protected void txtBuscarFiltro_TextChanged(object sender, EventArgs e)
        {
            paginaActual = 0;
            cargarCostos();
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            txtDescripcion.Text = "";
            txtMonto.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalNuevo();", true);
        }

        protected void btnNuevoCosto_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtDescripcion.Text))
            {
                if (!String.IsNullOrEmpty(txtMonto.Text)&&Convert.ToDouble(txtMonto.Text)>0)
                {
                    Costo costo = new Costo();
                    costo.descripcion = txtDescripcion.Text;
                    costo.monto = Convert.ToDouble(txtMonto.Text); ;
                    costoDatos.insertarCosto(costo);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se guardo el monto correctamente" + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalNuevo();", true);
                    Session["listaCostos"] = costoDatos.getCostos();
                    cargarCostos();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar el monto" + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar la descripción" + "');", true);
            }
        }

        protected void btnSieliminar_Click(object sender, EventArgs e)
        {
            costoDatos.eliminarCosto(costoSeleccionado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El costo ha sido eliminado correctamente" + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEliminar();", true);
            Session["listaCostos"] = costoDatos.getCostos();
            cargarCostos();
        }

        protected void btnSiEditar_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEditarDescripcion.Text))
            {
                if (!String.IsNullOrEmpty(txtEditarMonto.Text)&&Convert.ToDouble(txtEditarMonto.Text)>0)
                {
                    costoSeleccionado.descripcion = txtEditarDescripcion.Text;
                    costoSeleccionado.monto = Convert.ToDouble(txtEditarMonto.Text);
                    costoDatos.actualizarCosto(costoSeleccionado);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se edito el monto correctamente" + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEditar();", true);
                    Session["listaCostos"] = costoDatos.getCostos();
                    cargarCostos();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar el monto" + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar una descripción" + "');", true);
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idCosto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Costo> listaCostos = (List<Costo>)Session["listaCostos"];

            costoSeleccionado = (Costo)(listaCostos.Where(costo => costo.idCosto == idCosto).ToList().First());

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEditar();", true);
            txtEditarDescripcion.Text = costoSeleccionado.descripcion;
            txtEditarMonto.Text = costoSeleccionado.monto.ToString();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idCosto = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Costo> listaCostos = (List<Costo>)Session["listaCostos"];

            costoSeleccionado = (Costo)(listaCostos.Where(costo => costo.idCosto == idCosto).ToList().First());

            lblEliminar.Text = "¿Esta seguro de eliminar el costo de " + costoSeleccionado.descripcion + " de forma permanente y toda su información asociada?";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEliminar();", true);
        }
        #endregion
    }
}