using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//

using System.Linq;
using System.Text;
using System.IO;

using View = Autodesk.Revit.DB.View;


namespace RAA_M02C
{
    internal static class Utils
    {


        //Get level by name
        public static Level GetLevelByName(Document curDoc, string levelName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(curDoc);
            collector.OfClass(typeof(Level));

            foreach(Level curLvl in collector)
            {
                if (curLvl.Name == levelName)
                    return curLvl;
            }
            return null;
        }

        //Get wall type by name
        public static WallType GetWallTypeByName(Document curDoc, string wallTypeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(curDoc);
            collector.OfClass(typeof(WallType));

            foreach (WallType curWT in collector)
            {
                if (curWT.Name == wallTypeName)
                    return curWT;
            }
            return null;

        }

        //Get system type by name
        public static MEPSystemType GetMEPSystemTypeByName(Document curDoc, string mepSystemType)
        {
            FilteredElementCollector collector = new FilteredElementCollector(curDoc);
            collector.OfClass(typeof(MEPSystemType));

            foreach(MEPSystemType curMEPSystemType in collector)
            {
                if(curMEPSystemType.Name == mepSystemType)
                    return curMEPSystemType;
            }
            return null;

        }

        //Create pipe
        public static PipeType GetPipeTypeByName(Document curDoc, string pipeTypeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(curDoc);
            collector.OfClass(typeof(PipeType));

            foreach (PipeType curPipeType in collector)
            {
                if (curPipeType.Name == pipeTypeName)
                    return curPipeType;
            }
            return null;
        }

        //Create duct
        public static DuctType GetDuctTypeByName(Document curDoc, string ductTypeName)
        {
            FilteredElementCollector collector = new FilteredElementCollector(curDoc);
            collector.OfClass(typeof(DuctType));

            foreach(DuctType curDuctType in collector)
            {
                if (curDuctType.Name == ductTypeName)
                    return curDuctType;  
            }
            return null;
        }

    }
}
