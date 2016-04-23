﻿using agsXMPP.Xml.Dom;
using System.Web.Script.Serialization;

namespace HarmonyHub
{
	internal class HarmonyDocuments
    {
	    private const string Namespace = "connect.logitech.com";
        public class HarmonyDocument : Document
        {
            public HarmonyDocument()
            {
                Namespace = Namespace;
            }
        }

	    private static Element CreateOaElement(string command)
	    {
			var element = new Element("oa");
			element.Attributes.Add("xmlns", Namespace);
			element.Attributes.Add("mime", $"vnd.logitech.harmony/vnd.logitech.harmony.engine?{command}");
		    return element;
	    }

		public static HarmonyDocument StartActivityDocument(string activityId)
        {
            var document = new HarmonyDocument();

			var element = CreateOaElement("startactivity");
            element.Value = $"activityId={activityId}:timestamp=0";
            document.AddChild(element);
            return document;
        }

        public static HarmonyDocument GetCurrentActivityDocument()
        {
            var document = new HarmonyDocument();
            document.AddChild(CreateOaElement("getCurrentActivity"));
            return document;
        }

        public static HarmonyDocument IrCommandDocument(string deviceId, string command)
        {
            var document = new HarmonyDocument();

            var action = new HarmonyAction { type = "IRCommand", deviceId = deviceId, command = command };
            var json = new JavaScriptSerializer().Serialize(action);

            // At this point our valid json won't work - we need to break it so it looks like:
            // {"type"::"IRCommand","deviceId"::"deviceId","command"::"command"}
            // note double colons 

            json = json.Replace(":", "::");
			var element = CreateOaElement("holdAction");

			element.Value = $"action={json}:status=press";

            document.AddChild(element);

            return document;
        }

        public static HarmonyDocument ConfigDocument()
        {
            var document = new HarmonyDocument();
            document.AddChild(CreateOaElement("config"));
            return document;
        }

        public static HarmonyDocument LogitechPairDocument(string token)
        {
            var document = new HarmonyDocument();

            var element = new Element("oa");
            element.Attributes.Add("xmlns", "connect.logitech.com");
            element.Attributes.Add("mime", "vnd.logitech.connect/vnd.logitech.pair");
            element.Value = $"token={token}:name=foo#iOS6.0.1#iPhone";
            document.AddChild(element);
            return document;
        }
    }
}
