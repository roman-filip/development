using System;
using Microsoft.Exchange.WebServices.Data;
using TestExchange.Properties;

namespace TestExchange
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ExchangeService(ExchangeVersion.Exchange2010);

            // Setting credentials is unnecessary when you connect from a computer that is logged on to the domain.
            service.Credentials = new WebCredentials(Settings.Default.username, Settings.Default.password, Settings.Default.domain);
            // Or use NetworkCredential directly (WebCredentials is a wrapper around NetworkCredential).

            //To set the URL manually, use the following:
            //service.Url = new Uri("https://proliant32.hci.tetra.ppf.cz/EWS/Exchange.asmx");

            //To set the URL by using Autodiscover, use the following:
            service.AutodiscoverUrl("roman.filip@XXXXXXXXXXX.cz");

            FindItemsResults<Item> findResults = service.FindItems(WellKnownFolderName.Inbox, new ItemView(10));
            foreach (Item item in findResults.Items)
            {
                Console.WriteLine(item.Subject);
            }
        }
    }
}
