using System;
using System.Collections.Generic;
using System.Linq;
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
                    String url = page.ResolveUrl("~/Default.aspx");
                    page.Response.Redirect(url);
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
                    String url = page.ResolveUrl("~/Default.aspx");
                    page.Response.Redirect(url);
                }
            }
            else
            {
                page.Session.RemoveAll();
                page.Session.Abandon();
                page.Session.Clear();
                String url = page.ResolveUrl("~/Default.aspx");
                page.Response.Redirect(url);
            }
        }
    }
}