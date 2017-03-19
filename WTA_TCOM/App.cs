#region Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace PlunkOMaticTCOM {
    class App : IExternalApplication {
        public Result OnStartup(UIControlledApplication a) {

            // Add TCOM drops  to WTA-TCOM Ribbon
            AddTCOMDrops_WTA_TCOM_Ribbon(a);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a) {
            return Result.Succeeded;
        }

        public void AddTCOMDrops_WTA_TCOM_Ribbon(UIControlledApplication a) {
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ExecutingAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            // create ribbon tab 
            String thisNewTabName = "WTA-TCOM";
            try {
                a.CreateRibbonTab(thisNewTabName);
            } catch (Autodesk.Revit.Exceptions.ArgumentException) {
                // Assume error generated is due to "WTA" already existing
            }
            //   Create a push button in the ribbon panel 
            //PushButtonData pbData = new PushButtonData("QVis", "   QVis   ", ExecutingAssemblyPath, ExecutingAssemblyName + ".QVisCommand");
            //   Add new ribbon panel. 
            String thisNewPanelName = "TCOM Drops";
            RibbonPanel thisNewRibbonPanel = a.CreateRibbonPanel(thisNewTabName, thisNewPanelName);
            // add button to ribbon panel
            //PushButton pushButton = thisNewRibbonPanel.AddItem(pbData) as PushButton;
            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            //pushButton.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.QVis.png");

    
            // provide button tips
            //pushButton.ToolTip = "Floats a form with buttons to toggle visibilities.";
            //pushButton.LongDescription = "On this form, the way the buttons look indicate the current visibility status.";

            //   Create push button in this ribbon panel 
            PushButtonData pbData2DH = new PushButtonData("2D Hosted", "2D Hosted", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop2DHInstance");
            PushButtonData pbData4DH = new PushButtonData("4D Hosted", "4D Hosted", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop4DHInstance");
            PushButtonData pbDataAPH = new PushButtonData("AP Hosted", "AP Hosted", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDropAPHInstance");
            PushButtonData pbData2DN = new PushButtonData(" 2D Free ",   " 2D Free ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop2DNHInstance");
            PushButtonData pbData4DN = new PushButtonData(" 4D Free ",   " 4D Free ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop4DNHInstance");
            PushButtonData pbDataAPN = new PushButtonData(   " AP Free ",   " AP Free ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDropAPNHInstance");
            PushButtonData pbData2PTTAG = new PushButtonData(" 2PT TAG ", " 2PT TAG ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickTag");


            // add button tips (when data, must be defined prior to adding button.)
            pbData2DH.ToolTip = "2D Hosted Drop";
            pbData4DH.ToolTip = "4D Hosted Drop";
            pbDataAPH.ToolTip = "AP Hosted Drop";
            pbData2DN.ToolTip = "2D Non Hosted Drop";
            pbData4DN.ToolTip = "4D Non Hosted Drop";
            pbDataAPN.ToolTip = "AP Non Hosted Drop";
            pbData2PTTAG.ToolTip = "Tag a drop with two picks.";

            string lDesc = " => Places a drop of the INSTANCE value indicated and then prompts for the TAG location. The drops's Workset will be TCOM.";
            string lDesc2PTTAG = " => Tags an existing drop with two picks. First pick selects drop. Second pick is tag location. Press ESC to exit the command.";

            pbData2DH.LongDescription = pbData2DH.ToolTip + lDesc ;
            pbData4DH.LongDescription = pbData4DH.ToolTip + lDesc ;
            pbDataAPH.LongDescription = pbDataAPH.ToolTip + lDesc ;
            pbData2DN.LongDescription = pbData2DN.ToolTip + lDesc ;
            pbData4DN.LongDescription = pbData4DN.ToolTip + lDesc ;
            pbDataAPN.LongDescription = pbDataAPN.ToolTip + lDesc ;
            pbData2PTTAG.LongDescription = lDesc2PTTAG;

            // add button to ribbon panel
            thisNewRibbonPanel.AddItem(pbData2PTTAG);
            List<RibbonItem> projectButtons = new List<RibbonItem>();
            projectButtons.AddRange(thisNewRibbonPanel.AddStackedItems(pbData2DH, pbData4DH, pbDataAPH));
            projectButtons.AddRange(thisNewRibbonPanel.AddStackedItems(pbData2DN, pbData4DN, pbDataAPN));
            
            

        } // AddTCOMDrops_WTA-TCOM_Ribbon

        /// <summary>
        /// Load a new icon bitmap from embedded resources.
        /// For the BitmapImage, make sure you reference WindowsBase and Presentation Core
        /// and PresentationCore, and import the System.Windows.Media.Imaging namespace. 
        /// </summary>
        BitmapImage NewBitmapImage(System.Reflection.Assembly a, string imageName) {
            Stream s = a.GetManifestResourceStream(imageName);
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();
            return img;
        }

    }
}
