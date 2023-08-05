#region Namespaces
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


#endregion

namespace RAA_M02C
{
    [Transaction(TransactionMode.Manual)]
    public class Module02Challenge : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // User selection 
            TaskDialog.Show("Selection", "Select lines to reveal the hidden message.");
            IList<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select some elements: ");
            List<CurveElement> curveList = new List<CurveElement>();

            // Get model elements 
            //Walls
            WallType curWallTypeGlaz = Utils.GetWallTypeByName(doc, "Storefront");
            WallType curWallTypeGeneric = Utils.GetWallTypeByName(doc, @"Generic - 8""");
            Level curLevel = Utils.GetLevelByName(doc, "Level 1");

            //Pipe
            MEPSystemType curSystemTypePipe = Utils.GetMEPSystemTypeByName(doc, "Domestic Hot Water");
            PipeType curPipeType = Utils.GetPipeTypeByName(doc, "Default");

            //Duct
            MEPSystemType curSystemTypeDuct = Utils.GetMEPSystemTypeByName(doc, "Supply Air");
            DuctType curDuctType = Utils.GetDuctTypeByName(doc, "Default");

            int counterWall = 0;
            int counterDuct = 0;
            int counterPipe = 0;
            int counterCW = 0;

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Hidden Message");

                List<ElementId> id_lines = new List<ElementId>();

              
                foreach (Element element in pickList)
                {
                    //Get id lines
                    id_lines.Add(element.Id);

                    if (element is CurveElement)
                    {

                        //Convert element 
                        CurveElement curve = (CurveElement)element;

                        curveList.Add(curve);


                        GraphicsStyle curGS = curve.LineStyle as GraphicsStyle;
                        Curve curCurve = curve.GeometryCurve;

                        //Switch statement for creating model elements
                        switch (curGS.Name)
                        {
                            case "A-GLAZ":
                                Wall newWallGlaz = Wall.Create(doc, curCurve, curWallTypeGlaz.Id, curLevel.Id, 15, 0, false, false);
                                counterCW++;
                                break;

                            case "A-WALL":
                                Wall newWallGeneric = Wall.Create(doc, curCurve, curWallTypeGeneric.Id, curLevel.Id, 15, 0, false, false);
                                counterWall++;
                                break;

                            case "M-DUCT":
                                XYZ startPoint = curCurve.GetEndPoint(0);
                                XYZ endPoint = curCurve.GetEndPoint(1);
                                Duct newDuct = Duct.Create(doc, curSystemTypeDuct.Id, curDuctType.Id, curLevel.Id, startPoint, endPoint);
                                counterDuct++;
                                break;

                            case "P-PIPE":
                                XYZ startPointP = curCurve.GetEndPoint(0);
                                XYZ endPointP = curCurve.GetEndPoint(1);
                                Pipe newPipe = Pipe.Create(doc, curSystemTypePipe.Id, curPipeType.Id, curLevel.Id, startPointP, endPointP);
                                counterPipe++;
                                break;

                            default:
                                break;
                        }
                    }
                }

                //Delete model lines
                doc.Delete(id_lines);

                t.Commit();
            }

            //Alert user
            TaskDialog.Show("complete", $"{curveList.Count} lines deleted. {counterWall} generic walls, {counterDuct} ducts, {counterPipe} pipes, {counterCW} curtain walls created.");

            return Result.Succeeded;
        }
    }
}
