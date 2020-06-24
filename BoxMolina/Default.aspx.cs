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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                for (int y = 0; y < 3; y++)
                {
                    System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
                    image.ID = "image" + y.ToString();
                    image.CssClass = "img-fluid";
                    image.Style["height"] = "200px";
                    image.Style["margin"] = "10px";
                    image.ImageUrl = "~/Imagenes/gym" + (y + 1) + ".jpg";

                    pnlImagenes.Controls.Add(image);
                }
            }
        }
    }
}