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
		private Vector2 velocity;

		private int normalSpeed = 200;
		private int dashSpeed = 600;
		private int gravityScale = 100;
		private int bounceScale = 200;

		private int shortTimer = 1000;
		private int longTimer = 2000;
		private bool test;
		bool isBouncing = false;

		public MovementStates MoveState { get => moveState; set => moveState = value; }
		public Vector2 Gravity { get; } = new Vector2(0, 1);

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
					newVelocity += Input() * normalSpeed;
					newVelocity += ObjectCollision(objects) * normalSpeed;
					if (isBouncing)
					{
						newVelocity += velocity * bounceScale;
						if (CooldownTimer(gameTime, shortTimer))
						{
							isBouncing = false;
						}
					}
					else
					{
						newVelocity += Gravity * gravityScale;
					}
					break;	// got a problem with gravity + going down
							// which is a problem when i want them to dash
							// in any direction
				case MovementStates.Dash:
					newVelocity += Input() * dashSpeed; // may have to do this in two different functuresr
														// one for normal inputs one for dash
					newVelocity += ObjectCollision(objects) * dashSpeed;
					if (isBouncing)
					{
						newVelocity += velocity * dashSpeed;
						if (CooldownTimer(gameTime, shortTimer))
						{
							isBouncing = false;
						}
					}
					else
					{
						newVelocity += Gravity * gravityScale;
					}
					break;
				case MovementStates.ShortKnockBack:
					ObjectCollision(objects);
					newVelocity += velocity * normalSpeed;
					if (CooldownTimer(gameTime, shortTimer))
					{
						moveState = MovementStates.Normal;
					}
					break;
				case MovementStates.LongKnockBack:
					ObjectCollision(objects);
					newVelocity += velocity * dashSpeed;
					if (CooldownTimer(gameTime, longTimer))
					{
						moveState = MovementStates.Normal;
					}
					break;
			}
			if (!test)
			Console.WriteLine(velocity);

			Position += newVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		#endregion

		#region Force

		public Vector2 NormalRepel(Vector2 otherPosition, float distance)
		{
			Vector2 direction =  VectorDirection(otherPosition);
			return GetNormal(direction, distance);
		}

		public Vector2 WallRepel(Vector2 otherPosition, float distance)
		{
			Vector2 direction = VectorDirection(otherPosition);
			direction = GetNormal(direction, distance);
			
			if (direction.X < 0)
			{
				velocity.X = -velocity.X;
			}
			else if (direction.X > 0)
			{
				velocity.X = Math.Abs(velocity.X);
			}
			if (direction.Y < 0)
			{
				velocity.Y = -velocity.Y;
			}
			else if (direction.Y > 0)
			{
				velocity.Y = Math.Abs(velocity.Y);
			}
			return velocity;
		}

		#endregion

		#region MoveStateController

		public void UpdateMoveState(CollisionObject o)
		{
			Player p = (Player)o;
			MovementStates other = p.moveState;

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
			/*else if (moveState == MovementStates.Dash
			&& other == MovementStates.LongKnockBack)
			{
				moveState = MovementStates.ShortKnockBack;
			}
			else if (moveState == MovementStates.LongKnockBack
			&& other == MovementStates.Dash)
			{
				moveState = MovementStates.LongKnockBack;
			}*/
			else
			{
				moveState = MovementStates.ShortKnockBack;
			}
		}

		#endregion

		#region Collision
		
		public Vector2 ObjectCollision(CollisionObject[] objects)
		{
			foreach (CollisionObject o in objects)
			{
				if (o != this)
				{
					if (o.Shape == Shape.Circle)
					{
						float checkDistance = o.GetDistance(Position);
						Circle other = (Circle)o;
						bool hit = HasCollided(checkDistance, other.Radius);
						if (hit)
						{
							if (o.IsPlayer)
							{
								if (moveState == MovementStates.Normal
								|| moveState == MovementStates.Dash)
								{ // might have to change this to update while knockbacked
									UpdateMoveState(o);
								}
								velocity = NormalRepel(o.Position, checkDistance);
								break;
							}
							if (moveState == MovementStates.Normal
								|| moveState == MovementStates.Dash)
							{
								isBouncing = true;
							}
							velocity = NormalRepel(o.Position, checkDistance);
							break;
						}
					}
					if (o.Shape == Shape.Square)
					{
						Vector2 closestEdge = o.ClosestEdge(Position);
						float checkDistance = GetDistance(closestEdge);
						bool hit = HasCollided(checkDistance);
						if (hit)
						{
							if (moveState == MovementStates.ShortKnockBack 
								|| moveState == MovementStates.LongKnockBack)
							{
								velocity = WallRepel(closestEdge, checkDistance);
								break;
							}
							return NormalRepel(closestEdge, checkDistance);
						}
					}

				}
			}
			return Vector2.Zero;
		}

		#endregion

		#region Inputs

		private Vector2 Input()
		{
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

				if (keyboard.IsKeyDown(Keys.Space))
				{
					moveState = MovementStates.Dash;
				}
				else
				{
					moveState = MovementStates.Normal;
				}
			}

			if (inputPos != Vector2.Zero)
			{
				float distance = GetDistance(Position + inputPos);
				inputPos = GetNormal(inputPos, distance);
			}

			return inputPos;
		}
		#endregion
	}
}
