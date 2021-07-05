using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D49 RID: 7497
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x0600A2DD RID: 41693 RVA: 0x0006C291 File Offset: 0x0006A491
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x0600A2DE RID: 41694 RVA: 0x002F642C File Offset: 0x002F462C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					val *= room.GetStat(this.roomStat);
				}
			}
		}

		// Token: 0x0600A2DF RID: 41695 RVA: 0x002F6464 File Offset: 0x002F4664
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					string str;
					if (!this.customLabel.NullOrEmpty())
					{
						str = this.customLabel;
					}
					else
					{
						str = this.roomStat.LabelCap;
					}
					return str + ": x" + room.GetStat(this.roomStat).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x04006E9B RID: 28315
		private RoomStatDef roomStat;

		// Token: 0x04006E9C RID: 28316
		[MustTranslate]
		private string customLabel;

		// Token: 0x04006E9D RID: 28317
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;
	}
}
