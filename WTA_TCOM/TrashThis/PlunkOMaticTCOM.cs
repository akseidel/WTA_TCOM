using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Lighting;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using System.Diagnostics;
using Autodesk.Revit.DB.Events;


using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using ComponentManager = Autodesk.Windows.ComponentManager;
using IWin32Window = System.Windows.Forms.IWin32Window;
using Keys = System.Windows.Forms.Keys;




namespace PlunkOMaticTCOM {
    public partial class PlunkOMaticTCOM : System.Windows.Forms.Form {
        Autodesk.Revit.ApplicationServices.Application app;
        UIDocument uidoc;
        Document doc;

        ///// <summary>
        ///// Set this flag to true to abort after 
        ///// placing the first instance.
        ///// </summary>
        public bool _place_one_single_instance_then_abort = true;

        ///// <summary>
        ///// Send messages to main Revit application window.
        ///// </summary>
        IWin32Window _revit_window;

        List<ElementId> _added_element_ids = new List<ElementId>();


        private void buttonQuit2_Click(object sender, EventArgs e) {
            Close();
        }

        public PlunkOMaticTCOM(UIApplication thisUIApp, UIDocument thisUIDoc, IWin32Window revit_window) {
            InitializeComponent();
            app = thisUIApp.Application;
            uidoc = thisUIDoc;
            doc = uidoc.Document;
            _revit_window = revit_window;
        }

        //public void OnDocumentChanged(object sender, DocumentChangedEventArgs e) {
        //    ICollection<ElementId> idsAdded = e.GetAddedElementIds();
        //    int n = idsAdded.Count;
        //    //Debug.Print("{0} id{1} added.", n, Util.PluralSuffix(n));

        //    // this does not work, because the handler will
        //    // be called each time a new instance is added,
        //    // overwriting the previous ones recorded:
        //    //_added_element_ids = e.GetAddedElementIds();

        //    _added_element_ids.AddRange(idsAdded);

        //    if (_place_one_single_instance_then_abort && 0 < n) {
        //        // Why do we send the WM_KEYDOWN message twice?
        //        // I tried sending it once only, and that does
        //        // not work. Maybe the proper thing to do would 
        //        // be something like the Press.OneKey method...
        //        // nope, that did not work.
        //        //Press.OneKey( _revit_window.Handle,
        //        //  (char) Keys.Escape );

        //        Press.PostMessage(_revit_window.Handle,
        //          (uint)Press.KEYBOARD_MSG.WM_KEYDOWN,
        //          (uint)Keys.Escape, 0);

        //        Press.PostMessage(_revit_window.Handle,
        //          (uint)Press.KEYBOARD_MSG.WM_KEYDOWN,
        //          (uint)Keys.Escape, 0);
        //    }
        //}



        private void PlunkThisFamilyWithThisTagWithThisParameterSet(Autodesk.Revit.DB.Document _doc,
                                                                   string _FamilyName,
                                                                   string _FamilySymbolName,
                                                                   string _pName,
                                                                   string _pNameVal
                                                                   ) {
            /// <summary>
            ///// Set this flag to true to abort after 
            ///// placing the first instance.
            ///// </summary>
            //static bool _place_one_single_instance_then_abort = true;

            ///// <summary>
            ///// Send messages to main Revit application window.
            ///// </summary>
            //IWin32Window _revit_window;

            //List<ElementId> _added_element_ids = new List<ElementId>();

            //public Result Execute(
            //  ExternalCommandData commandData,
            //  ref string message,
             //  ElementSet elements) {

           // _revit_window = new JtWindowHandle( ComponentManager.ApplicationWindow);

            //    UIApplication uiapp = commandData.Application;
            //    UIDocument uidoc = uiapp.ActiveUIDocument;
            //    Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            //    Autodesk.Revit.DB.Document doc = uidoc.Document;

            //    //FilteredElementCollector collector = new FilteredElementCollector(doc);
            //    //collector.OfCategory(BuiltInCategory.OST_Doors);
            //    //collector.OfClass(typeof(FamilySymbol));
            //    //FamilySymbol symbol = collector.FirstElement() as FamilySymbol;

            //    string FamilyName = "T-COM DROP-WTA";
            //    string FamilySymbolName = "DROP";

            // Get the active view of the current document.
            Autodesk.Revit.DB.View viewA = _doc.ActiveView;

            // Get the class type of the active view, and format the prompt string
            String prompt = "Revit is currently in ";
            if (viewA is Autodesk.Revit.DB.View3D) {
                prompt += "3D view.";
            } else if (viewA is Autodesk.Revit.DB.ViewSection) {
                prompt += "section view.";
            } else if (viewA is Autodesk.Revit.DB.ViewSheet) {
                prompt += "sheet view.";
            } else if (viewA is Autodesk.Revit.DB.ViewDrafting) {
                prompt += "drafting view.";
            } else {
                prompt += "normal view, the view name is " + viewA.Name;
            }

            // Give the user some information
            TaskDialog.Show("Revit", prompt);


            Element thisfamilySymb = FamilyUtils.FindFamilyType(
                    _doc,
                    typeof(FamilySymbol),
                    _FamilyName,
                    _FamilySymbolName,
                    BuiltInCategory.OST_CommunicationDevices);

            FamilySymbol thisFs = (FamilySymbol)thisfamilySymb;

            MessageBox.Show("Have thisFs " + thisFs.Name);

            _added_element_ids.Clear();

            MessageBox.Show(" _added_element_ids.Clear();");

            app.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);

            MessageBox.Show("new EventHandler<DocumentChangedEventArgs>  add");

            uidoc.PromptForFamilyInstancePlacement(thisFs);

            app.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);

            MessageBox.Show("new EventHandler<DocumentChangedEventArgs> minus");

            int n = _added_element_ids.Count;

            if (n > 0) {
                //TaskDialog.Show("Added", doc.GetElement(_added_element_ids[0]).Name);
                ObjectSnapTypes snapTypes = ObjectSnapTypes.None;
                try {
                    Element e = doc.GetElement(_added_element_ids[0]);

                    Transaction tp = new Transaction(doc, "PlunkOMatic:SetParam");
                    tp.Start();
                    string pName = "TCOM - INSTANCE";
                    string pNameVal = "2D";
                    Parameter parForTag = e.LookupParameter(pName);
                    if (null != parForTag) {
                        //parForTag.SetValueString("PLUNKED");  // not for text, use for other
                        parForTag.Set(pNameVal);
                    } else {
                        TaskDialog.Show("There is not parameter named", pName);
                    }
                    tp.Commit();

                    XYZ point = uidoc.Selection.PickPoint(snapTypes, "Pick Tag Location for " + pName);
                    // make sure active view is not a 3D view
                    Autodesk.Revit.DB.View view = doc.ActiveView;
                    // define tag mode and tag orientation for new tag
                    TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
                    TagOrientation tagOrn = TagOrientation.Horizontal;

                    Transaction t = new Transaction(doc, "PlunkOMatic:Tag");
                    t.Start();
                    IndependentTag tag = doc.Create.NewTag(view, e, false, tagMode, tagOrn, point);
                    t.Commit();
                } catch (Exception) {
                    // do nothing
                }

            }


        }

        private void buttonPlunk_MouseClick(object sender, MouseEventArgs e) {
            string FamilyName = "T-COM DROP-WTA";
            string FamilySymbolName = "DROP";
            string pName = "TCOM - INSTANCE";
            string pNameVal = "2D";

            Hide();
            PlunkThisFamilyWithThisTagWithThisParameterSet(doc, FamilyName, FamilySymbolName, pName, pNameVal);
            Show();

        }



        public void OnDocumentChanged(
                             object sender,
                             DocumentChangedEventArgs e) {
            ICollection<ElementId> idsAdded = e.GetAddedElementIds();

            int n = idsAdded.Count;

            //Debug.Print("{0} id{1} added.", n, Util.PluralSuffix(n));

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






    }
}
