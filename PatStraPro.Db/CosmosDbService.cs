using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using PatStraPro.Entities;

namespace PatStraPro.Db
{
    public class CosmosDbService : ICosmosDbService
    {
        private const string AccountEndpoint = "https://cosmoshack.documents.azure.com:443/";
        //TODO Take from System Environment or KeyVault
        private const string AccountKey = "YourAccountKey";
        private const string DatabaseId = "DbId";
        private const string ContainerId = "ContainerId";
        private readonly CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        //"YourAccountEndpoint", "YourAccountKey", "YourDatabaseId", "YourContainerId"
        public CosmosDbService()
        {
            var cosmosClient = new CosmosClient(AccountEndpoint, AccountKey);
            _cosmosClient = cosmosClient;
            _container = cosmosClient.GetContainer(DatabaseId, ContainerId);
            InitializeAsync().Wait();
        }

        private async Task InitializeAsync()
        {
            _database = _cosmosClient.GetDatabase(DatabaseId);
            _container = await _database.CreateContainerIfNotExistsAsync(new ContainerProperties
            {
                Id = "PatientContainer",
                PartitionKeyPath = "/PatientId",
                UniqueKeyPolicy = new UniqueKeyPolicy
                {
                    UniqueKeys = { new UniqueKey { Paths = { "/ContactNumber" } } }
                }
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<List<DashboardResponse>> GetItemsAsync()
        {
            var query = _container.GetItemQueryIterator<DashboardResponse>();
            var results = new List<DashboardResponse>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }
            return results;
        }

        /// <inheritdoc/>
        public async Task AddItemAsync<T>(T item, string partitionKey)
        {
            try
            {
                // Log the item and partitionKey
                Console.WriteLine($"Item: {JsonConvert.SerializeObject(item)}, PartitionKey: {partitionKey}");

                await _container.CreateItemAsync(item, new PartitionKey(partitionKey));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"CosmosException: {ex.Message}, StatusCode: {ex.StatusCode}");
                throw;
            }
        }
    }
}
