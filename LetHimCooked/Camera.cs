
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LetHimCooked
{
    public class Camera
    { 
        public Vector2 cameraPos;
        public static Vector2 objectPos;
        public static Vector2 playerPos;
        public Vector2 scroll_Factor = new Vector2 (1.0f,1);

        
        public void WorldPos(float objectposX, float objectposY)
        {
            objectPos = new Vector2 (objectposX, objectposY);
        }

        public void PlayerHitBox(Rectangle fa)
        {
            
        }
    }
}
