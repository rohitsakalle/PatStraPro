using PatStraPro.Entities;

namespace PatStraPro.Db
{
    /// <summary>
    /// Cosmos Db Service to save patient information
    /// </summary>
    public interface ICosmosDbService
    {
        /// <summary>
        /// Get all patient information from the Cosmos Db to show on dashboard
        /// </summary>
        /// <returns></returns>
        Task<List<DashboardResponse>> GetItemsAsync();
        /// <summary>
        /// Add Patient information to Cosmos Db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">patient information</param>
        /// <param name="partitionKey">patient id</param>
        /// <returns></returns>
        Task AddItemAsync<T>(T item, string partitionKey);
    }
}
