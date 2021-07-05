using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E4 RID: 5348
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x06007F6D RID: 32621 RVA: 0x002D0995 File Offset: 0x002CEB95
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06007F6E RID: 32622 RVA: 0x002D09A4 File Offset: 0x002CEBA4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					val *= room.GetStat(this.roomStat);
				}
			}
		}

		// Token: 0x06007F6F RID: 32623 RVA: 0x002D09E0 File Offset: 0x002CEBE0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
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

		// Token: 0x04004F94 RID: 20372
		private RoomStatDef roomStat;

		// Token: 0x04004F95 RID: 20373
		[MustTranslate]
		private string customLabel;

		// Token: 0x04004F96 RID: 20374
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;
	}
}
