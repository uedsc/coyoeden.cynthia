﻿//  Author:                     Joe Audette
//  Created:                    2009-08-16
//	Last Modified:              2010-02-12
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Cynthia.Web.Framework
{
    public static class IOHelper
    {

        public static string ToCleanFileName(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            return s.ToLower().RemoveInvalidPathChars().RemoveLineBreaks().Replace("'", string.Empty).Replace("\"", string.Empty).Replace(" ", string.Empty);

        }

        public static string ToCleanFileName(this string s, bool forceLowerCase)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            if (forceLowerCase)
            {
                return s.ToLower().RemoveInvalidPathChars().RemoveLineBreaks().Replace("'", string.Empty).Replace(" ", string.Empty);
            }

            return s.RemoveInvalidPathChars().RemoveLineBreaks().Replace("'", string.Empty).Replace(" ", string.Empty);

        }

        public static string ToCleanFolderName(this string s, bool forceLowerCase)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            if (forceLowerCase)
            {
                return s.ToLower().RemoveInvalidPathChars().RemoveLineBreaks().Replace("'", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty).Replace(",", string.Empty);
            }

            return s.RemoveInvalidPathChars().RemoveLineBreaks().Replace("'", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty).Replace(",", string.Empty);

        }

        public static string RemoveInvalidPathChars(this string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            char[] invalidPathChars = Path.GetInvalidPathChars();
            char[] result = new char[s.Length];
            int resultIndex = 0;
            foreach (char c in s)
            {
                if (!invalidPathChars.Contains(c))
                    result[resultIndex++] = c;
            }
            if (0 == resultIndex)
                s = string.Empty;
            else if (result.Length != resultIndex)
                s = new string(result, 0, resultIndex);

            return s;


        }

        /// <summary>
        /// Determines if file is video or image file based upon its extension.
        /// </summary>
        /// <param name="fileInfo">File to inspect</param>
        /// <returns>Results of the inspection.</returns>
        public static bool IsMediaFile(this FileInfo fileInfo)
        {
            if (string.Equals(fileInfo.Extension, ".bmp", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".gif", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".jpe", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".jpeg", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".jpg", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".png", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".tif", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".asf", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".asx", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".avi", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".mov", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".mp4", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".mpeg", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".mpg", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".wmv", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            
            return false;
        }

        /// <summary>
        /// Determines if file is video or image file based upon its extension.
        /// </summary>
        /// <param name="fileInfo">File to inspect</param>
        /// <returns>Results of the inspection.</returns>
        public static bool IsWebImageFile(this FileInfo fileInfo)
        {
            if (string.Equals(fileInfo.Extension, ".gif", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".jpeg", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".jpg", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            if (string.Equals(fileInfo.Extension, ".png", StringComparison.InvariantCultureIgnoreCase)) { return true; }
            
            return false;
        }

        public static bool IsDecendentDirectory(DirectoryInfo directoryToSearch, DirectoryInfo directoryToFind)
        {
            if (directoryToFind.FullName.StartsWith(directoryToSearch.FullName)) { return true; }

            return false;

        }

        public static bool IsDecendentDirectory(DirectoryInfo directoryToSearch, FileInfo fileToFind)
        {
            if (fileToFind.FullName.StartsWith(directoryToSearch.FullName)) { return true; }

            return false;

        }

        public static bool IsDecendentDirectory(string virtualPathToSearch, string virtualPathToFind)
        {
            if (string.IsNullOrEmpty(virtualPathToSearch)) { return false; }
            if (string.IsNullOrEmpty(virtualPathToFind)) { return false; }

            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(HostingEnvironment.MapPath(virtualPathToSearch));
            DirectoryInfo requestedDirectoryInfo = new DirectoryInfo(HostingEnvironment.MapPath(virtualPathToFind));

            return IsDecendentDirectory(rootDirectoryInfo, requestedDirectoryInfo);
        }

        public static bool IsDecendentFile(string virtualPathToSearch, string virtualPathToFind)
        {
            if (string.IsNullOrEmpty(virtualPathToSearch)) { return false; }
            if (string.IsNullOrEmpty(virtualPathToFind)) { return false; }

            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(HostingEnvironment.MapPath(virtualPathToSearch));
            FileInfo requestedDirectoryInfo = new FileInfo(HostingEnvironment.MapPath(virtualPathToFind));

            return IsDecendentDirectory(rootDirectoryInfo, requestedDirectoryInfo);
        }


        public static string GetMimeType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return string.Empty; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);

            switch (fileType)
            {
                case "doc":
                case "docx":
                    return "application/msword";

                case "xls":
                case "xlsx":
                    return "application/vnd.ms-excel";

                case "exe":
                    return "application/octet-stream";

                case "ppt":
                case "pptx":
                    return "application/vnd.ms-powerpoint";

                case "jpg":
                case "jpeg":
                    return "image/jpeg";

                case "gif":
                    return "image/gif";

                case "png":
                    return "image/png";

                case "bmp":
                    return "image/bmp";

                case "wmv":
                    return "video/x-ms-wmv";

                case "mpg":
                case "mpeg":
                    return "video/mpeg";

                case "mov":
                    return "video/quicktime";

                case "flv":
                    return "video/x-flv";

                case "ico":
                    return "image/x-icon";

                case "htm":
                case "html":
                    return "text/html";

                case "css":
                    return "text/css";

                case "eps":
                    return "application/postscript";

            }

            return "application/" + fileType;


        }

        public static bool IsNonAttacmentFileType(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension)) { return false; }

            string fileType = fileExtension.ToLower().Replace(".", string.Empty);
            if (fileType == "pdf") { return true; }
            //if (fileType == "wmv") { return true; } //necessary?


            return false;
        }

    }
}
