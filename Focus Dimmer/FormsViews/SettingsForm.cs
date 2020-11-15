using Focus_Dimmer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Focus_Dimmer.FormsViews
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            LoadOptions();
        }

        private void LoadOptions()
        {
            InitOnOffCBList();
            InitDimmingModeCBList();
            SetCBSelectedItems();
        }
        
        #region LoadOptionsMethods
        
        private void InitOnOffCBList()
        {
            var CBSource = InitOnOffCBListContent();
            OnOffCB.DataSource = new BindingSource(CBSource, null);
            OnOffCB.DisplayMember = "Key";
            OnOffCB.ValueMember = "Value";
        }

        private Dictionary<string, bool> InitOnOffCBListContent()
        {
            Dictionary<string, bool> CBSource = new Dictionary<string, bool>();
            CBSource.Add("On", true);
            CBSource.Add("Off", false);
            return CBSource;
        }

        private void InitDimmingModeCBList()
        {
            var CBSource = InitDimmingModeCBListContent();
            DimmingModeCB.DataSource = new BindingSource(CBSource, null);
            DimmingModeCB.DisplayMember = "Key";
            DimmingModeCB.ValueMember = "Value";
        }

        private Dictionary<string, int> InitDimmingModeCBListContent()
        {
            Dictionary<string, int> CBSource = new Dictionary<string, int>();

            foreach (var value in Enum.GetValues(typeof(Modes)))
            {
                CBSource.Add(value.ToString(), (int) value);
            }

            return CBSource;
        }

        private void SetCBSelectedItems()
        {
            OnOffCB.SelectedItem = ((Dictionary<string, bool>)((BindingSource)OnOffCB.DataSource).DataSource)
                .Where(x => x.Value == Properties.Settings.Default.defaultOnOff).First();
            
            DimmingModeCB.SelectedItem = ((Dictionary<string, int>)((BindingSource)DimmingModeCB.DataSource).DataSource)
                .Where(x => x.Value == Properties.Settings.Default.defaultDimmingMode).First();
        }
        
        #endregion

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            UpdatePropertyValues();
            Properties.Settings.Default.Save();
            Close();
        }

        private void UpdatePropertyValues()
        {
            Properties.Settings.Default.defaultOnOff = (bool) OnOffCB.SelectedValue;
            Properties.Settings.Default.defaultDimmingMode = (int) DimmingModeCB.SelectedValue;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
