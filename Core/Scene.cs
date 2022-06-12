using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PadZex.Collision;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PadZex.Core
{
	/// <summary>
	/// This contains the base functionality a scene needs to function.
	/// Entities can be added here and then 
	/// </summary>
	public class Scene
	{
		/// <summary>
		/// The scene that is currently set as "active" and updated and drawn
		/// </summary>
		public static Scene MainScene { get; private set; }

		protected readonly List<Entity> entities;
		private readonly List<Entity> entityGulag;
		private readonly List<Entity> addedEntities;
		private readonly ContentManager content;
		private readonly CollisionField quadTree;

		public Camera Camera { get; protected set; }
		public Scene(ContentManager content)
		{
			entities = new List<Entity>();
			entityGulag = new List<Entity>();
			addedEntities = new List<Entity>();
			quadTree = new CollisionField();
			this.content = content;
		}

		/// <summary>
		/// Add an entity to the addedEntities List
		/// </summary>
		public void AddEntity(Entity entity)
		{
			addedEntities.Add(entity);
		}

		/// <summary>
		/// Immediately add an entity to the scene.
		/// The scene can not be updating at the moment when adding.
		///</summary>
		public void AddEntityImmediate(Entity entity)
		{
			entities.Add(entity);
			entity.Initialize(content);
			Shape shape = entity.InitializeShape();
			if (shape != null)
			{
				quadTree.AddShape(shape);
			}
		}

		/// <summary>
		/// Initializes new entities and adds them to the enties List
		/// </summary>
		public void InitEntities()
		{
			for (int i = 0; i < addedEntities.Count; i++)
			{
				entities.Add(addedEntities[i]);
				addedEntities[i].Initialize(content);
				Shape shape = addedEntities[i].InitializeShape();

				if (shape != null)
				{
					quadTree.AddShape(shape);
				}
			}

			addedEntities.Clear();
		}

		/// <summary>
		/// Initialize is called when the scene activates.
		/// </summary>
		public virtual void Initialize() { }

		public virtual void Draw(SpriteBatch spriteBatch, Time time)
		{
			// TODO: check whats on screen first before drawing it.

			foreach (var entity in entities)
			{
				entity.Draw(spriteBatch, time);
			}
		}

		public virtual void Update(Time time)
		{
            // delete all entities in the dirty list first.
            for (int i = entityGulag.Count - 1; i >= 0; i--)
            {
                Entity entity = entityGulag[i];
                if (!entities.Contains(entity)) continue;

				entity.OnDestroy();
				entities.Remove(entity);

				if(entity.Shape != null)
                {
					quadTree.RemoveShape(entity.Shape);
                }
			}

			entityGulag.Clear();

			foreach (var entity in entities)
			{
				entity.Update(time);
			}

			if (addedEntities.Any())
			{
				InitEntities();
			}


			quadTree.UpdateCollision();
		}

		/// <summary>
		/// Set the current scene as the main scene
		/// </summary>
		public void SetAsMainScene(Camera camera = null)
		{
			MainScene = this;
			if(this.Camera == null) Camera = camera;
		}

		/// <summary>
		/// Find an entity in the Main Scene and return it
		/// </summary>
		/// <param name="tag">Tag the entity needs</param>
		/// <returns>An entity, null if not found</returns>
		public Entity FindEntity(string tag) => entities.FirstOrDefault(x => x.Tags.Contains(tag));

		/// <summary>
		/// Find an Entity of a specific type in the main scene and return it
		/// </summary>
		/// <typeparam name="T">Must inherit <see cref="Entity"/>.</typeparam>
		/// <param name="tag">Tag the entity needs</param>
		/// <returns>An entity of Type <typeparamref name="T"/>, or null if not found.</returns>
		public T FindEntity<T>(string tag) where T : Entity => entities.FirstOrDefault(x => x.Tags.Contains(tag) && x is T) as T;

		/// <summary>
		/// Find an Entity with a specific type and return it.
		/// </summary>
		public T FindEntity<T>() where T : Entity => entities.FirstOrDefault(x => x is T) as T;

		/// <summary>
		/// Mark an entity in the MainScene for death and delete it the next frame.
		/// </summary>
		/// <param name="entity"></param>
		public void DeleteEntity(Entity entity) => entityGulag.Add(entity);

		/// <summary>
		/// Tests for collision against a shape in the quad tree.
		/// </summary>
		/// <returns>returns whether collided and the shape it collided with if true.</returns>
		public (bool, Shape) TestCollision(Shape shape) => quadTree.TestCollision(shape);

		/// <summary>
		/// Tests for collision against a shape in the quad tree.
		/// </summary>
		/// <returns>returns whether collided and the shapes it collided with if true.</returns>
		public (bool, IEnumerable<Shape>) TestAllCollision(Shape shape) => quadTree.TestAllCollision(shape);
	}
}
