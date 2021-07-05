using System;

namespace Verse
{
	// Token: 0x02000068 RID: 104
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x000162DC File Offset: 0x000144DC
		public static TraverseParms For(Pawn pawn, Danger maxDanger = Danger.Deadly, TraverseMode mode = TraverseMode.ByPawn, bool canBashDoors = false, bool alwaysUseAvoidGrid = false, bool canBashFences = false)
		{
			if (pawn == null)
			{
				Log.Error("TraverseParms for null pawn.");
				return TraverseParms.For(TraverseMode.NoPassClosedDoors, maxDanger, canBashDoors, alwaysUseAvoidGrid, canBashFences);
			}
			return new TraverseParms
			{
				pawn = pawn,
				maxDanger = maxDanger,
				mode = mode,
				canBashDoors = canBashDoors,
				canBashFences = canBashFences,
				alwaysUseAvoidGrid = alwaysUseAvoidGrid,
				fenceBlocked = pawn.ShouldAvoidFences
			};
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001634C File Offset: 0x0001454C
		public static TraverseParms For(TraverseMode mode, Danger maxDanger = Danger.Deadly, bool canBashDoors = false, bool alwaysUseAvoidGrid = false, bool canBashFences = false)
		{
			return new TraverseParms
			{
				pawn = null,
				mode = mode,
				maxDanger = maxDanger,
				canBashDoors = canBashDoors,
				canBashFences = canBashFences,
				alwaysUseAvoidGrid = alwaysUseAvoidGrid,
				fenceBlocked = false
			};
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0001639B File Offset: 0x0001459B
		public TraverseParms WithFenceblockedOf(Pawn otherPawn)
		{
			return this.WithFenceblocked(otherPawn.ShouldAvoidFences);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000163AC File Offset: 0x000145AC
		public TraverseParms WithFenceblocked(bool forceFenceblocked)
		{
			return new TraverseParms
			{
				pawn = this.pawn,
				mode = this.mode,
				maxDanger = this.maxDanger,
				canBashDoors = this.canBashDoors,
				canBashFences = this.canBashFences,
				alwaysUseAvoidGrid = this.alwaysUseAvoidGrid,
				fenceBlocked = (this.fenceBlocked || forceFenceblocked)
			};
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0001641F File Offset: 0x0001461F
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.");
			}
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0001643B File Offset: 0x0001463B
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false, false, false);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00016458 File Offset: 0x00014658
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBashDoors == b.canBashDoors && a.canBashFences == b.canBashFences && a.maxDanger == b.maxDanger && a.alwaysUseAvoidGrid == b.alwaysUseAvoidGrid && a.fenceBlocked == b.fenceBlocked;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x000164CC File Offset: 0x000146CC
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return a.pawn != b.pawn || a.mode != b.mode || a.canBashDoors != b.canBashDoors || a.canBashFences != b.canBashFences || a.maxDanger != b.maxDanger || a.alwaysUseAvoidGrid != b.alwaysUseAvoidGrid || a.fenceBlocked != b.fenceBlocked;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00016540 File Offset: 0x00014740
		public override bool Equals(object obj)
		{
			return obj is TraverseParms && this.Equals((TraverseParms)obj);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00016558 File Offset: 0x00014758
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBashDoors == this.canBashDoors && other.canBashFences == this.canBashFences && other.maxDanger == this.maxDanger && other.alwaysUseAvoidGrid == this.alwaysUseAvoidGrid && other.fenceBlocked == this.fenceBlocked;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000165CC File Offset: 0x000147CC
		public override int GetHashCode()
		{
			int seed = this.canBashDoors ? 1 : 0;
			if (this.pawn != null)
			{
				seed = Gen.HashCombine<Pawn>(seed, this.pawn);
			}
			else
			{
				seed = Gen.HashCombineStruct<TraverseMode>(seed, this.mode);
			}
			seed = Gen.HashCombineStruct<bool>(seed, this.canBashFences);
			seed = Gen.HashCombineStruct<Danger>(seed, this.maxDanger);
			seed = Gen.HashCombineStruct<bool>(seed, this.alwaysUseAvoidGrid);
			return Gen.HashCombineStruct<bool>(seed, this.fenceBlocked);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00016640 File Offset: 0x00014840
		public override string ToString()
		{
			string text = this.canBashDoors ? " canBashDoors" : "";
			string text2 = this.canBashFences ? " canBashFences" : "";
			string text3 = this.alwaysUseAvoidGrid ? " alwaysUseAvoidGrid" : "";
			string text4 = this.fenceBlocked ? " fenceBlocked" : "";
			if (this.mode == TraverseMode.ByPawn)
			{
				return string.Format("({0} {1} {2}{3}{4}{5}{6})", new object[]
				{
					this.mode,
					this.maxDanger,
					this.pawn,
					text,
					text2,
					text3,
					text4
				});
			}
			return string.Format("({0} {1}{2}{3}{4}{5})", new object[]
			{
				this.mode,
				this.maxDanger,
				text,
				text2,
				text3,
				text4
			});
		}

		// Token: 0x0400014E RID: 334
		public Pawn pawn;

		// Token: 0x0400014F RID: 335
		public TraverseMode mode;

		// Token: 0x04000150 RID: 336
		public Danger maxDanger;

		// Token: 0x04000151 RID: 337
		public bool canBashDoors;

		// Token: 0x04000152 RID: 338
		public bool canBashFences;

		// Token: 0x04000153 RID: 339
		public bool alwaysUseAvoidGrid;

		// Token: 0x04000154 RID: 340
		public bool fenceBlocked;
	}
}
