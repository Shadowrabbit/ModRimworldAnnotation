using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCA RID: 3018
	public class QuestPart_Infestation : QuestPart
	{
		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x060046AB RID: 18091 RVA: 0x00175C2E File Offset: 0x00173E2E
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

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x060046AC RID: 18092 RVA: 0x00175C3E File Offset: 0x00173E3E
		public override string QuestSelectTargetsLabel
		{
			get
			{
				return "SelectHiveTargets".Translate();
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x060046AD RID: 18093 RVA: 0x00175C4F File Offset: 0x00173E4F
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

		// Token: 0x060046AE RID: 18094 RVA: 0x00175C60 File Offset: 0x00173E60
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.loc = IntVec3.Invalid;
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, this.mapParent.Map, true, true, this.tag, this.overrideLoc, null);
					if (thing != null)
					{
						this.loc = (this.overrideLoc ?? thing.Position);
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

		// Token: 0x060046AF RID: 18095 RVA: 0x00175DD8 File Offset: 0x00173FD8
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
			Scribe_Values.Look<IntVec3?>(ref this.overrideLoc, "overrideLoc", null, false);
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x00175EAC File Offset: 0x001740AC
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

		// Token: 0x04002B1C RID: 11036
		public string inSignal;

		// Token: 0x04002B1D RID: 11037
		public int hivesCount;

		// Token: 0x04002B1E RID: 11038
		public MapParent mapParent;

		// Token: 0x04002B1F RID: 11039
		public string tag;

		// Token: 0x04002B20 RID: 11040
		public IntVec3? overrideLoc;

		// Token: 0x04002B21 RID: 11041
		public string customLetterText;

		// Token: 0x04002B22 RID: 11042
		public string customLetterLabel;

		// Token: 0x04002B23 RID: 11043
		public LetterDef customLetterDef;

		// Token: 0x04002B24 RID: 11044
		public bool sendStandardLetter = true;

		// Token: 0x04002B25 RID: 11045
		private IntVec3 loc = IntVec3.Invalid;
	}
}
