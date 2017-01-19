using System.Collections.Generic;
using FlatRedBall.Math;
using FlatRedBall.Math.Geometry;
using Gum.Wireframe;
using GumEntityTester.GumRuntimes;
using RenderingLibrary.Graphics;
using System.Linq;
using Microsoft.Xna.Framework;

namespace FlatRedBall.Gum
{
    public class GumPositionedObject : PositionedObject, ICollidable
    {
        #region Properties
        private GraphicalUiElement _gumUiElement;
        public GraphicalUiElement GumUiElement
        {
            get { return _gumUiElement; }
            set
            {
                _gumUiElement = value;
                ConvertGumShapes(_gumUiElement.Children);
            }
        }

        protected ShapeCollection collision;
        public ShapeCollection Collision => collision;
        #endregion

        #region Private Methods
        private void ConvertGumShapes(List<IRenderableIpso> elements)
        {
            foreach (var child in elements)
            {
                // reCURSE YOUR DESCENDANTS!!!
                if (child.Children?.Count > 0)
                {
                    ConvertGumShapes(child.Children);
                }

                var childAsCircle = child as CircleRuntime;
                if (childAsCircle != null)
                {
                    AddCircle(childAsCircle);
                    continue;
                }

                var childAsRectangle = child as RectangleRuntime;
                if (childAsRectangle != null)
                {
                    AddRectangle(childAsRectangle);
                    continue;
                }
            }
        }
        private void WindowToAbsolute(GraphicalUiElement gumElement, ref float worldX, ref float worldY)
        {
            Vector3 position = new Vector3();

            MathFunctions.WindowToAbsolute((int)gumElement.X, (int)gumElement.Y, ref position);

            worldX = position.X;
            worldY = position.Y;
        }

        private void MapPositionedObjectCommonProperties(GraphicalUiElement uiElement, PositionedObject positionedObject)
        {
            float worldX = 0, worldY = 0;

            WindowToAbsolute(uiElement, ref worldX, ref worldY);

            positionedObject.X = worldX + uiElement.Width / 2.0f;
            positionedObject.Y = worldY - uiElement.Height / 2.0f;
            positionedObject.Name = uiElement.Name;
        }

        private void AddRectangle(RectangleRuntime childAsRectangle)
        {
            var rectangle = new AxisAlignedRectangle();
            MapRectangle(childAsRectangle, rectangle);

            this.collision.AxisAlignedRectangles.Add(rectangle);

            rectangle.Visible = true;
            rectangle.Color = Color.Blue;
        }

        private void MapRectangle(RectangleRuntime childAsRectangle, AxisAlignedRectangle rectangle)
        {
            MapPositionedObjectCommonProperties(childAsRectangle, rectangle);

            rectangle.Width = childAsRectangle.Width;
            rectangle.Height = childAsRectangle.Height;
            rectangle.RotationZ = childAsRectangle.Rotation;
        }

        private void AddCircle(CircleRuntime childAsCircle)
        {
            var circle = new Circle();
            MapCircle(childAsCircle, circle);
            this.collision.Circles.Add(circle);

            circle.Visible = true;
            circle.Color = Color.Blue;
        }

        private void MapCircle(CircleRuntime childAsCircle, Circle circle)
        {
            MapPositionedObjectCommonProperties(childAsCircle, circle);
            circle.Radius = childAsCircle.Radius;
        }
        #endregion

        #region Public Methods
        public override void UpdateDependencies(double currentTime)
        {
            base.UpdateDependencies(currentTime);

            if (GumUiElement == null) return;

            float worldX = 0, worldY = 0;

            WindowToAbsolute(GumUiElement, ref worldX, ref worldY);

            X = worldX + GumUiElement.Width / 2.0f;
            Y = worldY - GumUiElement.Height / 2.0f;

            if (collision != null)
            {
                foreach (var axisAlignedRectangle in collision.AxisAlignedRectangles)
                {
                    var gumRectangle = (RectangleRuntime)GumUiElement.GetGraphicalUiElementByName(axisAlignedRectangle.Name);

                    if (gumRectangle != null)
                    {
                        MapRectangle(gumRectangle, axisAlignedRectangle);
                        axisAlignedRectangle.ForceUpdateDependencies();
                    }
                }

                foreach (var circle in collision.Circles)
                {
                    var gumCircle = (CircleRuntime)GumUiElement.GetGraphicalUiElementByName(circle.Name);

                    if (gumCircle != null)
                    {
                        MapCircle(gumCircle, circle);
                        circle.ForceUpdateDependencies();
                    }
                }
            }
        }
        #endregion
    }
}
