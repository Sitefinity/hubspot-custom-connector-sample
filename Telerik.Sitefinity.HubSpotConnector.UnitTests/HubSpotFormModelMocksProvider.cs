using System;
using System.Collections.Generic;
using Telerik.Sitefinity.HubSpotConnector.Model;

namespace Telerik.Sitefinity.Test.Unit.HubSpot
{
    internal static class HubSpotFormModelMocksProvider
    {
        public static IEnumerable<HubSpotForm> CreateMockedFormsCollection(int count, int fieldGroupsCount, int fieldsCount)
        {
            IList<HubSpotForm> forms = new List<HubSpotForm>();

            for (int i = 0; i < count; i++)
            {
                HubSpotForm currentForm = HubSpotFormModelMocksProvider.CreateMockedForm(fieldGroupsCount, fieldsCount);
                forms.Add(currentForm);
            }

            return forms;
        }

        public static HubSpotForm CreateMockedForm(int fieldGroupsCount, int fieldsCount)
        {
            HubSpotForm hubSpotForm = new HubSpotForm();
            hubSpotForm.Guid = Guid.NewGuid();
            hubSpotForm.Name = Guid.NewGuid().ToString();
            hubSpotForm.PortalId = Guid.NewGuid().ToString();
            hubSpotForm.FormFieldGroups = HubSpotFormModelMocksProvider.CreateMockedHubSpotFormFieldGroups(fieldGroupsCount, fieldsCount);

            return hubSpotForm;
        }

        public static IEnumerable<HubSpotFormFieldGroup> CreateMockedHubSpotFormFieldGroups(int count, int fieldsCount)
        {
            IList<HubSpotFormFieldGroup> hubSpotFormElements = new List<HubSpotFormFieldGroup>();

            for (int i = 0; i < count; i++)
            {
                HubSpotFormFieldGroup currentHubSpotFormElement
                    = HubSpotFormModelMocksProvider.CreateMockedHubSpotFormFieldGroup(fieldsCount);

                hubSpotFormElements.Add(currentHubSpotFormElement);
            }

            return hubSpotFormElements;
        }

        public static HubSpotFormFieldGroup CreateMockedHubSpotFormFieldGroup(int fieldsCount)
        {
            HubSpotFormFieldGroup hubSpotFormFieldGroup = new HubSpotFormFieldGroup();
            hubSpotFormFieldGroup.Fields = HubSpotFormModelMocksProvider.CreateMockedHubSpotFormFields(fieldsCount);

            return hubSpotFormFieldGroup;
        }

        public static IEnumerable<HubSpotFormField> CreateMockedHubSpotFormFields(int count)
        {
            IList<HubSpotFormField> hubSpotFormFields = new List<HubSpotFormField>();

            for (int i = 0; i < count; i++)
            {
                HubSpotFormField hubSpotFormField = HubSpotFormModelMocksProvider.CreateMockedHubSpotFormField();
                hubSpotFormFields.Add(hubSpotFormField);
            }

            return hubSpotFormFields;
        }

        public static HubSpotFormField CreateMockedHubSpotFormField()
        {
            HubSpotFormField hubSpotFormField = new HubSpotFormField()
            {
                Name = Guid.NewGuid().ToString()
            };

            return hubSpotFormField;
        }
    }
}