using CitizenFX.Core;

namespace PolyZoneLib.Client
{
    internal class ClientMain : BaseScript
    {
        public ClientMain()
        {
            Debug.WriteLine("Hi from PolyZoneLib.Client!");

            new PolyZone(new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            });
        }
    }
}