using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

enum MovementStates
{
	Normal, Dash, StartKnockBack, ShortKnockBack, LongKnockBack
}

namespace PinBallBattles
{
	class Player : Circle
	{
		MovementStates moveState = MovementStates.Normal;

		private int normalSpeed = 200;
		private int dashSpeed = 600;
		private Vector2 velocity;

		private int shortTimer = 1000;
		private int longTimer = 2000;
		private bool test;

		public MovementStates MoveState { get => moveState; set => moveState = value; }

		#region Main

		public Player(Shape shape, Vector2 position, int radius, bool isPlayer, bool test)
			: base(shape, position, radius, isPlayer)
		{
			this.test = test;
		}

		public void Update(GameTime gameTime, CollisionObject[] objects)
		{
			MovementControl(gameTime, objects);
		}

		#endregion

		#region MovementControl

		public void MovementControl(GameTime gameTime, CollisionObject[] objects)
		{
			Vector2 newVelocity = new Vector2();

			switch (moveState)
			{
				case MovementStates.Normal:
					newVelocity += Input(normalSpeed);
					newVelocity += ObjectCollision(objects, normalSpeed);
					break;
				case MovementStates.Dash:
					newVelocity += Input(dashSpeed);
					newVelocity += ObjectCollision(objects, dashSpeed); // it here
					break;
				case MovementStates.ShortKnockBack:
					newVelocity += velocity;
					if (CooldownTimer(gameTime, shortTimer))
					{
						moveState = MovementStates.Normal;
					}
					break;
				case MovementStates.LongKnockBack:
					newVelocity += velocity;
					if (CooldownTimer(gameTime, longTimer))
					{
						moveState = MovementStates.Normal;
					}
					break;
			}
			if (!test)
				Console.WriteLine(moveState);

			Position += newVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		#endregion

		#region Force

		public void SetVelocityAgainstPlayer(Vector2 playerPosition, float distance, float speed)
		{
			velocity = RepelDirection(playerPosition, distance) * normalSpeed;
		}
		// f() bounce off walls

		// f() bounce off pegs

		// f() jummp on pegs

		#endregion

		#region MoveStateController

		public void UpdateMoveState(MovementStates other)
		{
			if (moveState == MovementStates.Dash
			&& other == MovementStates.Dash)
			{
				moveState = MovementStates.ShortKnockBack;
			}
			else if (moveState == MovementStates.Dash
			&& other == MovementStates.Normal)
			{
				moveState = MovementStates.ShortKnockBack;
			}
			else if (moveState == MovementStates.Normal
			&& other == MovementStates.Dash)
			{
				moveState = MovementStates.LongKnockBack;
			}
			else
			{
				moveState = MovementStates.ShortKnockBack;
			}
		}

		#endregion

		#region Collision

		public Vector2 ObjectCollision(CollisionObject[] objects, float speed)
		{
			// could i get this to output a velocity
			foreach (CollisionObject o in objects)
			{
				if (o != this)
				{
					if (o.Shape == Shape.Circle)
					{
						float checkDistance = o.GetDistance(Position);
						bool hit = o.HasCollidedCircle(checkDistance, Radius);
						if (hit)
						{
							if (o.IsPlayer)
							{
								Player p = (Player)o;
								UpdateMoveState(p.MoveState);
								SetVelocityAgainstPlayer(o.Position, checkDistance, speed);
								break;
							}
							return RepelDirection(o.Position, checkDistance) * speed;
						}
					}
					if (o.Shape == Shape.Square)
					{
						Vector2 closestEdge = o.ClosestEdge(Position);
						float checkDistance = GetDistance(closestEdge);
						bool hit = HasCollidedSquare(checkDistance);
						if (hit)
						{
							return RepelDirection(closestEdge, checkDistance) * speed;
						}
					}

				}
			}
			return Vector2.Zero;
		}

		#endregion

		#region Inputs

		private Vector2 Input(float speed)
		{
			// should try to normalise this
			Vector2 inputPos = new Vector2();
			var keyboard = Keyboard.GetState();
			if (test)
			{
				if (keyboard.IsKeyDown(Keys.A))
				{
					inputPos.X -= 1;
				}
				if (keyboard.IsKeyDown(Keys.D))
				{
					inputPos.X += 1;
				}
				if (keyboard.IsKeyDown(Keys.S))
				{
					inputPos.Y += 1;
				}
				if (keyboard.IsKeyDown(Keys.W))
				{
					inputPos.Y -= 1;
				}

				//float distance = GetDistance(Position + inputPos);
				//inputPos = SetNormal(inputPos, distance);

				if (keyboard.IsKeyDown(Keys.Space))
				{
					moveState = MovementStates.Dash;
				}
				else
				{
					moveState = MovementStates.Normal;
				}
			}

			return inputPos * speed;
		}

		#endregion
	}
}
