using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000895 RID: 2197
	public struct ShootLine
	{
		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x0002A4C5 File Offset: 0x000286C5
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x0002A4CD File Offset: 0x000286CD
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x0002A4D5 File Offset: 0x000286D5
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x0015C464 File Offset: 0x0015A664
		public void ChangeDestToMissWild(float aimOnChance)
		{
			float num = ShootTuning.MissDistanceFromAimOnChanceCurves.Evaluate(aimOnChance, Rand.Value);
			if (num < 0f)
			{
				Log.ErrorOnce("Attempted to wild-miss less than zero tiles away", 94302089, false);
			}
			IntVec3 a;
			do
			{
				Vector2 unitVector = Rand.UnitVector2;
				Vector3 b = new Vector3(unitVector.x * num, 0f, unitVector.y * num);
				a = (this.dest.ToVector3Shifted() + b).ToIntVec3();
			}
			while (Vector3.Dot((this.dest - this.source).ToVector3(), (a - this.source).ToVector3()) < 0f);
			this.dest = a;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x0002A4E5 File Offset: 0x000286E5
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x0015C518 File Offset: 0x0015A718
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.source,
				"->",
				this.dest,
				")"
			});
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x0015C564 File Offset: 0x0015A764
		[DebugOutput]
		public static void WildMissResults()
		{
			IntVec3 intVec = new IntVec3(100, 0, 0);
			ShootLine shootLine = new ShootLine(IntVec3.Zero, intVec);
			IEnumerable<int> enumerable = Enumerable.Range(0, 101);
			IEnumerable<int> colValues = Enumerable.Range(0, 12);
			int[,] results = new int[enumerable.Count<int>(), colValues.Count<int>()];
			foreach (int num in enumerable)
			{
				for (int i = 0; i < 10000; i++)
				{
					ShootLine shootLine2 = shootLine;
					shootLine2.ChangeDestToMissWild((float)num / 100f);
					if (shootLine2.dest.z == 0 && shootLine2.dest.x > intVec.x)
					{
						results[num, shootLine2.dest.x - intVec.x]++;
					}
				}
			}
			DebugTables.MakeTablesDialog<int, int>(colValues, (int cells) => cells.ToString() + "-away\ncell\nhit%", enumerable, (int hitchance) => ((float)hitchance / 100f).ToStringPercent() + " aimon chance", delegate(int cells, int hitchance)
			{
				float num2 = (float)hitchance / 100f;
				if (cells == 0)
				{
					return num2.ToStringPercent();
				}
				return ((float)results[hitchance, cells] / 10000f * (1f - num2)).ToStringPercent();
			}, "");
		}

		// Token: 0x040025F5 RID: 9717
		private IntVec3 source;

		// Token: 0x040025F6 RID: 9718
		private IntVec3 dest;
	}
}
