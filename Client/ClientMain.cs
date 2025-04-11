using CitizenFX.Core;
using System.Drawing;
using System.Threading.Tasks;

namespace PolyZoneLib.Client
{
    internal class ClientMain : BaseScript
    {
        private bool IsRecording = false;
        private Vector2 PreviousPosition = new Vector2(0, 0);
        private Vector3 StartMarkerPosition = new Vector3(0, 0, 0);

        public ClientMain()
        {
            var testZone = new PolyZone(new Vector2[]
            {
                new Vector2(-1858.797f, -632.3256f),
                new Vector2(-1860.446f, -630.6107f),
                new Vector2(-1863.622f, -627.6506f),
                new Vector2(-1862.642f, -626.3374f),
                new Vector2(-1859.648f, -623.0164f),
                new Vector2(-1856.58f, -619.6808f),
                new Vector2(-1854.122f, -617.1497f),
                new Vector2(-1851.67f, -619.1915f),
                new Vector2(-1848.887f, -621.5549f),
                new Vector2(-1851.483f, -624.6349f),
                new Vector2(-1854.477f, -628.041f),
                new Vector2(-1857.334f, -631.3133f),
                new Vector2(-1858.548f, -632.9578f)
            }, 10.30f, 13.30f, Color.FromArgb(180, 11, 55, 123));

            Tick += testZone.Draw;

            var speedZone = new PolyZone(new Vector2[]
            {
                new Vector2(-2054.94f, -481.113f),
                new Vector2(-2060.766f, -476.3067f),
                new Vector2(-2072.406f, -466.3821f),
                new Vector2(-2077.627f, -462.0201f),
                new Vector2(-2079.107f, -460.5365f),
                new Vector2(-2076.051f, -458.0151f),
                new Vector2(-2070.524f, -462.0693f),
                new Vector2(-2058.843f, -471.9008f),
                new Vector2(-2051.405f, -478.1877f),
                new Vector2(-2053.055f, -480.1611f),
                new Vector2(-2054.209f, -481.7351f)
            }, 10.00f, 13.30f, Color.FromArgb(180, 224, 50, 50));

            Tick += speedZone.Draw;
            Tick += async () =>
            {
                await speedZone.IsPlayerInside(() =>
                {
                    if (Game.PlayerPed.IsInVehicle())
                    {
                        Game.PlayerPed.CurrentVehicle.Speed = 100.0f;
                    }
                });
            };
        }

        public async Task OnRecord()
        {
            if (PreviousPosition == new Vector2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y))
            {
                return;
            }

            PreviousPosition = new Vector2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y);

            Debug.WriteLine($"new Vector2({Game.PlayerPed.Position.X}f, {Game.PlayerPed.Position.Y}f),");

            await Delay(2500);
        }

        public async Task OnRecordDrawMarkerAtStart()
        {
            World.DrawMarker(MarkerType.VerticalCylinder, StartMarkerPosition + new Vector3(0, 0, 1), Vector3.Zero, Vector3.Zero, new Vector3(1.0f, 1.0f, 1.0f), Color.FromArgb(255, 255, 0, 0));

            await Task.FromResult(0);
        }

        [Command("record")]
        public void Record()
        {
            IsRecording = !IsRecording;

            if (!IsRecording)
            {
                Debug.WriteLine("RECORDING STOPPED");
                Tick -= OnRecord;
                Tick -= OnRecordDrawMarkerAtStart;
                return;
            }

            Debug.WriteLine("RECORDING STARTED");

            StartMarkerPosition = Game.PlayerPed.Position;

            Tick += OnRecord;
            Tick += OnRecordDrawMarkerAtStart;
        }
    }
}