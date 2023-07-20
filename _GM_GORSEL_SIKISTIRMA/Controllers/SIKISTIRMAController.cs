using nQuant;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _GM_GORSEL_SIKISTIRMA.Controllers
{
    public class SIKISTIRMAController : Controller
    {
        public int RastgeleDeger;
        public string yol;
        [HttpGet]
        public ActionResult Index()
        {

            var suan = DateTime.Now;
            Random r = new Random();
            RastgeleDeger = r.Next(0, 100);
            Session["yol"] = "~/Uploads/" + suan.ToString().Replace(" ", "_").Replace(".", "_").Replace(":", "_").ToString() + "_" + RastgeleDeger.ToString() + "";
            return View();
        }

        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> fileData)
        {
            yol = Session["yol"].ToString();
            var path = "";
            if (Server.MapPath(yol).Count() > 0)
            {
                Directory.CreateDirectory(Server.MapPath(yol + "/"));
                path = Server.MapPath(yol);
            }
            else
            {
                path = Server.MapPath(yol);
            }


            foreach (HttpPostedFileBase postedFile in fileData)
            {
                if (postedFile != null)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);

                    path = Path.Combine(Session["yol"].ToString());//Path.Combine(Server.MapPath(yol), fileName);
                    postedFile.SaveAs(Server.MapPath(path + "/" + fileName));
                    Session["yol"] = yol;
                    sikistir(fileName);
                }
            }
            return Content("Success");
        }


        public void sikistir(string filename)
        {
            yol = Session["yol"].ToString() + "/" + filename;
            Bitmap bmp1 = new Bitmap(Server.MapPath(yol));
            try
            {
                var quantizer = new WuQuantizer();
                using (var quantized = quantizer.QuantizeImage(bmp1))
                {
                    quantized.Save(yol + filename, ImageFormat.Png);
                }
            }
            catch { }
        }
    }
}