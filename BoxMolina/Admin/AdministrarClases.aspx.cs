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
    public partial class AdministrarClases : System.Web.UI.Page
    {
        #region variables globales
        ClaseDatos claseDatos = new ClaseDatos();
        public static Clase claseSeleccionada;
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
                Session["listaClases"] = claseDatos.getClases();
                cargarClases();
            }
        }
        #endregion

        #region logica
        public void cargarClases()
        {
            List <Clase> listaClases = (List<Clase>)Session["listaClases"];

            List<Clase> listaClasesFiltrada = (List<Clase>)listaClases.Where(clase => clase.cupo.ToString().ToUpper().Contains(txtBuscarCupo.Text.ToUpper())
            && clase.hora.ToString().ToUpper().Contains(txtBuscarHora.Text.ToUpper())).ToList();
            
            var dt = listaClasesFiltrada;
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

            rpClases.DataSource = pgsource;
            rpClases.DataBind();

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
            cargarClases();
        }

        protected void lbUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            cargarClases();
        }

        protected void lbAnterior_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            cargarClases();
        }

        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            cargarClases();
        }

        protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
            cargarClases();
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
            cargarClases();
        }
        
        protected void btnNueva_Click(object sender, EventArgs e)
        {
            txtCupo.Text = "";
            txtHora.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalNueva();", true);
        }

        protected void btnNuevaClase_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtHora.Text))
            {
                if (!String.IsNullOrEmpty(txtCupo.Text))
                {
                    String[] horaText = txtHora.Text.Split(':');
                    int hora = Convert.ToInt32(horaText[0]);
                    int minutos = Convert.ToInt32(horaText[1]);
                    int cupo = Convert.ToInt32(txtCupo.Text);

                    Clase clase = new Clase();
                    clase.cupo = cupo;
                    clase.hora = hora;
                    clase.minutos = minutos;
                    claseDatos.insertarClase(clase);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se guardo la clase correctamente" + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalNueva();", true);
                    Session["listaClases"] = claseDatos.getClases();
                    cargarClases();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar el cupo" + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar una hora" + "');", true);
            }
        }

        protected void btnSieliminar_Click(object sender, EventArgs e)
        {
            claseDatos.eliminarClase(claseSeleccionada);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "La clase ha sido eliminada correctamente" + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEliminar();", true);
            Session["listaClases"] = claseDatos.getClases();
            cargarClases();
        }
        
        protected void btnSiEditar_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtEditarHora.Text))
            {
                if (!String.IsNullOrEmpty(txtEditarCupo.Text))
                {
                    String[] horaText = txtEditarHora.Text.Split(':');
                    int hora = Convert.ToInt32(horaText[0]);
                    int minutos = Convert.ToInt32(horaText[1]);
                    int cupo = Convert.ToInt32(txtEditarCupo.Text);

                    claseSeleccionada.cupo = cupo;
                    claseSeleccionada.hora = hora;
                    claseSeleccionada.minutos = minutos;
                    claseDatos.actualizarClase(claseSeleccionada);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se edito la clase correctamente" + "');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEditar();", true);
                    Session["listaClases"] = claseDatos.getClases();
                    cargarClases();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar el cupo" + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe de ingresar una hora" + "');", true);
            }
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idClase = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Clase> listaClases = (List<Clase>)Session["listaClases"];

            claseSeleccionada = (Clase)(listaClases.Where(clase => clase.idClase == idClase).ToList().First());

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEditar();", true);
            txtEditarCupo.Text = claseSeleccionada.cupo.ToString();
            txtEditarHora.Text = (claseSeleccionada.hora<10?"0"+claseSeleccionada.hora:claseSeleccionada.hora.ToString())+":"+(claseSeleccionada.minutos<10?"0"+claseSeleccionada.minutos:claseSeleccionada.minutos.ToString());
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idClase = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Clase> listaClases = (List<Clase>)Session["listaClases"];

            claseSeleccionada = (Clase)(listaClases.Where(clase => clase.idClase == idClase).ToList().First());

            lblEliminar.Text = "¿Esta seguro de eliminar la clase de " + (claseSeleccionada.hora<13?claseSeleccionada.hora:(claseSeleccionada.hora-12))+":"+claseSeleccionada.minutos+(claseSeleccionada.hora<13?" am":" pm") + " de forma permanente y toda su información asociada?";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEliminar();", true);
        }
        #endregion
    }
}