﻿#region Namespaces
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
    class AppMechRibbon : IExternalApplication {
        public Result OnStartup(UIControlledApplication a) {
            // Add TCOM drops  to WTA-TCOM Ribbon
            AddMech_WTA_MECH_Ribbon(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a) {
            return Result.Succeeded;
        }

        public void AddMech_WTA_MECH_Ribbon(UIControlledApplication a) {
            string ExecutingAssemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string ExecutingAssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            // create ribbon tab 
            String thisNewTabName = "WTA-MECH";
            try {
                a.CreateRibbonTab(thisNewTabName);
            } catch (Autodesk.Revit.Exceptions.ArgumentException) {
                // Assume error generated is due to "WTA" already existing
            }
            //   Create a push button in the ribbon panel 
            //PushButtonData pbData = new PushButtonData("QVis", "   QVis   ", ExecutingAssemblyPath, ExecutingAssemblyName + ".QVisCommand");
            //   Add new ribbon panel. 
            String thisNewPanelName = " Sensor ";
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
            PushButtonData pbMechStatUnit1 = new PushButtonData("SensForUnit1", "Sens for\nUnit ID/#", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatForMechUnitInstance1");
            PushButtonData pbMechStatUnitOff1 = new PushButtonData("SensForUnitOff1", "Sens w/L\nfor Unit ID/#", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatOffsetForMechUnitInstance1");
            PushButtonData pbMechStatUnit2 = new PushButtonData("SensForUnit2", "Sens for\nUnit #", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatForMechUnitInstance2");
            PushButtonData pbMechStatUnitOff2 = new PushButtonData("SensForUnitOff2", "Sens w/L\nfor Unit #", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdPlaceStatOffsetForMechUnitInstance2");

            PushButtonData pbMechStatTag1 = new PushButtonData("Sens IDT", "ID/# Tag", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagEuip1");
            PushButtonData pbMechStatTagOff1 = new PushButtonData("Sens OffsetT", "ID/# Offset", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagOffEuip1");
            PushButtonData pbMechStatTag2 = new PushButtonData("Sens IDO", "# Tag", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagEuip2");
            PushButtonData pbMechStatTagOff2 = new PushButtonData("Sens OffsetO", "# Offset", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagOffEuip2");

            SplitButtonData sbMSU = new SplitButtonData("sbMSU", "Sens\nfor Unit");
            SplitButtonData sbMSUO = new SplitButtonData("sbMSUO", "Sens w/L\nfor Unit");
            SplitButtonData sbSTagReg = new SplitButtonData("sbSensTag", "Split Sens ID");
            SplitButtonData sbSTagOff = new SplitButtonData("sbSensTagOff", "Split Sens Offset");

            PushButtonData pbMechSpinStat = new PushButtonData("Sens Spin", "Sens Spin", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdSpinStat");
            PushButtonData pbMechTogSym = new PushButtonData("Sens Symb", "Sens Symb", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdSymbStat");



            //   Set the large image shown on button
            //Note that the full image name is namespace_prefix + "." + the actual imageName);
            pbMechStatUnit1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.TStatUnit_1.png");
            pbMechStatUnitOff1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.TStatUnitO_1.png");
            pbMechStatUnit2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.TStatUnit_2.png");
            pbMechStatUnitOff2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.TStatUnitO_2.png");


            pbMechStatTag1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.SensTag_IDNO.png");
            pbMechStatTag2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.SensTag_NO.png");
            pbMechStatTagOff1.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.SensOffTag_IDNO.png");
            pbMechStatTagOff2.LargeImage = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.SensOffTag_NO.png");


            //pbData2DN.Image = NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(), "PlunkOMaticTCOM.TagDrop2Dsmall.png");

            // add button tips (when data, must be defined prior to adding button.)
            pbMechStatUnit1.ToolTip = "Places a Sensor Stat for mechanical equip with equip. tag.";
            pbMechStatUnitOff1.ToolTip = "Places a Sensor Stat with leader for mechanical equip with equip. tag.";
            pbMechStatUnit2.ToolTip = "Places a Sensor Stat for mechanical equip with equip. tag.";
            pbMechStatUnitOff2.ToolTip = "Places a Sensor Stat with leader for mechanical equip with equip. tag.";
            pbMechStatTag1.ToolTip = "Places ID and number equipment tag that is for Sensors.";
            pbMechStatTagOff1.ToolTip = "Places offset Sensor symbol with ID and number equipment tag that is ID for Sensors.";
            pbMechStatTag2.ToolTip = "Places number only equipment tag that is ID for Sensors.";
            pbMechStatTagOff2.ToolTip = "Places offset Sensor symbol with number only equipment tag that is ID for Sensors.";

            pbMechSpinStat.ToolTip = "Toggles Horizontal/Vertical parameter value.";
            pbMechTogSym.ToolTip = "Toggles Sensor symbol visibility.";

            string lDescMechStatUnit =
                "First pick is the Sensor location."
                + "\nSecond pick selects the mechanical equipment the Sensor is client to."
                + "\nThird pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";
            string lDescMechStatUnitOff =
                "Places a Sensor for mechanical equipment in four picks."
                + "\nFirst pick is the Sensor location. Ignore the symbol. It will be turned off later."
                + "\nSecond pick is for the offset Sensor tag symbol location."
                + "\nThird pick selects the mechanical equipment the Sensor is client to."
                + "\nLast pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";
            string lDescMechStatTag =
                "First pick is the mechanical equipment the Sensor is client to."
                + "\nLast pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";
            string lDescMechStatTagOff = "First pick is the Sensor to change."
                + "\nSecond pick is for the Sensor offset symbol location."
                + "\nIf needed, third pick selects the mechanical equipment the Sensor is client to. Otherwise press Esc to exit."
                + "\nLast pick selects the mechanical equipment Sensor ID text location at the Sensor. This is a Tag on the mechanical equipment.";

            pbMechStatUnit1.LongDescription = lDescMechStatUnit;
            pbMechStatUnitOff1.LongDescription = lDescMechStatUnitOff;
            pbMechStatUnit2.LongDescription = lDescMechStatUnit;
            pbMechStatUnitOff2.LongDescription = lDescMechStatUnitOff;
            pbMechStatTag1.LongDescription = lDescMechStatTag;
            pbMechStatTagOff1.LongDescription = lDescMechStatTagOff;
            pbMechStatTag2.LongDescription = lDescMechStatTag;
            pbMechStatTagOff2.LongDescription = lDescMechStatTagOff;

            // add split buttons
            SplitButton sb_MU = thisNewRibbonPanel.AddItem(sbMSU) as SplitButton;
            sb_MU.AddPushButton(pbMechStatUnit1);
            sb_MU.AddPushButton(pbMechStatUnit2);
            SplitButton sb_MUO = thisNewRibbonPanel.AddItem(sbMSUO) as SplitButton;
            sb_MUO.AddPushButton(pbMechStatUnitOff1);
            sb_MUO.AddPushButton(pbMechStatUnitOff2);

            thisNewRibbonPanel.AddSeparator();

            // Add split buttons
            SplitButton sbT = thisNewRibbonPanel.AddItem(sbSTagReg) as SplitButton;
            sbT.AddPushButton(pbMechStatTag1);
            sbT.AddPushButton(pbMechStatTag2);
            SplitButton sbOffT = thisNewRibbonPanel.AddItem(sbSTagOff) as SplitButton;
            sbOffT.AddPushButton(pbMechStatTagOff1);
            sbOffT.AddPushButton(pbMechStatTagOff2);

            thisNewRibbonPanel.AddSeparator();
            List<RibbonItem> ribbonItems = new List<RibbonItem>();
            ribbonItems.AddRange(thisNewRibbonPanel.AddStackedItems(pbMechSpinStat, pbMechTogSym));


        } // AddMech_WTA_MECH_Ribbon

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



//IList<RibbonItem> stackedItems = thisNewRibbonPanel.AddStackedItems(pbMechStatTag, pbMechStatTagOff, cbData);
//if (stackedItems.Count > 1) {
//    ComboBox cBox = stackedItems[2] as ComboBox;
//    if (cBox != null) {
//        //cBox.ItemText = "ComboBox";

//        cBox.ToolTip = "Options";
//        cBox.LongDescription = "Sets with or without\nthe equipment ID.";

//        ComboBoxMemberData cboxMemDataA = new ComboBoxMemberData("WithID", "Two Liner");
//        cBox.AddItem(cboxMemDataA);

//        ComboBoxMemberData cboxMemDataB = new ComboBoxMemberData("NoID", "One Liner");
//        cBox.AddItem(cboxMemDataB);

//    }
//}

//PushButtonData pbMechStatTag = new PushButtonData("Sens ID", "Sens ID", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagEuip");
//PushButtonData pbMechStatTagOff = new PushButtonData("Sens Offset", "Sens Offset", ExecutingAssemblyPath, ExecutingAssemblyName + ".CmdTwoPickMechSensorTagOffEuip");

//thisNewRibbonPanel.AddItem(pbMechStatUnit1);
//thisNewRibbonPanel.AddItem(pbMechStatUnitOff1);
