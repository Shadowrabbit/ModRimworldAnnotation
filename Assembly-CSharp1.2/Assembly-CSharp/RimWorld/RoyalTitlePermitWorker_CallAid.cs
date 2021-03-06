using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145F RID: 5215
	public class RoyalTitlePermitWorker_CallAid : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x06007093 RID: 28819 RVA: 0x0004BDD4 File Offset: 0x00049FD4
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			string t;
			if (this.AidDisabled(map, pawn, faction, out t))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			if (NeutralGroupIncidentUtility.AnyBlockingHostileLord(pawn.MapHeld, faction))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "HostileVisitorsPresent".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield break;
			}
			Action action = null;
			string label = this.def.LabelCap + ": ";
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.BeginCallAid(pawn, map, faction, free, 1f);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		// Token: 0x06007094 RID: 28820 RVA: 0x002271BC File Offset: 0x002253BC
		private void BeginCallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			RoyalTitlePermitWorker_CallAid.<>c__DisplayClass3_0 CS$<>8__locals1 = new RoyalTitlePermitWorker_CallAid.<>c__DisplayClass3_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.caller = caller;
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.faction = faction;
			CS$<>8__locals1.free = free;
			CS$<>8__locals1.biocodeChance = biocodeChance;
			IEnumerable<Faction> source = from f in (from p in CS$<>8__locals1.map.mapPawns.AllPawnsSpawned
			where p.Faction != null && !p.Faction.IsPlayer && p.Faction != CS$<>8__locals1.faction
			select p.Faction).Distinct<Faction>()
			where f.HostileTo(Faction.OfPlayer) && !CS$<>8__locals1.faction.HostileTo(f)
			select f;
			if (source.Any<Faction>())
			{
				Find.WindowStack.Add(new Dialog_MessageBox("CommandCallRoyalAidWarningNonHostileFactions".Translate(CS$<>8__locals1.faction, (from f in source
				select f.NameColored.Resolve()).ToCommaList(false)), "Confirm".Translate(), new Action(CS$<>8__locals1.<BeginCallAid>g__Call|0), "GoBack".Translate(), null, null, false, null, null));
				return;
			}
			CS$<>8__locals1.<BeginCallAid>g__Call|0();
		}

		// Token: 0x06007095 RID: 28821 RVA: 0x0004BDF9 File Offset: 0x00049FF9
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallAid_NewTemp(this.caller, this.map, target.Cell, this.calledFaction, this.free, this.biocodeChance);
		}

		// Token: 0x06007096 RID: 28822 RVA: 0x0004BE26 File Offset: 0x0004A026
		[Obsolete]
		private void CallAid(Pawn caller, Map map, Faction faction, bool free, float biocodeChance = 1f)
		{
			this.CallAid_NewTemp(caller, map, caller.Position, faction, free, biocodeChance);
		}

		// Token: 0x06007097 RID: 28823 RVA: 0x002272EC File Offset: 0x002254EC
		private void CallAid_NewTemp(Pawn caller, Map map, IntVec3 spawnPos, Faction faction, bool free, float biocodeChance = 1f)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = map;
			incidentParms.faction = faction;
			incidentParms.raidArrivalModeForQuickMilitaryAid = true;
			incidentParms.biocodeApparelChance = biocodeChance;
			incidentParms.biocodeWeaponsChance = biocodeChance;
			incidentParms.spawnCenter = spawnPos;
			if (this.def.royalAid.pawnKindDef != null)
			{
				incidentParms.pawnKind = this.def.royalAid.pawnKindDef;
				incidentParms.pawnCount = this.def.royalAid.pawnCount;
			}
			else
			{
				incidentParms.points = (float)this.def.royalAid.points;
			}
			faction.lastMilitaryAidRequestTick = Find.TickManager.TicksGame;
			if (IncidentDefOf.RaidFriendly.Worker.TryExecute(incidentParms))
			{
				if (!free)
				{
					caller.royalty.TryRemoveFavor(faction, this.def.royalAid.favorCost);
				}
				caller.royalty.GetPermit(this.def, faction).Notify_Used();
				return;
			}
			Log.Error(string.Concat(new object[]
			{
				"Could not send aid to map ",
				map,
				" from faction ",
				faction
			}), false);
		}

		// Token: 0x04004A2B RID: 18987
		private Faction calledFaction;

		// Token: 0x04004A2C RID: 18988
		private float biocodeChance;
	}
}
