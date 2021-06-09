using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011EF RID: 4591
	internal class RaidLootDistributor
	{
		// Token: 0x06006487 RID: 25735 RVA: 0x00044F04 File Offset: 0x00043104
		public RaidLootDistributor(IncidentParms parms, List<Pawn> allPawns, List<Thing> loot)
		{
			this.parms = parms;
			this.allPawns = allPawns;
			this.loot = loot;
			this.unusedPawns = new List<Pawn>(allPawns);
		}

		// Token: 0x06006488 RID: 25736 RVA: 0x001F30D8 File Offset: 0x001F12D8
		public void DistributeLoot()
		{
			this.recipient = this.allPawns.MaxBy((Pawn p) => p.kindDef.combatPower);
			this.recipientMassGiven = 0f;
			foreach (IGrouping<ThingDef, Thing> grouping in from t in this.loot
			group t by t.def)
			{
				foreach (Thing item in grouping)
				{
					this.DistributeItem(item);
				}
				this.NextRecipient();
			}
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x001F31B8 File Offset: 0x001F13B8
		private void DistributeItem(Thing item)
		{
			int num = item.stackCount;
			int num2 = 0;
			while (num > 0 && num2++ < 5)
			{
				num -= this.TryGiveToRecipient(item, num, false);
				if (num > 0)
				{
					this.NextRecipient();
				}
			}
			if (num > 0)
			{
				this.NextRecipient();
				this.TryGiveToRecipient(item, num, true);
			}
		}

		// Token: 0x0600648A RID: 25738 RVA: 0x001F3208 File Offset: 0x001F1408
		private int TryGiveToRecipient(Thing item, int count, bool force = false)
		{
			float num = 10f * Mathf.Max(1f, this.recipient.BodySize) - this.recipientMassGiven;
			float statValue = item.GetStatValue(StatDefOf.Mass, true);
			int num2 = force ? count : Mathf.RoundToInt(Mathf.Clamp(num / statValue, 0f, (float)count));
			if (num2 > 0)
			{
				int num3 = this.recipient.inventory.innerContainer.TryAdd(item, num2, true);
				this.recipientMassGiven += (float)num3 * statValue;
				return num3;
			}
			return 0;
		}

		// Token: 0x0600648B RID: 25739 RVA: 0x00044F2D File Offset: 0x0004312D
		private void NextRecipient()
		{
			this.recipientMassGiven = 0f;
			if (this.unusedPawns.Any<Pawn>())
			{
				this.recipient = this.unusedPawns.Pop<Pawn>();
				return;
			}
			this.recipient = this.allPawns.RandomElement<Pawn>();
		}

		// Token: 0x04004302 RID: 17154
		private readonly IncidentParms parms;

		// Token: 0x04004303 RID: 17155
		private readonly List<Pawn> allPawns;

		// Token: 0x04004304 RID: 17156
		private readonly List<Thing> loot;

		// Token: 0x04004305 RID: 17157
		private readonly List<Pawn> unusedPawns;

		// Token: 0x04004306 RID: 17158
		private Pawn recipient;

		// Token: 0x04004307 RID: 17159
		private float recipientMassGiven;
	}
}
