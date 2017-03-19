using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;

namespace WTA_TCOM {
    [Transaction(TransactionMode.Manual)]
    class CmdTCOMSettings : IExternalCommand {
        public Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements) {

            WPF_TCOMSettings WTATabControler = new WPF_TCOMSettings(commandData);
            WTATabControler.ShowDialog();
            return Result.Succeeded;
        }
    }

}
