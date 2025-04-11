using CitizenFX.Core;
using System.Drawing;
using System.Threading.Tasks;

namespace PolyZoneLib.Client
{
    internal class ClientMain : BaseScript
    {
        private bool IsRecording = false;
        private Vector2 PrevPos = new Vector2(0, 0);

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
        }

        public async Task OnRecord()
        {
            if (PrevPos == new Vector2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y))
            {
                return;
            }

            PrevPos = new Vector2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y);

            Debug.WriteLine($"new Vector2({Game.PlayerPed.Position.X}f, {Game.PlayerPed.Position.Y}f),");

            await Delay(2500);
        }

        [Command("record")]
        public void Record()
        {
            IsRecording = !IsRecording;

            if (!IsRecording)
            {
                Debug.WriteLine("RECORDING STOPPED");
                Tick -= OnRecord;
                return;
            }

            Debug.WriteLine("RECORDING STARTED");
            Tick += OnRecord;
        }
    }
}