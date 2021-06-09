using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200145E RID: 5214
	public class RoyalTitlePermitWorker
	{
		// Token: 0x0600708B RID: 28811 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600708C RID: 28812 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600708D RID: 28813 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<DiaOption> GetFactionCommDialogOptions(Map map, Pawn pawn, Faction factionInFavor)
		{
			return null;
		}

		// Token: 0x0600708E RID: 28814 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600708F RID: 28815 RVA: 0x00226EEC File Offset: 0x002250EC
		protected virtual bool AidDisabled(Map map, Pawn pawn, Faction faction, out string reason)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				reason = "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION"));
				return true;
			}
			if (!faction.def.allowedArrivalTemperatureRange.ExpandedBy(-4f).Includes(pawn.MapHeld.mapTemperature.SeasonalTemp))
			{
				reason = "BadTemperature".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x06007090 RID: 28816 RVA: 0x00226F70 File Offset: 0x00225170
		protected bool FillAidOption(Pawn pawn, Faction faction, ref string description, out bool free)
		{
			int lastUsedTick = pawn.royalty.GetPermit(this.def, faction).LastUsedTick;
			int num = Math.Max(GenTicks.TicksGame - lastUsedTick, 0);
			if (lastUsedTick < 0 || num >= this.def.CooldownTicks)
			{
				description += "CommandCallRoyalAidFreeOption".Translate();
				free = true;
				return true;
			}
			int numTicks = (lastUsedTick > 0) ? Math.Max(this.def.CooldownTicks - num, 0) : 0;
			description += "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), this.def.royalAid.favorCost, faction.Named("FACTION"));
			if (pawn.royalty.GetFavor(faction) >= this.def.royalAid.favorCost)
			{
				free = false;
				return true;
			}
			free = false;
			return false;
		}

		// Token: 0x06007091 RID: 28817 RVA: 0x00227070 File Offset: 0x00225270
		protected bool FillCaravanAidOption(Pawn pawn, Faction faction, out string description, out bool free, out bool disableNotEnoughFavor)
		{
			description = this.def.description;
			disableNotEnoughFavor = false;
			free = false;
			if (!this.def.usableOnWorldMap)
			{
				free = false;
				return false;
			}
			int lastUsedTick = pawn.royalty.GetPermit(this.def, faction).LastUsedTick;
			int num = Math.Max(GenTicks.TicksGame - lastUsedTick, 0);
			if (lastUsedTick < 0 || num >= this.def.CooldownTicks)
			{
				description += " (" + "CommandCallRoyalAidFreeOption".Translate() + ")";
				free = true;
			}
			else
			{
				int numTicks = (lastUsedTick > 0) ? Math.Max(this.def.CooldownTicks - num, 0) : 0;
				description += " (" + "CommandCallRoyalAidFavorOption".Translate(numTicks.TicksToDays().ToString("0.0"), this.def.royalAid.favorCost, faction.Named("FACTION")) + ")";
				if (pawn.royalty.GetFavor(faction) < this.def.royalAid.favorCost)
				{
					disableNotEnoughFavor = true;
				}
			}
			return true;
		}

		// Token: 0x04004A2A RID: 18986
		public RoyalTitlePermitDef def;
	}
}
