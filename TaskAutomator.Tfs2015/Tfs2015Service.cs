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

        private readonly Uri _address;
        private readonly ICredentials _credentials;

        public Tfs2015Service(Uri address, ICredentials credentials)
        {
            _address = address;
            _credentials = credentials;
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

                var client = new WorkItemTrackingHttpClient(_address, credentials, settings);
                return client;
            }
        }

        public Task GetTask(string id)
        {
            var client = WorkItemTrackingHttpClient;
            var res = client.GetWorkItemAsync(int.Parse(id)).Result;
            return new Task
            {
                Id = res.Id.GetValueOrDefault().ToString(),
                Description = (string)res.Fields["System.Description"]
            };
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
