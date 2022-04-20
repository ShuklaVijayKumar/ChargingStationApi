using ChargingStationApi.CosmosInitializers.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ChargingStationApi.CosmosInitializers
{
    public class ContainerInitializer : IMustInitialize
    {
        private readonly CosmosClient dbClient;
        private readonly IConfiguration configuration;
        private readonly ILogger<ContainerInitializer> logger;
        private readonly string containerKey;
        private int resourceDocumentsRequestLimit;
        private int partitionKeyLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerInitializer"/> class.
        /// </summary>
        /// <param name="containerKey">The container key of the container to be created.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="cosmosClient">The cosmos client instance.</param>
        public ContainerInitializer(
            string containerKey,
            IConfiguration configuration,
            ILogger<ContainerInitializer> logger,
            CosmosClient cosmosClient)
        {
            this.containerKey = containerKey;
            this.configuration = configuration;
            this.logger = logger;
            this.dbClient = cosmosClient;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync()
        {
            try
            {
                string serviceName = this.configuration["ServiceName"];
                string cfgKeyDbName = $"{serviceName}:Repository:CosmosDb:DbName";
                string cfgKeyContainerName = $"{serviceName}:Repository:CosmosDb:Containers:{this.containerKey}:ContainerName";
                string cfgKeyThroughput = $"{serviceName}:Repository:CosmosDb:Containers:{this.containerKey}:Throughput";
                string cfgKeyParitionKey = $"{serviceName}:Repository:CosmosDb:Containers:{this.containerKey}:PartitionKey";
                string cfgKeyRequestLimit = $"{serviceName}:Repository:CosmosDb:Containers:{this.containerKey}:ResourceDocumentsRequestLimit";
                string cfgKeyParitionKeyLength = $"{serviceName}:Repository:CosmosDb:Containers:{this.containerKey}:KeyLength";

                this.logger.LogInformation($"ContainerInitializer: initializing containerkey: {this.containerKey}");
                this.logger.LogInformation($"ContainerInitializer config keys: DbName: {this.configuration[cfgKeyDbName]}");
                this.logger.LogInformation($"ContainerInitializer config keys: ContainerName: {this.configuration[cfgKeyContainerName]}");
                this.logger.LogInformation($"ContainerInitializer config keys: Throughput: {this.configuration[cfgKeyThroughput]}");
                this.logger.LogInformation($"ContainerInitializer config keys: ParitionKey: {this.configuration[cfgKeyParitionKey]}");
                this.logger.LogInformation($"ContainerInitializer config keys: RequestLimit: {this.configuration[cfgKeyRequestLimit]}");
                this.logger.LogInformation($"ContainerInitializer config keys: ParitionKeyLength: {this.configuration[cfgKeyParitionKeyLength]}");

                // Setting some default values if the properties are missing
                if (!int.TryParse(this.configuration[cfgKeyThroughput], out int throughput))
                {
                    this.logger.LogWarning($"Cosmos DB initialised with default 400 RU, " +
                        $"as no config supplied for throughput for DB:{this.configuration[cfgKeyDbName]}, " +
                        $"Container:{this.configuration[cfgKeyContainerName]}");
                    throughput = 400;
                }

                if (!int.TryParse(this.configuration[cfgKeyRequestLimit], out this.resourceDocumentsRequestLimit))
                {
                    this.logger.LogWarning($"Request Default Limit 1000 Resource Documents, " +
                        $"as no config supplied for resourceDocumentsRequestLimit for " +
                        $"DB:{this.configuration[cfgKeyDbName]}, Container:{this.configuration[cfgKeyContainerName]}");
                    this.resourceDocumentsRequestLimit = 1000;
                }

                if (!int.TryParse(this.configuration[cfgKeyParitionKeyLength], out this.partitionKeyLength))
                {
                    this.logger.LogWarning($"Default Partition Key Length 1, " +
                        $"as no config supplied for KeyLength for " +
                        $"DB:{this.configuration[cfgKeyDbName]}, Container:{this.configuration[cfgKeyParitionKeyLength]}");
                    this.partitionKeyLength = 2;
                }

                var database = this.dbClient.GetDatabase(this.configuration[cfgKeyDbName]);

                this.logger.LogInformation($"{nameof(ContainerInitializer)} creating {cfgKeyContainerName} container started for database {database.Id}!");

                await database.CreateContainerIfNotExistsAsync(
                        id: this.configuration[cfgKeyContainerName],
                        partitionKeyPath: this.configuration[cfgKeyParitionKey],
                        throughput: throughput);

                this.logger.LogInformation($"{nameof(ContainerInitializer)} creating {cfgKeyContainerName} container completed for database {database.Id}!");
            }
            catch (System.Exception e)
            {
                this.logger.LogError(e, $"Exception {e.Message}");
                throw;
            }
        }
    }
}
