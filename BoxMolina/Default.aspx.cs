using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoxMolina
{
    public partial class Default : System.Web.UI.Page
    {
        #region variables globales
        AdministradorDatos administradorDatos = new AdministradorDatos();
        ClienteDatos clienteDatos = new ClienteDatos();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Master.FindControl("menu").Visible = false;
            if (!IsPostBack)
            {
                limpiarCampos();
            }
            
        }
        #endregion

        #region eventos
        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
            limpiarCampos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalSesion();", true);
        }

        protected void btnViewIngresar_Click(object sender, EventArgs e)
        {
            liRegistrarse.Attributes["class"] = "";
            ViewRegistrarse.Style["display"] = "none";

            liIngresar.Attributes["class"] = "active";
            ViewIngresar.Style["display"] = "block";
        }

        protected void btnViewRegistrarse_Click(object sender, EventArgs e)
        {
            liRegistrarse.Attributes["class"] = "active";
            ViewRegistrarse.Style["display"] = "block";

            liIngresar.Attributes["class"] = "";
            ViewIngresar.Style["display"] = "none";
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (ChckBxAdministrador.Checked)
            {
                Administrador administrador = new Administrador();
                administrador.contrasenna = txtPassword.Text;
                administrador.usuario = txtUsuario.Text;

                if (administradorDatos.login(administrador))
                {
                    Session["usuario"] = "Administrador";
                    Session["rol"] = 2;
                    String url = Page.ResolveUrl("~/Inicio.aspx");
                    Response.Redirect(url);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Datos incorrectos" + "');", true);
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(txtUsuario.Text) && !String.IsNullOrEmpty(txtPassword.Text))
                {
                    try
                    {
                        Cliente cliente = new Cliente();
                        cliente.cedula = Convert.ToInt32(txtUsuario.Text);
                        cliente.contrasenna = txtPassword.Text;

                        cliente = clienteDatos.login(cliente);
                        if (cliente.idCliente != 0)
                        {
                            if (!cliente.activo)
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El cliente ya no esta activo" + "');", true);
                            }
                            else
                            {
                                if (!cliente.confirmado)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "El cliente no ha sido confirmado por parte del administrador" + "');", true);
                                }
                                else
                                {
                                    Session["usuario"] = cliente.nombreCompleto;
                                    Session["rol"] = 3;
                                    String url = Page.ResolveUrl("~/Inicio.aspx");
                                    Response.Redirect(url);
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Datos incorrectos" + "');", true);
                        }
                    }
                    catch
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Datos incorrectos" + "');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Datos incorrectos" + "');", true);
                }
            }
        }


        protected void btnRegistrar_Click(object sender, EventArgs e)
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
                            if (String.IsNullOrEmpty(txtContrasenna1.Text))
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Favor ingresar la contraseña" + "');", true);
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(txtContrasenna2.Text))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Favor ingresar la confirmación de la contraseña" + "');", true);
                                }
                                else
                                {
                                    if (txtContrasenna1.Text.Equals(txtContrasenna2.Text))
                                    {
                                        Cliente cliente = new Cliente();
                                        cliente.activo = true;
                                        cliente.cedula = Convert.ToInt32(txtCedula.Text);
                                        cliente.confirmado = false;
                                        cliente.contrasenna = txtContrasenna1.Text;
                                        cliente.correo = txtCorreo.Text;
                                        cliente.nombreCompleto = txtNombreCompleto.Text;
                                        cliente.telefono = txtTelefono.Text;

                                        if (RadioButtonList1.SelectedValue.ToString().Equals("F"))
                                        {
                                            cliente.tipoClase = false;
                                        }
                                        else
                                        {
                                            if (RadioButtonList1.SelectedValue.ToString().Equals("T"))
                                            {
                                                cliente.tipoClase = true;
                                            }
                                        }

                                        clienteDatos.insertarCliente(cliente);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Los datos deben ser confirmados por el administrador, por favor esperar hasta que se realice esta acción" + "');", true);
                                        limpiarCampos();
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.warning('" + "La contraseña y la confirmación de la contraseña no coinciden" + "');", true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void limpiarCampos()
        {
            txtCedula.Text = "";
            txtContrasenna1.Text = "";
            txtContrasenna2.Text = "";
            txtCorreo.Text = "";
            txtNombreCompleto.Text = "";
            txtPassword.Text = "";
            txtTelefono.Text = "";
            txtUsuario.Text = "";
        }
        #endregion
    }
}