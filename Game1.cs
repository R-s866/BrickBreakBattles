using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PinBallBattles
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Player[] players = new Player[2];
		CollisionObject[] drawObjects = new CollisionObject[6];
		CollisionObject[] objects = new CollisionObject[8];

		#region GameInit

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 900;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{

			base.Initialize();
		}

		#endregion

		#region ContentInit

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			Vector2 player1Pos = new Vector2(100, 100);
			Player player1 = new Player(Shape.Circle, player1Pos, 50, true, true);
			player1.CreateTexture(GraphicsDevice);
			objects[6] = player1;
			players[0] = player1;

			Vector2 player2Pos = new Vector2(400, 100);
			Player player2 = new Player(Shape.Circle, player2Pos, 50, true, false);
			player2.CreateTexture(GraphicsDevice);
			objects[7] = player2;
			players[1] = player2;

			LoadWalls();
			LoadPegs();
		}

		private void LoadWalls()
		{
			Vector2 wall1Pos = new Vector2(0, 450);
			Square wall1 = new Square(Shape.Square, wall1Pos, 60, 900, false);
			wall1.CreateTexture(GraphicsDevice);
			objects[0] = wall1;
			drawObjects[0] = wall1;

			Vector2 wall2Pos = new Vector2(800, 450);
			Square wall2 = new Square(Shape.Square, wall2Pos, 60, 900, false);
			wall2.CreateTexture(GraphicsDevice);
			objects[1] = wall2;
			drawObjects[1] = wall2;

			Vector2 wall3Pos = new Vector2(400, 0);
			Square wall3 = new Square(Shape.Square, wall3Pos, 800, 60, false);
			wall3.CreateTexture(GraphicsDevice);
			objects[2] = wall3;
			drawObjects[2] = wall3;

			Vector2 wall4Pos = new Vector2(400, 900);
			Square wall4 = new Square(Shape.Square, wall4Pos, 800, 60, false);
			wall4.CreateTexture(GraphicsDevice);
			objects[3] = wall4;
			drawObjects[3] = wall4;
		}

		private void LoadPegs()
		{
			Vector2 circle1Pos = new Vector2(100, 700);
			Circle circle1 = new Circle(Shape.Circle, circle1Pos, 50, false);
			circle1.CreateTexture(GraphicsDevice);
			objects[4] = circle1;
			drawObjects[4] = circle1;

			Vector2 circle2Pos = new Vector2(100, 400);
			Circle circle2 = new Circle(Shape.Circle, circle2Pos, 50, false);
			circle2.CreateTexture(GraphicsDevice);
			objects[5] = circle2;
			drawObjects[5] = circle2;
		}

		protected override void UnloadContent()
		{
		}

		#endregion

		#region Update

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			players[0].Update(gameTime, objects);
			players[1].Update(gameTime, objects);
			
			base.Update(gameTime);
		}
		
		#endregion

		#region Draw

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			foreach (Player p in players)
			{
				spriteBatch.Draw(p.Texture, p.Position, null, Color.AntiqueWhite, 0f,
					new Vector2(p.Texture.Width / 2, p.Texture.Height / 2),
					Vector2.One, SpriteEffects.None, 0f);
			}

			foreach (CollisionObject o in drawObjects)
			{
				spriteBatch.Draw(o.Texture, o.Position, null, Color.AntiqueWhite, 0f,
					new Vector2(o.Texture.Width / 2, o.Texture.Height / 2),
					Vector2.One, SpriteEffects.None, 0f);
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}

		#endregion
	}
}
