using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001153 RID: 4435
	public class QuestPart_Infestation : QuestPart
	{
		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x000430A0 File Offset: 0x000412A0
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				if (this.mapParent != null && this.mapParent.HasMap && this.loc.IsValid)
				{
					yield return new TargetInfo(this.loc, this.mapParent.Map, false);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x0600615C RID: 24924 RVA: 0x000430B0 File Offset: 0x000412B0
		public override string QuestSelectTargetsLabel
		{
			get
			{
				return "SelectHiveTargets".Translate();
			}
		}

		// Token: 0x17000F43 RID: 3907
		// (get) Token: 0x0600615D RID: 24925 RVA: 0x000430C1 File Offset: 0x000412C1
		public override IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestSelectTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					List<Thing> hives = this.mapParent.Map.listerThings.ThingsOfDef(ThingDefOf.Hive);
					int num;
					for (int i = 0; i < hives.Count; i = num + 1)
					{
						Hive hive;
						if ((hive = (hives[i] as Hive)) != null && !hive.questTags.NullOrEmpty<string>() && hive.questTags.Contains(this.tag))
						{
							yield return hive;
						}
						num = i;
					}
					hives = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600615E RID: 24926 RVA: 0x001E7A1C File Offset: 0x001E5C1C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.loc = IntVec3.Invalid;
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, this.mapParent.Map, true, true, this.tag);
					if (thing != null)
					{
						this.loc = thing.Position;
						if (this.sendStandardLetter)
						{
							TaggedString label = this.customLetterLabel.NullOrEmpty() ? IncidentDefOf.Infestation.letterLabel : this.customLetterLabel.Formatted(IncidentDefOf.Infestation.letterLabel.Named("BASELABEL"));
							TaggedString text = this.customLetterText.NullOrEmpty() ? IncidentDefOf.Infestation.letterText : this.customLetterText.Formatted(IncidentDefOf.Infestation.letterText.Named("BASETEXT"));
							Find.LetterStack.ReceiveLetter(label, text, this.customLetterDef ?? IncidentDefOf.Infestation.letterDef, new TargetInfo(this.loc, this.mapParent.Map, false), null, this.quest, null, null);
						}
					}
				}
			}
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x001E7B6C File Offset: 0x001E5D6C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.hivesCount, "hivesCount", 0, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<IntVec3>(ref this.loc, "loc", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x000430D1 File Offset: 0x000412D1
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.hivesCount = 5;
			}
		}

		// Token: 0x0400410D RID: 16653
		public string inSignal;

		// Token: 0x0400410E RID: 16654
		public int hivesCount;

		// Token: 0x0400410F RID: 16655
		public MapParent mapParent;

		// Token: 0x04004110 RID: 16656
		public string tag;

		// Token: 0x04004111 RID: 16657
		public string customLetterText;

		// Token: 0x04004112 RID: 16658
		public string customLetterLabel;

		// Token: 0x04004113 RID: 16659
		public LetterDef customLetterDef;

		// Token: 0x04004114 RID: 16660
		public bool sendStandardLetter = true;

		// Token: 0x04004115 RID: 16661
		private IntVec3 loc = IntVec3.Invalid;
	}
}
