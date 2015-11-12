using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;

namespace Tfs2015SetPicture
{
    internal static class TfsUploader
    {
        public static void UploadImage(string serverName, string username, byte[] imageBytes)
        {
            var serverUri = GetServerUri(serverName);
            var configurationServer = TfsConfigurationServerFactory.GetConfigurationServer(serverUri);
            configurationServer.EnsureAuthenticated();
            var idService = configurationServer.GetService<IIdentityManagementService2>();
            var identity = idService.ReadIdentity(IdentitySearchFactor.AccountName, username, MembershipQuery.Direct, ReadIdentityOptions.None);

            try
            {
                identity.SetProperty("Microsoft.TeamFoundation.Identity.Image.Data", imageBytes);
                identity.SetProperty("Microsoft.TeamFoundation.Identity.Image.Type", "image/png");
                identity.SetProperty("Microsoft.TeamFoundation.Identity.Image.Id", Guid.NewGuid().ToByteArray());
                identity.SetProperty("Microsoft.TeamFoundation.Identity.CandidateImage.Data", null);
                identity.SetProperty("Microsoft.TeamFoundation.Identity.CandidateImage.UploadDate", null);

                idService.UpdateExtendedProperties(identity);
            }
            catch (NullReferenceException)
            {
                Console.Error.WriteLine("Failed to set image on user. Make sure you are authorized to update the image of this user.");
                throw;
            }
        }

        private static Uri GetServerUri(string serverName)
        {
            if (serverName.StartsWith("http://") || serverName.StartsWith("https://"))
            {
                return new Uri(serverName);
            }
            return new Uri($"http://{serverName}:8080/tfs/");
        }
    }
}