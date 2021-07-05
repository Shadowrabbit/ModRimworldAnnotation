using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E9 RID: 1257
	public struct ShootLine
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060025EC RID: 9708 RVA: 0x000EB0BC File Offset: 0x000E92BC
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060025ED RID: 9709 RVA: 0x000EB0C4 File Offset: 0x000E92C4
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000EB0CC File Offset: 0x000E92CC
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x000EB0DC File Offset: 0x000E92DC
		public void ChangeDestToMissWild(float aimOnChance)
		{
			float num = ShootTuning.MissDistanceFromAimOnChanceCurves.Evaluate(aimOnChance, Rand.Value);
			if (num < 0f)
			{
				Log.ErrorOnce("Attempted to wild-miss less than zero tiles away", 94302089);
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

		// Token: 0x060025F0 RID: 9712 RVA: 0x000EB18C File Offset: 0x000E938C
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000EB1A0 File Offset: 0x000E93A0
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

		// Token: 0x060025F2 RID: 9714 RVA: 0x000EB1EC File Offset: 0x000E93EC
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

		// Token: 0x040017B9 RID: 6073
		private IntVec3 source;

		// Token: 0x040017BA RID: 6074
		private IntVec3 dest;
	}
}
