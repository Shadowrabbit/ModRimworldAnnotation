using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001680 RID: 5760
	public class QuestNode_GetPawnKind : QuestNode
	{
		// Token: 0x0600860F RID: 34319 RVA: 0x0030190A File Offset: 0x002FFB0A
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x00301914 File Offset: 0x002FFB14
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06008611 RID: 34321 RVA: 0x00301924 File Offset: 0x002FFB24
		private void SetVars(Slate slate)
		{
			QuestNode_GetPawnKind.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetPawnKind.Option x) => x.weight);
			PawnKindDef var;
			if (option.kindDef != null)
			{
				var = option.kindDef;
			}
			else if (option.anyAnimal)
			{
				int highestAnimalSkill = option.mustBeAbleToHandleAnimal ? 0 : int.MaxValue;
				if (option.mustBeAbleToHandleAnimal)
				{
					Map map = slate.Get<Map>("map", null, false);
					if (map != null)
					{
						using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Pawn pawn = enumerator.Current;
								highestAnimalSkill = Mathf.Max(highestAnimalSkill, pawn.skills.GetSkill(SkillDefOf.Animals).Level);
							}
							goto IL_171;
						}
					}
					foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists)
					{
						highestAnimalSkill = Mathf.Max(highestAnimalSkill, pawn2.skills.GetSkill(SkillDefOf.Animals).Level);
					}
				}
				IL_171:
				var = (from x in DefDatabase<PawnKindDef>.AllDefs
				where x.RaceProps.Animal && (option.onlyAllowedFleshType == null || x.RaceProps.FleshType == option.onlyAllowedFleshType) && base.<SetVars>g__CanHandle|1(x)
				select x).RandomElementWithFallback(null);
			}
			else
			{
				var = null;
			}
			slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x040053E5 RID: 21477
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040053E6 RID: 21478
		public SlateRef<List<QuestNode_GetPawnKind.Option>> options;

		// Token: 0x02002929 RID: 10537
		public class Option
		{
			// Token: 0x04009B04 RID: 39684
			public PawnKindDef kindDef;

			// Token: 0x04009B05 RID: 39685
			public float weight;

			// Token: 0x04009B06 RID: 39686
			public bool anyAnimal;

			// Token: 0x04009B07 RID: 39687
			public bool mustBeAbleToHandleAnimal;

			// Token: 0x04009B08 RID: 39688
			public FleshTypeDef onlyAllowedFleshType;
		}
	}
}
