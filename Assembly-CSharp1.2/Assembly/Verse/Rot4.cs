using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000021 RID: 33
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000080DA File Offset: 0x000062DA
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000080E6 File Offset: 0x000062E6
		// (set) Token: 0x0600017B RID: 379 RVA: 0x000080EE File Offset: 0x000062EE
		public byte AsByte
		{
			get
			{
				return this.rotInt;
			}
			set
			{
				this.rotInt = value % 4;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000080E6 File Offset: 0x000062E6
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000080FA File Offset: 0x000062FA
		public int AsInt
		{
			get
			{
				return (int)this.rotInt;
			}
			set
			{
				if (value < 0)
				{
					value += 4000;
				}
				this.rotInt = (byte)(value % 4);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0007CD14 File Offset: 0x0007AF14
		public float AsAngle
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return 0f;
				case 1:
					return 90f;
				case 2:
					return 180f;
				case 3:
					return 270f;
				default:
					return 0f;
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0007CD60 File Offset: 0x0007AF60
		public SpectateRectSide AsSpectateSide
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return SpectateRectSide.Up;
				case 1:
					return SpectateRectSide.Right;
				case 2:
					return SpectateRectSide.Down;
				case 3:
					return SpectateRectSide.Left;
				default:
					return SpectateRectSide.None;
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0007CD98 File Offset: 0x0007AF98
		public Quaternion AsQuat
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Quaternion.identity;
				case 1:
					return Quaternion.LookRotation(Vector3.right);
				case 2:
					return Quaternion.LookRotation(Vector3.back);
				case 3:
					return Quaternion.LookRotation(Vector3.left);
				default:
					Log.Error("ToQuat with Rot = " + this.AsInt, false);
					return Quaternion.identity;
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0007CE0C File Offset: 0x0007B00C
		public Vector2 AsVector2
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Vector2.up;
				case 1:
					return Vector2.right;
				case 2:
					return Vector2.down;
				case 3:
					return Vector2.left;
				default:
					throw new Exception("rotInt's value cannot be >3 but it is:" + this.rotInt);
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00008113 File Offset: 0x00006313
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00008129 File Offset: 0x00006329
		public static Rot4 North
		{
			get
			{
				return new Rot4(0);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00008131 File Offset: 0x00006331
		public static Rot4 East
		{
			get
			{
				return new Rot4(1);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00008139 File Offset: 0x00006339
		public static Rot4 South
		{
			get
			{
				return new Rot4(2);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00008141 File Offset: 0x00006341
		public static Rot4 West
		{
			get
			{
				return new Rot4(3);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00008149 File Offset: 0x00006349
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0007CE6C File Offset: 0x0007B06C
		public static Rot4 Invalid
		{
			get
			{
				return new Rot4
				{
					rotInt = 200
				};
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008157 File Offset: 0x00006357
		public Rot4(byte newRot)
		{
			this.rotInt = newRot;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000080EE File Offset: 0x000062EE
		public Rot4(int newRot)
		{
			this.rotInt = (byte)(newRot % 4);
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600018B RID: 395 RVA: 0x0007CE90 File Offset: 0x0007B090
		public IntVec3 FacingCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(0, 0, 1);
				case 1:
					return new IntVec3(1, 0, 0);
				case 2:
					return new IntVec3(0, 0, -1);
				case 3:
					return new IntVec3(-1, 0, 0);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0007CEEC File Offset: 0x0007B0EC
		public IntVec3 RighthandCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(1, 0, 0);
				case 1:
					return new IntVec3(0, 0, -1);
				case 2:
					return new IntVec3(-1, 0, 0);
				case 3:
					return new IntVec3(0, 0, 1);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600018D RID: 397 RVA: 0x0007CF48 File Offset: 0x0007B148
		public Rot4 Opposite
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new Rot4(2);
				case 1:
					return new Rot4(3);
				case 2:
					return new Rot4(0);
				case 3:
					return new Rot4(1);
				default:
					return default(Rot4);
				}
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0007CF9C File Offset: 0x0007B19C
		public void Rotate(RotationDirection RotDir)
		{
			if (RotDir == RotationDirection.Clockwise)
			{
				int asInt = this.AsInt;
				this.AsInt = asInt + 1;
			}
			if (RotDir == RotationDirection.Counterclockwise)
			{
				int asInt = this.AsInt;
				this.AsInt = asInt - 1;
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0007CFD4 File Offset: 0x0007B1D4
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			result.Rotate(RotDir);
			return result;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0007CFF4 File Offset: 0x0007B1F4
		public static Rot4 FromAngleFlat(float angle)
		{
			angle = GenMath.PositiveMod(angle, 360f);
			if (angle < 45f)
			{
				return Rot4.North;
			}
			if (angle < 135f)
			{
				return Rot4.East;
			}
			if (angle < 225f)
			{
				return Rot4.South;
			}
			if (angle < 315f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0007D04C File Offset: 0x0007B24C
		public static Rot4 FromIntVec3(IntVec3 offset)
		{
			if (offset.x == 1)
			{
				return Rot4.East;
			}
			if (offset.x == -1)
			{
				return Rot4.West;
			}
			if (offset.z == 1)
			{
				return Rot4.North;
			}
			if (offset.z == -1)
			{
				return Rot4.South;
			}
			Log.Error("FromIntVec3 with bad offset " + offset, false);
			return Rot4.North;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008160 File Offset: 0x00006360
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000816E File Offset: 0x0000636E
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00008180 File Offset: 0x00006380
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0007D0B0 File Offset: 0x0007B2B0
		public override int GetHashCode()
		{
			switch (this.rotInt)
			{
			case 0:
				return 235515;
			case 1:
				return 5612938;
			case 2:
				return 1215650;
			case 3:
				return 9231792;
			default:
				return (int)this.rotInt;
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008195 File Offset: 0x00006395
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0007D0FC File Offset: 0x0007B2FC
		public string ToStringHuman()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North".Translate();
			case 1:
				return "East".Translate();
			case 2:
				return "South".Translate();
			case 3:
				return "West".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0007D170 File Offset: 0x0007B370
		public string ToStringWord()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North";
			case 1:
				return "East";
			case 2:
				return "South";
			case 3:
				return "West";
			default:
				return "error";
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0007D1BC File Offset: 0x0007B3BC
		public static Rot4 FromString(string str)
		{
			int num;
			byte newRot;
			if (int.TryParse(str, out num))
			{
				newRot = (byte)num;
			}
			else if (!(str == "North"))
			{
				if (!(str == "East"))
				{
					if (!(str == "South"))
					{
						if (!(str == "West"))
						{
							newRot = 0;
							Log.Error("Invalid rotation: " + str, false);
						}
						else
						{
							newRot = 3;
						}
					}
					else
					{
						newRot = 2;
					}
				}
				else
				{
					newRot = 1;
				}
			}
			else
			{
				newRot = 0;
			}
			return new Rot4(newRot);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000081A2 File Offset: 0x000063A2
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000081BA File Offset: 0x000063BA
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x04000075 RID: 117
		private byte rotInt;
	}
}
