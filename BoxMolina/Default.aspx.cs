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
        #endregion

        #region page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
            }
        }
        #endregion

        #region eventos
        protected void btnIniciarSesion_Click(object sender, EventArgs e)
        {
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Datos correctos" + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Datos incorrectos" + "');", true);
                }
            }
        }
        #endregion


    }
}