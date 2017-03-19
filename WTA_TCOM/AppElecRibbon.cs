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
    class AppElecRibbon : IExternalApplication {
        public Result OnStartup(UIControlledApplication a) {
            // Add TCOM drops  to WTA-TCOM Ribbon
            AddElec_WTA_ELEC_Ribbon(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a) {
            return Result.Succeeded;
        }

        public void AddElec_WTA_ELEC_Ribbon(UIControlledApplication a) {
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ExecutingAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            // create ribbon tab 
            String thisNewTabName = "WTA-ELEC";
            try {
                a.CreateRibbonTab(thisNewTabName);
            } catch (Autodesk.Revit.Exceptions.ArgumentException) {
                // Assume error generated is due to "WTA" already existing
            }
            //   Add new ribbon panel. 
            String thisNewPanelNameA = " Light Fixture ";
            RibbonPanel thisNewRibbonPanelA = a.CreateRibbonPanel(thisNewTabName, thisNewPanelNameA);
            String thisNewPanelNameB = " Aiming Lights ";
            RibbonPanel thisNewRibbonPanelB = a.CreateRibbonPanel(thisNewTabName, thisNewPanelNameB);

            
            //System.Windows.MessageBox.Show(a.GetRibbonPanels(thisNewTabName).Count.ToString());
     
            //   Create push buttons 
            PushButtonData pbTwoPickTagLight = new PushButtonData("Two Pick\nLighting Tag", "Two Pick\nLighting Tag", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickLightingTag");
            PushButtonData pbTwoPickAimLight = new PushButtonData(" Aim Light ", " Aim Light ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickLightRot");
            PushButtonData pbAimManyLights = new PushButtonData(" Aim Many Lights ", " Aim Many Lights ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdAimManyLights");
            PushButtonData pb3DAim = new PushButtonData(" 3D Aim ", " 3d Aim ", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickSprinkRot3D");

            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            pbTwoPickTagLight.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.TwoPickTag.png");

            //pbData2DN.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.TagDrop2Dsmall.png");

            // add button tips (when data, must be defined prior to adding button.)
            pbTwoPickTagLight.ToolTip = "Places lighting tag in two picks.";
            pbTwoPickAimLight.ToolTip  = "2D Aims a non hosted light.";
            pbAimManyLights.ToolTip = "2D Aims a selection of non hosted lights.";
            pb3DAim.ToolTip = "EXPERIMENTAL NOVELTY, 3D Aims a special element.";

            string lDescpbTwoPickTagLight = "Places the lighting tag in two picks.\nThe first pick selects the light fixture.\nThe second pick is the tag location.";
            string lDescpbTwoPickAimLight = "Pick a light.\nThen pick where it is supposed to aim.";
            string lDescpbAimManyLights = "Select a bunch of lights.\nThen pick the one spot where they all should aim towards.";
            string lDescpb3DAim = "The special element has to be a Sprinkler category family instance.";
            
            pbTwoPickTagLight.LongDescription = lDescpbTwoPickTagLight;
            pbTwoPickAimLight.LongDescription = lDescpbTwoPickAimLight;
            pbAimManyLights.LongDescription = lDescpbAimManyLights;
            pb3DAim.LongDescription = lDescpb3DAim;


            // add button to ribbon panel
            thisNewRibbonPanelA.AddItem(pbTwoPickTagLight);
            List<RibbonItem> projectButtonsB = new List<RibbonItem>();
            projectButtonsB.AddRange(thisNewRibbonPanelB.AddStackedItems(pbTwoPickAimLight, pbAimManyLights, pb3DAim));
            //projectButtons.AddRange(thisNewRibbonPanel.AddStackedItems(pbData2DN, pbData4DN, pbDataAPN));

           

        } // AddMech_WTA_Elec_Ribbon

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
