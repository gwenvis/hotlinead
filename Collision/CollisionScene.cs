using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PadZex.Collision
{
    /// <summary>
    /// Handles all collisions that are added as shapes and
    /// fires events when objects are hit.
    /// </summary>
    public partial class CollisionField
    {
        private List<Cell> cells;

        public CollisionField()
        {
            cells = new List<Cell>();
            cells.Add(new Cell());
        }

        public void UpdateCollision()
        {
            var cell = cells[0];
            foreach (var shape1 in cell.Objects)
            {
                foreach (var shape2 in cell.Objects)
                {
                    if (shape1 == shape2)
                        continue;

                    shape1.CheckCollision(shape2);
                }
            }
        }

        /// <summary>
        /// Add a shape to the cell as candidate to be tested against
        /// </summary>
        /// <param name="shape"></param>
        public void AddShape(Shape shape)
        {
            Cell cell = GetCell(shape);
            cell.AddShape(shape);
        }

        /// <summary>
        /// Remove a shape from the cell
        /// </summary>
        public void RemoveShape(Shape shape)
        {
            Cell cell = GetCell(shape);
            cell.RemoveShape(shape);
        }

        public (bool, Shape) TestCollision(Shape shape)
        {
            foreach (var other in cells[0].Objects)
            {
                if(other.IsColliding(shape))
                {
                    return (true, other);
                }
            }

            return (false, null);
        }

        public  (bool, IEnumerable<Shape>) TestAllCollision(Shape shape)
        {
            var entities = new List<Shape>();

            foreach (var other in cells[0].Objects)
            {
                if(other.IsColliding(shape))
                {
                    entities.Add(other);
                }
            }

            return (entities.Count > 0, entities);
        }

        private Cell GetCell(Shape shape)
        {
            return cells[0];
        }
    }
}
