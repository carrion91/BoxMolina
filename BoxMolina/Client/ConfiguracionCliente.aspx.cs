using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoxMolina.Client
{
    public partial class ConfiguracionCliente : System.Web.UI.Page
    {
        #region variables globales
        public static Cliente clienteSeleccionado;
        ClienteDatos clienteDatos = new ClienteDatos();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            int[] rolesPermitidos = { 3 };
            Utilidades.escogerMenu(Page, rolesPermitidos);

            if (!IsPostBack)
            {
                int idCliente = (int)Session["idUsuario"];
                List<Cliente> listaClientes = clienteDatos.getClientes();
                clienteSeleccionado = (Cliente)(listaClientes.Where(cliente => cliente.idCliente == idCliente).ToList().First());

                ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalEditar();", true);
                txtNombreCompleto.Text = clienteSeleccionado.nombreCompleto;
                txtCedula.Text = clienteSeleccionado.cedula.ToString();
                txtCorreo.Text = clienteSeleccionado.correo;
                txtTelefono.Text = clienteSeleccionado.telefono;
                RadioButtonList1.SelectedValue = (clienteSeleccionado.tipoClase ? "T" : "F");
            }
        }
        #endregion

        #region logica
        #endregion

        #region eventos
        protected void btnActualizar_Click(object sender, EventArgs e)
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
                            if (String.IsNullOrEmpty(txtContrasenna0.Text))
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
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se han actualizado los datos del cliente exitosamente, excepto la contraseña" + "');", true);
                            }
                            else
                            {
                                if (txtContrasenna0.Text.Equals(clienteSeleccionado.contrasenna))
                                {
                                    if (txtContrasenna1.Text.Equals(txtContrasenna2.Text))
                                    {
                                        clienteSeleccionado.cedula = Convert.ToInt32(txtCedula.Text);
                                        clienteSeleccionado.correo = txtCorreo.Text;
                                        clienteSeleccionado.nombreCompleto = txtNombreCompleto.Text;
                                        clienteSeleccionado.telefono = txtTelefono.Text;
                                        clienteSeleccionado.contrasenna = txtContrasenna1.Text;

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
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "La nueva contraseña no coincide con la confirmación" + "');", true);
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "La contraseña actual no coincide" + "');", true);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

    }
}