using System;
using System.Reflection;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001F1F RID: 7967
	public class QuestNode_GetFieldValue : QuestNode
	{
		// Token: 0x0600AA68 RID: 43624 RVA: 0x0006FA58 File Offset: 0x0006DC58
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600AA69 RID: 43625 RVA: 0x0006FA62 File Offset: 0x0006DC62
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600AA6A RID: 43626 RVA: 0x0031B974 File Offset: 0x00319B74
		private void SetVars(Slate slate)
		{
			object obj = (this.type.GetValue(slate) != null) ? ConvertHelper.Convert(this.obj.GetValue(slate), this.type.GetValue(slate)) : this.obj.GetValue(slate);
			FieldInfo fieldInfo = obj.GetType().GetField(this.field.GetValue(slate), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fieldInfo == null)
			{
				Log.Error("QuestNode error: " + obj.GetType().Name + " doesn't have a field named " + this.field.GetValue(slate), false);
				return;
			}
			slate.Set<object>(this.storeAs.GetValue(slate), fieldInfo.GetValue(obj), false);
		}

		// Token: 0x040073CA RID: 29642
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040073CB RID: 29643
		[NoTranslate]
		public SlateRef<object> obj;

		// Token: 0x040073CC RID: 29644
		[NoTranslate]
		public SlateRef<string> field;

		// Token: 0x040073CD RID: 29645
		public SlateRef<Type> type;
	}
}
