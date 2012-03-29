using System.Drawing.Imaging;
using System.IO;
using Growl.Connector;
using Growl.CoreLibrary;

namespace GmailNotifierPlus
{
    public static class GrowlManager {

        private static GrowlConnector Growl = GetGrowl();
        private static bool Registered = false;

        private static GrowlConnector GetGrowl() {
            return new GrowlConnector();
        }
        
        public static void Register() {
            if (Registered) return;

            Application application = new Application(Resources.WindowTitle);
            using (MemoryStream s = new MemoryStream()) {
                Utilities.ResourceHelper.GetImage("about-icon.png"
                    ).Save(s, ImageFormat.Png);
                application.Icon = new BinaryData(s.ToArray());
            }

            NotificationType new_message = new NotificationType(
                "NEWMESSAGE", Localization.Locale.Current.Growl.NewMessage);

            Growl.Register(application, new NotificationType[] {
                new_message });

            Registered = true;
        }
    }
}
