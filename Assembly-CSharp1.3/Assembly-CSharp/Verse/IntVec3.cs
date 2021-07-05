using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000018 RID: 24
	public struct IntVec3 : IEquatable<IntVec3>
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000063AB File Offset: 0x000045AB
		public IntVec2 ToIntVec2
		{
			get
			{
				return new IntVec2(this.x, this.z);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000063BE File Offset: 0x000045BE
		public bool IsValid
		{
			get
			{
				return this.y >= 0;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000106 RID: 262 RVA: 0x000063CC File Offset: 0x000045CC
		public int LengthHorizontalSquared
		{
			get
			{
				return this.x * this.x + this.z * this.z;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000107 RID: 263 RVA: 0x000063E9 File Offset: 0x000045E9
		public float LengthHorizontal
		{
			get
			{
				return GenMath.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000108 RID: 264 RVA: 0x0000640C File Offset: 0x0000460C
		public int LengthManhattan
		{
			get
			{
				return ((this.x >= 0) ? this.x : (-this.x)) + ((this.z >= 0) ? this.z : (-this.z));
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00006440 File Offset: 0x00004640
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

		// Token: 0x0600010A RID: 266 RVA: 0x0000647B File Offset: 0x0000467B
		public IntVec3(int newX, int newY, int newZ)
		{
			this.x = newX;
			this.y = newY;
			this.z = newZ;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006492 File Offset: 0x00004692
		public IntVec3(Vector3 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.z;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x000064B5 File Offset: 0x000046B5
		public IntVec3(Vector2 v)
		{
			this.x = (int)v.x;
			this.y = 0;
			this.z = (int)v.y;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600010D RID: 269 RVA: 0x000064D8 File Offset: 0x000046D8
		public static IntVec3 Zero
		{
			get
			{
				return new IntVec3(0, 0, 0);
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000064E2 File Offset: 0x000046E2
		public static IntVec3 North
		{
			get
			{
				return new IntVec3(0, 0, 1);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000064EC File Offset: 0x000046EC
		public static IntVec3 East
		{
			get
			{
				return new IntVec3(1, 0, 0);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000110 RID: 272 RVA: 0x000064F6 File Offset: 0x000046F6
		public static IntVec3 South
		{
			get
			{
				return new IntVec3(0, 0, -1);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006500 File Offset: 0x00004700
		public static IntVec3 West
		{
			get
			{
				return new IntVec3(-1, 0, 0);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000112 RID: 274 RVA: 0x0000650A File Offset: 0x0000470A
		public static IntVec3 NorthWest
		{
			get
			{
				return new IntVec3(-1, 0, 1);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00006514 File Offset: 0x00004714
		public static IntVec3 NorthEast
		{
			get
			{
				return new IntVec3(1, 0, 1);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000114 RID: 276 RVA: 0x0000651E File Offset: 0x0000471E
		public static IntVec3 SouthWest
		{
			get
			{
				return new IntVec3(-1, 0, -1);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00006528 File Offset: 0x00004728
		public static IntVec3 SouthEast
		{
			get
			{
				return new IntVec3(1, 0, -1);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00006532 File Offset: 0x00004732
		public static IntVec3 Invalid
		{
			get
			{
				return new IntVec3(-1000, -1000, -1000);
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006548 File Offset: 0x00004748
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
				Log.Warning(str + " is not a valid IntVec3 format. Exception: " + arg);
				result = IntVec3.Invalid;
			}
			return result;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000065F0 File Offset: 0x000047F0
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, (float)this.y, (float)this.z);
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000660C File Offset: 0x0000480C
		public Vector3 ToVector3Shifted()
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y, (float)this.z + 0.5f);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006634 File Offset: 0x00004834
		public Vector3 ToVector3ShiftedWithAltitude(AltitudeLayer AltLayer)
		{
			return this.ToVector3ShiftedWithAltitude(AltLayer.AltitudeFor());
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006642 File Offset: 0x00004842
		public Vector3 ToVector3ShiftedWithAltitude(float AddedAltitude)
		{
			return new Vector3((float)this.x + 0.5f, (float)this.y + AddedAltitude, (float)this.z + 0.5f);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000666C File Offset: 0x0000486C
		public bool InHorDistOf(IntVec3 otherLoc, float maxDist)
		{
			float num = (float)(this.x - otherLoc.x);
			float num2 = (float)(this.z - otherLoc.z);
			return num * num + num2 * num2 <= maxDist * maxDist;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x000066A4 File Offset: 0x000048A4
		public static IntVec3 FromVector3(Vector3 v)
		{
			return IntVec3.FromVector3(v, 0);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000066AD File Offset: 0x000048AD
		public static IntVec3 FromVector3(Vector3 v, int newY)
		{
			return new IntVec3((int)v.x, newY, (int)v.z);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000066C3 File Offset: 0x000048C3
		public Vector2 ToUIPosition()
		{
			return this.ToVector3Shifted().MapToUIPosition();
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000066D0 File Offset: 0x000048D0
		public Rect ToUIRect()
		{
			Vector2 vector = this.ToVector3().MapToUIPosition();
			Vector2 vector2 = (this + IntVec3.NorthEast).ToVector3().MapToUIPosition();
			return new Rect(vector.x, vector2.y, vector2.x - vector.x, vector.y - vector2.y);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006734 File Offset: 0x00004934
		public bool AdjacentToCardinal(IntVec3 other)
		{
			return this.IsValid && ((other.z == this.z && (other.x == this.x + 1 || other.x == this.x - 1)) || (other.x == this.x && (other.z == this.z + 1 || other.z == this.z - 1)));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000067AC File Offset: 0x000049AC
		public bool AdjacentToDiagonal(IntVec3 other)
		{
			return this.IsValid && Mathf.Abs(this.x - other.x) == 1 && Mathf.Abs(this.z - other.z) == 1;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000067E4 File Offset: 0x000049E4
		public bool AdjacentToCardinal(District district)
		{
			if (!this.IsValid)
			{
				return false;
			}
			Map map = district.Map;
			if (this.InBounds(map) && this.GetDistrict(map, RegionType.Set_All) == district)
			{
				return true;
			}
			IntVec3[] cardinalDirections = GenAdj.CardinalDirections;
			for (int i = 0; i < cardinalDirections.Length; i++)
			{
				IntVec3 intVec = this + cardinalDirections[i];
				if (intVec.InBounds(map) && intVec.GetDistrict(map, RegionType.Set_All) == district)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006862 File Offset: 0x00004A62
		public IntVec3 ClampInsideMap(Map map)
		{
			return this.ClampInsideRect(CellRect.WholeMap(map));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006870 File Offset: 0x00004A70
		public IntVec3 ClampInsideRect(CellRect rect)
		{
			this.x = Mathf.Clamp(this.x, rect.minX, rect.maxX);
			this.y = 0;
			this.z = Mathf.Clamp(this.z, rect.minZ, rect.maxZ);
			return this;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000068C4 File Offset: 0x00004AC4
		public static IntVec3 operator +(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000068F2 File Offset: 0x00004AF2
		public static IntVec3 operator -(IntVec3 a, IntVec3 b)
		{
			return new IntVec3(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006920 File Offset: 0x00004B20
		public static IntVec3 operator *(IntVec3 a, int i)
		{
			return new IntVec3(a.x * i, a.y * i, a.z * i);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000693F File Offset: 0x00004B3F
		public static bool operator ==(IntVec3 a, IntVec3 b)
		{
			return a.x == b.x && a.z == b.z && a.y == b.y;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000696E File Offset: 0x00004B6E
		public static bool operator !=(IntVec3 a, IntVec3 b)
		{
			return a.x != b.x || a.z != b.z || a.y != b.y;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000699D File Offset: 0x00004B9D
		public override bool Equals(object obj)
		{
			return obj is IntVec3 && this.Equals((IntVec3)obj);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000069B5 File Offset: 0x00004BB5
		public bool Equals(IntVec3 other)
		{
			return this.x == other.x && this.z == other.z && this.y == other.y;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000069E3 File Offset: 0x00004BE3
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.x), this.y), this.z);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006A07 File Offset: 0x00004C07
		public ulong UniqueHashCode()
		{
			return (ulong)(0L + (long)this.x + 4096L * (long)this.z + 16777216L * (long)this.y);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006A34 File Offset: 0x00004C34
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

		// Token: 0x04000042 RID: 66
		public int x;

		// Token: 0x04000043 RID: 67
		public int y;

		// Token: 0x04000044 RID: 68
		public int z;
	}
}
