using BienOblige.ActivityStream.Aggregates;

namespace BienOblige.ActivityStream.Collections;

public class NetworkObjectCollection: List<NetworkObject>
{ 
    public static NetworkObjectCollection From(IEnumerable<NetworkObject> objects)
    {
        var result = new NetworkObjectCollection();
        result.AddRange(objects); 
        return result;
    }

}
