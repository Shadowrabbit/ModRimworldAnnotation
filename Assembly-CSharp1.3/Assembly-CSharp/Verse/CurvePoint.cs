using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000014 RID: 20
	public struct CurvePoint
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005C22 File Offset: 0x00003E22
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00005C2A File Offset: 0x00003E2A
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00005C37 File Offset: 0x00003E37
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00005C44 File Offset: 0x00003E44
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005C53 File Offset: 0x00003E53
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005C5C File Offset: 0x00003E5C
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint(ParseHelper.FromString<Vector2>(str));
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00005C69 File Offset: 0x00003E69
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00005C22 File Offset: 0x00003E22
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x0400003B RID: 59
		private Vector2 loc;
	}
}
