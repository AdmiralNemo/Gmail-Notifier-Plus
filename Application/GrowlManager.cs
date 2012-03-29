using System.Drawing.Imaging;
using System.IO;
using Growl.Connector;
using Growl.CoreLibrary;

namespace GmailNotifierPlus
{
    public static class GrowlManager {

        private enum MessageTypes
        {
            NEWMESSAGE
        }

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
                MessageTypes.NEWMESSAGE.ToString(),
                Localization.Locale.Current.Growl.NewMessage);

            Growl.Register(application, new NotificationType[] {
                new_message });

            Registered = true;
        }

        public static void Notify(Account account) {
            string title = Localization.Locale.Current.Growl.NewMessage;
            string message;

            if (account.Emails.Count < 0) {
                return;
            }
            else if (account.Emails.Count == 1) {
                Email email = account.Emails[0];
                message = string.Format(
                    Localization.Locale.Current.Growl.NewMessageFrom,
                    email.Title, email.From);
            }
            else {
                message = string.Format(
                    Localization.Locale.Current.Growl.HasNewMessages,
                    account.Name, account.Emails.Count);
            }
            Notification notification = new Notification(
                Resources.WindowTitle,
                MessageTypes.NEWMESSAGE.ToString(),
                null,
                title,
                message
            );
            Growl.Notify(notification);
        }
    }
}
