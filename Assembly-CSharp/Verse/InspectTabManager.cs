using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000A0 RID: 160
	public static class InspectTabManager
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x0008B4CC File Offset: 0x000896CC
		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase;
			if (InspectTabManager.sharedInstances.TryGetValue(tabType, out inspectTabBase))
			{
				return inspectTabBase;
			}
			inspectTabBase = (InspectTabBase)Activator.CreateInstance(tabType);
			InspectTabManager.sharedInstances.Add(tabType, inspectTabBase);
			return inspectTabBase;
		}

		// Token: 0x0400029D RID: 669
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();
	}
}
