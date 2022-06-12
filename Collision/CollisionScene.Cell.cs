using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace PadZex.Collision
{
    public partial class CollisionField
    {
        /// <summary>
        /// A cell holds different kinds of objects that are in a scene.
        /// </summary>
        private class Cell
        {
            public IReadOnlyList<Shape> Objects => objects;

            private List<Shape> objects;

            public Cell()
            {
                objects = new List<Shape>();
            }

            internal void AddShape(Shape shape)
            {
                objects.Add(shape);
            }

            internal void RemoveShape(Shape shape)
            {
                objects.Remove(shape);
            }
        }
    }
}
