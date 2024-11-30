using System.Runtime.Serialization;

namespace PresenceBackend.Modules;

[Serializable]
public class Reservation
{
    public int Burst;
    public int Remaining;
    public DateTime Reset;
    
    [IgnoreDataMember]
    public bool Success;
}