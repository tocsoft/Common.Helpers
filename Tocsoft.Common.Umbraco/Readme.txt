Add @using Tocsoft.Common.Umbraco to the top of your razor files then you can use macros from inside other razor files.

You can us the @Library.RenderMacro("alias", new { property1 = "value",  property2 = "value2", }) to render a macro from inside another razor file.

This should also be compatible with UmbraMVCo as long as you use @inherit UmbraMVCo.RenderViewPage to get access to the RazorLibrary helpers.
