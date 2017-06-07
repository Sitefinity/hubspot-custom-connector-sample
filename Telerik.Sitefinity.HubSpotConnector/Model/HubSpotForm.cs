using System;
using System.Collections.Generic;

namespace Telerik.Sitefinity.HubSpotConnector.Model
{
    internal class HubSpotForm
    {
        /// <summary>
        /// Gets or sets the form Portal ID.
        /// </summary>
        public string PortalId { get; set; }

        /// <summary>
        /// Gets or set the form Guid.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the form name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of form field groups.
        /// </summary>
        public IEnumerable<HubSpotFormFieldGroup> FormFieldGroups { get; set; }
    }
}