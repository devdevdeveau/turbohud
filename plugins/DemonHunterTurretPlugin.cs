using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.MayorMcCheese
{
    public class DemonHunterTurretPlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection Decorator { get; set; }

        public DemonHunterTurretPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            Decorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(255, 128, 255, 0, -2),
                    Radius = 10.0f,
                }
            );
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (Hud.Game.IsInTown)
                return;

            var actors = Hud.Game.Actors.Where(x => x.SnoActor.Sno == ActorSnoEnum._dh_sentry);

            foreach (var actor in actors)
            {
                Decorator.Paint(layer, actor, actor.FloorCoordinate, null);
            }
        }
    }
}