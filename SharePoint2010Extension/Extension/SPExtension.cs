using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.SharePoint;

namespace SharePoint2010Extension
{
    public partial class SPExtension : Form
    {
        public SPExtension()
        {
            InitializeComponent();
        }
        string _CurrentUser = @"HOME\Administrator";

        private void button1_Click(object sender, EventArgs e)
        {
            string siteUrl = "http://localhost/";

            using (SPWeb _web = new SPSite(siteUrl).OpenWeb())
            {
                //GetListFieldNameWithDisplayName(_web, true);

                SetUrlValueExtension(_web);
                //SharePointListExist(_web);
                //UserIngroup(_web);
                //UserExistInSite(_web);

            }
        }

        private void UserExistInSite(SPWeb _web)
        {
            if (_web.IsUserExist(_CurrentUser))
            {
                MessageBox.Show("User exist");
            }
            else
            {
                MessageBox.Show("User not exist");
            }
        }

        /// <summary>
        /// it is for moss 2007
        /// </summary>
        /// <param name="_web"></param>
        public void SharePointListExist(SPWeb _web)
        {
            SPList SharepointList = _web.IsListExist("Customer");

            if (SharepointList != null)
            {
                MessageBox.Show("List Exist");
            }
            else
            {
                MessageBox.Show("List Not Exist");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_web"></param>
        public void UserIngroup(SPWeb _web)
        {
            if (lstSecurityGroup.Items.Count > 0)
            {
                SPUser _UserName = _web.SiteUsers[_CurrentUser];

                if (_UserName.InUserGroup(lstSecurityGroup.SelectedItem.ToString()))
                {
                    MessageBox.Show("User exist in group:" + lstSecurityGroup.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("User not exist in group:" + lstSecurityGroup.SelectedItem.ToString());
                }
            }
            else
            {
                MessageBox.Show("Please first bind the site groups in list");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_web"></param>
        public void GetAllGroups(SPWeb _web)
        {

            lstSecurityGroup.Items.Clear();
            List<string> _items = new List<string>();

            foreach (SPGroup item in _web.Groups)
            {
                _items.Add(item.Name);

            }
            lstSecurityGroup.DataSource = _items;
        }
          
        private void btnbindusers_Click(object sender, EventArgs e)
        {
            string siteUrl = "http://localhost/";

            using (SPWeb _web = new SPSite(siteUrl).OpenWeb())
            {
                GetAllGroups(_web);
            }

        }

        public void GetListFieldNameWithDisplayName(SPWeb web, bool isList)
        {
            SPList SharepointList = web.IsListExist("Customer City");
            if (isList)
            {


                if (SharepointList != null)
                {
                    //example column Internal name "test" and display name "text col"
                    MessageBox.Show(SharepointList.GetFieldTitle("test"));
                }
            }
            else
            {
                if (SharepointList != null)
                {
                    SPListItem item = SharepointList.Items.GetItemById(1); 
                    MessageBox.Show(item.GetFieldTitle("test"));
                    
                }

            }
        }


        public void SetUrlValueExtension(SPWeb web)
        {
            SPList SharepointList = web.IsListExist("Links"); 

            if (SharepointList != null)
            {
                //example column Internal name "test" and display name "text col"
                SPListItem _item = SharepointList.AddItem();
                _item.SetFieldValueUrl("URL", "http://google.com", "Google");
                _item.Update();

            } 
           
        }

    }
}
