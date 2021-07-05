using System;

namespace RimWorld
{
	// Token: 0x02000DC5 RID: 3525
	public class PawnGroupMakerParms
	{
		// Token: 0x06005195 RID: 20885 RVA: 0x001B7FE4 File Offset: 0x001B61E4
		public override string ToString()
		{
			object[] array = new object[24];
			array[0] = "groupKind=";
			array[1] = this.groupKind;
			array[2] = ", tile=";
			array[3] = this.tile;
			array[4] = ", inhabitants=";
			array[5] = this.inhabitants.ToString();
			array[6] = ", points=";
			array[7] = this.points;
			array[8] = ", faction=";
			array[9] = this.faction;
			array[10] = ", ideo=";
			int num = 11;
			Ideo ideo = this.ideo;
			array[num] = ((ideo != null) ? ideo.name : null);
			array[12] = ", traderKind=";
			array[13] = this.traderKind;
			array[14] = ", generateFightersOnly=";
			array[15] = this.generateFightersOnly.ToString();
			array[16] = ", dontUseSingleUseRocketLaunchers=";
			array[17] = this.dontUseSingleUseRocketLaunchers.ToString();
			array[18] = ", raidStrategy=";
			array[19] = this.raidStrategy;
			array[20] = ", forceOneIncap=";
			array[21] = this.forceOneIncap.ToString();
			array[22] = ", seed=";
			array[23] = this.seed;
			return string.Concat(array);
		}

		// Token: 0x04003055 RID: 12373
		public PawnGroupKindDef groupKind;

		// Token: 0x04003056 RID: 12374
		public int tile = -1;

		// Token: 0x04003057 RID: 12375
		public bool inhabitants;

		// Token: 0x04003058 RID: 12376
		public float points;

		// Token: 0x04003059 RID: 12377
		public Faction faction;

		// Token: 0x0400305A RID: 12378
		public Ideo ideo;

		// Token: 0x0400305B RID: 12379
		public TraderKindDef traderKind;

		// Token: 0x0400305C RID: 12380
		public bool generateFightersOnly;

		// Token: 0x0400305D RID: 12381
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x0400305E RID: 12382
		public RaidStrategyDef raidStrategy;

		// Token: 0x0400305F RID: 12383
		public bool forceOneIncap;

		// Token: 0x04003060 RID: 12384
		public int? seed;
	}
}
