using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000018 RID: 24
	public struct CurvePoint
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00007535 File Offset: 0x00005735
		public Vector2 Loc
		{
			get
			{
				return this.loc;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000753D File Offset: 0x0000573D
		public float x
		{
			get
			{
				return this.loc.x;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000754A File Offset: 0x0000574A
		public float y
		{
			get
			{
				return this.loc.y;
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00007557 File Offset: 0x00005757
		public CurvePoint(float x, float y)
		{
			this.loc = new Vector2(x, y);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00007566 File Offset: 0x00005766
		public CurvePoint(Vector2 loc)
		{
			this.loc = loc;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000756F File Offset: 0x0000576F
		public static CurvePoint FromString(string str)
		{
			return new CurvePoint(ParseHelper.FromString<Vector2>(str));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000757C File Offset: 0x0000577C
		public override string ToString()
		{
			return this.loc.ToStringTwoDigits();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00007535 File Offset: 0x00005735
		public static implicit operator Vector2(CurvePoint pt)
		{
			return pt.loc;
		}

		// Token: 0x04000067 RID: 103
		private Vector2 loc;
	}
}
