using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200001B RID: 27
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000078DD File Offset: 0x00005ADD
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000078EC File Offset: 0x00005AEC
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000078FE File Offset: 0x00005AFE
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00007907 File Offset: 0x00005B07
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00007910 File Offset: 0x00005B10
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00007919 File Offset: 0x00005B19
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00007922 File Offset: 0x00005B22
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000792B File Offset: 0x00005B2B
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00007934 File Offset: 0x00005B34
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600011D RID: 285 RVA: 0x0000793D File Offset: 0x00005B3D
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00007960 File Offset: 0x00005B60
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00007979 File Offset: 0x00005B79
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007992 File Offset: 0x00005B92
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000079A2 File Offset: 0x00005BA2
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000079BE File Offset: 0x00005BBE
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x000079D3 File Offset: 0x00005BD3
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000079ED File Offset: 0x00005BED
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0007C5A4 File Offset: 0x0007A7A4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.x.ToString(),
				", ",
				this.z.ToString(),
				")"
			});
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00007A00 File Offset: 0x00005C00
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0007C5F0 File Offset: 0x0007A7F0
		public static IntVec2 FromString(string str)
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
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int newX = Convert.ToInt32(array[0], invariantCulture);
			int newZ = Convert.ToInt32(array[1], invariantCulture);
			return new IntVec2(newX, newZ);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00007A22 File Offset: 0x00005C22
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007A33 File Offset: 0x00005C33
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007A54 File Offset: 0x00005C54
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007A75 File Offset: 0x00005C75
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00007A96 File Offset: 0x00005C96
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00007AAD File Offset: 0x00005CAD
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00007AC4 File Offset: 0x00005CC4
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007AD8 File Offset: 0x00005CD8
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007AF9 File Offset: 0x00005CF9
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007B1A File Offset: 0x00005D1A
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007B32 File Offset: 0x00005D32
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00007B52 File Offset: 0x00005D52
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x0400006C RID: 108
		public int x;

		// Token: 0x0400006D RID: 109
		public int z;
	}
}
