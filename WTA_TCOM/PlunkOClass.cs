﻿#region Header
//
// based on CmdPlaceFamilyInstance.cs - call PromptForFamilyInstancePlacement
// to place family instances and use the DocumentChanged event to
// capture the newly added element ids
//
// Copyright (C) 2010-2015 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
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
using Autodesk.Revit.DB.Structure;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
#endregion // Namespaces

namespace WTA_TCOM {
    class PlunkOClass {
        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(int hWnd);

        /// <summary>
        /// Set this flag to true to abort after placing the first instance.
        /// </summary>
        static bool _place_one_single_instance_then_abort = true;

        /// <summary>
        /// Send messages to main Revit application window.
        /// </summary>
        IWin32Window _revit_window;

        List<ElementId> _added_element_ids = new List<ElementId>();
        Autodesk.Revit.ApplicationServices.Application _app;
        Autodesk.Revit.DB.Document _doc;
        UIDocument _uidoc;
        UIApplication _uiapp;

        public PlunkOClass(UIApplication uiapp) {
            _revit_window = new JtWindowHandle(ComponentManager.ApplicationWindow);
            _uiapp = uiapp;
            _uidoc = _uiapp.ActiveUIDocument;
            _app = _uiapp.Application;
            _doc = _uidoc.Document;
        }

        public Result PlunkThisFamilyWithThisTagWithThisParameterSet(string _FamilyName, string _FamilySymbolName,
                                                              string _pName, string _pNameVal,
                                                              string _wsName,
                                                              string _FamilyTagName,
                                                              string _FamilyTagNameSymb,
                                                              BuiltInCategory _bicTagBeing, BuiltInCategory _bicFamily,
                                                              out Element _elemPlunked,
                                                              bool _oneShot, bool _hasLeader
                                                              ) {
            _elemPlunked = null;  // default state

            if (NotInThisView()) { return Result.Cancelled; }

            Element thisfamilySymb = FamilyUtils.FindFamilyType(_doc, typeof(FamilySymbol),
                                                                _FamilyName, _FamilySymbolName,
                                                                _bicFamily);

            if (thisfamilySymb == null) {
                return Result.Cancelled;
            }

            WorksetTable wst = _doc.GetWorksetTable();
            WorksetId wsRestoreTo = wst.GetActiveWorksetId();
            WorksetId wsID = FamilyUtils.WhatIsThisWorkSetIDByName(_wsName, _doc);
            if (wsID != null) {
                using (Transaction trans = new Transaction(_doc, "WillChangeWorkset")) {
                    trans.Start();
                    wst.SetActiveWorksetId(wsID);
                    trans.Commit();
                }
            }

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());

            bool keepOnTruckn = true;
            while (keepOnTruckn) {
                _elemPlunked = null;
                FamilySymbol thisFs = (FamilySymbol)thisfamilySymb;
                _added_element_ids.Clear();
                _app.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                try {
                    formMsg.SetMsg("Pick the location for:\n" + _FamilyName + " / " + _FamilySymbolName, "Item With Tag");
                    _uidoc.PromptForFamilyInstancePlacement(thisFs);
                } catch (Exception) {
                    SayOutOfContextMsg();
                    _app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                    //throw;
                    return Result.Cancelled;
                }
                _app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                int n = _added_element_ids.Count;
                //TaskDialog.Show(n.ToString(),n.ToString());
                if (n > 0) {
                    //TaskDialog.Show("Added", doc.GetElement(_added_element_ids[0]).Name);
                    try {
                        _elemPlunked = _doc.GetElement(_added_element_ids[0]);
                        using (Transaction tp = new Transaction(_doc, "PlunkOMatic:SetParam")) {
                            tp.Start();
                            //TaskDialog.Show(_pName, _pName);
                            Parameter parForTag = _elemPlunked.LookupParameter(_pName);
                            if (null != parForTag) {
                                //parForTag.SetValueString("PLUNKED");  // not for text, use for other
                                parForTag.Set(_pNameVal);
                                //TaskDialog.Show("_pNameVal", _pNameVal);
                            } else {
                                FamilyUtils.SayMsg("Cannot Set Parameter Value: " + _pNameVal, "... because parameter:\n" + _pName
                                    + "\ndoes not exist in the family:\n" + _FamilyName
                                    + "\nof Category:\n" + _bicFamily.ToString().Replace("OST_", ""));
                            }
                            tp.Commit();
                        }
                        formMsg.SetMsg("Now pick its tag location.", "Item With Tag");
                        AddThisTag(_elemPlunked, _FamilyTagName, _FamilyTagNameSymb, _pName, _bicTagBeing, _hasLeader);

                    } catch (Exception) {
                        // do nothing
                        keepOnTruckn = false;
                    }
                    if (_oneShot) { keepOnTruckn = false; }
                } else {  // added count = 0 therefore time to exit
                    keepOnTruckn = false;
                }
            } // end truckn loop
            formMsg.Close();
            return Result.Succeeded;
        }

        public Result PlunkThisFamilyType(string _FamilyName, string _FamilySymbolName,
                                                              string _wsName,
                                                              BuiltInCategory _bicFamily,
                                                              out Element _elemPlunked,
                                                              bool _oneShot
                                                              ) {
            _elemPlunked = null;  // default state

            if (NotInThisView()) { return Result.Cancelled; }

            Element thisfamilySymb = FamilyUtils.FindFamilyType(_doc, typeof(FamilySymbol),
                                                                _FamilyName, _FamilySymbolName,
                                                                _bicFamily);

            if (thisfamilySymb == null) {
                return Result.Cancelled;
            }

            WorksetTable wst = _doc.GetWorksetTable();
            WorksetId wsRestoreTo = wst.GetActiveWorksetId();
            WorksetId wsID = FamilyUtils.WhatIsThisWorkSetIDByName(_wsName, _doc);
            if (wsID != null) {
                using (Transaction trans = new Transaction(_doc, "WillChangeWorkset")) {
                    trans.Start();
                    wst.SetActiveWorksetId(wsID);
                    trans.Commit();
                }
            }

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());

            bool keepOnTruckn = true;
            while (keepOnTruckn) {
                _elemPlunked = null;
                FamilySymbol thisFs = (FamilySymbol)thisfamilySymb;
                _added_element_ids.Clear();
                _app.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                try {
                    formMsg.SetMsg("Pick the location for:\n" + _FamilyName + " / " + _FamilySymbolName, "Item Plunk");
                    _uidoc.PromptForFamilyInstancePlacement(thisFs);
                } catch (Exception) {
                    SayOutOfContextMsg();
                    _app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                    //throw;
                    return Result.Cancelled;
                }
                _app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                int n = _added_element_ids.Count;
                //TaskDialog.Show(n.ToString(),n.ToString());
                if (n > 0) {
                    //TaskDialog.Show("Added", doc.GetElement(_added_element_ids[0]).Name);
                    try {
                        _elemPlunked = _doc.GetElement(_added_element_ids[0]);
                    } catch (Exception) {
                        // do nothing
                        keepOnTruckn = false;
                    }
                    if (_oneShot) { keepOnTruckn = false; }
                } else {  // added count = 0 therefore time to exit
                    keepOnTruckn = false;
                }
            } // end truckn loop
            formMsg.Close();
            return Result.Succeeded;
        }


        ////// Creating a list of views to place this same tag into
        public System.Collections.Generic.List<Autodesk.Revit.DB.View> ViewsForTagList(Autodesk.Revit.DB.View thisView ) {
            System.Collections.Generic.List<Autodesk.Revit.DB.View> returningList = new System.Collections.Generic.List<Autodesk.Revit.DB.View>();
            bool tagOtherViews = WTA_TCOM.Properties.Settings.Default.TagOtherViews;
            Autodesk.Revit.DB.View _thisView = thisView;
            ElementId parentViewId = _thisView.GetPrimaryViewId();
            if (parentViewId != ElementId.InvalidElementId) {
                // need to use the parent view for name parsing, not this dependent view 
                _thisView = _doc.GetElement(parentViewId) as Autodesk.Revit.DB.View;
            }

            string actViewName = _thisView.Name;
            string viewTypeWrkMrk = "- WV ";
            string viewTypeCopyToMrk = "- PV ";
            string theOtherView = "";
            /// Is active view a working view?
            if (_thisView.Name.Contains(viewTypeWrkMrk)) {
                theOtherView = _thisView.Name.Replace(viewTypeWrkMrk, viewTypeCopyToMrk, StringComparison.CurrentCultureIgnoreCase);
            }
            /// Is active view a plotting view?
            if (_thisView.Name.Contains(viewTypeCopyToMrk)) {
                theOtherView = _thisView.Name.Replace(viewTypeCopyToMrk, viewTypeWrkMrk, StringComparison.CurrentCultureIgnoreCase);
            }
            FilteredElementCollector viewCollector = new FilteredElementCollector(_doc);
            viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));
            /// First add active view's parent view 
            returningList.Add(_thisView);
            if (tagOtherViews) {
                /// Now add the comparable other view
                foreach (Autodesk.Revit.DB.View posView in viewCollector) {
                    if (posView.ViewType == _thisView.ViewType) {
                        if (posView.Name.Equals(theOtherView, StringComparison.CurrentCultureIgnoreCase)
                        && (posView.Name != _thisView.Name)) {
                            returningList.Add(posView);
                            break;  /// done, there would be only one other view
                        }
                    }
                }
            }
            return returningList;
        }

        public void AddThisTag(Element _elemPlunked, string _FamilyTagName, string _FamilyTagNameSymb, string _pName,
                                BuiltInCategory _bicTagBeing, bool _hasLeader ) {
            ObjectSnapTypes snapTypes = ObjectSnapTypes.None;
            // make sure active view is not a 3D view
            Autodesk.Revit.DB.View actView = _doc.ActiveView;
            // tags will be automatically placed in other views
            System.Collections.Generic.List<Autodesk.Revit.DB.View> putTagsIntoTheseViews = ViewsForTagList(actView);
            // PickPoint requires a workplane to have been set. That is not always the case.
            try {
                bool chk = actView.SketchPlane.IsValidObject;
                //TaskDialog.Show("MSG", "Did not have to set a workplane");
            } catch (NullReferenceException) {
                using (Transaction wpt = new Transaction(_doc, "SetWorkplane")) {
                    wpt.Start();
                    Plane plane = new Plane(_doc.ActiveView.ViewDirection, _doc.ActiveView.Origin);
                    SketchPlane sp = SketchPlane.Create(_doc, plane);
                    _doc.ActiveView.SketchPlane = sp;
                    wpt.Commit();
                    //TaskDialog.Show("MSG", "Had to set a workplane");
                }
            }
            XYZ point = _uidoc.Selection.PickPoint(snapTypes, "Pick Tag Location for " + _pName);
            // define tag mode and tag orientation for new tag
            TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
            TagOrientation tagOrn = TagOrientation.Horizontal;
            using (Transaction t = new Transaction(_doc, "PlunkOMatic:Tag")) {
                t.Start();
                //// Will try to place the same tag in each of these views
                foreach (Autodesk.Revit.DB.View plunkView in putTagsIntoTheseViews) {
                    IndependentTag tag = _doc.Create.NewTag(plunkView, _elemPlunked, _hasLeader, tagMode, tagOrn, point);
                    Element desiredTagType = FamilyUtils.FindFamilyType(_doc, typeof(FamilySymbol), _FamilyTagName, _FamilyTagNameSymb, _bicTagBeing);
                    try {
                        if (desiredTagType != null) {
                            tag.ChangeTypeId(desiredTagType.Id);
                        }
                    } catch (Exception) {
                        //throw;
                    }
                }
                t.Commit();
            }
        }   // end AddThisTag

        public Result TagThisFamilyWithThisTag(string _FamilyTagName,
                                                              string _FamilyTagNameSymb,
                                                              BuiltInCategory _bicTagBeing,
                                                              BuiltInCategory _bicItemBeingTagged
                                                              ) {

            if (NotInThisView()) { return Result.Cancelled; }

            ICollection<BuiltInCategory> categories = new[] {
                _bicItemBeingTagged
            };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            bool keepOnTruckn = true;
            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());
            while (keepOnTruckn) {
                try {
                    formMsg.SetMsg("Select light fixture for tag.", "Fixture Tagging");
                    Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select light fixture for two pick tag.");
                    Element pickedElem = _doc.GetElement(pickedElemRef.ElementId);

                    ObjectSnapTypes snapTypes = ObjectSnapTypes.None;

                    // make sure active view is not a 3D view
                    Autodesk.Revit.DB.View actView = _doc.ActiveView;
                    // tags will be automatically placed in other views
                    System.Collections.Generic.List<Autodesk.Revit.DB.View> putTagsIntoTheseViews = ViewsForTagList(actView);
                    // PickPoint requires a workplane to have been set. That is not always the case.
                    try {
                        bool chk = actView.SketchPlane.IsValidObject;
                    } catch (NullReferenceException) {
                        using (Transaction wpt = new Transaction(_doc, "SetWorkplane")) {
                            wpt.Start();
                            Plane plane = new Plane(_doc.ActiveView.ViewDirection, _doc.ActiveView.Origin);
                            SketchPlane sp = SketchPlane.Create(_doc, plane);
                            _doc.ActiveView.SketchPlane = sp;
                            wpt.Commit();
                        }
                    }
                    // get the location for tag placement
                    formMsg.SetMsg("Now pick the tag text point.", "Fixture Tagging");
                    XYZ point = _uidoc.Selection.PickPoint(snapTypes, "Pick Tag Location for " + pickedElem.Name);

                    // define tag mode and tag orientation for new tag
                    TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
                    TagOrientation tagOrn = TagOrientation.Horizontal;
                    using (Transaction t = new Transaction(_doc, "PlunkOMatic:TwoPickTag")) {
                        t.Start();
                        //// Will try to place the same tag in each of these views
                        foreach (Autodesk.Revit.DB.View plunkView in putTagsIntoTheseViews) {
                            IndependentTag tag = _doc.Create.NewTag(plunkView, pickedElem, false, tagMode, tagOrn, point);
                            Element desiredTagType = FamilyUtils.FindFamilyType(_doc, typeof(FamilySymbol), _FamilyTagName, _FamilyTagNameSymb, _bicTagBeing);
                            try {
                                if (desiredTagType != null) {
                                    tag.ChangeTypeId(desiredTagType.Id);
                                } else {
                                    string msg = "... because this Tag Family:\n" + _FamilyTagName
                                    + "\ndoes not have the Type:\n" + _FamilyTagNameSymb
                                    + "\nof Category:\n" + _bicTagBeing.ToString().Replace("OST_", "");
                                    FamilyUtils.SayMsg("Cannot Set The Right Tag Type", msg);
                                }
                            } catch (Exception) {
                                //throw;
                            }
                        }
                        t.Commit();
                    }
                } catch (Autodesk.Revit.Exceptions.OperationCanceledException) {
                    keepOnTruckn = false;
                    //    TaskDialog.Show("Where", "here  " );
                }
            }
            formMsg.Close();
            return Result.Cancelled;
        }

        public Result TwoPickTag(string _wsName, string _FamilyTagName, string _FamilyTagNameSymb,
                                           BuiltInCategory _bicItemBeingTagged, BuiltInCategory _bicTagBeing,
                                            bool _hasLeader, bool _oneShot, ref Element _elemTagged,
                                             string _cmdPurpose = "na") {
            Element __pickedElem = null;
            if (NotInThisView()) { return Result.Cancelled; }

            // ===========
            ICollection<BuiltInCategory> categories = new[] {
                _bicItemBeingTagged
            };
            string bicName = _bicItemBeingTagged.ToString().Replace("OST_", "");
            string cmdPurpose = "";
            if (_cmdPurpose != "na") {
                cmdPurpose = _cmdPurpose + ": ";
            }
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            bool keepOnTruckn = true;
            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());
            while (keepOnTruckn) {
                try {
                    if (_elemTagged == null) { // need to make a pick
                        string msg = cmdPurpose + "Select the " + bicName + " you are tagging.";
                        formMsg.SetMsg(msg, bicName + " Tag");
                        Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, msg);
                        __pickedElem = _doc.GetElement(pickedElemRef.ElementId);
                        _elemTagged = __pickedElem;
                    } else { // pick has come in from outside
                        __pickedElem = _elemTagged;
                    }
                    //TaskDialog.Show("Picked", pickedElem.Name);
                    ObjectSnapTypes snapTypes = ObjectSnapTypes.None;
                    formMsg.SetMsg("Now pick tag location for this:\n" + __pickedElem.Name, cmdPurpose);
                    XYZ point = _uidoc.Selection.PickPoint(snapTypes, "Pick Tag Location for " + __pickedElem.Name);
                    // make sure active view is not a 3D view
                    Autodesk.Revit.DB.View actView = _doc.ActiveView;
                    // tags will be automatically placed in other views
                    System.Collections.Generic.List<Autodesk.Revit.DB.View> putTagsIntoTheseViews = ViewsForTagList(actView);
                    // define tag mode and tag orientation for new tag
                    TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
                    TagOrientation tagOrn = TagOrientation.Horizontal;
                    using (Transaction t = new Transaction(_doc, "PlunkOMatic:TwoPickTag")) {
                        t.Start();
                        //// Will try to place the same tag in each of these views
                        foreach (Autodesk.Revit.DB.View plunkView in putTagsIntoTheseViews) {
                            IndependentTag tag = _doc.Create.NewTag(plunkView, __pickedElem, _hasLeader, tagMode, tagOrn, point);
                            Element desiredTagType = FamilyUtils.FindFamilyType(_doc, typeof(FamilySymbol), _FamilyTagName, _FamilyTagNameSymb, _bicTagBeing);
                            try {
                                if (desiredTagType != null) {
                                    tag.ChangeTypeId(desiredTagType.Id);
                                } else {
                                    string msg = "... because this Tag Family:\n" + _FamilyTagName
                                    + "\ndoes not have the Type:\n" + _FamilyTagNameSymb
                                    + "\nof Category:\n" + _bicTagBeing.ToString().Replace("OST_", "");
                                    FamilyUtils.SayMsg("Cannot Set The Right Tag Type", msg);
                                }
                            } catch (Exception) {
                                //throw;
                            }
                        }
                        t.Commit();
                    }
                } catch (Exception) {
                    keepOnTruckn = false;
                    _elemTagged = null;  // need null out elem pick for two step process
                    formMsg.Close();
                    return Result.Cancelled;
                    //throw;
                }
                if (_oneShot) {
                    keepOnTruckn = false; // alow exit for one shot, _elemTagged is established 
                    formMsg.Close();
                    return Result.Cancelled;
                } else {
                    _elemTagged = null;  // need to reset for next round
                }
            }
            //  ==========
            formMsg.Close();
            return Result.Succeeded;
        }

        public Result MatchParamenterValue(string _pName, BuiltInCategory _bicItemBeingTagged, BuiltInCategory _bicTagBeing) {

            Parameter paramFromExamp = null;
            string strValueFromExampParm = null;
            bool keepOnTruckn = true;

            ICollection<BuiltInCategory> categories = new[] {
                _bicTagBeing
            };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            /// This is filters categories for the second step. Here both the item category
            /// being tagged and the tag for that category will be allowed picks.
            ICollection<BuiltInCategory> categoriesB = new[] {
                _bicItemBeingTagged,
                _bicTagBeing         // also allowing an existing tag in the pick
            };
            ElementFilter myPCatFilterB = new ElementMulticategoryFilter(categoriesB);
            ISelectionFilter myPickFilterB = SelFilter.GetElementFilter(myPCatFilterB);

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());
            // pick example section
            try {
                formMsg.SetMsg("Select example item for matching ...", "Parameter Match");
                Reference pickedExampElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select example item for matching ...");
                Element pickedExampElem = _doc.GetElement(pickedExampElemRef.ElementId);
                if (pickedExampElem.GetType() == typeof(IndependentTag)) {
                    IndependentTag _tag = (IndependentTag)pickedExampElem;
                    Element _taggedExampleE = _doc.GetElement(_tag.TaggedLocalElementId);
                    FamilyInstance exampFi = (FamilyInstance)_taggedExampleE;
                    Family _exampFam = exampFi.Symbol.Family;
                    FamilySymbol _exampFamSymb = exampFi.Symbol;
                    paramFromExamp = exampFi.LookupParameter(_pName);
                    strValueFromExampParm = paramFromExamp.AsString();
                    if (null != paramFromExamp) {
                        // FamilyUtils.SayMsg("parValFrmExamp.AsString()", strValueFromExampParm);
                    } else {
                        FamilyUtils.SayMsg("Cannot Match Parameter Values", "... because parameter:\n" + _pName
                            + "\ndoes not exist in the family:\n" + _exampFam.Name
                            + "\nof Category:\n" + _bicTagBeing.ToString().Replace("OST_", ""));
                    }
                }

            } catch (Exception) {
                keepOnTruckn = false;
                //throw;
            }

            // pick targets section
            while (keepOnTruckn) {
                try {
                    formMsg.SetMsg("Now pick item to be the same as the example ...", "Parameter Match");
                    Reference pickedTargetElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilterB, "Now pick item to be the same as the match example ...");
                    Element elemPicked = _doc.GetElement(pickedTargetElemRef.ElementId);
                    /// First set the intended item as the element picked. This will be
                    /// changed if the element picked was a tag itself, presumably the tag
                    /// on the intended item. 
                    Element intendedItemForChng = elemPicked;
                    /// Change element picked to the tagged element instead if user picked
                    /// the tag instead of the item tagged.
                    if (elemPicked.GetType() == typeof(IndependentTag)) {
                        IndependentTag _tag = (IndependentTag)elemPicked;
                        intendedItemForChng = _doc.GetElement(_tag.TaggedLocalElementId);
                    }

                    if (intendedItemForChng.GetType() == typeof(FamilyInstance)) {
                        FamilyInstance targetFi = (FamilyInstance)intendedItemForChng;
                        Family _targetFam = targetFi.Symbol.Family;
                        FamilySymbol _targetFamSymb = targetFi.Symbol;

                        Parameter parValFrmTarget = targetFi.LookupParameter(_pName);
                        string targetParmVal = parValFrmTarget.AsString();  // may not need to store as this
                        using (Transaction t = new Transaction(_doc, "PlunkOMatic:Tag")) {
                            t.Start();
                            if (null != parValFrmTarget) {
                                if (paramFromExamp != null) {
                                    parValFrmTarget.Set(strValueFromExampParm);
                                }
                            } else {
                                FamilyUtils.SayMsg("Cannot Match Parameter Values", "... because parameter:\n" + _pName
                                    + "\ndoes not exist in the family:\n" + _targetFam.Name
                                    + "\nof Category:\n" + _bicTagBeing.ToString().Replace("OST_", ""));
                            }
                            t.Commit();
                        }
                    }
                } catch (Exception) {
                    keepOnTruckn = false;
                    //throw;
                }
            }
            formMsg.Close();
            return Result.Cancelled;
        }

        public Result PickTheseBicsOnly(BuiltInCategory _bicItemsDesired, out List<ElementId> _selIds) {
            Element _pickedElemItems = null;
            _selIds = new List<ElementId>();
            if (NotInThisView()) { return Result.Cancelled; }
            ICollection<BuiltInCategory> categories = new[] {
                _bicItemsDesired
            };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());

            try {
                string strCats = FamilyUtils.BICListMsg(categories);
                formMsg.SetMsg("Selecting " + strCats + "Press the under the ribbon finish button when done.", "Filtering");
                IList<Reference> pickedElemRefs = _uidoc.Selection.PickObjects(ObjectType.Element, myPickFilter, "Filtered Selecting");
                using (Transaction t = new Transaction(_doc, "Filtered Selecting")) {
                    t.Start();
                    foreach (Reference pickedElemRef in pickedElemRefs) {
                        _pickedElemItems = _doc.GetElement(pickedElemRef.ElementId);
                        _selIds.Add(_pickedElemItems.Id);
                    }  // end foreach
                    t.Commit();
                }  // end using transaction
            } catch {
                // Get here when the user hits ESC when prompted for selection
                // "break" exits from the while loop
                //throw;
            }
            formMsg.Close();
            return Result.Succeeded;
        }

        public Result MatchRotationMany(BuiltInCategory _bicItemBeingRot, out List<ElementId> _selIds) {
            Element _pickedElemTargetItem = null;
            Element _PEMS = null;
            _selIds = new List<ElementId>();
            if (NotInThisView()) { return Result.Cancelled; }
            ICollection<BuiltInCategory> categories = new[] {
                _bicItemBeingRot
            };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());

            while (true) {
                try {
                    formMsg.SetMsg("Select the example item for rotation match.", "Match Many");
                    Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select item to rotate.");
                    // PEMS - picked element match source
                    _PEMS = _doc.GetElement(pickedElemRef.ElementId);

                    FamilyInstance fiSource = _PEMS as FamilyInstance;
                    if (fiSource.Symbol.Family.FamilyPlacementType != FamilyPlacementType.OneLevelBased) {
                        MessageBox.Show("Rotating a hosted item is not possible.");
                        continue;
                    }

                    Autodesk.Revit.DB.Location PEMS_Position = _PEMS.Location;
                    Autodesk.Revit.DB.LocationPoint PEMS_PositionPoint = PEMS_Position as Autodesk.Revit.DB.LocationPoint;
                    // The positionPoint also contains the element rotation about the z axis. 
                    double anglePEMS = PEMS_PositionPoint.Rotation;

                    formMsg.SetMsg("Now Select items to match rotation. Press the under the ribbon finish button when done.", "Match Many");
                    IList<Reference> pickedElemTargetRefs = _uidoc.Selection.PickObjects(ObjectType.Element, myPickFilter, "Select items to rotate.");
                    //Reference pickedElemTargetRef_P = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select item to rotate.");

                    //IList<Reference> pickedElemTargetRefs = null;
                    //pickedElemTargetRefs.Add(pickedElemTargetRef_P);

                    using (Transaction t = new Transaction(_doc, "Rotating Many")) {
                        t.Start();
                        foreach (Reference pickedElemTargetRef in pickedElemTargetRefs) {
                            _pickedElemTargetItem = _doc.GetElement(pickedElemTargetRef.ElementId);
                            FamilyInstance fiTarget = _pickedElemTargetItem as FamilyInstance;

                            if (fiTarget.Symbol.Family.FamilyPlacementType != FamilyPlacementType.OneLevelBased) {
                                continue;
                            }

                            XYZ _elemPoint = null;
                            Autodesk.Revit.DB.Location elemItemsPosition = _pickedElemTargetItem.Location;
                            Autodesk.Revit.DB.LocationPoint elemItemsLocPoint = elemItemsPosition as Autodesk.Revit.DB.LocationPoint;
                            double angleTargetExistingRot = elemItemsLocPoint.Rotation;

                            if (null != elemItemsLocPoint) {
                                _elemPoint = elemItemsLocPoint.Point;
                            }

                            if (null != _elemPoint) {
                                Line axis = Line.CreateBound(_elemPoint, new XYZ(_elemPoint.X, _elemPoint.Y, _elemPoint.Z + 10));
                                double angleToRot = anglePEMS - angleTargetExistingRot;
                                ElementTransformUtils.RotateElement(_doc, _pickedElemTargetItem.Id, axis, angleToRot);
                                _selIds.Add(_pickedElemTargetItem.Id);
                            }
                        }  // end foreach
                        t.Commit();
                    }  // end using transaction

                } catch {
                    // Get here when the user hits ESC when prompted for selection
                    // "break" exits from the while loop
                    formMsg.Close();
                    //throw;
                    break;
                }
            }
            return Result.Succeeded;
        }

        public Result TwoPickAimRotateMany(BuiltInCategory _bicItemBeingRot, out List<ElementId> _selIds) {
            Element _pickedElemItems = null;
            _selIds = new List<ElementId>();
            if (NotInThisView()) { return Result.Cancelled; }
            ICollection<BuiltInCategory> categories = new[] {
                _bicItemBeingRot
            };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());

            while (true) {
                try {
                    formMsg.SetMsg("Select items to rotate. Press the under the ribbon finish button when done.", "Aim Many");
                    IList<Reference> pickedElemRefs = _uidoc.Selection.PickObjects(ObjectType.Element, myPickFilter, "Select items to rotate.");

                    formMsg.SetMsg("Click to specify aiming point.", "Aim Many");
                    XYZ aimToPoint = _uidoc.Selection.PickPoint("Click to specify aiming point. ESC to quit.");

                    using (Transaction t = new Transaction(_doc, "Aiming Many")) {
                        t.Start();
                        foreach (Reference pickedElemRef in pickedElemRefs) {
                            _pickedElemItems = _doc.GetElement(pickedElemRef.ElementId);
                            FamilyInstance fi = _pickedElemItems as FamilyInstance;

                            if (fi.Symbol.Family.FamilyPlacementType != FamilyPlacementType.OneLevelBased) {
                                continue;
                            }

                            XYZ _elemPoint = null;
                            Autodesk.Revit.DB.Location elemItemsPosition = _pickedElemItems.Location;
                            // If the location is a point location, give the user information
                            Autodesk.Revit.DB.LocationPoint elemItemsLocPoint = elemItemsPosition as Autodesk.Revit.DB.LocationPoint;
                            // The positionPoint also contains the rotation. The rotation is about the transformed instance. Therefore
                            // any tilt will result in the wrong angle turned.
                            // MessageBox.Show((positionPoint.Rotation*(180.0/Math.PI)).ToString());

                            if (null != elemItemsLocPoint) {
                                _elemPoint = elemItemsLocPoint.Point;
                            }

                            if (null != _elemPoint) {
                                // Create a line between the two points
                                // A transaction is not needed because the line is a transient
                                // element created in the application, not in the document
                                Line aimToLine = Line.CreateBound(_elemPoint, aimToPoint);
                                double angle = XYZ.BasisY.AngleTo(aimToLine.Direction);

                                // AngleTo always returns the smaller angle between the two lines
                                // (for example, it will always return 10 degrees, never 350)
                                // so if the orient point is to the left of the pick point, then
                                // correct the angle by subtracting it from 2PI (Revit measures angles in degrees)
                                if (aimToPoint.X < _elemPoint.X) { angle = 2 * Math.PI - angle; }
                                double angleToRot = 2 * Math.PI - (elemItemsLocPoint.Rotation + angle);

                                // Create an axis in the Z direction 
                                Line axis = Line.CreateBound(_elemPoint, new XYZ(_elemPoint.X, _elemPoint.Y, _elemPoint.Z + 10));
                                ElementTransformUtils.RotateElement(_doc, _pickedElemItems.Id, axis, angleToRot);
                                _selIds.Add(_pickedElemItems.Id);
                            }
                        }  // end foreach
                        t.Commit();
                    }  // end using transaction

                } catch {
                    // Get here when the user hits ESC when prompted for selection
                    // "break" exits from the while loop
                    formMsg.Close();
                    //throw;
                    break;
                }
            }
            return Result.Succeeded;
        }

        public Result TwoPickAimRotateOne(BuiltInCategory _bicItemBeingRot) {
            Element _pickedElem = null;
            if (NotInThisView()) { return Result.Cancelled; }
            ICollection<BuiltInCategory> categories = new[] {
                _bicItemBeingRot
            };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categories);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());

            while (true) {
                try {
                    formMsg.SetMsg("Select item to rotate.", "Aiming One");
                    Reference pickedElemRef = _uidoc.Selection.PickObject(ObjectType.Element, myPickFilter, "Select item to rotate.");
                    _pickedElem = _doc.GetElement(pickedElemRef.ElementId);

                    FamilyInstance fi = _pickedElem as FamilyInstance;
                    if (fi.Symbol.Family.FamilyPlacementType != FamilyPlacementType.OneLevelBased) {
                        MessageBox.Show("Rotating a hosted item is not possible.");
                        continue;
                    }

                    XYZ PEOP = null;   // Picked Element Origin Point
                    Autodesk.Revit.DB.Location pickedElemPosition = _pickedElem.Location;
                    Autodesk.Revit.DB.LocationPoint pickedElemPositionPoint = pickedElemPosition as Autodesk.Revit.DB.LocationPoint;
                    // MessageBox.Show((positionPoint.Rotation*(180.0/Math.PI)).ToString());
                    // The positionPoint also contains the element rotation about the z axis. This is the element's
                    // transformed Z axis. Therefore any tilt results in an incorrect angle projected to project horizon XY plane.
                    if (null != pickedElemPositionPoint) {
                        PEOP = pickedElemPositionPoint.Point;
                    }
                    formMsg.SetMsg("Pick to specify aiming point.", "Aiming One");
                    XYZ aimAtThisPoint;
                    aimAtThisPoint = _uidoc.Selection.PickPoint("Click to specify aiming point. ESC to quit.");

                    if (null != PEOP) {
                        // Creating line between the two points for calculation purposes.
                        Line aimingLineXY = Line.CreateBound(PEOP, aimAtThisPoint);
                        double angleInXYPlane = XYZ.BasisY.AngleTo(aimingLineXY.Direction);

                        // AngleTo always returns the smaller angle between the two lines
                        // (for example, it will always return 10 degrees, never 350)
                        // so if the aim to point is to the left of the pick point, then
                        // correct the angle by subtracting it from 2PI (Revit measures angles in degrees)
                        if (aimAtThisPoint.X < PEOP.X) { angleInXYPlane = 2 * Math.PI - angleInXYPlane; }
                        double angleToRotXY = 2 * Math.PI - (pickedElemPositionPoint.Rotation + angleInXYPlane);

                        // An axis in the Z direction from the picked elememt position to be used for the rotation axis.
                        Line PEOP_axisZ = Line.CreateBound(PEOP, new XYZ(PEOP.X, PEOP.Y, PEOP.Z + 10));

                        using (Transaction t = new Transaction(_doc, "Aiming One")) {
                            t.Start();
                            ElementTransformUtils.RotateElement(_doc, _pickedElem.Id, PEOP_axisZ, angleToRotXY);
                            t.Commit();
                        }
                    }
                } catch {
                    // Get here when the user hits ESC when prompted for selection
                    // "break" exits from the while loop
                    formMsg.Close();
                    //throw;
                    break;
                }
            }
            return Result.Succeeded;
        }

        public Result ReHostLightsMany(BuiltInCategory _bicItemBeingRot) {
            Element _pickedElemItem = null;
            if (NotInThisView()) { return Result.Cancelled; }
            ICollection<BuiltInCategory> categoriesA = new[] {
                _bicItemBeingRot
            };
            ElementFilter myPCatFilter = new ElementMulticategoryFilter(categoriesA);
            ISelectionFilter myPickFilter = SelFilter.GetElementFilter(myPCatFilter);

            CeilingSelectionFilter cf = new CeilingSelectionFilter();

            FormMsgWPF formMsg = new FormMsgWPF();
            formMsg.Show();
            SetForegroundWindow(ComponentManager.ApplicationWindow.ToInt32());

            while (true) {
                try {
                    formMsg.SetMsg("Select items to pin/rehost. Press the under the ribbon finish button when done.", "Lights Rehosting");
                    IList<Reference> pickedElemRefs = _uidoc.Selection.PickObjects(ObjectType.Element, myPickFilter, "Select items to rehost.");

                    //formMsg.SetMsg("Now pick the new link file host ceiling.");
                    //Reference pickedCeilingReference = _uidoc.Selection.PickObject(ObjectType.LinkedElement, cf, "Selecting Linked Ceilings Only");



                    using (Transaction t = new Transaction(_doc, "Rehosting Many")) {
                        t.Start();
                        foreach (Reference pickedElemRef in pickedElemRefs) {
                            _pickedElemItem = _doc.GetElement(pickedElemRef.ElementId);
                            FamilyInstance fi = _pickedElemItem as FamilyInstance;

                            if (fi.Symbol.Family.FamilyPlacementType == FamilyPlacementType.OneLevelBased) {
                                continue;   // skip over nonhosting lights
                            }


                            fi.Pinned = true;





                            //fi.HostFace.CreateLinkReference() = newHost.ElementId;
                            //fi.Host = _doc.GetElement(newHost.ElementId);

                            //Reference r = fi.HostFace;


                            //    FamilyInstance fi = _famInstance as FamilyInstance;
                            //    Reference r = fi.HostFace;
                            //    Element e = null;
                            //    if (fi.HostFace != null) {
                            //    ElementId hostFaceReferenceId;
                            //    if (fi.Host.Category.Name != "RVT Links") {
                            //        hostFaceReferenceId = fi.HostFace.ElementId;
                            //        e = _doc.GetElement(hostFaceReferenceId);
                            //    } else {
                            //        FilteredElementCollector RevitLinksCollector = new FilteredElementCollector(_doc);
                            //        RevitLinksCollector.OfClass(typeof(RevitLinkInstance)).OfCategory(BuiltInCategory.OST_RvtLinks);
                            //        List<RevitLinkInstance> RevitLinkInstances = RevitLinksCollector.Cast<RevitLinkInstance>().ToList();
                            //        RevitLinkInstance rvtlink = RevitLinkInstances.Where(i => i.Id == fi.Host.Id).FirstOrDefault();
                            //        Document LinkedDoc = rvtlink.GetLinkDocument();
                            //        hostFaceReferenceId = fi.HostFace.LinkedElementId;
                            //        e = LinkedDoc.GetElement(hostFaceReferenceId);
                            //        r = fi.HostFace.CreateReferenceInLink();
                            //    }
                            //}


                            //fi.Host.Id = newHost.ElementId;


                        }  // end foreach




                        t.Commit();



                    }  // end using transaction

                } catch {
                    // Get here when the user hits ESC when prompted for selection
                    // "break" exits from the while loop
                    formMsg.Close();
                    //throw;
                    break;
                }



            }  // end while
            return Result.Succeeded;
        }

        public void OrientTheInsides(Element _elemPlunked) {
            if (HostedFamilyOrientation(_doc, _elemPlunked)) {
                Parameter parForHoriz = _elemPlunked.LookupParameter("HORIZONTAL");
                if (null != parForHoriz) {
                    parForHoriz.Set(0);
                }
            }
        }

        void OnDocumentChanged(
        object sender,
        DocumentChangedEventArgs e) {
            ICollection<ElementId> idsAdded = e.GetAddedElementIds();
            int n = idsAdded.Count;
            // this does not work, because the handler will
            // be called each time a new instance is added,
            // overwriting the previous ones recorded:
            //_added_element_ids = e.GetAddedElementIds();
            _added_element_ids.AddRange(idsAdded);
            if (_place_one_single_instance_then_abort && 0 < n) {
                // Why do we send the WM_KEYDOWN message twice?
                // I tried sending it once only, and that does
                // not work. Maybe the proper thing to do would 
                // be something like the Press.OneKey method...
                // nope, that did not work.
                //Press.OneKey( _revit_window.Handle,
                //  (char) Keys.Escape );

                Press.PostMessage(_revit_window.Handle,
                  (uint)Press.KEYBOARD_MSG.WM_KEYDOWN,
                  (uint)Keys.Escape, 0);

                Press.PostMessage(_revit_window.Handle,
                  (uint)Press.KEYBOARD_MSG.WM_KEYDOWN,
                  (uint)Keys.Escape, 0);
            }
        } // end OnDocumentChanged

        private void SayOutOfContextMsg() {
            TaskDialog thisDialog = new TaskDialog("Revit Says No Way");
            thisDialog.TitleAutoPrefix = false;
            thisDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
            thisDialog.MainInstruction = "Revit does not allow placing a family instance in this context.";
            thisDialog.MainContent = "";
            TaskDialogResult tResult = thisDialog.Show();
        }

        // returns true if view is not of type for plunking
        private bool NotInThisView() {
            if ((_doc.ActiveView.ViewType != ViewType.CeilingPlan) & (_doc.ActiveView.ViewType != ViewType.FloorPlan)
                & (_doc.ActiveView.ViewType != ViewType.Section) & (_doc.ActiveView.ViewType != ViewType.Elevation)) {
                string msg = "That action is not possible in this " + _doc.ActiveView.ViewType.ToString() + " viewtype?";
                FamilyUtils.SayMsg("Sorry, Not In This Neighborhood", msg);
                return true;
            }
            return false;
        }

        // For the time being this returns True if Horizontal for parameter rotation needs to be unchecked.
        // In the future this could return a rotation angle to actually rotate a rotatable sysmbol in the fanily
        public bool HostedFamilyOrientation(Document _doc, Element _famInstance) {
            if (_famInstance != null) {
                try {
                    FamilyInstance fi = _famInstance as FamilyInstance;
                    Reference r = fi.HostFace;
                    Element e = null;
                    if (fi.HostFace != null) {
                        ElementId hostFaceReferenceId;
                        if (fi.Host.Category.Name != "RVT Links") {
                            hostFaceReferenceId = fi.HostFace.ElementId;
                            e = _doc.GetElement(hostFaceReferenceId);
                        } else {
                            FilteredElementCollector RevitLinksCollector = new FilteredElementCollector(_doc);
                            RevitLinksCollector.OfClass(typeof(RevitLinkInstance)).OfCategory(BuiltInCategory.OST_RvtLinks);
                            List<RevitLinkInstance> RevitLinkInstances = RevitLinksCollector.Cast<RevitLinkInstance>().ToList();
                            RevitLinkInstance rvtlink = RevitLinkInstances.Where(i => i.Id == fi.Host.Id).FirstOrDefault();
                            Document LinkedDoc = rvtlink.GetLinkDocument();
                            hostFaceReferenceId = fi.HostFace.LinkedElementId;
                            e = LinkedDoc.GetElement(hostFaceReferenceId);
                            r = fi.HostFace.CreateReferenceInLink();

                        }
                    }

                    if (e != null) {
                        GeometryObject obj = e.GetGeometryObjectFromReference(r);
                        PlanarFace face = obj as PlanarFace;
                        //Face face = obj as Face;
                        UV q = r.UVPoint;
                        if (q == null) {
                            //FamilyUtils.SayMsg("Debug", "q is Null");
                            return false;
                        }
                        Transform trf = face.ComputeDerivatives(q);
                        XYZ v = trf.BasisX;
                        string mmm = "fi.FacingOrientation:  " + fi.FacingOrientation.ToString();
                        mmm = mmm + "\n" + "XYZ v" + v.ToString();
                        mmm = mmm + "\n" + "fi.FacingOrientation.CrossProduct(v)" + fi.FacingOrientation.CrossProduct(v).ToString();
                        mmm = mmm + "\n" + "fi.FacingOrientation.AngleTo(v)" + fi.FacingOrientation.AngleTo(v).ToString();
                        mmm = mmm + "\n" + "fi.FacingOrientation.DotProduct(v)" + fi.FacingOrientation.DotProduct(v).ToString();
                        if (Math.Abs(fi.FacingOrientation.CrossProduct(v).Y) < 0.000001) {
                            mmm = mmm + "\n\n" + "Symb needs to change orientation";
                            //FamilyUtils.SayMsg("Horiz/Vert", mmm);
                            return true;
                        }
                    }
                } catch (Exception) {
                    //throw;
                    return false;
                }

            }
            return false;
        }

        public FamilyInstance fi { get; set; }

        // This is the selection filter class
        public class CeilingSelectionFilter : ISelectionFilter {
            private RevitLinkInstance thisInstance = null;
            // If the calling document is needed then we'll use this to pass it to this class.
            //Document doc = null;
            //public CeilingSelectionFilter(Document document) {
            //    doc = document;
            //}

            public bool AllowElement(Element e) {
                if (e.GetType() == typeof(Ceiling)) { return true; }
                // Accept any link instance, and save the handle for use in AllowReference()
                thisInstance = e as RevitLinkInstance;
                return (thisInstance != null);
            }

            public bool AllowReference(Reference refer, XYZ point) {
                if (thisInstance == null) { return false; }
                //// Get the handle to the element in the link
                Document linkedDoc = thisInstance.GetLinkDocument();
                Element elem = linkedDoc.GetElement(refer.LinkedElementId);
                if (elem.GetType() == typeof(Ceiling)) { return true; }
                return false;
            }
        }
    }  // end class plunkoclass

    public static class StringExtensions {
        public static string Replace(this string str, string oldValue, string newValue, StringComparison comparison) {
            StringBuilder sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1) {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));
            return sb.ToString();
        }
    }
}

