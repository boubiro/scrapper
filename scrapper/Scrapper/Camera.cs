using Microsoft.Xna.Framework;
using scrapper.Scrapper.Entities;

namespace scrapper.Scrapper
{
    public class Camera
    {
        // Construct a new Camera class with standard zoom (no scaling)
        public Camera()
        {
            Zoom = 1.0f;
        }

        public Rectangle Map { get; set; }

        // Centered Position of the Camera in pixels.
        public Vector2 Position { get; private set; }

        // Current Zoom level with 1.0f being standard
        public float Zoom { get; private set; }

        // Current Rotation amount with 0.0f being standard orientation
        public float Rotation { get; private set; }

        // Height and width of the viewport window which we need to adjust
        // any time the player resizes the game window.
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        // Center of the Viewport which does not account for scale
        public Vector2 ViewportCenter => new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f);

        // Create a matrix for the camera to offset everything we draw,
        // the map and our objects. since the camera coordinates are where
        // the camera is, we offset everything by the negative of that to simulate
        // a camera moving. We also cast to integers to avoid filtering artifacts.
        public Matrix TranslationMatrix => Matrix.CreateTranslation(-(int) Position.X,
                                               -(int) Position.Y, 0) *
                                           Matrix.CreateRotationZ(Rotation) *
                                           Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                           Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));

        // Call this method with negative values to zoom out
        // or positive values to zoom in. It looks at the current zoom
        // and adjusts it by the specified amount. If we were at a 1.0f
        // zoom level and specified -0.5f amount it would leave us with
        // 1.0f - 0.5f = 0.5f so everything would be drawn at half size.
        public void AdjustZoom(float amount)
        {
            Zoom += amount;
            if (Zoom < 0.25f) Zoom = 0.25f;
        }

        // Move the camera in an X and Y amount based on the cameraMovement param.
        // if clampToMap is true the camera will try not to pan outside of the
        // bounds of the map.
        public void MoveCamera(Vector2 cameraMovement, bool clampToMap = false)
        {
            var newPosition = Position + cameraMovement;

            if (clampToMap)
                Position = MapClampedPosition(newPosition);
            else
                Position = newPosition;
        }

        public Rectangle ViewportWorldBoundry()
        {
            var viewPortCorner = ScreenToWorld(new Vector2(0, 0));
            var viewPortBottomCorner =
                ScreenToWorld(new Vector2(ViewportWidth, ViewportHeight));

            return new Rectangle((int) viewPortCorner.X,
                (int) viewPortCorner.Y,
                (int) (viewPortBottomCorner.X - viewPortCorner.X),
                (int) (viewPortBottomCorner.Y - viewPortCorner.Y));
        }

        // Center the camera on specific pixel coordinates
        public void CenterOn(Vector2 position)
        {
            Position = position;
        }

        // Center the camera on a specific player in the map
        public void CenterOn(Player player, bool clampToMap = true)
        {
            Position = CenteredPosition(player, clampToMap);
        }

        private Vector2 CenteredPosition(Player player, bool clampToMap = false)
        {
            if (clampToMap) return MapClampedPosition(player.Position);

            return player.Position;
        }

        // Clamp the camera so it never leaves the visible area of the map.
        private Vector2 MapClampedPosition(Vector2 position)
        {
            var width = ViewportWidth / Zoom;
            var height = ViewportHeight / Zoom;

            if (width < Map.Width || height < Map.Height)
            {
                var cameraMax = new Vector2(Map.X + Map.Width -
                                            width / 2,
                    Map.Y + Map.Height -
                    height / 2);

                return Vector2.Clamp(position,
                    new Vector2(Map.X + width / 2, Map.Y + height / 2),
                    cameraMax);
            }

            return new Vector2(Map.Center.X, Map.Center.Y);
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition,
                Matrix.Invert(TranslationMatrix));
        }
    }
}