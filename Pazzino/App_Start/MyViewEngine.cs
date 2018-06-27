using System.Web.Mvc;

public class MyViewEngine : RazorViewEngine
{
    public MyViewEngine()
        : base()
    {
        ViewLocationFormats = new[] {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/{1}/{0}.vbhtml",
        "~/Views/Shared/{0}.cshtml",
        "~/Views/Shared/{0}.vbhtml",
        "~/Views/Job/{0}.cshtml",
        "~/Views/Company/{0}.cshtml",
        "~/Views/Home/{0}.cshtml",
        "~/Views/Resume/{0}.cshtml"

    };

        PartialViewLocationFormats = new[] {
        "~/Views/{1}/{0}.cshtml",
        "~/Views/{1}/{0}.vbhtml",
        "~/Views/Shared/{0}.cshtml",
        "~/Views/Shared/{0}.vbhtml",
        "~/Views/Job/{0}.cshtml",
        "~/Views/Company/{0}.cshtml",
        "~/Views/Home/{0}.cshtml",
        "~/Views/Resume/{0}.cshtml"

    };
    }


}