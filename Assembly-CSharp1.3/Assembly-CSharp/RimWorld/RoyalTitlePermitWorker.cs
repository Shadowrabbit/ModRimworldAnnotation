using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE7 RID: 3559
	public class RoyalTitlePermitWorker
	{
		// Token: 0x0600526C RID: 21100 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<DiaOption> GetFactionCommDialogOptions(Map map, Pawn pawn, Faction factionInFavor)
		{
			return null;
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			return null;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x001BCB78 File Offset: 0x001BAD78
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

		// Token: 0x06005271 RID: 21105 RVA: 0x001BCBFC File Offset: 0x001BADFC
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

		// Token: 0x06005272 RID: 21106 RVA: 0x001BCCFC File Offset: 0x001BAEFC
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

		// Token: 0x040030AB RID: 12459
		public RoyalTitlePermitDef def;
	}
}
