using System;

namespace RimWorld
{
	// Token: 0x0200142E RID: 5166
	public class PawnGroupMakerParms
	{
		// Token: 0x06006F82 RID: 28546 RVA: 0x00222538 File Offset: 0x00220738
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"groupKind=",
				this.groupKind,
				", tile=",
				this.tile,
				", inhabitants=",
				this.inhabitants.ToString(),
				", points=",
				this.points,
				", faction=",
				this.faction,
				", traderKind=",
				this.traderKind,
				", generateFightersOnly=",
				this.generateFightersOnly.ToString(),
				", dontUseSingleUseRocketLaunchers=",
				this.dontUseSingleUseRocketLaunchers.ToString(),
				", raidStrategy=",
				this.raidStrategy,
				", forceOneIncap=",
				this.forceOneIncap.ToString(),
				", seed=",
				this.seed
			});
		}

		// Token: 0x040049A9 RID: 18857
		public PawnGroupKindDef groupKind;

		// Token: 0x040049AA RID: 18858
		public int tile = -1;

		// Token: 0x040049AB RID: 18859
		public bool inhabitants;

		// Token: 0x040049AC RID: 18860
		public float points;

		// Token: 0x040049AD RID: 18861
		public Faction faction;

		// Token: 0x040049AE RID: 18862
		public TraderKindDef traderKind;

		// Token: 0x040049AF RID: 18863
		public bool generateFightersOnly;

		// Token: 0x040049B0 RID: 18864
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x040049B1 RID: 18865
		public RaidStrategyDef raidStrategy;

		// Token: 0x040049B2 RID: 18866
		public bool forceOneIncap;

		// Token: 0x040049B3 RID: 18867
		public int? seed;
	}
}
