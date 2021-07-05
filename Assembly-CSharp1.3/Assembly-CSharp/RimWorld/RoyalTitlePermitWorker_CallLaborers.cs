using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE9 RID: 3561
	public class RoyalTitlePermitWorker_CallLaborers : RoyalTitlePermitWorker
	{
		// Token: 0x06005279 RID: 21113 RVA: 0x001BD0F6 File Offset: 0x001BB2F6
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			string t;
			if (this.AidDisabled(map, pawn, faction, out t))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			Action action = null;
			string label = this.def.LabelCap + " (" + "CommandCallLaborersNumLaborers".Translate(this.def.royalAid.pawnCount) + "): ";
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.CallLaborers(pawn, map, faction, free);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x0600527A RID: 21114 RVA: 0x001BD11C File Offset: 0x001BB31C
		private void CallLaborers(Pawn pawn, Map map, Faction faction, bool free)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				return;
			}
			QuestScriptDef permit_CallLaborers = QuestScriptDefOf.Permit_CallLaborers;
			Slate slate = new Slate();
			slate.Set<Map>("map", map, false);
			slate.Set<int>("laborersCount", this.def.royalAid.pawnCount, false);
			slate.Set<Faction>("permitFaction", faction, false);
			slate.Set<PawnKindDef>("laborersPawnKind", this.def.royalAid.pawnKindDef, false);
			slate.Set<float>("laborersDurationDays", this.def.royalAid.aidDurationDays, false);
			QuestUtility.GenerateQuestAndMakeAvailable(permit_CallLaborers, slate);
			pawn.royalty.GetPermit(this.def, faction).Notify_Used();
			if (!free)
			{
				pawn.royalty.TryRemoveFavor(faction, this.def.royalAid.favorCost);
			}
		}
	}
}
