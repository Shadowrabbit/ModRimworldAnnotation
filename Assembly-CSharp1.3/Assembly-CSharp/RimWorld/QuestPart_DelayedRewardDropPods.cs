using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6B RID: 2923
	public class QuestPart_DelayedRewardDropPods : QuestPart_AddQuest
	{
		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x06004460 RID: 17504 RVA: 0x0016AF6D File Offset: 0x0016916D
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.DelayedRewardDropPods;
			}
		}

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x06004461 RID: 17505 RVA: 0x0016AF74 File Offset: 0x00169174
		public override bool CanAdd
		{
			get
			{
				return Rand.Chance(this.chance);
			}
		}

		// Token: 0x06004462 RID: 17506 RVA: 0x0016AF84 File Offset: 0x00169184
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<List<Thing>>("rewards", this.rewards, false);
			slate.Set<Faction>("faction", this.faction, false);
			slate.Set<Pawn>("giver", this.giver, false);
			slate.Set<int>("delayTicks", this.delayTicks, false);
			slate.Set<string>("customLetterLabel", this.customLetterLabel, false);
			slate.Set<string>("customLetterText", this.customLetterText, false);
			return slate;
		}

		// Token: 0x06004463 RID: 17507 RVA: 0x0016B002 File Offset: 0x00169202
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06004464 RID: 17508 RVA: 0x0016B014 File Offset: 0x00169214
		public override void PostAdd()
		{
			this.rewards.Clear();
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0016B024 File Offset: 0x00169224
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.rewardThings.Clear();
				this.rewardPawns.Clear();
				foreach (Thing thing in this.rewards)
				{
					Pawn item;
					if ((item = (thing as Pawn)) != null)
					{
						this.rewardPawns.Add(item);
					}
					else
					{
						this.rewardThings.Add(thing);
					}
				}
			}
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.giver, "giver", false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Values.Look<float>(ref this.chance, "chance", 0f, false);
			Scribe_Collections.Look<Pawn>(ref this.rewardPawns, "rewardPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.rewardThings, "rewardThings", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.rewardPawns.RemoveAll((Pawn x) => x == null);
				foreach (Pawn item2 in this.rewardPawns)
				{
					this.rewards.Add(item2);
				}
				foreach (Thing item3 in this.rewardThings)
				{
					this.rewards.Add(item3);
				}
				this.rewardThings.Clear();
				this.rewardPawns.Clear();
			}
		}

		// Token: 0x0400297B RID: 10619
		public List<Thing> rewards = new List<Thing>();

		// Token: 0x0400297C RID: 10620
		public int delayTicks;

		// Token: 0x0400297D RID: 10621
		public string customLetterLabel;

		// Token: 0x0400297E RID: 10622
		public string customLetterText;

		// Token: 0x0400297F RID: 10623
		public Faction faction;

		// Token: 0x04002980 RID: 10624
		public Pawn giver;

		// Token: 0x04002981 RID: 10625
		public float chance = 1f;

		// Token: 0x04002982 RID: 10626
		private List<Thing> rewardThings = new List<Thing>();

		// Token: 0x04002983 RID: 10627
		private List<Pawn> rewardPawns = new List<Pawn>();
	}
}
