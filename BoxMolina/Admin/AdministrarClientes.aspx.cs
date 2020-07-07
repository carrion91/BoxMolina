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
    public partial class AdministrarClientes : System.Web.UI.Page
    {
        #region varibles globales
        ClienteDatos clienteDatos = new ClienteDatos();
        public static Cliente clienteSeleccionado;
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
                Session["listaClientes"] = clienteDatos.getClientes();
                cargarClientes();
            }
        }
        #endregion

        #region logica
        public void cargarClientes()
        {
            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];
            List<Cliente> listaFiltrada = (List<Cliente>)listaClientes.Where(cliente =>
            cliente.nombreCompleto.ToUpper().Contains(txtBuscarNombre.Text.ToUpper()) &&
            cliente.cedula.ToString().ToUpper().Contains(txtBuscarCedula.Text.ToUpper()) &&
            cliente.correo.ToUpper().Contains(txtBucarCorreo.Text.ToUpper()) && cliente.telefono.ToUpper().Contains(txtBuscarTelefono.Text.ToUpper())
            && (cliente.tipoClase ? "Taekwondo" : "Funcional").ToUpper().Contains(txtBuscarTipoClase.Text.ToUpper())).ToList();

            var dt = listaFiltrada;
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

            rpClientes.DataSource = pgsource;
            rpClientes.DataBind();

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
            cargarClientes();
        }

        protected void lbUltimo_Click(object sender, EventArgs e)
        {
            paginaActual = (Convert.ToInt32(ViewState["TotalPaginas"]) - 1);
            cargarClientes();
        }

        protected void lbAnterior_Click(object sender, EventArgs e)
        {
            paginaActual -= 1;
            cargarClientes();
        }

        protected void lbSiguiente_Click(object sender, EventArgs e)
        {
            paginaActual += 1;
            cargarClientes();
        }

        protected void rptPaginacion_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (!e.CommandName.Equals("nuevaPagina")) return;
            paginaActual = Convert.ToInt32(e.CommandArgument.ToString());
            cargarClientes();
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
            cargarClientes();
        }

        protected void bntActivar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];

            Cliente clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

            clienteSeleccionado.activo = true;

            clienteDatos.actualizarCliente(clienteSeleccionado);
            cargarClientes();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El cliente ha sido activado" + "');", true);
        }

        protected void btnDesactivar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];

            Cliente clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

            clienteSeleccionado.activo = false;

            clienteDatos.actualizarCliente(clienteSeleccionado);
            cargarClientes();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El cliente ha sido desactivado" + "');", true);
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];

            Cliente clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

            clienteSeleccionado.confirmado = true;

            clienteDatos.actualizarCliente(clienteSeleccionado);
            cargarClientes();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El cliente ha sido confirmado" + "');", true);
        }

        protected void btnDesconfirmar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];

            Cliente clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

            clienteSeleccionado.confirmado = false;

            clienteDatos.actualizarCliente(clienteSeleccionado);
            cargarClientes();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El cliente ha sido desconfirmado" + "');", true);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];

            clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEditar();", true);
            txtNombreCompleto.Text = clienteSeleccionado.nombreCompleto;
            txtCedula.Text = clienteSeleccionado.cedula.ToString();
            txtCorreo.Text = clienteSeleccionado.correo;
            txtTelefono.Text = clienteSeleccionado.telefono;
            RadioButtonList1.SelectedValue = (clienteSeleccionado.tipoClase ? "T" : "F");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int idCliente = Convert.ToInt32((((LinkButton)(sender)).CommandArgument).ToString());

            List<Cliente> listaClientes = (List<Cliente>)Session["listaClientes"];

            clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

            lblEliminar.Text = "¿Esta seguro de eliminar a " + clienteSeleccionado.nombreCompleto + " de forma permanente y toda su información asociada?";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEliminar();", true);
        }

        protected void btnSieliminar_Click(object sender, EventArgs e)
        {
            clienteDatos.eliminarCliente(clienteSeleccionado);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "El cliente ha sido eliminado correctamente" + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEliminar();", true);
            Session["listaClientes"] = clienteDatos.getClientes();
            cargarClientes();
        }

        protected void btnSiEditar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNombreCompleto.Text))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Favor ingresar el nombre completo" + "');", true);
            }
            else
            {
                if (String.IsNullOrEmpty(txtCedula.Text) || txtCedula.Text.Count() != 9)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Favor ingresar la cédula con el formato de 9 dígitos" + "');", true);
                }
                else
                {
                    if (String.IsNullOrEmpty(txtTelefono.Text))
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Favor ingresar el teléfono" + "');", true);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(txtCorreo.Text))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Favor ingresar el correo" + "');", true);
                        }
                        else
                        {
                            clienteSeleccionado.cedula = Convert.ToInt32(txtCedula.Text);
                            clienteSeleccionado.correo = txtCorreo.Text;
                            clienteSeleccionado.nombreCompleto = txtNombreCompleto.Text;
                            clienteSeleccionado.telefono = txtTelefono.Text;

                            if (RadioButtonList1.SelectedValue.ToString().Equals("F"))
                            {
                                clienteSeleccionado.tipoClase = false;
                            }
                            else
                            {
                                if (RadioButtonList1.SelectedValue.ToString().Equals("T"))
                                {
                                    clienteSeleccionado.tipoClase = true;
                                }
                            }

                            clienteDatos.actualizarCliente(clienteSeleccionado);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se han actualizado los datos del cliente exitosamente" + "');", true);
                            limpiarCampos();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalEditar();", true);
                            Session["listaClientes"] = clienteDatos.getClientes();
                            cargarClientes();
                        }
                    }
                }
            }
        }

        private void limpiarCampos()
        {
            txtCedula.Text = "";
            txtCorreo.Text = "";
            txtNombreCompleto.Text = "";
            txtTelefono.Text = "";
        }
        #endregion
    }
}