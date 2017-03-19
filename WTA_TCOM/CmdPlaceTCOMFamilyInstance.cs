#region Header
//
// based on examples from BuildingCoder Jeremy Tammik,
// AKS 6/27/2016
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;
using Keys = System.Windows.Forms.Keys;
using Autodesk.Revit.UI.Selection;
using System.Text;
using System.Runtime.InteropServices;



#endregion // Namespaces

namespace WTA_TCOM {
    [Transaction(TransactionMode.Manual)]
    class CmdPlaceTComDrop2DHInstance : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "TCOM";
            string FamilyName = "T-COM DROP-WTA";
            string FamilySymbolName = "DROP";
            string pName = "TCOM - INSTANCE";
            string pNameVal = "2D";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagNameSymb = "T-COMM INSTANCE";
            bool oneShot = false;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_CommunicationDevices;
            Element elemPlunked;

            plunkThis.PlunkThisFamilyWithThisTagWithThisParameterSet(FamilyName, FamilySymbolName,
                pName, pNameVal, wsName, FamilyTagName, FamilyTagNameSymb, bicTagBeing, bicFamily, out elemPlunked, oneShot, hasLeader);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceTComDrop4DHInstance : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "TCOM";
            string FamilyName = "T-COM DROP-WTA";
            string FamilySymbolName = "DROP";
            string pName = "TCOM - INSTANCE";
            string pNameVal = "4D";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagNameSymb = "T-COMM INSTANCE";
            bool oneShot = false;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_CommunicationDevices;
            Element elemPlunked;

            plunkThis.PlunkThisFamilyWithThisTagWithThisParameterSet(FamilyName, FamilySymbolName,
                pName, pNameVal, wsName, FamilyTagName, FamilyTagNameSymb, bicTagBeing, bicFamily, out elemPlunked, oneShot, hasLeader);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceTComDropAPHInstance : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "TCOM";
            string FamilyName = "T-COM DROP-WTA";
            string FamilySymbolName = "DROP";
            string pName = "TCOM - INSTANCE";
            string pNameVal = "AP";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagNameSymb = "T-COMM INSTANCE";
            bool oneShot = false;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_CommunicationDevices;
            Element elemPlunked;

            plunkThis.PlunkThisFamilyWithThisTagWithThisParameterSet(FamilyName, FamilySymbolName,
                pName, pNameVal, wsName, FamilyTagName, FamilyTagNameSymb, bicTagBeing, bicFamily, out elemPlunked, oneShot, hasLeader);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceTComDrop2DNHInstance : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "TCOM";
            string FamilyName = "T-COM DROP-NH-WTA";
            string FamilySymbolName = "DROP";
            string pName = "TCOM - INSTANCE";
            string pNameVal = "2D";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagNameSymb = "T-COMM INSTANCE";
            bool oneShot = false;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_CommunicationDevices;
            Element elemPlunked;

            plunkThis.PlunkThisFamilyWithThisTagWithThisParameterSet(FamilyName, FamilySymbolName,
                pName, pNameVal, wsName, FamilyTagName, FamilyTagNameSymb, bicTagBeing, bicFamily, out elemPlunked, oneShot, hasLeader);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceTComDrop4DNHInstance : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "TCOM";
            string FamilyName = "T-COM DROP-NH-WTA";
            string FamilySymbolName = "DROP";
            string pName = "TCOM - INSTANCE";
            string pNameVal = "4D";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagNameSymb = "T-COMM INSTANCE";
            bool oneShot = false;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_CommunicationDevices;
            Element elemPlunked;

            plunkThis.PlunkThisFamilyWithThisTagWithThisParameterSet(FamilyName, FamilySymbolName,
                pName, pNameVal, wsName, FamilyTagName, FamilyTagNameSymb, bicTagBeing, bicFamily, out elemPlunked, oneShot, hasLeader);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdPlaceTComDropAPNHInstance : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "TCOM";
            string FamilyName = "T-COM DROP-NH-WTA";
            string FamilySymbolName = "DROP";
            string pName = "TCOM - INSTANCE";
            string pNameVal = "AP";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagNameSymb = "T-COMM INSTANCE";
            bool oneShot = false;
            bool hasLeader = false;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            BuiltInCategory bicFamily = BuiltInCategory.OST_CommunicationDevices;
            Element elemPlunked;

            plunkThis.PlunkThisFamilyWithThisTagWithThisParameterSet(FamilyName, FamilySymbolName,
                pName, pNameVal, wsName, FamilyTagName, FamilyTagNameSymb, bicTagBeing, bicFamily, out elemPlunked, oneShot, hasLeader);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdTwoPickTag : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string wsName = "MECH HVAC";
            string FamilyTagName = "T-COMM TAG - INSTANCE";
            string FamilyTagSymbName = "T-COMM INSTANCE";
            bool hasLeader = false;
            bool oneShot = false;
            BuiltInCategory bicItemBeingTagged = BuiltInCategory.OST_CommunicationDevices;
            BuiltInCategory bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;
            Element elemTagged = null;

            plunkThis.TwoPickTag(wsName, FamilyTagName, FamilyTagSymbName, bicItemBeingTagged, bicTagBeing, hasLeader, oneShot, ref elemTagged);

            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.Manual)]
    class CmdPickSprinksOnly : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            BuiltInCategory _bicItemDesired = BuiltInCategory.OST_Sprinklers;

            List<ElementId> _selIds;
            plunkThis.PickTheseBicsOnly(_bicItemDesired, out _selIds);
            _uidoc.Selection.SetElementIds(_selIds);

            return Result.Succeeded;
        }
    }

 
    [Transaction(TransactionMode.Manual)]
    class CmdMatchParamterForTCOMDropTag : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            PlunkOClass plunkThis = new PlunkOClass(commandData.Application);
            string pName = "TCOM - INSTANCE";
            BuiltInCategory _bicItemBeingTagged = BuiltInCategory.OST_CommunicationDevices;
            BuiltInCategory _bicTagBeing = BuiltInCategory.OST_CommunicationDeviceTags;

            plunkThis.MatchParamenterValue(pName, _bicItemBeingTagged, _bicTagBeing);

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    class CmdCycleAirDeviceTypes : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {

            UIApplication uiapp = commandData.Application;
            UIDocument _uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application _app = uiapp.Application;
            Autodesk.Revit.DB.Document _doc = _uidoc.Document;

            BuiltInCategory bicFamilyA = BuiltInCategory.OST_DuctTerminal;
            BuiltInCategory bicFamilyB = BuiltInCategory.OST_DataDevices;
            BuiltInCategory bicFamilyC = BuiltInCategory.OST_MechanicalEquipment;
            //BuiltInCategory bicFamilyC = BuiltInCategory.OST_Sprinklers;

            ICollection<BuiltInCategory> categories = new[] { bicFamilyA, bicFamilyB, bicFamilyC };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            bool keepOnTruckn = true;
            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();

            using (TransactionGroup pickGrp = new TransactionGroup(_doc)) {
                pickGrp.Start("CmdCycleType");
                bool firstTime = true;

                //string strCats= "";
                //foreach (BuiltInCategory iCat in categories) {
                //    strCats = strCats + iCat.ToString().Replace("OST_", "") + ", "; 
                //}
                string strCats = FamilyUtils.BICListMsg(categories);

                formMsg.SetMsg("Pick the " + strCats + " to check its type.", "Type Cycle:");
                while (keepOnTruckn) {
                    try {
                        Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Pick the " + bicFamilyA.ToString() + " to cycle its types. (Press ESC to cancel)");
                        Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);

                        FamilyInstance fi = pickedElem as FamilyInstance;
                        FamilySymbol fs = fi.Symbol;

                        var famTypesIds = fs.Family.GetFamilySymbolIds().OrderBy(e => _doc.GetElement(e).Name).ToList();
                        int thisIndx = famTypesIds.FindIndex(e => e == fs.Id);
                        int nextIndx = thisIndx;
                        if (!firstTime) {
                            nextIndx = nextIndx + 1;
                            if (nextIndx >= famTypesIds.Count) {
                                nextIndx = 0;
                            }
                        } else {
                            firstTime = false;
                        }

                        if (pickedElem != null) {
                            using (Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam")) {
                                tp.Start();
                                if (pickedElem != null) {
                                    fi.Symbol = _doc.GetElement(famTypesIds[nextIndx]) as FamilySymbol;
                                    formMsg.SetMsg("Currently:\n" + fi.Symbol.Name + "\n\nPick again to cycle its types.", "Type Cycling");
                                }
                                tp.Commit();
                            }
                        } else {
                            keepOnTruckn = false;
                        }
                    } catch (Exception) {
                        keepOnTruckn = false;
                        //throw;
                    }
                }
                pickGrp.Assimilate();
            }

            formMsg.Close();
            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.Manual)]
    class CmdTest : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                             ref string message,
                             ElementSet elements) {
            FormMsgWPF formMsgWPF = new FormMsgWPF();
            formMsgWPF.Show();
            formMsgWPF.SetMsg("What", "What", "What");
            //System.Windows.MessageBox.Show(formMsgWPF.IsVisible.ToString());
            return Result.Succeeded;
        }
    }

   
    [Transaction(TransactionMode.Manual)]
    class CmdOpenDocFolder : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            string docFile = System.IO.Path.Combine(AppTCOMRibbon._app.docsPath, AppTCOMRibbon._app.docPDF01);
            if (System.IO.File.Exists(docFile)) {
                System.Diagnostics.Process.Start("explorer.exe", docFile);
            } else {
                System.Diagnostics.Process.Start("explorer.exe", AppTCOMRibbon._app.docsPath);
            }
            return Result.Succeeded;
        }
    }

}
