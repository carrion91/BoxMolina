using AccesoDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        Thread threadEnviarCorreo;//para enviar correos sin que quede en esperando el sistema a que termine de enviar todos
        public static Cliente clienteSeleccionado;
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
                    Server.Transfer("~/Inicio.aspx");
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
                                    Session["idUsuario"] = cliente.idCliente;
                                    Session["rol"] = 3;
                                    Server.Transfer("~/Inicio.aspx");
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

                                        String destinatario = cliente.correo;
                                        String mensaje = "<br/><h1>Se ha registrado en el sistema de Box Molina</h1><br/>¡Muchas gracias por su registro!</b><br/><span style='color:red'>*El administrador debe de confirmar su información para que pueda ingresar al sistema<br/>Se le enviara un correo cuando sea confirmado por el administrador.</span>";
                                        String asunto = "Box Molina Registro exitoso";
                                        String copiaOculta = "leo.carrion.23@gmail.com";

                                        threadEnviarCorreo = new Thread(delegate () { enviarCorreo(destinatario, mensaje, copiaOculta, asunto); });
                                        threadEnviarCorreo.Start();
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

        protected void btnOlvidoContrasenna_Click(object sender, EventArgs e)
        {
            limpiarCampos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "levantarModalOlvido();", true);
            btnAceptarCedula.Visible = true;
            divCorreo.Style.Add("display", "none");
            btnEnViar.Visible = false;
            txtCedulaOlvido.Text = "";
            txtCorreoOlvido.Text = "";
        }

        protected void btnAceptarCedula_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtCedulaOlvido.Text))
            {
                Cliente cliente = new Cliente();
                cliente.cedula = Convert.ToInt32(txtCedulaOlvido.Text);
                cliente = clienteDatos.getClientePorCedula(cliente);

                if (!String.IsNullOrEmpty(cliente.correo))
                {
                    btnAceptarCedula.Visible = false;
                    btnEnViar.Visible = true;
                    divCorreo.Style.Add("display", "block");
                    String[] finalCorreo = cliente.correo.Split('@');
                    txtCorreoOlvido.Text = cliente.correo.Substring(0,3)+"***"+finalCorreo[1];
                    clienteSeleccionado = cliente;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "No se encontro ninngún cliente registrado con esta cédula" + "');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.error('" + "Debe ingresar el número de cédula" + "');", true);
            }
        }

        protected void btnEnViar_Click(object sender, EventArgs e)
        {
            // Obtención de los correos de los usuarios a los que se les va a enviar el correo
            String contraseña = GenerarContraseña(8);
            String destinatario = clienteSeleccionado.correo;
            String mensaje = "<br/><h1>Se ha cambiado su contraseña para ingresar al sistema de Box Molina</h1><br/>Su nueva contraseña es: <b>"+contraseña+"</b><br/><span style='color:red'>*Luego de ingresar al sistema puede cambiar la contraseña por la que desee.</span>";
            String asunto = "Box Molina olvidó de contraseña";
            String copiaOculta = "";

            threadEnviarCorreo = new Thread(delegate () { enviarCorreo(destinatario,mensaje,copiaOculta,asunto); });
            threadEnviarCorreo.Start();

            clienteSeleccionado.contrasenna = contraseña;
            clienteDatos.actualizarCliente(clienteSeleccionado);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "activar", "cerrarModalOlvido();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "toastr.success('" + "Se envio correctamente el correo con la nueva contraseña." + "');", true);
        }

        public void enviarCorreo(String destinatario,String mensaje, String copiaOculta,String asunto)
        {
            #region Envío de correos

            // Cuerpo del correo
            String cuerpoCorreo = "<div style='width: 100 %; background - color:black'><img width='90' height='90' src='https://scontent.fsjo6-1.fna.fbcdn.net/v/t1.0-9/53909715_373770576548472_2873678814351720448_o.jpg?_nc_cat=101&_nc_sid=09cbfe&_nc_ohc=mnFhZYoTLnoAX-as-M7&_nc_ht=scontent.fsjo6-1.fna&oh=ea4c2e5b14efe889055e1bf52e1e1967&oe=5F2C2F02' /></div>"+mensaje;

            Dictionary<String, String> informacionCorreo = new Dictionary<String, String>();
            informacionCorreo["destinatarios"] = destinatario;
            informacionCorreo["conCopia"] = "";
            informacionCorreo["conCopiaOculta"] = copiaOculta;
            informacionCorreo["asunto"] = asunto;
            informacionCorreo["cuerpo"] = cuerpoCorreo;
            informacionCorreo["remitente"] = "consejotecnico2016@gmail.com";
            informacionCorreo["archivos"] = "";

            Boolean enviado = Utilidades.enviarCorreo(informacionCorreo);

            #endregion

            try
            {
                threadEnviarCorreo.Abort();
            }
            catch
            {
            }
        }


        public string GenerarContraseña(int longitud)
        {
            string contraseña = string.Empty;
            string[] letras = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            Random EleccionAleatoria = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int LetraAleatoria = EleccionAleatoria.Next(0, 100);
                int NumeroAleatorio = EleccionAleatoria.Next(0, 9);

                if (LetraAleatoria < letras.Length)
                {
                    contraseña += letras[LetraAleatoria];
                }
                else
                {
                    contraseña += NumeroAleatorio.ToString();
                }
            }
            return contraseña;
        }
        #endregion

    }
}