using Microsoft.Xna.Framework;
namespace Raiji.Main
{

    public class CollisionUtil
    {


        public static Vector2 CalculateCollisionDepth(Rectangle characterBounds, Rectangle otherBounds)
        {
            //If the distance on both axis is higher than the sum of the half of both Rectangles, they are not colliding
            if(!characterBounds.Intersects(otherBounds))
            {
                //Not colliding at all, Zero Vector is returned
                return Vector2.Zero;
            }

            //Create depth Vector
            Vector2 depth = new Vector2(0, 0);

            //Get the Bounds Center
            Vector2 characterBoundsCenter = new Vector2(characterBounds.Center.X, characterBounds.Center.Y);
            Vector2 otherBoundsCenter = new Vector2(otherBounds.Center.X, otherBounds.Center.Y);

            //Get the distance between both Rectangles on both axis
            float distanceX = characterBoundsCenter.X - otherBoundsCenter.X;
            float distanceY = characterBoundsCenter.Y - otherBoundsCenter.Y;

            //Is the distance on X-Axis higher than 0 -> Determine relative position of other Rectangle
            if (distanceX > 0)
            {
                //Depth on X-Axis is half of both Rectangles - the distance
                depth.X = (characterBounds.Width / 2 + otherBounds.Width / 2) - distanceX;
            }
            //Is the distance on X-Axis lower than 0: turn around the + and -
            else depth.X = -(characterBounds.Width / 2 + otherBounds.Width / 2) - distanceX;

            //Same on the Y-Axis
            if (distanceY > 0) depth.Y = (characterBounds.Height / 2 + otherBounds.Height / 2) - distanceY;
            else depth.Y = -(characterBounds.Height / 2 + otherBounds.Height / 2) - distanceY;

            //Return the final depth
            return depth;

        }

    }
}
