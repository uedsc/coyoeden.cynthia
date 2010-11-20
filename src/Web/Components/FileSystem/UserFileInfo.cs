﻿/*
 * QtPet Online File Manager v1.0
 * Copyright (c) 2009, Zhifeng Lin (fszlin[at]gmail.com)
 * 
 * Licensed under the MS-PL license.
 * http://qtfile.codeplex.com/license
 */
// Last Modified 2009-12-26 Joe Audette

using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using Cynthia.Web.Framework;

namespace Cynthia.FileSystem
{
    /// <summary>
    /// Represents the information of a user file.
    /// </summary>
    public class UserFileInfo
    {
        /// <summary>
        /// Gets or sets the name of file.
        /// </summary>
        /// <value>
        /// The name of file.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of file.
        /// </summary>
        /// <value>
        /// The size of file.
        /// </value>
        public long Size
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public DateTime Modified
        {
            get;
            set;
        }

        public virtual object ToJson()
        {
            return new { name = Name, size = Size, contentType = ContentType, modified = (Modified - new DateTime(1970, 1, 1)).TotalMilliseconds };
        }

        public static UserFileInfo FromPostedFile(HttpPostedFile file, string newName)
        {
           
            UIHelper.ValidateNotNull(file, "file");

            if (string.IsNullOrEmpty(newName)) { newName = Path.GetFileName(file.FileName); }

            return new UserFileInfo()
            {
                Name = newName,
                Size = file.ContentLength,
                ContentType = file.ContentType,
                Modified = DateTime.Now
            };
        }
    }
}
