
#region ================== Copyright (c) 2007 Pascal vd Heiden

/*
 * Copyright (c) 2007 Pascal vd Heiden, www.codeimp.com
 * This program is released under GNU General Public License
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 */

#endregion

#region ================== Namespaces

using System.Collections.Generic;
using mxd.DukeBuilder.Map;
using mxd.DukeBuilder.Rendering;

#endregion

namespace mxd.DukeBuilder.EditModes
{
	[FindReplace("Sector Index", BrowseButton = false, Replacable = false)]
	internal class FindSectorNumber : FindReplaceType
	{
		#region ================== Constants

		#endregion

		#region ================== Variables

		#endregion

		#region ================== Properties

		#endregion

		#region ================== Constructor / Destructor

		// Constructor
		public FindSectorNumber()
		{
			// Initialize

		}

		// Destructor
		~FindSectorNumber()
		{
		}

		#endregion

		#region ================== Methods

		// This is called when the browse button is pressed
		public override string Browse(string initialvalue)
		{
			return "";
		}


		// This is called to perform a search (and replace)
		// Returns a list of items to show in the results list
		// replacewith is null when not replacing
		public override FindReplaceObject[] Find(string value, bool withinselection, string replacewith, bool keepselection)
		{
			List<FindReplaceObject> objs = new List<FindReplaceObject>();

			// Interpret the number given
			int index;
			if(int.TryParse(value, out index))
			{
				Sector s = General.Map.Map.GetSectorByIndex(index);
				if(s != null)
				{
					if(General.Map.Config.SectorEffects.ContainsKey(s.LoTag))
						objs.Add(new FindReplaceObject(s, "Sector " + s.Index + " (" + General.Map.Config.SectorEffects[s.LoTag].Title + ")"));
					else
						objs.Add(new FindReplaceObject(s, "Sector " + s.Index));
				}
			}

			return objs.ToArray();
		}

		// This is called when a specific object is selected from the list
		public override void ObjectSelected(FindReplaceObject[] selection)
		{
			if(selection.Length == 1)
			{
				ZoomToSelection(selection);
				General.Interface.ShowInfo(selection[0].Sector);
			}
			else
				General.Interface.HideInfo();

			General.Map.Map.ClearAllSelected();
			foreach(FindReplaceObject obj in selection) obj.Sector.Selected = true;
		}

		// Render selection
		public override void PlotSelection(IRenderer2D renderer, FindReplaceObject[] selection)
		{
			foreach(FindReplaceObject o in selection)
			{
				foreach(Sidedef sd in o.Sector.Sidedefs)
				{
					renderer.PlotLinedef(sd.Line, General.Colors.Selection);
				}
			}
		}

		// Edit objects
		public override void EditObjects(FindReplaceObject[] selection)
		{
			List<Sector> sectors = new List<Sector>(selection.Length);
			foreach(FindReplaceObject o in selection) sectors.Add(o.Sector);
			General.Interface.ShowEditSectors(sectors);
		}

		#endregion
	}
}
