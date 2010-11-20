using System;
using System.Collections.Generic;
using System.Text;

namespace Cynthia.Business
{
    public class GroupTopicMovedArgs
    {
        private int groupID;
        private int originalGroupID;

        public int GroupId
        {
            get { return groupID; }
            set { groupID = value; }
        }

        public int OriginalGroupId
        {
            get { return originalGroupID; }
            set { originalGroupID = value; }
        }

    }
}
