using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F76 RID: 3958
	public class RitualOutcomeEffectWorker_ConnectToTree : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06005DD7 RID: 24023 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005DD8 RID: 24024 RVA: 0x002019DD File Offset: 0x001FFBDD
		public RitualOutcomeEffectWorker_ConnectToTree()
		{
		}

		// Token: 0x06005DD9 RID: 24025 RVA: 0x002019E5 File Offset: 0x001FFBE5
		public RitualOutcomeEffectWorker_ConnectToTree(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x06005DDA RID: 24026 RVA: 0x002034D4 File Offset: 0x002016D4
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			Thing thing = jobRitual.selectedTarget.Thing;
			float quality = base.GetQuality(jobRitual, progress);
			int num = Mathf.Max(1, Mathf.RoundToInt(quality * 50f));
			CompSpawnSubplantDuration compSpawnSubplantDuration = thing.TryGetComp<CompSpawnSubplantDuration>();
			if (compSpawnSubplantDuration != null)
			{
				ThingDef subplant = compSpawnSubplantDuration.Props.subplant;
				foreach (Pawn pawn in totalPresence.Keys)
				{
					for (int i = 0; i < num; i++)
					{
						compSpawnSubplantDuration.DoGrowSubplant(true);
					}
				}
				compSpawnSubplantDuration.SetupNextSubplantTick();
			}
			Pawn pawn2 = jobRitual.PawnWithRole("connector");
			CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
			if (pawn2 != null && compTreeConnection != null)
			{
				compTreeConnection.ConnectToPawn(pawn2);
				Find.LetterStack.ReceiveLetter("LetterLabelPawnConnected".Translate(thing.Named("TREE")), "LetterTextPawnConnected".Translate(thing.Named("TREE"), pawn2.Named("CONNECTOR")), LetterDefOf.RitualOutcomePositive, pawn2, null, null, new List<ThingDef>
				{
					thing.def
				}, null);
			}
		}

		// Token: 0x0400362F RID: 13871
		private const float NumMossPerQuality = 50f;
	}
}
