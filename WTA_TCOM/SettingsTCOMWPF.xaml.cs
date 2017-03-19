using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System.ComponentModel;


namespace WTA_TCOM {
    /// <summary>
    /// Interaction logic for WPF_WTATabControler.xaml
    /// </summary>
    public partial class WPF_TCOMSettings : Window {
        Autodesk.Revit.UI.UIApplication uiapp;
        Autodesk.Revit.UI.UIDocument uidoc;
        Autodesk.Revit.ApplicationServices.Application app;
        Autodesk.Revit.DB.Document doc;
        List<wtaTabState> wtaTStates = new List<wtaTabState>();
        String TagOtherViewsSettingName = "TagOtherViews";

        public WPF_TCOMSettings(ExternalCommandData commandData) {
            InitializeComponent();
            uiapp = commandData.Application;
            uidoc = uiapp.ActiveUIDocument;
            app = uiapp.Application;
            doc = uidoc.Document;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            FillOnOffSettingsGrid();
        }

        private void FillOnOffSettingsGrid() {
            wtaTabState TCOMSettingOnOffState = new wtaTabState();
            TCOMSettingOnOffState.MySetName = TagOtherViewsSettingName;
            TCOMSettingOnOffState.MyOnOffSetBool = WTA_TCOM.Properties.Settings.Default.TagOtherViews;
            wtaTStates.Add(TCOMSettingOnOffState);

            RootSearchPath.Text = WTA_TCOM.Properties.Settings.Default.RootSearchPath;
            TabsControlGrid.ItemsSource = wtaTStates;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            foreach (wtaTabState wtaTabState in wtaTStates) {
                SaveUserPref(wtaTabState);
            }
            WTA_TCOM.Properties.Settings.Default.RootSearchPath = RootSearchPath.Text;
            WTA_TCOM.Properties.Settings.Default.Save();
        }

        public void SaveUserPref(wtaTabState wtaTabState) {
            switch (wtaTabState.MySetName) {
                case "TagOtherViews":
                    WTA_TCOM.Properties.Settings.Default.TagOtherViews = wtaTabState.MyOnOffSetBool;
                    break;
                default:
                    break;
            }
        }

        public void DragWindow(object sender, MouseButtonEventArgs args) {
            // Watch out. Fatal error if not primary button!
            if (args.LeftButton == MouseButtonState.Pressed) { DragMove(); }
        }

        private void Quit_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void BotLine_MouseEnter(object sender, MouseEventArgs e) {
            BotLine.FontWeight = FontWeights.Bold;
        }

        private void BotLine_MouseLeave(object sender, MouseEventArgs e) {
            BotLine.FontWeight = FontWeights.Normal;
        }

        private void BotLine_MouseDown(object sender, MouseButtonEventArgs e) {
            Close();
        }
    }

    public class wtaTabState : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public string MySetName { get; set; }
        public bool MyOnOffSetBool { get; set; }
    }
}
