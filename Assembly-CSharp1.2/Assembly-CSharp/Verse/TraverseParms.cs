using System;

namespace Verse
{
	// Token: 0x020000B5 RID: 181
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x060005AB RID: 1451 RVA: 0x0008C19C File Offset: 0x0008A39C
		public static TraverseParms For(Pawn pawn, Danger maxDanger = Danger.Deadly, TraverseMode mode = TraverseMode.ByPawn, bool canBash = false)
		{
			if (pawn == null)
			{
				Log.Error("TraverseParms for null pawn.", false);
				return TraverseParms.For(TraverseMode.NoPassClosedDoors, maxDanger, canBash);
			}
			return new TraverseParms
			{
				pawn = pawn,
				maxDanger = maxDanger,
				mode = mode,
				canBash = canBash
			};
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0008C1EC File Offset: 0x0008A3EC
		public static TraverseParms For(TraverseMode mode, Danger maxDanger = Danger.Deadly, bool canBash = false)
		{
			return new TraverseParms
			{
				pawn = null,
				mode = mode,
				maxDanger = maxDanger,
				canBash = canBash
			};
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0000ACAD File Offset: 0x00008EAD
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.", false);
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000ACCA File Offset: 0x00008ECA
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0000ACE2 File Offset: 0x00008EE2
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBash == b.canBash && a.maxDanger == b.maxDanger;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0000AD1E File Offset: 0x00008F1E
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return a.pawn != b.pawn || a.mode != b.mode || a.canBash != b.canBash || a.maxDanger != b.maxDanger;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0000AD5D File Offset: 0x00008F5D
		public override bool Equals(object obj)
		{
			return obj is TraverseParms && this.Equals((TraverseParms)obj);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0000AD75 File Offset: 0x00008F75
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBash == this.canBash && other.maxDanger == this.maxDanger;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0008C224 File Offset: 0x0008A424
		public override int GetHashCode()
		{
			int seed = this.canBash ? 1 : 0;
			if (this.pawn != null)
			{
				seed = Gen.HashCombine<Pawn>(seed, this.pawn);
			}
			else
			{
				seed = Gen.HashCombineStruct<TraverseMode>(seed, this.mode);
			}
			return Gen.HashCombineStruct<Danger>(seed, this.maxDanger);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0008C270 File Offset: 0x0008A470
		public override string ToString()
		{
			string text = this.canBash ? " canBash" : "";
			if (this.mode == TraverseMode.ByPawn)
			{
				return string.Concat(new object[]
				{
					"(",
					this.mode,
					" ",
					this.maxDanger,
					" ",
					this.pawn,
					text,
					")"
				});
			}
			return string.Concat(new object[]
			{
				"(",
				this.mode,
				" ",
				this.maxDanger,
				text,
				")"
			});
		}

		// Token: 0x040002C9 RID: 713
		public Pawn pawn;

		// Token: 0x040002CA RID: 714
		public TraverseMode mode;

		// Token: 0x040002CB RID: 715
		public Danger maxDanger;

		// Token: 0x040002CC RID: 716
		public bool canBash;
	}
}
