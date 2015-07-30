using Google.GData.Client;
using System;
namespace TimeNazi
{
    public interface IDatabaseWrapper
    {
        bool InitializeActivityLogTable();
        bool InitializeConfigTable();
        void Initialize(OAuth2Parameters authParams);
        IConfiguration LoadConfig();
        void SaveConfig(IConfiguration config);
        void StoreActivity(ActivityLogEntry entry);
    }
}
