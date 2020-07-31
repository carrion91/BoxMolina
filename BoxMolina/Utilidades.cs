using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

namespace BoxMolina
{
    internal static class Utilidades
    {
        public static void escogerMenu(Page page, int[] rolesPermitidos)
        {
            int rol = page.Session["rol"] == null ? 0 : Int32.Parse(page.Session["rol"].ToString());//Int32.Parse(page.Session["rol"].ToString());
            if (page.Session["usuario"] == null)
            {
                page.Session.RemoveAll();
                page.Session.Abandon();
                page.Session.Clear();
                String url = page.ResolveUrl("~/Default.aspx");
                page.Response.Redirect(url);
            }

            /*
             * Rol..................idRol
             * Administrador.........2
             * Cliente...............3
             */
            else if (rol == 2)
            {
                if (rolesPermitidos.Contains(rol))
                {
                    page.Master.FindControl("MenuAdministrador").Visible = true;
                    page.Master.FindControl("MenuCliente").Visible = false;
                }
                else
                {
                    page.Session.RemoveAll();
                    page.Session.Abandon();
                    page.Session.Clear();
                    page.Server.Transfer("~/Default.aspx");
                }

            }
            else if (rol == 3)
            {
                if (rolesPermitidos.Contains(rol))
                {
                    page.Master.FindControl("MenuAdministrador").Visible = false;
                    page.Master.FindControl("MenuCliente").Visible = true;
                }
                else
                {
                    page.Session.RemoveAll();
                    page.Session.Abandon();
                    page.Session.Clear();
                    page.Server.Transfer("~/Default.aspx");
                }
            }
            else
            {
                page.Session.RemoveAll();
                page.Session.Abandon();
                page.Session.Clear();
                page.Server.Transfer("~/Default.aspx");
            }
        }

        public static Boolean enviarCorreo(Dictionary<String, String> informacionCorreo)
        {
            // Código obtenido en http://oscarsotorrio.com/post/2011/01/22/Envio-de-correo-en-NET-con-CSharp.aspx

            /*-------------------------MENSAJE DE CORREO ----------------------*/

            //Creamos un nuevo Objeto de mensaje
            System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();

            // Obtenemos las direcciones de correo de los destinatarios desde el diccionario informaciónCorreo
            String destinatarios = informacionCorreo["destinatarios"];
            String[] listaDestinatarios = destinatarios.Split(';');

            foreach (String destinatario in listaDestinatarios)
            {
                try
                {
                    //Direccion de correo electronico a la que queremos enviar el mensaje
                    if (destinatario.Trim() != "")
                        mmsg.To.Add(destinatario); //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario
                }
                catch { }
            }

            // Asunto
            mmsg.Subject = informacionCorreo["asunto"];
            mmsg.SubjectEncoding = System.Text.Encoding.UTF8;

            String conCopia = informacionCorreo["conCopia"];
            String[] listaConCopia = conCopia.Split(';');
            if (conCopia.Trim() != "")
            {
                //Direccion de correo electronico que queremos que reciba una copia del mensaje
                //envia una copia del correo
                //mmsg.Bcc.Add(conCopia);

                //adjunta como copia a "X"
                foreach (String conCopiaA in listaConCopia)
                {
                    try
                    {
                        if (conCopiaA.Trim() != "")
                        {
                            mmsg.CC.Add(conCopiaA);
                        }
                    }
                    catch { }
                }
            }

            String conCopiaOculta = informacionCorreo["conCopiaOculta"];
            String[] listaConCopiaOculta = conCopiaOculta.Split(';');
            if (conCopiaOculta.Trim() != "")
            {
                foreach (String conCopiaOcultaA in listaConCopiaOculta)
                {
                    try
                    {
                        //Direccion de correo electronico que queremos que reciba una copia del mensaje oculto
                        //envia una copia del correo
                        if (conCopiaOcultaA.Trim() != "")
                        {
                            mmsg.Bcc.Add(conCopiaOcultaA);
                        }
                    }
                    catch { }
                }
            }

            //Cuerpo del Mensaje
            mmsg.Body = informacionCorreo["cuerpo"];
            mmsg.BodyEncoding = System.Text.Encoding.UTF8;
            mmsg.IsBodyHtml = true;

            //Correo electronico desde la que enviamos el mensaje
            String remitente = informacionCorreo["remitente"].Split(';')[0];
            mmsg.From = new System.Net.Mail.MailAddress(remitente);

            // Prioridad del mensaje
            mmsg.Priority = MailPriority.High;

            // Si queremos enviar un archivo adjunto
            // mmsg.Attachments.Add(new Attachment(@"G:\LANAMME\Viaticos locales\24022014_Reporte viaticos.pdf"));

            String rutasArchivos = informacionCorreo["archivos"];
            String[] listaRutasArchivos = rutasArchivos.Split(';');

            foreach (String rutaArchivo in listaRutasArchivos)
            {
                //Direccion de correo electronico a la que queremos enviar el mensaje
                if (rutaArchivo.Trim() != "")
                    mmsg.Attachments.Add(new Attachment(rutaArchivo));
            }

            /*---------------------- FIN MENSAJE DE CORREO --------------------*/

            /*-------------------------CLIENTE DE CORREO ----------------------*/

            //Creamos un objeto de cliente de correo
            System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();

            //Hay que crear las credenciales del correo emisor
            cliente.Credentials = new System.Net.NetworkCredential("consejotecnico2016@gmail.com", "lanamme2016");
            //cliente.Credentials = new System.Net.NetworkCredential("laboratorios.lanamme@ucr.ac.cr", "14bs.l4n4mm3");

            //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
            cliente.Port = 587;
            cliente.EnableSsl = true;

            cliente.Host = "smtp.gmail.com"; //Para Gmail "smtp.gmail.com";
            //cliente.Host = "smtp.ucr.ac.cr"; //Para UCR "smtp.ucr.ac.cr";

            /*----------------------FIN CLIENTE DE CORREO -------------------*/

            /*-------------------------ENVIO DE CORREO ----------------------*/

            Boolean enviado = true;

            try
            {
                //Enviamos el mensaje      
                cliente.Send(mmsg);
                mmsg.Dispose();
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                //Aquí gestionamos los errores al intentar enviar el correo
                enviado = false;
            }

            /*---------------------- FIN ENVIO DE CORREO --------------------*/

            return enviado;
        }
    }
}