﻿// Author:					Joe Audette
// Created:				    2008-10-06
// Last Modified:			2008-10-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


namespace Cynthia.Business
{
    /// <summary>
    ///
    /// </summary>
    public class ContentRatingStats
    {
        private int averageRating = 0;
        private int totalVotes = 0;

        public int AverageRating
        {
            get { return averageRating; }
            set { averageRating = value; }
        }

        public int TotalVotes
        {
            get { return totalVotes; }
            set { totalVotes = value; }
        }

    }
}
