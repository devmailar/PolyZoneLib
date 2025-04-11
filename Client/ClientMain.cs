using CitizenFX.Core;

namespace PolyZoneLib.Client
{
    internal class ClientMain : BaseScript
    {
        public ClientMain()
        {
            Debug.WriteLine("Hi from PolyZoneLib.Client!");

            var test = new PolyZone(new Vector2[]
            {
                new Vector2(-1916.43f, -708.24f),
                new Vector2(-1919.53f, -705.05f),
                new Vector2(-1922.82f, -709.64f),
                new Vector2(-1919.58f, -711.68f)
            });

            Tick += test.Draw;
        }
    }
}