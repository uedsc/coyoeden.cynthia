// Author:					Joe Audette
// Created:				    2007-09-22
// Last Modified:			2009-02-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using Cynthia.Data;

namespace Cynthia.Business
{
    /// <summary>
    ///
    /// </summary>
    public class BannedIPAddress
    {

        #region Constructors

        public BannedIPAddress()
        { }


        public BannedIPAddress(int rowId)
        {
            GetBannedIPAddress(rowId);
        }

        #endregion

        #region Private Properties

        private int rowID;
        private string bannedIP;
        private DateTime bannedUTC;
        private string bannedReason;

        #endregion

        #region Public Properties

        public int RowId
        {
            get { return rowID; }
            set { rowID = value; }
        }
        public string BannedIP
        {
            get { return bannedIP; }
            set { bannedIP = value; }
        }
        public DateTime BannedUtc
        {
            get { return bannedUTC; }
            set { bannedUTC = value; }
        }
        public string BannedReason
        {
            get { return bannedReason; }
            set { bannedReason = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of BannedIPAddress.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        private void GetBannedIPAddress(int rowId)
        {
            using (IDataReader reader = DBBannedIP.GetOne(rowId))
            {
                if (reader.Read())
                {
                    this.rowID = Convert.ToInt32(reader["RowID"]);
                    this.bannedIP = reader["BannedIP"].ToString();
                    this.bannedUTC = Convert.ToDateTime(reader["BannedUTC"]);
                    this.bannedReason = reader["BannedReason"].ToString();

                }

            }

        }

        /// <summary>
        /// Persists a new instance of BannedIPAddress. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBBannedIP.Add(
                this.bannedIP,
                this.bannedUTC,
                this.bannedReason);

            this.rowID = newID;

            return (newID > 0);

        }


        /// <summary>
        /// Updates this instance of BannedIPAddress. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBBannedIP.Update(
                this.rowID,
                this.bannedIP,
                this.bannedUTC,
                this.bannedReason);

        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of BannedIPAddress. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowID > 0)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of BannedIPAddress. Returns true on success.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(int rowId)
        {
            return DBBannedIP.Delete(rowId);
        }

        /// <summary>
        /// Returns true if the passed in address is banned
        /// </summary>
        /// <param name="ipAddress"> ipAddress </param>
        /// <returns>bool</returns>
        public static bool IsBanned(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress)) { return false; }
            return DBBannedIP.IsBanned(ipAddress);
        }

        /// <summary>
        /// Gets an IList with all instances of BannedIPAddress.
        /// </summary>
        public static List<BannedIPAddress> GetAll()
        {
            List<BannedIPAddress> bannedIPAddressList
                = new List<BannedIPAddress>();

            using (IDataReader reader = DBBannedIP.GetAll())
            {
                while (reader.Read())
                {
                    BannedIPAddress bannedIPAddress = new BannedIPAddress();
                    bannedIPAddress.rowID = Convert.ToInt32(reader["RowID"]);
                    bannedIPAddress.bannedIP = reader["BannedIP"].ToString();
                    bannedIPAddress.bannedUTC = Convert.ToDateTime(reader["BannedUTC"]);
                    bannedIPAddress.bannedReason = reader["BannedReason"].ToString();
                    bannedIPAddressList.Add(bannedIPAddress);
                }
            }

            return bannedIPAddressList;

        }

        public static List<String> GetAllBannedIPs()
        {
            List<String> bannedIPAddressList
                = new List<String>();

            using (IDataReader reader = DBBannedIP.GetAll())
            {
                while (reader.Read())
                {

                    bannedIPAddressList.Add(reader["BannedIP"].ToString());

                }
            }

            return bannedIPAddressList;

        }

        /// <summary>
        /// Gets an IList with page of instances of BannedIPAddresse.
        /// </summary>
        public static List<BannedIPAddress> GetPage(
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;

            List<BannedIPAddress> bannedIPAddressList 
                = new List<BannedIPAddress>();

            using (IDataReader reader
                = DBBannedIP.GetPage(
                pageNumber,
                pageSize,
                out totalPages))
            {
                while (reader.Read())
                {
                    BannedIPAddress bannedIPAddress = new BannedIPAddress();
                    bannedIPAddress.rowID = Convert.ToInt32(reader["RowID"]);
                    bannedIPAddress.bannedIP = reader["BannedIP"].ToString();
                    bannedIPAddress.bannedUTC = Convert.ToDateTime(reader["BannedUTC"]);
                    bannedIPAddress.bannedReason = reader["BannedReason"].ToString();
                    bannedIPAddressList.Add(bannedIPAddress);

                }
            }

            return bannedIPAddressList;

        }

        /// <summary>
        /// Gets an IDataReader with rows from the cy_BannedIPAddresses table.
        /// </summary>
        /// <param name="ipAddress"> ipAddress </param>
        public static IDataReader GeByIpAddress(string ipAddress)
        {
            return DBBannedIP.GeByIpAddress(ipAddress);

        }


        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of BannedIPAddresse.
        /// </summary>
        public static int CompareByRowId(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
        {
            return bannedIPAddress1.RowId.CompareTo(bannedIPAddress2.RowId);
        }
        /// <summary>
        /// Compares 2 instances of BannedIPAddresse.
        /// </summary>
        public static int CompareByBannedIP(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
        {
            return bannedIPAddress1.BannedIP.CompareTo(bannedIPAddress2.BannedIP);
        }
        /// <summary>
        /// Compares 2 instances of BannedIPAddresse.
        /// </summary>
        public static int CompareByBannedUtc(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
        {
            return bannedIPAddress1.BannedUtc.CompareTo(bannedIPAddress2.BannedUtc);
        }
        /// <summary>
        /// Compares 2 instances of BannedIPAddresse.
        /// </summary>
        public static int CompareByBannedReason(BannedIPAddress bannedIPAddress1, BannedIPAddress bannedIPAddress2)
        {
            return bannedIPAddress1.BannedReason.CompareTo(bannedIPAddress2.BannedReason);
        }

        #endregion


    }

}

