using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200038B RID: 907
	public abstract class ThingComp
	{
		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001A87 RID: 6791 RVA: 0x00099D6B File Offset: 0x00097F6B
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x06001A88 RID: 6792 RVA: 0x00099D78 File Offset: 0x00097F78
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06001A89 RID: 6793 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06001A8B RID: 6795 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x06001A8C RID: 6796 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDeSpawn(Map map)
		{
		}

		// Token: 0x06001A8D RID: 6797 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostPostMake()
		{
		}

		// Token: 0x06001A8F RID: 6799 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompTick()
		{
		}

		// Token: 0x06001A90 RID: 6800 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompTickRare()
		{
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompTickLong()
		{
		}

		// Token: 0x06001A92 RID: 6802 RVA: 0x0009511A File Offset: 0x0009331A
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06001A93 RID: 6803 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDraw()
		{
		}

		// Token: 0x06001A95 RID: 6805 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x000210E7 File Offset: 0x0001F2E7
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x00099D81 File Offset: 0x00097F81
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x00099D8A File Offset: 0x00097F8A
		public virtual IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			yield break;
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00002688 File Offset: 0x00000888
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00099D93 File Offset: 0x00097F93
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x00099D9C File Offset: 0x00097F9C
		public virtual IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			yield break;
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x06001AA4 RID: 6820 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06001AA5 RID: 6821 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		// Token: 0x06001AA6 RID: 6822 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void DrawGUIOverlay()
		{
		}

		// Token: 0x06001AA9 RID: 6825 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x06001AAA RID: 6826 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x06001AAB RID: 6827 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_Unequipped(Pawn pawn)
		{
		}

		// Token: 0x06001AAC RID: 6828 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x06001AAD RID: 6829 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_KilledPawn(Pawn pawn)
		{
		}

		// Token: 0x06001AAE RID: 6830 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void Notify_AddBedThoughts(Pawn pawn)
		{
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x00099DA8 File Offset: 0x00097FA8
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent != null) ? this.parent.Position : IntVec3.Invalid,
				")"
			});
		}

		// Token: 0x04001139 RID: 4409
		public ThingWithComps parent;

		// Token: 0x0400113A RID: 4410
		public CompProperties props;
	}
}
