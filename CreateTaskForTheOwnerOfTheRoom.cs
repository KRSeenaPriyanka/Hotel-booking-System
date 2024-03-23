using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Microsoft.Xrm.Sdk;

namespace veriperkassignment
{
    public class CreateTaskForTheOwnerOfTheRoom : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                if (entity.LogicalName == "kopr_room")
                {
                    if (context.MessageName.ToLower() == "create" || context.MessageName.ToLower() == "update")
                    {
                        if (entity.Attributes.Contains("ownerid"))
                        {
                            EntityReference ownerRef = (EntityReference)entity.Attributes["ownerid"];

                            Entity task = new Entity("task");
                            task["subject"] = "Follow up with Customer for Checkout";
                            task["regardingobjectid"] = new EntityReference(entity.LogicalName, entity.Id);
                            task["ownerid"] = ownerRef;

                            if (entity.Attributes.Contains("kopr_plannedcheckoutdate") && entity.Attributes["kopr_plannedcheckoutdate"] is DateTime)
                            {
                                DateTime checkoutDate = (DateTime)entity.Attributes["kopr_plannedcheckoutdate"];
                                task["scheduledend"] = checkoutDate;
                            }

                            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                            service.Create(task);
                        }
                    }
                }
            }
        }
    }
}
