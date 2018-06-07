using System;
using System.Net;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using TaskAutomator.Core;
using WindowsCredential = Microsoft.VisualStudio.Services.Common.WindowsCredential;

namespace TaskAutomator.Tfs2015
{
    public class Tfs2015Service : ITaskService
    {
        private const int PingTaskId = 92757;

        public Uri Address { get; private set; }
        private readonly ICredentials _credentials;

        public Tfs2015Service(Uri address, ICredentials credentials)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
        }

        public string Ping()
        {
            var client = WorkItemTrackingHttpClient;
            try
            {
                var result = client.GetWorkItemAsync(PingTaskId).Result;
                return string.Empty;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.GetBaseException().Message;
            }
        }

        private WorkItemTrackingHttpClient WorkItemTrackingHttpClient
        {
            get
            {
                var credentials = new VssCredentials(new WindowsCredential(_credentials));
                var settings = new VssHttpRequestSettings {BypassProxyOnLocal = true};

                var client = new WorkItemTrackingHttpClient(Address, credentials, settings);
                return client;
            }
        }

        public ActionResult<Task> GetTask(string id)
        {
            try
            {
                var client = WorkItemTrackingHttpClient;
                var res = client.GetWorkItemAsync(int.Parse(id)).Result;
                var name = res.Fields["System.Title"];

                var description = string.Empty;
                if (res.Fields.ContainsKey("System.Description"))
                    description = (string)res.Fields["System.Description"];
                else
                {
                    if (res.Fields.ContainsKey("Microsoft.VSTS.TCM.ReproSteps"))
                        description = (string)res.Fields["Microsoft.VSTS.TCM.ReproSteps"];
                }

                var task = new Task
                {
                    Id = id,
                    Name = (string)name,
                    Description = description,
                    Link = new Uri(res.Url)
                };
                return ActionResult<Task>.Success(task);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ActionResult<Task>.Fail(e);
            }
        }

        public ActionResult<bool> UpdateTask(Task task)
        {
            try
            {
                var client = WorkItemTrackingHttpClient;
                var document = new JsonPatchDocument
                {
                    new JsonPatchOperation { Path = "/fields/System.Description", Operation = Operation.Replace, Value = task.Description }
                };
                var result = client.UpdateWorkItemAsync(document, int.Parse(task.Id)).Result;
                return ActionResult<bool>.Success(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ActionResult<bool>.Fail(e);
            }
        }
    }
}
