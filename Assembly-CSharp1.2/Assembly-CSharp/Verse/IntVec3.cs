using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001C RID: 28
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00007B65 File Offset: 0x00005D65
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00007B78 File Offset: 0x00005D78
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00007B86 File Offset: 0x00005D86
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00007BA3 File Offset: 0x00005DA3
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00007BC6 File Offset: 0x00005DC6
		public int LengthManhattan
		{
			get
			{
				return ((this.x >= 0) ? this.x : (-this.x)) + ((this.z >= 0) ? this.z : (-this.z));
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0007C654 File Offset: 0x0007A854
		public float AngleFlat
		{
			get
			{
				if (this.x == 0 && this.z == 0)
				{
					return 0f;
				}
				return Quaternion.LookRotation(this.ToVector3()).eulerAngles.y;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007BF9 File Offset: 0x00005DF9
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007C10 File Offset: 0x00005E10
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007C33 File Offset: 0x00005E33
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00007C56 File Offset: 0x00005E56
		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00007C60 File Offset: 0x00005E60
		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00007C6A File Offset: 0x00005E6A
		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00007C74 File Offset: 0x00005E74
		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00007C7E File Offset: 0x00005E7E
		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00007C88 File Offset: 0x00005E88
		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00007C92 File Offset: 0x00005E92
		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00007C9C File Offset: 0x00005E9C
		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00007CA6 File Offset: 0x00005EA6
		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00007CB0 File Offset: 0x00005EB0
		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0007C690 File Offset: 0x0007A890
		public static IntVec3 FromString(string str)
		{
			str = str.TrimStart(new char[]
			{
				'('
			});
			str = str.TrimEnd(new char[]
			{
				')'
			});
			string[] array = str.Split(new char[]
			{
				','
			});
			IntVec3 result;
			try
			{
				CultureInfo invariantCulture = CultureInfo.InvariantCulture;
				int newX = Convert.ToInt32(array[0], invariantCulture);
				int newY = Convert.ToInt32(array[1], invariantCulture);
				int newZ = Convert.ToInt32(array[2], invariantCulture);
				result = new IntVec3(newX, newY, newZ);
			}
			catch (Exception arg)
			{
				Log.Warning(str + " is not a valid IntVec3 format. Exception: " + arg, false);
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007CC6 File Offset: 0x00005EC6
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007CE2 File Offset: 0x00005EE2
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007D0A File Offset: 0x00005F0A
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007D18 File Offset: 0x00005F18
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0007C738 File Offset: 0x0007A938
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007D42 File Offset: 0x00005F42
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007D4B File Offset: 0x00005F4B
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007D61 File Offset: 0x00005F61
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0007C770 File Offset: 0x0007A970
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007D6E File Offset: 0x00005F6E
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0007C7E8 File Offset: 0x0007A9E8
		public bool AdjacentToCardinal(Room room)
		{
			if (!this.IsValid)
			{
				return false;
			}
			Map map = room.Map;
			if (this.InBounds(map) && this.GetRoom(map, RegionType.Set_All) == room)
			{
				return true;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			for (int i = 0; i < cardinalDirections.Length; i++)
			{
				IntVec3 intVec = this + cardinalDirections[i];
				if (intVec.InBounds(map) && intVec.GetRoom(map, RegionType.Set_All) == room)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007DA6 File Offset: 0x00005FA6
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0007C864 File Offset: 0x0007AA64
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007DB4 File Offset: 0x00005FB4
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007DE2 File Offset: 0x00005FE2
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007E10 File Offset: 0x00006010
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007E2F File Offset: 0x0000602F
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007E5E File Offset: 0x0000605E
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007E8D File Offset: 0x0000608D
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007EA5 File Offset: 0x000060A5
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00007ED3 File Offset: 0x000060D3
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007EF7 File Offset: 0x000060F7
		public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0007C8B8 File Offset: 0x0007AAB8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.y.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x0400006E RID: 110
		public int x;

		// Token: 0x0400006F RID: 111
		public int y;

		// Token: 0x04000070 RID: 112
		public int z;
	}
}
