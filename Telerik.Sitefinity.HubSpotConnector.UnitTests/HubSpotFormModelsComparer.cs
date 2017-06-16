using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.HubSpotConnector.Model;

namespace Telerik.Sitefinity.Test.Unit.HubSpot
{
    internal static class HubSpotFormModelsComparer
    {
        public static bool AreEqual(IEnumerable<HubSpotForm> first, IEnumerable<HubSpotForm> second)
        {
            if (first == second)
            {
                return true;
            }

            HubSpotForm[] firstArray = first.OrderBy(f => f.Guid).ToArray();
            HubSpotForm[] secondArray = second.OrderBy(f => f.Guid).ToArray();

            bool areEqlual = true;

            for (int i = 0; i < first.Count(); i++)
            {
                areEqlual = areEqlual && HubSpotFormModelsComparer.AreEqual(firstArray[i], secondArray[i]);
            }

            return areEqlual;
        }

        public static bool AreEqual(HubSpotForm first, HubSpotForm second)
        {
            bool areEqlual = first.Guid == second.Guid;
            areEqlual = areEqlual && first.Name == second.Name;
            areEqlual = areEqlual && first.PortalId == second.PortalId;

            return areEqlual;
        }
    }
}