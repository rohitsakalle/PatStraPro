using PatStraPro.Entities;

namespace PatStraPro.Dashboard.Service;
/// <summary>
/// Dashboard service for doctors
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Get all Patient information from Cosmos Db
    /// </summary>
    /// <returns></returns>
    IEnumerable<DashboardResponse> GetItems();
    /// <summary>
    /// Save Patient information for dashboard
    /// </summary>
    /// <param name="item"></param>
    void AddItem(DashboardResponse item);
    /// <summary>
    /// Clear All item to clean up dashboard
    /// </summary>
    void ClearItems();
    /// <summary>
    /// Remove patient from dashboard for which doctor has completed the checkup
    /// </summary>
    void RemoveCheckedItems();
}