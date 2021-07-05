using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001143 RID: 4419
	public class CompHackable : ThingComp
	{
		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06006A13 RID: 27155 RVA: 0x0023B404 File Offset: 0x00239604
		public CompProperties_Hackable Props
		{
			get
			{
				return (CompProperties_Hackable)this.props;
			}
		}

		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06006A14 RID: 27156 RVA: 0x0023B411 File Offset: 0x00239611
		public float ProgressPercent
		{
			get
			{
				return this.progress / this.defence;
			}
		}

		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06006A15 RID: 27157 RVA: 0x0023B420 File Offset: 0x00239620
		public bool IsHacked
		{
			get
			{
				return this.progress >= this.defence;
			}
		}

		// Token: 0x06006A16 RID: 27158 RVA: 0x0023B433 File Offset: 0x00239633
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.defence = this.Props.defence;
		}

		// Token: 0x06006A17 RID: 27159 RVA: 0x0023B44D File Offset: 0x0023964D
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("CompHackable"))
			{
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06006A18 RID: 27160 RVA: 0x0023B464 File Offset: 0x00239664
		public void Hack(float amount, Pawn hacker = null)
		{
			bool isHacked = this.IsHacked;
			this.progress += amount;
			this.progress = Mathf.Clamp(this.progress, 0f, this.defence);
			if (!isHacked && this.IsHacked)
			{
				if (!this.hackingCompletedSignal.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.hackingCompletedSignal, this.parent.Named("SUBJECT")));
				}
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Hacked", this.parent.Named("SUBJECT"));
				this.parent.BroadcastCompSignal("Hackend");
				if (this.Props.completedQuest != null)
				{
					Slate slate = new Slate();
					slate.Set<Map>("map", this.parent.Map, false);
					if (this.Props.completedQuest.CanRun(slate))
					{
						QuestUtility.GenerateQuestAndMakeAvailable(this.Props.completedQuest, slate);
					}
				}
			}
			if (this.lastHackTick < 0)
			{
				if (!this.hackingStartedSignal.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.hackingStartedSignal, this.parent.Named("SUBJECT")));
				}
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "HackingStarted", this.parent.Named("SUBJECT"));
			}
			this.lastUserSpeed = amount;
			this.lastHackTick = Find.TickManager.TicksGame;
			this.lastUser = hacker;
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x0023B5E5 File Offset: 0x002397E5
		public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			if (this.IsHacked)
			{
				yield return new FloatMenuOption("CannotHack".Translate(this.parent.Label) + ": " + "AlreadyHacked".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			CompHackable.tmpAllowedPawns.Clear();
			for (int i = 0; i < selPawns.Count; i++)
			{
				if (selPawns[i].CanReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					CompHackable.tmpAllowedPawns.Add(selPawns[i]);
				}
			}
			if (CompHackable.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotHack".Translate(this.parent.Label) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			CompHackable.tmpAllowedPawns.Clear();
			for (int j = 0; j < selPawns.Count; j++)
			{
				if (HackUtility.IsCapableOfHacking(selPawns[j]))
				{
					CompHackable.tmpAllowedPawns.Add(selPawns[j]);
				}
			}
			if (CompHackable.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotHack".Translate(this.parent.Label) + ": " + "IncapableOfHacking".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			CompHackable.tmpAllowedPawns.Clear();
			for (int k = 0; k < selPawns.Count; k++)
			{
				if (HackUtility.IsCapableOfHacking(selPawns[k]) && selPawns[k].CanReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					CompHackable.tmpAllowedPawns.Add(selPawns[k]);
				}
			}
			if (CompHackable.tmpAllowedPawns.Count > 0)
			{
				yield return new FloatMenuOption("Hack".Translate(this.parent.Label), delegate()
				{
					CompHackable.tmpAllowedPawns[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, this.parent), new JobTag?(JobTag.Misc), false);
					for (int l = 1; l < CompHackable.tmpAllowedPawns.Count; l++)
					{
						FloatMenuMakerMap.PawnGotoAction(this.parent.Position, CompHackable.tmpAllowedPawns[l], RCellFinder.BestOrderedGotoDestNear(this.parent.Position, CompHackable.tmpAllowedPawns[l], null));
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x06006A1A RID: 27162 RVA: 0x0023B5FC File Offset: 0x002397FC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<int>(ref this.lastHackTick, "lastHackTick", 0, false);
			Scribe_Values.Look<float>(ref this.defence, "defence", 0f, false);
			Scribe_References.Look<Pawn>(ref this.lastUser, "lasterUser", false);
			Scribe_Values.Look<string>(ref this.hackingStartedSignal, "hackingStartedSignal", null, false);
			Scribe_Values.Look<string>(ref this.hackingCompletedSignal, "hackingCompletedSignal", null, false);
		}

		// Token: 0x06006A1B RID: 27163 RVA: 0x0023B698 File Offset: 0x00239898
		public override string CompInspectStringExtra()
		{
			TaggedString taggedString = "HackProgress".Translate() + ": " + this.progress.ToStringWorkAmount() + " / " + this.defence.ToStringWorkAmount();
			if (this.IsHacked)
			{
				taggedString += " (" + "Hacked".Translate() + ")";
			}
			if (this.lastHackTick > Find.TickManager.TicksGame - 30)
			{
				string str;
				if (this.lastUser != null)
				{
					str = "HackingLastUser".Translate(this.lastUser) + " " + "HackingSpeed".Translate();
				}
				else
				{
					str = "HackingSpeed".Translate();
				}
				taggedString += "\n" + str + ": " + StatDefOf.HackingSpeed.ValueToString(this.lastUserSpeed, ToStringNumberSense.Absolute, true);
			}
			return taggedString;
		}

		// Token: 0x04003B32 RID: 15154
		public string hackingStartedSignal;

		// Token: 0x04003B33 RID: 15155
		public string hackingCompletedSignal;

		// Token: 0x04003B34 RID: 15156
		private float progress;

		// Token: 0x04003B35 RID: 15157
		public float defence;

		// Token: 0x04003B36 RID: 15158
		private float lastUserSpeed = 1f;

		// Token: 0x04003B37 RID: 15159
		private int lastHackTick = -1;

		// Token: 0x04003B38 RID: 15160
		private Pawn lastUser;

		// Token: 0x04003B39 RID: 15161
		public const string HackedSignal = "Hackend";

		// Token: 0x04003B3A RID: 15162
		private static List<Pawn> tmpAllowedPawns = new List<Pawn>();
	}
}
