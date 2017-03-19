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

namespace WTA_TCOM {
    class AppTCOMRibbon : IExternalApplication {
        /// Singleton external application class instance.
        internal static AppTCOMRibbon _app = null;
        public string docsPath = "N:\\CAD\\BDS PRM 2016\\WTA Common\\Revit Resources\\WTAAddins\\SourceCode\\Docs";
        public string docPDF01 = "WTA_TCOM.pdf";
        /// Provide access to singleton class instance.
        public static AppTCOMRibbon Instance {
            get { return _app; }
        }
        public Result OnStartup(UIControlledApplication a) {
            _app = this;
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
            RibbonPanel thisNewRibbonPanelTCOM = a.CreateRibbonPanel(thisNewTabName, thisNewPanelName);
            // add button to ribbon panel
            //PushButton pushButton = thisNewRibbonPanel.AddItem(pbData) as PushButton;
            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            //pushButton.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".QVis.png");

    
            // provide button tips
            //pushButton.ToolTip = "Floats a form with buttons to toggle visibilities.";
            //pushButton.LongDescription = "On this form, the way the buttons look indicate the current visibility status.";

            //   Create push button in this ribbon panel 
            PushButtonData pbData2DH = new PushButtonData("2D H", "2D H", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop2DHInstance");
            PushButtonData pbData4DH = new PushButtonData("4D H", "4D H", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop4DHInstance");
            PushButtonData pbDataAPH = new PushButtonData("AP H", "AP H", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDropAPHInstance");
            PushButtonData pbData2DN = new PushButtonData(" 2D F ",   " 2D F ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop2DNHInstance");
            PushButtonData pbData4DN = new PushButtonData(" 4D F ",   " 4D F ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDrop4DNHInstance");
            PushButtonData pbDataAPN = new PushButtonData(   " AP F ",   " AP F ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceTComDropAPNHInstance");
            PushButtonData pbData2PTTAG = new PushButtonData(" 2PT\nTag ", " 2PT\nTag ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickTag");
            PushButtonData pbDataMtchTAG = new PushButtonData(" Match\nTag ", " Match\nTag ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdMatchParamterForTCOMDropTag");


            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            pbData2PTTAG.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".TagDrop.png");
            pbDataMtchTAG.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".MtchTagDrop.png");
            pbData2DN.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".TagDrop2Dsmall.png");
            pbData2DH.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".TagDrop2Dsmall.png");
            pbData4DN.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".TagDrop4Dsmall.png");
            pbData4DH.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".TagDrop4Dsmall.png");
            pbDataAPN.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".TagDropAPsmall.png");
            pbDataAPH.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),  ExecutingAssemblyName + ".TagDropAPsmall.png");


            // add button tips (when data, must be defined prior to adding button.)
            pbData2DH.ToolTip = "2D Hosted Drop";
            pbData4DH.ToolTip = "4D Hosted Drop";
            pbDataAPH.ToolTip = "AP Hosted Drop";
            pbData2DN.ToolTip = "2D Non Hosted (Free) Drop";
            pbData4DN.ToolTip = "4D Non Hosted (Free) Drop";
            pbDataAPN.ToolTip = "AP Non Hosted (Free) Drop";
            pbData2PTTAG.ToolTip = "Tag a drop with two picks.";
            pbDataMtchTAG.ToolTip = "Match one tag value to another.";

            string lDesc = " => Places a drop of the INSTANCE value indicated and then prompts for the TAG location.\n\n\u00A7Workset will be TCOM.";
            string lDesc2PTTAG = " => Tags an existing drop with two picks.\nFirst pick selects drop.\nSecond pick is tag location.\nPress ESC to exit the command.";
            string lDescMtchTAG = " => Matches the drop instance tag value by first picking the example tag and then picking the target drop instances.\nPress ESC to exit the command.";

            pbData2DH.LongDescription = pbData2DH.ToolTip + lDesc ;
            pbData4DH.LongDescription = pbData4DH.ToolTip + lDesc ;
            pbDataAPH.LongDescription = pbDataAPH.ToolTip + lDesc ;
            pbData2DN.LongDescription = pbData2DN.ToolTip + lDesc ;
            pbData4DN.LongDescription = pbData4DN.ToolTip + lDesc ;
            pbDataAPN.LongDescription = pbDataAPN.ToolTip + lDesc ;
            pbData2PTTAG.LongDescription = lDesc2PTTAG;
            pbDataMtchTAG.LongDescription = lDescMtchTAG;

            // add button to ribbon panel
            thisNewRibbonPanelTCOM.AddItem(pbData2PTTAG);
            List<RibbonItem> projectButtons = new List<RibbonItem>();
            projectButtons.AddRange(thisNewRibbonPanelTCOM.AddStackedItems(pbData2DH, pbData4DH, pbDataAPH));
            projectButtons.AddRange(thisNewRibbonPanelTCOM.AddStackedItems(pbData2DN, pbData4DN, pbDataAPN));
            thisNewRibbonPanelTCOM.AddItem(pbDataMtchTAG);

            /// Once there is a slideout, all new items go in it.
            /// It is not possbile to get back to the panel panel. 
            thisNewRibbonPanelTCOM.AddSlideOut();

            #region TCOM Info
            PushButtonData bInfoTCOM = new PushButtonData("Info", "Info", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdOpenDocFolder");
            bInfoTCOM.ToolTip = "See the TCOM help document.";
            bInfoTCOM.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".InfoSm.png");
            #endregion

            #region pbTCOMSettings
            PushButtonData pbTCOMSettings = new PushButtonData("TCOMSet", "TCOM Settings", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTCOMSettings");
            pbTCOMSettings.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), ExecutingAssemblyName + ".WTATabs.png");
            pbTCOMSettings.ToolTip = "Set TCOM Settings - like Tags in the companion view.";
            #endregion

            List<RibbonItem> slideOutPanelButtons = new List<RibbonItem>();
            slideOutPanelButtons.AddRange(thisNewRibbonPanelTCOM.AddStackedItems(bInfoTCOM, pbTCOMSettings));

   
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
