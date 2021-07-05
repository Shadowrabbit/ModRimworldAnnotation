using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011AC RID: 4524
	public class CompStudiable : ThingComp
	{
		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06006CF4 RID: 27892 RVA: 0x002491AD File Offset: 0x002473AD
		public CompProperties_Studiable Props
		{
			get
			{
				return (CompProperties_Studiable)this.props;
			}
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06006CF5 RID: 27893 RVA: 0x002491BA File Offset: 0x002473BA
		public float ProgressPercent
		{
			get
			{
				return this.progress / (float)this.cost;
			}
		}

		// Token: 0x170012E2 RID: 4834
		// (get) Token: 0x06006CF6 RID: 27894 RVA: 0x002491CA File Offset: 0x002473CA
		public bool Completed
		{
			get
			{
				return this.progress >= (float)this.cost;
			}
		}

		// Token: 0x06006CF7 RID: 27895 RVA: 0x002491DE File Offset: 0x002473DE
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.cost = this.Props.cost;
		}

		// Token: 0x06006CF8 RID: 27896 RVA: 0x002491F8 File Offset: 0x002473F8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("CompStudiable"))
			{
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06006CF9 RID: 27897 RVA: 0x00249210 File Offset: 0x00247410
		public void DoResearch(float amount)
		{
			bool completed = this.Completed;
			amount *= 0.00825f;
			amount *= Find.Storyteller.difficulty.researchSpeedFactor;
			this.progress += amount;
			this.progress = Mathf.Clamp(this.progress, 0f, (float)this.cost);
			if (!completed && this.Completed)
			{
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Researched", this.parent.Named("SUBJECT"));
			}
		}

		// Token: 0x06006CFA RID: 27898 RVA: 0x00249299 File Offset: 0x00247499
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode && !this.Completed)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Complete study",
					action = delegate()
					{
						int num = 100;
						while (!this.Completed && num > 0)
						{
							this.DoResearch(float.MaxValue);
							num--;
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x06006CFB RID: 27899 RVA: 0x002492A9 File Offset: 0x002474A9
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetRelatedQuestHyperlinks()
		{
			List<Quest> quests = Find.QuestManager.QuestsListForReading;
			int num;
			for (int i = 0; i < quests.Count; i = num + 1)
			{
				if (!quests[i].hidden && (quests[i].State == QuestState.Ongoing || quests[i].State == QuestState.NotYetAccepted))
				{
					List<QuestPart> partsListForReading = quests[i].PartsListForReading;
					for (int j = 0; j < partsListForReading.Count; j++)
					{
						QuestPart_RequirementsToAcceptThingStudied questPart_RequirementsToAcceptThingStudied;
						if ((questPart_RequirementsToAcceptThingStudied = (partsListForReading[j] as QuestPart_RequirementsToAcceptThingStudied)) != null && questPart_RequirementsToAcceptThingStudied.thing == this.parent)
						{
							yield return new Dialog_InfoCard.Hyperlink(quests[i], -1);
							break;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06006CFC RID: 27900 RVA: 0x002492B9 File Offset: 0x002474B9
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			IEnumerable<StatDrawEntry> enumerable = base.SpecialDisplayStats();
			if (enumerable != null)
			{
				foreach (StatDrawEntry statDrawEntry in enumerable)
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Researched".Translate(), this.ProgressPercent.ToStringPercent(), "Stat_Studiable_Desc".Translate(), 3000, null, this.GetRelatedQuestHyperlinks(), false);
			yield break;
			yield break;
		}

		// Token: 0x06006CFD RID: 27901 RVA: 0x002492C9 File Offset: 0x002474C9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
			Scribe_Values.Look<int>(ref this.cost, "costf", 0, false);
		}

		// Token: 0x06006CFE RID: 27902 RVA: 0x002492FC File Offset: 0x002474FC
		public override string CompInspectStringExtra()
		{
			TaggedString taggedString = "StudyProgress".Translate() + ": " + Mathf.RoundToInt(this.progress).ToString() + " / " + this.cost.ToString();
			if (this.Completed)
			{
				taggedString += " (" + "StudyCompleted".Translate() + ")";
			}
			return taggedString;
		}

		// Token: 0x04003C9E RID: 15518
		private float progress;

		// Token: 0x04003C9F RID: 15519
		public int cost;
	}
}
