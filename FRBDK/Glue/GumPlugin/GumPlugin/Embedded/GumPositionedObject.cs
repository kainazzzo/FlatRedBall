using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using Gum.Wireframe;

namespace FlatRedBall.Gum
{
    public class GumPositionedObject : PositionedObject, ICollidable
    {
        public GraphicalUiElement GumUiElement { get; set; }

        public override void UpdateDependencies(double currentTime)
        {
            base.UpdateDependencies(currentTime);

            if (GumUiElement == null) return;
            float worldX = 0, worldY = 0;

            // TODO: Multiple cameras
            MathFunctions.WindowToAbsolute((int)GumUiElement.X, (int)GumUiElement.Y, ref worldX, ref worldY, 0,
                Camera.Main,
                Camera.CoordinateRelativity.RelativeToCamera);

            X = worldX + GumUiElement.Width / 2.0f;
            Y = worldY - GumUiElement.Height / 2.0f;

            // TODO: Sync shapes based on name. This will probably require a Dictionary initialized when the GuiUiElement is set, since we want it to be as performant as possible.
            if (collision != null && collision.AxisAlignedRectangles.Count > 0)
            {
                // Right now just use the GumUiElement width/height and a single AAR for simplicity
                collision.AxisAlignedRectangles[0].Width = GumUiElement.Width;
                collision.AxisAlignedRectangles[0].Height = GumUiElement.Height;
            }
        }

        protected ShapeCollection collision;
        public ShapeCollection Collision => collision;
    }
}
