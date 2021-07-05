using System;
using System.Globalization;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000017 RID: 23
	public struct IntVec2 : IEquatable<IntVec2>
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000606E File Offset: 0x0000426E
		public bool IsInvalid
		{
			get
			{
				return this.x < -500;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000607D File Offset: 0x0000427D
		public bool IsValid
		{
			get
			{
				return this.x >= -500;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x0000608F File Offset: 0x0000428F
		public static IntVec2 Zero
		{
			get
			{
				return new IntVec2(0, 0);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00006098 File Offset: 0x00004298
		public static IntVec2 One
		{
			get
			{
				return new IntVec2(1, 1);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000060A1 File Offset: 0x000042A1
		public static IntVec2 Two
		{
			get
			{
				return new IntVec2(2, 2);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000060AA File Offset: 0x000042AA
		public static IntVec2 North
		{
			get
			{
				return new IntVec2(0, 1);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000060B3 File Offset: 0x000042B3
		public static IntVec2 East
		{
			get
			{
				return new IntVec2(1, 0);
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000EB RID: 235 RVA: 0x000060BC File Offset: 0x000042BC
		public static IntVec2 South
		{
			get
			{
				return new IntVec2(0, -1);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000EC RID: 236 RVA: 0x000060C5 File Offset: 0x000042C5
		public static IntVec2 West
		{
			get
			{
				return new IntVec2(-1, 0);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000060CE File Offset: 0x000042CE
		public float Magnitude
		{
			get
			{
				return Mathf.Sqrt((float)(this.x * this.x + this.z * this.z));
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000060F1 File Offset: 0x000042F1
		public int MagnitudeManhattan
		{
			get
			{
				return Mathf.Abs(this.x) + Mathf.Abs(this.z);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000610A File Offset: 0x0000430A
		public int Area
		{
			get
			{
				return Mathf.Abs(this.x) * Mathf.Abs(this.z);
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006123 File Offset: 0x00004323
		public IntVec2(int newX, int newZ)
		{
			this.x = newX;
			this.z = newZ;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006133 File Offset: 0x00004333
		public IntVec2(Vector2 v2)
		{
			this.x = (int)v2.x;
			this.z = (int)v2.y;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000614F File Offset: 0x0000434F
		public Vector2 ToVector2()
		{
			return new Vector2((float)this.x, (float)this.z);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006164 File Offset: 0x00004364
		public Vector3 ToVector3()
		{
			return new Vector3((float)this.x, 0f, (float)this.z);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000617E File Offset: 0x0000437E
		public IntVec2 Rotated()
		{
			return new IntVec2(this.z, this.x);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00006194 File Offset: 0x00004394
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

		// Token: 0x060000F6 RID: 246 RVA: 0x000061E0 File Offset: 0x000043E0
		public string ToStringCross()
		{
			return this.x.ToString() + " x " + this.z.ToString();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006204 File Offset: 0x00004404
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00006268 File Offset: 0x00004468
		public static IntVec2 Invalid
		{
			get
			{
				return new IntVec2(-1000, -1000);
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006279 File Offset: 0x00004479
		public Vector2 ToVector2Shifted()
		{
			return new Vector2((float)this.x + 0.5f, (float)this.z + 0.5f);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000629A File Offset: 0x0000449A
		public static IntVec2 operator +(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x + b.x, a.z + b.z);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000062BB File Offset: 0x000044BB
		public static IntVec2 operator -(IntVec2 a, IntVec2 b)
		{
			return new IntVec2(a.x - b.x, a.z - b.z);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000062DC File Offset: 0x000044DC
		public static IntVec2 operator *(IntVec2 a, int b)
		{
			return new IntVec2(a.x * b, a.z * b);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000062F3 File Offset: 0x000044F3
		public static IntVec2 operator /(IntVec2 a, int b)
		{
			return new IntVec2(a.x / b, a.z / b);
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000FE RID: 254 RVA: 0x0000630A File Offset: 0x0000450A
		public IntVec3 ToIntVec3
		{
			get
			{
				return new IntVec3(this.x, 0, this.z);
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000631E File Offset: 0x0000451E
		public static bool operator ==(IntVec2 a, IntVec2 b)
		{
			return a.x == b.x && a.z == b.z;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000633F File Offset: 0x0000453F
		public static bool operator !=(IntVec2 a, IntVec2 b)
		{
			return a.x != b.x || a.z != b.z;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00006360 File Offset: 0x00004560
		public override bool Equals(object obj)
		{
			return obj is IntVec2 && this.Equals((IntVec2)obj);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00006378 File Offset: 0x00004578
		public bool Equals(IntVec2 other)
		{
			return this.x == other.x && this.z == other.z;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00006398 File Offset: 0x00004598
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.x, this.z);
		}

		// Token: 0x04000040 RID: 64
		public int x;

		// Token: 0x04000041 RID: 65
		public int z;
	}
}
