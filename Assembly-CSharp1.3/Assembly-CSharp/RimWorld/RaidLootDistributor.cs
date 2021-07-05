using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C20 RID: 3104
	internal class RaidLootDistributor
	{
		// Token: 0x060048E4 RID: 18660 RVA: 0x001819E9 File Offset: 0x0017FBE9
		public RaidLootDistributor(IncidentParms parms, List<Pawn> allPawns, List<Thing> loot)
		{
			this.parms = parms;
			this.allPawns = allPawns;
			this.loot = loot;
			this.unusedPawns = new List<Pawn>(allPawns);
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x00181A14 File Offset: 0x0017FC14
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

		// Token: 0x060048E6 RID: 18662 RVA: 0x00181AF4 File Offset: 0x0017FCF4
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

		// Token: 0x060048E7 RID: 18663 RVA: 0x00181B44 File Offset: 0x0017FD44
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

		// Token: 0x060048E8 RID: 18664 RVA: 0x00181BCD File Offset: 0x0017FDCD
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

		// Token: 0x04002C72 RID: 11378
		private readonly IncidentParms parms;

		// Token: 0x04002C73 RID: 11379
		private readonly List<Pawn> allPawns;

		// Token: 0x04002C74 RID: 11380
		private readonly List<Thing> loot;

		// Token: 0x04002C75 RID: 11381
		private readonly List<Pawn> unusedPawns;

		// Token: 0x04002C76 RID: 11382
		private Pawn recipient;

		// Token: 0x04002C77 RID: 11383
		private float recipientMassGiven;
	}
}
