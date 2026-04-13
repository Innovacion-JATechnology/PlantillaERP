

using System;
using System.Web;
using System.Web.UI;

public abstract class AgentOnlyPage : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        Response.Cache.SetProxyMaxAge(TimeSpan.Zero);
        Response.Cache.SetValidUntilExpires(false);
        Response.Cache.SetNoServerCaching();

        // Optional: extra headers for some proxies
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";

        var role = Context?.Session?["role"] as string;
        if (!string.Equals(role, "agente", StringComparison.OrdinalIgnoreCase))
        {
            Response.Redirect("~/AccesoRestringido.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

    }

    protected void ShowClientMessage(string message)
    {
        // Safer than Response.Write
        var safe = HttpUtility.JavaScriptStringEncode(message);
        ClientScript.RegisterStartupScript(
            GetType(),
            "alert",
            $"alert('{safe}');",
            addScriptTags: true);
    }

    protected void ShowClientMessageAndRedirect(string message, string url)
    {
        var safe = HttpUtility.JavaScriptStringEncode(message);
        var script = $"alert('{safe}'); window.location='{HttpUtility.JavaScriptStringEncode(url)}';";

        // If you use UpdatePanel/ScriptManager, prefer ScriptManager.RegisterStartupScript
        if (ScriptManager.GetCurrent(this.Page) != null)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertAndRedirect", script, addScriptTags: true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alertAndRedirect", script, addScriptTags: true);
        }
    }


}

public abstract class UsuarioOnlyPage : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        var role = Context?.Session?["role"] as string;
        if (!string.Equals(role, "usuario", StringComparison.OrdinalIgnoreCase))
        {
            Response.Redirect("~/AccesoRestringido.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }

    protected void ShowClientMessage(string message)
    {
        // Safer than Response.Write
        var safe = HttpUtility.JavaScriptStringEncode(message);
        ClientScript.RegisterStartupScript(
            GetType(),
            "alert",
            $"alert('{safe}');",
            addScriptTags: true);
    }

    protected void ShowClientMessageAndRedirect(string message, string url)
    {
        var safe = HttpUtility.JavaScriptStringEncode(message);
        var script = $"alert('{safe}'); window.location='{HttpUtility.JavaScriptStringEncode(url)}';";

        // If you use UpdatePanel/ScriptManager, prefer ScriptManager.RegisterStartupScript
        if (ScriptManager.GetCurrent(this.Page) != null)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertAndRedirect", script, addScriptTags: true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alertAndRedirect", script, addScriptTags: true);
        }
    }
}
