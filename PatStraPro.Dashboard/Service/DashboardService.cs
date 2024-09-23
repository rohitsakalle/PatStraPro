using PatStraPro.Entities;

namespace PatStraPro.Dashboard.Service
{
    public class DashboardService : IDashboardService
    {
        private List<DashboardResponse> _items = new();

    public IEnumerable<DashboardResponse> GetItems()
    {
        return _items.OrderByDescending(x => x.EmergencyScore);
    }

        public void AddItem(DashboardResponse item)
        {
            _items.Add(item);
        }

        public void ClearItems()
        {
            _items.Clear();
        }

        public void RemoveCheckedItems()
        {
            _items = _items.Where(item => !item.IsChecked).ToList();
        }
    }
}