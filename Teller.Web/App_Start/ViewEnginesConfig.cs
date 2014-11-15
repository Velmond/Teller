namespace Teller.Web.App_Start
{
    using System.Web.Mvc;

    public class ViewEnginesConfig
    {
        public static void RegisterViewEngines()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}
