using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TrackApartmentsApp.Core.Settings;
using TrackApartmentsApp.Data;
using TrackApartmentsApp.Data.PageParsers.Onliner;
using TrackApartmentsApp.Data.Storage;
using TrackApartmentsApp.Data.Storage.Abstract;
using TrackApartmentsApp.Domain.Conditions;
using TrackApartmentsApp.Domain.Connectors.OnlinerConnector;
using TrackApartmentsApp.Domain.Models;
using TrackApartmentsApp.Domain.Secrets;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace TrackApartmentsApp
{
    public static class TrackApartmentFunction
    {
        [FunctionName("TrackApartmentFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            try
            {
                await StartAsync(log, context);
            }
            catch (Exception ex)
            {
                var message = $"Function failed: {ex.Message}, with data {ex.Data}.";

                if (ex.InnerException != null)
                {
                    message += $"Inner Exception: {ex.InnerException}";
                }

                log.LogError(message, ex);

                throw;
            }
        }

        private static async Task StartAsync(ILogger log, ExecutionContext context)
        {
            GetSettings(context, out var storageSettings, out var appSettings, out var onlinerSettings, out var twilioSettings);
            await LoadSecretSettings(storageSettings, appSettings, twilioSettings);

            var engine = new LoadEngine();
            var parser = new ResponseParser();
            var pageParser = new OnlinerPageParser();
            var connector = new OnlinerConnector(engine, parser, pageParser);

            var results = GetOnlinerApartments(log, onlinerSettings, connector);

            var worker = new StorageWorker(storageSettings);
            var readRepository = new StorageAppartmentReadRepository(worker);
            var writeRepository = new StorageAppartmentWriteRepository(worker);
            var storageConnector = new OnlinerStorageConnector(storageSettings, writeRepository, readRepository);

            var items = await GetNewlyCreatedItemsAsync(log, storageConnector, results);
            SendSmsMessages(items, twilioSettings);
        }

        private static async Task LoadSecretSettings(StorageSettings storageSettings, AppSettings appSettings, TwilioSettings twilioSettings)
        {
            var store = new SecretsStore(appSettings);

            storageSettings.ConnectionString = await store.GetOrLoadSettingAsync(storageSettings.ConnectionString);
            twilioSettings.AccountSid = await store.GetOrLoadSettingAsync(twilioSettings.AccountSid);
            twilioSettings.AuthToken = await store.GetOrLoadSettingAsync(twilioSettings.AuthToken);
            twilioSettings.PhoneNumberFrom = await store.GetOrLoadSettingAsync(twilioSettings.PhoneNumberFrom);
            twilioSettings.PhoneNumberTo = await store.GetOrLoadSettingAsync(twilioSettings.PhoneNumberTo);
        }

        private static void GetSettings(
            ExecutionContext context,
            out StorageSettings storageSettings,
            out AppSettings appSettings,
            out OnlinerSettings onlinerSettings,
            out TwilioSettings twilioSettings)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            storageSettings = new StorageSettings();
            appSettings = new AppSettings();
            onlinerSettings = new OnlinerSettings();
            twilioSettings = new TwilioSettings();

            config.GetSection(nameof(StorageSettings)).Bind(storageSettings);
            config.GetSection(nameof(AppSettings)).Bind(appSettings);
            config.GetSection(nameof(OnlinerSettings)).Bind(onlinerSettings);
            config.GetSection(nameof(TwilioSettings)).Bind(twilioSettings);
        }


        private static List<Apartment> GetOnlinerApartments(ILogger log, OnlinerSettings onlinerSettings, OnlinerConnector connector)
        {
            var results = connector.GetAsync(onlinerSettings.ConnectorUrl)
                .GetAwaiter()
                .GetResult();

            foreach (var item in results)
            {
                log.LogDebug($"All items: {item.Address}, url: {item.Uri}");
            }

            return results;
        }

        private static async Task<List<Apartment>> GetNewlyCreatedItemsAsync(ILogger log, OnlinerStorageConnector storageConnector, List<Apartment> results)
        {
            var newItems = await storageConnector.MergeAndGetNewAsync(results);

            log.LogDebug($"Count new items: {newItems.Count}");

            SmsCondition condition = new SmsCondition();
            var validResults = condition.GetValid(newItems).ToList();

            foreach (var item in validResults)
            {
                log.LogDebug($"New items: {item.Address} url: {item.Uri}");
            }
            return validResults;
        }

        private static void SendSmsMessages(List<Apartment> validResults, TwilioSettings twilioSettings)
        {
            TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.AuthToken);

            foreach (var item in validResults)
            {
                var to = new PhoneNumber(twilioSettings.PhoneNumberTo);
                var message = MessageResource.Create(
                    to,
                    from: new PhoneNumber(twilioSettings.PhoneNumberFrom),
                    body: item.ToString());

                Console.WriteLine(message.Sid);
            }
        }
    }
}

