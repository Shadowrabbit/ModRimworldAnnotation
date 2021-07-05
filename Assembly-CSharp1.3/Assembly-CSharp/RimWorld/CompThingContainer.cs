using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020011BA RID: 4538
	public class CompThingContainer : ThingComp, IThingHolder
	{
		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x06006D49 RID: 27977 RVA: 0x0024A15F File Offset: 0x0024835F
		public CompProperties_ThingContainer Props
		{
			get
			{
				return (CompProperties_ThingContainer)this.props;
			}
		}

		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06006D4A RID: 27978 RVA: 0x0024A16C File Offset: 0x0024836C
		public Thing ContainedThing
		{
			get
			{
				if (!this.innerContainer.Any)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06006D4B RID: 27979 RVA: 0x0024A189 File Offset: 0x00248389
		public bool Empty
		{
			get
			{
				return this.ContainedThing == null;
			}
		}

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06006D4C RID: 27980 RVA: 0x0024A194 File Offset: 0x00248394
		public bool Full
		{
			get
			{
				return !this.Empty && this.ContainedThing.stackCount >= this.Props.stackLimit;
			}
		}

		// Token: 0x06006D4D RID: 27981 RVA: 0x0024A1BB File Offset: 0x002483BB
		public CompThingContainer()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x06006D4E RID: 27982 RVA: 0x0024A1CF File Offset: 0x002483CF
		public virtual bool Accepts(ThingDef thingDef)
		{
			return this.Empty || (this.ContainedThing.stackCount < this.Props.stackLimit && thingDef == this.ContainedThing.def);
		}

		// Token: 0x06006D4F RID: 27983 RVA: 0x0024A203 File Offset: 0x00248403
		public virtual bool Accepts(Thing thing)
		{
			return this.Accepts(thing.def);
		}

		// Token: 0x06006D50 RID: 27984 RVA: 0x0024A214 File Offset: 0x00248414
		public override void PostDraw()
		{
			if (!this.Empty)
			{
				this.ContainedThing.DrawAt((this.parent.Position + this.Props.containedThingOffset.RotatedBy(this.parent.Rotation)).ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop), false);
			}
		}

		// Token: 0x06006D51 RID: 27985 RVA: 0x0024A26C File Offset: 0x0024846C
		public override void DrawGUIOverlay()
		{
			if (this.parent.Spawned && this.Props.drawStackLabel && !this.Empty && Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				GenMapUI.DrawThingLabel(this.parent, this.ContainedThing.stackCount.ToStringCached());
			}
		}

		// Token: 0x06006D52 RID: 27986 RVA: 0x0024A2C2 File Offset: 0x002484C2
		public override void PostDeSpawn(Map map)
		{
			this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x06006D53 RID: 27987 RVA: 0x0024A2E0 File Offset: 0x002484E0
		public override void CompTick()
		{
			this.innerContainer.ThingOwnerTick(true);
		}

		// Token: 0x06006D54 RID: 27988 RVA: 0x0024A2F0 File Offset: 0x002484F0
		public override string CompInspectStringExtra()
		{
			return "Contents".Translate() + ": " + (this.Empty ? "Nothing".Translate() : this.ContainedThing.LabelCap);
		}

		// Token: 0x06006D55 RID: 27989 RVA: 0x0024A33F File Offset: 0x0024853F
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (!this.Empty)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "Contents".Translate(), this.ContainedThing.LabelCap, this.ContainedThing.LabelCap, 1200, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.ContainedThing, -1)), false);
			}
			yield break;
		}

		// Token: 0x06006D56 RID: 27990 RVA: 0x0024A34F File Offset: 0x0024854F
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06006D57 RID: 27991 RVA: 0x0024A35D File Offset: 0x0024855D
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06006D58 RID: 27992 RVA: 0x0024A365 File Offset: 0x00248565
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x04003CB7 RID: 15543
		public ThingOwner innerContainer;
	}
}
