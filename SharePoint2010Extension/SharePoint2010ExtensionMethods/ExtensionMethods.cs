using System;
using System.IO;
using Microsoft.SharePoint;
using System.Linq;
using System.Collections.Generic;

namespace SharePoint2010Extension
{

    public static class ExtensionMethods
    {
        /// <summary>
        /// In Group
        /// </summary>
        /// <param name="User"></param>
        /// <param name="GroupName"></param>
        /// <returns></returns>
        public static bool InUserGroup(this SPUser User, string GroupName)
        {
            return User.Groups.Cast<SPGroup>().Any(g => g.Name.ToLower() == GroupName.ToLower());
        }

        /// <summary>
        /// Is User Exist in Site
        /// </summary>
        /// <param name="web"></param>
        /// <param name="UserLoginName"></param>
        /// <returns></returns>
        public static bool IsUserExist(this SPWeb web, String UserLoginName)
        {
            return web.SiteUsers.Cast<SPUser>().Any(U => U.LoginName.ToLower() == UserLoginName.ToLower());
        }

        /// <summary>
        /// SharePoint 2007 MOOS Extension Method
        /// </summary>
        /// <param name="web"></param>
        /// <param name="ListName"></param>
        /// <returns></returns>
        public static SPList IsListExist(this SPWeb web, string ListName)
        {

            SPList IsExistSPList = null;
            try
            {
                IsExistSPList = web.Lists[ListName];
            }
            catch
            {
                IsExistSPList = null;
            }
            return IsExistSPList;

        }


        public static string GetFieldTitle(this SPList list, string fieldInternalName)
        {
            return list.Fields.GetFieldByInternalName(fieldInternalName).Title;
        }


        /// <summary>
        /// Returns the login name of an User-Field.
        /// </summary>
        public static string GetFieldValueUserLogin(this SPListItem item, string fieldName) 
        {
            if (item != null)
            {
                SPFieldUserValue userValue =
                  new SPFieldUserValue(
                    item.Web, item[fieldName] as string);
                return userValue.User.LoginName;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the value of a User-Field to a login name.
        /// </summary>
        public static void SetFieldValueUser(this SPListItem item, string fieldName, string loginName) 
        {
            if (item != null)
            {
                item[fieldName] = item.Web.EnsureUser(loginName);
            }
        }

        /// <summary>
        /// Sets the value of a User-Field to an SPPrincipal 
        /// (SPGroup or SPUser).
        /// </summary>
        public static void SetFieldValueUser(this SPListItem item, string fieldName, SPPrincipal principal) 
        {
            if (item != null)
            {
                item[fieldName] = principal;
            }
        }

        public static void SetFieldValueUser(this SPListItem item, string fieldName, IEnumerable<SPPrincipal> principals) 
        {
            if (item != null)
            {
                SPFieldUserValueCollection fieldValues =
                  new SPFieldUserValueCollection();
                
                foreach (SPPrincipal principal in principals)
                {
                    fieldValues.Add(
                      new SPFieldUserValue(
                        item.Web, principal.ID, principal.Name));
                }
                item[fieldName] = fieldValues;
            }
        }

        /// <summary>
        /// Sets the value of a multivalue User-Field to 
        /// a list of user names.
        /// </summary>
        public static void SetFieldValueUser(this SPListItem item, string fieldName, IEnumerable<string> loginNames) 
        {
            if (item != null)
            {
                SPFieldUserValueCollection fieldValues = new SPFieldUserValueCollection(); 

                foreach (string loginName in loginNames)
                {
                    SPUser user = item.Web.EnsureUser(loginName);
                    fieldValues.Add(
                      new SPFieldUserValue(
                        item.Web, user.ID, user.Name));
                }

                item[fieldName] = fieldValues;
            }
        }

        //Lookups--------------------------------

        /// <summary>
        /// Returns the value of a Lookup Field.
        /// </summary>
        public static string GetFieldValueLookup(this SPListItem item, string fieldName) 
        {
            if (item != null)
            {
                SPFieldLookupValue lookupValue =
                    new SPFieldLookupValue(item[fieldName] as string);
                return lookupValue.LookupValue;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the value of a Lookup Field.
        /// </summary>
        public static int GetFieldIDLookup(this SPListItem item, string fieldName)  
        {
            if (item != null)
            {
                SPFieldLookupValue lookupValue =
                    new SPFieldLookupValue(item[fieldName] as string);
                return lookupValue.LookupId;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the value of a Lookup-Field with multiple values.
        /// </summary>
        public static IEnumerable<string> GetFieldValueLookupCollection(this SPListItem item, string fieldName) 
        {
            List<string> result = new List<string>();
            if (item != null)
            {
                SPFieldLookupValueCollection values = item[fieldName] as SPFieldLookupValueCollection;  
                foreach (SPFieldLookupValue value in values)
                {
                    result.Add(value.LookupValue);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns the SPFieldLookupValue instance of a lookup value. 
        /// The ID value will be obtained using SPQuery.
        /// </summary>
        private static SPFieldLookupValue GetLookupValue(SPWeb web, SPFieldLookup field, string lookupValue) 
        {
            string queryFormat =
                @"<Where>
            <Eq>
                <FieldRef Name='{0}' />
                <Value Type='Text'>{1}</Value>
            </Eq>
          </Where>";

            string queryText = string.Format(queryFormat, field.LookupField, lookupValue);
                
            SPList lookupList = web.Lists[new Guid(field.LookupList)];

            SPListItemCollection lookupItems = lookupList.GetItems(new SPQuery() { Query = queryText }); 

            if (lookupItems.Count > 0)
            {
                int lookupId =
                    Convert.ToInt32(lookupItems[0][SPBuiltInFieldId.ID]);

                return new SPFieldLookupValue(lookupId, lookupValue);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Returns the SPFieldLookupValue instance of a lookup value. 
        /// The ID value will be obtained using SPQuery.
        /// </summary>
        private static SPFieldLookupValue GetLookupValue(SPWeb web, SPFieldLookup field, int lookupid) 
        {
            string queryFormat =
                @"<Where>
            <Eq>
                <FieldRef Name='ID' />
                <Value Type='Number'>{0}</Value>
            </Eq>
          </Where>";

            string queryText = string.Format(queryFormat, lookupid);
            SPList lookupList = web.Lists[new Guid(field.LookupList)];

            SPListItemCollection lookupItems =
                lookupList.GetItems(new SPQuery() { Query = queryText });

            if (lookupItems.Count > 0)
            {
                int lookupId =
                    Convert.ToInt32(lookupItems[0][SPBuiltInFieldId.ID]);

                string lookupValue =
                   Convert.ToString(lookupItems[0][SPBuiltInFieldId.Title]);

                return new SPFieldLookupValue(lookupId, lookupValue);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the value of a Lookup-Field.
        /// </summary>
        public static void SetFieldValueLookup(this SPListItem item, string fieldName, string lookupValue) 
        {
            if (item != null)
            {
                SPFieldLookup field = item.Fields.GetField(fieldName) as SPFieldLookup; 
                item[fieldName] = GetLookupValue(item.Web, field, lookupValue);
            }
            else
            {
                item[fieldName] = null;
            }
        }

        /// <summary>
        /// Sets the value of a Lookup-Field.
        /// </summary>
        public static void SetFieldIDValueLookup(this SPListItem item, string fieldName, int lookupID) 
        {
            if (item != null)
            {
                SPFieldLookup field = item.Fields.GetField(fieldName) as SPFieldLookup; 
                item[fieldName] = GetLookupValue(item.Web, field, lookupID);
            }
            else
            {
                item[fieldName] = null;
            }
        }


        /// <summary>
        /// Set the values of a Lookup-Field with multiple values allowed.
        /// </summary>
        public static void SetFieldValueLookup(this SPListItem item, string fieldName, IEnumerable<string> lookupValues) 
        {
            if (item != null)
            {
                SPFieldLookup field = item.Fields.GetField(fieldName) as SPFieldLookup;

                SPFieldLookupValueCollection fieldValues = new SPFieldLookupValueCollection(); 

                foreach (string lookupValue in lookupValues)
                {
                    fieldValues.Add(
                        GetLookupValue(item.Web, field, lookupValue));
                }
                item[fieldName] = fieldValues;
            }
        }

        //Field URL----------------------------------------------

        /// <summary>
        /// Returns the value of an Url-Field.
        /// </summary>
        public static string GetFieldValueUrl(this SPListItem item, string fieldName) 
        {
            if (item != null)
            {
                SPFieldUrlValue urlValue = new SPFieldUrlValue(item[fieldName] as string); 
                return urlValue.Url;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the value of an URL-Field.
        /// </summary>
        public static void SetFieldValueUrl(this SPListItem item, string fieldName, string url, string description) 
        {
            if (item != null)
            {
                item[fieldName] = new SPFieldUrlValue()
                                {
                                    Description = description,
                                    Url = url
                                }; 
            }
        } 

        public static string GetFieldTitle(this SPListItem item, string fieldInternalName)
        {
            return item.Fields.GetFieldByInternalName(fieldInternalName).Title;
        }

        public static string GetFieldValueUser(SPListItem item, string fieldName)
        {
            if (item != null)
            {
                string siteUrl = item.Web.Site.Url;
                string userName = string.Empty;

                if (item[fieldName] != null)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(siteUrl))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPFieldUserValue userValue = new SPFieldUserValue(web, item[fieldName] as string);
                                userName = userValue.User.LoginName;
                            }
                        }
                    });
                }
                return userName;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the field value user collection.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] GetFieldValueUserCollection(SPListItem item, string fieldName)
        {
            List<string> result = new List<string>();
            if (item != null)
            {
                string siteUrl = item.Web.Site.Url;
                if (item[fieldName] != null)
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(siteUrl))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                SPFieldUserValueCollection values = item[fieldName] as SPFieldUserValueCollection;

                                foreach (SPFieldUserValue value in values)
                                {
                                    result.Add(value.User.LoginName);
                                }
                            }
                        }
                    });
                }
            }
            return result.ToArray();
        }
    }
}
