using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BoxMolina
{
    public partial class Site : System.Web.UI.MasterPage
    {
        #region variables globales
        UsuarioServicios usuarioServicios = new UsuarioServicios();
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region logica
        public Boolean validar()
        {
            Boolean validar = true;

            txtUsuario.CssClass = "form-control is-valid";
            txtContrasena.CssClass = "form-control is-valid";

            String usuario = txtUsuario.Text;
            if (String.IsNullOrEmpty(usuario))
            {
                txtUsuario.CssClass = "form-control is-invalid";
                validar = false;
            }

            String contrasena = txtContrasena.Text;

            if (String.IsNullOrEmpty(contrasena))
            {
                txtContrasena.CssClass = "form-control is-invalid";
                validar = false;
            }

            return validar;
        }
        #endregion

        #region eventos

        /// <summary>
        /// Leonardo Carrion
        /// 29/ago/2018
        /// Efecto: valida los datos ingresados por el usuario y muestra el menu de administrador
        /// Requiere: ingresar datos Usuario y Contraseña
        /// Modifica: menu
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnInicioSesion_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                Usuario usuario = new Usuario();
                usuario.usuario = txtUsuario.Text;
                usuario.contrasena = txtContrasena.Text;

                if (usuarioServicios.login(usuario))
                {
                    usuario = usuarioServicios.getUsuarioPorUsuario(usuario);
                    nombreUsuario.Text = usuario.nombreCompleto;
                    txtUsuario.Text = "";
                    txtContrasena.Text = "";
                    btnUsuario.Visible = false;
                    btnCerrarSesion.Visible = true;
                    divUsuarioIncorrecto.Visible = false;
                }
                else
                {
                    txtUsuario.CssClass = "form-control is-invalid";
                    txtContrasena.CssClass = "form-control is-invalid";
                    divUsuarioIncorrecto.Visible = true;
                    btnUsuario.Visible = true;
                    btnCerrarSesion.Visible = false;

                    string message = "$('#modalInicioSesion').modal({ backdrop: 'static', keyboard: false, show: true });";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", message, true);
                }
            }
            else
            {
                string message = "$('#modalInicioSesion').modal({ backdrop: 'static', keyboard: false, show: true });";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", message, true);
            }
        }

        /// <summary>
        /// Leonardo Carrion
        /// 22/set/2018
        /// Efecto: cierra la sesion activa
        /// Requiere: dar clic al boton de "cerrar sesion"
        /// Modifica: sesion activa
        /// Devuelve: -
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            divUsuarioIncorrecto.Visible = false;
            btnUsuario.Visible = true;
            btnCerrarSesion.Visible = false;
        }

        #endregion
    }
}