using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001E RID: 30
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000151 RID: 337 RVA: 0x000071C2 File Offset: 0x000053C2
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000152 RID: 338 RVA: 0x000071CE File Offset: 0x000053CE
		// (set) Token: 0x06000153 RID: 339 RVA: 0x000071D6 File Offset: 0x000053D6
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

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000071CE File Offset: 0x000053CE
		// (set) Token: 0x06000155 RID: 341 RVA: 0x000071E2 File Offset: 0x000053E2
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000071FC File Offset: 0x000053FC
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000157 RID: 343 RVA: 0x00007248 File Offset: 0x00005448
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

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00007280 File Offset: 0x00005480
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
					Log.Error("ToQuat with Rot = " + this.AsInt);
					return Quaternion.identity;
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000072F4 File Offset: 0x000054F4
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007352 File Offset: 0x00005552
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600015B RID: 347 RVA: 0x00007368 File Offset: 0x00005568
		public static Rot4 North
		{
			get
			{
				return new Rot4(0);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00007370 File Offset: 0x00005570
		public static Rot4 East
		{
			get
			{
				return new Rot4(1);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00007378 File Offset: 0x00005578
		public static Rot4 South
		{
			get
			{
				return new Rot4(2);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007380 File Offset: 0x00005580
		public static Rot4 West
		{
			get
			{
				return new Rot4(3);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00007388 File Offset: 0x00005588
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007398 File Offset: 0x00005598
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

		// Token: 0x06000161 RID: 353 RVA: 0x000073BA File Offset: 0x000055BA
		public Rot4(byte newRot)
		{
			this.rotInt = newRot;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000071D6 File Offset: 0x000053D6
		public Rot4(int newRot)
		{
			this.rotInt = (byte)(newRot % 4);
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000163 RID: 355 RVA: 0x000073C4 File Offset: 0x000055C4
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007420 File Offset: 0x00005620
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000747C File Offset: 0x0000567C
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

		// Token: 0x06000166 RID: 358 RVA: 0x000074D0 File Offset: 0x000056D0
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

		// Token: 0x06000167 RID: 359 RVA: 0x00007508 File Offset: 0x00005708
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			result.Rotate(RotDir);
			return result;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007528 File Offset: 0x00005728
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

		// Token: 0x06000169 RID: 361 RVA: 0x00007580 File Offset: 0x00005780
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
			Log.Error("FromIntVec3 with bad offset " + offset);
			return Rot4.North;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000075E3 File Offset: 0x000057E3
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000075F1 File Offset: 0x000057F1
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007603 File Offset: 0x00005803
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00007618 File Offset: 0x00005818
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

		// Token: 0x0600016E RID: 366 RVA: 0x00007662 File Offset: 0x00005862
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00007670 File Offset: 0x00005870
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

		// Token: 0x06000170 RID: 368 RVA: 0x000076E4 File Offset: 0x000058E4
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

		// Token: 0x06000171 RID: 369 RVA: 0x00007730 File Offset: 0x00005930
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
							Log.Error("Invalid rotation: " + str);
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

		// Token: 0x06000172 RID: 370 RVA: 0x000077AA File Offset: 0x000059AA
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000077C2 File Offset: 0x000059C2
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x0400004C RID: 76
		private byte rotInt;
	}
}
