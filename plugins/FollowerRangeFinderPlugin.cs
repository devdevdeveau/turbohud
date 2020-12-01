using System.Linq;
using Turbo.Plugins.Default;


namespace Turbo.Plugins.MayorMcCheese
{
    public class FollowerRangeFinderPlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection Decorator { get; set; }

        public FollowerRangeFinderPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            // TODO: WorldDecoratorCollection needs some differently colored brushes to support the idea of distance breakpoints, so that the further apart a follower is the color shades or gets thinner, etc.  Not sure of the max distance where a follower snaps back.  Close followers probably don't need a line.
            Decorator = new WorldDecoratorCollection(
                new GroundShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 230, 200, 230, 2),
                    ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                    Radius = 2.0f,
                    ShapePainter = new WorldShapeLinePainter(Hud)
                }
            );
        }

        public void PaintWorld(WorldLayer layer)
        {
            var actors = Hud.Game.Actors.Where(x => !x.IsDisabled && x.SnoActor.Kind == ActorKind.Follower);
            foreach (var actor in actors)
            {
                Decorator.Paint(layer, actor, actor.FloorCoordinate, null);
            }
        }

        private class WorldShapeLinePainter : IWorldShapePainter
        {
            public IController Hud { get; }

            public WorldShapeLinePainter(IController hud)
            {
                Hud = hud;
            }

            public void Paint(float x, float y, float z, float radius, float rotation, IBrush brush, IBrush shadowBrush)
            {
                Hud.Render.GetMinimapCoordinates(Hud.Game.Me.FloorCoordinate.X, Hud.Game.Me.FloorCoordinate.Y, out var meOnMapX, out var meOnMapY);
                Hud.Render.GetMinimapCoordinates(x, y, out var pointOnMapX, out var pointOnMapY);

                brush.DrawLine(pointOnMapX, pointOnMapY, meOnMapX, meOnMapY);
            }
        }
    }
}